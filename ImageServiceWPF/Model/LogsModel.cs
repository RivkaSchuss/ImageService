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
    /// <summary>
    /// logs model interface
    /// </summary>
    /// <seealso cref="ImageServiceWPF.Model.ILogsModel" />
    class LogsModel : ILogsModel
    {
        private ObservableCollection<MessageReceivedEventArgs> logEntries;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogsModel"/> class.
        /// </summary>
        public LogsModel()
        {
            this.Connection.DataReceived += OnDataReceived;
            CommandReceivedEventArgs request = new CommandReceivedEventArgs((int)CommandEnum.LogCommand, null, null);
            //this.Connection.Initialize(request);
            this.Connection.Read();
        }

        /// <summary>
        /// Gets or sets the log entries.
        /// </summary>
        /// <value>
        /// The log entries.
        /// </value>
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

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        public IClientConnection Connection
        {
            get
            {
                return ClientConnection.Instance;
            }
        }


        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="propName">Name of the property.</param>
        private void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        /// <summary>
        /// Called when [data received].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message.</param>
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
