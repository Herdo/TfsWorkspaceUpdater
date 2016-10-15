namespace TfsWorkspaceUpdater.Shared.Views.ConfigurationView
{
    using System.Collections.ObjectModel;
    using Data;

    public interface IConfigurationViewModel : IViewModel
    {
        ObservableCollection<TfsConnectionInformation> TfsConnectionInformations { get; }
    }
}