using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceWPF.Model
{
    interface IMainWindowModel :INotifyPropertyChanged
    {
        bool IsConnected { get; set; }
    }
}
