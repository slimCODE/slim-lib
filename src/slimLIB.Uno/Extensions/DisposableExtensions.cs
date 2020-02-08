using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;

namespace slimCODE.Extensions
{
    /// <summary>
    /// Provides extension methods on <see cref="IDisposable"/> instances.
    /// </summary>
    public static partial class DisposableExtensions
    {
        /// <summary>
        /// Calls <see cref="IDisposable.Dispose"/> on an <see cref="IDisposable"/> object when not null.
        /// </summary>
        /// <param name="disposable"></param>
        public static void SafeDispose(this IDisposable disposable)
        {
            disposable.DoWhen(d => d.Dispose());
        }

        /// <summary>
        /// Adds a disposable object into a <see cref="CompositeDisposable"/> collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="disposables"></param>
        /// <returns></returns>
        public static T DisposableBy<T>(this T item, CompositeDisposable disposables)
            where T : IDisposable
        {
            disposables.Add(item);
            return item;
        }

        /// <summary>
        /// Adds an object into a <see cref="CompositeDisposable"/> collection if it's not null and implements
        /// <see cref="IDisposable"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="disposables"></param>
        /// <returns></returns>
        public static T SafeDisposableBy<T>(this T item, CompositeDisposable disposables)
        {
            (item as IDisposable)
                .DoWhen(d => disposables.Add(d));

            return item;
        }
    }
}
