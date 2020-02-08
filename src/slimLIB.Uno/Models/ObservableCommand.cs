using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using slimCODE.Extensions;
using slimCODE.Validation;

namespace slimCODE.Models
{
    public class ObservableCommand<TParam, TResult> : IObservableCommand<TParam, TResult>, IDisposable
    {
        private readonly Subject<TParam> _input = new Subject<TParam>();
        private readonly CompositeDisposable _subscriptions = new CompositeDisposable();

        private readonly IObservable<TResult> _output;

        private Func<TParam, bool> _canExecuteFunc;

        public ObservableCommand()
        {
            _output = _input
                .Select(input => (TResult)Convert.ChangeType(input, typeof(TResult)))
                .Publish()
                .RefCount();
        }

        public ObservableCommand(Func<CancellationToken, TParam, Task<TResult>> processing)
        {
            _output = _input
                .SelectManyCancelPrevious(processing)
                .Publish()
                .RefCount();
        }

        public ObservableCommand(IObservable<bool> canExecuteObservable)
            : this()
        {
            SubscribeToCanExecute(canExecuteObservable.Validate(nameof(canExecuteObservable)).IsNotNull().Value);
        }

        public ObservableCommand(Func<CancellationToken, TParam, Task<TResult>> processing, IObservable<bool> canExecuteObservable)
            : this(processing)
        {
            SubscribeToCanExecute(canExecuteObservable.Validate(nameof(canExecuteObservable)).IsNotNull().Value);
        }

        public ObservableCommand(Func<TParam, bool> canExecuteFunc, IObservable<Unit> canExecuteChangedObservable = null)
            : this()
        {
            _canExecuteFunc = canExecuteFunc.Validate(nameof(canExecuteFunc)).IsNotNull().Value;

            if (canExecuteChangedObservable != null)
            {
                SubscribeToCanExecuteChanged(canExecuteChangedObservable);
            }
        }

        public ObservableCommand(Func<CancellationToken, TParam, Task<TResult>> processing, Func<TParam, bool> canExecuteFunc, IObservable<Unit> canExecuteChangedObservable = null)
            : this(processing)
        {
            _canExecuteFunc = canExecuteFunc.Validate(nameof(canExecuteFunc)).IsNotNull().Value;

            if (canExecuteChangedObservable != null)
            {
                SubscribeToCanExecuteChanged(canExecuteChangedObservable);
            }
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecuteFunc?.Invoke((TParam)parameter) ?? true;
        }

        public void Dispose()
        {
            _subscriptions.Dispose();
            _input.Dispose();
        }

        public void Execute(object parameter)
        {
            // TODO: CommandContext will be responsible to handle lifetime of this execution.
            //       We could auto-dispose previous here.

            // The model is not about observing events on specific threads, but making sure
            // we fire events on such specific background threads.
            Task.Run(() => _input.OnNext(this.ConvertToParameter(parameter)));
        }

        public void OnNext(TParam value)
        {
            _input.OnNext(value);
        }

        public void OnCompleted()
        {
            throw new NotSupportedException("You cannot complete an ObservableCommand.");
        }

        public void OnError(Exception error)
        {
            //Logs.UseLogger(l => l.ReportError("An ObservableCommand was set on error", error));
            throw new NotSupportedException("You cannot set on error an ObservableCommand.");
        }

        public IDisposable Subscribe(IObserver<TResult> observer)
        {
            return _output.Subscribe(observer);
        }

        private TParam ConvertToParameter(object parameter)
        {
            var type = typeof(TParam);

            if (type == typeof(Unit))
            {
                return default(TParam);
            }

            return (TParam)Convert.ChangeType(parameter, type);
        }

        private void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
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
                    _canExecuteFunc = new Func<TParam, bool>(_ => canExecute);
                    this.RaiseCanExecuteChanged();
                })
                .DisposableBy(_subscriptions);
        }
    }
}
