using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Model;
using ImageService.Model;
using ImageService.Model.Event;
using ImageService.Tcp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Server
{
    /// <summary>
    /// the image server.
    /// </summary>
    public class ImageServer
    {
        //regionMembers
        private IImageController m_controller;
        private ILoggingService m_logging;
        private int port;
        private TcpListener tcpListener;
        private IClientHandler ch;
        private Dictionary<string, IDirectoryHandler> handlers;

        //endregion

        public event EventHandler<CommandReceivedEventArgs> CommandReceived; //event notifies about a new command being received
        //endregion
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageServer"/> class.
        /// </summary>
        /// <param name="logging">The logging.</param>
        /// <param name="outputDir">The output dir.</param>
        /// <param name="thumbnailSize">Size of the thumbnail.</param>
        /// <param name="handler">The handler.</param>
        public ImageServer(ILoggingService logging, string outputDir, int thumbnailSize, string handler, int port)
        {
            IImageServiceModel serviceModel = new ImageServiceModel(outputDir, thumbnailSize);
            m_controller = new ImageController(serviceModel);
            m_controller.Server = this;
            handlers = new Dictionary<string, IDirectoryHandler>();
            m_logging = logging;
            string[] directoriesToHandle = handler.Split(';');
            foreach(string path in directoriesToHandle)
            {
                CreateHandler(path);
            }
            this.port = port;
            this.ch = new ClientHandler();
            this.Start();
        }

        public Dictionary<string, IDirectoryHandler> Handlers
        {
            get { return this.handlers; }
        }

        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <param name="directory">The directory.</param>
        public void CreateHandler(string directory)
        {
            Thread.Sleep(1000);
            if (!Directory.Exists(directory))
            {
               m_logging.Log("The file given to handle does not exist.", MessageTypeEnum.FAIL);
               throw new FileNotFoundException();
            }
            DirectoryHandler handler = new DirectoryHandler(m_controller, m_logging);
            handlers[directory] = handler;
            CommandReceived += handler.OnCommandReceived;
            handler.DirectoryClose += RemoveHandler;
            handler.StartHandleDirectory(directory);
        }

        /// <summary>
        /// Sends the command.
        /// </summary>
        /// <param name="e">The <see cref="CommandReceivedEventArgs"/> instance containing the event data.</param>
        public void SendCommand(CommandReceivedEventArgs e)
        {
            CommandReceived?.Invoke(this, e);
        }

        /// <summary>
        /// deals with closing the server.
        /// </summary>
        public void OnCloseServer()
        {
            SendCommand(new CommandReceivedEventArgs((int)CommandEnum.CloseCommand, null, null));
        }

        /// <summary>
        /// Removes the handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DirectoryCloseEventArgs"/> instance containing the event data.</param>
        public void RemoveHandler(object sender, DirectoryCloseEventArgs e)
        {
            DirectoryHandler handler = (DirectoryHandler) sender;
            CommandReceived -= handler.OnCommandReceived;
            handler.DirectoryClose -= RemoveHandler;
            m_logging.Log("The " + e.Message + " directory has been closed.", MessageTypeEnum.INFO);
        }

        public void Start()
        {
            Thread.Sleep(100);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            tcpListener = new TcpListener(ep);
            tcpListener.Start();
            Console.WriteLine("Waiting for connections...");

            Task task = new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        TcpClient client = tcpListener.AcceptTcpClient();
                        Console.WriteLine("Got new connection");
                        ch.HandleClient(client, m_controller);
                    }
                    catch (SocketException)
                    {
                        break;
                    }
                }
                Console.WriteLine("Server stopped");
            });
            task.Start();
        }

        public void Stop()
        {
            tcpListener.Stop();
        }

    }
}