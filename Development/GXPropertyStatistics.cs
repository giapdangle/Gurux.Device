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
	/// Contains statistics for GXProperty.
	/// </summary>
	/// <seealso href="M_Gurux_Device_GXProperty_GetStatistic.htm">GetStatistic</seealso>
	/// <seealso href="M_Gurux_Device_GXProperty_ResetStatistic.htm">ResetStatistic</seealso>
	public class GXPropertyStatistics
	{
		/// <summary>
		/// Reset statistics.
		/// </summary>
        public void Reset()
        {
            ReadCount = WriteCount = ReadFailCount = WriteFailCount = 0;
            ExecutionTime = ExecutionAverage = 0;
            Minimun = Maximum = null;
        }

        internal void UpdateExecutionTime(TimeSpan tp)
        {
            ExecutionTime = tp.Milliseconds;
            ExecutionAverage += (ExecutionTime - ExecutionAverage) / (ReadCount + WriteCount + ReadFailCount + WriteFailCount);
        }

		/// <summary>
		/// Amount of executed reads.
		/// </summary>
        public int ReadCount
        {
            get;
            internal set;
        }
		/// <summary>
		/// Amount of executed writes.
		/// </summary>
        public int WriteCount
        {
            get;
            internal set;
        }

		/// <summary>
		/// Amount of failed reads.
		/// </summary>
        public int ReadFailCount
        {
            get;
            internal set;
        }
		/// <summary>
		/// Amount of failed writes.
		/// </summary>
        public int WriteFailCount
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
        
        /// <summary>
		/// Average time that value read or write takes, in ms.
		/// </summary>
        public int ExecutionAverage
        {
            get;
            internal set;
        }
        /// <summary>
        /// Minimum value.
        /// </summary>        
        public object Minimun
        {
            get;
            internal set;
        }
        /// <summary>
        /// Maximum value.
        /// </summary>        
        public object Maximum
        {
            get;
            internal set;
        }
        /// <summary>
        /// Average value.
        /// </summary>        
        public object Average
        {
            get;
            internal set;
        }
        
	};
}
