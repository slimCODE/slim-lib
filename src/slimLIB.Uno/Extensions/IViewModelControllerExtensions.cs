using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using slimCODE.Models;
using slimCODE.Views;

namespace slimCODE.Extensions
{
    /// <summary>
    /// Contains extension methods on <see cref="IViewModelController"/> instances.
    /// </summary>
    public static partial class IViewModelControllerExtensions
    {
        /// <summary>
        /// Registers a <typeparamref name="TViewModel"/> with a <typeparamref name="TPage"/>, using the view-model's
        /// default constructor to create it on navigation.
        /// </summary>
        /// <typeparam name="TPage"></typeparam>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="controller"></param>
        public static void RegisterViewModel<TPage, TViewModel>(this IViewModelController controller)
            where TViewModel : BaseViewModel, new()
        {
            controller.RegisterViewModel(typeof(TPage), () => new TViewModel());
        }
    }
}
