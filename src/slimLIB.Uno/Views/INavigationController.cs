using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace slimCODE.Views
{
    public interface INavigationController
    {
        void ConnectWithFrame(Frame rootFrame);
        void Navigate(Type viewType);
        void Navigate(Type viewType, object argument);

        IDisposable RegisterBackInterceptor(IObservable<bool> observeCanGoBack, Func<CancellationToken, Task> onBackIntercepted);
    }
}
