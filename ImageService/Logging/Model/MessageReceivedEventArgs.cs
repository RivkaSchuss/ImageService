using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging.Model
{
    /// <summary>
    /// the message receive event argument class, implements event args
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class MessageReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageReceivedEventArgs"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="type">The type.</param>
        public MessageReceivedEventArgs(string message, MessageTypeEnum type)
        {
            Status = type;
            Message = message;
        }
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public MessageTypeEnum Status { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }
    }
}
