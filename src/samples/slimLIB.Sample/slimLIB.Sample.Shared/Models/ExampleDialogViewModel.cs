using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using slimCODE.Extensions;
using slimCODE.Models;
using Windows.UI.Core;

namespace slimLIB.Sample.Models
{
    public class ExampleDialogViewModel : BaseChildViewModel
    {
        public ExampleDialogViewModel(IObservable<Unit> resetCountdown, BaseViewModel parent)
            : base (parent)
        {
            // Since this view-model is created as a child of another, its state will depend on its parent.
            // If we were to create the "dialog" view-model when it gets displayed, we could use this.ObserveState.
            var countdown = this.CreateProperty(
                "Countdown",
                () => resetCountdown
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
                    0,
                    // A ContentDialog does not honor a command's CanExecute tied to its buttons.
                    // We need another property to bind to IsPrimaryButtonEnabled and co.
                    includeCanProperty: true));
        }
    }
}
