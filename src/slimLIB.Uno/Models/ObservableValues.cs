using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Text;

namespace slimCODE.Models
{
    /// <summary>
    /// Holds a list of <see cref="ObservableValue{T}"/> accessible by name.
    /// </summary>
    public class ObservableValues : IObservable<string>, IDisposable
    {
        private readonly ObservableValues _alternateValues;

        private readonly Dictionary<string, Func<object>> _valueGetters = new Dictionary<string, Func<object>>();
        private readonly Dictionary<string, Action<object>> _valueSetters = new Dictionary<string, Action<object>>();

        private Func<string, object> _designTimeHandler;

        private CompositeDisposable _subscriptions = new CompositeDisposable();
        private readonly Subject<string> _propertyChanged = new Subject<string>();

        public ObservableValues(Func<string, object> designTimeHandler = null)
        {
            _designTimeHandler = designTimeHandler;
        }

        public ObservableValues(ObservableValues alternateValues, Func<string, object> designTimeHandler = null)
            : this(designTimeHandler)
        {
            _alternateValues = alternateValues;
        }

        public void AddValue<T>(string name, ObservableValue<T> observableValue)
        {
            _valueGetters.Add(name, () => observableValue.Latest);
            _valueSetters.Add(name, value => observableValue.OnNext((T)value));

            _subscriptions.Add(observableValue.Subscribe(_ => _propertyChanged.OnNext(name)));
        }

        public object GetValue(string name)
        {
            if ((_designTimeHandler != null) && Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                return _designTimeHandler(name);
            }

            if (_valueGetters.TryGetValue(name, out Func<object> getter))
            {
                return getter();
            }
            else if (_alternateValues != null)
            {
                return _alternateValues.GetValue(name);
            }
            else
            {
                //Logs.UseLogger(l => l.ReportError($"Bound property {name} not found."));
                return null;
            }
        }

        public void SetValue(string name, object value)
        {
            if (_valueSetters.TryGetValue(name, out Action<object> setter))
            {
                setter(value);
            }
            else if (_alternateValues != null)
            {
                _alternateValues.SetValue(name, value);
            }
            else
            {
                throw new ArgumentException("Unknown value name.");
            }
        }

        public IDisposable Subscribe(IObserver<string> observer)
        {
            return _propertyChanged.Subscribe(observer);
        }

        public void Dispose()
        {
            _propertyChanged.Dispose();
            _subscriptions.Dispose();
        }
    }
}
