namespace TfsWorkspaceUpdater.Views.MainView
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using Shared.Views;
    using Shared.Views.MainView;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IMainView
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
            Header.MouseDown += Header_MouseDown;
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Event Handler

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Displayed?.Invoke(sender, e);
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void Close_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CloseExecuted?.Invoke(sender, e);
        }

        private void OpenConfiguration_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            OpenConfigurationExecuted?.Invoke(sender, e);
        }

        private void OpenConfiguration_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            OpenConfigurationCanExecute?.Invoke(sender, e);
        }

        private void Start_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            StartExecuted?.Invoke(sender, e);
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region IMainView Members

        void IView<IMainViewModel>.ConnectDataSource(IMainViewModel viewModel)
        {
            DataContext = viewModel;
        }

        public event EventHandler Displayed;
        public event EventHandler<ExecutedRoutedEventArgs> CloseExecuted;
        public event EventHandler<ExecutedRoutedEventArgs> OpenConfigurationExecuted;
        public event EventHandler<CanExecuteRoutedEventArgs> OpenConfigurationCanExecute;
        public event EventHandler<ExecutedRoutedEventArgs> StartExecuted;

        void IMainView.Display()
        {
            ShowDialog();
        }

        #endregion
    }
}
