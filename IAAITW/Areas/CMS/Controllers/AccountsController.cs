﻿using System;
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
using IAAITW.Areas.CMS.Security;
using IAAITW.Models;
using MvcPaging;
using Newtonsoft.Json;

namespace IAAITW.Areas.CMS.Controllers
{
    [PermissionFilters]
    public class AccountsController : Controller
    {
        private IaaiTwDb db = new IaaiTwDb();
        private const int DefaultPageSize = 5;

        // GET: CMS/Accounts
        //[Authorize(Roles = "最高管理者")]
        public ActionResult Index(int? page, string search)
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "帳號管理", Url = Url.Action("Index", "Accounts") });
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

            var cMSAccounts = db.CMSAccounts.Include(c => c.MyIdentity);
            if (!string.IsNullOrEmpty(search))
            {
                cMSAccounts = cMSAccounts.Where(x => x.MyIdentity.Identity.Contains(search) || x.Name.Contains(search) || x.Email.Contains(search));
            }

            ViewBag.Search = search;
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


            List<Permission> permissions = db.Permissions.ToList();
            var roots = permissions.Where(p => p.ParentId == null);

            StringBuilder sbTree = new StringBuilder();
            sbTree.Append("[");
            foreach (var item in roots)
            {
                sbTree.Append(GetNode(item));
                sbTree.Append(",");
            }
            sbTree.Append("]");
            ViewBag.Tree = sbTree.ToString();

            ViewBag.IdentityId = new SelectList(db.CMSIdentities, "Id", "Identity");
            return View();
        }

        private string GetNode(Permission item)
        {
            StringBuilder sbNode = new StringBuilder();
            sbNode.Append($@"{{'id': '{item.Code}', 'text': '{item.Subject}'");

            if (item.Permissions.Count > 0)
            {
                sbNode.Append(", 'children': [");
                foreach (var items in item.Permissions)
                {
                    sbNode.Append(GetNode(items));
                    sbNode.Append(",");
                }
                sbNode.Append("]");
            }
            
            sbNode.Append("}");

            return sbNode.ToString();
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

            List<Permission> permissions = db.Permissions.ToList();
            var roots = permissions.Where(p => p.ParentId == null);

            StringBuilder sbTree = new StringBuilder();
            sbTree.Append("[");
            foreach (var item in roots)
            {
                sbTree.Append(GetNode(item));
                sbTree.Append(",");
            }
            sbTree.Append("]");
            ViewBag.Tree = sbTree.ToString();

            if (!String.IsNullOrEmpty(cMSAccount.Permission))
            {
                string[] array = cMSAccount.Permission.ToString().Split(',');
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = "'" + array[i] + "'";
                }
                string open = "[" + string.Join(",", array) + "]";
                ViewBag.Default = open.ToString();
            }
            else
            {
                ViewBag.Default = "['']";
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
        //[Authorize(Roles = "最高管理者")]
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
