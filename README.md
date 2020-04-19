# slimLIB
.NET Library for Uno to help you build applications using observable patterns.

_Disclaimer_: The documentation below is still in progress.

## What's inside
This library uses Reactive Extensions extensively. It allows creation of view-models for XAML data binding, with services to perform navigation.

### ObservableValue
Put simply, it's a subject that exposes its latest value synchroneously. It serves as the bridge between the observable pattern and data binding. You can attach external observable inputs to it. It will manage the subscriptions itself until disposed (usually by its owner view-model).

### ObservableCommand
This object implements ICommand, IObservable and IObserver. It allows very nifty chain reactions.

### ObservableValues
Container for ObservableValue instances, accessible by name. It automatically subscribes to each added observable value to report changes, allowing for an easy implementation of INotifyPropertyChanged (see BaseViewModel). It also handles getting and setting values in a sync way, for easy data-binding.

### BaseViewModel
This is the base class of any view-model you wish to create and bind to a view. It holds and manages observable values via ObservableValues instances. It implements INotifyPropertyChanged in a UI-thread-safe way. All properties of a BaseViewModel are accessible via data-binding via the [] operator. Instead of doing `{Binding PropertyName}`, you do `{Binding [PropertyName]}`.

Its main method for building properties and interactions is `AddProperty`, but you will generally prefer one of the many extension methods to build a view-model's properties.

### BaseViewModel extension methods

### First example - Creating a view-model
