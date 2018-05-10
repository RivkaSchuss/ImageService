using ImageService.Controller;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    public interface IClientHandler
    {
        void HandleClient(TcpClient client, IImageController controller, ObservableCollection<TcpClient> clients);
        void Close();
    }
}
