namespace TfsWorkspaceUpdater.Shared.DAL
{
    using System.Collections.Generic;
    using Data;

    public interface ITfsAccessor
    {
        List<UpdateableWorkingFolder> LoadAllWorkingFolders(IEnumerable<TfsConnectionInformation> connectionInformations);
    }
}