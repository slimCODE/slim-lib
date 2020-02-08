using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using slimCODE.Views;
using Windows.UI.Xaml.Controls;

namespace slimCODE.Extensions
{
    /// <summary>
    /// Exposes methods of a navigation service.
    /// </summary>
    public static partial class INavigationControllerExtensions
    {
        /// <summary>
        /// Navigates the active <see cref="Frame"/> to a new instance of the specified <typeparamref name="TView"/>.
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <param name="controller"></param>
        public static void Navigate<TView>(this INavigationController controller)
            where TView : Control
        {
            controller.Navigate(typeof(TView));
        }

        /// <summary>
        /// Navigates the active <see cref="Frame"/> to a new instance of the specified <typeparamref name="TView"/>,
        /// providing parameters that can received by the registered view-model.
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <param name="controller"></param>
        /// <param name="arguments"></param>
        public static void Navigate<TView>(this INavigationController controller, object arguments)
            where TView : Control
        {
            controller.Navigate(typeof(TView), arguments);
        }
    }
}
