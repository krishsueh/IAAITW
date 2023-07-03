using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IAAITW.Areas.CMS.Security;
using IAAITW.Models;

namespace IAAITW.Areas.CMS.Controllers
{
    [PermissionFilters]
    public class ArticleController : Controller
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

        public ActionResult Job()
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "協會業務", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "協會業務", Url = null });
            ViewBag.Breadcrumb = breadcrumb;

            var existingService = db.Services.FirstOrDefault();
            if (existingService == null)
            {
                return View();
            }
            return View(existingService);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Job(Service view)
        {
            if (ModelState.IsValid)
            {
                var existingService = db.Services.FirstOrDefault();
                if (existingService == null)
                {
                    Service addingService = new Service();
                    addingService.Jobs = view.Jobs;
                    db.Services.Add(addingService);
                    db.SaveChanges();
                    return RedirectToAction("Job");
                }
                existingService.Jobs = view.Jobs;
                db.Entry(existingService).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Job");
            }
            return View(view);
        }

        public ActionResult Licenses()
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "協會業務", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "訓練、認證、發照", Url = null });
            ViewBag.Breadcrumb = breadcrumb;

            var existingService = db.Services.FirstOrDefault();
            if (existingService == null)
            {
                return View();
            }
            return View(existingService);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Licenses(Service view)
        {
            if (ModelState.IsValid)
            {
                var existingService = db.Services.FirstOrDefault();
                if (existingService == null)
                {
                    Service addingService = new Service();
                    addingService.Licenses = view.Licenses;
                    db.Services.Add(addingService);
                    db.SaveChanges();
                    return RedirectToAction("Licenses");
                }
                existingService.Licenses = view.Licenses;
                db.Entry(existingService).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Licenses");
            }
            return View(view);
        }

        public ActionResult Refer()
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "協會業務", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "諮詢、顧問", Url = null });
            ViewBag.Breadcrumb = breadcrumb;

            var existingService = db.Services.FirstOrDefault();
            if (existingService == null)
            {
                return View();
            }
            return View(existingService);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Refer(Service view)
        {
            if (ModelState.IsValid)
            {
                var existingService = db.Services.FirstOrDefault();
                if (existingService == null)
                {
                    Service addingService = new Service();
                    addingService.Refer = view.Refer;
                    db.Services.Add(addingService);
                    db.SaveChanges();
                    return RedirectToAction("Refer");
                }
                existingService.Refer = view.Refer;
                db.Entry(existingService).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Refer");
            }
            return View(view);
        }

        public ActionResult Survey()
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "協會業務", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "縱火調查", Url = null });
            ViewBag.Breadcrumb = breadcrumb;

            var existingService = db.Services.FirstOrDefault();
            if (existingService == null)
            {
                return View();
            }
            return View(existingService);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Survey(Service view)
        {
            if (ModelState.IsValid)
            {
                var existingService = db.Services.FirstOrDefault();
                if (existingService == null)
                {
                    Service addingService = new Service();
                    addingService.Survey = view.Survey;
                    db.Services.Add(addingService);
                    db.SaveChanges();
                    return RedirectToAction("Survey");
                }
                existingService.Survey = view.Survey;
                db.Entry(existingService).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Survey");
            }
            return View(view);
        }
    }
}