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
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Server
{
    class ServerConnection : IServerConnection
    {
        private int port;
        private TcpListener tcpListener;
        private IImageController m_controller;
        private ILoggingService m_logging;
        private ObservableCollection<TcpClient> clients;
        private bool isStopped;
        private IClientHandler ch;
        private static Mutex m_mutex = new Mutex();

        public ServerConnection(IImageController m_controller, ILoggingService m_logging, int port)
        {
            this.m_controller = m_controller;
            this.m_logging = m_logging;
            m_logging.NewLogEntry += UpdateLog;
            this.port = port;
            this.ch = new ClientHandler(m_logging);
            this.ch.M_mutex = m_mutex;
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
            //m_logging.Log("Waiting for connections...", MessageTypeEnum.INFO);

            Task task = new Task(() =>
            {
                while (!isStopped)
                {
                    try
                    {
                        TcpClient client = tcpListener.AcceptTcpClient();
                        Clients.Add(client);
                        ch.HandleClient(client, m_controller, Clients);
                        m_logging.Log("Client Connected", MessageTypeEnum.INFO);
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

            try
            {
                bool result;
                foreach (TcpClient client in Clients)
                {
                    new Task(() =>
                    {
                        try
                        {
                            if (e.CommandID.Equals((int)CommandEnum.LogCommand))
                            {
                                NetworkStream stream = client.GetStream();
                                BinaryWriter writer = new BinaryWriter(stream);
                                string message = m_controller.ExecuteCommand(e.CommandID, e.Args, out result);
                                m_mutex.WaitOne();
                                writer.Write(message);
                                m_mutex.ReleaseMutex();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            //this.clients.Remove(client);
                            //m_logging.Log("Failed to update a specific client due to: " + ex.Message, MessageTypeEnum.FAIL);
                        }
                    }).Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //m_logging.Log("Failed to update log due to: " + ex.Message, MessageTypeEnum.FAIL);
            }
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
