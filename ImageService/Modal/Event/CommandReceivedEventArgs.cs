using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    public class CommandReceivedEventArgs : EventArgs
    {
        public int CommandID { get; set; } //the command id
        public string[] Args { get; set; }
        public string RequestDirPath { get; set; }

        public CommandReceivedEventArgs(int id, string[] args, string path) //the request directory
        {
            CommandID = id;
            Args = args;
            RequestDirPath = path;
        }
    }
}
