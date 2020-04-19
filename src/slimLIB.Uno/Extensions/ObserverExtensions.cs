using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

namespace slimCODE.Extensions
{
    /// <summary>
    /// Provides extension methods on <see cref="IObserver{T}"/> instances.
    /// </summary>
    public static partial class ObserverExtensions
    {
        /// <summary>
        /// Pushes <see cref="Unit.Default"/> in the provided observer. 
        /// </summary>
        /// <param name="observer"></param>
        public static void OnNext(this IObserver<Unit> observer)
        {
            observer.OnNext(Unit.Default);
        }
    }
}
