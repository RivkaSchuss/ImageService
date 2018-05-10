using ImageService.Model;
using ImageService.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class LogCommand : ICommand
    {
        private IImageServiceModel model;
        private ImageServer server;

        public LogCommand(IImageServiceModel model, ImageServer server)
        {
            this.model = model;
            this.server = server;
        }
        public string Execute(string[] args, out bool result)
        {
            try
            {
                return model.UpdateEntries(this.server, out result);
            }
            catch (Exception e)
            {
                result = false;
                return e.Message;
            }
        }
    }
}
