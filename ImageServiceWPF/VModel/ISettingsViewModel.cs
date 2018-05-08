using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceWPF.VModel
{
    interface ISettingsViewModel
    {
        string VM_OutputDirectory { get; }
        string VM_SourceName { get; }
        string VM_LogName { get; }
        int VM_ThumbnailSize { get; }
    }
}
