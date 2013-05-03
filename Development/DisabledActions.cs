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
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Gurux.Device
{
   /// <summary>
	/// Determines which actions are disabled.  
	/// </summary>
	/// <remarks>
	/// If not using .NET Framework, enumeration is GX_DISABLED_ACTIONS.
	/// </remarks>
	/// <seealso cref="GXProperty.DisabledActions">GXProperty.DisabledActions</seealso>
    /// <seealso cref="GXCategory.DisabledActions">GXCategory.DisabledActions</seealso>
    /// <seealso cref="GXTable.DisabledActions">GXTable.DisabledActions</seealso>
    /// <seealso cref="GXDevice.DisabledActions">GXDevice.DisabledActions</seealso>
    /// <seealso cref="GXDeviceGroup.DisabledActions">GXDeviceGroup.DisabledActions</seealso>				
    [Flags]
    [DataContract()]
    public enum DisabledActions : int
	{
		/// <summary>
		/// All actions are disabled.
		/// </summary>		
        [EnumMember(Value = "-1")]
		All = 0x7FFFFFFF,
		/// <summary>
		/// No action(s) disabled.
		/// </summary>
        [EnumMember(Value = "0")]
        None = 0x0,
		/// <summary>
		/// Reading is disabled.
		/// </summary>
        [EnumMember(Value = "1")]
        Read = 0x1,
		/// <summary>
		/// Writing is disabled.
		/// </summary>
        [EnumMember(Value = "2")]
        Write = 0x2,
		/// <summary>
		/// Monitoring is disabled.
		/// </summary>
        [EnumMember(Value = "4")]        
        Monitor = 0x4,
		/// <summary>
		/// Scheduling is disabled.
		/// </summary>
        [EnumMember(Value = "8")]
        Schedule = 0x8
	};
}
