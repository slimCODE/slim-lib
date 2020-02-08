using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Seclusion.Models;
using Seclusion.Views;
using slimCODE.Applications;
using slimCODE.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Seclusion
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : SlimApplication
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnRegisterViewModels(IViewModelController viewModelController)
        {
            viewModelController.RegisterViewModel(typeof(GamePage), () => new GameViewModel());
        }

        protected override void OnInitialNavigation(LaunchActivatedEventArgs args, INavigationController navigation)
        {
            navigation.Navigate(typeof(GamePage));
        }
    }
}
