using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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

        NetworkStream stream;
        private bool isConnected;

        private ClientConnection()
        {
            this.isConnected = this.Connect();
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
                try
                {
                    while (this.isConnected)
                    {
                        stream = client.GetStream();
                        StreamReader reader = new StreamReader(stream);
                        string jSonString = reader.ReadLine();
                        while (reader.Peek() > 0)
                        {
                            jSonString += reader.ReadLine();
                        }
                        CommandMessage msg = CommandMessage.ParseJSON(jSonString);
                        this.DataReceived?.Invoke(this, msg);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            });
            task.Start();
            

        }

        /*
        public void Read()
        {

            Task<CommandMessage> task = new Task<CommandMessage>(() =>
            {
                try
                {
                    stream = client.GetStream();
                    StreamReader reader = new StreamReader(stream);
                    string jSonString = reader.ReadLine();
                    while (reader.Peek() > 0)
                    {
                        jSonString += reader.ReadLine();
                    }
                    CommandMessage msg = CommandMessage.ParseJSON(jSonString);
                    return msg;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            });
            task.Start();
            this.DataReceived?.Invoke(this, task.Result);

        }
        */

        public void Write(CommandReceivedEventArgs e)
        {
            Task task = new Task(() =>
            {
                try
                {
                    stream = client.GetStream();
                    StreamWriter writer = new StreamWriter(stream);
                    string toSend = JsonConvert.SerializeObject(e);
                    writer.WriteLine(toSend);
                    writer.Flush();
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
