using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using slimCODE.Applications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace slimCODE.Views.Behaviors
{
    public class FrameBehavior : BaseBehavior<Frame>
    {
        #region Controller PROPERTY

        public NavigationController Controller
        {
            get { return (NavigationController)GetValue(ControllerProperty); }
            set { SetValue(ControllerProperty, value); }
        }

        public static readonly DependencyProperty ControllerProperty =
            DependencyProperty.Register("Controller", typeof(NavigationController), typeof(FrameBehavior), new PropertyMetadata(null, OnControllerChanged));

        private static void OnControllerChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
        }

        #endregion

        protected override IDisposable Attach()
        {
            // Only the root navigation controller can handle the back button on the app view.
            var controller = new NavigationController(SlimApplication.Services.GetRequiredService<IViewModelController>(), preventAppViewBackButton: true);
            controller.ConnectWithFrame(this.AssociatedElement);

            this.Controller = controller;

            return controller;
        }
    }
}
