using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using slimCODE.Extensions;

namespace slimCODE.Models
{
    public class ObservableValue<T> : IObservableValue<T>, IDisposable
    {
        private readonly CompositeDisposable _subscriptions = new CompositeDisposable();
        private readonly ISubject<Tuple<bool, T>> _changes;
        private T _latest;
        private bool _isCompleted;

        public ObservableValue(T initialValue = default(T))
        {
            _changes = new ReplaySubject<Tuple<bool, T>>(1).DisposableBy(_subscriptions);
            _changes.OnNext(new Tuple<bool, T>(false, _latest = initialValue));
        }

        public T Latest
        {
            get { return _latest; }
        }

        public object Value
        {
            get { return _latest; }
        }

        public static implicit operator T(ObservableValue<T> value)
        {
            return value.Latest;
        }

        public void AddInput(IObservable<T> inputObservable)
        {
            // TODO: Subscribe when we get subscribed. same below.
            _subscriptions.Add(inputObservable
                .Do(value => _latest = value)
                .Subscribe(
                    value => _changes.OnNext(new Tuple<bool, T>(false, value)),
                    error => /*Logs.UseLogger(l => l.ReportError("Observing input", error))*/ { }));
        }

        public void AddInput(Func<IObservable<T>, IObservable<T>> inputObservableSelector)
        {
            _subscriptions.Add(
                inputObservableSelector(
                    _changes
                        .Where(change => change.Item1)
                        .Select(change => change.Item2))
                .Do(value => _latest = value)
                .Subscribe(
                    value => _changes.OnNext(new Tuple<bool, T>(false, value)),
                    error => /*Logs.UseLogger(l => l.ReportError("Observing input", error))*/ { }));

        }

        //public void AddEffect(IObserver<T> effectObserver)
        //{
        //	_subscriptions.Add(_changes.Subscribe(effectObserver));
        //}

        //public void AddEffect(Func<CancellationToken, T, Task> effectTask)
        //{
        //	_subscriptions.Add(
        //		_changes
        //			.Do(effectTask)
        //			.Subscribe());
        //}

        public void OnCompleted()
        {
            _isCompleted = true;
        }

        public void OnError(Exception error)
        {
            throw new NotSupportedException("An ObservableValue can never go into an error state.");
        }

        public void OnNext(T value)
        {
            if (_isCompleted)
            {
                throw new InvalidOperationException("This ObservableValue has already completed.");
            }

            _changes.OnNext(new Tuple<bool, T>(true, _latest = value));
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return _changes.Subscribe(
                change => observer.OnNext(change.Item2),
                error => observer.OnError(error),
                () => observer.OnCompleted());
        }

        public void Dispose()
        {
            _subscriptions.Dispose();
        }
    }
}
