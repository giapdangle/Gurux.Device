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
using System.Threading;
using Gurux.Device.Properties;

namespace Gurux.Device
{
    class GXScheduleJob : IJob
    {		
        /// <summary>
        /// Initialize connection.
        /// </summary>        
        /// <returns>Returns connected device. Null if connection alreay exists.</returns>
        GXDevice InitializeConnection(GXSchedule schedule, GXDevice device)
        {
            lock (device.SyncRoot)
            {
                bool connected = (device.Status & DeviceStates.Connected) != 0;
                if (!connected)
                {
                    int pos = -1;
                    do
                    {
                        try
                        {
                            device.Connect();
                            //Increase count in case connection failed.
                            if ((device.Status & DeviceStates.Connected) == 0)
                            {
                                ++pos;
                                Thread.Sleep(schedule.ConnectionFailWaitTime);
                            }
                        }
                        catch (Exception Ex)
                        {
                            device.NotifyError(schedule, Ex);
                            ++pos;
                            Thread.Sleep(schedule.ConnectionFailWaitTime);
                        }
                    }
                    while ((device.Status & DeviceStates.Connected) == 0 && pos < schedule.ConnectionFailTryCount);
                    if ((device.Status & DeviceStates.Connected) == 0)
                    {
                        throw new Exception(Resources.ConnectionFailed);
                    }
                    return device;
                }
                return null;
            }
        }

        static void SendDataThread(object data)
        {
            object[] tmp = (object[]) data;
            GXScheduleJob job = (GXScheduleJob)tmp[0];
            GXSchedule schedule = (GXSchedule)tmp[1];
            GXDevice device = (GXDevice)tmp[2];
            bool read = (bool)tmp[3];
            GXDevice connectDevice = null;
            try
            {
                connectDevice = job.InitializeConnection(schedule, device);
                if ((device.Status & (DeviceStates.Reading | DeviceStates.Writing)) == 0)
                {
                    device.Status |= DeviceStates.Scheduling;
                    if (read)
                    {
                        device.Read();
                    }
                    else
                    {
                        device.Write();
                    }
                }                
                else
                {
                    Exception ex = new Exception(Resources.TransactionIsAlreadyInProgress);
                    device.NotifyError(device, ex);
                }
            }
            catch (Exception Ex)
            {
                device.NotifyError(device, Ex);
            }
            finally
            {
                if (connectDevice != null)
                {
                    try
                    {
                        //Clear flag.
                        connectDevice.Status &= ~DeviceStates.Scheduling;
                        connectDevice.Disconnect();
                    }
                    catch (Exception ex)
                    {
                        device.NotifyError(device, ex);
                    }
                }
            }

        }
        void DeviceTransaction(GXSchedule schedule, GXDevice device, bool read)
        {
            try
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(SendDataThread), new object[] { this, schedule, device, read });
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        /// <summary> 
        /// Called by the <see cref="IScheduler" /> when a
        /// <see cref="Trigger" /> fires that is associated with
        /// the <see cref="IJob" />.
        /// </summary>
        public virtual void Execute(IJobExecutionContext context)
        {
            GXSchedule schedule = null;
            try
            {
                schedule = context.JobDetail.JobDataMap["Target"] as GXSchedule;
				if (schedule != null && (schedule.Status & ScheduleState.TaskRun) == 0)
				{
					try
					{
						Gurux.Common.GXCommon.TraceWriteLine(DateTime.Now.ToShortTimeString() + Resources.ScheduleStarted);
						//Update status 
						schedule.Status |= ScheduleState.TaskRun;
						schedule.Statistics.StartTime = DateTime.Now;
						schedule.NotifyChange(ScheduleState.TaskStart);
						GXDeviceCollection devices = null;
						foreach (object it in schedule.Items)
						{
							if (schedule.ExcludedItems.Contains(it))
							{
								continue;
							}
                            //If user has cancel schedule.
                            if ((schedule.Status & ScheduleState.Run) == 0)
                            {
                                break;
                            }
							if (it is GXDeviceList)
							{
								devices = ((GXDeviceList)it).DeviceGroups.GetDevicesRecursive();
								foreach (GXDevice dev in devices)
								{
                                    if ((dev.DisabledActions & DisabledActions.Schedule) == 0)
                                    {
                                        DeviceTransaction(schedule, dev, schedule.Action == ScheduleAction.Read);
                                    }
								}
							}
							else if (it is GXDeviceGroup)
							{
								devices = ((GXDeviceGroup)it).GetDevicesRecursive();
								foreach (GXDevice dev in devices)
								{
                                    if ((dev.DisabledActions & DisabledActions.Schedule) == 0)
                                    {
                                        DeviceTransaction(schedule, dev, schedule.Action == ScheduleAction.Read);
                                    }
								}
							}
							else if (it is GXDevice)
							{
                                GXDevice device = it as GXDevice;
                                if ((device.DisabledActions & DisabledActions.Schedule) == 0)
                                {
                                    DeviceTransaction(schedule, device, schedule.Action == ScheduleAction.Read);
                                }
							}
							else if (it is GXCategory)
							{
								GXDevice connectDevice = InitializeConnection(schedule, ((GXCategory)it).Device);
								try
								{
                                    GXCategory cat = it as GXCategory;
                                    if ((cat.DisabledActions & DisabledActions.Schedule) == 0)
                                    {
                                        if (schedule.Action == ScheduleAction.Read)
                                        {
                                            cat.Read();
                                        }
                                        else
                                        {
                                            cat.Write();
                                        }
                                    }
								}
								finally
								{
									if (connectDevice != null)
									{
										connectDevice.Disconnect();
									}
								}
							}
							else if (it is GXTable)
							{
								GXDevice connectDevice = InitializeConnection(schedule, ((GXTable)it).Device);
                                try
                                {
                                    GXTable table = it as GXTable;
                                    if ((table.DisabledActions & DisabledActions.Schedule) == 0)
                                    {
                                        if (schedule.Action == ScheduleAction.Read)
                                        {
                                            ((GXTable)it).Read();
                                        }
                                        else
                                        {
                                            ((GXTable)it).Write();
                                        }
                                    }
                                }
                                finally
                                {
                                    if (connectDevice != null)
                                    {
                                        connectDevice.Disconnect();
                                    }
                                }
							}
							else if (it is GXProperty)
							{
								GXDevice connectDevice = InitializeConnection(schedule, ((GXProperty)it).Device);
								try
								{
                                    GXProperty prop = it as GXProperty;
                                    if ((prop.DisabledActions & DisabledActions.Schedule) == 0)
                                    {
                                        if (schedule.Action == ScheduleAction.Read)
                                        {
                                            prop.Read();
                                        }
                                        else
                                        {
                                            prop.Write();
                                        }
                                    }
								}
								finally
								{
									if (connectDevice != null)
									{
										connectDevice.Disconnect();
									}
								}
							}
						}
					}
					finally
					{
						if (schedule != null)
						{
							if ((schedule.Status & ScheduleState.TaskRun) != 0)
							{
								Gurux.Common.GXCommon.TraceWriteLine(DateTime.Now.ToShortTimeString() + Resources.ScheduleEnded);
								schedule.Status &= ~ScheduleState.TaskRun;
								schedule.NotifyChange(ScheduleState.TaskFinish);
							}
						}
					}
				}
            }
            catch (Exception Ex)
            {
                System.Diagnostics.Debug.WriteLine(Ex.Message);
//                JobExecutionException ex = new JobExecutionException(Ex);
//                throw ex;
            }
        }
    }
}
