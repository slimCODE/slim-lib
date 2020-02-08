using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using slimCODE.Extensions;
using slimCODE.Tools;
using System.Reactive.Disposables;

namespace slimCODE.Views.Behaviors
{
    public class AttachedFlyoutBehavior : BaseBehavior<FrameworkElement>
    {
        private bool _preventUpdate;

        #region IsAutoWidth PROPERTY

        public bool IsAutoWidth
        {
            get { return (bool)GetValue(IsAutoWidthProperty); }
            set { SetValue(IsAutoWidthProperty, value); }
        }

        public static readonly DependencyProperty IsAutoWidthProperty =
            DependencyProperty.Register("IsAutoWidth", typeof(bool), typeof(AttachedFlyoutBehavior), new PropertyMetadata(false));

        #endregion

        #region IsVisible PROPERTY

        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.Register("IsVisible", typeof(bool), typeof(AttachedFlyoutBehavior), new PropertyMetadata(false, OnIsVisibleChanged));

        private static void OnIsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AttachedFlyoutBehavior)?.UpdateVisibility((bool)e.NewValue);
        }

        #endregion

        protected override IDisposable Attach()
        {
            var flyout = FlyoutBase.GetAttachedFlyout(this.AssociatedElement);

            if (flyout != null)
            {
                return flyout
                    .ObserveIsVisible()
                    .Subscribe(
                        isVisible => SetIsVisible(isVisible),
                        error => Logs.UseLogger(l => l.ReportError("Observing Flyout closed or opened.", error)));
            }

            return Disposable.Empty;
        }

        private void SetIsVisible(bool isVisible)
        {
            _preventUpdate = true;

            try
            {
                this.IsVisible = isVisible;
            }
            finally
            {
                _preventUpdate = false;
            }
        }

        private void UpdateVisibility(bool isVisible)
        {
            if (!_preventUpdate)
            {
                var flyout = FlyoutBase.GetAttachedFlyout(this.AssociatedElement);

                if (flyout != null)
                {
                    if (isVisible)
                    {
                        FlyoutBase.ShowAttachedFlyout(this.AssociatedElement);

                        if (this.IsAutoWidth)
                        {
                            this.ResizeToElement((flyout as Flyout)?.Content as FrameworkElement, this.AssociatedObject as FrameworkElement);
                        }
                    }
                    else
                    {
                        flyout.Hide();
                    }
                }
            }
        }

        private void ResizeToElement(FrameworkElement element, FrameworkElement outerElement)
        {
            if ((element == null) || (outerElement == null))
            {
                return;
            }

            element.Width = outerElement.ActualWidth;
        }
    }
}
