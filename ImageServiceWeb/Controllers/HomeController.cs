using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageServiceWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Config()
        {
            ViewBag.Message = "The App Configuration.";

            return View();
        }

        public ActionResult ImageWeb()
        {
            ViewBag.Message = "The main home page.";

            return View();
        }

        public ActionResult Logs()
        {
            ViewBag.Message = "The list of service logs.";

            return View();
        }

        public ActionResult Photos()
        {
            ViewBag.Message = "The photos saved.";

            return View();
        }
    }
}