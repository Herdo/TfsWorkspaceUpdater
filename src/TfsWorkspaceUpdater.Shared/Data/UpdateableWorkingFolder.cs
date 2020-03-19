using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.TeamFoundation;
using Microsoft.TeamFoundation.VersionControl.Client;
using TfsWorkspaceUpdater.Shared.Views.MainView;

namespace TfsWorkspaceUpdater.Shared.Data
{
    public class UpdateableWorkingFolder : INotifyPropertyChanged
    {
        public const int MaxRetries = 10;
        private const int RetryDelayInMilliseconds = 6_000;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Fields

        private readonly IMainView _mainView;
        private readonly Workspace _workspace;
        private readonly WorkingFolder _workingFolder;

        private bool _started;
        private bool _done;
        private int _retries;

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Properties

        public string LocalPath => _workingFolder.LocalItem;

        public string ServerPath => _workingFolder.ServerItem;

        public bool MayGet => !_workingFolder.IsCloaked;

        public bool Started
        {
            get => _started;
            private set
            {
                if (value == _started) return;
                _started = value;
                OnPropertyChanged();
            }
        }

        public bool Done
        {
            get => _done;
            private set
            {
                if (value == _done) return;
                _done = value;
                OnPropertyChanged();
            }
        }

        public int Retries
        {
            get => _retries;
            private set
            {
                if (value == _retries)  return;
                _retries = value;
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
            Retries = 1;
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Base Overrides

        public override string ToString() => $"{_workspace.DisplayName} - {_workingFolder.LocalItem}";

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Methods

        public async Task GetAsync()
        {
            Started = true;
            GetStatus status;
            do
            {
                if (Retries > 1)
                    await Task.Delay(RetryDelayInMilliseconds);

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
                catch (WebException)
                {
                    if (Retries + 1 > MaxRetries)
                        throw;

                    Retries++;
                    status = null;
                }
                catch (TeamFoundationServiceUnavailableException)
                {
                    if (Retries + 1 > MaxRetries)
                        throw;

                    Retries++;
                    status = null;
                }
            } while (status == null);

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