using Infrastructure.Event;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    interface IServerConnection
    {
        void Start();
        ObservableCollection<TcpClient> Clients { get; }
        void UpdateLog(object sender, CommandReceivedEventArgs e);
    }
}
