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
	/// Determines the statistics of a GXCategory. The enumeration tells, what statistic value is in question.
	/// </summary>
	/// <remarks>
	/// If not using .NET Framework, enumeration is GX_CATEGORY_STATISTIC. 
	/// </remarks>
	/// <seealso cref="GXCategory.GetStatistic">GetStatistic</seealso>
	/// <seealso cref="GXCategory.ResetStatistic">ResetStatistic</seealso>
	[Serializable]
    public class GXCategoryStatistics
	{
		/// <summary>
		/// Reset statistics.
		/// </summary>
        public void Reset()
        {
            ReadCount = WriteCount = ReadFailCount = WriteFailCount = 0;
            ExecutionTime = ExecutionAverage = 0;
        }

        internal void UpdateExecutionTime(TimeSpan tp)
        {
            ExecutionTime = tp.Milliseconds;
            ExecutionAverage += (ExecutionTime - ExecutionAverage) / (ReadCount + WriteCount + ReadFailCount + WriteFailCount);
        }

		/// <summary>
		/// Amount of executed read commands.
		/// </summary>
		public int ReadCount
        {
            get;
            internal set;
        }
		/// <summary>
		/// Amount of executed write commands.
		/// </summary>
        public int WriteCount
        {
            get;
            internal set;
        }
		/// <summary>
		/// Amount of failed read commands.
		/// </summary>
        public int ReadFailCount
        {
            get;
            internal set;
        }
		/// <summary>
		/// Amount of failed write commands.
		/// </summary>
        public int WriteFailCount
        {
            get;
            internal set;
        }
		/// <summary>
		/// The time that the latest value read or write took, in ms.
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
	};
}
