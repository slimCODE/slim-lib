using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using slimCODE.Extensions;
using slimCODE.Validation;

namespace slimCODE.Models
{
    /// <summary>
    /// Represents the default <see cref="IAsyncCommand"/> implementation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AsyncCommand<T> : IAsyncCommand, IDisposable
    {
        private readonly Func<CancellationToken, T, Task> _asyncAction;
        private readonly ObservableValue<bool> _isExecutingOV = new ObservableValue<bool>(false);
        private readonly SerialDisposable _executionContext = new SerialDisposable();
        private readonly CompositeDisposable _subscriptions = new CompositeDisposable();

        private Func<T, bool> _canExecuteFunc;

        /// <summary>
        /// Creates an <see cref="AsyncCommand{T}"/> from an async action.
        /// </summary>
        /// <param name="asyncAction"></param>
        public AsyncCommand(Func<CancellationToken, T, Task> asyncAction)
        {
            _asyncAction = asyncAction.Validate(nameof(asyncAction)).IsNotNull().Value;

            // TODO: Subscribe only when CanExecuteWhenExecuting is false
            this.SubscribeToCanExecuteChanged(_isExecutingOV.Skip(1).DistinctUntilChanged().SelectUnit());
        }

        /// <summary>
        /// Creates an <see cref="AsyncCommand{T}"/> from an async action, a function that tells if the command
        /// can be executed, and an optional <see cref="IObservable{Unit}"/> that fires when that function must be re-evaluated.
        /// </summary>
        /// <param name="asyncAction"></param>
        /// <param name="canExecuteFunc"></param>
        /// <param name="canExecuteChangedObservable"></param>
        public AsyncCommand(Func<CancellationToken, T, Task> asyncAction, Func<T, bool> canExecuteFunc, IObservable<Unit> canExecuteChangedObservable = null)
            : this(asyncAction)
        {
            _canExecuteFunc = canExecuteFunc.Validate(nameof(canExecuteFunc)).IsNotNull().Value;

            if (canExecuteChangedObservable != null)
            {
                this.SubscribeToCanExecuteChanged(canExecuteChangedObservable);
            }
        }

        /// <summary>
        /// Creates an <see cref="AsyncCommand{T}"/> from an async action and an <see cref="IObservable{Boolean}"/>
        /// that fires whenever the command can or cannot be executed.
        /// </summary>
        /// <param name="asyncAction"></param>
        /// <param name="canExecuteObservable"></param>
        public AsyncCommand(Func<CancellationToken, T, Task> asyncAction, IObservable<bool> canExecuteObservable)
            : this(asyncAction)
        {
            canExecuteObservable.Validate("canExecuteObservable").IsNotNull();

            this.SubscribeToCanExecute(canExecuteObservable);
        }

        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Determines if this command can be executed when already executing.
        /// </summary>
        public bool CanExecuteWhenExecuting { get; set; }

        /// <summary>
        /// Gets an <see cref="IObservable{Boolean}"/> that reports when the command is executing or not.
        /// </summary>
        public IObservable<bool> IsExecuting
        {
            get { return _isExecutingOV; }
        }

        private void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged
                .DoWhen(handlers => handlers(this, EventArgs.Empty));
        }

        /// <inheritdoc/>
        public bool CanExecute(object parameter)
        {
            return (this.CanExecuteWhenExecuting || !_isExecutingOV.Latest)
                && _canExecuteFunc.SelectWhen(func => func((T)parameter), () => true);
        }

        /// <inheritdoc/>
        public void Execute(object parameter)
        {
            var cancellationSource = new CancellationTokenSource();

            _executionContext.Disposable = new CompositeDisposable(
                Disposable.Create(() => cancellationSource.Cancel()),
                cancellationSource);

            Task.Run(() => this.InnerExecute(cancellationSource.Token, (T)parameter), cancellationSource.Token);
        }

        public void Dispose()
        {
            _isExecutingOV.Dispose();
            _executionContext.Dispose();
            _subscriptions.Dispose();
        }

        private async Task InnerExecute(CancellationToken ct, T parameter)
        {
            try
            {
                _isExecutingOV.OnNext(true);

                await _asyncAction(ct, parameter);
            }
            finally
            {
                _isExecutingOV.OnNext(false);
            }
        }

        private void SubscribeToCanExecuteChanged(IObservable<Unit> canExecuteChangedObservable)
        {
            canExecuteChangedObservable
                .Subscribe(_ => this.RaiseCanExecuteChanged())
                .DisposableBy(_subscriptions);
        }

        private void SubscribeToCanExecute(IObservable<bool> canExecuteObservable)
        {
            canExecuteObservable
                .Subscribe(canExecute =>
                {
                    _canExecuteFunc = new Func<T, bool>(_ => canExecute);
                    this.RaiseCanExecuteChanged();
                })
                .DisposableBy(_subscriptions);
        }
    }

    /// <summary>
    /// Represents the default <see cref="IAsyncCommand"/> implementation that ignores any parameter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AsyncCommand : AsyncCommand<object>
    {
        /// <summary>
        /// Creates an <see cref="AsyncCommand"/> that doesn't execute anything.
        /// </summary>
        public AsyncCommand()
            : base(async (ctor, parameter) => { })
        {
        }

        /// <summary>
        /// Creates an <see cref="AsyncCommand"/> from an async action.
        /// </summary>
        /// <param name="asyncAction"></param>
        public AsyncCommand(Func<CancellationToken, Task> asyncAction)
            : base(new Func<CancellationToken, object, Task>((ct, obj) => asyncAction(ct)))
        {
        }

        /// <summary>
        /// Creates an <see cref="AsyncCommand"/> from an async action and an <see cref="IObservable{Boolean}"/>
        /// that fires whenever the command can or cannot be executed.
        /// </summary>
        /// <param name="asyncAction"></param>
        /// <param name="canExecuteObservable"></param>
        public AsyncCommand(Func<CancellationToken, Task> asyncAction, IObservable<bool> canExecuteObservable)
            : base(new Func<CancellationToken, object, Task>((ct, obj) => asyncAction(ct)), canExecuteObservable)
        {
        }
    }
}
