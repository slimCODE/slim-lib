using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slimCODE.Extensions
{
    /// <summary>
    /// Provides extension methods on different Func instances.
    /// </summary>
    public static class FuncExtensions
    {
        /// <summary>
        /// Returns a new <see cref="Func{T}"/> from a source <see cref="Func{T}"/>, which only calls
        /// the source function once, and caches the result for every subsequent call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="function"></param>
        /// <returns></returns>
        public static Func<T> AsMemoized<T>(this Func<T> function)
        {
            var result = new Lazy<T>(function);

            return () => result.Value;
        }

        /// <summary>
        /// Returns a new <see cref="Func{TParam, TResult}"/> from a source <see cref="Func{TParam, TResult}"/>, 
        /// which only calls the source function once, and caches the result for every subsequent call with the same
        /// parameter.
        /// </summary>
        /// <typeparam name="TParam"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="function"></param>
        /// <returns></returns>
        public static Func<TParam, TResult> AsMemoized<TParam, TResult>(this Func<TParam, TResult> function)
        {
            var seenParameters = new Dictionary<TParam, TResult>();

            return (value) => seenParameters.GetOrUpdate(value, v => function(v));
        }

        /// <summary>
        /// Creates a memoized function from a source function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="function"></param>
        /// <returns></returns>
        public static Func<T> CreateMemoized<T>(Func<T> function)
        {
            return function.AsMemoized();
        }
    }
}
