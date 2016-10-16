namespace TfsWorkspaceUpdater.Shared.Views.MainView
{
    using Data;

    public interface IMainPresenter
    {
        void Initialize(CommandLineParams parameter);
        void DisplayMainView();
    }
}