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

namespace slimLIB.Sample.Models
{
    public class MainPageViewModel : BaseViewModel
    {
        private INavigationController _navigation;

        public MainPageViewModel(INavigationController navigation)
        {
            _navigation = navigation;

            this.CreateCommand("ShowMessageAndDialogCommand", this.ShowMessageAndDialog);

            // TODO: Remove below
            var clearCommand = this.CreateObservableCommand("ClearCommand");
            var testNumbersCommand = this.CreateObservableCommand("TestNumbers");            

            this.CreateProperty<string>("Name", () => ObserveName(clearCommand));
            this.CreateProperty<string>("Number", () => ObserveNumber(testNumbersCommand));
        }

        private async Task ShowMessageAndDialog(CancellationToken ct)
        {
            _navigation.Navigate<MessageAndDialogExamplesPage>();
        }

        // TODO: Remove below

        private IObservable<string> ObserveName(IObservableCommand<Unit> clearObservable)
        {
            return clearObservable
                .Select(_ => "")
                .StartWith("");
        }

        private IObservable<string> ObserveNumber(IObservableCommand<Unit> testNumbersObservable)
        {
            return testNumbersObservable
                .SelectManyCancelPrevious((ct, _) => GetNumberFact(ct));
        }

        private async Task<string> GetNumberFact(CancellationToken ct)
        {
            /*
            var client = new NumbersClient();

            var responses = await Task.WhenAll(
                client.GetRandomFact(ct),
                client.GetNumberFact(ct, 45),
                client.GetDateFact(ct, 3, 1971),
                client.GetYearFact(ct, 2002));

            return string.Join("\n", responses.Select(r => r.Entity.Text));
            */
            // TODO: Replace with Refit!
            return "TODO 42!";
        }
    }
}
