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
    ///<summary>
    ///Used, when ScheduleRepeat is set to Weekly).
    /// Enumeration represents day of the week.
    ///</summary>
    [Flags]
    public enum ScheduleDay
    {
        ///<summary>
        ///Day is not selected.
        ///</summary>
        None = 0,        
        ///<summary>
        ///Sunday.
        ///</summary>
        Sunday = 1,        
        ///<summary>
        ///Monday.
        ///</summary>
        Monday = 2,        
        ///<summary>
        ///Tuesday.
        ///</summary>
        Tuesday = 4,
        ///<summary>
        ///Wednesday.
        ///</summary>
        Wednesday = 8,        
        ///<summary>
        ///Thursday.
        ///</summary>
        Thursday = 16,        
        ///<summary>
        ///Friday.
        ///</summary>
        Friday = 32,
        ///<summary>
        ///Saturday.
        ///</summary>
        Saturday = 64,
        /// <summary>
        /// Every day.
        /// </summary>
        All = 0x7F
    }
}
