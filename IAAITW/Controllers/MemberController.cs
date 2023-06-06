using IAAITW.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace IAAITW.Controllers
{
    public class MemberController : Controller
    {
        private IaaiTwDb db = new IaaiTwDb();

        // GET: Members/Login
        public ActionResult Login()
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "會員專區", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "會員登入", Url = Url.Action("Login", "Member") });
            ViewBag.Breadcrumb = breadcrumb;

            return View();
        }

        // GET: Member/Register
        public ActionResult Register()
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "會員專區", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "會員註冊", Url = Url.Action("Register", "Member") });
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

                if (member.PastEmployer2 == null)
                {
                    member.PastJobTitle2 = null;
                    member.StartYear2 = null;
                    member.EndYear2 = null;
                    member.StartMonth2 = null;
                    member.EndMonth2 = null;
                }
                else if (!ValidateDate(member.StartYear2, member.StartMonth2, member.EndYear2, member.EndMonth2))
                {
                    ViewBag.MsgDate2 = "起訖日期有誤";
                    return View(member);
                }
                else
                {
                    ViewBag.MsgDate2 = "請填寫完整起訖日期";
                    return View(member);
                }

                if (member.PastEmployer3 == null)
                {
                    member.PastJobTitle3 = null;
                    member.StartYear3 = null;
                    member.EndYear3 = null;
                    member.StartMonth3 = null;
                    member.EndMonth3 = null;
                }
                else if (!ValidateDate(member.StartYear3, member.StartMonth3, member.EndYear3, member.EndMonth3))
                {
                    ViewBag.MsgDate3 = "起訖日期有誤";
                    return View(member);
                }
                else
                {
                    ViewBag.MsgDate3 = "請填寫完整起訖日期";
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

        bool ValidateDate(int? startYear, int? startMonth, int? endYear, int? endMonth)
        {
            if (endYear < startYear || (endYear == startYear && endMonth < startMonth) || startYear < 1 || startYear > DateTime.Today.Year || endYear < 1 || endYear > DateTime.Today.Year || startMonth < 1 || startMonth > 12 || endMonth < 1 || endMonth > 12)
            {
                return false;
            }
            return true;
        }
    }
}