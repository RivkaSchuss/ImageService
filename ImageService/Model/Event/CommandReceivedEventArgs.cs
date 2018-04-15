using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Model
{
    /// <summary>
    /// the command received event args class, implements eventargs.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class CommandReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the command identifier.
        /// </summary>
        /// <value>
        /// The command identifier.
        /// </value>
        public int CommandID { get; set; } //the command id
        /// <summary>
        /// Gets or sets the arguments.
        /// </summary>
        /// <value>
        /// The arguments.
        /// </value>
        public string[] Args { get; set; }
        /// <summary>
        /// Gets or sets the request dir path.
        /// </summary>
        /// <value>
        /// The request dir path.
        /// </value>
        public string RequestDirPath { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandReceivedEventArgs"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="path">The path.</param>
        public CommandReceivedEventArgs(int id, string[] args, string path) //the request directory
        {
            CommandID = id;
            Args = args;
            RequestDirPath = path;
        }
    }
}
