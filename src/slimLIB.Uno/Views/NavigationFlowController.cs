using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using slimCODE.Extensions;
using Windows.UI.Xaml.Controls;

namespace slimCODE.Views
{
    public class NavigationFlowController<TState> : INavigationFlowController<TState>
        where TState : struct
    {
        private readonly INavigationController _navigationController;
        private readonly Dictionary<TState, NavigationFlow<TState>> _flows = new Dictionary<TState, NavigationFlow<TState>>();

        public NavigationFlowController(INavigationController navigationController)
        {
            _navigationController = navigationController;
        }

        public void AddState<TView>(TState state, params TState[] permittedAncestors)
            where TView : Control
        {
            _flows.Add(
                state,
                new NavigationFlow<TState>(
                    state,
                    () => _navigationController.Navigate<TView>(),
                    permittedAncestors));

            BaseViewModelExtensions.CreateGlobalCommand(
                typeof(TState).Name + "_" + state.ToString(),
                async ct => _navigationController.Navigate<TView>(),
                // TODO: Observe navigation stack.
                Observable.Return(true));
        }

        public void GoToState(TState state)
        {
            var flow = _flows.GetOrDefault(state);

            if (flow == null)
            {
                throw new NotSupportedException("This navigation flow state was not registered with the flow controller.");
            }

            flow.NavigateAction();
        }

        public IObservable<NavigationState<TState>> ObserveCurrentState()
        {
            throw new NotImplementedException();
        }
    }
}
