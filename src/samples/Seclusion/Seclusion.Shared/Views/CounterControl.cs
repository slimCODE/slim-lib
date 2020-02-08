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
    [TemplatePart(Name = DecreaseButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = IncreaseButtonPartName, Type = typeof(Button))]
    public sealed partial class CounterControl : Control
    {
        private const string DecreaseButtonPartName = "PART_DecreaseButton";
        private const string IncreaseButtonPartName = "PART_IncreaseButton";

        public CounterControl()
        {
            this.DefaultStyleKey = typeof(CounterControl);
        }

        #region Title DEPENDENCY PROPERTY

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(CounterControl), new PropertyMetadata(""));

        #endregion

        #region Value DEPENDENCY PROPERTY

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(CounterControl), new PropertyMetadata(0));

        #endregion

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var decreaseButton = this.GetTemplateChild(DecreaseButtonPartName) as Button;
            var increaseButton = this.GetTemplateChild(IncreaseButtonPartName) as Button;

            if (decreaseButton != null)
            {
                decreaseButton.Click += (s, e) =>
                {
                    this.Value = Math.Max(0, this.Value - 1);
                };
            }

            if (increaseButton != null)
            {
                increaseButton.Click += (s, e) =>
                {
                    this.Value = this.Value + 1;
                };
            }
        }
    }
}
