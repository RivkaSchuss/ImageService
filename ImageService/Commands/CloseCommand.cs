using ImageService.Controller.Handlers;
using ImageService.Model;
using ImageService.Server;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    /// <summary>
    /// the Close command class to close a selected handler
    /// </summary>
    /// <seealso cref="ImageService.Commands.ICommand" />
    class CloseCommand : ICommand
    {
        private IImageServiceModel model;
        private ImageServer server;

        /// <summary>
        /// Initializes a new instance of the <see cref="CloseCommand"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="server">The server.</param>
        public CloseCommand(IImageServiceModel model, ImageServer server)
        {
            this.model = model;
            this.server = server;
        }

        /// <summary>
        /// Executes the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <returns></returns>
        /// <exception cref="Exception">invalid args</exception>
        public string Execute(string[] args, out bool result)
        {
            try
            {
                if (args == null || args.Length == 0)
                {
                    throw new Exception("invalid args");
                }


                server.CloseSpecifiedHandler(args[0]);
               
                result = true;
                return model.BuildHandlerRemovedMessage(args[0], out result); 
            }
            catch (Exception e)
            {
                result = false;
                return e.ToString();
            }
        }
    }
}
