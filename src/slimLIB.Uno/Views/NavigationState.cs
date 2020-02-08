using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slimCODE.Views
{
    public class NavigationState<TState>
        where TState : struct
    {
        public NavigationState(TState current, TState[] ancestors)
        {
            this.Current = current;
            this.Ancestors = ancestors;
        }

        public TState Current { get; }
        public TState[] Ancestors { get; }
    }
}
