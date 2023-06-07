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
    [Authorize]
    public class ForumController : Controller
    {
        private IaaiTwDb db = new IaaiTwDb();
        private const int DefaultPageSize = 5;

        // GET: Forum
        public ActionResult Index(int? page)
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "會員專區", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "討論區", Url = "#" });
            ViewBag.Breadcrumb = breadcrumb;

            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            // 排序由最新到最舊
            return View(db.Boards.OrderByDescending(p => p.PostDate).ToPagedList(currentPageIndex, DefaultPageSize));
        }

        // GET: Forum/Create
        public ActionResult Create()
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "會員專區", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "討論區", Url = "#" });
            ViewBag.Breadcrumb = breadcrumb;

            return View();
        }

        // POST: Forum/Create
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string subject, string article)
        {
            Board board = new Board();
            if (ModelState.IsValid)
            {
                board.Subject = subject;
                board.Article = article;
                board.Author = Session["Name"].ToString();

                db.Boards.Add(board);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(board);
        }

        //// GET: Forum/Details/5
        //public ActionResult Details(int? id)
        //{
        //    var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
        //    breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "會員專區", Url = null });
        //    breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "討論區", Url = "#" });
        //    ViewBag.Breadcrumb = breadcrumb;

        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Board board = db.Boards.Find(id);
        //    if (board == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View(board);

        //}

        // GET: Forum/Details/5
        public ActionResult Details(int? id, int? page)
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "會員專區", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "討論區", Url = "#" });
            ViewBag.Breadcrumb = breadcrumb;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Board board = db.Boards.Find(id);
            if (board == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubjectId = board.SubjectId;
            ViewBag.Subject = board.Subject;
            ViewBag.Author = board.Author;
            ViewBag.PostDate = board.PostDate.ToString("yyyy-MM-dd");
            ViewBag.Article = board.Article;

            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            return View(board.BoardReplies.OrderByDescending(p => p.ReplyDate).ToPagedList(currentPageIndex, DefaultPageSize));
        }

        // GET: Forum/CreateRe
        public ActionResult CreateRe(int id)
        {
            var breadcrumb = new List<ViewModel.BreadcrumbsItem>();
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "會員專區", Url = null });
            breadcrumb.Add(new ViewModel.BreadcrumbsItem { Text = "討論區", Url = "#" });
            ViewBag.Breadcrumb = breadcrumb;

            Board board = db.Boards.Find(id);
            ViewBag.Subject = board.Subject;

            return View();
        }

        // POST: Forum/CreateRe
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRe(int id, string replyArticle)
        {
            BoardReply reply = new BoardReply();
            if (ModelState.IsValid)
            {
                reply.SubjectId = id;
                reply.ReplyArticle = replyArticle;
                reply.Replyer = Session["Name"].ToString();

                db.BoardReplies.Add(reply);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reply);
        }
    }
}