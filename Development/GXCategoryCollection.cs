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

namespace Gurux.Device
{
	/// <summary>
	/// The GXCategoryCollection collection component implements the GXCategoryCollection interface. 
    /// The GXCategoryCollection is the collection of categories, used to arrange GXProperty objects. 
	/// </summary>    
    [TypeConverter(typeof(GXTypeConverterNoExpand))]
    [Serializable]
    public class GXCategoryCollection : GenericList<GXCategory>, System.Collections.IList
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
		/// Add the specified value.
		/// </summary>
		/// <param name='value'>
		/// Value.
		/// </param>
		/// <remarks>
		/// Mono needs this. Do not remove!
		/// </remarks>
		int System.Collections.IList.Add(object value)
		{
			base.Add (value as GXCategory);
			return this.Items.Count - 1;
		}

        /// <summary>
        /// Device increases categories and properties ID after creation.
        /// </summary>
        internal void UpdateID(ulong value)
        {
            foreach (GXCategory cat in this)
            {
                cat.ID = value + (cat.ID & 0xFFFF);
                foreach (GXProperty it in cat.Properties)
                {
                    it.ID = value + (it.ID & 0xFFFF);
                }
            }
        }

		/// <summary>
		/// Sets item ID, parent and notifies parent device.
		/// </summary>
        protected override void OnBeforeItemAdded(object sender, GenericItemEventArgs<GXCategory> e)
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
            GXDevice device = this.Parent as GXDevice;
            //Device is null when we serialize items.
            if (device != null && device.Site != null)
            {                
                device.NotifyAdded(sender, new GXItemChangedEventArgs(e.Item, this.IndexOf(e.Item)));
            }
        }

		/// <summary>
		/// Removes item ID and parent.
		/// </summary>
        protected override void OnBeforeItemRemoved(object sender, GenericItemEventArgs<GXCategory> e)
        {            
            if (this.Parent is GXDevice)
            {
                GXDevice device = this.Parent as GXDevice;
                device.NotifyRemoved(sender, new GXItemChangedEventArgs(e.Item, this.IndexOf(e.Item)));
            }            
            
            if (e.Item.Parent == this)
            {
                e.Item.Properties.Clear();
                GXDeviceList.IDGenerator.Remove(e.Item.ID);
                e.Item.ID = 0;
                e.Item.Parent = null;
            }
        }

		/// <summary>
		/// Removes item ID and parent for all items.
		/// </summary>
        public override void Clear()
        {
            if (this.Parent is GXDevice)
            {
                GXDevice device = this.Parent as GXDevice;
                device.NotifyClear(this, new GXItemEventArgs(this));
            }

            foreach (GXCategory it in this)
            {                
                if (it.Parent == this)
                {
                    it.Properties.Clear();
                    GXDeviceList.IDGenerator.Remove(it.ID);
                    it.ID = 0;
                    it.Parent = null;
                }
            }
            base.Clear();
        }
        
		/// <summary>
		/// Name based indexer.
		/// </summary>
        public GXCategory this[string name]
        {
            get
            {
                foreach (GXCategory it in this)
                {
                    if (string.Compare(it.Name, name, true) == 0)
                    {
                        return it;
                    }
                }                
                return null;
            }
            /*
            set
            {
                int pos = 0;
                foreach (GXCategory it in this)
                {
                    if (string.Compare(it.Name, name, true) == 0)
                    {
                        this[pos] = value;
                        break;
                    }
                    ++pos;
                }
            }
             * */
        }

		/// <summary>
		/// Parent object of the collection. Usually a GXDevice.
		/// </summary>
        [Browsable(false)]
        [System.Xml.Serialization.XmlIgnore()]
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
		/// Read the collection.
		/// </summary>
        public void Read()
        {
            this.Device.Read(this);
        }

		/// <summary>
		/// Write the collection.
		/// </summary>
        public void Write()
        {
            this.Device.Write(this);
        }

        /// <summary>
        /// Serialization constructor.
        /// </summary>
        public GXCategoryCollection()
        {
            return;
        }

        /// <summary>
        /// Finds an item by its name.
        /// </summary>
        /// <param name="name">Name of the item to find.</param>
        /// <returns>Item found by given name.</returns>
		public virtual GXCategory Find(string name)
		{
			foreach (GXCategory cat in this)
			{
				if (string.Compare(cat.Name, name, true) == 0)
				{
					return cat;
				}
			}
			return null;
		}


		/// <summary>
		/// Finds an item by its ID.
		/// </summary>
		/// <param name="id">ID of the item to find.</param>
        /// <returns>Item found by given ID.</returns>
        virtual public object FindItemByID(ulong id)
		{
			object prop = null;
			foreach (GXCategory cat in this)
			{
				if (cat.ID == id)
				{
					return cat;
				}
                if ((prop = cat.Properties.FindItemByID(id)) != null)
				{
					return prop;
				}
			}
			return null;
		}
		

        /// <summary>
        /// Retrieves a string representation of the value of the instance in the GXCategories class.
        /// </summary>
        /// <returns>A string representation of the value of the instance.</returns>
		public override string ToString()
		{
			return "Categories";
		}

		/// <summary>
		/// Indicates the number of items in the collection.
		/// </summary>
		[System.ComponentModel.Category("Design"), System.ComponentModel.Description("Indicates the number of items in the collection.")]
        [Browsable(false)]
        public new int Count
		{
			get
			{
                return base.Count;
			}
		}

        /// <summary>
        /// Checks if the properties in a category are valid. 
        /// </summary>
		/// <param name="designMode"></param>
        /// <param name="tasks">Collection of tasks.</param>
        public void Validate(bool designMode, GXTaskCollection tasks)
		{
			foreach (GXCategory it in this)
			{
                it.Validate(designMode, tasks);
			}
		}
      
		/// <summary>
		/// Checks if the category is in use, and can not be removed.
		/// </summary>
		/// <param name="categoryName">The name of the category to check.</param>
		/// <param name="device">The GXDevice that the category belongs to.</param>
		/// <returns>The amount of properties in the category.</returns>
		public GXProperty IsCatecoryInUse(string categoryName, ref GXDevice device)
		{
			GXCategory cat = (GXCategory)device.Categories.Find(categoryName);
			if (cat != null && cat.Properties.Count > 0)
			{
				return cat.Properties[0];
			}
			return null;
		}		
	}
}
