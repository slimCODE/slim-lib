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
    public class CreatureViewModel : BaseViewModel
    {
        private readonly IObservableValue<int> _power;
        private readonly IObservableValue<int> _toughness;
        private readonly IObservableValue<Ability[]> _abilities;

        public CreatureViewModel(IObservable<Unit> untapAll, int power, int toughness, params Ability[] abilities)
        {
            _power = this.CreateProperty("Power", power);
            _toughness = this.CreateProperty("Toughness", toughness);
            _abilities = this.CreateProperty("Abilities", abilities);

            this.CreateActiveProperty<bool>(
                "IsTapped",
                "Toggle",
                "Tap",
                (toggle, tap) => Observable
                    .Merge<bool?>(
                        untapAll.Select(_ => false as bool?),
                        toggle.Select(_ => default(bool?)),
                        tap.Select(_ => true as bool?))
                    .Scan(false, (previous, isTapped) =>
                        isTapped.HasValue ? isTapped.Value : !previous),
                false);
        }

        public override string ToString()
        {
            return $"{_power.Latest}/{_toughness.Latest} {string.Join("+", _abilities.Latest)}";
        }
    }
}
