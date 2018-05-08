using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ImageService.Controller;
using ImageService.Controller.Handlers;
using Infrastructure;
using Infrastructure.Enums;
using ImageService.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Infrastructure.Event;

namespace ImageService.Tcp
{
    class ClientHandler : IClientHandler
    {
        public void HandleClient(TcpClient client, IImageController controller)
        {

            new Task(() =>
            {

                NetworkStream stream = client.GetStream();
                StreamReader reader = new StreamReader(stream);
                StreamWriter writer = new StreamWriter(stream);
                bool result;
                {
                    string input = reader.ReadLine();
                    while (reader.Peek() > 0)
                    {
                        input += reader.ReadLine();
                    }
                    if (input != null)
                    {
                        CommandReceivedEventArgs commandReceived = JsonConvert.DeserializeObject<CommandReceivedEventArgs>(input);
                        string message = controller.ExecuteCommand(commandReceived.CommandID, commandReceived.Args, out result);
                        writer.WriteLine(message);
                        writer.Flush();
                    }
                    //log

                }
                client.Close();
            }).Start();
        }


    }

}
