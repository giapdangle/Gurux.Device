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
using System.Threading;
using Quartz;

namespace Gurux.Device
{
    class GXMonitorJob : IJob
    {        
        public virtual void Execute(JobExecutionContext context)
        {
            GXDevice device = null;
            try
            {
                device = context.JobDetail.JobDataMap["Target"] as GXDevice;
                //Do work if device is connected and there is no read ongoing.
                if (device != null &&
                    (device.Status & (DeviceStates.Reading | DeviceStates.Writing)) == 0 &&
                    (device.Status & DeviceStates.Connected) != 0)
                {
                    device.Read();
                }
                else //Is reading is already in progres.
                {
                    device = null;
                }
            }
            catch (Exception Ex)
            {
                JobExecutionException ex = new JobExecutionException(Ex);
                throw ex;
            }
            finally
            {
                if (device != null)
                {

                }
            }
        }
    }
}
