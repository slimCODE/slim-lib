using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace slimCODE.Views
{
    public interface INavigationFlowController<TState>
        where TState : struct
    {
        void AddState<TView>(TState state, params TState[] permittedAncestors)
            where TView : Control;

        void GoToState(TState state);

        IObservable<NavigationState<TState>> ObserveCurrentState();
    }
}
