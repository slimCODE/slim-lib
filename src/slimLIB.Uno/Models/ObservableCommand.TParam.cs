using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace slimCODE.Models
{
    public class ObservableCommand<TParam> : ObservableCommand<TParam, TParam>, IObservableCommand<TParam>
    {
        public ObservableCommand()
            : base(PassThru)
        {
        }

        public ObservableCommand(IObservable<bool> canExecuteObservable)
            : base(PassThru, canExecuteObservable)
        {
        }

        public ObservableCommand(Func<TParam, bool> canExecuteFunc, IObservable<Unit> canExecuteChangedObservable = null)
            : base(PassThru, canExecuteFunc, canExecuteChangedObservable)
        {
        }

        private static Task<TParam> PassThru(CancellationToken ct, TParam parameter)
        {
            return Task.FromResult(parameter);
        }
    }
}
