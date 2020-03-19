using System.Collections.Generic;
using System.Threading.Tasks;
using TfsWorkspaceUpdater.Shared.Data;

namespace TfsWorkspaceUpdater.Shared.DAL
{
    public interface ITfsAccessor
    {
        Task<List<UpdateableWorkingFolder>> LoadAllWorkingFoldersAsync(IEnumerable<TfsConnectionInformation> connectionInformation);
    }
}