namespace TfsWorkspaceUpdater.Shared.Views.MainView
{
    using System;
    using System.Collections.Generic;
    using Data;

    public interface IMainModel : IModel
    {
        event EventHandler SettingsChanged;
        
        bool UseAutoStart { get; set; }
        
        bool UseAutoClose { get; set; }
        
        bool UseForceClose { get; set; }

        List<UpdateableWorkingFolder> LoadAllWorkingFolders();
    }
}