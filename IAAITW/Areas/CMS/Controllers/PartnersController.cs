using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IAAITW.Models;
using MvcPaging;

namespace IAAITW.Areas.CMS.Controllers
{
    public class PartnersController : Controller
    {
        private IaaiTwDb db = new IaaiTwDb();
        private const int DefaultPageSize = 3;

        // GET: CMS/Partners
        public ActionResult Index(int? page)
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "協力單位", Url = null });
            ViewBag.Breadcrumb = breadcrumb;

            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            return View(db.Partners.OrderBy(p => p.Id).ToPagedList(currentPageIndex, DefaultPageSize));
        }

        [HttpPost]
        public ActionResult Index(string search, int? page)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            var partners = db.Partners.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                partners = partners.Where(x => x.Name.Contains(search));
            }
            ViewBag.Search = search;
            return View(partners.OrderBy(p => p.Id).ToPagedList(currentPageIndex, DefaultPageSize));
        }

        // GET: CMS/Partners/Create
        public ActionResult Create()
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "協力單位", Url = Url.Action("Index", "Partners") });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "新增協力單位", Url = null });
            ViewBag.Breadcrumb = breadcrumb;

            return View();
        }

        // POST: CMS/Partners/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Partner partner, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                // 圖片
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string extension = Path.GetExtension(fileName);

                    if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif")
                    {
                        string newFileName = partner.Name + extension;
                        var path = Path.Combine(Server.MapPath("~/upload/partner"), newFileName);
                        file.SaveAs(path);

                        partner.Image = newFileName;
                    }
                    else
                    {
                        ViewBag.Msg = "檔案格式不符合";
                        return View(partner);
                    }
                }
                else
                {
                    ViewBag.Msg = "請上傳圖片";
                    return View(partner);
                }

                db.Partners.Add(partner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(partner);
        }

        // GET: CMS/Partners/Edit/5
        public ActionResult Edit(int? id)
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "協力單位", Url = Url.Action("Index", "Partners") });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "編輯協力單位", Url = null });
            ViewBag.Breadcrumb = breadcrumb;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partner partner = db.Partners.Find(id);
            if (partner == null)
            {
                return HttpNotFound();
            }
            return View(partner);
        }

        // POST: CMS/Partners/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Partner partner, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                // 圖片
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string extension = Path.GetExtension(fileName);

                    if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif")
                    {
                        string newFileName = partner.Name + extension;
                        var path = Path.Combine(Server.MapPath("~/upload/partner"), newFileName);
                        file.SaveAs(path);

                        partner.Image = newFileName;
                    }
                    else
                    {
                        ViewBag.Msg = "檔案格式不符合";
                        return View(partner);
                    }
                }
                else
                {
                    partner.Image = db.Partners.Where(x => x.Id == partner.Id).Select(x => x.Image).FirstOrDefault();
                }

                db.Entry(partner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(partner);
        }

        // POST: CMS/Partners/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Partner partner = db.Partners.Find(id);
            db.Partners.Remove(partner);
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
