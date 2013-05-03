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
	/// Contains statistics for GXDevice.
	/// </summary>
    public class GXDeviceStatistics
    {
		/// <summary>
		/// Reset statistics.
		/// </summary>
        public void Reset()
        {
            ConnectTime = DisconnectTime = MonitorStartTime = MonitorEndTime = DateTime.MinValue;
            WriteCount = ReadFailCount = WriteFailCount = ReadCount = 0;
            ExecutionAverage = ExecutionTime = 0;
        }

        internal void UpdateExecutionTime(TimeSpan tp)
        {
            ExecutionTime = tp.Milliseconds;
            ExecutionAverage += (ExecutionTime - ExecutionAverage) / (ReadCount + WriteCount + ReadFailCount + WriteFailCount);
        }

        /// <summary>
		/// The latest connection time of the device.
		/// </summary>
		public DateTime ConnectTime
        {
            get;
            internal set;
        }
		/// <summary>
		/// The latest disconnection time of the device.
		/// </summary>
        public DateTime DisconnectTime
        {
            get;
            internal set;
        }		
		/// <summary>
		/// The latest monitoring start time of the device.
		/// </summary>
        public DateTime MonitorStartTime
        {
            get;
            internal set;
        }		 
		/// <summary>
		/// The latest monitoring end time of the device.
		/// </summary>
        public DateTime MonitorEndTime
        {
            get;
            internal set;
        }

        /// <summary>
        /// The time when a packet was last sent.
        /// </summary>
        public DateTime PacketSendTime
        {
            get;
            internal set;
        }

		/// <summary>
		/// Properties read count.
		/// </summary>
		public int ReadCount
        {
            get;
            internal set;
        }        
		/// <summary>
		/// Properties write count.
		/// </summary>
        public int WriteCount
        {
            get;
            internal set;
        }
        /// <summary>
		/// Properties failed read count.
		/// </summary>
        public int ReadFailCount
        {
            get;
            internal set;
        }
		/// <summary>
		/// Properties failed write count.
		/// </summary>
        public int WriteFailCount
        {
            get;
            internal set;
        }
		/// <summary>
		/// Average time that value read or write takes, in ms.
		/// </summary>
        public int ExecutionAverage
        {
            get;
            internal set;
        }

        /// <summary>
        /// Time that last value read or write took, in ms.
        /// </summary>		
        public int ExecutionTime
        {
            get;
            internal set;
        }
    }
}
