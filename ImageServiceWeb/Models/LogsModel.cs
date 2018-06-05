using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Communication.Client;
using Infrastructure;
using Infrastructure.Enums;
using Infrastructure.Event;
using Infrastructure.Model;
using Newtonsoft.Json;

namespace ImageServiceWeb.Models
{
    public class LogsModel
    {
        private IImageServiceClient client;

        public LogsModel()
        {
            client = ImageServiceClient.Instance;
            client.DataReceived += NotifyChange;
        }

        public void SendLogRequest()
        {
            CommandReceivedEventArgs request = new CommandReceivedEventArgs((int)CommandEnum.LogCommand, null, null);
            this.client.Initialize(request);
        }

        public void NotifyChange(object sender, CommandMessage message)
        {
            if (message.CommandID.Equals((int)CommandEnum.LogCommand))
            {
                try
                {
                    string listOfEntries = (string)message.CommandArgs["LogEntries"];
                    ObservableCollection<MessageReceivedEventArgs> arr = JsonConvert.DeserializeObject<ObservableCollection<MessageReceivedEventArgs>>(listOfEntries);
                    this.LogEntries = arr;
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
        public ObservableCollection<MessageReceivedEventArgs> LogEntries { get; set; }
    }
}