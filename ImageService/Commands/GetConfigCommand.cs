using ImageService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class GetConfigCommand : ICommand
    {
        private IImageServiceModel model;

        public GetConfigCommand(IImageServiceModel model)
        {
            this.model = model;
        }

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
