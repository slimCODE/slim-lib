using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class LandsViewModel : BaseChildViewModel
    {
        public LandsViewModel(
            Func<IObservable<Unit>> resetSelector, 
            Func<IObservable<Unit>> untapAllSelector, 
            Func<IObservable<Unit>> playLandSelector,
            BaseViewModel parent)
            : base (parent)
        {
            var tapLands = this.CreateObservableCommand<int>("TapLands");

            this.LandsObservable = this.CreateCollectionProperty<LandViewModel>(
                "Lands",
                proxy => Observable
                    .Merge(
                        resetSelector().Do(_ => proxy.Clear()),
                        playLandSelector().Do(_ => proxy.Add(this.AddChild(new LandViewModel(untapAllSelector, this)))),
                        this.ObserveTapLands(tapLands)
                    )
            );
        }

        public IObservable<ObservableCollection<LandViewModel>> LandsObservable { get; }

        public IObservable<IEnumerable<LandViewModel>> UntappedLandsObservable
        {
            get
            {
                return this.LandsObservable
                    .WhereNotNull()
                    .SelectManyCancelPrevious(lands => lands
                        .ObserveChanged()
                        .SelectManyCancelPrevious(_ =>
                            Observable.CombineLatest(
                                lands.Select(land => land
                                    .IsTappedObservable
                                    .Select(isTapped => isTapped ? null : land))))
                        .Select(untappedLands => untappedLands.WhereNotNull()));
            }
        }

        public void TapLands(int count)
        {
            (this["TapLands"] as IObservableCommand<int>).OnNext(count);
        }

        private IObservable<Unit> ObserveTapLands(IObservable<int> tapLands)
        {
            return this.UntappedLandsObservable
                .SelectManyCancelPrevious(lands => tapLands
                    .Do(count => lands.Take(count).ForEach(land => land.Tap())))
                .SelectUnit();
        }
    }
}
