using IAAITW.Models;
using MvcPaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace IAAITW.Controllers
{
    public class NewsController : Controller
    {
        private IaaiTwDb db = new IaaiTwDb();
        private const int DefaultPageSize = 5;

        // GET: News
        public ActionResult Index(int? page)
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "訊息發佈", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "訊息發佈", Url = "#" });
            ViewBag.Breadcrumb = breadcrumb;

            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            // 排序由最新到最舊
            return View(db.News.OrderByDescending(p => p.ReleaseDate).ToPagedList(currentPageIndex, DefaultPageSize));
        }

        // GET: News/Details/5
        public ActionResult Details(int? id)
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "訊息發佈", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "訊息發佈", Url = "#" });
            ViewBag.Breadcrumb = breadcrumb;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }
    }
}