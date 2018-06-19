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
        private string outputDir;

        public AndroidConnection(ILoggingService m_logging, int port)
        {
            this.m_logging = m_logging;
            this.port = port;
            this.isStopped = false;
            this.Start();
            string[] handlers = ConfigurationManager.AppSettings["Handler"].Split(';');
            this.handlerPath = handlers[0];
            this.outputDir = ConfigurationManager.AppSettings["OutputDir"];


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
                                try
                                {
                                    NetworkStream stream = client.GetStream();
                                    byte[] bytes = new byte[4096];
                                    
                                    //gets the size of the picture.
                                    int bytesRead = stream.Read(bytes, 0, bytes.Length);
                                    string picSize = Encoding.ASCII.GetString(bytes, 0, bytesRead);
                                    
                                    if (picSize == "End\n") { break; }
                                    bytes = new byte[int.Parse(picSize)];

                                    //gets the name of the picture.
                                    bytesRead = stream.Read(bytes, 0, bytes.Length);
                                    string picName = Encoding.ASCII.GetString(bytes, 0, bytesRead);

                                    //gets the image.
                                    int bytesReadFirst = stream.Read(bytes, 0, bytes.Length);
                                    int tempBytes = bytesReadFirst;
                                    byte[] bytesCurrent;
                                    while(tempBytes < bytes.Length)
                                    {
                                        bytesCurrent = new byte[int.Parse(picSize)];
                                        bytesRead = stream.Read(bytesCurrent, 0, bytesCurrent.Length);
                                        transferBytes(bytes, bytesCurrent, tempBytes);
                                        tempBytes += bytesRead;
                                    }

                                    //converts to an image file.
                                    byteArrayToImage(bytes, picName);
                                
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

        public void byteArrayToImage(byte[] byteArray, string picName)
        {
            DirectoryInfo outputD = new DirectoryInfo(outputDir);
            foreach (DirectoryInfo year in outputD.EnumerateDirectories())
            {
                foreach (DirectoryInfo month in year.EnumerateDirectories())
                {
                    foreach (FileInfo file in month.EnumerateFiles())
                    {
                        if (file.Name.Equals(picName))
                        {
                            try
                            {
                                File.Delete(file.FullName);
                                String thumbnailsPath = outputDir + "\\" + "Thumbnails" + "\\"
                                    + year.Name + "\\" + month.Name + "\\" + picName;
                                File.Delete(thumbnailsPath);
                            }
                            catch (Exception e)
                            {
                                m_logging.Log(e.Message, MessageTypeEnum.FAIL);
                            }
                            break;
                        }
                    }
                }
            }
            File.WriteAllBytes(handlerPath + "\\" + picName, byteArray);
        }
       
            
           
        

        public void transferBytes(byte[] origin, byte[] toCopy, int start)
        {
            for (int i = start; i < origin.Length; i++)
            {
                origin[i] = toCopy[i - start];
            }  
        }    
        
    }
}
