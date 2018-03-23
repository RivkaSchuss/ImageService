using ImageService.Logging;
using ImageService.Modal;
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

        //regionProperties
        public event EventHandler<CommandReceivedEventArgs> CommandReceived; //event notfies about a new command being received
        //endregion
    
    }
}
