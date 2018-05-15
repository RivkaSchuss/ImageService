using ImageService.Controller;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Server
{
    /// <summary>
    /// client handler interface
    /// </summary>
    public interface IClientHandler
    {
        void HandleClient(TcpClient client, IImageController controller, ObservableCollection<TcpClient> clients);
        void Close();
        Mutex M_mutex { get; set; }
    }
}
