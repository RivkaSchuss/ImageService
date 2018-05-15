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
    /// <summary>
    /// main window view model
    /// </summary>
    /// <seealso cref="ImageServiceWPF.VModel.IMainWindowViewModel" />
    class MainWindowViewModel : IMainWindowViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private IMainWindowModel model;
        private ICommand discCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel()
        {
            this.model = new MainWindowModel();
            this.discCommand = new DelegateCommand<object>(this.OnDisconnect, this.CanDisconnect);
            this.model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

        /// <summary>
        /// Determines whether this instance can disconnect the specified argument.
        /// </summary>
        /// <param name="arg">The argument.</param>
        /// <returns>
        ///   <c>true</c> if this instance can disconnect the specified argument; otherwise, <c>false</c>.
        /// </returns>
        private bool CanDisconnect(object arg)
        {
            return true;
        }

        /// <summary>
        /// Called when [disconnect].
        /// </summary>
        /// <param name="obj">The object.</param>
        private void OnDisconnect(object obj)
        {
            this.model.Client.Disconnect();
        }

        /// <summary>
        /// Gets a value indicating whether [vm is connected].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [vm is connected]; otherwise, <c>false</c>.
        /// </value>
        public bool VM_IsConnected
        {
            get
            {
                return model.IsConnected;
            }
        }

        /// <summary>
        /// Gets or sets the disconnect command.
        /// </summary>
        /// <value>
        /// The disconnect command.
        /// </value>
        public ICommand DisconnectCommand { get; set;}

        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="propName">Name of the property.</param>
        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
       

        
    }
}
