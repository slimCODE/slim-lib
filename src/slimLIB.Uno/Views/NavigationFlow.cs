using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slimCODE.Views
{
    public class NavigationFlow<TState>
        where TState : struct
    {
        public NavigationFlow(TState state, Action navigateAction, params TState[] permittedAncestors)
        {
            this.State = state;
            this.PermittedAncestors = permittedAncestors;
            this.NavigateAction = navigateAction;
        }

        public Action NavigateAction { get; }
        public TState State { get; }
        public TState[] PermittedAncestors { get; }
    }
}
