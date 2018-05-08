using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceWPF.VModel
{
    interface ILogsViewModel : INotifyPropertyChanged
    {
        ObservableCollection<string> VM_LogEntries { get; }
    }
}
