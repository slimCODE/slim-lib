using System;
using System.Collections.Generic;
using System.Text;
using slimCODE.Extensions;
using slimCODE.Models;
using Windows.UI.Xaml;

namespace slimCODE.Views
{
    public class ViewModelController : IViewModelController
    {
        private readonly Dictionary<Type, Func<BaseViewModel>> _viewModelBuilders = new Dictionary<Type, Func<BaseViewModel>>();
        private readonly Dictionary<Type, BaseViewModel> _recycledViewModels = new Dictionary<Type, BaseViewModel>();

        public ViewModelController()
        {
        }

        public void RegisterViewModel(Type viewType, Func<BaseViewModel> viewModelBuilder)
        {
            _viewModelBuilders[viewType] = viewModelBuilder;
        }

        public BaseViewModel GetViewModel(FrameworkElement element, object parameter)
        {
            var key = element.GetType();
            BaseViewModel viewModel = null;

            if (!_recycledViewModels.TryGetValue(key, out viewModel))
            {
                viewModel = _viewModelBuilders[key]();
            }

            viewModel.Activate(element.Dispatcher);

            return viewModel;
        }

        public void ReleaseViewModel(BaseViewModel viewModel, FrameworkElement element)
        {
            var key = element.GetType();
            BaseViewModel previous = null;

            _recycledViewModels.TryGetValue(key, out previous);
            _recycledViewModels[key] = viewModel;

            if (!object.ReferenceEquals(previous, viewModel))
            {
                previous.SafeDispose();
            }
        }
    }
}
