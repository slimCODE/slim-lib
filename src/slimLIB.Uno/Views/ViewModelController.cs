using System;
using System.Collections.Generic;
using System.Text;
using slimCODE.Extensions;
using slimCODE.Models;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace slimCODE.Views
{
    public class ViewModelController : IViewModelController
    {
        private readonly Dictionary<Type, Func<CoreDispatcher, BaseViewModel>> _viewModelBuilders = new Dictionary<Type, Func<CoreDispatcher, BaseViewModel>>();
        private readonly Dictionary<Type, BaseViewModel> _recycledViewModels = new Dictionary<Type, BaseViewModel>();

        public ViewModelController()
        {
        }

        public void RegisterViewModel(Type viewType, Func<CoreDispatcher, BaseViewModel> viewModelBuilder)
        {
            _viewModelBuilders[viewType] = viewModelBuilder;
        }

        public BaseViewModel GetViewModel(FrameworkElement element, object parameter)
        {
            var key = element.GetType();

            if (!_recycledViewModels.TryGetValue(key, out var viewModel))
            {
                viewModel = _viewModelBuilders[key](element.Dispatcher);
            }

            // TODO: Rethink the Activate / Deactivate.
            viewModel.Activate(element.Dispatcher);

            return viewModel;
        }

        public void ReleaseViewModel(BaseViewModel viewModel, FrameworkElement element)
        {
            var key = element.GetType();

            _recycledViewModels.TryGetValue(key, out var previous);
            _recycledViewModels[key] = viewModel;

            viewModel.Deactivate();

            if (!object.ReferenceEquals(previous, viewModel))
            {
                previous.SafeDispose();
            }
        }
    }
}
