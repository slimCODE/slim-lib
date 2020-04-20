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
    public class NavigationViewBehavior : BaseBehavior<NavigationView>
    {
        #region SelectionChangedCommand PROPERTY

        public ICommand SelectionChangedCommand
        {
            get { return (ICommand)GetValue(SelectionChangedCommandProperty); }
            set { SetValue(SelectionChangedCommandProperty, value); }
        }

        public static readonly DependencyProperty SelectionChangedCommandProperty =
            DependencyProperty.Register("SelectionChangedCommand", typeof(ICommand), typeof(NavigationViewBehavior), new PropertyMetadata(null));

        #endregion

        protected override IDisposable Attach()
        {
            this.AssociatedElement.SelectedItem = this.AssociatedElement.MenuItems.FirstOrDefault();

            return this
                .AssociatedElement
                .ObserveSelectionChanged()
                .Select(args => new NavigationViewSelection(args))
                .Where(selection => this.SelectionChangedCommand?.CanExecute(selection) ?? false)
                .Subscribe(
                    selection => this.SelectionChangedCommand?.Execute(selection),
                    error => Logs.UseLogger(l => l.ReportError("Observing SelectionChanged.", error)));
        }
    }
}
