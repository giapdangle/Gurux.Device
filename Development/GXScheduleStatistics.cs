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

namespace Gurux.Device
{
	/// <summary>
	/// Contains statistics for GXSchedule.
	/// </summary>
    public class GXScheduleStatistics
    {
        ///<summary>
        ///Resets the statistics.
        ///</summary>
        public void Reset()
        {
        }

        ///<summary>
        ///How many times the schedule is run.
        ///</summary>
        public int RunCount
        {
            get;
            set;
        }

        ///<summary>
        ///How many times the schedule run has failed.
        ///</summary>
        public int RunFailCount
        {
            get;
            set;
        }

        ///<summary>
        ///LastRunTime method is reserved for inner use.
        ///</summary>
        public DateTime LastRunTime
        {
            get;
            set;
        }

        ///<summary>
        ///Time when schedule item started.
        ///</summary>        
        public DateTime StartTime
        {
            get;
            set;
        }

        ///<summary>
        ///Time when schedule item stopped.
        ///</summary>        
        public DateTime EndTime
        {
            get;
            set;
        }
    }
}
