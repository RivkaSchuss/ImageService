using ImageService.Logging;
using ImageService.Model;
using ImageService.Model.Event;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageService.Controller.Handlers
{
    public class DirectoryHandler : IDirectoryHandler
    {
        //region members
        private IImageController m_controller; //the image processing controller
        private ILoggingService m_logging;
        private FileSystemWatcher m_watcher;
        private string direcPath;
        //end region
        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;
        

        public DirectoryHandler(IImageController controller, ILoggingService logging)
        {
            m_controller = controller;
            m_logging = logging;
        }

        public void StartHandleDirectory(string dirPath)
        {
            direcPath = dirPath;
            m_watcher = new FileSystemWatcher(direcPath);
            m_watcher.Created += new FileSystemEventHandler(NewPicture);
            m_watcher.Changed += new FileSystemEventHandler(NewPicture);
            m_watcher.EnableRaisingEvents = true;
        }

        public void NewPicture(object sender, FileSystemEventArgs e)
        {
            string[] filters = { ".jpg", ".png", ".gif", ".bmp" };
            if (filters.Contains(Path.GetExtension(e.FullPath)))
            {
                    DateTime dateTime = GetDateAndTime();
                    
            }
            
        }

        public DateTime GetDateAndTime()
        {
            Regex r = new Regex(":");
            using (FileStream fs = new FileStream(direcPath, FileMode.Open, FileAccess.Read))
            using (Image myImage = Image.FromStream(fs, false, false))
            {
                PropertyItem propItem = myImage.GetPropertyItem(36867);
                string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                return DateTime.Parse(dateTaken);
            }
        }

    
        public void OnCommandReceived(object sender, CommandReceivedEventArgs e)
        {
            if(direcPath.Equals(e.RequestDirPath))
            {
                bool result;
                m_controller.ExecuteCommand(e.CommandID, e.Args, out result);
            }
        }

        public void closeHandler()
        {
            m_watcher.Dispose();
            DirectoryClose.Invoke(this, new DirectoryCloseEventArgs(direcPath, ""));
        }


    }
}
