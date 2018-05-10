using Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private List<MessageReceivedEventArgs> logs;

        public LoggingService()
        {
            logs = new List<MessageReceivedEventArgs>();
        }

        public List<MessageReceivedEventArgs> Logs
        {
            get
            {
                return this.logs;
            }
        }
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
            MessageReceivedEventArgs msg = new MessageReceivedEventArgs(message, type);
            MessageReceived.Invoke(this, msg);
            this.logs.Add(msg);

        }
    }
}
