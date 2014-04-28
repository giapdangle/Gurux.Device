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
using Gurux.Device.Editor;
using System.ComponentModel;
using Gurux.Device.Properties;

namespace Gurux.Device
{
	/// <summary>
	/// A collection of GXDeviceGroups.
	/// </summary>
    public class GXDeviceGroupCollection : GenericList<GXDeviceGroup>, System.Collections.IList
    {
		/// <summary>
		/// The parent object of the GXDeviceGroups collection.
		/// </summary>
        public object Parent
        {
            get;
            internal set;
        }

		/// <summary>
		/// The parent GXDeviceList of the collection.
		/// </summary>
        [Browsable(false)]
        public GXDeviceList DeviceList
        {
            get
            {
                object parent = Parent;
                while (!(parent is GXDeviceList))
                {
                    if (parent == null)
                    {
                        return null;
                    }
                    if (parent is GXDeviceGroup)
                    {
                        parent = ((GXDeviceGroup)parent).Parent.Parent;
                    }
                    if (parent is GXDeviceGroupCollection)
                    {
                        parent = ((GXDeviceGroupCollection)parent).Parent;
                    }
                }
                return parent as GXDeviceList;
            }
        }

        /// <summary>
        /// FindItemByID finds an item using item identification.
        /// </summary>
        /// <param name="id">Item ID</param>
        /// <returns>
        /// Returns any item whose ID matches the given ID.
        /// </returns>
        public virtual object FindItemByID(UInt64 id)
        {
            if (id == 0)
            {
                throw new Exception(Resources.FindItemByIDFailedSearchIDCanTBeZero);
            }
            object item = null;
            foreach (GXDeviceGroup it in this)
            {
                item = it.FindItemByID(id);
                if (item != null)
                {
                    return item;
                }
            }
            return null;
        }

		/// <summary>
		/// Gets a value indicating whether the collection is read-only.
		/// </summary>
        [System.ComponentModel.Browsable(false)]
        public new bool IsReadOnly
        {
            get
            {
                return base.IsReadOnly;
            }
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
			base.Add (value as GXDeviceGroup);
			return this.Items.Count - 1;
		}
		
		/// <summary>
		/// Set item ID and parent. 
		/// </summary>
        protected override void OnBeforeItemAdded(object sender, GenericItemEventArgs<GXDeviceGroup> e)
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
            GXDeviceList list = DeviceList;
            if (list != null)
            {
                list.NotifyAdded(this, new GXItemChangedEventArgs(e.Item, this.Items.Count));
            }
        }

		/// <summary>
		/// Remove the device group from all schedules in the parent device list.
		/// </summary>
        void RemoveDeviceGroupFromSchedules(GXDeviceGroup group)
        {
            GXDeviceList list = DeviceList;
            if (list != null)
            {
                foreach(GXSchedule it in list.Schedules)
                {
                    it.Items.Remove(group);
                    it.ExcludedItems.Remove(group);
                }            
            }
        }

        /// <summary>
        /// Move items to other collection.
        /// </summary>
        /// <remarks>
        /// This method is used when we are deserializing items.
        /// </remarks>
        /// <param name="list"></param>
        internal void MoveTo(GXDeviceGroupCollection list)
        {
            list.AddRange(this);
            foreach(GXDeviceGroup it in this)         
            {
                it.Parent = list;
            }
        }

		/// <summary>
		/// Remove item ID and parent. 
		/// </summary>
        protected override void OnBeforeItemRemoved(object sender, GenericItemEventArgs<GXDeviceGroup> e)
        {
            if (e.Item.Parent == this)
            {
                GXDeviceList list = DeviceList;
                if (list != null)
                {
                    list.NotifyRemoved(this, new GXItemChangedEventArgs(e.Item, this.IndexOf(e.Item)));
                }
                //Remove item from schedules if any...
                RemoveDeviceGroupFromSchedules(e.Item);
                e.Item.Devices.Clear();
                e.Item.DeviceGroups.Clear();
                GXDeviceList.IDGenerator.Remove(e.Item.ID);
                e.Item.ID = 0;
                e.Item.Parent = null;
            }
        }

		/// <summary>
		/// Remove item ID and parent from all the items.
		/// </summary>
        public override void Clear()
        {
            GXDeviceList list = DeviceList;
            if (list != null)
            {
                list.NotifyClear(this, new GXItemEventArgs(this));
            }
            foreach (GXDeviceGroup it in this)
            {
                if (it.Parent == this)
                {
                    it.Devices.Clear();
                    it.DeviceGroups.Clear();
                    RemoveDeviceGroupFromSchedules(it);
                    GXDeviceList.IDGenerator.Remove(it.ID);
                    it.ID = 0;
                    it.Parent = null;
                }
            }
            base.Clear();
        }

		/// <summary>
		/// Read all the child GXDeviceGroups.
		/// </summary>
        public void Read()
        {
            foreach (GXDeviceGroup it in this)
            {
                it.Read();
            }        
        }

		/// <summary>
		/// Write all the child GXDeviceGroups.
		/// </summary>
        public void Write()
        {
            foreach (GXDeviceGroup it in this)
            {
                it.Write();
            } 
        }

		/// <summary>
		/// Connect all the child GXDeviceGroups.
		/// </summary>
        public void Connect()
        {
            foreach (GXDeviceGroup it in this)
            {
                it.Connect();
            }
        }

		/// <summary>
		/// Disconnect all the child GXDeviceGroups.
		/// </summary>
        public void Disconnect()
        {
            foreach (GXDeviceGroup it in this)
            {
                it.Disconnect();
            }
        }

		/// <summary>
		/// Start monitoring all the child GXDeviceGroups.
		/// </summary>
        public void StartMonitoring()
        {
            foreach (GXDeviceGroup it in this)
            {
                if ((it.DisabledActions & DisabledActions.Monitor) != 0)
                {
                    it.StartMonitoring();
                }
            }
        }

		/// <summary>
		/// Stop monitoring all the child GXDeviceGroups.
		/// </summary>
        public void StopMonitoring()
        {
            foreach (GXDeviceGroup it in this)
            {
                it.StopMonitoring();
            }
        }

		/// <summary>
		/// Reset all the child GXDeviceGroups.
		/// </summary>
        public void Reset(ResetTypes type)
        {
            foreach (GXDeviceGroup it in this)
            {
                it.Reset(type);
            }
        }

        /// <summary>
        /// Reset statistic value of determined type.
        /// </summary>
        public void ResetStatistic()
        {
            foreach (GXDeviceGroup it in this)
            {
                it.ResetStatistic();
            }            
        }

		/// <summary>
		/// Get all the devices from all the child device groups recursively.
		/// </summary>
        public GXDeviceCollection GetDevicesRecursive()
        {
            GXDeviceCollection devices = new GXDeviceCollection();
            foreach (GXDeviceGroup it in this)
            {                
				devices.AddRange(it.GetDevicesRecursive());
            }
            return devices;
        }        

        internal virtual void OnDeviceGroupUpdated(object sender, GXItemEventArgs e)
        {           
            if (Parent is GXDeviceGroup)
            {
                ((GXDeviceGroup) Parent).NotifyDeviceGroupUpdated(e);                
            }
            else if (Parent is GXDeviceList)
            {
                ((GXDeviceList)Parent).NotifyUpdated(sender, e);
            }
        }
    }
}
