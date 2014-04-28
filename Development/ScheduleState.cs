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
    ///<summary>
    ///Determines the status of a GXSchedule.
    ///</summary>
    [Flags]
    [DataContract()]
    public enum ScheduleState
    {
        ///<summary>
        ///Schedule is not running.
        ///</summary>
        [EnumMember(Value = "0")]
        None = 0x0,
        ///<summary>
        ///Schedule has started.
        ///</summary>
        [EnumMember(Value = "1")]
        Start = 0x1,
        ///<summary>
        ///Schedule is running.
        ///</summary>
        [EnumMember(Value = "2")]
        Run = 0x2,
        ///<summary>
        ///Schedule has ended.
        ///</summary>
        [EnumMember(Value = "4")]
        End = 0x4,
        ///<summary>
        ///Scheduled task execution has started.
        ///</summary>
        [EnumMember(Value = "8")]
        TaskStart = 0x8,
        ///<summary>
        ///Scheduled task execution is running.
        ///</summary>
        [EnumMember(Value = "16")]
        TaskRun = 0x10,
        ///<summary>
        ///Scheduled task execution is finished.
        ///</summary>
        [EnumMember(Value = "32")]
        TaskFinish = 0x20,
        /// <summary>
        /// User has updated content of schedule item.
        /// </summary>
        [EnumMember(Value = "64")]
        Updated = 0x40
    }
}
