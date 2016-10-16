namespace TfsWorkspaceUpdater.Shared.Data
{
    public sealed class CommandLineParams
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Properties

        /// <summary>
        /// Gets or sets if the update process should start automatically.
        /// </summary>
        public bool AutoStart { get; set; }

        /// <summary>
        /// Gets or sets if the application should close after updating.
        /// </summary>
        public bool AutoClose { get; set; }

        /// <summary>
        /// Gets or sets if the application should close after updating, even if some working folders have failures or conflicts.
        /// </summary>
        public bool ForceClose { get; set; }

        #endregion
    }
}