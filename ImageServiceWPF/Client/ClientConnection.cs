﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.Enums;
using Infrastructure.Event;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ImageServiceWPF.Client
{
    public class ClientConnection : IClientConnection
    {
        public event EventHandler<CommandMessage> DataReceived;
        private static ClientConnection clientInstance;
        private TcpClient client;
        private IPEndPoint ep;
        private static Mutex m_mutex = new Mutex();

        NetworkStream stream;
        private bool isConnected;

        private ClientConnection()
        {
            this.isConnected = this.Connect();
            CommandReceivedEventArgs request = new CommandReceivedEventArgs((int)CommandEnum.GetConfigCommand, null, null);
            //this.Initialize(request);
            //this.Read();
        }

        public void Initialize(CommandReceivedEventArgs request)
        {
            try
            {
                {
                    this.Write(request);
                    stream = client.GetStream();
                    BinaryReader reader = new BinaryReader(stream);
                    //m_mutex.WaitOne();
                    string jSonString = reader.ReadString();
                    //m_mutex.ReleaseMutex();
                    CommandMessage msg = CommandMessage.ParseJSON(jSonString);
                    this.DataReceived?.Invoke(this, msg);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static ClientConnection Instance
        {
            //singleton implementation
            get
            {
                if (clientInstance == null)
                {
                    clientInstance = new ClientConnection();
                    //clientInstance.IsConnected = Instance.Channel
                }
                return clientInstance;
            }
        }

        public bool IsConnected
        {
            get
            {
                return this.isConnected;
            }
            set
            {
                this.isConnected = value;
            }
        }

        public bool Connect()
        {
            try
            {
                bool result = true;
                ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
                client = new TcpClient();
                client.Connect(ep);
                isConnected = true;
                return result;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public void Disconnect()
        {
            try
            {
                CommandReceivedEventArgs eventArgs = new CommandReceivedEventArgs((int)CommandEnum.CloseGUI, null, null);
                this.Write(eventArgs);
                client.Close();
                isConnected = false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        public void Read()
        {
            Task task = new Task(() =>
            {
                while (this.IsConnected)
                {
                    try
                    {
                        {
                            stream = client.GetStream();
                            BinaryReader reader = new BinaryReader(stream);
                            string jSonString = reader.ReadString();
                            CommandMessage msg = CommandMessage.ParseJSON(jSonString);
                            this.DataReceived?.Invoke(this, msg);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            });
            task.Start();
        }



        public void Write(CommandReceivedEventArgs e)
        {
            Task task = new Task(() =>
            {
                try
                {
                    stream = client.GetStream();
                    BinaryWriter writer = new BinaryWriter(stream);
                    string toSend = JsonConvert.SerializeObject(e);
                    m_mutex.WaitOne();
                    writer.Write(toSend);
                    m_mutex.ReleaseMutex();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            });
            task.Start();
        }
    }
}
