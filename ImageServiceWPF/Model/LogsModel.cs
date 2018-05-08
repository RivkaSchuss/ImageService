using Infrastructure;
using ImageServiceWPF.Client;
using Infrastructure;
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

        private ObservableCollection<string> logMessages;
        public event PropertyChangedEventHandler PropertyChanged;

        public LogsModel()
        {
            this.logMessages = new ObservableCollection<string>();
            this.Connection.DataReceived += OnDataReceived;
        }

        public IClientConnection Connection
        {
            get
            {
                return ClientConnection.Instance;
            }
        }
        public ObservableCollection<string> LogMessages
        {
            get { return this.logMessages; }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void OnDataReceived(object sender, CommandMessage message)
        {

        }

      
    

    }
}
