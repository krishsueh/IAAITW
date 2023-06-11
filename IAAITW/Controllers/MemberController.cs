using IAAITW.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace IAAITW.Controllers
{
    public class MemberController : Controller
    {
        private IaaiTwDb db = new IaaiTwDb();

        // GET: Member/Login
        public ActionResult Login()
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "會員專區", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "會員登入", Url = "#" });
            ViewBag.Breadcrumb = breadcrumb;

            return View();
        }

        // POST: Member/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string account, string password)
        {
            if (String.IsNullOrEmpty(account) || String.IsNullOrEmpty(password))
            {
                ViewBag.Msg = "帳號和密碼皆為必填";
                return View();
            }

            password = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(password))).Replace("-", null);

            Member findMember = db.Members
                    .Where(c => c.Account == account.ToLower().Trim() && c.Password == password)
                    .FirstOrDefault();

            if (findMember != null)
            {
                Session["Id"] = findMember.Id;
                Session["Account"] = findMember.Account;
                Session["Name"] = findMember.Name;
                Session["Login"] = "OK";

                return RedirectToAction("Index", "Forum");
            }
            else
            {
                ViewBag.Msg = "帳號或密碼有誤";
                return View();
            }

        }

        // POST: Member/Logout
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

            return RedirectToAction("Login", "Member");
        }

        // GET: Member/Register
        public ActionResult Register()
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "會員專區", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "會員註冊", Url = "#" });
            ViewBag.Breadcrumb = breadcrumb;

            return View();
        }

        // POST: Member/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Member member, string confirmPassword, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                // 確認帳號是否已存在
                Member findMember = db.Members
                    .Where(c => c.Account == member.Account.ToLower().Trim())
                    .FirstOrDefault();
                if (findMember != null)
                {
                    ViewBag.MsgAccount = "此帳號已存在";
                    return View(member);
                }

                // 確認兩次密碼是否符合
                if (member.Password != confirmPassword)
                {
                    ViewBag.MsgPW = "密碼與確認密碼不相符";
                    return View(member);
                }

                // 密碼加密
                member.Password = BitConverter
                    .ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(member.Password))).Replace("-", null);

                // 國際會籍
                if (member.InternationalMembership == true)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(file.FileName);
                        string extension = Path.GetExtension(fileName);

                        if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif" || extension == ".pdf")
                        {
                            string newFileName = member.Account + "_" + member.Name + extension;
                            var path = Path.Combine(Server.MapPath("~/upload/membership"), newFileName);
                            file.SaveAs(path);

                            member.MembershipFile = newFileName;
                        }
                        else
                        {
                            ViewBag.Msg = "檔案格式不符合";
                            return View(member);
                        }
                    }
                    else
                    {
                        ViewBag.Msg = "請上傳會員證影本，如非國際會籍，請取消勾選";
                        return View(member);
                    }
                }
                else
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        ViewBag.Msg = "如為國際會籍，請勾選確認框後上傳會員證影本";
                        return View(member);
                    }
                }

                // 服務經歷年月判斷
                if (!ValidateDate(member.StartYear1, member.StartMonth1, member.EndYear1, member.EndMonth1))
                {
                    ViewBag.MsgDate1 = "起訖日期有誤";
                    return View(member);
                }

                if (!ValidateDate(member.StartYear2, member.StartMonth2, member.EndYear2, member.EndMonth2))
                {
                    ViewBag.MsgDate2 = "起訖日期有誤";
                    return View(member);
                }
             
                if (!ValidateDate(member.StartYear3, member.StartMonth3, member.EndYear3, member.EndMonth3))
                {
                    ViewBag.MsgDate3 = "起訖日期有誤";
                    return View(member);
                }

                db.Members.Add(member);
                db.SaveChanges();

                //return RedirectToAction("Login");
                //成功註冊後不直接導向 Login 頁面，而是先跳彈窗再導向
                ViewBag.RegisterOk = "註冊成功，請重新登入";
                ViewBag.RedirectUrl = Url.Action("Login");
                return View();
            }

            return View(member);
        }

        // GET: Member/Edit/5
        public ActionResult Edit(int? id)
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "會員專區", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "修改個人資料", Url = "#" });
            ViewBag.Breadcrumb = breadcrumb;

            if (id == null)
            {
                return RedirectToAction("Login");
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Member/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Member member, string newPassword, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(newPassword))
                {
                    // 密碼加密
                    member.Password = BitConverter
                        .ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(newPassword))).Replace("-", null);
                }

                // 國際會籍
                if (member.InternationalMembership == true)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(file.FileName);
                        string extension = Path.GetExtension(fileName);

                        if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif" || extension == ".pdf")
                        {
                            string newFileName = member.Account + "_" + member.Name + extension;
                            var path = Path.Combine(Server.MapPath("~/upload/membership"), newFileName);
                            file.SaveAs(path);

                            member.MembershipFile = newFileName;
                        }
                        else
                        {
                            ViewBag.Msg = "檔案格式不符合";
                            return View(member);
                        }
                    }
                }
                else
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        ViewBag.Msg = "如為國際會籍，請勾選確認框後上傳會員證影本";
                        return View(member);
                    }
                }

                // 服務經歷年月判斷
                if (!ValidateDate(member.StartYear1, member.StartMonth1, member.EndYear1, member.EndMonth1))
                {
                    ViewBag.MsgDate1 = "起訖日期有誤";
                    return View(member);
                }

                if (!ValidateDate(member.StartYear2, member.StartMonth2, member.EndYear2, member.EndMonth2))
                {
                    ViewBag.MsgDate2 = "起訖日期有誤";
                    return View(member);
                }


                if (!ValidateDate(member.StartYear3, member.StartMonth3, member.EndYear3, member.EndMonth3))
                {
                    ViewBag.MsgDate3 = "起訖日期有誤";
                    return View(member);
                }

                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();

                //return RedirectToAction("Index");
                //成功註冊後不直接導向，而是先跳彈窗再導向
                ViewBag.EditOk = "修改個人資料成功";
                ViewBag.RedirectUrl = Url.Action("Edit");
                return View();
            }
            return View(member);
        }

        bool ValidateDate(int? startYear, int? startMonth, int? endYear, int? endMonth)
        {
            if (endYear < startYear || (endYear == startYear && endMonth < startMonth) || startYear > DateTime.Today.Year || endYear > DateTime.Today.Year)
            {
                return false;
            }
            return true;
        }
    }
}