namespace TfsWorkspaceUpdater.Shared.Views
{
    public interface IView<in TViewModel> where TViewModel : IViewModel
    {
        void ConnectDataSource(TViewModel viewModel);
    }
}