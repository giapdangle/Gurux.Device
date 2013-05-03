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
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace Gurux.Device
{
	/// <summary>
	/// A collection of GXSchedules.
	/// </summary>
    [CollectionDataContract()]
    public class GXScheduleCollection : GenericList<GXSchedule>, System.Collections.IList
    {        
		/// <summary>
		/// An event called when the collection is cleared.
		/// </summary>
        public event ItemClearEventHandler OnClear;
		/// <summary>
		/// An event called when an item is removed from the collection.
		/// </summary>
        public event ItemRemovedEventHandler OnRemoved;
		/// <summary>
		/// An event called when an item is updated in the collection.
		/// </summary>
        public event ItemUpdatedEventHandler OnUpdated;
		/// <summary>
		/// An event called when an item is added to the collection.
		/// </summary>
        public event ItemAddedEventHandler OnAdded;
        ///<summary>
        ///OnScheduleItemStateChanged notifies that the state of the GXSchedule has changed.        
		///</summary>
        public event ScheduleItemStateChangedEventHandler OnScheduleItemStateChanged;

		/// <summary>
		/// The parent GXDeviceList of the collection.
		/// </summary>
        public GXDeviceList Parent
        {
            get;
            internal set;
        }       
		
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
			base.Add (value as GXSchedule);
			return this.Items.Count - 1;
		}
		
		/// <summary>
		/// Event handled before an item is added to the collection
		/// </summary>
        protected override void OnBeforeItemAdded(object sender, GenericItemEventArgs<GXSchedule> e)
        {
            if (e.Item.ID == 0)
            {
                e.Item.ID = GXDeviceList.IDGenerator.GenerateID();
            }
            else
            {
                GXDeviceList.IDGenerator.Add(e.Item.ID);
            }
            if (e.Item.Parent == null)
            {
                e.Item.Parent = this;
            }
            NotifyAdded(this, new GXItemChangedEventArgs(e.Item, this.IndexOf(e.Item)));
        }

		/// <summary>
		/// Event handled before an item is removed from the collection
		/// </summary>
        protected override void OnBeforeItemRemoved(object sender, GenericItemEventArgs<GXSchedule> e)
        {
            if (e.Item.Parent == this)
            {
                e.Item.Stop();
                NotifyRemoved(this, new GXItemChangedEventArgs(e.Item, this.IndexOf(e.Item)));                
                GXDeviceList.IDGenerator.Remove(e.Item.ID);
                e.Item.ID = 0;
                e.Item.Parent = null;
            }
        }

		/// <summary>
		/// Clears the collection.
		/// </summary>
        public override void Clear()
        {
            NotifyClear(this, new GXItemEventArgs(this));
            if (Parent != null)
            {
                Parent.NotifyClear(this, new GXItemEventArgs(this));
            }
            foreach (GXSchedule it in this)
            {
                if (it.Parent == this)
                {
                    GXDeviceList.IDGenerator.Remove(it.ID);
                    it.Stop();
                    it.ID = 0;
                    it.Parent = null;
                }
            }
            base.Clear();
        }

		/// <summary>
		/// Returns schedules that are currently running.
		/// </summary>
		/// <returns></returns>
        public GXScheduleCollection GetActiveSchedules()
        {
            GXScheduleCollection activeSchedules = new GXScheduleCollection();
            lock (this)
            {
                foreach (GXSchedule it in this)
                {
                    if ((it.Status & ScheduleState.Run) != 0)
                    {
                        activeSchedules.Add(it);
                    }
                }
            }
            return activeSchedules;
        }

		/// <summary>
		/// Clears the collection and notifies parent about it.
		/// </summary>
        protected virtual void NotifyClear(object sender, GXItemEventArgs e)
        {
            if (OnClear != null)
            {
                OnClear(sender, e);
            }
            if (this.Parent != null)
            {
                this.Parent.NotifyClear(sender, e);
            }
        }

		/// <summary>
		/// Removes an item from the collection and notifies parent about it.
		/// </summary>
        protected virtual void NotifyRemoved(object sender, GXItemChangedEventArgs e)
        {
            if (OnRemoved != null)
            {
                OnRemoved(sender, e);
            }
            if (this.Parent != null)
            {
                this.Parent.NotifyRemoved(sender, e);
            }
        }

		/// <summary>
		/// Updates an item in the collection and notifies parent about it.
		/// </summary>
        internal virtual void NotifyUpdated(object sender, GXItemEventArgs e)
        {
            if (OnUpdated != null)
            {
                OnUpdated(sender, e);
            }
            if (this.Parent != null)
            {
                this.Parent.NotifyUpdated(sender, e);
            }
        }

		/// <summary>
		/// Adds an item in the collection and notifies parent about it.
		/// </summary>
        protected virtual void NotifyAdded(object sender, GXItemChangedEventArgs e)
        {
            if (OnAdded != null)
            {
                OnAdded(sender, e);
            }
            if (this.Parent != null)
            {
                this.Parent.NotifyAdded(sender, e);
            }
        }

		/// <summary>
		/// Invokes OnScheduleItemStateChanged event.
		/// </summary>
        internal void NotifyChange(object sender, GXScheduleEventArgs e)
        {
            if (OnScheduleItemStateChanged != null)
            {
                OnScheduleItemStateChanged(sender, e);
            }
            NotifyUpdated(sender, e);
        }
    }
}
