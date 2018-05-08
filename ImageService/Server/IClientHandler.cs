using ImageService.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Tcp
{
    interface IClientHandler
    {
        void HandleClient(TcpClient client, IImageController controller);
    }
}
