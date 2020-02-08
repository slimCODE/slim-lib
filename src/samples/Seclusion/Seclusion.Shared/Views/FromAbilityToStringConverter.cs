using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seclusion.Models;
using Windows.UI.Xaml.Data;

namespace Seclusion.Views
{
    public class FromAbilityToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return "";
            }

            switch((Ability)value)
            {
                case Ability.Deathtouch:
                    return "Death Touch";

                case Ability.Evolve:
                    return "Evolve";

                case Ability.FirstStrike:
                    return "First Strike";

                case Ability.Flying:
                    return "Flying";

                case Ability.Hexproof:
                    return "Hexproof";

                case Ability.Indestructible:
                    return "Indestructible";

                case Ability.Lifelink:
                    return "Lifelink";

                case Ability.Vigilence:
                    return "Vigilence";

                default:
                    return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
