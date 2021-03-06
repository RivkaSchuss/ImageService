﻿using System;
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

            //new Task(() =>
            //{
            try
            {
                while (true)
                {
                    NetworkStream stream = client.GetStream();
                    BinaryReader reader = new BinaryReader(stream);
                    BinaryWriter writer = new BinaryWriter(stream);
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
                        else
                        {
                            if (!this.SendToClient(controller, commandReceived, writer))
                            {
                                clients.Remove(client);
                                client.Close();
                                break;
                            }
                        }


                    }
                }
            }
            catch (Exception e)
            {
                clients.Remove(client);
                client.Close();
            }
            //},this.tokenSource.Token).Start();
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            this.tokenSource.Cancel();
        }


        /// <summary>
        /// Sends to client.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="command">The <see cref="CommandReceivedEventArgs"/> instance containing the event data.</param>
        /// <param name="writer">The writer.</param>
        /// <returns></returns>
        public Boolean SendToClient(IImageController controller, CommandReceivedEventArgs command, BinaryWriter writer)
        {
            try
            {
                bool result;
                string message = controller.ExecuteCommand(command.CommandID, command.Args, out result);
                M_mutex.WaitOne();
                writer.Write(message);
                M_mutex.ReleaseMutex();
                return true;
            }
            catch (Exception e)
            {
                m_logger.Log("Failed to send to client due to: " + e.Message, MessageTypeEnum.FAIL);
                return false;
            }
        }



    }

}
