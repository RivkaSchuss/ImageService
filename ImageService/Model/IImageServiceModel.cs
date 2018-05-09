using ImageService.Controller.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Model
{
    /// <summary>
    /// the image service model interface.
    /// </summary>
    public interface IImageServiceModel
    {
        /// <summary>
        /// Adds the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <returns></returns>
        string AddFile(string path, out bool result);
        string BuildConfig(out bool result);
        string BuildHandlerRemovedMessage(string handlerRemoved, out bool result);
    }
}
