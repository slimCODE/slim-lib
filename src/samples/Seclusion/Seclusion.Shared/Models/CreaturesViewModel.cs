using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using slimCODE.Extensions;
using slimCODE.Models;

namespace Seclusion.Models
{
    public class CreaturesViewModel : BaseViewModel
    {
        private Random _random = new Random();

        public CreaturesViewModel(
            Func<IObservable<Unit>> resetSelector, 
            Func<IObservable<CreatureViewModel>> playCreatureSelector)
        {
            this.CreateCollectionProperty<CreatureViewModel>(
                "Creatures",
                proxy => Observable
                    .Merge(
                        playCreatureSelector()
                            .Do(creature => proxy.Add(this.AddChild(creature)))
                            .SelectUnit(),
                        resetSelector()
                            .Do(_ => proxy.Clear())));
        }
    }
}
