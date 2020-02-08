using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace slimCODE.Models
{
    public class ObservableCollectionProxy<T> : IList<T>
    {
        private readonly CoreDispatcher _dispatcher;
        private readonly ObservableCollection<T> _collection;

        public ObservableCollectionProxy(CoreDispatcher dispatcher, ObservableCollection<T> collection)
        {
            _dispatcher = dispatcher;
            _collection = collection;
        }

        public T this[int index]
        {
            get => _collection[index];
            set => this.RunAsync(() => _collection[index] = value);
        }

        public int Count => _collection.Count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            this.RunAsync(() => _collection.Add(item));
        }

        public void Clear()
        {
            this.RunAsync(() => _collection.Clear());
        }

        public bool Contains(T item)
        {
            return _collection.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _collection.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return _collection.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            this.RunAsync(() => _collection.Insert(index, item));
        }

        public bool Remove(T item)
        {
            return this.RunAsyncAndWait(() => _collection.Remove(item));
        }

        public void RemoveAt(int index)
        {
            this.RunAsync(() => _collection.RemoveAt(index));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        private void RunAsync(Action action)
        {
#pragma warning disable CS4014 
            _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
#pragma warning restore CS4014 
        }

        private TResult RunAsyncAndWait<TResult>(Func<TResult> func)
        {
            var result = default(TResult);

            _dispatcher
                .RunAsync(
                    CoreDispatcherPriority.Normal, 
                    () => { result = func(); })
                .AsTask()
                .Wait();

            return result;
        }
    }
}
