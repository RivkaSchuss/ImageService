using ImageServiceWPF.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceWPF.VModel
{
    class MainWindowViewModel : IMainWindowViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private IMainWindowModel model;

        public MainWindowViewModel()
        {
            this.model = new MainWindowModel();
            this.model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

        public bool VM_IsConnected
        {
            get
            {
                return model.IsConnected;
            }
        }

        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
       

        
    }
}
