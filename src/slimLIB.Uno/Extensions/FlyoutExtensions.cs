using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace slimCODE.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="FlyoutBase"/> instances.
    /// </summary>
    public static partial class FlyoutExtensions
    {
        /// <summary>
        /// Returns an observable firing units every time a <see cref="FlyoutBase"/> is closed.
        /// </summary>
        /// <param name="flyout"></param>
        /// <returns></returns>
        public static IObservable<Unit> ObserveClosed(this FlyoutBase flyout)
        {
#if HAS_UNO
            return Observable
                .FromEventPattern(
                    h => flyout.Closed += h,
                    h => flyout.Closed -= h)
                .SelectUnit();
#else
            return Observable
                .FromEventPattern<object>(
                    h => flyout.Closed += h,
                    h => flyout.Closed -= h)
                .SelectUnit();
#endif
        }

        /// <summary>
        /// Returns an observable firing units every time a <see cref="FlyoutBase"/> is opening.
        /// </summary>
        /// <param name="flyout"></param>
        /// <returns></returns>
        public static IObservable<Unit> ObserveOpening(this FlyoutBase flyout)
        {
#if HAS_UNO
            return Observable
                .FromEventPattern(
                    h => flyout.Opening += h,
                    h => flyout.Opening -= h)
                .SelectUnit();
#else
            return Observable
                .FromEventPattern<object>(
                    h => flyout.Opening += h,
                    h => flyout.Opening -= h)
                .SelectUnit();
#endif
        }

        /// <summary>
        /// Returns an observable firing units every time a <see cref="FlyoutBase"/> is opened.
        /// </summary>
        /// <param name="flyout"></param>
        /// <returns></returns>
        public static IObservable<Unit> ObserveOpened(this FlyoutBase flyout)
        {
#if HAS_UNO
            return Observable
                .FromEventPattern(
                    h => flyout.Opened += h,
                    h => flyout.Opened -= h)
                .SelectUnit();
#else
            return Observable
                .FromEventPattern<object>(
                    h => flyout.Opened += h,
                    h => flyout.Opened -= h)
                .SelectUnit();
#endif
        }

        /// <summary>
        /// Returns an observable firing boolean every time the visibility of a <see cref="FlyoutBase"/> changes.
        /// </summary>
        /// <param name="flyout"></param>
        /// <returns></returns>
        public static IObservable<bool> ObserveIsVisible(this FlyoutBase flyout)
        {
            return Observable
                .Merge(
                    flyout.ObserveClosed().Select(_ => false),
                    flyout.ObserveOpened().Select(_ => true));
        }
    }
}
