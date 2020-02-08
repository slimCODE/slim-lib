using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;

namespace slimCODE.Models
{
    public interface IObservableValue
    {
        object Value { get; }
    }

    public interface IObservableValue<T> : IObservableValue, ISubject<T>, IHotObservable<T>
    {
        T Latest { get; }
    }
}
