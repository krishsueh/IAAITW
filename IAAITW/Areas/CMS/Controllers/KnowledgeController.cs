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
    public class KnowledgeController : Controller
    {
        private IaaiTwDb db = new IaaiTwDb();
        private const int DefaultPageSize = 5;

        // GET: CMS/Knowledge
        public ActionResult Index(int? page)
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "知識庫", Url = null });
            ViewBag.Breadcrumb = breadcrumb;

            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            return View(db.Knowledges.OrderByDescending(p => p.ReleaseDate).ToPagedList(currentPageIndex, DefaultPageSize));
        }

        // GET: CMS/Knowledge/Details/5
        public ActionResult Details(int? id)
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "知識庫", Url = Url.Action("Index", "Knowledge") });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "知識庫項目", Url = null });
            ViewBag.Breadcrumb = breadcrumb;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Knowledge knowledge = db.Knowledges.Find(id);
            if (knowledge == null)
            {
                return HttpNotFound();
            }
            return View(knowledge);
        }

        // GET: CMS/Knowledge/Create
        public ActionResult Create()
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "知識庫", Url = Url.Action("Index", "Knowledge") });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "新增知識庫", Url = null });
            ViewBag.Breadcrumb = breadcrumb;

            return View();
        }

        // POST: CMS/Knowledge/Create
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Knowledge knowledge, HttpPostedFileBase file)
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
                        string newFileName = knowledge.ReleaseDate.ToString("yyyy-MM-dd") + "_" + fileName;
                        var path = Path.Combine(Server.MapPath("~/upload/knowledge_cover"), newFileName);
                        file.SaveAs(path);

                        knowledge.CoverImg = newFileName;
                    }
                    else
                    {
                        ViewBag.Msg = "檔案格式不符合";
                        return View(knowledge);
                    }
                }

                db.Knowledges.Add(knowledge);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(knowledge);
        }

        // GET: CMS/Knowledge/Edit/5
        public ActionResult Edit(int? id)
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "知識庫", Url = Url.Action("Index", "Knowledge") });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "編輯知識庫", Url = null });
            ViewBag.Breadcrumb = breadcrumb;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Knowledge knowledge = db.Knowledges.Find(id);
            if (knowledge == null)
            {
                return HttpNotFound();
            }
            return View(knowledge);
        }

        // POST: CMS/Knowledge/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Knowledge knowledge, HttpPostedFileBase file)
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
                        string newFileName = knowledge.ReleaseDate.ToString("yyyy-MM-dd") + "_" + fileName;
                        var path = Path.Combine(Server.MapPath("~/upload/knowledge_cover"), newFileName);
                        file.SaveAs(path);

                        knowledge.CoverImg = newFileName;
                    }
                    else
                    {
                        ViewBag.Msg = "檔案格式不符合";
                        return View(knowledge);
                    }
                }
                else
                {
                    knowledge.CoverImg = db.Knowledges.Where(x => x.Id == knowledge.Id).Select(x => x.CoverImg).FirstOrDefault();
                }

                db.Entry(knowledge).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(knowledge);
        }

        // GET: CMS/Knowledgess/Delete/5
        public ActionResult Delete(int? id)
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "知識庫", Url = Url.Action("Index", "Knowledge") });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "刪除知識庫", Url = null });
            ViewBag.Breadcrumb = breadcrumb;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Knowledge knowledge = db.Knowledges.Find(id);
            if (knowledge == null)
            {
                return HttpNotFound();
            }
            return View(knowledge);
        }

        // POST: CMS/Knowledgess/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Knowledge knowledge = db.Knowledges.Find(id);
            db.Knowledges.Remove(knowledge);
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
