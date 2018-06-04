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
        public static ConfigModel config = new ConfigModel();
        public static LogsModel logs = new LogsModel();
        public static PhotosModel photos = new PhotosModel();
        public static ImageWebModel imageWeb = new ImageWebModel();

        public ActionResult Config()
        {
            ViewBag.Message = "The App Configuration.";
           
            return View(config);
        }

        public ActionResult ImageWeb()
        {
            ViewBag.Message = "The main home page.";

            ViewBag.IsConnected = imageWeb.IsConnected;
            ViewBag.NumOfPics = imageWeb.NumOfPics;
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

        public ActionResult Confirm()
        {
            ViewBag.Message = "The photos saved.";

            return View(config);
        }

        public ActionResult DeleteCancel()
        {
            return RedirectToAction("Config");
        }

        public ActionResult DeleteOK(string handlerToRemove)
        {
            config.RemoveHandler(handlerToRemove);
            return RedirectToAction("Config");
        }
    }
}