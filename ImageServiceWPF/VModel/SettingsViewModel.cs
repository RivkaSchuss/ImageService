using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Infrastructure;
using Infrastructure.Enums;
using ImageServiceWPF.Model;
using Infrastructure.Event;
using Newtonsoft.Json;
using Prism.Commands;

namespace ImageServiceWPF.VModel
{
    /// <summary>
    /// settings view model
    /// </summary>
    /// <seealso cref="ImageServiceWPF.VModel.ISettingsViewModel" />
    class SettingsViewModel : ISettingsViewModel
    {
        private ISettingsModel model;
        
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
        /// </summary>
        public SettingsViewModel()
        {
            this.model = new SettingsModel();
            this.model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                this.NotifyPropertyChanged("VM_" + e.PropertyName);
            };
            this.RemoveCommand = new DelegateCommand<object>(this.OnRemove, this.CanRemove);
        }

        /// <summary>
        /// Called when [remove].
        /// </summary>
        /// <param name="obj">The object.</param>
        private void OnRemove(object obj)
        {            
            string[] args = { this.model.SelectedHandler };
            CommandReceivedEventArgs eventArgs = new CommandReceivedEventArgs((int) CommandEnum.CloseCommand, args, null);

            this.model.Connection.Write(eventArgs);
        }

        /// <summary>
        /// Determines whether this instance can remove the specified argument.
        /// </summary>
        /// <param name="arg">The argument.</param>
        /// <returns>
        ///   <c>true</c> if this instance can remove the specified argument; otherwise, <c>false</c>.
        /// </returns>
        private bool CanRemove(object arg)
        {
            if (string.IsNullOrEmpty(this.model.SelectedHandler))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets the remove command.
        /// </summary>
        /// <value>
        /// The remove command.
        /// </value>
        public ICommand RemoveCommand
        {
            get; private set;
        }

        /// <summary>
        /// Gets the vm output directory.
        /// </summary>
        /// <value>
        /// The vm output directory.
        /// </value>
        public string VM_OutputDirectory
        {
            get { return this.model.OutputDirectory; }
        }

        /// <summary>
        /// Gets the name of the vm source.
        /// </summary>
        /// <value>
        /// The name of the vm source.
        /// </value>
        public string VM_SourceName
        {
            get { return this.model.SourceName; }
        }

        /// <summary>
        /// Gets the name of the vm log.
        /// </summary>
        /// <value>
        /// The name of the vm log.
        /// </value>
        public string VM_LogName
        {
            get { return this.model.LogName; }
        }

        /// <summary>
        /// Gets the size of the vm thumbnail.
        /// </summary>
        /// <value>
        /// The size of the vm thumbnail.
        /// </value>
        public int VM_ThumbnailSize
        {
            get { return this.model.ThumbnailSize; }
        }

        /// <summary>
        /// Gets or sets the vm selected handler.
        /// </summary>
        /// <value>
        /// The vm selected handler.
        /// </value>
        public string VM_SelectedHandler
        {
            get { return this.model.SelectedHandler; }
            set
            {
                this.model.SelectedHandler = value;
                var command = this.RemoveCommand as DelegateCommand<object>;
                command.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="propName">Name of the property.</param>
        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        /// <summary>
        /// Gets or sets the vm handlers.
        /// </summary>
        /// <value>
        /// The vm handlers.
        /// </value>
        public ObservableCollection<string> VM_Handlers
        {
            get { return this.model.Handlers; }
            set { this.model.Handlers = value; }
        }
    }
}
