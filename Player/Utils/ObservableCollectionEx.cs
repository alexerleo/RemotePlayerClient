using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reflection;

namespace Player.Utils
{
    public class ObservableCollectionEx<T> : ObservableCollection<T> where T : class 
    {
        /// <summary> 
        /// Adds the elements of the specified collection to the end of the ObservableCollection(Of T). 
        /// </summary> 
        public void AddRange(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            foreach (var item in items)
                Items.Add(item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Add or update reange of items
        /// </summary>
        /// <param name="items">New items</param>
        /// <param name="comparer">Comparer function</param>
        public void AddUpdateRange(IEnumerable<T> items, Func<T,T, bool> comparer)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            foreach (T item in items)
                AddUpdate(item, comparer);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Add or update item
        /// </summary>
        /// <param name="item">New item</param>
        /// <param name="comparer">Comparer function</param>
        public void AddUpdate(T item, Func<T,T, bool> comparer)
        {
            T existing = Items.FirstOrDefault(x => comparer(x, item));
            if (existing == null)
                Items.Add(item);
            else
                existing = item;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary> 
        /// Initializes a new instance of the System.Collections.ObjectModel.ObservableCollection(Of T) class. 
        /// </summary> 
        public ObservableCollectionEx(): base()
        {

        }

        /// <summary> 
        /// Initializes a new instance of the System.Collections.ObjectModel.ObservableCollection(Of T) class that contains elements copied from the specified collection. 
        /// </summary> 
        /// <param name="collection">collection: The collection from which the elements are copied.</param> 
        /// <exception cref="System.ArgumentNullException">The collection parameter cannot be null.</exception> 
        public ObservableCollectionEx(IEnumerable<T> collection): base(collection)
        {

        }
    }
}
