using IAAITW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAAITW.Controllers
{
    public class ArticleController : Controller
    {
        private IaaiTwDb db = new IaaiTwDb();

        // GET: Article/AboutUs
        public ActionResult AboutUs()
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "關於我們", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "關於我們", Url = "#" });
            ViewBag.Breadcrumb = breadcrumb;

            About about = db.Abouts.FirstOrDefault();

            return View(about);
        }

        // GET: Article/Organization
        public ActionResult Organization()
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "關於我們", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "組織架構", Url = "#" });
            ViewBag.Breadcrumb = breadcrumb;

            About about = db.Abouts.FirstOrDefault();

            return View(about);
        }

        // GET: Article/Organization
        public ActionResult History()
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "關於我們", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "沿革", Url = "#" });
            ViewBag.Breadcrumb = breadcrumb;

            About about = db.Abouts.FirstOrDefault();

            return View(about);
        }

        // GET: Article/Organization
        public ActionResult Member()
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "關於我們", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "配證會員", Url = "#" });
            ViewBag.Breadcrumb = breadcrumb;

            About about = db.Abouts.FirstOrDefault();

            return View(about);
        }

        // GET: Article/Expert
        public ActionResult Expert()
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "關於我們", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "專家介紹", Url = "#" });
            ViewBag.Breadcrumb = breadcrumb;

            About about = db.Abouts.FirstOrDefault();

            return View(about);
        }

        // GET: Article/Job
        public ActionResult Job()
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "協會業務", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "協會業務", Url = "#" });
            ViewBag.Breadcrumb = breadcrumb;

            Service service = db.Services.FirstOrDefault();

            return View(service);
        }

        // GET: Article/Licenses
        public ActionResult Licenses()
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "協會業務", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "訓練、認證、發照", Url = "#" });
            ViewBag.Breadcrumb = breadcrumb;

            Service service = db.Services.FirstOrDefault();

            return View(service);
        }

        // GET: Article/Refer
        public ActionResult Refer()
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "協會業務", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "諮詢、顧問", Url = "#" });
            ViewBag.Breadcrumb = breadcrumb;

            Service service = db.Services.FirstOrDefault();

            return View(service);
        }

        // GET: Article/Survey
        public ActionResult Survey()
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "協會業務", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "縱火調查", Url = "#" });
            ViewBag.Breadcrumb = breadcrumb;

            Service service = db.Services.FirstOrDefault();

            return View(service);
        }
    }
}