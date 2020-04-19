using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace slimCODE.Extensions
{
    /// <summary>
    /// Provides extension methods on <see cref="IDictionary{TKey, TValue}"/> instances.
    /// </summary>
    public static partial class DictionaryExtensions
    {
        /// <summary>
        /// Gets a value in a dictionary, or a default value when not found.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value = default(TValue))
        {
            dictionary.TryGetValue(key, out value);
            return value;
        }

        /// <summary>
        /// Gets a value in a dictionary when found, or adds a new value if not found.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="valueFactory"></param>
        /// <returns></returns>
        public static TValue GetOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> valueFactory)
        {
            if(!dictionary.TryGetValue(key, out var value))
            {
                dictionary.Add(key, value = valueFactory(key));
            }

            return value;
        }

        /// <summary>
        /// Gets and updates a value in a dictionary when found, otherwise adding a default value from another function.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="updateFunc"></param>
        /// <param name="initialValueFunc"></param>
        /// <returns></returns>
        public static TValue GetAndUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue, TValue> updateFunc, Func<TKey, TValue> initialValueFunc = null)
        {
            if (!dictionary.TryGetValue(key, out var value))
            {
                dictionary.Add(key, value = (initialValueFunc == null ? default : initialValueFunc(key)));
            }
            else
            {
                value = dictionary[key] = updateFunc(key, value);
            }

            return value;
        }

        /// <summary>
        /// Gets and updates a value in a dictionary when found, otherwise adding a default value.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="updateFunc"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TValue GetAndUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue, TValue> updateFunc, TValue defaultValue = default)
        {
            if (!dictionary.TryGetValue(key, out var value))
            {
                dictionary.Add(key, value = defaultValue);
            }
            else
            {
                value = dictionary[key] = updateFunc(key, value);
            }

            return value;
        }

        /// <summary>
        /// Performs a selection of either a found value in a dictionary, or a default selector.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="whenFound"></param>
        /// <param name="whenNotFound"></param>
        /// <returns></returns>
        public static TResult SelectOrDefault<TKey, TValue, TResult>(
            this IDictionary<TKey, TValue> dictionary, 
            TKey key, 
            Func<TValue, TResult> whenFound,
            Func<TResult> whenNotFound = null)
        {
            TValue value = default(TValue);

            if(dictionary.TryGetValue(key, out value))
            {
                return whenFound(value);
            }
            else if(whenNotFound == null)
            {
                return default(TResult);
            }
            else
            {
                return whenNotFound();
            }
        }

        /// <summary>
        /// Executes either an action with a found value in a dictionary, or a parameterless action when not found.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="whenFound"></param>
        /// <param name="whenNotFound"></param>
        public static void Do<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            Action<TValue> whenFound,
            Action whenNotFound = null)
        {
            TValue value = default(TValue);

            if (dictionary.TryGetValue(key, out value))
            {
                whenFound(value);
            }
            else if (whenNotFound != null)
            {
                whenNotFound();
            }
        }

        /// <summary>
        /// Eexcutes either an async action with a found value in a dictionary, 
        /// or a parameterless async action when not found.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="ct"></param>
        /// <param name="key"></param>
        /// <param name="whenFound"></param>
        /// <param name="whenNotFound"></param>
        /// <returns></returns>
        public static async Task Do<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            CancellationToken ct,
            TKey key,
            Func<CancellationToken, TValue, Task> whenFound,
            Func<CancellationToken, Task> whenNotFound = null)
        {
            TValue value = default(TValue);

            if (dictionary.TryGetValue(key, out value))
            {
                await whenFound(ct, value);
            }
            else if (whenNotFound != null)
            {
                await whenNotFound(ct);
            }
        }
    }
}
