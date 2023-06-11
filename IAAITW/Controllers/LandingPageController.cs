using IAAITW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAAITW.Controllers
{
    public class LandingPageController : Controller
    {
        private IaaiTwDb db = new IaaiTwDb();

        // GET: LandingPage
        public ActionResult Index()
        {
            IEnumerable<Carousel> carousels = db.Carousels.ToList().OrderBy(p => p.Id);
            ViewBag.Carousel = carousels;

            IEnumerable<News> news = db.News.ToList().OrderByDescending(p => p.GoTop).ThenByDescending(p => p.ReleaseDate).Take(4);
            ViewBag.News = news;

            IEnumerable<Partner> partners = db.Partners.ToList().OrderBy(p => p.Id);
            ViewBag.Partners = partners;

            return View();
        }
    }
}