using System.Windows;

namespace TfsWorkspaceUpdater
{
    using System;
    using System.Linq;
    using Core.Views.ConfigurationView;
    using Core.Views.MainView;
    using DAL;
    using Microsoft.Practices.Unity;
    using Shared;
    using Shared.Data;
    using Shared.DAL;
    using Shared.Views.ConfigurationView;
    using Shared.Views.MainView;
    using Views.ConfigurationView;
    using Views.MainView;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : IApplication
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors

        public App()
        {
            Startup += App_Startup;
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Private Methods

        private IUnityContainer ConfigureContainer()
        {
            var container = new UnityContainer()

                // App
                .RegisterInstance(typeof(IApplication), this, new ContainerControlledLifetimeManager())

                // Main View
                .RegisterType<IMainPresenter, MainPresenter>()
                .RegisterType<IMainView, MainWindow>()
                .RegisterType<IMainViewModel, MainViewModel>()
                .RegisterType<IMainModel, MainModel>()

                // Configuration View
                .RegisterType<IConfigurationPresenter, ConfigurationPresenter>()
                .RegisterType<IConfigurationView, ConfigurationWindow>()
                .RegisterType<IConfigurationViewModel, ConfigurationViewModel>()
                .RegisterType<IConfigurationModel, ConfigurationModel>()

                // DAL
                .RegisterType<IConfiguration, Configuration>(new ContainerControlledLifetimeManager())
                .RegisterType<ITfsAccessor, TfsAccessor>()

                ;
            
            // Navigation
            container.RegisterInstance(typeof(Func<IConfigurationPresenter>),
                new Func<IConfigurationPresenter>(() => container.Resolve<IConfigurationPresenter>()));

            return container;
        }

        private static CommandLineParams GetCommandLineParameters()
        {
            var args = Environment.GetCommandLineArgs();
            var result = new CommandLineParams();

            if (args.Contains("/s"))
                result.AutoStart = true;
            if (args.Contains("/c"))
                result.AutoClose = true;
            if (args.Contains("/f"))
                result.ForceClose = true;

            return result;
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Event Handler

        private void App_Startup(object sender, StartupEventArgs e)
        {
            var parameter = GetCommandLineParameters();
            using (var container = ConfigureContainer())
            {
                var mainPresenter = container.Resolve<IMainPresenter>();
                mainPresenter.Initialize(parameter);
                mainPresenter.DisplayMainView();
            }
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region IApplication Members

        void IApplication.Close()
        {
            Shutdown();
        }

        #endregion
    }
}
