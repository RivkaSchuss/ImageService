using Infrastructure;
using Infrastructure.Enums;
using Infrastructure.Event;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Communication.Client
{
    public class ImageServiceClient : IImageServiceClient
    {
        public event EventHandler<CommandMessage> DataReceived;
        private static ImageServiceClient clientInstance;
        private TcpClient client;
        private IPEndPoint ep;
        private static Mutex m_mutex = new Mutex();

        NetworkStream stream;
        private bool isConnected;

        /// <summary>
        /// Prevents a default instance of the <see cref="ClientConnection"/> class from being created.
        /// </summary>
        private ImageServiceClient()
        {
            this.isConnected = this.Connect();
            CommandReceivedEventArgs request = new CommandReceivedEventArgs((int)CommandEnum.GetConfigCommand, null, null);
        }

        /// <summary>
        /// Initializes the specified request.
        /// </summary>
        /// <param name="request">The <see cref="CommandReceivedEventArgs"/> instance containing the event data.</param>
        public void Initialize(CommandReceivedEventArgs request)
        {
            try
            {
                {
                    this.Write(request);
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

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static ImageServiceClient Instance
        {
            //singleton implementation
            get
            {
                if (clientInstance == null)
                {
                    clientInstance = new ImageServiceClient();
                }
                return clientInstance;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is connected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is connected; otherwise, <c>false</c>.
        /// </value>
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

        /// <summary>
        /// Connects this instance.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Disconnects this instance.
        /// </summary>
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


        /// <summary>
        /// Reads this instance.
        /// </summary>
        public void Read()
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



        /// <summary>
        /// Writes the specified e.
        /// </summary>
        /// <param name="e">The <see cref="CommandReceivedEventArgs"/> instance containing the event data.</param>
        public void Write(CommandReceivedEventArgs e)
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
        }
    }
}

