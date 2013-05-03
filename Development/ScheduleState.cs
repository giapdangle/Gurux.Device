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
    ///Determines the status of a GXSchedule.
    ///</summary>
    [Flags]
    public enum ScheduleState
    {
        ///<summary>
        ///Schedule is not running.
        ///</summary>
        None = 0x0,
        ///<summary>
        ///Schedule has started.
        ///</summary>
        Start = 0x1,
        ///<summary>
        ///Schedule is running.
        ///</summary>
        Run = 0x2,
        ///<summary>
        ///Schedule has ended.
        ///</summary>
        End = 0x4,
        ///<summary>
        ///Scheduled task execution has started.
        ///</summary>
        TaskStart = 0x8,
        ///<summary>
        ///Scheduled task execution is running.
        ///</summary>
        TaskRun = 0x10,
        ///<summary>
        ///Scheduled task execution is finished.
        ///</summary>
        TaskFinish = 0x10,
        /// <summary>
        /// User has updated content of schedule item.
        /// </summary>
        Updated = 0x20
    }
}
