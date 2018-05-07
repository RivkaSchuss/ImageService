using ImageService.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    /// <summary>
    /// the image controller interface.
    /// </summary>
    public interface IImageController
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="commandID">The command identifier.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <returns></returns>
        string ExecuteCommand(int commandID, string[] args, out bool result); //executing the command request
        ImageServer Server { get; set; }
    }
}
