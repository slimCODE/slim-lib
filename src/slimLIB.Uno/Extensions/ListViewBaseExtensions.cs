using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace slimCODE.Extensions
{
    /// <summary>
    /// Contains extension methods on <see cref="ListViewBase"/> instances.
    /// </summary>
    public static partial class ListViewBaseExtensions
    {
        /// <summary>
        /// Returns an observable firing with an object representing the clicked item.
        /// </summary>
        /// <param name="listViewBase"></param>
        /// <returns></returns>
        public static IObservable<object> ObserveItemClick(this ListViewBase listViewBase)
        {
            return Observable
                .FromEventPattern<ItemClickEventHandler, ItemClickEventArgs>(
                    h => listViewBase.ItemClick += h,
                    h => listViewBase.ItemClick -= h)
                .Select(ep => ep.EventArgs.ClickedItem);
        }
    }
}
