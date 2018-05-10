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
    /// the logging service interface.
    /// </summary>
    public interface ILoggingService
    {
        /// <summary>
        /// Occurs when [message received].
        /// </summary>
        event EventHandler<MessageReceivedEventArgs> MessageReceived;
        event EventHandler<CommandReceivedEventArgs> NewLogEntry;
        ObservableCollection<MessageReceivedEventArgs> Logs { get; }
        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="type">The type.</param>
        void Log(string message, MessageTypeEnum type); //logging the message
    }
}
