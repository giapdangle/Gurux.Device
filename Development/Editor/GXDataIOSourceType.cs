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

namespace Gurux.Device.Editor
{
    /// <summary>
    /// Enumerates type of the data source. 
    /// </summary>
    /// <remarks>
    /// //Do not alter the numeric values!
    /// </remarks>
    public enum GXDataIOSourceType : int
    {
        /// <summary>
        /// The type of the data source can be a parameter, a property, a category, 
        /// a table, a device, a device group, or a device list. 
        /// </summary>
        All = -1,
        /// <summary>
        /// The type of the data source is not set. 
        /// </summary>
        None = 1,
        /// <summary>
        /// The type of the data source is property. 
        /// </summary>
        Property = 0x2,
        /// <summary>
        /// The type of the data source is category. 
        /// </summary>
        Category = 0x4,
        /// <summary>
        /// The type of the data source is table. 
        /// </summary>
        Table = 0x8,
        /// <summary>
        /// The type of the data source is device. 
        /// </summary>
        Device = 0x10,
        /// <summary>
        /// The type of the data source is device group. 
        /// </summary>
        DeviceGroup = 0x20,
        /// <summary>
        /// The type of the data source is device list. 
        /// </summary>
        DeviceList = 0x40,
        /// <summary>
        /// The type of the data source is parameter. 
        /// </summary>
        Parameter = 0x80
    }
}
