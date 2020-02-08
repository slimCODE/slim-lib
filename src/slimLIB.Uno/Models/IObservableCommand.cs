using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace slimCODE.Models
{
    public interface IObservableCommand<TParam, TOutput> : ICommand, IObserver<TParam>, IObservable<TOutput>
    {
    }

    public interface IObservableCommand<T> : IObservableCommand<T, T>
    {
    }
}
