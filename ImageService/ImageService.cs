using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Configuration;
using ImageService.Server;
using ImageService.Logging;
using ImageService.Model;
using Infrastructure.Enums;
using System.IO;
using ImageService.Controller;
using Infrastructure.Model;

namespace ImageService
{

    public enum ServiceState
    {
        SERVICE_STOPPED = 0x00000001,
        SERVICE_START_PENDING = 0x00000002,
        SERVICE_STOP_PENDING = 0x00000003,
        SERVICE_RUNNING = 0x00000004,
        SERVICE_CONTINUE_PENDING = 0x00000005,
        SERVICE_PAUSE_PENDING = 0x00000006,
        SERVICE_PAUSED = 0x00000007,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceStatus
    {
        public int dwServiceType;
        public ServiceState dwCurrentState;
        public int dwControlsAccepted;
        public int dwWin32ExitCode;
        public int dwServiceSpecificExitCode;
        public int dwCheckPoint;
        public int dwWaitHint;
    };

    /// <summary>
    /// the image service base class.
    /// </summary>
    /// <seealso cref="System.ServiceProcess.ServiceBase" />
    public partial class ImageService : ServiceBase
    {
        private int eventId = 1;
        private ImageServer server;
        private ILoggingService m_logging;
        private string outputDir;
        private string handler;
        private string thumbnailSize;
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageService"/> class.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <exception cref="FileNotFoundException"></exception>
        public ImageService(string[] args)
        {
            handler = ConfigurationManager.AppSettings["Handler"];
            outputDir = ConfigurationManager.AppSettings["OutputDir"];
            string eventSourceName = ConfigurationManager.AppSettings["SourceName"];
            string logName = ConfigurationManager.AppSettings["LogName"];
            thumbnailSize = ConfigurationManager.AppSettings["ThumbnailSize"];
            InitializeComponent();
            
            if (eventSourceName == null)
            {
                eventSourceName = "Source";
            }
            if (logName == null)
            {
                logName = "NewLog";
            }
            if (args.Count() > 0)
            {
                eventSourceName = args[0];
            }
            if (args.Count() > 1)
            {
                logName = args[1];
            }
            eventLog1 = new EventLog();
            if (!EventLog.SourceExists(eventSourceName))
            {
                EventLog.CreateEventSource(eventSourceName, logName);
            }
            eventLog1.Source = eventSourceName;
            eventLog1.Log = logName;
            //craeting the logger
            m_logging = new LoggingService();
            m_logging.MessageReceived += onMsg;
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control Manager (SCM) or
        /// when the operating system starts (for a service that starts automatically). Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.
            try
            {
                Thread.Sleep(10000);
                ServiceStatus serviceStatus = new ServiceStatus();
                serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
                serviceStatus.dwWaitHint = 100000;
                SetServiceStatus(this.ServiceHandle, ref serviceStatus);
                eventLog1.WriteEntry("In OnStart");
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = 60000; // 60 seconds  
                timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
                timer.Start();
                // Update the service state to Running.
                serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
                SetServiceStatus(this.ServiceHandle, ref serviceStatus);
                //creating the server
                IImageServiceModel serviceModel = new ImageServiceModel(outputDir, Int32.Parse(thumbnailSize));
                IImageController m_controller = new ImageController(serviceModel);
                server = new ImageServer(m_logging, m_controller, outputDir, Int32.Parse(thumbnailSize), handler);
                m_controller.Server = server;
                IServerConnection connection = new ServerConnection(m_controller, m_logging, 8000);
                connection.Start();
            }
            catch (Exception e)
            {
                this.m_logging.Log(e.ToString(), MessageTypeEnum.FAIL);
            }
        }

        /// <summary>
        /// Called when [timer].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.Timers.ElapsedEventArgs"/> instance containing the event data.</param>
        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.  
            eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
            // Updating the service state to stop Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            // writing stop to event log
            eventLog1.WriteEntry("In onStop.");

            // close the server
            //this.server.OnCloseServer();

            // Updating the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        /// <summary>
        /// When implemented in a derived class, <see cref="M:System.ServiceProcess.ServiceBase.OnContinue" /> runs when a Continue command is sent to the service by the 
        /// Service Control Manager (SCM). Specifies actions to take when a service resumes normal functioning after being paused.
        /// </summary>
        protected override void OnContinue()
        {
            eventLog1.WriteEntry("In OnContinue.");
        }
        /// <summary>
        /// when a message is received, this function is implemented.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The <see cref="MessageReceivedEventArgs"/> instance containing the event data.</param>
        public void onMsg(object sender, MessageReceivedEventArgs message)
        {
            EventLogEntryType type;
            switch(message.Status)
            {
                case MessageTypeEnum.FAIL: type = EventLogEntryType.Error; break;
                case MessageTypeEnum.INFO: type = EventLogEntryType.Information; break;
                case MessageTypeEnum.WARNING: type = EventLogEntryType.Warning; break;
                default: type = EventLogEntryType.Information; break;
            }
            eventLog1.WriteEntry(message.Message);
        }
    }
}
