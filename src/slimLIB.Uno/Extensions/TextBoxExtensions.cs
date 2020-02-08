using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace slimCODE.Extensions
{
    /// <summary>
    /// Provides extension methods on <see cref="TextBox"/> instances.
    /// </summary>
    public static partial class TextBoxExtensions
    {
        /// <summary>
        /// Observes the <see cref="TextBox.TextChanged"/> event.
        /// </summary>
        /// <param name="textBox"></param>
        /// <returns></returns>
        public static IObservable<Unit> ObserveTextChanged(this TextBox textBox)
        {
            return Observable
                .FromEventPattern<TextChangedEventHandler, TextChangedEventArgs>(
                    h => textBox.TextChanged += h,
                    h => textBox.TextChanged -= h)
                .SelectUnit();
        }
    }
}
