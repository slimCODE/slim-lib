using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using slimCODE.Extensions;
using slimCODE.Tools;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace slimCODE.Views.Behaviors
{
    public class TextBoxBehavior : BaseBehavior<TextBox>
    {
        protected override IDisposable Attach()
        {
            return this
                .AssociatedElement
                .ObserveTextChanged()
                .Select(_ => this.AssociatedElement.GetBindingExpression(TextBox.TextProperty))
                .WhereNotNull()
                .Subscribe(
                    binding => binding.UpdateSource(),
                    error => Logs.UseLogger(logger => logger.ReportError("Observing TextChanged for explicit binding updates.", error)));
        }
    }
}
