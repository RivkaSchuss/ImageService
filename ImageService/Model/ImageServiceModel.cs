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
        public DirectoryInfo outputDir = null;
        public string AddFile(string path, DateTime dateTime, out bool result)
        {
            if (!outputDir.Exists)
            {
                outputDir.Create();
            }
            int year = dateTime.Year;
            string yearPath = year.ToString();
            string[] subs = Directory.GetDirectories(outputDir.FullName);
            if (!subs.Contains(yearPath))
            {
                outputDir.CreateSubdirectory(yearPath);
            }
            int month = dateTime.Month;
            string monthPath = month.ToString();
            subs = Directory.GetDirectories(yearPath);
            if (!subs.Contains(monthPath))
            {
                outputDir.CreateSubdirectory(monthPath);
            }
            try
            {
                MoveFile(path, monthPath);
                result = true;
                return "Adding a new file succeeded";
            } catch (Exception e)
            {
                result = false;
                return e.ToString();
            }
        }

        public void MoveFile(String source, String destination)
        {
            File.Move(source, destination);
            Image image = Image.FromFile(destination);
            Size thumbnailSize = GetThumbnailSize(image);
            Image thumbnail = image.GetThumbnailImage(thumbnailSize.Width, thumbnailSize.Height, null, IntPtr.Zero);
            thumbnail.Save(destination);
        }

        public static Size GetThumbnailSize(Image original)
        {
            const int maxPixels = 40;
            // Width and height.
            int originalWidth = original.Width;
            int originalHeight = original.Height;
            
            double factor;
            if (originalWidth > originalHeight)
            {
                factor = (double)maxPixels / originalWidth;
            }
            else
            {
                factor = (double)maxPixels / originalHeight;
            }

            // Return thumbnail size.
            return new Size((int)(originalWidth * factor), (int)(originalHeight * factor));
        }

        public DirectoryInfo CreateFolder(DirectoryInfo root, String name)
        {
            return root.CreateSubdirectory(name);
        }
    }
}
