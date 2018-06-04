using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class PhotosModel
    {
        private List<Image> imageList;
        //public event NotifyAboutChange NotifyEvent;

        public PhotosModel()
        {
            imageList = new List<Image>();
        }

        public List<Image> ImageList
        {
            get
            {
                return this.imageList;
            }
        } 

        

        public void GetPhotos()
        {

        }
    }
}