using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace slimCODE.Views.Converters
{
    public class FromBoolToIntConverter : IValueConverter
    {
        public int NullValue { get; set; }
        public int FalseValue { get; set; }
        public int TrueValue { get; set; }

        public int NullOrFalseValue
        {
            get { return this.FalseValue; }
            set { this.NullValue = this.FalseValue = value; }
        }

        public int NullOrTrueValue
        {
            get { return this.TrueValue; }
            set { this.NullValue = this.TrueValue = value; }
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value == null)
                ? this.NullValue
                : (bool)value ? this.TrueValue : this.FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
