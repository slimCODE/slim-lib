using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slimCODE.Extensions
{
    /// <summary>
    /// Provides extension methods on <see cref="String"/> instances.
    /// </summary>
    public static partial class StringExtensions
    {
        /// <summary>
        /// Returns true if a string is not null nor empty.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool HasValue(this string value)
        {
            return (value?.Length > 0);
        }

        /// <summary>
        /// Uses a string as the format parameter of a <see cref="string.Format(IFormatProvider, string, object[])"/>
        /// with the <see cref="CultureInfo.InvariantCulture"/> format provider.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatInvariant(this string format, params object[] args)
        {
            return string.Format(CultureInfo.InvariantCulture, format, args);
        }
    }
}
