namespace TfsWorkspaceUpdater.DAL
{
    using System;
    using System.Collections.Generic;
    using Properties;
    using Shared.Data;
    using Shared.DAL;

    public class Configuration : IConfiguration
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Fields
            
        private readonly List<TfsConnectionInformation> _connections;

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors

        public Configuration()
        {
            _connections = new List<TfsConnectionInformation>();
            if (Settings.Default.ConnectionInformations != null)
                _connections.AddRange(Settings.Default.ConnectionInformations);
            AutoStart = Settings.Default.AutoStart;
            AutoClose = Settings.Default.AutoClose;
            ForceClose = Settings.Default.ForceClose;
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region IConfiguration Members

        public event EventHandler Saved;

        public bool AutoStart { get; set; }
        public bool AutoClose { get; set; }
        public bool ForceClose { get; set; }

        List<TfsConnectionInformation> IConfiguration.Connections => _connections;

        void IConfiguration.Save()
        {
            Settings.Default.AutoStart = AutoStart;
            Settings.Default.AutoClose = AutoClose;
            Settings.Default.ForceClose = ForceClose;
            Settings.Default.ConnectionInformations = _connections.ToArray();
            Settings.Default.Save();
            Saved?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}