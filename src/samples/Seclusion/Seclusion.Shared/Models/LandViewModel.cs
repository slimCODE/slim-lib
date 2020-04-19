using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using slimCODE.Extensions;
using slimCODE.Models;

namespace Seclusion.Models
{
    public class LandViewModel : BaseChildViewModel
    {
        public LandViewModel(Func<IObservable<Unit>> untapAllSelector, BaseViewModel parent)
            : base (parent)
        {
            this.IsTappedObservable = this.CreateActiveProperty<bool>(
                "IsTapped",
                "Toggle",
                "Tap",
                (toggle, tap) => Observable
                    .Merge<bool?>(
                        untapAllSelector().Select(_ => false as bool?),
                        toggle.Select(_ => default(bool?)),
                        tap.Select(_ => true as bool?))
                    .Scan(false, (previous, isTapped) => 
                        isTapped.HasValue ? isTapped.Value : !previous),
                false);
        }

        public IObservable<bool> IsTappedObservable { get; }

        public void Tap()
        {
            (this["Tap"] as IObservableCommand<Unit>).OnNext(Unit.Default);
        }
    }
}
