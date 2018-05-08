using ImageService.Model;
using Infrastructure.Event;
using System;
using System.Collections.Generic;
using System.IO;
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
        FileSystemWatcher Watcher { get; }
        void StartHandleDirectory(string dirPath); //the function receives the directory to handle
        event EventHandler<DirectoryCloseEventArgs> DirectoryClose;
        void InvokeCloseEvent();

        /// <summary>
        /// Called when [command received].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="CommandReceivedEventArgs"/> instance containing the event data.</param>
        void OnCommandReceived(object sender, CommandReceivedEventArgs e); //event that will be activated upon new command
        void OnCreated(object sender, FileSystemEventArgs e);
    }
}
