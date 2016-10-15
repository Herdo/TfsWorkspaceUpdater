namespace TfsWorkspaceUpdater.Shared.Data
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using Microsoft.TeamFoundation.VersionControl.Client;

    public class UpdateableWorkingFolder : INotifyPropertyChanged
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Fields

        private readonly Workspace _workspace;
        private readonly WorkingFolder _workingFolder;

        private bool _started;
        private bool _done;

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Properties

        public string LocalPath => _workingFolder.LocalItem;

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

        public UpdateableWorkingFolder(Workspace workspace, WorkingFolder workingFolder)
        {
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
            var status = await Task.Run(() => _workspace.Get(new GetRequest(_workingFolder.LocalItem, RecursionType.Full, VersionSpec.Latest), GetOptions.None));
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