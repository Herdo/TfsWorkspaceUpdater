namespace TfsWorkspaceUpdater.Shared.Views.ConfigurationView
{
    using System.Collections.Generic;
    using Data;

    public interface IConfigurationModel : IModel
    {
        List<TfsConnectionInformation> ConnectionInformations { get; set; }

        void Save();
    }
}