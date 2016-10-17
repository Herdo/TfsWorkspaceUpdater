namespace TfsWorkspaceUpdater.Views.ConfigurationView
{
    using System;
    using System.Windows.Input;
    using Shared.Views;
    using Shared.Views.ConfigurationView;

    /// <summary>
    /// Interaction logic for ConfigurationWindow.xaml
    /// </summary>
    public partial class ConfigurationWindow : IConfigurationView
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors

        public ConfigurationWindow()
        {
            InitializeComponent();

            Header.MouseDown += Header_MouseDown;
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Event Handler

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void SaveConfiguration_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SaveConfigurationExecuted?.Invoke(sender, e);
        }

        private void SaveConfiguration_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            SaveConfigurationCanExecute?.Invoke(sender, e);
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region IConfigurationView Members

        void IView<IConfigurationViewModel>.ConnectDataSource(IConfigurationViewModel viewModel)
        {
            DataContext = viewModel;
        }

        public event EventHandler<ExecutedRoutedEventArgs> SaveConfigurationExecuted;
        public event EventHandler<CanExecuteRoutedEventArgs> SaveConfigurationCanExecute;

        void IConfigurationView.Display()
        {
            ShowDialog();
        }

        #endregion
    }
}
