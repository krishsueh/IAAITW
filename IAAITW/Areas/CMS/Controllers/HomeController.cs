using IAAITW.Areas.CMS.Security;
using IAAITW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace IAAITW.Areas.CMS.Controllers
{
    public class HomeController : Controller
    {
        private IaaiTwDb db = new IaaiTwDb();

        // GET: CMS/Home
        [PermissionFilters]
        public ActionResult Index()
        {
            return View();
        }

        //設定驗證票
        private void SetAuthenTicket(string userData, string userId)
        {
            //宣告一個驗證票
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userId, DateTime.Now, DateTime.Now.AddHours(3), false, userData);
            //加密驗證票
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            //建立 Cookie
            HttpCookie authenticationCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            //將 Cookie 寫入回應
            Response.Cookies.Add(authenticationCookie);
        }

        // GET: CMS/Home/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        // POST: CMS/Home/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(ViewModel.Login view)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Msg = "欄位輸入格式錯誤";
                return View();
            }

            view.Password = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(view.Password))).Replace("-", null);

            CMSAccount findAccount = db.CMSAccounts
                    .Where(c => c.Email == view.Email.ToLower().Trim() && c.Password == view.Password)
                    .FirstOrDefault();

            if (findAccount != null)
            {
                //宣告驗證票要夾帶的資料 (用;區隔)
                string userData = findAccount.Id + ";" + findAccount.Name + ";" + findAccount.Email + ";" + findAccount.MyIdentity.Identity + ";" + findAccount.Headshot + ";" + findAccount.Permission;

                //設定驗證票(夾帶資料，cookie 命名)
                SetAuthenTicket(userData, findAccount.Name);

                return RedirectToAction("Index", "Home");
            }
            ViewBag.Msg = "帳號或密碼有誤";
            return View();
        }

        // POST: CMS/Home/Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            //清除所有的 session
            Session.Abandon();
            Session.RemoveAll();

            //建立一個同名的 Cookie 來覆蓋原本的 Cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            //建立 ASP.NET 的 Session Cookie 同樣是為了覆蓋
            HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);

            return RedirectToAction("Login", "Home");
        }

        public ActionResult SideMenu()
        {
            List<Permission> permissions = db.Permissions.ToList();
            var roots = permissions.Where(p => p.ParentId == null);

            StringBuilder sbTree = new StringBuilder();
            foreach (var root in roots)
            {
                sbTree.Append(GetNode(root));
            }
            ViewBag.Tree = sbTree.ToString();

            return PartialView();
        }

        private string GetNode(Permission root)
        {
            StringBuilder sbNode = new StringBuilder();

            if (root.Permissions.Count() > 0)
            {
                sbNode.Append($"<li class='nav-item pcoded-hasmenu'>");
                sbNode.Append($"<a href='javascript:' class='nav-link '><span class='pcoded-micon'><i class='feather icon-box'></i></span><span class='pcoded-mtext'>{root.Subject}</span></a>");
                sbNode.Append("<ul class='pcoded-submenu'>");
                foreach (var items in root.Permissions)
                {
                    sbNode.Append(GetSubNode(items));
                }
                sbNode.Append("</ul>");
            }
            else
            {
                sbNode.Append($"<li class='nav-item'>");
                sbNode.Append($"<a href='{root.Url}'><span class='pcoded-micon'><i class='feather icon-file-text'></i></span><span class='pcoded-mtext'>{root.Subject}</span></a>");
            }
            sbNode.Append("</li>");
            return sbNode.ToString();
        }

        private string GetSubNode(Permission items)
        {
            StringBuilder sbSubNode = new StringBuilder();
            sbSubNode.Append($"<li><a href='{items.Url}'>{items.Subject}</a></li>");
            return sbSubNode.ToString();
        }
    }
}