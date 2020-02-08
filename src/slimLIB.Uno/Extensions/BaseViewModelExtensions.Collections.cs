using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using slimCODE.Models;

namespace slimCODE.Extensions
{
    public static partial class BaseViewModelExtensions
    {
        /// <summary>
        /// Creates an <see cref="ObservableValue{ObservableCollection{T}}"/> available as a property via data binding, with side effects
        /// that can affect the collection's contents. All notifications are fired on the UI thread.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="propertyName"></param>
        /// <param name="observableActions"></param>
        /// <param name="initialContent"></param>
        /// <returns></returns>
        public static ObservableValue<ObservableCollection<T>> CreateCollectionProperty<T>(
            this BaseViewModel viewModel,
            string propertyName,
            Func<ObservableCollectionProxy<T>, IObservable<Unit>> observableActions,
            IEnumerable<T> initialContent = null)
        {
            return viewModel.CreateProperty<ObservableCollection<T>>(
                propertyName,
                () => Observable
                    .Return(new ObservableCollection<T>(initialContent ?? Enumerable.Empty<T>()))
                    .SelectMany(collection =>
                        observableActions(new ObservableCollectionProxy<T>(
                            viewModel.Dispatcher,
                            collection))
                        .IgnoreElements()
                        .Select(_ => default(ObservableCollection<T>))
                        .StartWith(collection)));
        }
    }
}
