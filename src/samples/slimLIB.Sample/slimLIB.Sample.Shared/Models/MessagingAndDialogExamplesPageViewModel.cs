using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using slimCODE.Extensions;
using slimCODE.Models;

namespace slimLIB.Sample.Models
{
    public class MessagingAndDialogExamplesPageViewModel : BaseViewModel
    {
        public MessagingAndDialogExamplesPageViewModel()
        {
            BaseViewModelExtensions.RegisterDialog<>
            // See App.xaml.cs for registered dialogs
            this.CreateCommand("OpenOkMessage", this.OpenOkMessage);
        }

        private async Task OpenOkMessage(CancellationToken ct)
        {
            await this.ShowMessage(ct, "This is a simple message with the default OK button.", "This is a title");
        }

        private async Task OpenYesNoQuestion(CancellationToken ct)
        {
            await this.ShowDialog()
        }
    }
}
