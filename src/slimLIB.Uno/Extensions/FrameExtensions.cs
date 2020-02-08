using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace slimCODE.Extensions
{
    /// <summary>
    /// Provides extension methods on <see cref="Frame"/> instances.
    /// </summary>
    public static partial class FrameExtensions
    {
        /// <summary>
        /// Returns an observable exposing any navigation failure.
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        public static IObservable<NavigationFailedEventArgs> ObserveNavigationFailed(this Frame frame)
        {
            return Observable
                .FromEventPattern<NavigationFailedEventHandler, NavigationFailedEventArgs>(
                    h => frame.NavigationFailed += h,
                    h => frame.NavigationFailed -= h)
                .Select(ep => ep.EventArgs);
        }

        /// <summary>
        /// Returns an observable exposing any interrupted navigation.
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        public static IObservable<NavigationEventArgs> ObserveNavigationStopped(this Frame frame)
        {
            return Observable
                .FromEventPattern<NavigationStoppedEventHandler, NavigationEventArgs>(
                    h => frame.NavigationStopped += h,
                    h => frame.NavigationStopped -= h)
                .Select(ep => ep.EventArgs);
        }

        /// <summary>
        /// Returns an observable exposing any navigation that is starting.
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        public static IObservable<NavigatingCancelEventArgs> ObserveNavigating(this Frame frame)
        {
            return Observable
                .FromEventPattern<NavigatingCancelEventHandler, NavigatingCancelEventArgs>(
                    h => frame.Navigating += h,
                    h => frame.Navigating -= h)
                .Select(ep => ep.EventArgs);
        }

        /// <summary>
        /// Returns an observable exposing completed navigations.
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        public static IObservable<NavigationEventArgs> ObserveNavigated(this Frame frame)
        {
            return Observable
                .FromEventPattern<NavigatedEventHandler, NavigationEventArgs>(
                    h => frame.Navigated += h,
                    h => frame.Navigated -= h)
                .Select(ep => ep.EventArgs);
        }
    }
}
