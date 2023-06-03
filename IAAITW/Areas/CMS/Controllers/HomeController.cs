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
                string userData = findAccount.Id + ";" + findAccount.Name + ";" + findAccount.Email + ";" + findAccount.MyIdentity.Identity + ";" + findAccount.Headshot;

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
    }
}