using Communication.Client;
using Infrastructure;
using Infrastructure.Enums;
using Infrastructure.Event;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace ImageServiceWeb.Models
{
    public class ConfigModel
    {
        private IImageServiceClient client;
        private List<string> handlers;

        public ConfigModel()
        {
            client = ImageServiceClient.Instance;
            handlers = new List<string>();
            this.client.DataReceived += NotifyChange;
            CommandReceivedEventArgs request = new CommandReceivedEventArgs((int)CommandEnum.GetConfigCommand, null, null);
            this.client.Initialize(request);
        }

        public List<string> Handlers
        {
            get
            {
                return this.handlers;
            }
        }

        public void RemoveHandler(string handlerToRemove)
        {
            try
            {
                string[] args = { handlerToRemove };
                CommandReceivedEventArgs eventArgs = new CommandReceivedEventArgs((int)CommandEnum.CloseCommand, args, null);
                client.Write(eventArgs);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void NotifyChange(object sender, CommandMessage message)
        {
            if (message.CommandID.Equals((int)CommandEnum.GetConfigCommand))
            {
                try
                {
                    this.OutputDirectory = (string)message.CommandArgs["OutputDirectory"];
                    this.SourceName = (string)message.CommandArgs["SourceName"];
                    this.LogName = (string)message.CommandArgs["LogName"];
                    this.ThumbnailSize = (int)message.CommandArgs["ThumbnailSize"];
                    JArray arr = (JArray)message.CommandArgs["Handlers"];
                    string[] array = arr.Select(c => (string)c).ToArray();
                    foreach (var item in array)
                    {
                        this.Handlers.Add(item);
                    }
                
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            if (message.CommandID.Equals((int)CommandEnum.CloseCommand))
            {
                try
                {
                    this.Handlers.Remove((string)message.CommandArgs["HandlerRemoved"]);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }



        [Required]
        [Display(Name = "Output Directory")]
        public string OutputDirectory { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Source Name")]
        public string SourceName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Log Name")]
        public string LogName { get; set; }

        [Required]
        [Display(Name = "Thumbnail Size")]
        public int ThumbnailSize { get; set; }
    }
}