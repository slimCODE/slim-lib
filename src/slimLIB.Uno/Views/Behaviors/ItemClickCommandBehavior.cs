using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using slimCODE.Extensions;
using slimCODE.Tools;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace slimCODE.Views.Behaviors
{
    public class ItemClickCommandBehavior : BaseBehavior<ListViewBase>
    {
        #region Command PROPERTY

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(ItemClickCommandBehavior), new PropertyMetadata(null));

        #endregion

        protected override IDisposable Attach()
        {
            return this
                .AssociatedElement
                .ObserveItemClick()
                .Where(item => this.Command?.CanExecute(item) ?? false)
                .Subscribe(
                    item => this.Command?.Execute(item),
                    error => Logs.UseLogger(l => l.ReportError("Observing ItemClick.", error)));
        }
    }
}
