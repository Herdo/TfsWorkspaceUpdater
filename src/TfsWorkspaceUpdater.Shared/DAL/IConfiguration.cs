namespace TfsWorkspaceUpdater.Shared.DAL
{
    using System;
    using System.Collections.Generic;
    using Data;

    public interface IConfiguration
    {
        event EventHandler Saved;

        List<TfsConnectionInformation> Connections { get; }

        void Save();
    }
}