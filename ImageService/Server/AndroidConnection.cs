using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
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

        public AndroidConnection(ILoggingService m_logging, int port, string handlerPath)
        {
            this.m_logging = m_logging;
            this.port = port;
            this.handlerPath = handlerPath;
            this.isStopped = false;
            this.Start();

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
                            while (true)
                            {
                                NetworkStream stream = client.GetStream();
                                StreamReader reader = new StreamReader(stream);
                                BinaryWriter writer = new BinaryWriter(stream);
                                byte[] bytes = new byte[500000];
                                try
                                {
                                    int numBytes = stream.Read(bytes, 0, 500000);
                                    byteArrayToImage(bytes);
                                } catch (Exception e)
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

        public void byteArrayToImage(byte[] byteArray)
        {
            //Image image = (Bitmap)((new ImageConverter()).ConvertFrom(byteArray));
            //using (var ms = new MemoryStream(byteArray))
            //{
              //  Image image = Image.FromStream(ms);
                File.WriteAllBytes(@"C:\Users\USER\Pictures\pics1\pic.jpg" , byteArray);
            //}
           
        }
    }
}
