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
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region IConfiguration Members

        public event EventHandler Saved;

        List<TfsConnectionInformation> IConfiguration.Connections => _connections;

        void IConfiguration.Save()
        {
            Settings.Default.ConnectionInformations = _connections.ToArray();
            Settings.Default.Save();
            Saved?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}