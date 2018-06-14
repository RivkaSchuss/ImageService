using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ImageService.Logging;
using Infrastructure.Model;

namespace ImageService.Server
{
    class AndroidConnection
    {
        private int port;
        private ILoggingService m_logging;
        private Boolean isStopped;
        private TcpListener tcpListener;
        private string handlerPath;

        public AndroidConnection(ILoggingService m_logging, int port)
        {
            this.m_logging = m_logging;
            this.port = port;
            this.isStopped = false;
            this.Start();
            string[] handlers = ConfigurationManager.AppSettings["Handler"].Split(';');
            this.handlerPath = handlers[0];


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
                        try
                        {
                            NetworkStream stream = client.GetStream();
                            BinaryReader reader = new BinaryReader(stream);
                            BinaryWriter writer = new BinaryWriter(stream);
                            byte[] nameArrayBytes = new byte[4096];
                            byte[] imageArrayBytes = new byte[1000000];

                            while (true)
                            {
                                try
                                {
                                    int bytesRead = stream.Read(nameArrayBytes, 0, nameArrayBytes.Length);
                                    string picName = Encoding.ASCII.GetString(nameArrayBytes, 0, bytesRead);

                                    if (picName == "End\n") { break; }

                                    bytesRead = stream.Read(imageArrayBytes, 0, imageArrayBytes.Length);
                                    File.WriteAllBytes(handlerPath + "\\" + picName, imageArrayBytes);
                                    //byteArrayToImage(imageArrayBytes, picName);
                                }
                                catch (Exception e)
                                {
                                    this.m_logging.Log(e.Message, MessageTypeEnum.FAIL);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            this.m_logging.Log(e.Message, MessageTypeEnum.FAIL);
                        }
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
    }
}
