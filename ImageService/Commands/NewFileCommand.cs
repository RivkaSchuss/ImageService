using ImageService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    /// <summary>
    /// the new file command class that implements command.
    /// </summary>
    /// <seealso cref="ImageService.Commands.ICommand" />
    public class NewFileCommand : ICommand
    {
        private IImageServiceModel m_model;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewFileCommand"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public NewFileCommand(IImageServiceModel model)
        {
            m_model = model;
        }

        /// <summary>
        /// Executes the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <returns></returns>
        public string Execute(string[] args, out bool result)
        {
            string path = args[0];
            try
            {
                return m_model.AddFile(path, out result);
            } catch (Exception e)
            {
                result = false;
                return e.ToString();
            }
        }
    }
}
