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
        private string[] filters = { ".jpg", ".png", ".gif", ".bmp" };
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
            if (filters.Contains(Path.GetExtension(e.FullPath).ToLower()))
            {
                string[] args = { e.FullPath };
                CommandReceivedEventArgs commmandArgs = new CommandReceivedEventArgs((int)CommandEnum.NewFileCommand, args, direcPath);
                OnCommandReceived(this, commmandArgs);
            }
        }
            
        public void OnCommandReceived(object sender, CommandReceivedEventArgs e)
        {
            if (e.CommandID == (int)CommandEnum.CloseCommand)
            {
                m_logging.Log("Close command", MessageTypeEnum.INFO);
                closeHandler();
                return;
            } else
            {
                if (e.RequestDirPath.Equals(direcPath))
                {
                    string[] args = { e.RequestDirPath};
                    bool result;
                    string message = m_controller.ExecuteCommand(e.CommandID, e.Args, out result);
                    if (result)
                    {
                        //succeed
                        m_logging.Log("Command of ID: " + e.CommandID + " executed successfully", MessageTypeEnum.INFO);
                    } else
                    {
                        //fail
                        m_logging.Log("Error on executing: " + message, MessageTypeEnum.FAIL);
                    }
                }
            }
        }

        public void closeHandler()
        {
            try
            {
                m_watcher.EnableRaisingEvents = false; //stops monitoring
                m_watcher.Created -= new FileSystemEventHandler(onCreated);
                m_watcher.Changed -= new FileSystemEventHandler(onCreated);
            } catch(Exception e)
            {
                m_logging.Log(e.Message, MessageTypeEnum.FAIL);   
            }
            DirectoryClose?.Invoke(this, new DirectoryCloseEventArgs(direcPath, "Directory " + this.direcPath + " closed"));
        }


    }
}
