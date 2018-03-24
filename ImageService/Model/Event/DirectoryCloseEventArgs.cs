using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Model.Event
{
    public class DirectoryCloseEventArgs : EventArgs
    {
        public string DirectoryPath { get; set; }
        public string Message { get; set; } //the message that goes to the logger

        public DirectoryCloseEventArgs(string dirPath, string message)
        {
            DirectoryPath = dirPath; // setting the Directory name
            Message = message; //storing the string
        }
    }
}
