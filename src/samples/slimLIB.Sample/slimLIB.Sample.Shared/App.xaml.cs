using Microsoft.Extensions.Logging;
using slimCODE.Applications;
using slimCODE.Extensions;
using slimCODE.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
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
using slimLIB.Sample.Views;
using slimLIB.Sample.Views.Controls;

namespace slimLIB.Sample
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

        protected override void OnRegisterViewModels(IViewModelController viewModels)
        {
            viewModels.RegisterViewModel<MainPage, Models.MainPageViewModel>(() => new Models.MainPageViewModel(_hack));
            viewModels.RegisterViewModel<MessageAndDialogExamplesPage, Models.MessageAndDialogExamplesPageViewModel>();

            // Just an example of global properties
            BaseViewModelExtensions.CreateGlobalProperty(
                "DateAndTime",
                () => Observable
                    .Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
                    .Select(_ => DateTimeOffset.Now.ToString()),
                "-");

            // Dialogs must also be registered by name.
            // TODO: It's wrong to be there, and be on extensions.
            BaseViewModelExtensions.RegisterDialog<ExampleDialog>("ExampleDialog");
        }

        private INavigationController _hack;

        protected override void OnInitialNavigation(LaunchActivatedEventArgs args, INavigationController navigation)
        {
            _hack = navigation;
            navigation.Navigate(typeof(MainPage));
        }
    }
}
