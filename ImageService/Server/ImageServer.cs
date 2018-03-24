using ImageService.Controller;
using ImageService.Controller.Handlers;
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
        private DirectoryHandler handler;
        //endregion

        //regionProperties
        public event EventHandler<CommandReceivedEventArgs> CommandReceived; //event notfies about a new command being received
        //endregion
        public ImageServer()
        {
            IImageServiceModel serviceModel = new ImageServiceModel();
            m_controller = new ImageController(serviceModel);
        }

        public void createHandler(string directory)
        {
            handler = new DirectoryHandler(directory, m_controller);
            CommandReceived += handler.OnCommandReceived;
            handler.DirectoryClose += onCloseServer;
        }

        public void sendCommand(int id, string[] args, string path)
        {
            CommandReceived("*", new CommandReceivedEventArgs(id, args, path));
        }

        public void onCloseServer(object sender, DirectoryCloseEventArgs e)
        {
            handler = (DirectoryHandler)sender;
            CommandReceived -= handler.OnCommandReceived;
            handler.DirectoryClose -= onCloseServer;
        }


    }
}
