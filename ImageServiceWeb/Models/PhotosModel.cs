using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class PhotosModel
    {
        
        //public event NotifyAboutChange NotifyEvent;

        public PhotosModel()
        {
            ImageList = GetPhotos();
        }

        public List<Image> ImageList
        {
            get; set;
        } 

        

        public List<Image> GetPhotos()
        {
            List<Image> photosList = new List<Image>();



            return photosList;
        }
    }
}