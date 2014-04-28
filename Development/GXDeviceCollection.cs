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
using Microsoft.Win32;
using System.Xml;
using System.Xml.Schema;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections;
using Gurux.Device.Editor;
using System.Collections.Generic;
using Gurux.Device.Properties;

namespace Gurux.Device
{
	/// <summary>
	/// A collection of GXDevices.
	/// </summary>
    public class GXDeviceCollection : GenericList<GXDevice>, System.Collections.IList
    {
		/// <summary>
		/// The parent object of the collection. Usually a GXDeviceGroup.
		/// </summary>
        [Browsable(false)]
        public object Parent
        {
            get;
            internal set;
        }

		/// <summary>
		/// Gets a value indicating whether the collection is read-only.
		/// </summary>
        [Browsable(false)]
        public new bool IsReadOnly
        {
            get
            {
                return base.IsReadOnly;
            }
        }
        
        internal GXDeviceList GetDeviceList()
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
                    GXDeviceGroupCollection tmp = ((GXDeviceGroup)parent).Parent;
                    if (tmp == null)
                    {
                        return null;
                    }
                    parent = tmp.Parent;
                }
                if (parent is GXDeviceGroupCollection)
                {
                    parent = ((GXDeviceGroupCollection)parent).Parent;
                }
            }
            return parent as GXDeviceList;
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
            foreach (GXDevice it in this)
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
			base.Add (value as GXDevice);
			return this.Items.Count - 1;
		}

		/// <summary>
		/// Sets the item ID and parent
		/// </summary>
        protected override void OnBeforeItemAdded(object sender, GenericItemEventArgs<GXDevice> e)
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
            GXDeviceList list = GetDeviceList();
            if (list != null)
            {
                list.NotifyAdded(this, new GXItemChangedEventArgs(e.Item, this.Items.Count));
            }
        }

        void RemoveDeviceFromSchedules(GXDevice device)
        {

        }

		/// <summary>
		/// Removes the item ID and parent
		/// </summary>
        protected override void OnBeforeItemRemoved(object sender, GenericItemEventArgs<GXDevice> e)
        {
            GXDeviceList list = GetDeviceList();
            if (list != null)
            {
                list.NotifyRemoved(this, new GXItemChangedEventArgs(e.Item, this.IndexOf(e.Item)));
            }
            if (e.Item.Parent == this)
            {
                e.Item.Disconnect();
                RemoveDeviceFromSchedules(e.Item);
                e.Item.Categories.Clear();
                e.Item.Tables.Clear();
                GXDeviceList.IDGenerator.Remove(e.Item.ID);
                e.Item.ID = 0;
                e.Item.Parent = null;
            }
        }

		/// <summary>
		/// Disconnect and remove all items from all relevant collections.
		/// </summary>
        public override void Clear()
        {
            GXDeviceList list = GetDeviceList();
            if (list != null)
            {
                list.NotifyClear(this, new GXItemEventArgs(this));
            }
            foreach (GXDevice it in this)
            {
                if (it.Parent == this)
                {
					if (it.GXClient != null)
					{
						it.GXClient.CloseServer();
					}
                    it.Disconnect();
                    it.Categories.Clear();
                    it.Tables.Clear();
                    RemoveDeviceFromSchedules(it);
                    GXDeviceList.IDGenerator.Remove(it.ID);
                    it.ID = 0;
                    it.Parent = null;
                }
            }            
            base.Clear();
        }

		/// <summary>
		/// Read all child GXDevices
		/// </summary>
        public void Read()
        {
            foreach (GXDevice it in this)
            {
                it.Read(this);
            }
        }

		/// <summary>
		/// Write all child GXDevices
		/// </summary>
        public void Write()
        {
            foreach (GXDevice it in this)
            {
                it.Write(this);
            }
        }

		/// <summary>
		/// Connect all child GXDevices
		/// </summary>
        public void Connect()
        {
			foreach (GXDevice it in this)
			{
				it.Connect();
			}
        }

		/// <summary>
		/// Disconnect all child GXDevices
		/// </summary>
        public void Disconnect()
        {
			foreach (GXDevice it in this)
			{
				it.Disconnect();
			}
        }

		/// <summary>
		/// StartMonitoring all child GXDevices
		/// </summary>
        public void StartMonitoring()
        {
			foreach (GXDevice it in this)
			{
				it.StartMonitoring();
			}
        }

		/// <summary>
		/// StopMonitoring all child GXDevices
		/// </summary>
        public void StopMonitoring()
        {
			foreach (GXDevice it in this)
			{
				it.StopMonitoring();
			}
        }

		/// <summary>
		/// Reset all child GXDevices
		/// </summary>
        public void Reset(ResetTypes type)
        {
			foreach (GXDevice it in this)
			{
				it.Reset(type);
			}
        }

        /// <summary>
        /// Reset statistic value of determined type.
        /// </summary>
        public void ResetStatistic()
        {
            foreach (GXDevice it in this)
            {
                it.ResetStatistic();
            }
        }
    }
}
