using System;
using System.Collections.Generic;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using slimCODE.Models;

namespace slimCODE.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="BaseViewModel"/> instances.
    /// </summary>
    public static partial class BaseViewModelExtensions
    {
        /// <summary>
        /// Creates a command available via data binding, executing the provided async function.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="name"></param>
        /// <param name="asyncExecute"></param>
        public static void CreateCommand(this BaseViewModel viewModel, string name, Func<CancellationToken, Task> asyncExecute)
        {
            var observableValue = new ObservableValue<IAsyncCommand>(new AsyncCommand(asyncExecute));

            viewModel.AddProperty<IAsyncCommand>(name, observableValue);
        }

        /// <summary>
        /// Creates a command available via data binding, executing the provided async function, with support for 
        /// <see cref="System.Windows.Input.ICommand.CanExecute"/> via an <see cref="IObservable{Boolean}"/>.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="name"></param>
        /// <param name="asyncExecute"></param>
        /// <param name="canExecuteObservable"></param>
        public static void CreateCommand(this BaseViewModel viewModel, string name, Func<CancellationToken, Task> asyncExecute, IObservable<bool> canExecuteObservable)
        {
            var observableValue = new ObservableValue<IAsyncCommand>(new AsyncCommand(asyncExecute, canExecuteObservable));

            viewModel.AddProperty<IAsyncCommand>(name, observableValue);
        }

        /// <summary>
        /// Creates a command accepting a parameter, available via data binding, executing the provided async function.
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="name"></param>
        /// <param name="asyncExecute"></param>
        public static void CreateCommand<TParameter>(this BaseViewModel viewModel, string name, Func<CancellationToken, TParameter, Task> asyncExecute)
        {
            var observableValue = new ObservableValue<IAsyncCommand>(new AsyncCommand<TParameter>(asyncExecute));

            viewModel.AddProperty<IAsyncCommand>(name, observableValue);
        }

        /// <summary>
        /// Creates a command accepting a parameter, available via data binding, executing the provided async function, 
        /// with support for <see cref="System.Windows.Input.ICommand.CanExecute"/> via an <see cref="IObservable{Boolean}"/>.
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="name"></param>
        /// <param name="asyncExecute"></param>
        /// <param name="canExecuteObservable"></param>
        public static void CreateCommand<TParameter>(this BaseViewModel viewModel, string name, Func<CancellationToken, TParameter, Task> asyncExecute, IObservable<bool> canExecuteObservable)
        {
            var observableValue = new ObservableValue<IAsyncCommand>(new AsyncCommand<TParameter>(asyncExecute, canExecuteObservable));

            viewModel.AddProperty<IAsyncCommand>(name, observableValue);
        }

        /// <summary>
        /// Creates an <see cref="IObservableCommand{T}"/> available via data binding.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IObservableCommand<T> CreateObservableCommand<T>(this BaseViewModel viewModel, string name)
        {
            var observableValue = new ObservableValue<IObservableCommand<T>>(new ObservableCommand<T>());

            viewModel.AddProperty<IObservableCommand<T>>(name, observableValue);

            return observableValue.Latest;
        }

        /// <summary>
        /// Creates an <see cref="IObservableCommand{Unit}"/>, which ignores any parameter, available via data binding.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IObservableCommand<Unit> CreateObservableCommand(this BaseViewModel viewModel, string name)
        {
            return viewModel.CreateObservableCommand<Unit>(name);
        }

        /// <summary>
        /// Creates an <see cref="IObservableCommand{T}"/> available via data binding, with support for <see cref="System.Windows.Input.ICommand.CanExecute"/>
        /// via an <see cref="IObservable{Boolean}"/>. You can optionally expose that observable via a "Can" + name property as well.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="name"></param>
        /// <param name="canExecuteObservable"></param>
        /// <param name="includeCanProperty"></param>
        /// <returns></returns>
        public static IObservableCommand<T> CreateObservableCommand<T>(
            this BaseViewModel viewModel, 
            string name, 
            IObservable<bool> canExecuteObservable, 
            bool includeCanProperty = false)
        {
            var observableValue = new ObservableValue<IObservableCommand<T>>(new ObservableCommand<T>(canExecuteObservable));
            viewModel.AddProperty<IObservableCommand<T>>(name, observableValue);

            if (includeCanProperty)
            {
                viewModel.CreateProperty($"Can{name}", () => canExecuteObservable, true);
            }

            return observableValue.Latest;
        }

        /// <summary>
        /// Creates an <see cref="IObservableCommand{T}"/> available via data binding, with support for <see cref="System.Windows.Input.ICommand.CanExecute"/>
        /// via an <see cref="IObservable{Unit}"/> that triggers a call to a <see cref="Func{T, Boolean}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="name"></param>
        /// <param name="canExecuteFunc"></param>
        /// <param name="canExecuteChangedObservable"></param>
        /// <returns></returns>
        public static IObservableCommand<T> CreateObservableCommand<T>(
            this BaseViewModel viewModel, 
            string name, 
            Func<T, bool> canExecuteFunc, 
            IObservable<Unit> canExecuteChangedObservable = null)
        {
            var observableValue = new ObservableValue<IObservableCommand<T>>(new ObservableCommand<T>(canExecuteFunc, canExecuteChangedObservable));

            viewModel.AddProperty<IObservableCommand<T>>(name, observableValue);

            return observableValue.Latest;
        }

        /// <summary>
        /// Creates an <see cref="ObservableValue{T}"/> available via data binding.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="name"></param>
        /// <param name="initialValue"></param>
        /// <returns></returns>
        public static ObservableValue<T> CreateProperty<T>(
            this BaseViewModel viewModel,
            string name,
            T initialValue = default(T))
        {
            var observableValue = new ObservableValue<T>(initialValue);

            viewModel.AddProperty<T>(name, observableValue);

            return observableValue;
        }

        /// <summary>
        /// Creates an <see cref="ObservableValue{T}"/> available via data binding, affected by a source observable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="name"></param>
        /// <param name="inputObservableSelector"></param>
        /// <param name="initialValue"></param>
        /// <returns></returns>
        public static ObservableValue<T> CreateProperty<T>(
            this BaseViewModel viewModel,
            string name,
            Func<IObservable<T>> inputObservableSelector,
            T initialValue = default(T))
        {
            var observableValue = new ObservableValue<T>(initialValue);

            viewModel.AddProperty<T>(name, observableValue);
            viewModel.AddTrigger(ViewModelState.Loaded, () => observableValue.AddInput(inputObservableSelector()));

            return observableValue;
        }

        /// <summary>
        /// Creates an <see cref="ObservableValue{T}"/> available via data binding, affected by a source observable, with late materialization
        /// of the observable logic.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="name"></param>
        /// <param name="inputObservableSelector"></param>
        /// <param name="initialValue"></param>
        /// <returns></returns>
        public static ObservableValue<T> CreateProperty<T>(
            this BaseViewModel viewModel,
            string name,
            Func<IObservable<T>, IObservable<T>> inputObservableSelector,
            T initialValue = default(T))
        {
            var observableValue = new ObservableValue<T>(initialValue);

            viewModel.AddProperty<T>(name, observableValue);
            viewModel.AddTrigger(ViewModelState.Loaded, () => observableValue.AddInput(inputObservableSelector));

            return observableValue;
        }

        /// <summary>
        /// Creates an <see cref="ObservableValue{T}"/> available via data binding from any page, 
        /// affected by a source observable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="inputObservableSelector"></param>
        /// <param name="initialValue"></param>
        /// <returns></returns>
        public static ObservableValue<T> CreateGlobalProperty<T>(string name, Func<IObservable<T>> inputObservableSelector, T initialValue = default(T))
        {
            var observableValue = new ObservableValue<T>(initialValue);

            BaseViewModel.AddGlobalProperty<T>(name, observableValue);

            observableValue.AddInput(inputObservableSelector());

            return observableValue;
        }

        /// <summary>
        /// Creates a command available via data binding from any page.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="asyncExecute"></param>
        /// <param name="canExecuteObservable"></param>
        public static void CreateGlobalCommand(string name, Func<CancellationToken, Task> asyncExecute, IObservable<bool> canExecuteObservable)
        {
            var observableValue = new ObservableValue<IAsyncCommand>(new AsyncCommand(asyncExecute, canExecuteObservable));

            BaseViewModel.AddGlobalProperty<IAsyncCommand>(name, observableValue);
        }
    }
}
