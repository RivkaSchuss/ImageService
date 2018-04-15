using ImageService.Model;
using ImageService.Model.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller.Handlers
{
    /// <summary>
    /// the directory handler interface.
    /// </summary>
    public interface IDirectoryHandler
    {

        /// <summary>
        /// Starts the handle directory.
        /// </summary>
        /// <param name="dirPath">The dir path.</param>
        void StartHandleDirectory(string dirPath); //the function receives the directory to handle
        /// <summary>
        /// Called when [command received].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="CommandReceivedEventArgs"/> instance containing the event data.</param>
        void OnCommandReceived(object sender, CommandReceivedEventArgs e); //event that will be activated upon new command
    }
}
