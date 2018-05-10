using Infrastructure;
using ImageServiceWPF.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Enums;
using Infrastructure.Event;
using Newtonsoft.Json.Linq;

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
            CommandReceivedEventArgs request = new CommandReceivedEventArgs((int)CommandEnum.LogCommand, null, null);
            this.Connection.Write(request);
            this.Connection.Read();
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
            try
            {
                if (message.CommandID.Equals((int)CommandEnum.LogCommand))
                {
                    JArray arr = (JArray)message.CommandArgs["LogEntries"];
                    string[] array = arr.Select(c => (string)c).ToArray();
                    foreach (var item in array)
                    {
                        this.LogEntries.Add(item);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

      
    

    }
}
