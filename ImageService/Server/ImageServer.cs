using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Model;
using ImageService.Model;
using ImageService.Model.Event;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        //endregion
        
        public event EventHandler<CommandReceivedEventArgs> CommandReceived; //event notifies about a new command being received
        //endregion
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageServer"/> class.
        /// </summary>
        /// <param name="logging">The logging.</param>
        /// <param name="outputDir">The output dir.</param>
        /// <param name="thumbnailSize">Size of the thumbnail.</param>
        /// <param name="handler">The handler.</param>
        public ImageServer(ILoggingService logging, string outputDir, int thumbnailSize, string handler)
        {
            IImageServiceModel serviceModel = new ImageServiceModel(outputDir, thumbnailSize);
            m_controller = new ImageController(serviceModel);
            m_logging = logging;
            string[] directoriesToHandle = handler.Split(';');
            foreach(string path in directoriesToHandle)
            {
                createHandler(path);
            }
        }

        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <param name="directory">The directory.</param>
        public void createHandler(string directory)
        {
            Thread.Sleep(1000);
            if (!Directory.Exists(directory))
            {
               m_logging.Log("The file given to handle does not exist.", MessageTypeEnum.FAIL);
               throw new FileNotFoundException();
            }
            DirectoryHandler handler = new DirectoryHandler(m_controller, m_logging);
            CommandReceived += handler.OnCommandReceived;
            handler.DirectoryClose += removeHandler;
            handler.StartHandleDirectory(directory);
        }

        /// <summary>
        /// Sends the command.
        /// </summary>
        /// <param name="e">The <see cref="CommandReceivedEventArgs"/> instance containing the event data.</param>
        public void sendCommand(CommandReceivedEventArgs e)
        {
            CommandReceived?.Invoke(this, e);
        }

        /// <summary>
        /// deals with closing the server.
        /// </summary>
        public void onCloseServer()
        {
            sendCommand(new CommandReceivedEventArgs((int)CommandEnum.CloseCommand, null, null));
        }

        /// <summary>
        /// Removes the handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DirectoryCloseEventArgs"/> instance containing the event data.</param>
        public void removeHandler(object sender, DirectoryCloseEventArgs e)
        {
            DirectoryHandler handler = (DirectoryHandler) sender;
            CommandReceived -= handler.OnCommandReceived;
            handler.DirectoryClose -= removeHandler;
            m_logging.Log("The " + e.Message + " directory has been closed.", MessageTypeEnum.INFO);
        }

    }
}