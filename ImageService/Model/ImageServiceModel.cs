using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Model
{
    public class ImageServiceModel : IImageServiceModel
    {
        public DirectoryInfo outputDir = null;
        public string AddFile(string path, out bool result)
        {
           
           if (!outputDir.Exists)
            {
                outputDir.Create();
            }
            DateTime creation = File.GetCreationTime(path);
            int year = creation.Year;
            String fullYearPath = outputDir.FullName + "/" + year.ToString();
            DirectoryInfo subYear = CreateFolder(fullYearPath);
            int month = creation.Month;
            String fullMonthPath = subYear.FullName + "/" + month.ToString();
            DirectoryInfo subMonth = CreateFolder(fullMonthPath);
            try
            {
                MoveFile(path, subMonth.FullName);
                result = true;
            } catch (Exception e)
            {
                result = false;
                return e.ToString();
            }
            return "";
        }

        public void MoveFile(String source, String destination)
        {
            File.Move(source, destination);
        }

        public DirectoryInfo CreateFolder(String fullPath)
        {
            return outputDir.CreateSubdirectory(fullPath);
        }
    }
}
