using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xaml.Interactivity;
using slimCODE.Extensions;
using slimCODE.Tools;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace slimCODE.Views.Behaviors
{
    public class DynamicItemSizeBehavior : BaseBehavior<ItemsWrapGrid>
    {
        #region MaxItemWidth PROPERTY

        public double MaxItemWidth
        {
            get { return (double)GetValue(MaxItemWidthProperty); }
            set { SetValue(MaxItemWidthProperty, value); }
        }

        public static readonly DependencyProperty MaxItemWidthProperty =
            DependencyProperty.Register("MaxItemWidth", typeof(double), typeof(DynamicItemSizeBehavior), new PropertyMetadata(0D));

        #endregion

        #region MinItemWidth PROPERTY

        public double MinItemWidth
        {
            get { return (double)GetValue(MinItemWidthProperty); }
            set { SetValue(MinItemWidthProperty, value); }
        }

        public static readonly DependencyProperty MinItemWidthProperty =
            DependencyProperty.Register("MinItemWidth", typeof(double), typeof(DynamicItemSizeBehavior), new PropertyMetadata(0D));

        #endregion

        protected override IDisposable Attach()
        {
            var parent = this.AssociatedObject.FindFirstParent<ListViewBase>();

            return parent
                ?.GetAndObserveActualSize()
                .Subscribe(
                    size => UpdateItemSize(size.Width),
                    error => Logs.UseLogger(l => l.ReportError("Error observing size for dynamic item size.", error)));
        }

        private void UpdateItemSize(double width)
        {
            var minCount = (this.MaxItemWidth == 0) ? 1 : (int)(width / this.MaxItemWidth);
            var maxCount = (this.MinItemWidth == 0) ? 1 : (int)(width / this.MinItemWidth);
            var count = Math.Max(1, Math.Max(minCount, maxCount));

            this.AssociatedElement.ItemWidth = Math.Floor(width / count);
        }
    }
}
