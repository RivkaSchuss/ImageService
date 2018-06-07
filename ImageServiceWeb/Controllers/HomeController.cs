using ImageServiceWeb.Models;
using Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageServiceWeb.Controllers
{
    public class HomeController : Controller
    {
        private static ConfigModel config = new ConfigModel();
        private static LogsModel logs = new LogsModel();
        private static PhotosModel photos = new PhotosModel(config);
        private static ImageWebModel imageWeb = new ImageWebModel();
        private static string m_handlerRequested = null;

        public ActionResult Config()
        {
            ViewBag.Message = "The App Configuration.";
            config.SendConfigRequest();
            return View(config);
        }

        public ActionResult ImageWeb()
        {
            ViewBag.Message = "The main home page.";
            ViewBag.IsConnected = imageWeb.IsConnected;
            photos.SetPhotos();
            imageWeb.NumOfPics = photos.NumOfPics;
            ViewBag.NumOfPics = imageWeb.NumOfPics;
            return View(imageWeb);
        }

        public ActionResult Logs()
        {
            ViewBag.Message = "The list of service logs.";
            logs.SendLogRequest();
            return View(logs);
        }

        public ActionResult Photos()
        {
            ViewBag.Message = "The photos saved.";
            photos.ImageList.Clear();
            photos.SetPhotos();
            return View(photos);
        }

        public ActionResult Confirm()
        {
            ViewBag.Message = "The photos saved.";

            return View(config);
        }

        public ActionResult ConfirmDeleteHandler(string handlerToRemove)
        {
            m_handlerRequested = handlerToRemove;
            return View();
        }

        public ActionResult DeleteOK()
        {
            config.RemoveHandler(m_handlerRequested);
            return RedirectToAction("Config");
        }

        public ActionResult DeleteCancel()
        {
            return RedirectToAction("Config");
        }

        public ActionResult FilterLogs(MessageTypeEnum filter)
        {
            logs.FilterLogList(filter);
            return View(logs);
        }

        public ActionResult PhotosViewer(string fullUrl)
        {
            Photo photo = new Photo(fullUrl);
            return View(photo);
        }

        public ActionResult PhotosDelete(string fullUrl)
        {
            Photo photo = new Photo(fullUrl);
            return View(photo);
        }

        public ActionResult DeleteSpecificPhoto(string fullUrl)
        {
            photos.DeletePhoto(fullUrl);
            return RedirectToAction("Photos");
        }
    }
}