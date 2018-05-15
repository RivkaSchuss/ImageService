using ImageService.Model;
using ImageService.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    /// <summary>
    /// log command class
    /// </summary>
    /// <seealso cref="ImageService.Commands.ICommand" />
    class LogCommand : ICommand
    {
        private IImageServiceModel model;
        private ImageServer server;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogCommand"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="server">The server.</param>
        public LogCommand(IImageServiceModel model, ImageServer server)
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
        public string Execute(string[] args, out bool result)
        {
            try
            {
                return model.UpdateEntries(this.server, out result);
            }
            catch (Exception e)
            {
                result = false;
                return e.Message;
            }
        }
    }
}
