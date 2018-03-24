using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Model;

namespace ImageService.Commands
{
    class CloseCommand : ICommand
    {
        private IImageServiceModel m_model;

        public CloseCommand(IImageServiceModel model)
        {
            m_model = model;
        }
        public string Execute(string[] args, out bool result)
        {
            throw new NotImplementedException();
        }
    }
}
