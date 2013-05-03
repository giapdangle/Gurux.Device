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

using System;
using System.Xml;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Gurux.Device.Editor
{
	/// <summary>
    /// Collection of tasks to fix, in order to make the device work properly. 
	/// </summary>
    public class GXTaskCollection : GenericList<GXTask>, System.Collections.IList
	{
		/// <summary>
		/// Event that is invoked when the collection is cleared.
		/// </summary>
        public event ItemClearEventHandler OnClear;
		/// <summary>
		/// Event that is invoked when an item is removed from the collection.
		/// </summary>
        public event ItemRemovedEventHandler OnRemoved;
		/// <summary>
		/// Event that is invoked when an item is added to the collection.
		/// </summary>
        public event ItemAddedEventHandler OnAdded;
		
		/// <summary>
		/// Add the specified value.
		/// </summary>
		/// <param name='value'>
		/// Value.
		/// </param>
		/// <remarks>
		/// Mono needs this. Do not remove!
		/// </remarks>
		int System.Collections.IList.Add (object value)
		{
			base.Add (value as GXTask);
			return this.Items.Count - 1;
		}
		
		/// <summary>
		/// Invokes OnAdded event and sets the item Parent.
		/// </summary>
        protected override void OnBeforeItemAdded(object sender, GenericItemEventArgs<GXTask> e)
        {
            if (e.Item.Parent == null)
            {
                e.Item.Parent = this;
            }
            if (OnAdded != null)
            {
                OnAdded(sender, new GXItemChangedEventArgs(e.Item, this.Count));
            }
        }

		/// <summary>
		/// Invokes OnClear event and clears Parent for items.
		/// </summary>
        public override void Clear()
        {
            if (OnClear != null)
            {
                OnClear(this, new GXItemEventArgs(this));
            }
            foreach (GXTask it in this)
            {
                if (it.Parent == this)
                {
                    it.Parent = null;
                }
            }
            base.Clear();
        }

		/// <summary>
		/// Invokes OnRemoved event and clears the item Parent.
		/// </summary>
        protected override void OnBeforeItemRemoved(object sender, GenericItemEventArgs<GXTask> e)
        {
            if (OnRemoved != null)
            {
                OnRemoved(sender, new GXItemChangedEventArgs(e.Item, this.Count));
            }
            if (e.Item.Parent == this)
            {
                e.Item.Parent = null;
            }
        }

		/// <summary>
		/// Initializes a new instance of the GXTaskCollection class.
		/// </summary>
		public GXTaskCollection()
		{            
		}

		/// <summary>
		/// The parent of the collection.
		/// </summary>
        public object Parent
        {
            get;
            internal set;
        }

		/// <summary>
		/// Resets tasks before validation.
		/// </summary>
		public void ResetTasks()
		{
			foreach (GXTask it in this)
			{
				//If task exists mark it dirty.
				if (it.Source != null)
				{
					it.Dirty = false;
				}
			}
		}

		/// <summary>
		/// Removes unused tasks after validation.
		/// </summary>
		/// <param name="source">The source of the task. Null, if task is added by a user.</param>
        /// <param name="dirtyOnly">False, to remove only the tasks that have not changed.</param>
		public void RemoveUnusedTasks(object source, bool dirtyOnly)
		{
			for (int pos = 0; pos < this.Count; ++pos)
			{
                GXTask it = (GXTask)this[pos];
				if (it.Source == source && (!it.Dirty || !dirtyOnly))
				{
                    Remove(this[pos]);
					--pos;
				}
			}
		}
	}
}
