namespace TfsWorkspaceUpdater.Core.Views.MainView
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Shared;
    using Shared.Views.ConfigurationView;
    using Shared.Views.MainView;

    public class MainPresenter : BasePresenter<IMainView, IMainViewModel, IMainModel>,
                                 IMainPresenter
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Fields
            
        private readonly IApplication _application;
        private readonly Func<IConfigurationPresenter> _configurationPresenterResolver;
        
        private bool _getting;

        #endregion
        
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors

        public MainPresenter(IMainView view,
                             IMainViewModel viewModel,
                             IMainModel model,
                             IApplication application,
                             Func<IConfigurationPresenter> configurationPresenterResolver)
            : base(view, viewModel, model)
        {
            _application = application;
            _configurationPresenterResolver = configurationPresenterResolver;

            ConnectEventHandler();
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Private Methods

        private void ConnectEventHandler()
        {
            View.Displayed += View_Displayed;
            View.CloseExecuted += View_CloseExecuted;
            View.OpenConfigurationExecuted += View_OpenConfigurationExecuted;
            View.OpenConfigurationCanExecute += View_OpenConfigurationCanExecute;
            View.StartExecuted += View_StartExecuted;

            Model.SettingsChanged += Model_SettingsChanged;
        }

        private async Task LoadWorkingFolders()
        {
            ViewModel.IsLoading = true;
            var allWorkingFolders = await Task.Run(() => Model.LoadAllWorkingFolders());
            ViewModel.WorkingFolders.Clear();
            foreach (var workingFolder in allWorkingFolders)
                ViewModel.WorkingFolders.Add(workingFolder);
            ViewModel.IsLoading = false;
        }

        private async Task GetAll()
        {
            _getting = true;
            CommandManager.InvalidateRequerySuggested();
            foreach (var workingFolder in ViewModel.WorkingFolders.Where(m => m.MayGet))
            {
                await workingFolder.Get();
            }
            _getting = false;
            CommandManager.InvalidateRequerySuggested();
        }

        private void CloseIfRequested()
        {
            if (!ViewModel.UseAutoClose) return;

            var anyErrors = ViewModel.WorkingFolders.Any(m => m.NumConflicts > 0 || m.NumFailures > 0);
            if (anyErrors)
            {
                if (ViewModel.UseForceClose)
                    _application.Close();
            }
            else if (ViewModel.WorkingFolders.Any())
                _application.Close();
        }

        private void OpenConfigurationView()
        {
            var configurationPresenter = _configurationPresenterResolver();
            configurationPresenter.DisplayConfigurationView();
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Event Handler

        private async void View_Displayed(object sender, EventArgs e)
        {
            await LoadWorkingFolders();

            if (!ViewModel.WorkingFolders.Any())
            {
                OpenConfigurationView();
            }
            
            if (ViewModel.UseAutoStart)
            {
                await GetAll();
            }
            else if (ViewModel.WorkingFolders.Any())
            {
                ViewModel.StartAvailable = true;
            }

            CloseIfRequested();
        }

        private void View_CloseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            _application.Close();
        }

        private void View_OpenConfigurationExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            OpenConfigurationView();
        }

        private void View_OpenConfigurationCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !_getting;
        }

        private async void View_StartExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.StartAvailable = false;
            await GetAll();
        }

        private async void Model_SettingsChanged(object sender, EventArgs e)
        {
            await LoadWorkingFolders();
            ViewModel.StartAvailable = ViewModel.WorkingFolders.Any();
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IMainViewModel.UseAutoStart):
                    Model.UseAutoStart = ViewModel.UseAutoStart;
                    break;
                case nameof(IMainViewModel.UseAutoClose):
                    Model.UseAutoClose = ViewModel.UseAutoClose;
                    break;
                case nameof(IMainViewModel.UseForceClose):
                    Model.UseForceClose = ViewModel.UseForceClose;
                    break;
            }
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region IMainPresenter Members

        void IMainPresenter.InitializeViewModel()
        {
            ViewModel.UseAutoStart = Model.UseAutoStart;
            ViewModel.UseAutoClose = Model.UseAutoClose;
            ViewModel.UseForceClose = Model.UseForceClose;

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        void IMainPresenter.DisplayMainView()
        {
            View.Display();
        }

        #endregion
    }
}