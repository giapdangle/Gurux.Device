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
	/// Determines the status of a GXTable, the enumeration tells, what has happened to the GXTable.
	/// </summary>
	/// <remarks>
	/// If not using .NET Framework, enumeration is GX_TABLE_STATE.
	/// </remarks>
	/// <seealso cref="GXDevice.OnTableChanged">GXDeviceEvents.OnTableChanged</seealso>
	/// <seealso cref="GXDeviceList.OnTableChanged">GXDeviceListEvents.OnTableChanged</seealso>
	[Flags]
	public enum TableStates : int
	{
		/// <summary>
		/// No changes.
		/// </summary>
        None = 0x0,
		/// <summary>
		/// Name of the table has changed.
		/// </summary>
        Updated = 0x1,
		/// <summary>
		/// Reading of a GXTable has started.
		/// </summary>
        ReadStart = 0x2,
		/// <summary>
		/// Reading of a GXTable has ended.
		/// </summary>
        ReadEnd = 0x4,
		/// <summary>
		/// Writing of a GXTable has started.
		/// </summary>
        WriteStart = 0x8,
		/// <summary>
		/// Writing of a GXTable has ended.
		/// </summary>
		WriteEnd = 0x10,
        /// <summary>
        /// New rows added.
        /// </summary>
        RowsAdded = 0x20,
        /// <summary>
        /// Rows Removed.
        /// </summary>
        RowsRemoved = 0x40,
        /// <summary>
        /// Rows updated.
        /// </summary>
        RowsUpdated = 0x80,
        /// <summary>
        /// All rows cleared.
        /// </summary>
        RowsClear = 0x100
	};
}
