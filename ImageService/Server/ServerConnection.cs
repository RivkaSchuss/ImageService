using ImageService.Controller;
using ImageService.Logging;
using Infrastructure.Enums;
using Infrastructure.Event;
using Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    class ServerConnection : IServerConnection
    {
        private int port;
        private TcpListener tcpListener;
        private IImageController m_controller;
        private ILoggingService m_logging;
        private IClientHandler ch;
        private ObservableCollection<TcpClient> clients;
        private bool isStopped;

        public ServerConnection(IImageController m_controller, ILoggingService m_logging, int port)
        {
            this.m_controller = m_controller;
            this.m_logging = m_logging;
            m_logging.NewLogEntry += UpdateLog;
            this.port = port;
            this.ch = new ClientHandler(m_logging);
            this.isStopped = false;
            this.clients = new ObservableCollection<TcpClient>();
        }

        public ObservableCollection<TcpClient> Clients
        {
            get
            {
                return this.clients;
            }
        }

        public void Start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            tcpListener = new TcpListener(ep);
            tcpListener.Start();
            m_logging.Log("Waiting for connections...", MessageTypeEnum.INFO);

            Task task = new Task(() =>
            {
                while (!isStopped)
                {
                    try
                    {
                        TcpClient client = tcpListener.AcceptTcpClient();
                        m_logging.Log("Client Connected", MessageTypeEnum.INFO);
                        Clients.Add(client);
                        ch.HandleClient(client, m_controller, Clients);
                    }
                    catch (SocketException e)
                    {
                        m_logging.Log(e.Message, MessageTypeEnum.FAIL);
                    }
                }
                m_logging.Log("Server Stopped", MessageTypeEnum.INFO);
            });
            task.Start();
        }

        public void UpdateLog(object sender, CommandReceivedEventArgs e)
        {
            //new Task(() =>
            //{
                try
                {
                    bool result;
                    foreach (TcpClient client in Clients)
                    {
                        if (e.CommandID.Equals((int)CommandEnum.LogCommand))
                        {
                            NetworkStream stream = client.GetStream();
                            StreamWriter writer = new StreamWriter(stream);
                            string message = m_controller.ExecuteCommand(e.CommandID, e.Args, out result);
                            writer.WriteLine(message);
                            writer.Flush();
                        }
                    }
                }
                catch (Exception ex)
                {
                    m_logging.Log("Failed to update log due to: " + ex.Message, MessageTypeEnum.FAIL);
                }
            //}).Start();
        }
        

        public void CloseCommunication()
        {
            //tell all clients that the server is closed
            try
            {
                foreach (TcpClient client in Clients)
                {
                    client.Close();
                }
                this.isStopped = true;
                this.tcpListener.Stop();
            }
            catch (Exception e)
            {
                m_logging.Log("Failed to stop the server due to: " + e.Message, MessageTypeEnum.FAIL);
            }
        }
    }
}
