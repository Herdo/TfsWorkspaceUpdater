namespace TfsWorkspaceUpdater.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Microsoft.TeamFoundation;
    using Microsoft.TeamFoundation.Client;
    using Microsoft.TeamFoundation.VersionControl.Client;
    using Shared.Data;
    using Shared.DAL;
    using Shared.Views.MainView;

    public class TfsAccessor : ITfsAccessor
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Fields

        private readonly IMainView _mainView;
        private readonly string _machineName;
        private readonly string _userDomainName;

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors

        public TfsAccessor(IMainView mainView)
        {
            _mainView = mainView;
            _machineName = Environment.MachineName;
            _userDomainName = Environment.UserDomainName;
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Private Methods

        private static TfsTeamProjectCollection OpenCollection(TfsConnectionInformation connectionInformation)
        {
            TfsTeamProjectCollection tpc;

            if (connectionInformation.IntegratedSecurity)
            {
                tpc = new TfsTeamProjectCollection(new Uri(connectionInformation.TfsAddress));
                tpc.EnsureAuthenticated();
            }
            else
            {
                var netCred = new NetworkCredential(connectionInformation.Username, connectionInformation.Password);
                var baseCred = new BasicAuthCredential(netCred);
                var tfsCred = new TfsClientCredentials(baseCred) { AllowInteractive = false };

                tpc = new TfsTeamProjectCollection(new Uri(connectionInformation.TfsAddress), tfsCred);
                tpc.Authenticate();
            }

            return tpc;
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region ITfsAccessor Members

        List<UpdateableWorkingFolder> ITfsAccessor.LoadAllWorkingFolders(IEnumerable<TfsConnectionInformation> connectionInformations)
        {
            var result = new List<UpdateableWorkingFolder>();

            foreach (var ci in connectionInformations)
            {
                TfsTeamProjectCollection tpc;
                try
                {
                    tpc = OpenCollection(ci);
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
                var workspaces = vcs.QueryWorkspaces(null, null, _machineName);

                result.AddRange(workspaces.SelectMany(w => w.Folders.Select(f => new UpdateableWorkingFolder(_mainView, w, f))));
            }
                
            return result;
        }

        #endregion
    }
}