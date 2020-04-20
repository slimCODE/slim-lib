using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace slimCODE.Extensions
{
    /// <summary>
    /// Contains extension methods on <see cref="NavigationView"/> instances.
    /// </summary>
    public static partial class NavigationViewExtensions
    {
        /// <summary>
        /// Returns an observable firing with event arguments about the newly selected item.
        /// </summary>
        /// <param name="navigationView"></param>
        /// <returns></returns>
        public static IObservable<NavigationViewSelectionChangedEventArgs> ObserveSelectionChanged(this NavigationView navigationView)
        {
            return Observable
                .FromEventPattern<TypedEventHandler<NavigationView, NavigationViewSelectionChangedEventArgs>, NavigationViewSelectionChangedEventArgs>(
                    h => navigationView.SelectionChanged += h,
                    h => navigationView.SelectionChanged -= h)
                .Select(ep => ep.EventArgs);
        }
    }
}
