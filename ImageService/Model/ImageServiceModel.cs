using ImageService.Controller.Handlers;
using ImageService.Logging;
using ImageService.Server;
using Infrastructure;
using Infrastructure.Enums;
using Infrastructure.Event;
using Infrastructure.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Model
{
    /// <summary>
    /// the image service model class.
    /// </summary>
    /// <seealso cref="ImageService.Model.IImageServiceModel" />
    public class ImageServiceModel : IImageServiceModel
    {
        private string outputDir = null;
        private int thumbnailSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageServiceModel"/> class.
        /// </summary>
        /// <param name="outputDir">The output dir.</param>
        /// <param name="thumbnailSize">Size of the thumbnail.</param>
        public ImageServiceModel(string outputDir, int thumbnailSize)
        {
            this.outputDir = outputDir;
            this.thumbnailSize = thumbnailSize;
        }

        /// <summary>
        /// Adds the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <returns></returns>
        public string AddFile(string path, out bool result)
        {
            
            if (File.Exists(path))
            {
                try
                {
                    //creating the output directory if it doesn't already exist
                    if (!Directory.Exists(outputDir))
                    {
                        DirectoryInfo outputDirectory = Directory.CreateDirectory(outputDir);
                        //setting the output directory to be hidden
                        outputDirectory.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                    }

                    //creating the thumbnails directory in the output directory:
                    //getting the path to the thumbnails path
                    string thumbPath = System.IO.Path.Combine(outputDir.ToString(), "Thumbnails");
                    Directory.CreateDirectory(thumbPath);

                    //creating the year and month directories:
                    //getting the creation time of the image file
                    DateTime dateTime = File.GetCreationTime(path);
                    //getting the path to the year and the month as strings
                    string year = dateTime.Year.ToString();
                    string month = dateTime.Month.ToString();
                    //getting the path to the year and the month directories in the output directory.
                    string yearPath = System.IO.Path.Combine(outputDir.ToString(), year);
                    string monthPath = System.IO.Path.Combine(yearPath, month);
                    Directory.CreateDirectory(monthPath);
                    //getting the path to the year and the month directories in the thumbnails directory.
                    string thumbYear = System.IO.Path.Combine(thumbPath, year);
                    string thumbMonth = System.IO.Path.Combine(thumbYear, month);
                    Directory.CreateDirectory(thumbMonth);

                    //copying the file if it doesn't already exist:
                    //getting the full path for the destination of the image
                    string imageFullPath = System.IO.Path.Combine(monthPath, Path.GetFileName(path));
                    if (!File.Exists(imageFullPath))
                    {
                        File.Move(path, imageFullPath);
                    }

                    //creating a thumbnail for the file if it doesn't already exist:
                    //getting the full path for the destination of the thumbnail of the image
                    string thumbnailFullPath = System.IO.Path.Combine(thumbMonth, Path.GetFileName(path));
                    if (!File.Exists(thumbnailFullPath))
                    {
                        Image image = Image.FromFile(imageFullPath);
                        Image thumbnail = image.GetThumbnailImage(this.thumbnailSize, this.thumbnailSize, () => false, IntPtr.Zero);
                        thumbnail.Save(thumbnailFullPath);
                    }

                    result = true;
                    return "The image: " + imageFullPath + " has been moved";
                } catch(Exception e)
                {
                    result = false;
                    return e.Message;
                }
            } else
            {
                result = false;
                return "File does not exist";
            }
        }

        /// <summary>
        /// Builds the configuration.
        /// </summary>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <returns></returns>
        public string BuildConfig(out bool result)
        {
            try
            {
                CommandMessage msg = new CommandMessage();
                msg.CommandID = (int)CommandEnum.GetConfigCommand;
                JObject jObj = new JObject();
                jObj["OutputDirectory"] = ConfigurationManager.AppSettings["OutputDir"];
                jObj["SourceName"] = ConfigurationManager.AppSettings["SourceName"];
                jObj["LogName"] = ConfigurationManager.AppSettings["LogName"];
                jObj["ThumbnailSize"] = ConfigurationManager.AppSettings["ThumbnailSize"];
                JArray arr = new JArray();
                string[] handlers = ConfigurationManager.AppSettings["Handler"].Split(';');
                arr = JArray.FromObject(handlers);
                jObj["Handlers"] = arr;
                msg.CommandArgs = jObj;
                string config = msg.ToJSON();
                result = true;
                return config;
            }
            catch (Exception e)
            {
                result = false;
                return e.Message;
            }
        }

        /// <summary>
        /// Builds the handler removed message.
        /// </summary>
        /// <param name="handlerRemoved">The handler removed.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <returns></returns>
        public string BuildHandlerRemovedMessage(string handlerRemoved, out bool result)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string[] handlersString = ConfigurationManager.AppSettings.Get("Handler").Split(';');
                foreach (string handlerString in handlersString)
                {
                    if (string.Compare(handlerRemoved, handlerString) != 0)
                    {
                        sb.Append(handlerString);
                        sb.Append(";");
                    }
                }
                string newHandlers = (sb.ToString()).TrimEnd(';');
                Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                //configuration.AppSettings.Settings.Remove("Handler");
                //configuration.AppSettings.Settings.Add("Handler", newHandlers);
                configuration.AppSettings.Settings["Handler"].Value = newHandlers;
                configuration.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                //TO DO: fix erasing from app config

                CommandMessage msg = new CommandMessage();
                msg.CommandID = (int)CommandEnum.CloseCommand;
                JObject jObj = new JObject();
                jObj["HandlerRemoved"] = handlerRemoved;
                msg.CommandArgs = jObj;
                result = true;
                return msg.ToJSON();  
            }
            catch (Exception e)
            {
                result = false;
                return e.Message;
            }
        }

        /// <summary>
        /// Updates the entries.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <returns></returns>
        public string UpdateEntries(ImageServer server, out bool result)
        {
            try
            {
                ILoggingService logger = server.Logging;
                CommandMessage msg = new CommandMessage();
                msg.CommandID = (int)CommandEnum.LogCommand;
                JObject jObj = new JObject();
                ObservableCollection<MessageReceivedEventArgs> logs = logger.Logs;
                var json = JsonConvert.SerializeObject(logs);
                jObj["LogEntries"] = json;
                msg.CommandArgs = jObj;
                result = true;
                return msg.ToJSON();
            }
            catch (Exception e)
            {
                result = false;
                return e.Message;
            }
        }
    }
}
