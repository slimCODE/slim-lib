using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Seclusion.Views
{
    public sealed partial class LandControl : Control
    {
        public LandControl()
        {
            this.DefaultStyleKey = typeof(LandControl);
        }

        #region IsTapped DEPENDENCY PROPERTY

        public bool IsTapped
        {
            get { return (bool)GetValue(IsTappedProperty); }
            set { SetValue(IsTappedProperty, value); }
        }

        public static readonly DependencyProperty IsTappedProperty =
            DependencyProperty.Register("IsTapped", typeof(bool), typeof(LandControl), new PropertyMetadata(false));

        #endregion
    }
}
