using ImageService.Commands;
using ImageService.Infrastructure.Enums;
using ImageService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        private IImageServiceModel m_model; //the model object
        private Dictionary<int, ICommand> commands; 

        public ImageController(IImageServiceModel model)
        {
            m_model = model; //storing the model of the system
            commands = new Dictionary<int, ICommand>()
            {
                {(int)CommandEnum.NewFileCommand, new NewFileCommand(m_model) },
                { (int)CommandEnum.CloseCommand, new CloseCommand(m_model) }
                //NEW_FILE_COMMAND
            };
        }

        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            ICommand toExecute;
            try
            {
                if (commands.ContainsKey(commandID))
                {
                    toExecute = commands[commandID];
                    toExecute.Execute(args, out resultSuccesful);
                    resultSuccesful = true;
                    return "";
                }


                resultSuccesful = false;
                return "ID not found";
            } catch (Exception e) {
                resultSuccesful = false;
                return e.ToString();
            }
        }

    }
}
