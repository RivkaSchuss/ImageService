using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageServiceWPF.Model;

namespace ImageServiceWPF.VModel
{
    class LogsViewModel : ILogsViewModel
    {
        private ILogsModel model;
        public event PropertyChangedEventHandler PropertyChanged;

        public LogsViewModel()
        {
            model = new LogsModel();
        }

        
    }
}
