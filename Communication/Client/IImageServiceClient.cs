using Infrastructure;
using Infrastructure.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Client
{
    public interface IImageServiceClient
    {
        bool Connect();
        void Disconnect();
        void Write(CommandReceivedEventArgs e);
        void Read();
        event EventHandler<CommandMessage> DataReceived;
        bool IsConnected { get; set; }
        void Initialize(CommandReceivedEventArgs request);
    }
}
