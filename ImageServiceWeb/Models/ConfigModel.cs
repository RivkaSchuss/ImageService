using Communication.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class ConfigModel
    {
        public delegate void NotifyAboutChange();
        private IImageServiceClient client;
        private List<string> handlers;

        public ConfigModel()
        {
            client = ImageServiceClient.Instance;
            handlers = new List<string>();
        }

        public List<string> Handlers
        {
            get
            {
                return this.handlers;
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