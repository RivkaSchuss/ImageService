﻿using ImageService.Logging.Model;
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
        void Log(MessageReceivedEventArgs message, MessageTypeEnum type); //logging the message
    }
}
