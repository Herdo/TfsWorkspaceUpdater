namespace TfsWorkspaceUpdater.Views.MainView
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Annotations;
    using Shared.Data;
    using Shared.Views.MainView;
    public class MainViewModel : IMainViewModel
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Fields

        private bool _isLoading;
        private bool _startAvailable;
        private bool _useAutoStart;
        private bool _useAutoClose;
        private bool _useForceClose;

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors

        public MainViewModel()
        {
            WorkingFolders = new ObservableCollection<UpdateableWorkingFolder>();
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region IMainViewModel Members

        public ObservableCollection<UpdateableWorkingFolder> WorkingFolders { get; }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (value == _isLoading) return;
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public bool StartAvailable
        {
            get { return _startAvailable; }
            set
            {
                if (value == _startAvailable) return;
                _startAvailable = value;
                OnPropertyChanged();
            }
        }

        public bool UseAutoStart
        {
            get { return _useAutoStart; }
            set
            {
                if (value == _useAutoStart) return;
                _useAutoStart = value;
                OnPropertyChanged();
            }
        }

        public bool UseAutoClose
        {
            get { return _useAutoClose; }
            set
            {
                if (value == _useAutoClose) return;
                _useAutoClose = value;
                OnPropertyChanged();
            }
        }

        public bool UseForceClose
        {
            get { return _useForceClose; }
            set
            {
                if (value == _useForceClose) return;
                _useForceClose = value;
                OnPropertyChanged();
            }
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
    }
}