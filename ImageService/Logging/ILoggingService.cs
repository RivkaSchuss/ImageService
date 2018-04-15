using ImageService.Logging.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    /// <summary>
    /// the logging service interface.
    /// </summary>
    public interface ILoggingService
    {
        /// <summary>
        /// Occurs when [message received].
        /// </summary>
        event EventHandler<MessageReceivedEventArgs> MessageReceived;
        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="type">The type.</param>
        void Log(string message, MessageTypeEnum type); //logging the message
    }
}
