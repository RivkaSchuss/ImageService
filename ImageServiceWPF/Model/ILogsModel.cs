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
    /// <summary>
    /// log model interface
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    interface ILogsModel : INotifyPropertyChanged
    {
        ObservableCollection<MessageReceivedEventArgs> LogEntries { get; set; }
    }
}
