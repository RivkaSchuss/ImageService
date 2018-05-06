using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ImageService.Infrastructure
{
    class CommandMessage
    {
        public int CommandID { get; set; }

        public JObject CommandArgs { get; set; }

        public string ToJSON()
        {
            JObject cmdObj = new JObject();
            cmdObj["CommandID"] = CommandID;
            JObject args = new JObject(CommandArgs);
            cmdObj["CommandArgs"] = args;
            return cmdObj.ToString();
        }

        public static CommandMessage ParseJSON(string str)
        {
            CommandMessage msg = new CommandMessage();
            JObject cmdObj = JObject.Parse(str);
            msg.CommandID = (int)cmdObj["CommandID"];
            JObject arr = (JObject)cmdObj["CommandArgs"];
            msg.CommandArgs = arr;
            return msg;
        }
    }
}
