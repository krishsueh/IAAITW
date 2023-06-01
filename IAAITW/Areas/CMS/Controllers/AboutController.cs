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

            var existingAbout = db.Abouts.FirstOrDefault();
            if (existingAbout == null)
            {
                return View();
            }
            return View(existingAbout);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult AboutUs(About editor)
        {
            if (ModelState.IsValid)
            {
                var existingAbout = db.Abouts.FirstOrDefault();
                if (existingAbout == null)
                {
                    About addingAbout = new About();
                    addingAbout.AboutUs = editor.AboutUs;
                    db.Abouts.Add(addingAbout);
                    db.SaveChanges();
                    return RedirectToAction("AboutUs");
                }
                existingAbout.AboutUs = editor.AboutUs;
                db.Entry(existingAbout).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AboutUs");
            }
            return View(editor);
        }

        public ActionResult Organization()
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "關於我們", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "組織架構", Url = null });
            ViewBag.Breadcrumb = breadcrumb;

            var existingAbout = db.Abouts.FirstOrDefault();
            if (existingAbout == null)
            {
                return View();
            }
            return View(existingAbout);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Organization(About editor)
        {
            if (ModelState.IsValid)
            {
                var existingAbout = db.Abouts.FirstOrDefault();
                if (existingAbout == null)
                {
                    About addingAbout = new About();
                    addingAbout.Organization = editor.Organization;
                    db.Abouts.Add(addingAbout);
                    db.SaveChanges();
                    return RedirectToAction("Organization");
                }
                existingAbout.Organization = editor.Organization;
                db.Entry(existingAbout).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Organization");
            }
            return View(editor);
        }

        public ActionResult History()
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "關於我們", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "沿革", Url = null });
            ViewBag.Breadcrumb = breadcrumb;

            var existingAbout = db.Abouts.FirstOrDefault();
            if (existingAbout == null)
            {
                return View();
            }
            return View(existingAbout);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult History(About editor)
        {
            if (ModelState.IsValid)
            {
                var existingAbout = db.Abouts.FirstOrDefault();
                if (existingAbout == null)
                {
                    About addingAbout = new About();
                    addingAbout.History = editor.History;
                    db.Abouts.Add(addingAbout);
                    db.SaveChanges();
                    return RedirectToAction("History");
                }
                existingAbout.History = editor.History;
                db.Entry(existingAbout).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("History");
            }
            return View(editor);
        }

        public ActionResult LicensedMember()
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "關於我們", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "配證會員", Url = null });
            ViewBag.Breadcrumb = breadcrumb;

            var existingAbout = db.Abouts.FirstOrDefault();
            if (existingAbout == null)
            {
                return View();
            }
            return View(existingAbout);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult LicensedMember(About editor)
        {
            if (ModelState.IsValid)
            {
                var existingAbout = db.Abouts.FirstOrDefault();
                if (existingAbout == null)
                {
                    About addingAbout = new About();
                    addingAbout.LicensedMember = editor.LicensedMember;
                    db.Abouts.Add(addingAbout);
                    db.SaveChanges();
                    return RedirectToAction("LicensedMember");
                }
                existingAbout.LicensedMember = editor.LicensedMember;
                db.Entry(existingAbout).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("LicensedMember");
            }
            return View(editor);
        }

        public ActionResult Expert()
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "關於我們", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "專家介紹", Url = null });
            ViewBag.Breadcrumb = breadcrumb;

            var existingAbout = db.Abouts.FirstOrDefault();
            if (existingAbout == null)
            {
                return View();
            }
            return View(existingAbout);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Expert(About editor)
        {
            if (ModelState.IsValid)
            {
                var existingAbout = db.Abouts.FirstOrDefault();
                if (existingAbout == null)
                {
                    About addingAbout = new About();
                    addingAbout.Expert = editor.Expert;
                    db.Abouts.Add(addingAbout);
                    db.SaveChanges();
                    return RedirectToAction("Expert");
                }
                existingAbout.Expert = editor.Expert;
                db.Entry(existingAbout).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Expert");
            }
            return View(editor);
        }
    }
}