using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IAAITW.Models;

namespace IAAITW.Areas.CMS.Controllers
{
    public class AboutController : Controller
    {
        private IaaiTwDb db = new IaaiTwDb();

        // GET: CMS/AboutUs
        public ActionResult AboutUs()
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "關於我們", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "關於我們", Url = null });
            ViewBag.Breadcrumb = breadcrumb;

            var existingAboutUs = db.Abouts.FirstOrDefault();
            if (existingAboutUs == null)
            {
                return View();
                //return HttpNotFound();
            }
            return View(existingAboutUs);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult AboutUs(ViewModel.CKEditor view)
        {
            if (ModelState.IsValid)
            {
                var existingAboutUs = db.Abouts.FirstOrDefault();
                if (existingAboutUs == null)
                {
                    About addingAboutUs = new About();
                    addingAboutUs.AboutUs = view.AboutUs;
                    addingAboutUs.InitDate = DateTime.Now;
                    db.Abouts.Add(addingAboutUs);
                    db.SaveChanges();
                    return RedirectToAction("AboutUs");
                }
                existingAboutUs.AboutUs = view.AboutUs;
                existingAboutUs.InitDate = DateTime.Now;
                db.Entry(existingAboutUs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AboutUs");
            }
            return View(view);
        }
    }
}