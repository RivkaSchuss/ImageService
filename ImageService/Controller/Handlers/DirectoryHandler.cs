using ImageService.Logging;
using ImageService.Modal;
using ImageService.Modal.Event;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller.Handlers
{
    public class DirectoryHandler : IDirectoryHandler
    {
        //region members
        private IImageController m_controller; //the image processing controller
        private ILoggingService m_logging;
        private FileSystemWatcher m_dirWatcher; //the path of the directory
        //end region

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose //the event that notifies that the directory is being closed
        {
            add { }
            remove { }
        }

        public void OnCommandReceived(object sender, CommandReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void StartHandleDirectory(string dirPath)
        {
            throw new NotImplementedException();
        }
    }
}
