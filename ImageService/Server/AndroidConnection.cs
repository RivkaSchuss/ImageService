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
                            while (true)
                            {
                                NetworkStream stream = client.GetStream();
                                BinaryReader reader = new BinaryReader(stream);
                                BinaryWriter writer = new BinaryWriter(stream);
                                byte[] picBytes = new byte[500000];
                                byte[] nameBytes = new byte[50000];
                                try
                                {
                                    int numBytesPic = stream.Read(picBytes, 0, 500000);
                                    //int numBytesName = stream.Read(nameBytes, 0, 50000);
                                   //if (picBytes != null && nameBytes != null)
                                    {
                                        //   string picName = Encoding.UTF8.GetString(nameBytes);
                                        //char[] stRead = reader.ReadString();
                                        //string picName = HttpUtility.UrlDecode(stRead, System.Text.Encoding.UTF8);
                                        //byteArrayToImage(picBytes, picName);
                                    }
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

        public void byteArrayToImage(byte[] byteArray, string picName)
        {
            //Image image = (Bitmap)((new ImageConverter()).ConvertFrom(byteArray));
            using (var ms = new MemoryStream(byteArray))
            {
                Image image = Image.FromStream(ms);
                //string imgName = image.
                File.WriteAllBytes(handlerPath + "\\" + picName , byteArray);
            }
           
        }
    }
}
