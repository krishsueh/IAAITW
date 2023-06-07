using IAAITW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}