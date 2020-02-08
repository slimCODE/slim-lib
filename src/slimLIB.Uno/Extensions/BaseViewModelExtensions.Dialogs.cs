using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using slimCODE.Models;
using Windows.UI.Xaml.Controls;

namespace slimCODE.Extensions
{
    public static partial class BaseViewModelExtensions
    {
        private static Dictionary<string, Func<ContentDialog>> _dialogsFactory = new Dictionary<string, Func<ContentDialog>>();

        /// <summary>
        /// Registers a <see cref="ContentDialog"/> by name, which can be shown by using 
        /// <see cref="ShowDialog{TDataContext}(BaseViewModel, CancellationToken, string, TDataContext, Func{CancellationToken, Task}[])"/>.
        /// </summary>
        /// <typeparam name="TDialog"></typeparam>
        /// <param name="name"></param>
        public static void RegisterDialog<TDialog>(string name) 
            where TDialog : ContentDialog, new()
        {
            _dialogsFactory.Add(name, () => new TDialog());
        }

        /// <summary>
        /// Shows a <see cref="ContentDialog"/> by its name, previously registered with <see cref="RegisterDialog{TDialog}(string)"/>, 
        /// using the provided context as its DataContext.
        /// </summary>
        /// <typeparam name="TDataContext"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="ct"></param>
        /// <param name="name"></param>
        /// <param name="dataContext"></param>
        /// <param name="buttonActions"></param>
        /// <returns></returns>
        public static Task<ContentDialogResult> ShowDialog<TDataContext>(
            this BaseViewModel viewModel,
            CancellationToken ct,
            string name,
            TDataContext dataContext,
            params Func<CancellationToken, Task>[] buttonActions)
        {
            return viewModel.Dispatcher.RunNormalAsync(
                ct,
                ct2 =>
                {
                    var dialog = _dialogsFactory[name]();

                    if (buttonActions.Length > 0)
                    {
                        dialog.PrimaryButtonCommand = new AsyncCommand(buttonActions[0]);

                        if (buttonActions.Length > 1)
                        {
                            dialog.SecondaryButtonCommand = new AsyncCommand(buttonActions[1]);

                            if (buttonActions.Length > 2)
                            {
                                dialog.CloseButtonCommand = new AsyncCommand(buttonActions[2]);
                            }
                        }
                    }

                    dialog.DataContext = dataContext;
                    return dialog.ShowAsync().AsTask(ct);
                });
        }
    }
}
