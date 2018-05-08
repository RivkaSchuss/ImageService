using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Event
{
    /// <summary>
    /// the directory close event args class, implements event args
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class DirectoryCloseEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the directory path.
        /// </summary>
        /// <value>
        /// The directory path.
        /// </value>
        public string DirectoryPath { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; } //the message that goes to the logger

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryCloseEventArgs"/> class.
        /// </summary>
        /// <param name="dirPath">The dir path.</param>
        /// <param name="message">The message.</param>
        public DirectoryCloseEventArgs(string dirPath, string message)
        {
            DirectoryPath = dirPath; // setting the Directory name
            Message = message; //storing the string
        }
    }
}
