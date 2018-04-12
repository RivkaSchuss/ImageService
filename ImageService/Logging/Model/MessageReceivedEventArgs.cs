using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging.Model
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public MessageReceivedEventArgs(string message, MessageTypeEnum type)
        {
            Status = type;
            Message = message;
        }
        public MessageTypeEnum Status { get; set; }
        public string Message { get; set; }
    }
}
