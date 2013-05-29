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

namespace Gurux.Device
{    
#region CommonEvents

	/// <summary>
	/// Event argument base class that is used with events of GXObjects suchs as GXDevice.
	/// </summary>
    public class GXItemEventArgs : EventArgs
    {
        /// <summary>
        /// Item
        /// </summary>
        public virtual object Item 
        { 
            get; 
            internal set; 
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="item"></param>
        public GXItemEventArgs(object item)
        {
            this.Item = item;
        }
    }

	/// <summary>
	/// Event argument class that is used with various Changed events.
	/// </summary>
    public class GXItemChangedEventArgs : GXItemEventArgs
    {
        /// <summary>
        /// Index
        /// </summary>
        public int Index
        {
            get;
            private set;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public GXItemChangedEventArgs(object item, int index) : base(item)
        {
            Index = index;
        }
    }

	/// <summary>
	/// Event argument class that is used with various GXDevice events.
	/// </summary>
    public class GXDeviceEventArgs : GXItemEventArgs
    {
        /// <summary>
        /// Item
        /// </summary>
        public DeviceStates Status
        {
            get;
            private set;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public GXDeviceEventArgs(object device, DeviceStates status)
            : base(device)
        {
            Status = status;
        }
    }

	/// <summary>
	/// Event argument class for GXCategory. Used in update notifications.
	/// </summary>
    public class GXCategoryEventArgs : GXItemEventArgs
    {
        /// <summary>
        /// Item
        /// </summary>
        public CategoryStates Status
        {
            get;
            private set;
        }

        /// <summary>
        /// GXCategory
        /// </summary>
        public new GXCategory Item
        {
            get
            {
                return base.Item as GXCategory;
            }
            private set
            {
                base.Item = value;
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public GXCategoryEventArgs(GXCategory category, CategoryStates status)
            : base(category)
        {
            Status = status;
        }
    }

	/// <summary>
	/// Event arguments class for GXTable.
	/// </summary>
    public class GXTableEventArgs : GXItemEventArgs
    {
        /// <summary>
        /// Item
        /// </summary>
        public TableStates Status
        {
            get;
            private set;
        }

        /// <summary>
        /// Item
        /// </summary>
        public new GXTable Item
        {
            get
            {
                return base.Item as GXTable;
            }
            private set
            {
                base.Item = value;
            }
        }

        /// <summary>
        /// Changed rows.
        /// </summary>
        public List<object[]> Rows
        {
            get;
            private set;
        }

		/// <summary>
		/// Index
		/// </summary>
        public int Index
        {
            get;
            private set;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public GXTableEventArgs(GXTable table, TableStates status, int index, List<object[]> rows)
            : base(table)
        {
            Rows = rows;
            Status = status;
            Index = index;
        }        
    }

	/// <summary>
	/// Event arguments class for GXProperty.
	/// </summary>
    public class GXPropertyEventArgs : GXItemEventArgs
    {
        /// <summary>
        /// Item
        /// </summary>
        public PropertyStates Status
        {
            get;
            private set;
        }

        /// <summary>
        /// Item
        /// </summary>
        public new GXProperty Item
        {
            get
            {
                return base.Item as GXProperty;
            }
            private set
            {
                base.Item = value;
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public GXPropertyEventArgs(GXProperty prop, PropertyStates status)
            : base(prop)
        {
            Status = status;
        }
    }

	/// <summary>
	/// Event argument class for GXSchedule.
	/// </summary>
    public class GXScheduleEventArgs : GXItemEventArgs
    {
        /// <summary>
        /// Item
        /// </summary>
        public ScheduleState Status
        {
            get;
            private set;
        }

        /// <summary>
        /// Item
        /// </summary>
        public new GXSchedule Item
        {
            get
            {
                return base.Item as GXSchedule;
            }
            private set
            {
                base.Item = value;
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public GXScheduleEventArgs(GXSchedule schedule, ScheduleState status)
            : base(schedule)
        {
            Status = status;
        }
    }
    
	/// <summary>
	/// Event argument class for GXTransactionProgress.
	/// </summary>
    public class GXTransactionProgressEventArgs : EventArgs
    {
        /// <summary>
        /// Item
        /// </summary>
        public object Item
        {
            get;
            private set;
        }

        /// <summary>
        /// Index of the transaction.
        /// </summary>
        public Int32 Current
        {
            get;
            private set;
        }

        /// <summary>
        /// Total amount of transactions.
        /// </summary>
        public Int32 Maximum
        {
            get;
            private set;
        }

		/// <summary>
		/// The current status of the item.
		/// </summary>
        public DeviceStates Status
        {
            get;
            private set;
        }

        /// <summary>
        /// User wants to cancel the transaction.
        /// </summary>
        public bool Cancel
        {
            get;
            set;
        }
       
        /// <summary>
        /// Default constructor
        /// </summary>
        public GXTransactionProgressEventArgs(object item, Int32 index, Int32 count, DeviceStates status)
        {
            Status = status;
            Cancel = false;
            this.Item = item;
            this.Current = index;
            this.Maximum = count;
        }
    }

	/// <summary>
	/// Event argument class for GXDirty.
	/// </summary>
    public class GXDirtyEventArgs : EventArgs
    {
        /// <summary>
        /// Item
        /// </summary>
        public object Item
        {
            get;
            private set;
        }

        /// <summary>
        /// IsDirty
        /// </summary>
        public bool IsDirty
        {
            get;
            private set;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public GXDirtyEventArgs(object item, bool dirty)
        {
            IsDirty = dirty;
            this.Item = item;
        }
    }

	/// <summary>
	/// Event argument class for GXSelectedItem.
	/// </summary>
    public class GXSelectedItemEventArgs : EventArgs
    {
        /// <summary>
        /// Item
        /// </summary>
        public object NewItem
        {
            get;
            private set;
        }


        /// <summary>
        /// Item
        /// </summary>
        public object OldItem
        {
            get;
            private set;
        }

		/// <summary>
		/// If the item can be changed.
		/// </summary>
        public bool CanChange
        {
            get;
            set;
        }

        /// <summary>
        /// Used customUI.
        /// </summary>
        public System.Windows.Forms.Form CustomUI
        {
            get;
            set;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public GXSelectedItemEventArgs(object newItem, object oldItem)
        {
            CanChange= true;
            NewItem = newItem;
            OldItem = oldItem;            
        }
    }    

	/// <summary>
	/// Notifies, when the content of an object is changing.
	/// </summary>
    /// <param name="sender">The target that is changing.</param>
	/// <param name="e">Event arguments</param>
	public delegate void BeginUpdateEventHandler(object sender, GXItemEventArgs e);
	
	/// <summary>
	/// Notifies, when the content of an object has changed.
	/// </summary>
    /// <param name="sender">The target that has changed.</param>
	/// <param name="e">Event arguments</param>
	public delegate void EndUpdateEventHandler(object sender, GXItemEventArgs e);

	/// <summary>
	/// Notifies, when the display type has changed.
	/// </summary>
	/// <param name="displayType">The new displayType state.</param>
	public delegate void DisplayTypeChangedEventHandler(DisplayTypes displayType);

	/// <summary>
	/// Notifies, when an item is added to the target.
	/// </summary>
    /// <param name="sender">item that is added</param>
	/// <param name="e">The index of the item</param>
    public delegate void ItemAddedEventHandler(object sender, GXItemChangedEventArgs e);	

	/// <summary>
	/// Notifies, when an item has changed.
	/// </summary>
    /// <param name="sender">item that is updated.</param>
	/// <param name="e">Event arguments</param>
	public delegate void ItemUpdatedEventHandler(object sender, GXItemEventArgs e);

	/// <summary>
	/// Notifies, when an item is removed from the device.
	/// </summary>
    /// <param name="sender">item that is removed.</param>
	/// <param name="e">Event arguments</param>
	public delegate void ItemRemovedEventHandler(object sender, GXItemChangedEventArgs e);	
	
	/// <summary>
	/// Notifies, when the collection is cleared.
	/// </summary>
    /// <param name="sender">The collection that is cleared.</param>
	/// <param name="e">Event arguments</param>
	public delegate void ItemClearEventHandler(object sender, GXItemEventArgs e);

    
	/// <summary>
	/// Notifies read and write transactions in the DeviceGroup, device, category, table and item.
	/// </summary>
    /// <param name="sender">The item that is executed.</param>
	/// <param name="e">Event arguments</param>
	public delegate void TransactionProgressEventHandler(object sender, GXTransactionProgressEventArgs e);
#endregion
		
#region DeviceEvents
	/// <summary>
	/// Notifies, when the device settings are changed.
	/// </summary>
    /// <param name="sender">device whose settings are changed.</param>
	/// <param name="e">Event arguments</param>
	public delegate void DirtyEventHandler(object sender, GXDirtyEventArgs e);			
#endregion DeviceEvents


#region DeviceListEvents	
	
	/// <summary>
	/// OnError sends all occurred errors through here after the connection has been established.
	/// </summary>
    /// <param name="sender">The item of the event.</param>
	/// <param name="errorInfo">The description of the error.</param>
	/// <remarks>
	/// Use NotifyError to notify custom errors from GXScript.
	/// </remarks>		
	/// <param name="severity">
	/// Determines severity level. -1 is reserved for internal errors.
	/// System has no predefined error levels, so the scaling is solely user defined.
	/// </param>
	/// <seealso cref="GXDevice.OnUpdated">GXDevice.PropertyChanged</seealso>
	/// <seealso cref="GXDevice.Connect">GXDevice.Connect</seealso>
	/// <seealso cref="GXDevice.NotifyPropertyChange">GXDevice.NotifyPropertyChange</seealso>
    public delegate void Error(object sender, string errorInfo, Int32 severity);

	/// <summary>
    /// Notifies, if the selected device, device group, category, table or item has changed.
	/// </summary>
	public delegate void SelectedItemChangedEventHandler(object sender, GXSelectedItemEventArgs e);

	/// <summary>
	/// Notifies, if the Dirty item has changed with its current value.
	/// </summary>
	/// <param name="isDirty">The current value of the Dirty item.</param>
	public delegate void DirtyChangedEventHandler(bool isDirty);
	
	/// <summary>
	/// Notifies that selected device, device group, category, table or item is about to change.
	/// </summary>
	public delegate void SelectedItemChangingEventHandler(object sender, GXSelectedItemEventArgs e);
	
	/// <summary>
	/// Notifies the progress of the loading.
	/// </summary>
	/// <remarks>
	/// Set Cancel to True, to cancel loading.
	/// </remarks>  
	/// <param name="index">The index of the loaded item.</param>
	/// <param name="count">The amount of loaded items.</param>
	/// <param name="item">The item that is loading.</param>
	/// <param name="cancel">User wants to cancel the load operation.</param>
    public delegate void LoadProgressEventHandler(int index, int count, object item, out bool cancel);
		
	/// <summary>
	/// Notifies that the input language of the application has been changed.
	/// </summary>
	/// <param name="localeIdentifier"> The new input language locale identifier.</param>
    public delegate void LocaleIdentifierChangedEventHandler(System.Globalization.CultureInfo localeIdentifier);
	
	/// <summary>
	/// Notifies that loading has begun.
	/// </summary>
	/// <param name="sender">The device list that is loading.</param>
	public delegate void LoadBeginEventHandler(GXDeviceList sender);
	
	/// <summary>
	/// Notifies that loading has ended.
	/// </summary>
	/// <param name="sender">The device list that was loaded.</param>
	public delegate void LoadEndEventHandler(GXDeviceList sender);
	
#endregion DeviceListEvents

    /// <summary>
    /// OnScheduleItemStateChanged notifies, if the state of a GXSchedule has changed.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e"></param>
    public delegate void ScheduleItemStateChangedEventHandler(object sender, GXScheduleEventArgs e);
}
