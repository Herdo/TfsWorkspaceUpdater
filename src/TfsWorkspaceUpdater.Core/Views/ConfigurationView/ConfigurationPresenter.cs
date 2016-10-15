namespace TfsWorkspaceUpdater.Core.Views.ConfigurationView
{
    using System.Linq;
    using System.Windows.Input;
    using Shared.Views.ConfigurationView;

    public class ConfigurationPresenter : BasePresenter<IConfigurationView, IConfigurationViewModel, IConfigurationModel>,
                                          IConfigurationPresenter
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors

        public ConfigurationPresenter(IConfigurationView view, IConfigurationViewModel viewModel, IConfigurationModel model)
            : base(view, viewModel, model)
        {
            ConnectEventHandler();
            InitializeViewModel();
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Private Methods

        private void ConnectEventHandler()
        {
            View.SaveConfigurationExecuted += View_SaveConfigurationExecuted;
        }

        private void InitializeViewModel()
        {
            foreach (var connectionInformation in Model.ConnectionInformations)
                ViewModel.TfsConnectionInformations.Add(connectionInformation);
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Event Handler

        private void View_SaveConfigurationExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Model.ConnectionInformations = ViewModel.TfsConnectionInformations.ToList();
            Model.Save();
            View.Close();
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region IConfigurationPresenter Members

        void IConfigurationPresenter.DisplayConfigurationView()
        {
            View.Display();
        }

        #endregion
    }
}