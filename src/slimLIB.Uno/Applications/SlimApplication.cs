using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using slimCODE.Extensions;
using slimCODE.Views;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace slimCODE.Applications
{
    /// <summary>
    /// Represents an application using the slimLIB library.
    /// </summary>
    /// <remarks>
    /// Your application must derive from SlimApplication and override at least the <see cref="OnInitialNavigation(LaunchActivatedEventArgs, INavigationController)"/>
    /// and <see cref="OnRegisterViewModels(IViewModelController)"/> methods. It can also override <see cref="ConfigureServices(IConfiguration, IServiceCollection)"/>
    /// to provide extra services available via dependency injection (using .NET Core's DI system).
    /// </remarks>
    public abstract class SlimApplication : Application
    {
        private readonly IHost _host;

        public SlimApplication()
        {
            _host = new HostBuilder()
                // TODO: This path is not good, but we don't really care for now.
                // .UseContentRoot(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData))
                .ConfigureServices((context, services) => this.ConfigureServices(context.Configuration, services))
                .Build();

            SlimApplication.Services = _host.Services;
        }

        /// <summary>
        /// Gets the main service provider. Useful for UI components that require access to services.
        /// </summary>
        public static IServiceProvider Services { get; private set; }

        /// <summary>
        /// Override to provide the initial navigation for your application.
        /// <code>
        /// protected override void OnInitialNavigation(LaunchActivatedEventArgs args, INavigationController navigation)
        /// {
        ///     navigation.Navigate(typeof(MainPage));
        /// }
        /// </code>
        /// </summary>
        /// <param name="args"></param>
        /// <param name="navigation"></param>
        protected abstract void OnInitialNavigation(LaunchActivatedEventArgs args, INavigationController navigation);

        /// <summary>
        /// Override to provide a list of view-models for each navigatable view.
        /// <code>
        /// protected override void OnRegisterViewModels(IViewModelController viewModels)
        /// {
        ///     viewModels.RegisterViewModel(typeof(MainPage), () => new Models.MainPageViewModel());
        /// }
        /// </code>
        /// </summary>
        /// <param name="viewModelController"></param>
        protected abstract void OnRegisterViewModels(IViewModelController viewModelController);

        /// <summary>
        /// Internal. Do not override.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            // Not required, fails on Android
            // _host.Start();

            base.OnLaunched(args);

            EnsureRootFrameCreated(args);
            EnsureViewModelsRegistered(args);
            EnsureFirstNavigation(args);
        }

        /// <summary>
        /// Performs default service registration required for any apps using slimLIB. You can override to
        /// provide extra services, but make sure to call the base class.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="services"></param>
        protected virtual void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            services
                .AddSingleton<IViewModelController, ViewModelController>()
                .AddSingleton<INavigationController, NavigationController>();
        }

        private void EnsureRootFrameCreated(LaunchActivatedEventArgs args)
        {
            var rootFrame = Windows.UI.Xaml.Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                rootFrame = new Frame();
                Windows.UI.Xaml.Window.Current.Content = rootFrame;

                // TODO: Assign Content with extended splash screen

                _host.Services.GetService<INavigationController>().ConnectWithFrame(rootFrame);
            }
        }

        private void EnsureViewModelsRegistered(LaunchActivatedEventArgs args)
        {
            if (!args.PreviousExecutionState.IsOneOf(ApplicationExecutionState.Running, ApplicationExecutionState.Suspended))
            {
                OnRegisterViewModels(_host.Services.GetService<IViewModelController>());
            }
        }

        private void EnsureFirstNavigation(LaunchActivatedEventArgs args)
        {
            OnInitialNavigation(args, _host.Services.GetService<INavigationController>());
            Windows.UI.Xaml.Window.Current.Activate();
        }

        [Conditional("DEBUG")]
        private void SetUpDebugSettings()
        {
            DebugSettings.EnableFrameRateCounter = true;
            DebugSettings.EnableRedrawRegions = false;
            DebugSettings.IsBindingTracingEnabled = true;
            DebugSettings.IsOverdrawHeatMapEnabled = false;
            DebugSettings.IsTextPerformanceVisualizationEnabled = false;

            DebugSettings.BindingFailed += OnBindingFailed;
        }

        private void OnBindingFailed(object sender, BindingFailedEventArgs e)
        {
            System.Diagnostics.Debugger.Break();
        }
    }
}
