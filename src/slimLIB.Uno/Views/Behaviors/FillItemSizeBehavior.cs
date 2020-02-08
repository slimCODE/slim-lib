using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using slimCODE.Extensions;
using slimCODE.Tools;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace slimCODE.Views.Behaviors
{
    public class FillItemSizeBehavior : BaseBehavior<ItemsWrapGrid>
    {
        protected override IDisposable Attach()
        {
            var parent = this.AssociatedObject.FindFirstParent<ListViewBase>();

            return parent
                ?.GetAndObserveActualSize()
                .Subscribe(
                    UpdateItemSize,
                    error => Logs.UseLogger(l => l.ReportError("Error observing size for dynamic item size.", error)));
        }

        private void UpdateItemSize(Size panelSize)
        {

        }
    }
}
