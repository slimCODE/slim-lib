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
        /// Creates an <see cref="ObservableValue{TValue}"/> available as a property via data binding, 
        /// and affected by a command also available via data binding.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="propertyName"></param>
        /// <param name="command1Name"></param>
        /// <param name="inputObservableSelector"></param>
        /// <param name="initialValue"></param>
        /// <returns></returns>
        public static ObservableValue<TValue> CreateActiveProperty<TValue>(
            this BaseViewModel viewModel,
            string propertyName,
            string command1Name,
            Func<IObservable<Unit>, IObservable<TValue>> inputObservableSelector,
            TValue initialValue = default)
        {
            var command1 = viewModel.CreateObservableCommand(command1Name);

            return viewModel.CreateProperty<TValue>(
                propertyName,
                () => inputObservableSelector(command1),
                initialValue);
        }

        /// <summary>
        /// Creates an <see cref="ObservableValue{TValue}"/> available as a property via data binding, 
        /// and affected by a command accepting a <typeparamref name="TParam"/> parameter also available 
        /// via data binding.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TParam"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="propertyName"></param>
        /// <param name="command1Name"></param>
        /// <param name="inputObservableSelector"></param>
        /// <param name="initialValue"></param>
        /// <returns></returns>
        public static ObservableValue<TValue> CreateActiveProperty<TValue, TParam>(
            this BaseViewModel viewModel,
            string propertyName,
            string command1Name,
            Func<IObservable<TParam>, IObservable<TValue>> inputObservableSelector,
            TValue initialValue = default)
        {
            var command1 = viewModel.CreateObservableCommand<TParam>(command1Name);

            return viewModel.CreateProperty(
                propertyName,
                () => inputObservableSelector(command1),
                initialValue);
        }

        /// <summary>
        /// Creates an <see cref="ObservableValue{TValue}"/> available as a property via data binding, 
        /// and affected by two commands also available via data binding.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="propertyName"></param>
        /// <param name="command1Name"></param>
        /// <param name="command2Name"></param>
        /// <param name="inputObservableSelector"></param>
        /// <param name="initialValue"></param>
        /// <returns></returns>
        public static ObservableValue<TValue> CreateActiveProperty<TValue>(
            this BaseViewModel viewModel,
            string propertyName,
            string command1Name,
            string command2Name,
            Func<IObservable<Unit>, IObservable<Unit>, IObservable<TValue>> inputObservableSelector,
            TValue initialValue = default)
        {
            var command1 = viewModel.CreateObservableCommand(command1Name);
            var command2 = viewModel.CreateObservableCommand(command2Name);

            return viewModel.CreateProperty<TValue>(
                propertyName,
                () => inputObservableSelector(command1, command2),
                initialValue);
        }

        /// <summary>
        /// Creates an <see cref="ObservableValue{TValue}"/> available as a property via data binding, 
        /// and affected by two commands respectively accepting <typeparamref name="TParam1"/> and 
        /// <typeparamref name="TParam2"/> parameters
        /// also available via data binding.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TParam1"></typeparam>
        /// <typeparam name="TParam2"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="propertyName"></param>
        /// <param name="command1Name"></param>
        /// <param name="command2Name"></param>
        /// <param name="inputObservableSelector"></param>
        /// <param name="initialValue"></param>
        /// <returns></returns>
        public static ObservableValue<TValue> CreateActiveProperty<TValue, TParam1, TParam2>(
            this BaseViewModel viewModel,
            string propertyName,
            string command1Name,
            string command2Name,
            Func<IObservable<TParam1>, IObservable<TParam2>, IObservable<TValue>> inputObservableSelector,
            TValue initialValue = default)
        {
            var command1 = viewModel.CreateObservableCommand<TParam1>(command1Name);
            var command2 = viewModel.CreateObservableCommand<TParam2>(command2Name);

            return viewModel.CreateProperty(
                propertyName,
                () => inputObservableSelector(command1, command2),
                initialValue);
        }

        /// <summary>
        /// Creates an <see cref="ObservableValue{TValue}"/> available as a property via data binding, 
        /// and affected by three commands also available via data binding.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="propertyName"></param>
        /// <param name="command1Name"></param>
        /// <param name="command2Name"></param>
        /// <param name="command3Name"></param>
        /// <param name="inputObservableSelector"></param>
        /// <param name="initialValue"></param>
        /// <returns></returns>
        public static ObservableValue<TValue> CreateActiveProperty<TValue>(
            this BaseViewModel viewModel,
            string propertyName,
            string command1Name,
            string command2Name,
            string command3Name,
            Func<IObservable<Unit>, IObservable<Unit>, IObservable<Unit>, IObservable<TValue>> inputObservableSelector,
            TValue initialValue = default)
        {
            var command1 = viewModel.CreateObservableCommand(command1Name);
            var command2 = viewModel.CreateObservableCommand(command2Name);
            var command3 = viewModel.CreateObservableCommand(command3Name);

            return viewModel.CreateProperty<TValue>(
                propertyName,
                () => inputObservableSelector(command1, command2, command3),
                initialValue);
        }

        /// <summary>
        /// Creates an <see cref="ObservableValue{TValue}"/> available as a property via data binding, 
        /// and affected by three commands respectively accepting <typeparamref name="TParam1"/>, 
        /// <typeparamref name="TParam2"/> and <typeparamref name="TParam3"/> parameters
        /// also available via data binding.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TParam1"></typeparam>
        /// <typeparam name="TParam2"></typeparam>
        /// <typeparam name="TParam3"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="propertyName"></param>
        /// <param name="command1Name"></param>
        /// <param name="command2Name"></param>
        /// <param name="command3Name"></param>
        /// <param name="inputObservableSelector"></param>
        /// <param name="initialValue"></param>
        /// <returns></returns>
        public static ObservableValue<TValue> CreateActiveProperty<TValue, TParam1, TParam2, TParam3>(
            this BaseViewModel viewModel,
            string propertyName,
            string command1Name,
            string command2Name,
            string command3Name,
            Func<IObservable<TParam1>, IObservable<TParam2>, IObservable<TParam3>, IObservable<TValue>> inputObservableSelector,
            TValue initialValue = default)
        {
            var command1 = viewModel.CreateObservableCommand<TParam1>(command1Name);
            var command2 = viewModel.CreateObservableCommand<TParam2>(command2Name);
            var command3 = viewModel.CreateObservableCommand<TParam3>(command3Name);

            return viewModel.CreateProperty(
                propertyName,
                () => inputObservableSelector(command1, command2, command3),
                initialValue);
        }

        /// <summary>
        /// Creates an <see cref="ObservableValue{TValue}"/> available as a property via data binding, 
        /// and affected by four commands also available via data binding.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="propertyName"></param>
        /// <param name="command1Name"></param>
        /// <param name="command2Name"></param>
        /// <param name="command3Name"></param>
        /// <param name="command4Name"></param>
        /// <param name="inputObservableSelector"></param>
        /// <param name="initialValue"></param>
        /// <returns></returns>
        public static ObservableValue<TValue> CreateActiveProperty<TValue>(
            this BaseViewModel viewModel,
            string propertyName,
            string command1Name,
            string command2Name,
            string command3Name,
            string command4Name,
            Func<IObservable<Unit>, IObservable<Unit>, IObservable<Unit>, IObservable<Unit>, IObservable<TValue>> inputObservableSelector,
            TValue initialValue = default)
        {
            var command1 = viewModel.CreateObservableCommand(command1Name);
            var command2 = viewModel.CreateObservableCommand(command2Name);
            var command3 = viewModel.CreateObservableCommand(command3Name);
            var command4 = viewModel.CreateObservableCommand(command4Name);

            return viewModel.CreateProperty<TValue>(
                propertyName,
                () => inputObservableSelector(command1, command2, command3, command4),
                initialValue);
        }

        /// <summary>
        /// Creates an <see cref="ObservableValue{TValue}"/> available as a property via data binding, 
        /// and affected by four commands respectively accepting <typeparamref name="TParam1"/>, 
        /// <typeparamref name="TParam2"/>, <typeparamref name="TParam3"/> and 
        /// <typeparamref name="TParam4"/> parameters also available via data binding.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TParam1"></typeparam>
        /// <typeparam name="TParam2"></typeparam>
        /// <typeparam name="TParam3"></typeparam>
        /// <typeparam name="TParam4"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="propertyName"></param>
        /// <param name="command1Name"></param>
        /// <param name="command2Name"></param>
        /// <param name="command3Name"></param>
        /// <param name="command4Name"></param>
        /// <param name="inputObservableSelector"></param>
        /// <param name="initialValue"></param>
        /// <returns></returns>
        public static ObservableValue<TValue> CreateActiveProperty<TValue, TParam1, TParam2, TParam3, TParam4>(
            this BaseViewModel viewModel,
            string propertyName,
            string command1Name,
            string command2Name,
            string command3Name,
            string command4Name,
            Func<IObservable<TParam1>, IObservable<TParam2>, IObservable<TParam3>, IObservable<TParam4>, IObservable<TValue>> inputObservableSelector,
            TValue initialValue = default)
        {
            var command1 = viewModel.CreateObservableCommand<TParam1>(command1Name);
            var command2 = viewModel.CreateObservableCommand<TParam2>(command2Name);
            var command3 = viewModel.CreateObservableCommand<TParam3>(command3Name);
            var command4 = viewModel.CreateObservableCommand<TParam4>(command4Name);

            return viewModel.CreateProperty(
                propertyName,
                () => inputObservableSelector(command1, command2, command3, command4),
                initialValue);
        }

        /// <summary>
        /// Creates an <see cref="ObservableValue{TValue}"/> available as a property via data binding, 
        /// and affected by a command also available via data binding, with an observable that determines
        /// if it can be executed. You can optionally expose that observable via a "Can" + commandName
        /// property.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="propertyName"></param>
        /// <param name="commandName"></param>
        /// <param name="inputObservableSelector"></param>
        /// <param name="canExecuteObservable"></param>
        /// <param name="initialValue"></param>
        /// <param name="includeCanProperty"></param>
        /// <returns></returns>
        public static ObservableValue<TValue> CreateActiveProperty<TValue>(
            this BaseViewModel viewModel,
            string propertyName,
            string commandName,
            Func<IObservable<Unit>, IObservable<TValue>> inputObservableSelector,
            IObservable<bool> canExecuteObservable,
            TValue initialValue = default,
            bool includeCanProperty = false)
        {
            var command = viewModel.CreateObservableCommand<Unit>(commandName, canExecuteObservable, includeCanProperty);

            return viewModel.CreateProperty<TValue>(
                propertyName,
                () => inputObservableSelector(command),
                initialValue);
        }

        /// <summary>
        /// Creates an <see cref="ObservableValue{TValue}"/> available as a property via data binding, 
        /// and affected by a command accepting a <typeparamref name="TParam"/> parameter also 
        /// available via data binding, with an observable that determines if it can be executed. You 
        /// can optionally expose that observable via a "Can" + commandName property.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TParam"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="propertyName"></param>
        /// <param name="commandName"></param>
        /// <param name="inputObservableSelector"></param>
        /// <param name="canExecuteObservable"></param>
        /// <param name="initialValue"></param>
        /// <param name="includeCanProperty"></param>
        /// <returns></returns>
        public static ObservableValue<TValue> CreateActiveProperty<TValue, TParam>(
            this BaseViewModel viewModel,
            string propertyName,
            string commandName,
            Func<IObservable<TParam>, IObservable<TValue>> inputObservableSelector,
            IObservable<bool> canExecuteObservable,
            TValue initialValue = default,
            bool includeCanProperty = false)
        {
            var command = viewModel.CreateObservableCommand<TParam>(commandName, canExecuteObservable, includeCanProperty);

            return viewModel.CreateProperty<TValue>(
                propertyName,
                () => inputObservableSelector(command),
                initialValue);
        }

        /// <summary>
        /// Creates an <see cref="ObservableValue{TValue}"/> available as a property via data binding, 
        /// and affected by a command accepting a <typeparamref name="TParam"/> parameter also 
        /// available via data binding, with an observable that determines if execution permission must
        /// be re-evaluated from a can execute function.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TParam"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="propertyName"></param>
        /// <param name="command1Name"></param>
        /// <param name="inputObservableSelector"></param>
        /// <param name="initialValue"></param>
        /// <returns></returns>
        public static ObservableValue<TValue> CreateActiveProperty<TValue, TParam>(
            this BaseViewModel viewModel,
            string propertyName,
            string command1Name,
            Func<IObservable<TParam>, IObservable<TValue>> inputObservableSelector,
            Func<TParam, bool> canExecuteFunc,
            IObservable<Unit> canExecuteChangedObservable,
            TValue initialValue = default)
        {
            var command1 = viewModel.CreateObservableCommand<TParam>(command1Name, canExecuteFunc, canExecuteChangedObservable);

            return viewModel.CreateProperty<TValue>(
                propertyName,
                () => inputObservableSelector(command1),
                initialValue);
        }

        // For now, not exposing the last three with more params.
    }
}
