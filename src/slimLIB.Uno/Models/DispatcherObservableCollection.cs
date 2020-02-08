using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using slimCODE.Validation;
using Windows.UI.Core;

namespace slimCODE.Models
{
    /// <summary>
    /// Represents a list that implements <see cref="INotifyCollectionChanged"/> and 
    /// <see cref="INotifyPropertyChanged"/> by firing events on the <see cref="CoreDispatcher"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DispatcherObservableCollection<T> : IList<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private readonly CoreDispatcher _dispatcher;
        private readonly ObservableCollection<T> _collection;

        /// <summary>
        /// Creates a <see cref="DispatcherObservableCollection{T}"/> using the specified <see cref="CoreDispatcher"/>.
        /// </summary>
        /// <param name="dispatcher"></param>
        public DispatcherObservableCollection(CoreDispatcher dispatcher)
        {
            _dispatcher = dispatcher.Validate(nameof(dispatcher)).IsNotNull();
            _collection = new ObservableCollection<T>();

            _collection.CollectionChanged += OnCollectionChanged;
            (_collection as INotifyPropertyChanged).PropertyChanged += OnPropertyChanged;
        }

        /// <summary>
        /// Creates a <see cref="DispatcherObservableCollection{T}"/> with an initial list of items, using the
        /// specified <see cref="CoreDispatcher"/>.
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <param name="items"></param>
        public DispatcherObservableCollection(CoreDispatcher dispatcher, IEnumerable<T> items)
        {
            _dispatcher = dispatcher.Validate(nameof(dispatcher)).IsNotNull();
            _collection = new ObservableCollection<T>(items);

            _collection.CollectionChanged += OnCollectionChanged;
            (_collection as INotifyPropertyChanged).PropertyChanged += OnPropertyChanged;
        }

        /// <inheritdoc/>

        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var handler = this.PropertyChanged;

            if (handler != null)
            {
                if (_dispatcher.HasThreadAccess)
                {
                    handler(this, e);
                }
                else
                {
#pragma warning disable CS4014 
                    _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => handler(this, e));
#pragma warning restore CS4014 
                }
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var handler = this.CollectionChanged;

            if (handler != null)
            {
                if (_dispatcher.HasThreadAccess)
                {
                    handler(this, e);
                }
                else
                {
#pragma warning disable CS4014 
                    _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => handler(this, e));
#pragma warning restore CS4014 
                }
            }
        }

        /// <inheritdoc/>
        public T this[int index]
        {
            get => _collection[index];
            set => _collection[index] = value;
        }

        /// <inheritdoc/>
        public int Count => _collection.Count();

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <inheritdoc/>
        public void Add(T item)
        {
            _collection.Add(item);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            _collection.Clear();
        }

        /// <inheritdoc/>
        public bool Contains(T item)
        {
            return _collection.Contains(item);
        }

        /// <inheritdoc/>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _collection.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        /// <inheritdoc/>
        public int IndexOf(T item)
        {
            return _collection.IndexOf(item);
        }

        /// <inheritdoc/>
        public void Insert(int index, T item)
        {
            _collection.Insert(index, item);
        }

        /// <inheritdoc/>
        public bool Remove(T item)
        {
            return _collection.Remove(item);
        }

        /// <inheritdoc/>
        public void RemoveAt(int index)
        {
            _collection.RemoveAt(index);
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _collection.GetEnumerator();
        }
    }
}
