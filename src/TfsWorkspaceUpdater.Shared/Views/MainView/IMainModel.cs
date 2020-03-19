using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TfsWorkspaceUpdater.Shared.Data;

namespace TfsWorkspaceUpdater.Shared.Views.MainView
{
    public interface IMainModel : IModel
    {
        event EventHandler SettingsChanged;
        
        bool UseAutoStart { get; set; }
        
        bool UseAutoClose { get; set; }
        
        bool UseForceClose { get; set; }

        Task<List<UpdateableWorkingFolder>> LoadAllWorkingFoldersAsync();
    }
}