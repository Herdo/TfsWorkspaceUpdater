﻿using System.Threading.Tasks;

namespace TfsWorkspaceUpdater.Views.MainView
{
    using System;
    using System.Collections.Generic;
    using Shared.Data;
    using Shared.DAL;
    using Shared.Views.MainView;
    public class MainModel : IMainModel
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Fields

        private readonly IConfiguration _configuration;
        private readonly ITfsAccessor _tfsAccessor;

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors

        public MainModel(IConfiguration configuration,
                         ITfsAccessor tfsAccessor)
        {
            _configuration = configuration;
            _configuration.Saved += Configuration_Saved;
            _tfsAccessor = tfsAccessor;
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Event Handler

        private void Configuration_Saved(object sender, EventArgs e)
        {
            SettingsChanged?.Invoke(sender, e);
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region IMainModel Members

        public event EventHandler SettingsChanged;

        bool IMainModel.UseAutoStart
        {
            get => _configuration.AutoStart;
            set
            {
                _configuration.AutoStart = value;
                _configuration.Saved -= Configuration_Saved;
                _configuration.Save();
                _configuration.Saved += Configuration_Saved;
            }
        }

        bool IMainModel.UseAutoClose
        {
            get => _configuration.AutoClose;
            set
            {
                _configuration.AutoClose = value;
                _configuration.Saved -= Configuration_Saved;
                _configuration.Save();
                _configuration.Saved += Configuration_Saved;
            }
        }

        bool IMainModel.UseForceClose
        {
            get => _configuration.ForceClose;
            set
            {
                _configuration.ForceClose = value;
                _configuration.Saved -= Configuration_Saved;
                _configuration.Save();
                _configuration.Saved += Configuration_Saved;
            }
        }

        async Task<List<UpdateableWorkingFolder>> IMainModel.LoadAllWorkingFoldersAsync() => await _tfsAccessor.LoadAllWorkingFoldersAsync(_configuration.Connections);

        #endregion
    }
}