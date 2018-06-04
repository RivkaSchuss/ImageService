using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class PhotosModel
    {

        //public event NotifyAboutChange NotifyEvent;
        private string outputDir;
        private static ConfigModel config;
        public PhotosModel()
        {
            ImageList = GetPhotos();
            config = new ConfigModel();
            outputDir = config.OutputDirectory;
        }

        public List<Image> ImageList
        {
            get; set;
        } 

        

        public List<Image> GetPhotos()
        {
            List<Image> photosList = new List<Image>();
            string thumbnailDir = outputDir + "\\Thumbnails";
            if (!Directory.Exists(thumbnailDir))
            {
                return null;
            }
            DirectoryInfo di = new DirectoryInfo(thumbnailDir);

            string[] validExtensions = { ".jpg", ".png", ".gif", ".bmp" };
            foreach (DirectoryInfo yearDirInfo in di.GetDirectories())
            {
                if (!Path.GetDirectoryName(yearDirInfo.FullName).EndsWith("Thumbnails"))
                {
                    continue;
                }
                foreach (DirectoryInfo monthDirInfo in yearDirInfo.GetDirectories())
                {


                    foreach (FileInfo fileInfo in monthDirInfo.GetFiles())
                    {
                        if (validExtensions.Contains(fileInfo.Extension.ToLower()))
                        {
                            
                            //photosList.Add(new Image(fileInfo.FullName));
                        }
                    }
                }
            }


            return photosList;
        }
    }
}