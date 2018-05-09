using ImageService.Commands;
using Infrastructure.Enums;
using ImageService.Model;
using ImageService.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    /// <summary>
    /// the image controller class.
    /// </summary>
    /// <seealso cref="ImageService.Controller.IImageController" />
    public class ImageController : IImageController
    {
        private IImageServiceModel m_model; //the model object
        private Dictionary<int, ICommand> commands;
        private ImageServer server;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageController"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public ImageController(IImageServiceModel model)
        {
            m_model = model; //storing the model of the system
            commands = new Dictionary<int, ICommand>()
            {
                {(int)CommandEnum.NewFileCommand, new NewFileCommand(m_model) } ,
                { (int)CommandEnum.GetConfigCommand, new GetConfigCommand(m_model) },
                { (int)CommandEnum.LogCommand, new LogCommand()} 
                //,
                //{ (int) CommandEnum.CloseCommand, new CloseCommand(m_model, Server.Handlers)} ,
                //{ (int) CommandEnum.CloseGUI, new CloseGUI(m_model, Server) }
            };
        }

        public ImageServer Server
        {
            get
            {
                return this.server;
            }
            set
            {
                this.server = value;
                this.commands[((int)CommandEnum.CloseCommand)] = new CloseCommand(this.m_model, this.server);
                this.commands[((int)CommandEnum.CloseGUI)] = new CloseGUI(this.m_model, this.server);
            }

        }
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="commandID">The command identifier.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="resultSuccesful">if set to <c>true</c> [result succesful].</param>
        /// <returns></returns>
        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            ICommand toExecute;
            try
            {
                if (commands.ContainsKey(commandID))
                {
                    toExecute = commands[commandID];
                    return toExecute.Execute(args, out resultSuccesful);
                }
                resultSuccesful = false;
                return "Command not found";
            } catch (Exception e) {
                resultSuccesful = false;
                return e.ToString();
            }
        }
    }
}