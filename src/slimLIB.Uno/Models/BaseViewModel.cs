using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using slimCODE.Extensions;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace slimCODE.Models
{
    /// <summary>
    /// Base implementation of any view-model when using slimLIB.
    /// </summary>
    public abstract class BaseViewModel : BaseNotifyPropertyChanged, IDisposable
    {
        private static readonly ObservableValues __globalValues = new ObservableValues();

        private readonly ObservableValues _localValues;

        private readonly ObservableValue<ViewModelState> _state = new ObservableValue<ViewModelState>(ViewModelState.NotLoaded);

        private readonly CompositeDisposable _subscriptions = new CompositeDisposable();
        private readonly CompositeDisposable _loadedSubscriptions = new CompositeDisposable();

        protected BaseViewModel()
        {
            _localValues = new ObservableValues(__globalValues, OnDesignTimePropertyRequested);
        }

        protected BaseViewModel(CoreDispatcher dispatcher)
            : base(dispatcher)
        {
            _localValues = new ObservableValues(__globalValues);
        }

        protected BaseViewModel(BaseViewModel parent)
            : this()
        {
            parent.AddChild(this);
        }

        protected CompositeDisposable LoadedSubscriptions
        {
            get { return _loadedSubscriptions; }
        }

        /// <summary>
        /// Gets the latest state of this view-model.
        /// </summary>
        public ViewModelState State
        {
            get { return _state.Latest; }
        }

        protected CompositeDisposable Subscriptions
        {
            get { return _subscriptions; }
        }

        /// <summary>
        /// Observes the state of this view-model.
        /// </summary>
        /// <returns></returns>
        public IObservable<ViewModelState> ObserveState()
        {
            return _state.AsObservable();
        }

        /// <summary>
        /// Activates this view-model, connecting it with a <see cref="CoreDispatcher"/>.
        /// </summary>
        /// <param name="dispatcher"></param>
        public void Activate(CoreDispatcher dispatcher)
        {
            this.Dispatcher = dispatcher;

            ObserveValues();
            _state.OnNext(ViewModelState.Loaded);
        }

        /// <summary>
        /// Deactivates this view-model, disposing any subscriptions that depends on it being loaded.
        /// </summary>
        public void Deactivate()
        {
            _loadedSubscriptions.Clear();
            _state.OnNext(ViewModelState.Unloaded);
        }

        /// <summary>
        /// Makes an <see cref="ObservableValue{T}"/> accessible via data-binding.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="observableValue"></param>
        public void AddProperty<T>(string name, ObservableValue<T> observableValue)
        {
            _localValues.AddValue(name, observableValue);
        }

        /// <summary>
        /// Makes an <see cref="ObservableValue{T}"/> accessible via data-binding to
        /// any control.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="observableValue"></param>
        public static void AddGlobalProperty<T>(string name, ObservableValue<T> observableValue)
        {
            __globalValues.AddValue(name, observableValue);
        }

        /// <summary>
        /// Adds a reaction that depends on this view-model entering a specified <see cref="ViewModelState"/>.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public IDisposable AddTrigger(ViewModelState state, Action action)
        {
            var subscription = _state
                .Where(s => s == state)
                .OnBackground()
                .Subscribe(
                    _ => action.VerifyThread(this.Dispatcher, false),
                    error => /*Logs.UseLogger(l => l.ReportError("Observing state for trigger.", error))*/ { });

            _subscriptions.Add(subscription);

            return Disposable.Create(() => _subscriptions.Remove(subscription));
        }

        /// <summary>
        /// Adds a view-model as a child of this view-model. It becomes connected to the parent state.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="childViewModel"></param>
        /// <returns></returns>
        public TViewModel AddChild<TViewModel>(TViewModel childViewModel)
            where TViewModel : BaseViewModel
        {
            childViewModel.Subscriptions.Add(
                this.AddTrigger(
                    ViewModelState.Loaded,
                    () => childViewModel.Activate(this.Dispatcher)));

            childViewModel.Subscriptions.Add(
                this.AddTrigger(
                    ViewModelState.Unloaded,
                    () => childViewModel.Deactivate()));

            return childViewModel;
        }

        /// <summary>
        /// Adds a view-model as a child of this view-model, and exposes it data-binding as a property.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="childViewModel"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public TViewModel AddChildProperty<TViewModel>(string name, TViewModel childViewModel)
            where TViewModel : BaseViewModel
        {
            // Note: Though this could have been implemented as a simple extension, I prefer to keep this
            //       local, in case there's any specificities later.
            this.AddProperty(name, new ObservableValue<TViewModel>(this.AddChild(childViewModel)));

            return childViewModel;
        }

        /// <summary>
        /// Gets or sets the latest value of a data-bound <see cref="ObservableValue{T}"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [IndexerName("Item")] // Technically not required, but cleaner.
        public object this[string name]
        {
            get { return _localValues.GetValue(name); }
            set { _localValues.SetValue(name, value); }
        }

        public void Dispose()
        {
            _localValues.Dispose();
        }

        protected void EnsureActivatedInDesignTime()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                this.Activate(Windows.UI.Xaml.Window.Current.Dispatcher);
            }
        }

        protected virtual object OnDesignTimePropertyRequested(string name)
        {
            return null;
        }

        private void ObserveValues()
        {
            _loadedSubscriptions.Add(
                _localValues.Subscribe(
                    propertyName => this.OnPropertyChanged("Item[" + propertyName + "]"))
            );

            _loadedSubscriptions.Add(
                __globalValues.Subscribe(
                    propertyName => this.OnPropertyChanged("Item[" + propertyName + "]"))
            );
        }
    }
}
