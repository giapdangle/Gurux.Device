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
using Gurux.Device.Editor;

namespace Gurux.Device
{
	/// <summary>
    /// Collection of GXProperty objects. GXPropertyCollection represents properties of a physical device. 
	/// </summary>    
    [Serializable]
    public class GXPropertyCollection : GenericList<GXProperty>, System.Collections.IList
	{
		/// <summary>
		/// Returns if the list is read only.
		/// </summary>
        [System.ComponentModel.Browsable(false)]
        [System.Xml.Serialization.XmlIgnore()]
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
			base.Add (value as GXProperty);
			return this.Items.Count - 1;
		}

		/// <summary>
		/// Sets item parent and ID
		/// </summary>
        protected override void OnBeforeItemAdded(object sender, GenericItemEventArgs<GXProperty> e)
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
            NotifyAdded(this, new GXItemChangedEventArgs(e.Item, this.IndexOf(e.Item)));
        }

		/// <summary>
		/// Removes item parent and ID
		/// </summary>
        protected override void OnBeforeItemRemoved(object sender, GenericItemEventArgs<GXProperty> e)
        {            
            if (e.Item.Parent == this)
            {
                NotifyRemoved(this, new GXItemChangedEventArgs(e.Item, this.IndexOf(e.Item)));
                GXDeviceList.IDGenerator.Remove(e.Item.ID);
                e.Item.ID = 0;
                e.Item.Parent = null;
            }
        }

		/// <summary>
		/// Removes items and clears their parent info and ID.
		/// </summary>
        public override void Clear()
        {
            NotifyClear(this, new GXItemEventArgs(this));
            foreach (GXProperty it in this)
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
        /// Serialization constructor.
        /// </summary>
        public GXPropertyCollection()
        {
        }

		/// <summary>
		/// The parent object. Usually GXCategory or GXTable.
		/// </summary>
        [System.Xml.Serialization.XmlIgnore()]
        public object Parent
        {
            get;
            internal set;
        }

		/// <summary>
		/// Resets the child properties.
		/// </summary>
        public void Reset(ResetTypes type)
        {
			//TODO: implement
        }

		/// <summary>
		/// Finds an item by its name. 
		/// </summary>
		/// <param name="name">Name of the property to find.</param>
        /// <param name="excludedProperty">Excluded property.</param>
		/// <returns>Item found by given name.</returns>
        public virtual GXProperty Find(string name, GXProperty excludedProperty)
		{
			foreach (GXProperty Prop in this)
			{
				if (excludedProperty != Prop && string.Compare(Prop.Name, name, true) == 0)
				{
					return Prop;
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
            foreach (GXProperty Prop in this)
			{
				if (Prop.ID == id)
				{
					return Prop;
				}
			}
			return null;
		}	

		/// <summary>
        /// Checks if the parameter values of a property are valid.
		/// </summary>
		/// <param name="tasks">Collection of tasks.</param>
		/// <param name="designMode"></param>
        public void Validate(bool designMode, GXTaskCollection tasks)
		{
			foreach (GXProperty it in this)
			{
                it.Validate(designMode, tasks);
			}
		}

        internal virtual void PropertyChanged(object sender, GXPropertyEventArgs e)
        {
            if (OnUpdated != null)
            {
                OnUpdated(sender, e);
            }
            if (this.Parent is GXCategory)
            {
                GXCategory cat = this.Parent as GXCategory;
                cat.NotifyUpdated(sender, e);
            }
        }

		internal virtual void NotifyDisplayTypeChanged(DisplayTypes type)
		{
			if (OnDisplayTypeChanged != null)
			{
				OnDisplayTypeChanged(type);
			}
		}
        
		/// <summary>
		/// Event called when display type is changed.
		/// </summary>
        public event DisplayTypeChangedEventHandler OnDisplayTypeChanged;

		/// <summary>
		/// Event called when the collection is cleared.
		/// </summary>
        public event ItemClearEventHandler OnClear;

		/// <summary>
		/// Event called when an item is removed from the collection.
		/// </summary>
		public event ItemRemovedEventHandler OnRemoved;

		/// <summary>
		/// Event called when an item is updated in the collection.
		/// </summary>
        public event ItemUpdatedEventHandler OnUpdated;

		/// <summary>
		/// Event called when an item is added to the collection.
		/// </summary>
        public event ItemAddedEventHandler OnAdded;


        internal GXDevice GetDevice()
        {
            if (this.Parent is GXCategory)
            {
                GXCategory cat = this.Parent as GXCategory;
                if (cat != null && cat.Parent != null)
                {
                    return cat.Parent.Parent as GXDevice;
                }
            }
            if (this.Parent is GXTable)
            {
                GXTable table = this.Parent as GXTable;
                if (table != null && table.Parent != null)
                {
                    return table.Parent.Parent as GXDevice;                    
                }
            }
            return null;
        }

		/// <summary>
		/// Notify clear and handle the event.
		/// </summary>
        protected virtual void NotifyClear(object sender, GXItemEventArgs e)
        {
            GXDevice device = GetDevice();
            if (device != null)
            {
                device.NotifyClear(sender, e);
            }
            if (OnClear != null)
            {
                OnClear(sender, e);
            }
        }

		/// <summary>
		/// Notify item removed and handle the event.
		/// </summary>
        protected virtual void NotifyRemoved(object sender, GXItemChangedEventArgs e)
        {
            GXDevice device = GetDevice();
            if (device != null)
            {
                device.NotifyRemoved(sender, e);
            }            
            if (OnRemoved != null)
            {
                OnRemoved(sender, e);
            }
        }

		/// <summary>
		/// Notify item updated and handle the event.
		/// </summary>
        internal void NotifyUpdated(object sender, GXItemEventArgs e)
        {
            if (OnUpdated != null)
            {
                OnUpdated(sender, e);
            }
            if (this.Parent is GXCategory)
            {
                ((GXCategory)this.Parent).NotifyUpdated(sender, e);
            }
        }

		/// <summary>
		/// Notify item added and handle the event.
		/// </summary>
        protected virtual void NotifyAdded(object sender, GXItemChangedEventArgs e)
        {
            GXDevice device = GetDevice();
            if (device != null)
            {
                device.NotifyAdded(sender, e);
            }
            if (OnAdded != null)
            {
                OnAdded(sender, e);
            }
        }
    }
}
