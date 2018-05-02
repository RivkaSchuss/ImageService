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
        public string Execute(string[] args, out bool result)
        {
            try
            {
                if (args == null || args.Length == 0)
                {
                    throw new Exception("invalid args");
                }
                string handlerToDelete = args[0];
                string[] directories = (ConfigurationManager.AppSettings.Get("Handler").Split(';'));
                result = true;
                return "";

            } catch (Exception e)
            {
                result = false;
                return e.ToString();
            }
        }
    }
}
