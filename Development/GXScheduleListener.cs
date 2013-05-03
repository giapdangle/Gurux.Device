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
using System.IO;

namespace Gurux.Device
{
	/// <summary>
	/// Quartz job listener for running schedules.
	/// </summary>
    public class GXScheduleListener : IJobListener
    {

        #region IJobListener Members

		/// <summary>
		/// Returns the type of the IJobListener. (GXScheduleListener)
		/// </summary>
        public string Name
        {
            get
            {
                return typeof(GXScheduleListener).Name;
            }
        }

		/// <summary>
		/// Not in use.
		/// </summary>
        public void JobToBeExecuted(JobExecutionContext context)
        {
        }

		/// <summary>
		/// Not in use.
		/// </summary>
        public void JobExecutionVetoed(JobExecutionContext context)
        {
        }

		/// <summary>
		/// Handles operations related to finished schedule fire such as statistics and next time of firing.
		/// </summary>
        public void JobWasExecuted(JobExecutionContext context, JobExecutionException jobException)
        {
            GXSchedule schedule = null;
            try
            {
                schedule = context.JobDetail.JobDataMap["Target"] as GXSchedule;
                schedule.Statistics.LastRunTime = DateTime.Now;
                if (jobException == null)
                {
                    ++schedule.Statistics.RunCount;
                }
                else
                {
                    ++schedule.Statistics.RunFailCount;
                    if (schedule != null)
                    {
                        schedule.Parent.Parent.NotifyError(schedule, jobException.GetBaseException());
                    }
                }
                //If this is last execution time.
                if (context.NextFireTimeUtc == null)
                {
                    if (schedule != null)
                    {
                        schedule.Statistics.EndTime = DateTime.Now;
                        schedule.Status &= ~ScheduleState.Run;
                        schedule.NotifyChange(ScheduleState.End);
                        context.Scheduler.DeleteJob(schedule.Name + schedule.ID.ToString(), schedule.Name + schedule.ID.ToString());
                    }
                }
            }
            catch (Exception Ex)
            {
                if (schedule != null)
                {
                    schedule.Parent.Parent.NotifyError(schedule, Ex);
                }
            }
        }

        #endregion
    }
}
