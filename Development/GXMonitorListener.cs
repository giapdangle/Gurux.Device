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
using Quartz;

namespace Gurux.Device
{
	/// <summary>
	/// Quartz job listener for monitoring devices.
	/// </summary>
    public class GXMonitorListener : IJobListener
    {

        #region IJobListener Members

		/// <summary>
		/// Return type of the IJobListener (GXMonitorListener).
		/// </summary>
        public string Name
        {
            get
            {
                return "GXMonitorListener";
            }
        }

		/// <summary>
		/// Not used.
		/// </summary>
        public void JobToBeExecuted(JobExecutionContext context)
        {
        }

		/// <summary>
		/// Not used.
		/// </summary>
		public void JobExecutionVetoed(JobExecutionContext context)
        {
        }

		/// <summary>
		/// Handles any error there was occurred.
		/// </summary>
		public void JobWasExecuted(JobExecutionContext context, JobExecutionException jobException)
        {
            GXDevice device = null;
            try
            {
                if (jobException != null)
                {
                    device = context.JobDetail.JobDataMap["Target"] as GXDevice;
                    device.NotifyError(device, jobException.GetBaseException());
                }
            }
            catch (Exception Ex)
            {
                if (device != null)
                {
                    device.NotifyError(device, Ex);
                }
            }
        }
        #endregion
    }
}
