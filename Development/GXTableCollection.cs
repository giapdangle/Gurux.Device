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
using System.ComponentModel;
using System.Collections.Generic;
using Gurux.Device.Editor;
using System.Runtime.Serialization;
using Gurux.Device.Properties;

namespace Gurux.Device
{
	/// <summary>
    /// The GXTables component implements the GXTables interface. Collection of GXTable objects. 
    /// </summary>
    [TypeConverter(typeof(GXTypeConverterNoExpand))]
    [Serializable]
    public class GXTableCollection : GenericList<GXTable>, System.Collections.IList
	{
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

        /// <summary>
        /// Serialization constructor.
        /// </summary>
        internal GXTableCollection()
        {
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
			base.Add (value as GXTable);
			return this.Items.Count - 1;
		}

        /// <summary>
        /// Device increases tables ID after acreation.
        /// </summary>
        internal void UpdateID(ulong value)
        {
            foreach (GXTable it in this)
            {
                it.ID = value + (it.ID & 0xFFFF);
                foreach (GXProperty pr in it.Columns)
                {
                    pr.ID = value + (pr.ID & 0xFFFF);
                }
            }
        }

		/// <summary>
		/// Set item ID and parent.
		/// </summary>
        protected override void OnBeforeItemAdded(object sender, GenericItemEventArgs<GXTable> e)
        {
            if (e.Item.ID == 0)
            {
                e.Item.ID = GXDeviceList.IDGenerator.GenerateTemplateID();
            }
            else
            {
                GXDeviceList.IDGenerator.Add(e.Item.ID);
            }
            if (e.Item.Parent == null)
            {
                e.Item.Parent = this;
            }
            //Device is null when we serialize items.
            if (this.Parent is GXDevice)
            {
                GXDevice device = this.Parent as GXDevice;
                device.NotifyAdded(sender, new GXItemChangedEventArgs(e.Item, this.IndexOf(e.Item)));
            }
        }

		/// <summary>
		/// Remove item ID and parent.
		/// </summary>
        protected override void OnBeforeItemRemoved(object sender, GenericItemEventArgs<GXTable> e)
        {            
            if (e.Item.Parent == this)
            {
                if (this.Parent is GXDevice)
                {
                    GXDevice device = this.Parent as GXDevice;
                    device.NotifyRemoved(sender, new GXItemChangedEventArgs(e.Item, this.IndexOf(e.Item)));
                }
                GXDeviceList.IDGenerator.Remove(e.Item.ID);
                e.Item.ID = 0;
                e.Item.Parent = null;
            }
        }

		/// <summary>
		/// Remove item ID and parent for all items.
		/// </summary>
        public override void Clear()
        {
            if (this.Parent is GXDevice)
            {
                GXDevice device = this.Parent as GXDevice;
                device.NotifyClear(this, new GXItemEventArgs(this));
            }
            foreach (GXTable it in this)
            {
                if (it.Parent == this)
                {
                    GXDeviceList.IDGenerator.Remove(it.ID);
                    it.ID = 0;
                    it.Parent = null;
                }
            }
            base.Clear();
        }

		/// <summary>
		/// Parent object. Usually a GXDevice.
		/// </summary>
        [System.Xml.Serialization.XmlIgnore()]
        [Browsable(false)]
        public object Parent
        {
            get;
            internal set;
        }
		
		[System.Xml.Serialization.XmlIgnore()]
        GXDevice Device
        {
            get
            {
                return Parent as GXDevice;
            }
        }

		/// <summary>
		/// Name based indexer.
		/// </summary>
        public GXTable this[string name]
        {
            get
            {
                foreach (GXTable it in this)
                {
                    if (string.Compare(it.Name, name, true) == 0)
                    {
                        return it;
                    }
                }
                return null;
            }
            set
            {
                int pos = 0;
                foreach (GXTable it in this)
                {
                    if (string.Compare(it.Name, name, true) == 0)
                    {
                        this[pos] = value;
                        break;
                    }
                    ++pos;
                }
            }
        }
       
        /// <summary>
        /// Finds an item by its name.
        /// </summary>
        /// <param name="name">Name of the item to find.</param>
        /// <returns>Item found by given name.</returns>
        virtual public GXTable Find(string name)
		{
            foreach (GXTable table in this)
			{
				if (string.Compare(table.Name, name, true) == 0)
				{
					return table;
				}
			}
			return null;
		}

		/// <summary>
		/// Read this collection.
		/// </summary>
        public void Read()
        {
            this.Device.Read(this);
        }

		/// <summary>
		/// Write this collection.
		/// </summary>
		public void Write()
        {
            this.Device.Write(this);
        }

        /// <summary>
        /// Finds an item by its ID.
        /// </summary>
        /// <param name="id">ID of the item to find.</param>
        /// <returns>Item found by given ID.</returns>
        virtual public object FindItemByID(ulong id)
		{
			foreach (GXTable table in this)
			{
				if (table.ID == id)
				{
					return table;
				}
			}
			return null;
		}

		/// <summary>
		/// Determines the number of tables in the collection.
		/// </summary>
		[System.ComponentModel.Category("Design"), System.ComponentModel.Description("Determines the number of tables in the collection.")]
        [Browsable(false)]
        public new int Count
		{
			get
			{
                return base.Count;
			}
		}
        
        /// <summary>
        /// Retrieves a string representation of the value of the instance in the GXTables class.
        /// </summary>
        /// <returns>A string representation of the value of the instance.</returns>
		public override string ToString()
		{
			return "Tables";
		}		

		/// <summary>
        /// Checks if the properties in a table are valid. 
		/// </summary>
		/// <param name="designMode"></param>
		/// <param name="tasks">Collection of tasks.</param>
        public void Validate(bool designMode, GXTaskCollection tasks)
		{
			GXTable item = null;
			try
			{
				foreach (GXTable it in this)
				{
					item = it;
                    it.Validate(designMode, tasks);
				}
			}
			catch (Exception Ex)
			{
				throw new Exception(Resources.FiledToValidateTable + item.Name + ".\n" + Ex.Message);
			}
		}		
	}
}
