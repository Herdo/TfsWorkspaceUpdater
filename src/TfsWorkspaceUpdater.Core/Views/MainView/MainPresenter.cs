namespace TfsWorkspaceUpdater.Core.Views.MainView
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Shared;
    using Shared.Data;
    using Shared.Views.ConfigurationView;
    using Shared.Views.MainView;

    public class MainPresenter : BasePresenter<IMainView, IMainViewModel, IMainModel>,
                                 IMainPresenter
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Fields
            
        private readonly IApplication _application;
        private readonly Func<IConfigurationPresenter> _configurationPresenterResolver;

        private CommandLineParams _parameter;
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
            if (!(_parameter?.AutoClose ?? false)) return;

            var anyErrors = ViewModel.WorkingFolders.Any(m => m.NumConflicts > 0 || m.NumFailures > 0);
            if (anyErrors)
            {
                if (_parameter.ForceClose)
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
            
            if (_parameter?.AutoStart ?? false)
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

        private async void Model_SettingsChanged(object sender, EventArgs e)
        {
            await LoadWorkingFolders();
            ViewModel.StartAvailable = ViewModel.WorkingFolders.Any();
        }

        private async void View_StartExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.StartAvailable = false;
            await GetAll();
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region IMainPresenter Members

        void IMainPresenter.Initialize(CommandLineParams parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));

            _parameter = parameter;
        }

        void IMainPresenter.DisplayMainView()
        {
            View.Display();
        }

        #endregion
    }
}