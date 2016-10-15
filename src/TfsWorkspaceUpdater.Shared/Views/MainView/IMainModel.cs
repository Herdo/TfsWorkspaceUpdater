namespace TfsWorkspaceUpdater.Shared.Views.MainView
{
    using System;
    using System.Collections.Generic;
    using Data;

    public interface IMainModel : IModel
    {
        event EventHandler SettingsChanged;

        List<UpdateableWorkingFolder> LoadAllWorkingFolders();
    }
}