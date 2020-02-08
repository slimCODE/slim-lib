using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using Windows.UI.Core;

namespace slimCODE.Extensions
{
    /// <summary>
    /// Provides extension methods on <see cref="SystemNavigationManager"/> instances.
    /// </summary>
    public static partial class SystemNavigationManagerExtensions
    {
        /// <summary>
        /// Observes the <see cref="SystemNavigationManager.BackRequested"/> event.
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        public static IObservable<BackRequestedEventArgs> ObserveBackRequested(this SystemNavigationManager manager)
        {
            return Observable
                .FromEventPattern<BackRequestedEventArgs>(
                    h => manager.BackRequested += h,
                    h => manager.BackRequested -= h)
                .Select(ep => ep.EventArgs);
        }
    }
}
