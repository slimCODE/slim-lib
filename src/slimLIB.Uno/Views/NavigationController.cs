using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using slimCODE.Extensions;
using slimCODE.Models;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace slimCODE.Views
{
    public class NavigationController : INavigationController, IDisposable
    {
        private readonly IViewModelController _viewModelController;
        private readonly SerialDisposable _frameSubscriptions = new SerialDisposable();
        private readonly Subject<Tuple<IObservable<bool>, Func<CancellationToken, Task>>> _addedInterceptors = new Subject<Tuple<IObservable<bool>, Func<CancellationToken, Task>>>();
        private readonly Subject<Tuple<IObservable<bool>, Func<CancellationToken, Task>>> _removedInterceptors = new Subject<Tuple<IObservable<bool>, Func<CancellationToken, Task>>>();

        private Frame _frame;
        private SystemNavigationManager _navigationManager;

        public NavigationController(IViewModelController viewModelController)
        {
            _viewModelController = viewModelController;
        }

        public void ConnectWithFrame(Frame rootFrame)
        {
            _frame = rootFrame;
            _navigationManager = SystemNavigationManager.GetForCurrentView();

            _frameSubscriptions.Disposable = new CompositeDisposable(
                SubscribeFrameNavigating(rootFrame),
                SubscribeFrameNavigated(rootFrame),
                SubscribeBackRequested(_navigationManager)
            );
        }

        public void Navigate(Type viewType)
        {
            this.Navigate(viewType, null);
        }

        public void Navigate(Type viewType, object argument)
        {
#pragma warning disable CS4014
            _frame.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                () => _frame?.Navigate(viewType, argument));
#pragma warning restore CS4014
        }

        public IDisposable RegisterBackInterceptor(IObservable<bool> observeCanGoBack, Func<CancellationToken, Task> onBackIntercepted)
        {
            var tuple = new Tuple<IObservable<bool>, Func<CancellationToken, Task>>(observeCanGoBack, onBackIntercepted);

            _addedInterceptors.OnNext(tuple);

            return Disposable.Create(() => _removedInterceptors.OnNext(tuple));
        }

        private IDisposable SubscribeFrameNavigating(Frame frame)
        {
            return frame
                .ObserveNavigating()
                .Select(_ => frame.Content as FrameworkElement)
                .WhereNotNull()
                .Do((ct, element) => DiscardViewModel(ct, element))
                .Subscribe(
                    _ => { },
                    error => { });
        }

        private IDisposable SubscribeFrameNavigated(Frame frame)
        {
            return frame
                .ObserveNavigated()
                .SelectManyCancelPrevious((ct, args) => UpdateViewModel(ct, args.Content as FrameworkElement, args.NavigationMode, args.Parameter))
                .SelectManyCancelPrevious((ct, _) =>
                    frame
                        .Dispatcher
                        .RunAsync(
                            CoreDispatcherPriority.Normal,
                            () => _navigationManager.AppViewBackButtonVisibility = frame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed)
                        .AsTask(ct))
                .Subscribe(
                    _ => { },
                    error => /*Logs.UseLogger(l => l.ReportError("Observing Navigated to update the view-model.", error))*/ { });
        }

        private IDisposable SubscribeBackRequested(SystemNavigationManager manager)
        {
            return _addedInterceptors
                .Except(_removedInterceptors)
                .SelectManyCancelPrevious(tuples => tuples
                    .Select(tuple => tuple.Item1.Select(isPreventingBack => isPreventingBack ? tuple.Item2 : null))
                    .CombineLatest())
                .Select(interceptors => interceptors.WhereNotNull().ToArray())
                // TODO: A SelectManyCancelPrevious with IObservable would look better.
                .Select(interceptors => manager
                    .ObserveBackRequested()
                    .Where(_ => _frame.CanGoBack)
                    .Do((ct, _) => Task.WhenAll(interceptors.Select(interceptor => interceptor(ct))))
                    .Where(_ => interceptors.Length == 0))
                .Switch()
                .Subscribe(
                    args =>
                    {
                        args.Handled = true;
                        _frame.GoBack();
                    },
                    error => /*Logs.UseLogger(l => l.ReportError("Observing BackRequested to navigate back.", error))*/ { });
        }

        private async Task DiscardViewModel(CancellationToken ct, FrameworkElement element)
        {
            var viewModel = element.DataContext as BaseViewModel;

            await viewModel.Dispatcher.RunNormalAsync(ct, () =>
            {
                element.DataContext = null;
            });

            _viewModelController.ReleaseViewModel(viewModel, element);
        }

        private async Task UpdateViewModel(CancellationToken ct, FrameworkElement element, NavigationMode mode, object parameter)
        {
            element.Dispatcher.VerifyThread(true);

            var viewModel = _viewModelController.GetViewModel(element, parameter);

            await viewModel.Dispatcher.RunNormalAsync(ct, () =>
            {
                element.DataContext = viewModel;
            });
        }

        public void Dispose()
        {
            _frameSubscriptions.Dispose();
            _addedInterceptors.Dispose();
            _removedInterceptors.Dispose();
        }
    }
}
