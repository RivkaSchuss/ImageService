using ImageServiceWPF.Model;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageServiceWPF.VModel
{
    class MainWindowViewModel : IMainWindowViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private IMainWindowModel model;
        private ICommand discCommand;

        public MainWindowViewModel()
        {
            this.model = new MainWindowModel();
            this.discCommand = new DelegateCommand<object>(this.OnDisconnect, this.CanDisconnect);
            this.model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

        private bool CanDisconnect(object arg)
        {
            return true;
        }

        private void OnDisconnect(object obj)
        {
            this.model.Client.Disconnect();
        }

        public bool VM_IsConnected
        {
            get
            {
                return model.IsConnected;
            }
        }

        public ICommand DisconnectCommand { get; set;}

        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
       

        
    }
}
