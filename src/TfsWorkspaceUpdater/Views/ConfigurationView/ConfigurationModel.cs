namespace TfsWorkspaceUpdater.Views.ConfigurationView
{
    using System.Collections.Generic;
    using Shared.Data;
    using Shared.DAL;
    using Shared.Views.ConfigurationView;

    public class ConfigurationModel : IConfigurationModel
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Fields

        private readonly IConfiguration _configuration;

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors

        public ConfigurationModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region IConfigurationModel Members

        List<TfsConnectionInformation> IConfigurationModel.ConnectionInformations
        {
            get { return _configuration.Connections; }
            set
            {
                _configuration.Connections.Clear();
                _configuration.Connections.AddRange(value);
            }
        }

        void IConfigurationModel.Save()
        {
            _configuration.Save();
        }

        #endregion
    }
}