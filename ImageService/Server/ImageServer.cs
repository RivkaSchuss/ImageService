using ImageService.Controller;
using ImageService.Controller.Handlers;
using Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Model;
using ImageService.Model;
using Infrastructure.Event;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace ImageService.Server
{
    /// <summary>
    /// the image server.
    /// </summary>
    public class ImageServer
    {
        //regionMembers
        private IImageController m_controller;
        private ILoggingService m_logging;
        private Dictionary<string, IDirectoryHandler> handlers;
        private ObservableCollection<IClientHandler> clients;

        //endregion

        public event EventHandler<CommandReceivedEventArgs> CommandReceived; //event notifies about a new command being received
        public event EventHandler<DirectoryCloseEventArgs> CloseServer;
        //endregion
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageServer"/> class.
        /// </summary>
        /// <param name="logging">The logging.</param>
        /// <param name="outputDir">The output dir.</param>
        /// <param name="thumbnailSize">Size of the thumbnail.</param>
        /// <param name="handler">The handler.</param>
        public ImageServer(ILoggingService logging, IImageController controller, string outputDir, int thumbnailSize, string handler)
        {
            IImageServiceModel serviceModel = new ImageServiceModel(outputDir, thumbnailSize);
            m_controller = new ImageController(serviceModel);
            //m_controller.Server = this;
            handlers = new Dictionary<string, IDirectoryHandler>();
            m_logging = logging;
            string[] directoriesToHandle = handler.Split(';');
            foreach (string path in directoriesToHandle)
            {
                try
                {
                    CreateHandler(path);
                }
                catch (Exception e)
                {
                    this.m_logging.Log("Error creating handler for directory: " + path + "due to " + e.Message, MessageTypeEnum.FAIL);
                }

            }
        }

        public ILoggingService Logging
        {
            get { return this.m_logging; }
        }

        public Dictionary<string, IDirectoryHandler> Handlers
        {
            get { return this.handlers; }
        }

        public ObservableCollection<IClientHandler> Clients
        {
            get { return this.clients; }
            set { this.clients = value; }
        }

        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <param name="directory">The directory.</param>
        public void CreateHandler(string directory)
        {
            Thread.Sleep(1000);
            if (!Directory.Exists(directory))
            {
               m_logging.Log("The file given to handle does not exist.", MessageTypeEnum.FAIL);
               throw new FileNotFoundException();
            }
            DirectoryHandler handler = new DirectoryHandler(m_controller, m_logging);
            handlers[directory] = handler;
            CommandReceived += handler.OnCommandReceived;
            CloseServer += handler.OnCloseHandler;
            handler.DirectoryClose += RemoveHandler;
            handler.StartHandleDirectory(directory);
        }

        /// <summary>
        /// Sends the command.
        /// </summary>
        /// <param name="e">The <see cref="CommandReceivedEventArgs"/> instance containing the event data.</param>
        public void SendCommand(CommandReceivedEventArgs e)
        {
            CommandReceived?.Invoke(this, e);
        }

        /// <summary>
        /// deals with closing the server.
        /// </summary>
        public void OnCloseServer()
        {
            try
            {
                CloseServer?.Invoke(this, null);
                m_logging.Log("Handlers notified.", MessageTypeEnum.INFO);
            }
            catch (Exception e)
            {
                this.m_logging.Log("Failed to notify handler due to: " + e.Message, MessageTypeEnum.FAIL);
            }
        }

        /// <summary>
        /// Removes the handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DirectoryCloseEventArgs"/> instance containing the event data.</param>
        public void RemoveHandler(object sender, DirectoryCloseEventArgs e)
        {
            DirectoryHandler handler = (DirectoryHandler) sender;
            CommandReceived -= handler.OnCommandReceived;
            CloseServer -= handler.OnCloseHandler;
            handler.DirectoryClose -= RemoveHandler;
            m_logging.Log("The " + e.Message + " directory has been closed.", MessageTypeEnum.INFO);
        }

        public void CloseSpecifiedHandler(string handlerToDelete)
        {
            if (handlers.ContainsKey(handlerToDelete))
            {
                IDirectoryHandler handler = handlers[handlerToDelete];
                this.CloseServer -= handler.OnCloseHandler;
                handler.OnCloseHandler(this, null);
            }
        }
    }
}