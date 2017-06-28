namespace TfsWorkspaceUpdater.Shared.Data
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using Microsoft.TeamFoundation.VersionControl.Client;
    using Views.MainView;

    public class UpdateableWorkingFolder : INotifyPropertyChanged
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Fields

        private readonly IMainView _mainView;
        private readonly Workspace _workspace;
        private readonly WorkingFolder _workingFolder;

        private bool _started;
        private bool _done;

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Properties

        public string LocalPath => _workingFolder.LocalItem;

        public string ServerPath => _workingFolder.ServerItem;

        public bool MayGet => !_workingFolder.IsCloaked;

        public bool Started
        {
            get { return _started; }
            set
            {
                if (value == _started) return;
                _started = value;
                OnPropertyChanged();
            }
        }

        public bool Done
        {
            get { return _done; }
            set
            {
                if (value == _done) return;
                _done = value;
                OnPropertyChanged();
            }
        }

        public int NumConflicts { get; set; }

        public int NumFailures { get; set; }

        public int NumUpdated { get; set; }

        public long NumFiles { get; set; }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors

        public UpdateableWorkingFolder(IMainView mainView, Workspace workspace, WorkingFolder workingFolder)
        {
            _mainView = mainView;
            _workspace = workspace;
            _workingFolder = workingFolder;
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Base Overrides

        public override string ToString() => $"{_workspace.DisplayName} - {_workingFolder.LocalItem}";

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Methods

        public async Task Get()
        {
            Started = true;
            GetStatus status;
            try
            {
                status = await Task.Run(() => _workspace.Get(new GetRequest(_workingFolder.LocalItem, RecursionType.Full, VersionSpec.Latest), GetOptions.None));
            }
            catch (IOException e)
            {
                _mainView.ShowError($"Error - getting working folder '{_workingFolder.LocalItem}' failed", e);
                NumConflicts = 0;
                NumFailures = 0;
                NumUpdated = 0;
                NumFiles = 0;         
                Done = true;
                return;
            }
            NumConflicts = status.NumConflicts;
            NumFailures = status.NumFailures;
            NumUpdated = status.NumUpdated;
            NumFiles = status.NumFiles;
            Done = true;
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region INotifyPropertyChanged Members & Extension

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}