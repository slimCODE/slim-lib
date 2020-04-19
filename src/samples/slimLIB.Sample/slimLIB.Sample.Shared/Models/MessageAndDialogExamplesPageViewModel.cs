using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using slimCODE.Extensions;
using slimCODE.Models;

namespace slimLIB.Sample.Models
{
    public class MessageAndDialogExamplesPageViewModel : BaseViewModel
    {
        private ExampleDialogViewModel _example;
        private Subject<Unit> _resetCountdown = new Subject<Unit>();

        public MessageAndDialogExamplesPageViewModel()
        {
            _resetCountdown.DisposableBy(this.Subscriptions);
            _example = this.AddChildProperty("Example", new ExampleDialogViewModel(_resetCountdown, this));

            this.CreateCommand("MessageExampleCommand", this.OpenOkMessage);
            this.CreateCommand("DialogExampleCommand", this.OpenExampleDialog);
        }

        private async Task OpenOkMessage(CancellationToken ct)
        {
            await this.ShowMessage(ct, "This is a simple message with the default OK button.", "This is a title");
        }

        private async Task OpenExampleDialog(CancellationToken ct)
        {
            // View-models must not know about the view. That's why dialogs are registered by name.
            // See App.xaml.cs for details.

            _resetCountdown.OnNext();

            // By the way: We could have used 'this["Example"]' instead of keeping the '_example' reference.
            await this.ShowDialog(ct, "ExampleDialog", _example);
        }
    }
}
