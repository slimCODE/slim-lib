using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace slimCODE.Models
{
    /// <summary>
    /// Represents an <see cref="ICommand"/> that can execute asynchroneously.
    /// </summary>
    public interface IAsyncCommand : ICommand
    {
        /// <summary>
        /// Gets an observable that reports when the command is executing.
        /// </summary>
        IObservable<bool> IsExecuting { get; }
    }
}
