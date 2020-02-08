using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using slimCODE.Views.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace slimCODE.Views.Behaviors
{
    public static class GridHelper
    {
        #region Columns ATTACHED PROPERTY

        public static string GetColumns(DependencyObject obj)
        {
            return (string)obj.GetValue(ColumnsProperty);
        }

        public static void SetColumns(DependencyObject obj, string value)
        {
            obj.SetValue(ColumnsProperty, value);
        }

        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.RegisterAttached("Columns", typeof(string), typeof(GridHelper), new PropertyMetadata("", OnColumnsChanged));

        private static void OnColumnsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is Grid grid)
            {
                grid.ColumnDefinitions.Clear();

                foreach (var definition in (e.NewValue as string ?? "")
                    .Split(',')
                    .Select(value => new ColumnDefinition() { Width = value.ToGridLength() }))
                {
                    grid.ColumnDefinitions.Add(definition);
                }

            }
        }

        #endregion

        #region Rows ATTACHED PROPERTY

        public static string GetRows(DependencyObject obj)
        {
            return (string)obj.GetValue(RowsProperty);
        }

        public static void SetRows(DependencyObject obj, string value)
        {
            obj.SetValue(RowsProperty, value);
        }

        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.RegisterAttached("Rows", typeof(string), typeof(GridHelper), new PropertyMetadata("", OnRowsChanged));

        private static void OnRowsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is Grid grid)
            {
                grid.RowDefinitions.Clear();

                foreach (var definition in (e.NewValue as string ?? "")
                    .Split(',')
                    .Select(value => new RowDefinition() { Height = value.ToGridLength() }))
                {
                    grid.RowDefinitions.Add(definition);
                }
            }
        }

        #endregion
    }
}
