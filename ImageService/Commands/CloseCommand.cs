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
    class CloseCommand : ICommand
    {
        private IImageServiceModel model;
        Dictionary<string, IDirectoryHandler> handlers;

        public CloseCommand(IImageServiceModel model, Dictionary<string, IDirectoryHandler> handlers)
        {
            this.model = model;
            this.handlers = handlers;
        }

        public string Execute(string[] args, out bool result)
        {
            try
            {
                if (args == null || args.Length == 0)
                {
                    throw new Exception("invalid args");
                }
                IDirectoryHandler handler = handlers[args[0]];
                string message = model.CloseHandler(handler, out result);
                return message;

            } catch (Exception e)
            {
                result = false;
                return e.ToString();
            }
        }
    }
}
