using ImageServiceWPF.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceWPF.Model
{
    class MainWindowModel : IMainWindowModel
    {
        private bool isConnected;
        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowModel()
        {
            IClientConnection client = ClientConnection.Instance;
            IsConnected = client.IsConnected;
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
