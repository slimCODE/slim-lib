using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace slimCODE.Extensions
{
    /// <summary>
    /// Provides extension methods on <see cref="IEnumerable{T}"/> instances.
    /// </summary>
    public static partial class EnumerableExtensions
    {
        /// <summary>
        /// Concatenates a single object to the end of an enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="addedItem"></param>
        /// <returns></returns>
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> enumerable, T addedItem)
        {
            foreach (var item in enumerable)
            {
                yield return item;
            }

            yield return addedItem;
        }

        /// <summary>
        /// Returns an enumerable that skips any instance of an item equal to items in the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="excludedItem"></param>
        /// <returns></returns>
        public static IEnumerable<T> Except<T>(this IEnumerable<T> enumerable, T excludedItem)
        {
            return enumerable
                .Where(item => !item.Equals(excludedItem));
        }

        /// <summary>
        /// Returns an enumerable that inverses every value of an <see cref="IEnumerable{Boolean}"/>.
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static IEnumerable<bool> Negate(this IEnumerable<bool> enumerable)
        {
            return enumerable
                .Select(value => !value);
        }

        /// <summary>
        /// Returns an enumerable that skips any value that is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> enumerable)
            where T : class
        {
            return enumerable
                .Where(value => value != null);
        }

        /// <summary>
        /// Executes an action for every item in an enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }
    }
}
