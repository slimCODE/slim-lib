using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace slimCODE.Extensions
{
    /// <summary>
    /// Provides extension methods on <see cref="FrameworkElement"/> instances.
    /// </summary>
    public static class FrameworkElementExtensions
    {
        /// <summary>
        /// Gets the actual <see cref="Size"/> of a <see cref="FrameworkElement"/>, from its 
        /// <see cref="FrameworkElement.ActualWidth"/> and <see cref="FrameworkElement.ActualHeight"/>.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Size GetActualSize(this FrameworkElement element)
        {
            return new Size(element.ActualWidth, element.ActualHeight);
        }

        /// <summary>
        /// Returns an observable that fires a <see cref="Unit"/> when a <see cref="FrameworkElement"/> is loaded.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static IObservable<Unit> ObserveLoaded(this FrameworkElement element)
        {
            return Observable
                .FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
                    h => element.Loaded += h,
                    h => element.Loaded -= h)
                .SelectUnit();
        }

        /// <summary>
        /// Returns an observable that fires when <see cref="FrameworkElement.SizeChanged"/> is called.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static IObservable<SizeChangedEventArgs> ObserveSizeChanged(this FrameworkElement element)
        {
            return Observable
                .FromEventPattern<SizeChangedEventHandler, SizeChangedEventArgs>(
                    h => element.SizeChanged += h,
                    h => element.SizeChanged -= h)
                .Select(ep => ep.EventArgs);
        }

        /// <summary>
        /// Returns a hot observable that fires when a <see cref="FrameworkElement"/>'s actual size changes.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static IObservable<Size> GetAndObserveActualSize(this FrameworkElement element)
        {
            return element
                .ObserveSizeChanged()
                .Select(args => args.NewSize)
                .StartWith(element.GetActualSize());
        }
    }
}
