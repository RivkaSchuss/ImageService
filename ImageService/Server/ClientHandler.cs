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
using System.Collections.ObjectModel;

namespace ImageService.Server
{
    /// <summary>
    /// client handler class
    /// </summary>
    /// <seealso cref="ImageService.Server.IClientHandler" />
    public class ClientHandler : IClientHandler
    {
        private CancellationTokenSource tokenSource;
        private ILoggingService m_logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientHandler"/> class.
        /// </summary>
        /// <param name="m_logger">The m logger.</param>
        public ClientHandler(ILoggingService m_logger)
        {
            this.tokenSource = new CancellationTokenSource();
            this.m_logger = m_logger;
        }

        public Mutex M_mutex { get; set; }

        /// <summary>
        /// Handles the client.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="clients">The clients.</param>
        public void HandleClient(TcpClient client, IImageController controller, ObservableCollection<TcpClient> clients)
        {

            new Task(() =>
            {
                try
                {
                    while (true)
                    {
                        NetworkStream stream = client.GetStream();
                        BinaryReader reader = new BinaryReader(stream);
                        BinaryWriter writer = new BinaryWriter(stream);
                        bool result;
                        string input = reader.ReadString();
                        if (input != null)
                        {
                            CommandReceivedEventArgs commandReceived = JsonConvert.DeserializeObject<CommandReceivedEventArgs>(input);
                            if (commandReceived.CommandID.Equals((int)CommandEnum.CloseGUI))
                            {
                                clients.Remove(client);
                                client.Close();
                                break;
                            }
                            string message = controller.ExecuteCommand(commandReceived.CommandID, commandReceived.Args, out result);
                            M_mutex.WaitOne();
                            writer.Write(message);
                            M_mutex.ReleaseMutex();
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

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            this.tokenSource.Cancel();
        }


    }

}
