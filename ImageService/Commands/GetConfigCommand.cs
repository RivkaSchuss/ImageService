using ImageService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    /// <summary>
    /// command to send the app config
    /// </summary>
    /// <seealso cref="ImageService.Commands.ICommand" />
    class GetConfigCommand : ICommand
    {
        private IImageServiceModel model;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetConfigCommand"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public GetConfigCommand(IImageServiceModel model)
        {
            this.model = model;
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
                return model.BuildConfig(out result);
            }
            catch (Exception e)
            {
                result = false;
                return e.Message;
            }
        }
    }
}
