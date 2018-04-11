using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Model;
using ImageService.Model.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public ImageServer(ILoggingService logging, string outputDir, int thumbnailSize)
        {
            IImageServiceModel serviceModel = new ImageServiceModel(outputDir, thumbnailSize);
            m_controller = new ImageController(serviceModel);
            m_logging = logging;
        }

        public void createHandler(string directory)
        {
            DirectoryHandler handler = new DirectoryHandler(m_controller, m_logging);
            CommandReceived += handler.OnCommandReceived;
            handler.DirectoryClose += onCloseServer;
            handler.StartHandleDirectory(directory);
        }

        public void sendCommand()
        {
            string[] args = { "*" };
            CommandReceived.Invoke(this, new CommandReceivedEventArgs((int)CommandEnum.CloseCommand,args, ""));
        }

        public void onCloseServer(object sender, DirectoryCloseEventArgs e)
        {
            DirectoryHandler handler = (DirectoryHandler)sender;
            CommandReceived -= handler.OnCommandReceived;
        }
    }
}