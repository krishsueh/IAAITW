using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IAAITW.Areas.CMS.Security;
using IAAITW.Models;
using MvcPaging;

namespace IAAITW.Areas.CMS.Controllers
{
    [PermissionFilters]
    public class NewsController : Controller
    {
        private IaaiTwDb db = new IaaiTwDb();
        private const int DefaultPageSize = 5;

        // GET: CMS/News
        public ActionResult Index(int? page, string search)
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "訊息列表", Url = null });
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

            var news = db.News.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                news = news.Where(x => x.Title.Contains(search) || x.Content.Contains(search));
            }

            ViewBag.Search = search;
            // 排序由最新到最舊
            return View(news.OrderByDescending(p => p.GoTop).ThenByDescending(p => p.ReleaseDate).ToPagedList(currentPageIndex, DefaultPageSize));
        }

        // GET: CMS/News/Create
        public ActionResult Create()
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "訊息列表", Url = Url.Action("Index", "News") });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "新增訊息", Url = null });
            ViewBag.Breadcrumb = breadcrumb;

            return View();
        }

        // POST: CMS/News/Create
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(News news, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                // 訊息封面
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string extension = Path.GetExtension(fileName);

                    if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif")
                    {
                        string newFileName = news.ReleaseDate.ToString("yyyy-MM-dd") + "_" + fileName;
                        var path = Path.Combine(Server.MapPath("~/upload/news_cover"), newFileName);
                        file.SaveAs(path);

                        news.CoverImg = newFileName;
                    }
                    else
                    {
                        ViewBag.Msg = "檔案格式不符合";
                        return View(news);
                    }
                }
                else
                {
                    news.CoverImg = "default.jpg";
                }

                db.News.Add(news);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(news);
        }

        // GET: CMS/News/Edit/5
        public ActionResult Edit(int? id)
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "訊息列表", Url = Url.Action("Index", "News") });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "編輯訊息", Url = null });
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

        // POST: CMS/News/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(News news, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                // 封面
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string extension = Path.GetExtension(fileName);

                    if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif")
                    {
                        string newFileName = news.ReleaseDate.ToString("yyyy-MM-dd") + "_" + fileName;
                        var path = Path.Combine(Server.MapPath("~/upload/news_cover"), newFileName);
                        file.SaveAs(path);

                        news.CoverImg = newFileName;
                    }
                    else
                    {
                        ViewBag.Msg = "檔案格式不符合";
                        return View(news);
                    }
                }
                else
                {
                    news.CoverImg = db.News.Where(x => x.Id == news.Id).Select(x => x.CoverImg).FirstOrDefault();
                }

                db.Entry(news).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(news);
        }

        // POST: CMS/News/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            News news = db.News.Find(id);
            db.News.Remove(news);
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

        // POST: CMS/News/GoTop
        [HttpPost]
        public ActionResult GoTop(int? id)
        {
            News news = db.News.Find(id);

            if (news.GoTop == false)
                news.GoTop = true;
            else
                news.GoTop = false;

            db.Entry(news).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
