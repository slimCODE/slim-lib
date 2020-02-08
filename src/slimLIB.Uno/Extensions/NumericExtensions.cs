using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slimCODE.Extensions
{
    /// <summary>
    /// Contains extension methods on any kind of numeric values.
    /// </summary>
    public static partial class NumericExtensions
    {
        /// <summary>
        /// Converts an <see cref="Int32"/> value into a string using <see cref="NumberFormatInfo.InvariantInfo"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToStringInvariant(this int value)
        {
            return value.ToString(NumberFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// Converts an <see cref="Int64"/> value into a string using <see cref="NumberFormatInfo.InvariantInfo"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToStringInvariant(this long value)
        {
            return value.ToString(NumberFormatInfo.InvariantInfo);
        }
    }
}
