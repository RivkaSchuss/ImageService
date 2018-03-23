using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    public interface ILoggingService
    {
        event EventHandler<MessageReceivedEventArgs> MessageReceived;
        void Log(string message, MessageTypeEnum type); //logging the message
    }
}
