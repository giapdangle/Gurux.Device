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
	/// Determines the status of a GXCategory, the enumeration tells, what has happened to the GXCategory.
	/// </summary>
	/// <remarks>
	/// If not using .NET Framework, enumeration is GX_CATEGORY_STATE.
	/// </remarks>
	/// <seealso href="E_Gurux_Device_GXDevice_OnCategoryChanged.htm">GXDeviceEvents.OnCategoryChanged</seealso>
	/// <seealso href="E_Gurux_Device_GXDeviceList_OnCategoryChanged.htm">GXDeviceListEvents.OnCategoryChanged</seealso>
	[Flags]
	public enum CategoryStates : int
	{
		/// <summary>
		/// No changes.
		/// </summary>
		None = 0x0,
		/// <summary>
		/// Name of the category has changed.
		/// </summary>
		Updated = 0x1,
		/// <summary>
		/// Reading of a GXCategory has started.
		/// </summary>
		ReadStart = 0x2,
		/// <summary>
		/// Reading of a GXCategory has ended.
		/// </summary>
		ReadEnd = 0x4,
		/// <summary>
		/// Writing of a GXCategory has started.
		/// </summary>
		WriteStart = 0x8,
		/// <summary>
		/// Writing of a GXCategory has ended.
		/// </summary>
		WriteEnd = 0x10
	};
}
