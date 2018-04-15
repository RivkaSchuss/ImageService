using ImageService.Logging.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    /// <summary>
    /// the logging service class.
    /// </summary>
    /// <seealso cref="ImageService.Logging.ILoggingService" />
    public class LoggingService : ILoggingService
    {
        /// <summary>
        /// Occurs when [message received].
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;
        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="type">The type.</param>
        public void Log(string message, MessageTypeEnum type)
        {
            MessageReceived.Invoke(this, new MessageReceivedEventArgs(message, type));
        }
    }
}
