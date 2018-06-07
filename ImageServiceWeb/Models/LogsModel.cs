using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Web;
using Communication.Client;
using Infrastructure;
using Infrastructure.Enums;
using Infrastructure.Event;
using Infrastructure.Model;
using Newtonsoft.Json;

namespace ImageServiceWeb.Models
{
    /// <summary>
    /// the logs model
    /// </summary>
    public class LogsModel
    {
        private IImageServiceClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogsModel"/> class.
        /// </summary>
        public LogsModel()
        {
            client = ImageServiceClient.Instance;
            client.DataReceived += NotifyChange;
        }

        /// <summary>
        /// Sends the log request.
        /// </summary>
        public void SendLogRequest()
        {
            CommandReceivedEventArgs request = new CommandReceivedEventArgs((int)CommandEnum.LogCommand, null, null);
            this.client.Write(request);
            this.client.Read();
        }

        /// <summary>
        /// invoked when a new command has been read from the service, sets the log entries
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message.</param>
        public void NotifyChange(object sender, CommandMessage message)
        {
            if (message.CommandID.Equals((int)CommandEnum.LogCommand))
            {
                try
                {
                    string listOfEntries = (string)message.CommandArgs["LogEntries"];
                    ObservableCollection<MessageReceivedEventArgs> arr = JsonConvert.DeserializeObject<ObservableCollection<MessageReceivedEventArgs>>(listOfEntries);
                    this.LogEntries = new List<MessageReceivedEventArgs>(arr);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Log Entries")]
        public List<MessageReceivedEventArgs> LogEntries { get; set; }
    }
}