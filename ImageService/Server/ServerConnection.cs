using ImageService.Controller;
using ImageService.Logging;
using ImageService.Logging.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<IClientHandler> clients;
        private bool isStopped;
        private IClientHandler ch;

        public ServerConnection(IImageController m_controller, ILoggingService m_logging, int port)
        {
            this.m_controller = m_controller;
            this.m_logging = m_logging;
            this.port = port;
            this.isStopped = false;
            this.clients = new ObservableCollection<IClientHandler>();
            this.ch = new ClientHandler();
        }

        public ObservableCollection<IClientHandler> Clients
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
                        //IClientHandler ch = new ClientHandler();
                        //Clients.Add(ch);
                        ch.HandleClient(client, m_controller, Clients.IndexOf(ch));
                        //client.Close();
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

        public void CloseCommunication()
        {
            //tell all clients that the server is closed
            this.isStopped = true;
            this.tcpListener.Stop();
        }
    }
}
