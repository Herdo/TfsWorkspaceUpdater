namespace TfsWorkspaceUpdater.Shared.Views.MainView
{
    using System;
    using System.Windows.Input;

    public interface IMainView : IView<IMainViewModel>
    {
        event EventHandler Displayed;
        event EventHandler<ExecutedRoutedEventArgs> CloseExecuted;
        event EventHandler<ExecutedRoutedEventArgs> OpenConfigurationExecuted;
        event EventHandler<CanExecuteRoutedEventArgs> OpenConfigurationCanExecute;
        event EventHandler<ExecutedRoutedEventArgs> StartExecuted; 

        void Display();
    }
}