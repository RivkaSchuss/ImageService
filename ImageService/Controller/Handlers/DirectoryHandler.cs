using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Model;
using ImageService.Model;
using ImageService.Model.Event;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
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
            m_watcher = new FileSystemWatcher(dirPath);
            //m_watcher.Path = dirPath;
            //Register a handler that gets called when a file is created
            m_watcher.Created += new FileSystemEventHandler(onCreated);
            m_watcher.Changed += new FileSystemEventHandler(onCreated);
            m_watcher.EnableRaisingEvents = true; //starts monitoring
        }

        public void onCreated(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(100);
            string[] filters = { ".jpg", ".png", ".gif", ".bmp" };
            if (filters.Contains(Path.GetExtension(e.FullPath)))
            {
                string[] args = { e.FullPath};
                bool result;
                string succeeded = m_controller.ExecuteCommand((int)CommandEnum.NewFileCommand, args, out result);
               // OnCommandReceived(this, new CommandReceivedEventArgs())
            }
        }
            
        public void OnCommandReceived(object sender, CommandReceivedEventArgs e)
        {
            if (e.CommandID == (int)CommandEnum.CloseCommand)
            {

            } else
            {
                if (e.RequestDirPath.Equals(direcPath))
                {
                    string[] args = { e.RequestDirPath};
                    bool result;
                    string succeed = m_controller.ExecuteCommand(e.CommandID, args, out result);
                    MessageReceivedEventArgs message = new MessageReceivedEventArgs();
                    message.Message = succeed;
                    if (result)
                    {
                        message.Status = MessageTypeEnum.INFO;
                        m_logging.Log(message, MessageTypeEnum.INFO);
                    } else
                    {
                        message.Status = MessageTypeEnum.FAIL;
                        m_logging.Log(message, MessageTypeEnum.FAIL);
                    }
                }
            }
        }

        public void closeHandler()
        {
            m_watcher.Changed -= onCreated;
            m_watcher.EnableRaisingEvents = false; //stops monitoring
            m_watcher.Dispose();
            DirectoryClose.Invoke(this, new DirectoryCloseEventArgs(direcPath, ""));
        }


    }
}
