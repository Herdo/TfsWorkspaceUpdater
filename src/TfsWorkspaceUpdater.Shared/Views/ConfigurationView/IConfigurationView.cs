namespace TfsWorkspaceUpdater.Shared.Views.ConfigurationView
{
    using System;
    using System.Windows.Input;

    public interface IConfigurationView : IView<IConfigurationViewModel>
    {
        event EventHandler<ExecutedRoutedEventArgs> SaveConfigurationExecuted; 

        void Display();
        void Close();
    }
}