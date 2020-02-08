using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using slimCODE.Extensions;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace slimCODE.Views.Converters
{
    [ContentProperty(Name = "Cases")]
    public class NumberSwitchConverter : IValueConverter
    {
        public IList<NumberSwitch> Cases { get; } = new List<NumberSwitch>();
        public object DefaultValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return DefaultValue;
            }

            var intValue = (int)value;

            return Cases
                .FirstOrDefault(item => item.Case == intValue)
                .SelectWhen(item => item.Value, () => DefaultValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
