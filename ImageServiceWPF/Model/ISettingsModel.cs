using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageServiceWPF.Client;

namespace ImageServiceWPF.Model
{
    interface ISettingsModel : INotifyPropertyChanged
    {

        string OutputDirectory { get; set; }
        string SourceName { get; set; }
        string LogName { get; set; }
        int ThumbnailSize { get; set; }
        string SelectedHandler { get; set; }

        ObservableCollection<string> Handlers {get; set;}

        IClientConnection Connection { get; }
    }
}
