using ImageServiceWPF.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceWPF.Model
{
    /// <summary>
    /// main window model class
    /// </summary>
    /// <seealso cref="ImageServiceWPF.Model.IMainWindowModel" />
    public class MainWindowModel : IMainWindowModel
    {
        private bool isConnected;
        public event PropertyChangedEventHandler PropertyChanged;
        private IClientConnection client;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowModel"/> class.
        /// </summary>
        public MainWindowModel()
        {
            client = ClientConnection.Instance;
            IsConnected = client.IsConnected;
        }

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        public IClientConnection Client
        {
            get
            {
                return this.client;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is connected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is connected; otherwise, <c>false</c>.
        /// </value>
        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                isConnected = value;
                this.NotifyPropertyChanged("IsConnected");
            }
        }

        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="propName">Name of the property.</param>
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
