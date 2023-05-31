using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using IAAITW.Models;
using MvcPaging;

namespace IAAITW.Areas.CMS.Controllers
{
    public class AccountsController : Controller
    {
        private IaaiTwDb db = new IaaiTwDb();
        private const int DefaultPageSize = 5;

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

        // GET: Accounts/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Accounts/Login
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

        // POST: Accounts/Logout
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

            return RedirectToAction("Login", "Accounts");
        }

        // GET: CMS/Accounts
        [Authorize(Roles = "最高管理者")]
        public ActionResult Index(int? page)
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "帳號管理", Url = Url.Action("Index", "Accounts") });
            ViewBag.Breadcrumb = breadcrumb;

            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            var cMSAccounts = db.CMSAccounts.Include(c => c.MyIdentity);
            return View(cMSAccounts.OrderBy(p => p.Id).ToPagedList(currentPageIndex, DefaultPageSize));
        }

        // GET: CMS/Accounts/Details/5
        public ActionResult Details(int? id)
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "帳號管理", Url = Url.Action("Index", "Accounts") });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "基本資料", Url = null });
            ViewBag.Breadcrumb = breadcrumb;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CMSAccount cMSAccount = db.CMSAccounts.Find(id);
            if (cMSAccount == null)
            {
                return HttpNotFound();
            }
            return View(cMSAccount);
        }

        // GET: CMS/Accounts/Create
        //[Authorize(Roles = "最高管理者")]
        public ActionResult Create()
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "帳號管理", Url = Url.Action("Index", "Accounts") });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "新增帳號", Url = null });
            ViewBag.Breadcrumb = breadcrumb;

            ViewBag.IdentityId = new SelectList(db.CMSIdentities, "Id", "Identity");
            return View();
        }

        // POST: CMS/Accounts/Create
        [HttpPost]
        //[Authorize(Roles = "最高管理者")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CMSAccount account, HttpPostedFileBase file)
        {
            // Password 在模型中沒有 [Reqired] 標籤，所以要自行使用判斷式確認是否 null。
            // 把 [Reqired] 標籤取消的用意是，在進入編輯頁面時，不填入密碼則沿用原有密碼。
            if (ModelState.IsValid && account.Password != null)
            {
                // 確認帳號是否已存在
                CMSAccount findAccount = db.CMSAccounts
                    .Where(c => c.Email == account.Email.ToLower().Trim())
                    .FirstOrDefault();
                if (findAccount != null)
                {
                    ViewBag.Msg = "此帳號已存在";
                    ViewBag.IdentityId = new SelectList(db.CMSIdentities, "Id", "Identity");
                    return View();
                }

                // 密碼加密
                account.Password = BitConverter
                    .ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(account.Password))).Replace("-", null);

                // 個人頭像
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string extension = Path.GetExtension(fileName);

                    if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif")
                    {
                        string newFileName = account.Email + extension;
                        var path = Path.Combine(Server.MapPath("~/upload/headshot"), newFileName);
                        file.SaveAs(path);

                        account.Headshot = newFileName;
                    }
                    else
                    {
                        ViewBag.Msg = "檔案格式不符合";
                        return View(account);
                    }
                }
                else
                {
                    account.Headshot = "default.jpg";
                }

                db.CMSAccounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            if (account.Password == null)
            {
                ViewBag.MsgForPW = "密碼必填";
            }
            ViewBag.IdentityId = new SelectList(db.CMSIdentities, "Id", "Identity", account.IdentityId);
            return View(account);
        }

        // GET: CMS/Accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "帳號管理", Url = Url.Action("Index", "Accounts") });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "基本資料編輯", Url = null });
            ViewBag.Breadcrumb = breadcrumb;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CMSAccount cMSAccount = db.CMSAccounts.Find(id);
            if (cMSAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdentityId = new SelectList(db.CMSIdentities, "Id", "Identity", cMSAccount.IdentityId);
            return View(cMSAccount);
        }

        // POST: CMS/Accounts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CMSAccount account, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (account.Password == null)
                {
                    account.Password = db.CMSAccounts.Where(x => x.Id == account.Id).Select(x => x.Password).FirstOrDefault();
                }
                else
                {
                    // 密碼加密
                    account.Password = BitConverter
                        .ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(account.Password))).Replace("-", null);
                }

                // 個人頭像
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string extension = Path.GetExtension(fileName);

                    if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif")
                    {
                        string newFileName = account.Email + extension;
                        var path = Path.Combine(Server.MapPath("~/upload/headshot"), newFileName);
                        file.SaveAs(path);

                        account.Headshot = newFileName;
                    }
                    else
                    {
                        ViewBag.Msg = "檔案格式不符合";
                        return View(account);
                    }
                }
                else
                {
                    account.Headshot = db.CMSAccounts.Where(x => x.Id == account.Id).Select(x => x.Headshot).FirstOrDefault();
                }

                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdentityId = new SelectList(db.CMSIdentities, "Id", "Identity", account.IdentityId);
            return View(account);
        }

        // POST: CMS/Accounts/Delete/5
        [HttpPost]
        [Authorize(Roles = "最高管理者")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            CMSAccount cMSAccount = db.CMSAccounts.Find(id);
            db.CMSAccounts.Remove(cMSAccount);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
