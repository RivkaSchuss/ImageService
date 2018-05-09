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
        private ImageServer server;

        public CloseCommand(IImageServiceModel model, ImageServer server)
        {
            this.model = model;
            this.server = server;
        }

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
