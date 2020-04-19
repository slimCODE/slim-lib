using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using slimCODE.Extensions;
using Windows.UI.Core;

namespace slimCODE.Models
{
    /// <summary>
    /// Base implementation of <see cref="INotifyPropertyChanged"/>, which supports <see cref="CoreDispatcher"/>
    /// redirections of notifications.
    /// </summary>
    public abstract class BaseNotifyPropertyChanged : INotifyPropertyChanged
    {
        private CoreDispatcher _dispatcher;
        private Action<string, PropertyChangedEventHandler> _propertyChanged;

        protected BaseNotifyPropertyChanged(CoreDispatcher dispatcher = null)
        {
            if (dispatcher == null)
            {
                _propertyChanged = GetGenericNotifier();
            }
            else
            {
                _dispatcher = dispatcher;
                _propertyChanged = GetDispatcherNotifier(dispatcher);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="CoreDispatcher"/> associated with this object.
        /// </summary>
        public CoreDispatcher Dispatcher
        {
            get { return _dispatcher; }
            set
            {
                if (value != _dispatcher)
                {
                    _dispatcher = value;
                    _propertyChanged = (value == null)
                        ? GetGenericNotifier()
                        : GetDispatcherNotifier(value);
                }
            }
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged.DoWhen(handler => _propertyChanged(propertyName, handler));
        }

        private Action<string, PropertyChangedEventHandler> GetGenericNotifier()
        {
            return (propertyName, handler) => handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private Action<string, PropertyChangedEventHandler> GetDispatcherNotifier(CoreDispatcher dispatcher)
        {
#pragma warning disable CS4014
            return (propertyName, handler) => dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                () => handler(this, new PropertyChangedEventArgs(propertyName)));
#pragma warning restore CS4014
        }
    }
}
