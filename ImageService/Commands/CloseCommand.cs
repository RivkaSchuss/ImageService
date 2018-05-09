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

                //removing the handler from the app config file
                StringBuilder sb = new StringBuilder();
                string[] handlersString = ConfigurationManager.AppSettings.Get("Handler").Split(';');
                foreach (string handlerString in handlersString)
                {
                    if (string.Compare(args[0], handlerString) != 0)
                    {
                        sb.Append(handlerString);
                        sb.Append(";");
                    }
                }
                ConfigurationManager.AppSettings.Set("Handler", sb.ToString());
                result = true;
                return args[0];

            } catch (Exception e)
            {
                result = false;
                return e.ToString();
            }
        }
    }
}
