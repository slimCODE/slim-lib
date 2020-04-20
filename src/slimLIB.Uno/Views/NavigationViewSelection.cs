using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace slimCODE.Views
{
    /// <summary>
    /// Represents information about the currently selected item in a NavigationView.
    /// </summary>
    public class NavigationViewSelection
    {
        internal NavigationViewSelection(NavigationViewSelectionChangedEventArgs args)
        {
            this.Item = args.SelectedItem;
            this.Tag = args.SelectedItemContainer?.Tag as string;
            this.IsSettings = args.IsSettingsSelected;
        }

        /// <summary>
        /// Gets the selected item when <see cref="NavigationView.MenuItems"/> is data-bound.
        /// </summary>
        public object Item { get; }

        /// <summary>
        /// Gets the string value of the Tag property from the selected <see cref="NavigationViewItem"/>.
        /// </summary>
        public string Tag { get; }

        /// <summary>
        /// Gets a value that indicates if the special settings item is selected.
        /// </summary>
        public bool IsSettings { get; }
    }
}
