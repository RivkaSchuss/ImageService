using Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Model;
using ImageService.Model;
using Infrastructure.Event;
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
    /// <summary>
    /// the directory handler class.
    /// </summary>
    /// <seealso cref="ImageService.Controller.Handlers.IDirectoryHandler" />
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

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryHandler"/> class.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="logging">The logging.</param>
        public DirectoryHandler(IImageController controller, ILoggingService logging)
        {
            m_controller = controller;
            m_logging = logging;
        }

        public FileSystemWatcher Watcher
        {
            get
            {
                return this.m_watcher;
            }
        }

        /// <summary>
        /// Starts the handle directory.
        /// </summary>
        /// <param name="dirPath">The dir path.</param>
        public void StartHandleDirectory(string dirPath)
        {
            direcPath = dirPath;
            m_watcher = new FileSystemWatcher(dirPath);
            //m_watcher.Path = dirPath;
            //Register a handler that gets called when a file is created
            m_watcher.Created += new FileSystemEventHandler(OnCreated);
            //m_watcher.Changed += new FileSystemEventHandler(onCreated);
            m_watcher.EnableRaisingEvents = true; //starts monitoring
        }

        /// <summary>
        /// when a new file is created, this function is implemented.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="FileSystemEventArgs"/> instance containing the event data.</param>
        public void OnCreated(object sender, FileSystemEventArgs e)
        {
            if (filters.Contains(Path.GetExtension(e.FullPath).ToLower()))
            {
                string[] args = { e.FullPath };
                CommandReceivedEventArgs commmandArgs = new CommandReceivedEventArgs((int)CommandEnum.NewFileCommand, args, direcPath);
                OnCommandReceived(this, commmandArgs);
            }
        }

        /// <summary>
        /// Called when [command received].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="CommandReceivedEventArgs"/> instance containing the event data.</param>
        public void OnCommandReceived(object sender, CommandReceivedEventArgs e)
        {

            if (e.RequestDirPath.Equals(direcPath))
            {
                string[] args = { e.RequestDirPath };
                bool result;
                string message = m_controller.ExecuteCommand(e.CommandID, e.Args, out result);
                if (result)
                {
                    //succeed
                    m_logging.Log("Command of ID: " + e.CommandID + " executed successfully", MessageTypeEnum.INFO);
                }
                else
                {
                    //fail
                    m_logging.Log("Error on executing: " + message, MessageTypeEnum.FAIL);
                }
            }
        }
        /// <summary>
        /// Closes the handler.
        /// </summary>
        public void InvokeCloseEvent()
        {
            DirectoryClose?.Invoke(this, new DirectoryCloseEventArgs(direcPath, "Directory " + this.direcPath + " closed"));
        }
    }
}

