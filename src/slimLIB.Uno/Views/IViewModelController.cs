using System;
using System.Collections.Generic;
using System.Text;
using slimCODE.Models;
using Windows.UI.Xaml;

namespace slimCODE.Views
{
    public interface IViewModelController
    {
        void RegisterViewModel(Type viewType, Func<BaseViewModel> viewModelBuilder);

        BaseViewModel GetViewModel(FrameworkElement element, object parameter);

        void ReleaseViewModel(BaseViewModel viewModel, FrameworkElement element);
    }
}
