using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slimCODE.Extensions
{
    /// <summary>
    /// Provides extension methods on <see cref="ObservableCollection{T}"/> instances.
    /// </summary>
    public static partial class ObservableCollectionExtensions
    {
        /// <summary>
        /// Returns an observable over the <see cref="ObservableCollection{T}.CollectionChanged"/> event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IObservable<Unit> ObserveChanged<T>(this ObservableCollection<T> collection)
        {
            return Observable
                .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                    h => collection.CollectionChanged += h,
                    h => collection.CollectionChanged -= h)
                .SelectUnit();
        }
    }
}
