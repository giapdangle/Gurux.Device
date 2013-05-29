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

namespace Gurux.Device
{
    /// <summary>
	/// Determines the access mode of a GXProperty.
	/// </summary>
	/// <remarks>
	/// If not using .NET Framework, enumeration is GX_ACCESS_MODE.
	/// </remarks>
    /// <seealso cref="GXProperty.AccessMode">GXProperty.AccessMode</seealso>
	/// <example>
	/// <code>
	/// If (GXProperty1.AccessMode = GX_ACCESS_READ Or GXProperty1.AccessMode = GX_ACCESS_READWRITE) Then
	///     'Reading is possible
	/// Else
	///     'Reading is not possible
	/// End If
	/// </code>
	/// </example>
	[Flags]
    [DataContract()]
	public enum AccessMode : int
	{
		/// <summary>
		/// The GXProperty value is not accessible at all. Integer value=0.
		/// </summary>
		[EnumMember(Value="0")]
        Denied = 0,
		/// <summary>
		/// Only read access is allowed. Integer value=1.
		/// </summary>
        [EnumMember(Value="1")]
		Read = 1,
		/// <summary>
		/// Only write access is allowed. Integer value=2.
        /// </summary>
		[EnumMember(Value="2")]
        Write = 2,
		/// <summary>
		/// Both read and write accesses are allowed. Integer value=3.
		/// </summary>
		[EnumMember(Value="3")]
        ReadWrite = 3,
		/// <summary>
		/// The device sends notifies to this GXProperty, and otherwise it 
		/// is not accessible. Integer value=4.
		/// </summary>
        [EnumMember(Value="4")]
		Notify = 4
	};
}
