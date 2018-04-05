using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Model
{
    public class ImageServiceModel : IImageServiceModel
    {
        private string outputDir = null;
        private int thumbnailSize;

        public ImageServiceModel(string outputDir, int thumbnailSize)
        {
            this.outputDir = outputDir;
            this.thumbnailSize = thumbnailSize;
        }

        public string AddFile(string path, out bool result)
        {
            if (File.Exists(path))
            {
                try
                {
                    //creating the output directory if it's not already exist
                    Directory.CreateDirectory(outputDir);

                    //crating the thumbnails directory in the output directory:
                    //getting the path to the thumbnails path
                    string thumbPath = System.IO.Path.Combine(outputDir.ToString(), "Thumbnails");
                    Directory.CreateDirectory(thumbPath);

                    //creating the year and month directories:
                    //getting the creation time of the image file
                    DateTime dateTime = File.GetCreationTime(path);
                    //getting the path to the year and the month as strings
                    string year = dateTime.Year.ToString();
                    string month = dateTime.Month.ToString();
                    //getting the path to the year and the month directories in the output directory.
                    string yearPath = System.IO.Path.Combine(outputDir.ToString(), year);
                    string monthPath = System.IO.Path.Combine(yearPath, month);
                    Directory.CreateDirectory(monthPath);
                    //getting the path to the year and the month directories in the thumbnails directory.
                    string thumbYear = System.IO.Path.Combine(thumbPath, yearPath);
                    string thumbMonth = System.IO.Path.Combine(thumbYear, monthPath);
                    Directory.CreateDirectory(thumbMonth);

                    //copying the file if it's not already exist:
                    //getting the full destinated path of the image
                    string imageFullPath = System.IO.Path.Combine(monthPath, Path.GetFileName(path));
                    if (!File.Exists(imageFullPath))
                    {
                        File.Copy(path, imageFullPath);
                    }

                    //creating a thumbnail for the file if it's not already exist:
                    //getting the full destinated path of the thumbnail of the image
                    string thumbnailFullPath = System.IO.Path.Combine(thumbMonth, Path.GetFileName(path));
                    if (!File.Exists(thumbnailFullPath))
                    {
                        Image image = Image.FromFile(path);
                        Image thumbnail = image.GetThumbnailImage(this.thumbnailSize, this.thumbnailSize, () => false, IntPtr.Zero);
                        thumbnail.Save(thumbnailFullPath);
                    }

                    result = true;
                    return "";
                } catch(Exception e)
                {
                    result = false;
                    return e.ToString();
                }
            } else
            {
                result = false;
                return "File does not exist";
            }
        }
    }
}
