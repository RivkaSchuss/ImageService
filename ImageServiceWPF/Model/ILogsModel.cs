using Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceWPF.Model
{
    interface ILogsModel : INotifyPropertyChanged
    {
        ObservableCollection<MessageReceivedEventArgs> LogEntries { get; set; }
    }
}
