using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace slimCODE.Extensions
{
    /// <summary>
    /// Conatins extension methods on any kind of objects.
    /// </summary>
    public static partial class ObjectExtensions
    {
        /// <summary>
        /// Executes one of two actions depending on if the object is null or not.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="whenNotNull"></param>
        /// <param name="whenNull"></param>
        public static void DoWhen<TSource>(this TSource source, Action<TSource> whenNotNull, Action whenNull = null)
            where TSource : class
        {
            if (source == null)
            {
                whenNull?.Invoke();
            }
            else
            {
                whenNotNull?.Invoke(source);
            }
        }

        /// <summary>
        /// Performs a selection from one of two functions depending on if the object is null or not.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="whenNotNull"></param>
        /// <param name="whenNull"></param>
        /// <returns></returns>
        public static TResult SelectWhen<TSource, TResult>(this TSource source, Func<TSource, TResult> whenNotNull, Func<TResult> whenNull = null)
            where TSource : class
        {
            return (source == null)
                ? (whenNull == null)
                    ? default
                    : whenNull()
                : (whenNotNull == null)
                    ? default
                    : whenNotNull(source);
        }

        /// <summary>
        /// Determines if an item is equal to another item in a list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool IsOneOf<T>(this T item, params T[] values)
        {
            return values.Any(value => value.Equals(item));
        }
    }
}
