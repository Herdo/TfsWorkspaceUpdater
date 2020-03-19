using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.TeamFoundation;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.Services.Common;
using TfsWorkspaceUpdater.Shared.DAL;
using TfsWorkspaceUpdater.Shared.Data;
using TfsWorkspaceUpdater.Shared.Views.MainView;

namespace TfsWorkspaceUpdater.DAL
{
    public class TfsAccessor : ITfsAccessor
    {
        private const int MaxRetries = 10;
        private const int RetryDelayInMilliseconds = 6_000;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Fields

        private readonly IMainView _mainView;
        private readonly string _machineName;

        private int _retries;

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors

        public TfsAccessor(IMainView mainView)
        {
            _mainView = mainView;
            _machineName = Environment.MachineName;
            _retries = 1;
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Private Methods

        private async Task<TfsTeamProjectCollection> OpenCollectionAsync(TfsConnectionInformation connectionInformation)
        {
            do
            {
                try
                {
                    if (_retries > 1)
                        await Task.Delay(RetryDelayInMilliseconds);

                    TfsTeamProjectCollection tpc;
                    if (connectionInformation.IntegratedSecurity)
                    {
                        tpc = new TfsTeamProjectCollection(new Uri(connectionInformation.TfsAddress));
                        tpc.EnsureAuthenticated();
                    }
                    else
                    {
                        var netCred = new NetworkCredential(connectionInformation.Username, connectionInformation.Password);
                        var windowsCred = new Microsoft.VisualStudio.Services.Common.WindowsCredential(netCred);
                        var vssCred = new VssCredentials(windowsCred);
                        tpc = new TfsTeamProjectCollection(new Uri(connectionInformation.TfsAddress), vssCred);
                        tpc.Authenticate();
                    }

                    return tpc;
                }
                catch (WebException)
                {
                    if (_retries + 1 > MaxRetries)
                        throw;
                    _retries++;
                }
                catch (TeamFoundationServiceUnavailableException)
                {
                    if (_retries + 1 > MaxRetries)
                        throw;
                    _retries++;
                }
            } while (true);
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region ITfsAccessor Members

        async Task<List<UpdateableWorkingFolder>> ITfsAccessor.LoadAllWorkingFoldersAsync(IEnumerable<TfsConnectionInformation> connectionInformation)
        {
            var result = new List<UpdateableWorkingFolder>();

            foreach (var ci in connectionInformation)
            {
                TfsTeamProjectCollection tpc;
                try
                {
                    tpc = await OpenCollectionAsync(ci);
                }
                catch (WebException e)
                {
                    _mainView.ShowError("Error - connecting to the repository failed", e);
                    return result;
                }
                catch (TeamFoundationServiceUnavailableException e)
                {
                    _mainView.ShowError("Error - connecting to the repository failed", e);
                    return result;
                }
                var vcs = tpc.GetService<VersionControlServer>();
                var workspaceCollection = vcs.QueryWorkspaces(null, null, _machineName);

                result.AddRange(workspaceCollection.SelectMany(w => w.Folders.Select(f => new UpdateableWorkingFolder(_mainView, w, f))));
            }
                
            return result;
        }

        #endregion
    }
}