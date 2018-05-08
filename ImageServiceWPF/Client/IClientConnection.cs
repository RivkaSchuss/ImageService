﻿using Infrastructure;
using Infrastructure.Event;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceWPF.Client
{
    interface IClientConnection
    {
        bool Connect();
        void Disconnect();
        void Write(CommandReceivedEventArgs e);
        void Read();
        event EventHandler<CommandMessage> DataReceived;
        bool IsConnected { get; set; }
    }
}
