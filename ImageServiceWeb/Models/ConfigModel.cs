using Communication.Client;
using Infrastructure;
using Infrastructure.Enums;
using Infrastructure.Event;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Windows;
using System.Windows.Threading;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

        public void NotifyChange(object sender, CommandMessage message)
        {
            if (message.CommandID.Equals((int)CommandEnum.GetConfigCommand))
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        //Console.WriteLine("I am here I am hereI am hereI am hereI am hereI am hereI am hereI am hereI am hereI am hereI am here");
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

                    }));

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