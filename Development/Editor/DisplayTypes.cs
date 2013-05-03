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

namespace Gurux.Device.Editor
{
   /// <summary>
	/// Determines how numeric values are presented.
	/// </summary>
	/// <remarks>
	/// If not using .NET Framework, enumeration is GX_DISPLAY_TYPE.
	/// </remarks>
	/// <seealso href="P_Gurux_Device_GXProperty_DisplayType.htm">GXProperty.DisplayType</seealso>
	/// <seealso href="P_Gurux_Device_GXProperty_AvailableDisplayTypes.htm">GXProperty.AvailableDisplayTypes</seealso>
	/// <seealso href="P_Gurux_Device_GXDeviceList_DisplayType.htm">GXDeviceList.DisplayType</seealso>
	/// <seealso href="E_Gurux_Device_GXProperty_OnDisplayTypeChanged.htm">GXPropertyEvents.OnDisplayTypeChanged</seealso>
	/// <seealso href="E_Gurux_Device_GXPropertyCollection_OnDisplayTypeChanged.htm">GXPropertyCollectionEvents.OnDisplayTypeChanged</seealso>
	/// <seealso href="E_Gurux_Device_GXDevice_OnDisplayTypeChanged.htm">GXDeviceEvents.OnDisplayTypeChanged</seealso>
	/// <seealso href="E_Gurux_Device_GXDeviceList_OnDisplayTypeChanged.htm">GXDeviceListEvents.OnDisplayTypeChanged</seealso>
	[Flags]
	public enum DisplayTypes : int
	{
		/// <summary>
		/// Values are shown as decimal numbers.<br/>
		/// </summary>
        [EnumMember(Value = "0")]
        None = 0x00,
		/// <summary>
		/// Values are shown as Little Endian, Intel byte order, hexadecimal numbers.<br/>
		/// </summary>
        [EnumMember(Value = "1")]
		LittleEndianHex = 0x01,
		/// <summary>
		/// Values are shown as Big Endian, Motorola byte order, hexadecimal numbers.<br/>
		/// </summary>
        [EnumMember(Value = "2")]
		BigEndianHex = 0x02,
	};
}
