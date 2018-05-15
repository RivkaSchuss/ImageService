using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageServiceWPF.Model;
using Infrastructure.Model;

namespace ImageServiceWPF.VModel
{
    /// <summary>
    /// logs view model class
    /// </summary>
    /// <seealso cref="ImageServiceWPF.VModel.ILogsViewModel" />
    class LogsViewModel : ILogsViewModel
    {
        private ILogsModel model;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogsViewModel"/> class.
        /// </summary>
        public LogsViewModel()
        {
            model = new LogsModel();
            this.model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                this.NotifyPropertyChanged("VM_" + e.PropertyName);
            };
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
        /// Gets the vm log entries.
        /// </summary>
        /// <value>
        /// The vm log entries.
        /// </value>
        public ObservableCollection<MessageReceivedEventArgs> VM_LogEntries
        {
            get
            {
                return this.model.LogEntries;
            }
        }
    }
}
