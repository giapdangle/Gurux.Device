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

namespace Gurux.Device
{
    /// <summary>
	/// Determines the status of a GXProperty, the enumeration tells, what has happened to the GXProperty.
	/// </summary>
	/// <remarks>
	/// If not using .NET Framework, enumeration is GX_PROPERTY_STATE.
	/// </remarks>
	/// <seealso href="P_Gurux_Device_GXProperty_Status.htm">GXProperty.Status</seealso>
	/// <seealso href="M_Gurux_Device_GXProperty_NotifyPropertyChange.htm">GXProperty.NotifyPropertyChange</seealso>
	/// <seealso href="M_Gurux_Device_GXProperty_SetValue.htm">GXProperty.SetValue</seealso>
	/// <seealso href="M_Gurux_Device_GXPropertyCollection_NotifyPropertyChange.htm">GXPropertyCollection.NotifyPropertyChange</seealso>
	/// <seealso href="M_Gurux_Device_GXDevice_NotifyPropertyChange.htm">GXDevice.NotifyPropertyChange</seealso>
	/// <seealso href="M_Gurux_Device_GXRow_SetValue.htm">GXRow.SetValue</seealso>
	/// <seealso href="E_Gurux_Device_GXProperty_OnUpdated.htm">GXPropertyEvents.PropertyChanged</seealso>
	/// <seealso href="E_Gurux_Device_GXPropertyCollection_OnUpdated.htm">GXPropertyCollectionEvents.PropertyChanged</seealso>
	/// <seealso href="E_Gurux_Device_GXDevice_OnUpdated.htm">GXDeviceEvents.PropertyChanged</seealso>
	/// <seealso href="E_Gurux_Device_GXDeviceList_OnUpdated.htm">GXDeviceListEvents.PropertyChanged</seealso>
	[Flags]
	public enum PropertyStates : int
	{
		/// <summary>
		/// This should never happen naturally. Reserved for internal use.
		/// </summary>
        All = -1,
		/// <summary>
		/// The initial state of a GXProperty. GXProperty is set to its initial state, when the GXDevice is reset. 
		/// </summary>
		None = 0,
		/// <summary>
		/// The latest read or write transaction was successful.
		/// </summary>
		Ok = 1,
		/// <summary>
		/// A value of a GXProperty has changed, but the new value has not been written 
		/// to the device. This occurs, when the user manually changes the value of a GXProperty
		/// </summary>
		ValueChangedByUser = 2,
		/// <summary>
		/// SetError method has been used.
		/// </summary>
		ErrorChanged = 4,
		/// <summary>
		/// Failed to read the value from the device.
		/// </summary>
		ReadFailed = 8,
		/// <summary>
		/// Failed to write the value to the device.
		/// </summary>
		WriteFailed = 0x10,
		/// <summary>
		/// Unspecified error.
		/// </summary>
		Error = 0x20,
		/// <summary>
		/// A value of the physical device has changed, the changed value is read, and shown on the user interface. 
		/// </summary>
		ValueChangedByDevice = 0x40,
		/// <summary>
		/// The statistical minimum value of a GXProperty has changed.
		/// </summary>
		MinChanged = 0x80,
		/// <summary>
		/// The statistical maximum value of a GXProperty has changed.
		/// </summary>
		MaxChanged = 0x100,
		/// <summary>
		/// The statistical average value of a GXProperty has changed.
        /// </summary>
		AverageChanged = 0x200,
		/// <summary>
		/// The value of a GXProperty is reset to default value. 
		/// Note: Reset of statistics is notified with a statistic change, and not this.
		/// </summary>
		ValueReset = 0x400,
		/// <summary>
		/// Reading of a GXProperty has started.
		/// </summary>
		ReadStart = 0x800,
		/// <summary>
		/// Writing of a GXProperty has started.
		/// </summary>
		WriteStart = 0x1000,
		/// <summary>
		/// The number of rows in a table has been altered.
		/// </summary>
		RowCountChanged = 0x2000,
		/// <summary>
		/// The display type property of device has changed.
		/// Display type informs how to present numeric values.
		/// </summary>
		DisplayTypeChanged = 0x4000,
		/// <summary>
		/// Value of the property has changed from the earlier value.
		/// </summary>
		ValueChanged = 0x8000,
		/// <summary>
		/// Notifies, when property read has ended.
		/// </summary>
		ReadEnd = 0x10000, 
		/// <summary>
		/// Notifies, when property write has ended.
		/// </summary>
		WriteEnd = 0x20000,
		/// <summary>
		/// Property read in progress.
		/// </summary>
		Reading = 0x40000,
		/// <summary>
		/// Property write in progress.
		/// </summary>
		Writing = 0x80000,
		/// <summary>
		/// Content of the property (Name or Type) has changed.
		/// </summary>
        Updated = 0x100000 
	};
}
