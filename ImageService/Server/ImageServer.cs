using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Model;
using ImageService.Model;
using ImageService.Model.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Server
{
    public class ImageServer
    {
        //regionMembers
        private IImageController m_controller;
        private ILoggingService m_logging;
        //endregion
        
        public event EventHandler<CommandReceivedEventArgs> CommandReceived; //event notifies about a new command being received
        //endregion
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

        public void createHandler(string directory)
        {
            Thread.Sleep(1000);
            DirectoryHandler handler = new DirectoryHandler(m_controller, m_logging);
            CommandReceived += handler.OnCommandReceived;
            handler.DirectoryClose += removeHandler;
            handler.StartHandleDirectory(directory);
        }

        public void sendCommand(CommandReceivedEventArgs e)
        {
            CommandReceived?.Invoke(this, e);
        }

        public void onCloseServer()
        {
            sendCommand(new CommandReceivedEventArgs((int)CommandEnum.CloseCommand, null, null));
        }

        public void removeHandler(object sender, DirectoryCloseEventArgs e)
        {
            DirectoryHandler handler = (DirectoryHandler) sender;
            CommandReceived -= handler.OnCommandReceived;
            handler.DirectoryClose -= removeHandler;
            m_logging.Log("The " + e.Message + " directory has been closed.", MessageTypeEnum.INFO);
        }

    }
}