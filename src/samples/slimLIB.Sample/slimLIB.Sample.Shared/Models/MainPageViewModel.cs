using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using slimCODE.Extensions;
using slimCODE.Models;
using slimCODE.Views;
using slimLIB.Sample.Views;
using Windows.UI.Xaml.Controls;

namespace slimLIB.Sample.Models
{
    public class MainPageViewModel : BaseViewModel
    {
        private IObservableValue<INavigationController> _navigationValue;

        public MainPageViewModel()
        {
            _navigationValue = this.CreateProperty<INavigationController>("NavigationController");

            this.Subscriptions.Add(_navigationValue.WhereNotNull().Subscribe(navigation => navigation.Navigate<GettingStartedPage>()));

            this.CreateCommand<NavigationViewSelection>("NavigationCommand", this.Navigate);
        }

        private async Task Navigate(CancellationToken ct, NavigationViewSelection selection)
        {
            var navigation = _navigationValue.Latest;

            if (navigation != null)
            {
                switch (selection.Tag)
                {
                    case "GettingStarted":
                        navigation.Navigate<GettingStartedPage>();
                        break;

                    case "MessagesAndDialogs":
                        navigation.Navigate<MessageAndDialogExamplesPage>();
                        break;
                }
            }
        }
    }
}
