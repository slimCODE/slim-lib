using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace slimCODE.Views.Extensions
{
    public static class StringExtensions
    {
        public static GridLength ToGridLength(this string value)
        {
            value = value?.Trim();

            if ((value == null) || value.Equals("auto", StringComparison.OrdinalIgnoreCase))
            {
                return GridLength.Auto;
            }

            var units = GridUnitType.Pixel;

            if (value.EndsWith("*"))
            {
                units = GridUnitType.Star;
                value = value.TrimEnd('*');
            }

            if (!double.TryParse(value, out var number))
            {
                number = 1;
            }

            return new GridLength(number, units);
        }
    }
}
