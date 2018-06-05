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
        private static ConfigModel config;
        private static LogsModel logs;
        private static PhotosModel photos;
        private static ImageWebModel imageWeb;

        public ActionResult Config()
        {
            ViewBag.Message = "The App Configuration.";
            config = new ConfigModel();
            return View(config);
        }

        public ActionResult ImageWeb()
        {
            ViewBag.Message = "The main home page.";
            imageWeb = new ImageWebModel();
            ViewBag.IsConnected = imageWeb.IsConnected;
            ViewBag.NumOfPics = imageWeb.NumOfPics;
            return View(imageWeb);
        }

        public ActionResult Logs()
        {
            ViewBag.Message = "The list of service logs.";
            logs = new LogsModel();
            return View(logs);
        }

        public ActionResult Photos()
        {
            ViewBag.Message = "The photos saved.";
            photos = new PhotosModel();
            return View(photos);
        }

        public ActionResult Confirm()
        {
            ViewBag.Message = "The photos saved.";

            return View(config);
        }

        public ActionResult Delete(string handlerToRemove)
        {
            config.RemoveHandler(handlerToRemove);
            return RedirectToAction("Config");
        }
    }
}