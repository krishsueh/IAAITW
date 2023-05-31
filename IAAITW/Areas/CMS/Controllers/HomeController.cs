using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAAITW.Areas.CMS.Controllers
{
    public class HomeController : Controller
    {
        // GET: CMS/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}