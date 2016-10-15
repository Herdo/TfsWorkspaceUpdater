namespace TfsWorkspaceUpdater.Views.ConfigurationView
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Annotations;
    using Shared.Data;
    using Shared.Views.ConfigurationView;

    public class ConfigurationViewModel : IConfigurationViewModel
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors

        public ConfigurationViewModel()
        {
            TfsConnectionInformations = new ObservableCollection<TfsConnectionInformation>();
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region INotifyPropertyChanged Members & Extension

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region IConfigurationViewModel Members

        public ObservableCollection<TfsConnectionInformation> TfsConnectionInformations { get; }

        #endregion
    }
}