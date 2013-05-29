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
	/// Determines what is reset.
	/// </summary>
	/// <remarks>
	/// If not using .NET Framework, enumeration is GX_RESET_TYPE.
	/// </remarks>
	/// <seealso cref="GXDevice.Reset">GXDevice.Reset</seealso>
	/// <seealso cref="GXDeviceGroup.ResetDevices">GXDeviceGroup.ResetDevices</seealso>
	/// <seealso cref="GXDeviceList.ResetDevices">GXDeviceList.ResetDevices</seealso>
	[Flags]
    public enum ResetTypes : int
	{
		/// <summary>
		/// Values are set to default.
		/// </summary>
		Values = 1,
		/// <summary>
		/// Transaction is reset.
		/// </summary>
		Transaction = 2,
		/// <summary>
		/// Error status is reset.
		/// </summary>
        Errors = 4,
	};
}
