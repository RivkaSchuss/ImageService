using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    public class NewFileCommand : ICommand
    {
        private IImageServiceModal m_modal;

        public NewFileCommand(IImageServiceModal modal)
        {
            m_modal = modal;
        }

        public string Execute(string[] args, out bool result)
        {
            throw new NotImplementedException();
            //string returns the new path if the result == true, and returns the error message
        }
    }
}
