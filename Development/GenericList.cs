//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL$
//
// Version:         $Revision$,
//                  $Date$
//                  $Author$
//
// Copyright (c) Gurux Ltd
//
//---------------------------------------------------------------------------
//
//  DESCRIPTION
//
// This file is a part of Gurux Device Framework.
//
// Gurux Device Framework is Open Source software; you can redistribute it
// and/or modify it under the terms of the GNU General Public License 
// as published by the Free Software Foundation; version 2 of the License.
// Gurux Device Framework is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of 
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details.
//
// This code is licensed under the GNU General Public License v2. 
// Full text may be retrieved at http://www.gnu.org/licenses/gpl-2.0.txt
//---------------------------------------------------------------------------

//**********************************************************************************
// Creator: T. Shrove
// Date: 7/25/09
// Email: tshrove@gmail.com
// Website: http://www.tshrove.com
// Code Website: http://code.tshrove.com
// This is for use only. Not for sale. If you make any changes to it please email 
// me a copy of the updated source code.
//**********************************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;

namespace Gurux.Device
{
    /// <summary>
    /// This is a generic list that has added the events:
    /// ItemRemove, ItemAdded, ItemsCleared, BeforeItemAdded, and BeforeItemRemoved
    /// that was not added by default.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class GenericList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable
    {
        #region Members
        private List<T> m_pItems = null;        
        #endregion

        #region Events
        /// <summary>
        /// Raises when an item is added to the list.
        /// </summary>
        private event EventHandler<GenericItemEventArgs<T>> ItemAdded;
        /// <summary>
        /// Raises before an item is added to the list.
        /// </summary>
        private event EventHandler<GenericItemEventArgs<T>> BeforeItemAdded;
        /// <summary>
        /// Raises when an item is removed from the list.
        /// </summary>
        private event EventHandler<EventArgs> ItemRemoved;
        /// <summary>
        /// Raises before an item is removed from the list.
        /// </summary>
        private event EventHandler<GenericItemEventArgs<T>> BeforeItemRemoved;
        /// <summary>
        /// Raises when the items are cleared from the list.
        /// </summary>
        private event EventHandler<EventArgs> ItemsCleared;
        #endregion

        #region Protected Properties
        /// <summary>
        /// Returns the list of items in the class.
        /// </summary>
        protected List<T> Items
        {
            get { return this.m_pItems; }
            private set { this.m_pItems = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor for the class.
        /// </summary>
        public GenericList()
            :this(0)
        {
            //Nothing to do
        }
        /// <summary>
        /// Constructor that sets the size of the list to the capacity number.
        /// </summary>
        /// <param name="capacity">Number of items you want the list to default to in size.</param>
        public GenericList(int capacity)
        {
            this.Items = new List<T>(capacity);
        }
        #endregion

        #region IList Methods
		/// <summary>
		/// Gets or sets the element at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the element to get or set.</param>
		/// <returns>The element at the specified index.</returns>
        public T this[int index] 
        {
            get { return this.Items[index]; }
            set { this.Items[index] = value; }
        }
		/// <summary>
		/// Determines the index of a specific item in the System.Collections.Generic.IList.
		/// </summary>
		/// <param name="item">The object to locate in the System.Collections.Generic.IList.</param>
		/// <returns>The index of item if found in the list; otherwise, -1.</returns>
        public int IndexOf(T item)
        {
            return this.Items.IndexOf(item);
        }

		/// <summary>
		/// Inserts an item to the System.Collections.Generic.IList at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which item should be inserted.</param>
		/// <param name="item">The object to insert into the System.Collections.Generic.IList.</param>
        public void Insert(int index, T item)
        {
            OnBeforeItemAdded(this, new GenericItemEventArgs<T>(item));
            this.Items.Insert(index, item);
            OnItemAdded(this, new GenericItemEventArgs<T>(item));
        }

		/// <summary>
		/// Removes the System.Collections.Generic.IList item at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
        public void RemoveAt(int index)
        {
            OnBeforeItemRemoved(this, new GenericItemEventArgs<T>(this.Items[index]));
            this.Items.RemoveAt(index);
            OnItemRemoved(this, new EventArgs());
        }
        #endregion

		/// <summary>
		/// Adds items from a collection to the current collection.
		/// </summary>
        public void AddRange(IEnumerable<T> collection)
        {
            foreach (T it in collection)
            {
                Add(it);
            }
        }

        #region ICollection Methods and Properties
		/// <summary>
		/// Gets the number of elements contained in the System.Collections.Generic.ICollection.
		/// </summary>
        public int Count { 
            get 
            {
                return this.Items.Count; 
            } 
        }
		/// <summary>
		/// Gets a value indicating whether the System.Collections.Generic.ICollection is read-only.
		/// </summary>
        public bool IsReadOnly { get { return false; } }
		/// <summary>
		/// Adds an item to the System.Collections.Generic.ICollection.
		/// </summary>
		/// <param name="item">The object to add to the System.Collections.Generic.ICollection.</param>
        public void Add(T item)
        {
            OnBeforeItemAdded(this, new GenericItemEventArgs<T>(item));
            this.Items.Add(item);
            OnItemAdded(this, new GenericItemEventArgs<T>(item));
        }
		/// <summary>
		/// Removes all items from the System.Collections.Generic.ICollection.
		/// </summary>
        public virtual void Clear()
        {
            this.Items.Clear();
            OnItemsCleared(this, new EventArgs());
        }
		/// <summary>
		/// Determines whether the System.Collections.Generic.ICollection contains a specific value.
		/// </summary>
		/// <param name="item">The object to locate in the System.Collections.Generic.ICollection.</param>
		/// <returns>
		///	true if item is found in the System.Collections.Generic.ICollection; otherwise, false.
		/// </returns>
        public bool Contains(T item)
        {
            return this.Items.Contains(item);
        }
		/// <summary>
		/// Copies the elements of the System.Collections.Generic.ICollection to an System.Array, starting at a particular System.Array index.
		/// </summary>
		/// <param name="array">
		/// The one-dimensional System.Array that is the destination of the elements
		/// copied from System.Collections.Generic.ICollection. The System.Array must
		/// have zero-based indexing.
		/// </param>
		/// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.Items.CopyTo(array, arrayIndex);
        }

		/// <summary>
		/// Removes the first occurrence of a specific object from the System.Collections.Generic.ICollection.
		/// </summary>
		/// <param name="item">The object to remove from the System.Collections.Generic.ICollection.</param>
		/// <returns>true if item was successfully removed from the System.Collections.Generic.ICollection;
		/// otherwise, false. This method also returns false if item is not found in
		/// the original System.Collections.Generic.ICollection.
		/// </returns>
        public bool Remove(T item)
        {
            OnBeforeItemRemoved(this, new GenericItemEventArgs<T>(item));
            bool happened = this.Items.Remove(item);
            OnItemRemoved(this, new EventArgs());
            return happened;
        }
        #endregion

        #region IEnumerable<T> Methods
		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>A System.Collections.Generic.IEnumerator that can be used to iterate through the collection.
		/// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }
        #endregion

        #region Event Methods
        /// <summary>
        /// Raises when an Item is added to the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">GenericItemEventArgs</param>
        protected virtual void OnItemAdded(object sender, GenericItemEventArgs<T> e)
        {
            if (ItemAdded != null)
                ItemAdded(sender, e);
        }
        /// <summary>
        /// Raises before an Item is added to the list.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GenericItemEventArgs</param>
        protected virtual void OnBeforeItemAdded(object sender, GenericItemEventArgs<T> e)
        {
            if (BeforeItemAdded != null)
                BeforeItemAdded(sender, e);
        }
        /// <summary>
        /// Raises when an Item is removed from the list.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventsArgs</param>
        protected virtual void OnItemRemoved(object sender, EventArgs e)
        {
            if (ItemRemoved != null)
                ItemRemoved(sender, e);
        }
        /// <summary>
        /// Raises before an Item is removed from the list.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GenericItemEventArgs</param>
        protected virtual void OnBeforeItemRemoved(object sender, GenericItemEventArgs<T> e)
        {
            if (BeforeItemRemoved != null)
                BeforeItemRemoved(sender, e);
        }
        /// <summary>
        /// Raises when the Items are cleared from this list.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected virtual void OnItemsCleared(object sender, EventArgs e)
        {
            if (ItemsCleared != null)
                ItemsCleared(sender, e);
        }
        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        #endregion

        #region ICollection Members

		/// <summary>
		/// Copies the entire collection to a compatible one-dimensional array, starting at the specified index of the target array.
		/// </summary>
        public void CopyTo(Array array, int index)
        {
            this.Items.CopyTo((T[]) array, index);
        }

		/// <summary>
		/// Is the collection synchronized. Always returns false.
		/// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public bool IsSynchronized
        {
            get 
            {
                return false;
            }
        }

		/// <summary>
		/// The SyncRoot object of the collection.
		/// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public object SyncRoot
        {
            get 
            {
                return null;                
            }
        }

        #endregion

        #region IList Members

		/// <summary>
		/// Adds an object to the end of the collection.
		/// </summary>
        public int Add(object value)
        {
            this.Items.Add((T) value);
            return this.Items.Count - 1;
        }			

		/// <summary>
		/// Determines whether an element is in the collection
		/// </summary>
        public bool Contains(object value)
        {
            return this.Items.Contains((T) value);
        }

		/// <summary>
		/// Searches for the specified object and returns the zero-based index of the
		///     first occurrence within the entire collection.
		/// </summary>
        public int IndexOf(object value)
        {
            return this.Items.IndexOf((T)value);
        }

		/// <summary>
		/// Inserts an element into the collection at the specified index.
		/// </summary>
        public void Insert(int index, object value)
        {
            this.Items.Insert(index, (T) value);
        }

		/// <summary>
		/// Returns true if the list size is fixed.
		/// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public bool IsFixedSize
        {
            get 
            {
                return false;
            }
        }

		/// <summary>
		/// Removes the first occurrence of a specific object from the collection.
		/// </summary>
        public void Remove(object value)
        {
            this.Items.Remove((T) value);
        }

        object IList.this[int index]
        {
            get
            {
                return this.Items[index];
            }
            set
            {
                this.Items[index] = (T) value;
            }
        }

        #endregion
    }

	/// <summary>
	/// Event argument class for the Generic List
	/// </summary>
	/// <typeparam name="T"></typeparam>
    public class GenericItemEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Item
        /// </summary>
        public T Item { get; private set; }
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="item"></param>
        public GenericItemEventArgs(T item)
        {
            this.Item = item;
        }
    }
}
