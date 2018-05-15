using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Infrastructure
{
    /// <summary>
    /// command message class
    /// </summary>
    public class CommandMessage
    {
        /// <summary>
        /// Gets or sets the command identifier.
        /// </summary>
        /// <value>
        /// The command identifier.
        /// </value>
        public int CommandID { get; set; }

        /// <summary>
        /// Gets or sets the command arguments.
        /// </summary>
        /// <value>
        /// The command arguments.
        /// </value>
        public JObject CommandArgs { get; set; }

        /// <summary>
        /// To the json.
        /// </summary>
        /// <returns></returns>
        public string ToJSON()
        {
            JObject cmdObj = new JObject();
            cmdObj["CommandID"] = CommandID;
            JObject args = new JObject(CommandArgs);
            cmdObj["CommandArgs"] = args;
            return cmdObj.ToString();
        }

        /// <summary>
        /// Parses the json.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
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
