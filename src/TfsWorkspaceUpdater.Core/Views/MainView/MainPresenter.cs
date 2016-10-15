namespace TfsWorkspaceUpdater.Core.Views.MainView
{
    using System;
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
            foreach (var workingFolder in ViewModel.WorkingFolders)
            {
                await workingFolder.Get();
            }
            _getting = false;
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Event Handler

        private async void View_Displayed(object sender, EventArgs e)
        {
            await LoadWorkingFolders();
            await GetAll();
        }

        private void View_CloseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            _application.Close();
        }

        private void View_OpenConfigurationExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var configurationPresenter = _configurationPresenterResolver();
            configurationPresenter.DisplayConfigurationView();
        }

        private void View_OpenConfigurationCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !_getting;
        }

        private async void Model_SettingsChanged(object sender, EventArgs e)
        {
            await LoadWorkingFolders();
            await GetAll();
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region IMainPresenter Members

        void IMainPresenter.DisplayMainView()
        {
            View.Display();
        }

        #endregion
    }
}