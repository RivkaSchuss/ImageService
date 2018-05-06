using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using Newtonsoft.Json.Linq;

namespace ImageService.Tcp
{
    class ClientHandler : IClientHandler
    {
        public void HandleClient(TcpClient client)
        {
            new Task(() =>
            {
                using (NetworkStream stream = client.GetStream())
                using (BinaryReader reader = new BinaryReader(stream))
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    
                    writer.Write(BuildConfig());

                    
                    string input = reader.ReadString(); 
                    if (input != null)
                    {
                        //close handler
                    }

                }
                client.Close();
            }).Start();
        }

        public string BuildConfig()
        {
            CommandMessage msg = new CommandMessage();
            msg.CommandID = (int)CommandEnum.GetConfigCommand;
            JObject jObj = new JObject();
            jObj["OutputDirectory"] = ConfigurationManager.AppSettings["OutputDirectory"];
            jObj["SourceName"] = ConfigurationManager.AppSettings["SourceName"];
            jObj["LogName"] = ConfigurationManager.AppSettings["LogName"];
            jObj["ThumbnailSize"] = ConfigurationManager.AppSettings["ThumbnailSize"];
            JArray arr = new JArray();
            string[] handlers = ConfigurationManager.AppSettings["Handler"].Split(';');
            arr = JArray.FromObject(handlers);
            jObj["Handlers"] = arr;
            msg.CommandArgs = jObj;
            string config = msg.ToJSON();
            return config;
        }
   
    }

}
