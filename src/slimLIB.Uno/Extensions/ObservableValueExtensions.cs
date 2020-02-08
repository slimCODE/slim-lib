using slimCODE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slimCODE.Extensions
{
    /// <summary>
    /// Provides extension methods on <see cref="ObservableValue{T}"/> instances.
    /// </summary>
    public static partial class ObservableValueExtensions
    {
        /// <summary>
        /// Observes the <see cref="IAsyncCommand.IsExecuting"/> of an <see cref="ObservableValue{IAsyncCommand}"/>
        /// and produces a unit every time it passes from true to false.
        /// </summary>
        /// <param name="observableCommand"></param>
        /// <returns></returns>
        public static IObservable<Unit> ObserveCommandCompleted(this ObservableValue<IAsyncCommand> observableCommand)
        {
            return observableCommand
                .Latest
                .IsExecuting
                .DistinctUntilChanged()
                .Skip(1)
                .WhereFalse()
                .SelectUnit();
        }
    }
}
