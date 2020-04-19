using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using slimCODE.Extensions;
using slimCODE.Models;

namespace slimLIB.Sample.Models
{
    public class ExampleDialogViewModel : BaseViewModel
    {
        public ExampleDialogViewModel()
        {
            var countdown = this.CreateProperty(
                "Countdown",
                () => this.ObserveState()
                    .Where(state => state == ViewModelState.Loaded)
                    .SelectManyCancelPrevious(_ => Observable
                        .Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1))
                        .Take(6)
                        .Select(count => 5 - count)));

            // We hate repeating code, right?
            new[] { "Good", "Fine", "Whatever" }
                .ForEach(name => this.CreateActiveProperty(
                    $"{name}Count", 
                    $"{name}Command",                 
                    commandObservable => commandObservable.Select((_, index) => index + 1), 
                    countdown.Select(c => c == 0),
                    0));
        }
    }
}
