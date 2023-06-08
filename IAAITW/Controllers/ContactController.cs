using IAAITW.Models;
using Recaptcha.Web;
using Recaptcha.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace IAAITW.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "聯絡我們", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "聯絡我們", Url = "#" });
            ViewBag.Breadcrumb = breadcrumb;

            return View();
        }

        // POST: Contact/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ViewModel.Contact contact)
        {
            if (ModelState.IsValid)
            {
                var recaptchaHelper = this.GetRecaptchaVerificationHelper();
                if (string.IsNullOrEmpty(recaptchaHelper.Response))
                {
                    ModelState.AddModelError("", "請勾選我不是機器人");
                    return View(contact);
                }
                var recaptchaResult = recaptchaHelper.VerifyRecaptchaResponse();
                if (!recaptchaResult.Success)
                {
                    ModelState.AddModelError("", "驗證不通過");
                    return View(contact);
                }

                // 建立郵件訊息
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(contact.Email);  // 發送者的郵件地址
                mail.To.Add(ConfigurationManager.AppSettings["SendTo"]);  // 接收者的郵件地址
                mail.Subject = contact.Subject;  // 郵件主題
                mail.Body = $"姓名: {contact.Name}\n\n性別: {contact.Gender}\n\n聯絡電話: {contact.Tel}\n\nEmail: {contact.Email}\n\n詢問內容: {contact.Article}";  // 郵件內容

                // 使用 SMTP 服務寄送郵件
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);  // SMTP 伺服器的地址和端口
                smtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SendTo"], ConfigurationManager.AppSettings["GmailPassword"]);  // SMTP 伺服器的用戶名和密碼
                smtpClient.EnableSsl = true;  // 啟用 SSL 加密
                smtpClient.Send(mail);  // 寄送郵件

                return RedirectToAction("Success");
            }

            return View(contact);
        }

        // GET: Contact/Success
        public ActionResult Success()
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "聯絡我們", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "聯絡我們", Url = "#" });
            ViewBag.Breadcrumb = breadcrumb;

            return View();
        }
    }
}