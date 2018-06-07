using ImageServiceWeb.Models;
using Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageServiceWeb.Controllers
{
    /// <summary>
    /// the home controller for all the views.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class HomeController : Controller
    {
        private static ConfigModel config = new ConfigModel();
        private static LogsModel logs = new LogsModel();
        private static PhotosModel photos = new PhotosModel(config);
        private static ImageWebModel imageWeb = new ImageWebModel();
        private static string m_handlerRequested = null;

        /// <summary>
        /// the controller for the view of the config settings
        /// </summary>
        /// <returns></returns>
        public ActionResult Config()
        {
            ViewBag.Message = "The App Configuration.";
            config.SendConfigRequest();
            return View(config);
        }

        /// <summary>
        /// the controller for the view of the main image web page
        /// </summary>
        /// <returns></returns>
        public ActionResult ImageWeb()
        {
            ViewBag.Message = "The main home page.";
            ViewBag.IsConnected = imageWeb.IsConnected;
            photos.SetPhotos();
            imageWeb.NumOfPics = photos.NumOfPics;
            ViewBag.NumOfPics = imageWeb.NumOfPics;
            return View(imageWeb);
        }

        /// <summary>
        /// the controller for the view of the logs page
        /// </summary>
        /// <returns></returns>
        public ActionResult Logs()
        {
            ViewBag.Message = "The list of service logs.";
            logs.SendLogRequest();
            return View(logs);
        }

        /// <summary>
        /// the controller for the view of the photos display
        /// </summary>
        /// <returns></returns>
        public ActionResult Photos()
        {
            ViewBag.Message = "The photos saved.";
            photos.ImageList.Clear();
            photos.SetPhotos();
            return View(photos);
        }

        /// <summary>
        /// the controller for when there has been a confirmation, returns to the config view
        /// </summary>
        /// <returns></returns>
        public ActionResult Confirm()
        {
            ViewBag.Message = "The photos saved.";

            return View(config);
        }

        /// <summary>
        /// sets the handler to be removed and removes to the view
        /// </summary>
        /// <param name="handlerToRemove">The handler to remove.</param>
        /// <returns></returns>
        public ActionResult ConfirmDeleteHandler(string handlerToRemove)
        {
            m_handlerRequested = handlerToRemove;
            return View();
        }

        /// <summary>
        /// invoked when the ok button has been selected to delete the handler, calls the function to delete the handler and returns to the view
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteOK()
        {
            config.RemoveHandler(m_handlerRequested);
            return RedirectToAction("Config");
        }

        /// <summary>
        /// invoked when the cancel button has been selected to delete the handler, returns to the config page
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteCancel()
        {
            return RedirectToAction("Config");
        }

        /// <summary>
        /// the controller for the photo viewer page, sends the photo to view
        /// </summary>
        /// <param name="fullUrl">The full URL.</param>
        /// <returns></returns>
        public ActionResult PhotosViewer(string fullUrl)
        {
            Photo photo = new Photo(fullUrl);
            return View(photo);
        }

        /// <summary>
        /// the controller for the photo deleting page, sends the photo to delete
        /// </summary>
        /// <param name="fullUrl">The full URL.</param>
        /// <returns></returns>
        public ActionResult PhotosDelete(string fullUrl)
        {
            Photo photo = new Photo(fullUrl);
            return View(photo);
        }

        /// <summary>
        /// Deletes the specific photo.
        /// </summary>
        /// <param name="fullUrl">The full URL.</param>
        /// <returns></returns>
        public ActionResult DeleteSpecificPhoto(string fullUrl)
        {
            photos.DeletePhoto(fullUrl);
            return RedirectToAction("Photos");
        }
    }
}