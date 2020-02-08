using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using slimCODE.Extensions;
using slimCODE.Models;

namespace Seclusion.Models
{
    public static class BaseViewModelExtensions
    {
        public static ObservableValue<int> CreateCounterProperty(
            this BaseViewModel viewModel, 
            string propertyName, 
            Func<IObservable<Unit>> reset,
            int initialValue = 0,
            int minimum = 0,
            int? maximum = null)
        {
            return viewModel.CreateActiveProperty(
                propertyName,
                "Increase" + propertyName,
                "Decrease" + propertyName,
                (increase, decrease) => Observable
                    .Merge(
                        reset().Select(_ => 0),
                        increase.Select(_ => 1),
                        decrease.Select(_ => -1))
                    .Scan(initialValue, (value, effect) => (effect == 0)
                        ? initialValue
                        : Math.Min(maximum ?? int.MaxValue, Math.Max(0, value + effect))),
                initialValue);
        }
    }
}
