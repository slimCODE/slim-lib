using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using slimCODE.Models;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace slimCODE.Extensions
{
    public static partial class BaseViewModelExtensions
    {
        /// <summary>
        /// Shows a <see cref="MessageDialog"/> using the provided title and message.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="ct"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static async Task ShowMessage(this BaseViewModel viewModel, CancellationToken ct, string message, string title = null)
        {
            var source = new TaskCompletionSource<Unit>();

            await viewModel.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                async () =>
                {
                    try
                    {
                        var dialog = (title == null) ? new MessageDialog(message) : new MessageDialog(message, title);

                        await dialog.ShowAsync();
                    }
                    finally
                    {
                        source.SetResult(Unit.Default);
                    }
                });

            await source.Task;
        }
    }
}
