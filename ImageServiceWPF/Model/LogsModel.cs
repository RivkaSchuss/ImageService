using Infrastructure;
using ImageServiceWPF.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ImageServiceWPF.Model
{
    class LogsModel : ILogsModel
    {

        private ObservableCollection<string> logEntries;
        public event PropertyChangedEventHandler PropertyChanged;

        public LogsModel()
        {
            this.logEntries = new ObservableCollection<string>();
            this.Connection.DataReceived += OnDataReceived;
        }

        public IClientConnection Connection
        {
            get
            {
                return ClientConnection.Instance;
            }
        }
        public ObservableCollection<string> LogEntries
        {
            get { return this.logEntries; }
            set
            {
                this.logEntries = value;
                NotifyPropertyChanged("LogEntries");
            }
        }

        private void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    
        public void OnDataReceived(object sender, CommandMessage message)
        {

        }

      
    

    }
}
