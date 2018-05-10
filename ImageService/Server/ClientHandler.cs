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
using System.Threading;
using ImageService.Logging;
using Infrastructure.Model;

namespace ImageService.Server
{
    public class ClientHandler : IClientHandler
    {
        private CancellationTokenSource tokenSource;
        private ILoggingService m_logger;

        public ClientHandler(ILoggingService m_logger)
        {
            this.tokenSource = new CancellationTokenSource();
            this.m_logger = m_logger;
        }

        public void HandleClient(TcpClient client, IImageController controller, int index)
        {

            new Task(() =>
            {
                try
                {
                    while (true)
                    {
                        NetworkStream stream = client.GetStream();
                        StreamReader reader = new StreamReader(stream);
                        StreamWriter writer = new StreamWriter(stream);
                        bool result;
                        string input = reader.ReadLine();
                        while (reader.Peek() > 0)
                        {
                            input += reader.ReadLine();
                        }
                        if (input != null)
                        {
                            CommandReceivedEventArgs commandReceived = JsonConvert.DeserializeObject<CommandReceivedEventArgs>(input);
                            if (commandReceived.CommandID.Equals((int)CommandEnum.CloseGUI))
                            {
                                commandReceived.Args[0] = index.ToString();
                            }
                            string message = controller.ExecuteCommand(commandReceived.CommandID, commandReceived.Args, out result);
                            writer.WriteLine(message);
                            writer.Flush();
                        }
                    }
                }
                catch (Exception e)
                {
                    this.tokenSource.Cancel();
                    m_logger.Log("Server failed due to: " + e.Message, MessageTypeEnum.FAIL);
                }
            },this.tokenSource.Token).Start();
        }

        public void Close()
        {
            this.tokenSource.Cancel();
        }


    }

}
