using ImageServiceWPF.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceWPF.Model
{
    public class MainWindowModel : IMainWindowModel
    {
        private bool isConnected;
        public event PropertyChangedEventHandler PropertyChanged;
        private IClientConnection client;

        public MainWindowModel()
        {
            client = ClientConnection.Instance;
            IsConnected = client.IsConnected;
        }

        public IClientConnection Client
        {
            get
            {
                return this.client;
            }
        }

        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                isConnected = value;
                this.NotifyPropertyChanged("IsConnected");
            }
        }

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
