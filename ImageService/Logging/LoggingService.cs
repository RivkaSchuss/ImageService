using Infrastructure.Enums;
using Infrastructure.Event;
using Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<MessageReceivedEventArgs> logs;
        public event EventHandler<CommandReceivedEventArgs> NewLogEntry;

        public LoggingService()
        {
            logs = new ObservableCollection<MessageReceivedEventArgs>();
        }

        public ObservableCollection<MessageReceivedEventArgs> Logs
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
            string[] args = { message, type.ToString() };
            //NewLogEntry?.Invoke(this, new CommandReceivedEventArgs((int) CommandEnum.LogCommand, args, null));
        }
    }
}
