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
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gurux.Device.Editor
{
    /// <summary>
    /// Enum defining different modes of partial reading.
    /// </summary>
    [DataContract()]
    public enum PartialReadType
    {
        /// <summary>
        /// Read new values. This is a default.
        /// </summary>
        [EnumMember(Value = "0")]
        New = 0,

		/// <summary>
		/// Read all values.
		/// </summary>
		[EnumMember(Value = "1")]
        All = 1,

		/// <summary>
		/// Read values between start index and count.
		/// </summary>
		[EnumMember(Value = "2")]
        Entry = 2,

		/// <summary>
		/// Read values between date times.
		/// </summary>
		[EnumMember(Value = "3")]
        Range = 3,

        /// <summary>
        /// Read last n days.
        /// </summary>
        [EnumMember(Value = "4")]
        Last = 4,
    }
}
