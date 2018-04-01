using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Model
{
    public interface IImageServiceModel
    {
        string AddFile(string path, DateTime dateTime, out bool result);
    }
}
