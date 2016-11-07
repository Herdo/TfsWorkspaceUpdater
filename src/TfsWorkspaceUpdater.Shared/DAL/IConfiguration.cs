namespace TfsWorkspaceUpdater.Shared.DAL
{
    using System;
    using System.Collections.Generic;
    using Data;

    public interface IConfiguration
    {
        event EventHandler Saved;

        /// <summary>
        /// Gets or sets if the update process should start automatically.
        /// </summary>
        bool AutoStart { get; set; }

        /// <summary>
        /// Gets or sets if the application should close after updating.
        /// </summary>
        bool AutoClose { get; set; }

        /// <summary>
        /// Gets or sets if the application should close after updating, even if some working folders have failures or conflicts.
        /// </summary>
        bool ForceClose { get; set; }

        List<TfsConnectionInformation> Connections { get; }

        void Save();
    }
}