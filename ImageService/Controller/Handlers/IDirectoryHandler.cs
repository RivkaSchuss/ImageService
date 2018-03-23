using ImageService.Modal;
using ImageService.Modal.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller.Handlers
{
    public interface IDirectoryHandler
    {
        event EventHandler<DirectoryCloseEventArgs> DirectoryClose; //event that notifies that the directory is being closed
        void StartHandleDirectory(string dirPath); //the function receives the directory to handle
        void OnCommandReceived(object sender, CommandReceivedEventArgs e); //event that will be activated upon new command
    }
}
