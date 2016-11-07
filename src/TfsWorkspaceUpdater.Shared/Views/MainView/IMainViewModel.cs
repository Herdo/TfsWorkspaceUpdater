namespace TfsWorkspaceUpdater.Shared.Views.MainView
{
    using System.Collections.ObjectModel;
    using Data;

    public interface IMainViewModel : IViewModel
    {
        ObservableCollection<UpdateableWorkingFolder> WorkingFolders { get; }
        bool IsLoading { get; set; }
        bool StartAvailable { get; set; }

        bool UseAutoStart { get; set; }
        bool UseAutoClose { get; set; }
        bool UseForceClose { get; set; }
    }
}