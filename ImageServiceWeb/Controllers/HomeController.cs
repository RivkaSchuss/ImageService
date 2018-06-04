using ImageServiceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageServiceWeb.Controllers
{
    public class HomeController : Controller
    {
        private ConfigModel config = new ConfigModel();
        private LogsModel logs = new LogsModel();
        private PhotosModel photos = new PhotosModel();
        private ImageWebModel imageWeb = new ImageWebModel();
        public ActionResult Config()
        {
            ViewBag.Message = "The App Configuration.";

            return View(config);
        }

        public ActionResult ImageWeb()
        {
            ViewBag.Message = "The main home page.";

            return View(imageWeb);
        }

        public ActionResult Logs()
        {
            ViewBag.Message = "The list of service logs.";

            return View(logs);
        }

        public ActionResult Photos()
        {
            ViewBag.Message = "The photos saved.";

            return View(photos);
        }
    }
}