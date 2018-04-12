using ImageService.Logging.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    public class LoggingService : ILoggingService
    {
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;
        public void Log(string message, MessageTypeEnum type)
        {
            MessageReceived.Invoke(this, new MessageReceivedEventArgs(message, type));
        }
    }
}
