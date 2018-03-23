using ImageService.Commands;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        private IImageServiceModal m_modal; //the modal object
        private Dictionary<int, ICommand> commands; 

        public ImageController(IImageServiceModal modal)
        {
            m_modal = modal; //storing the modal of the system
            commands = new Dictionary<int, ICommand>()
            {
                //NEW_FILE_COMMAND
            };
        }

        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            //code
            resultSuccesful = true;
            return "";
        }

    }
}
