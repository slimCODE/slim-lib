using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using slimCODE.Models;

namespace slimCODE.Extensions
{
    public static partial class BaseViewModelExtensions
    {
        /// <summary>
        /// Creates an <see cref="ObservableValue{T}"/> available as a property via data binding, and affected by a command also available via data binding.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="propertyName"></param>
        /// <param name="command1Name"></param>
        /// <param name="inputObservableSelector"></param>
        /// <param name="initialValue"></param>
        /// <returns></returns>
        public static ObservableValue<T> CreateActiveProperty<T>(
            this BaseViewModel viewModel,
            string propertyName,
            string command1Name,
            Func<IObservable<Unit>, IObservable<T>> inputObservableSelector,
            T initialValue = default(T))
        {
            var command1 = viewModel.CreateObservableCommand(command1Name);

            return viewModel.CreateProperty<T>(
                propertyName,
                () => inputObservableSelector(command1),
                initialValue);
        }

        /// <summary>
        /// Creates an <see cref="ObservableValue{T}"/> available as a property via data binding, and affected by two commands also available via data binding.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="propertyName"></param>
        /// <param name="command1Name"></param>
        /// <param name="command2Name"></param>
        /// <param name="inputObservableSelector"></param>
        /// <param name="initialValue"></param>
        /// <returns></returns>
        public static ObservableValue<T> CreateActiveProperty<T>(
            this BaseViewModel viewModel,
            string propertyName,
            string command1Name,
            string command2Name,
            Func<IObservable<Unit>, IObservable<Unit>, IObservable<T>> inputObservableSelector,
            T initialValue = default(T))
        {
            var command1 = viewModel.CreateObservableCommand(command1Name);
            var command2 = viewModel.CreateObservableCommand(command2Name);

            return viewModel.CreateProperty<T>(
                propertyName,
                () => inputObservableSelector(command1, command2),
                initialValue);
        }

        /// <summary>
        /// Creates an <see cref="ObservableValue{T}"/> available as a property via data binding, and affected by three commands also available via data binding.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="propertyName"></param>
        /// <param name="command1Name"></param>
        /// <param name="command2Name"></param>
        /// <param name="command3Name"></param>
        /// <param name="inputObservableSelector"></param>
        /// <param name="initialValue"></param>
        /// <returns></returns>
        public static ObservableValue<T> CreateActiveProperty<T>(
            this BaseViewModel viewModel,
            string propertyName,
            string command1Name,
            string command2Name,
            string command3Name,
            Func<IObservable<Unit>, IObservable<Unit>, IObservable<Unit>, IObservable<T>> inputObservableSelector,
            T initialValue = default(T))
        {
            var command1 = viewModel.CreateObservableCommand(command1Name);
            var command2 = viewModel.CreateObservableCommand(command2Name);
            var command3 = viewModel.CreateObservableCommand(command3Name);

            return viewModel.CreateProperty<T>(
                propertyName,
                () => inputObservableSelector(command1, command2, command3),
                initialValue);
        }

        /// <summary>
        /// Creates an <see cref="ObservableValue{T}"/> available as a property via data binding, and affected by four commands also available via data binding.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="propertyName"></param>
        /// <param name="command1Name"></param>
        /// <param name="command2Name"></param>
        /// <param name="command3Name"></param>
        /// <param name="command4Name"></param>
        /// <param name="inputObservableSelector"></param>
        /// <param name="initialValue"></param>
        /// <returns></returns>
        public static ObservableValue<T> CreateActiveProperty<T>(
            this BaseViewModel viewModel,
            string propertyName,
            string command1Name,
            string command2Name,
            string command3Name,
            string command4Name,
            Func<IObservable<Unit>, IObservable<Unit>, IObservable<Unit>, IObservable<Unit>, IObservable<T>> inputObservableSelector,
            T initialValue = default(T))
        {
            var command1 = viewModel.CreateObservableCommand(command1Name);
            var command2 = viewModel.CreateObservableCommand(command2Name);
            var command3 = viewModel.CreateObservableCommand(command3Name);
            var command4 = viewModel.CreateObservableCommand(command4Name);

            return viewModel.CreateProperty<T>(
                propertyName,
                () => inputObservableSelector(command1, command2, command3, command4),
                initialValue);
        }
    }
}
