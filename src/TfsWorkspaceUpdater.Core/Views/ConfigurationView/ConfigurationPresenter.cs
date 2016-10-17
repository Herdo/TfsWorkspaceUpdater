namespace TfsWorkspaceUpdater.Core.Views.ConfigurationView
{
    using System;
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
            View.SaveConfigurationCanExecute += View_SaveConfigurationCanExecute;
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

        private void View_SaveConfigurationCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var noConnections = !ViewModel.TfsConnectionInformations.Any();
            if (noConnections)
            {
                e.CanExecute = true;
                return;
            }

            Uri parsedUri;

            var allConnectionsValid = ViewModel.TfsConnectionInformations.All(
                m => Uri.TryCreate(m.TfsAddress, UriKind.Absolute, out parsedUri)   // Address must be valid
                  && m.IntegratedSecurity   // Connections with integrated security
                 ||(!m.IntegratedSecurity   // Connections with credential authentication
                 && !string.IsNullOrWhiteSpace(m.Username)
                 && !string.IsNullOrWhiteSpace(m.Password)));

            e.CanExecute = allConnectionsValid;
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