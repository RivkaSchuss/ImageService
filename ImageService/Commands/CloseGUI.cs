using ImageService.Model;
using ImageService.Server;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class CloseGUI : ICommand
    {
        private IImageServiceModel model;
        private ImageServer server;

        public CloseGUI(IImageServiceModel model, ImageServer server)
        {
            this.model = model;
            this.server = server;
        }

        public string Execute(string[] args, out bool result)
        {
            try
            {
                //TO DO: remove client from list


                int indexToRemove = Int32.Parse(args[0]);
                //IClientHandler toRemove = server.Clients[indexToRemove];
                //toRemove.Close();
                //server.Clients.Remove(toRemove);
                result = true;
                return "success";
            } catch (Exception e)
            {
                result = false;
                return e.Message;
            }
            
            
        }
    }
}
