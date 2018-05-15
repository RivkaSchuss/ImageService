using Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceWPF.VModel
{
    /// <summary>
    /// log view model interface
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    interface ILogsViewModel : INotifyPropertyChanged
    {
        ObservableCollection<MessageReceivedEventArgs> VM_LogEntries { get; }
    }
}
