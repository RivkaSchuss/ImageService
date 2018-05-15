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
using Infrastructure.Model;
using Newtonsoft.Json;
using System.Windows;

namespace ImageServiceWPF.Model
{
    class LogsModel : ILogsModel
    {
        private ObservableCollection<MessageReceivedEventArgs> logEntries;
        public event PropertyChangedEventHandler PropertyChanged;

        public LogsModel()
        {
            this.Connection.DataReceived += OnDataReceived;
            CommandReceivedEventArgs request = new CommandReceivedEventArgs((int)CommandEnum.LogCommand, null, null);
            this.Connection.ReadWrite(request);
            this.Connection.Read();
        }

        public ObservableCollection<MessageReceivedEventArgs> LogEntries
        {
            get
            {
                return this.logEntries;
            }
            set
            {
                this.logEntries = value;
                NotifyPropertyChanged("LogEntries");
            }
        }

        public IClientConnection Connection
        {
            get
            {
                return ClientConnection.Instance;
            }
        }


        private void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    
        public void OnDataReceived(object sender, CommandMessage message)
        {
            if (message.CommandID.Equals((int)CommandEnum.LogCommand))
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        string listOfEntries = (string)message.CommandArgs["LogEntries"];
                        ObservableCollection<MessageReceivedEventArgs> arr = JsonConvert.DeserializeObject<ObservableCollection<MessageReceivedEventArgs>>(listOfEntries);
                        this.LogEntries = arr;
                    }));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            } 
        }

      
    

    }
}
