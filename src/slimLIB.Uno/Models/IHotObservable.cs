using System;
using System.Collections.Generic;
using System.Text;

namespace slimCODE.Models
{
    /// <summary>
    /// Marker interface to identify observables that produce a value on subscription.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHotObservable<T> : IObservable<T>
    {
    }
}
