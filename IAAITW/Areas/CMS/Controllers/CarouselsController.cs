using IAAITW.Areas.CMS.Security;
using IAAITW.Models;
using MvcPaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAAITW.Areas.CMS.Controllers
{
    [PermissionFilters]
    public class CarouselsController : Controller
    {
        private IaaiTwDb db = new IaaiTwDb();
        private const int DefaultPageSize = 10;

        // GET: CMS/Carousels
        public ActionResult Index(int? page)
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "關於我們", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "輪播圖", Url = null });
            ViewBag.Breadcrumb = breadcrumb;

            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            return View(db.Carousels.OrderBy(p => p.Id).ToPagedList(currentPageIndex, DefaultPageSize));
        }

        // GET: CMS/Carousels/Create
        public ActionResult Create()
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "輪播圖", Url = Url.Action("Index", "Carousels") });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "新增輪播圖", Url = null });
            ViewBag.Breadcrumb = breadcrumb;

            return View();
        }

        // POST: CMS/Carousels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase file)
        {
            Carousel carousel = new Carousel();
            // 圖片
            if (file != null && file.ContentLength > 0)
            {
                string fileName = Path.GetFileName(file.FileName);
                string extension = Path.GetExtension(fileName);

                if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif")
                {
                    string newFileName = fileName;
                    var path = Path.Combine(Server.MapPath("~/upload/carousel"), newFileName);
                    file.SaveAs(path);

                    carousel.Image = newFileName;

                    db.Carousels.Add(carousel);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Msg = "檔案格式不符合";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.Msg = "請上傳圖片";
                return RedirectToAction("Index");
            }
        }

        // POST: CMS/Carousels/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Carousel carousel = db.Carousels.Find(id);
            db.Carousels.Remove(carousel);
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