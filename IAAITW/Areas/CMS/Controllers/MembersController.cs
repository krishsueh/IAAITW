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
using IAAITW.Areas.CMS.Security;
using IAAITW.Models;
using MvcPaging;

namespace IAAITW.Areas.CMS.Controllers
{
    [PermissionFilters]
    [Authorize(Roles = "最高管理者")]
    public class MembersController : Controller
    {
        private IaaiTwDb db = new IaaiTwDb();
        private const int DefaultPageSize = 5;

        // GET: CMS/Members
        public ActionResult Index(int? page, string search)
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "會員專區", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "會員列表", Url = null });
            ViewBag.Breadcrumb = breadcrumb;

            if (page == null)
            {
                Session.Clear();
            }

            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            if (!string.IsNullOrEmpty(search))
            {
                Session["search"] = search;
            }
            else
            {
                if (Session["search"] != null)
                {
                    search = Session["search"].ToString();
                }
            }

            var members = db.Members.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                members = members.Where(x => x.Account.Contains(search) || x.Name.Contains(search) || x.Gender.ToString().Contains(search) || x.MemberTypes.ToString().Contains(search) || x.Tel.Contains(search) || x.Mobile.Contains(search) || x.Address.Contains(search) || x.Email.Contains(search));
            }

            ViewBag.Search = search;
            return View(members.OrderBy(x => x.Id).ToPagedList(currentPageIndex, DefaultPageSize));
        }

        // GET: CMS/Members/Details/5
        public ActionResult Details(int? id)
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "會員專區", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "會員資料", Url = null });
            ViewBag.Breadcrumb = breadcrumb;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // GET: CMS/Members/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CMS/Members/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Member member, HttpPostedFileBase file)
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

                // 密碼加密
                member.Password = BitConverter
                    .ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(member.Password))).Replace("-", null);

                // 會員證影本
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

                db.Members.Add(member);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(member);
        }

        // GET: CMS/Members/Edit/5
        public ActionResult Edit(int? id)
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "會員專區", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "會員資料編輯", Url = null });
            ViewBag.Breadcrumb = breadcrumb;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: CMS/Members/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Member member)
        {
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(member);
        }

        // GET: CMS/Members/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: CMS/Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Member member = db.Members.Find(id);
            db.Members.Remove(member);
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