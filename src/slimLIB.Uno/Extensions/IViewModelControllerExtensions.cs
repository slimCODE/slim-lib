using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using slimCODE.Models;
using slimCODE.Views;
using Windows.UI.Core;

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
            controller.RegisterViewModel(typeof(TPage), dispatcher => new TViewModel());
        }

        // TODO: Remove this once we fully use IServiceProvider
        /// <summary>
        /// Registers a <typeparamref name="TViewModel"/> with a <typeparamref name="TPage"/>, using a view-model
        /// builder function to create it on navigation.
        /// </summary>
        /// <typeparam name="TPage"></typeparam>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="controller"></param>
        public static void RegisterViewModel<TPage, TViewModel>(this IViewModelController controller, Func<CoreDispatcher, TViewModel> viewModelBuilder)
            where TViewModel : BaseViewModel
        {
            controller.RegisterViewModel(typeof(TPage), viewModelBuilder);
        }
    }
}
