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
using System.ComponentModel;
using System.Runtime.Serialization;
using Quartz;
using System.Collections;
using System.Globalization;
using Gurux.Device.Properties;

namespace Gurux.Device
{
	/// <summary>
	/// A scheduling component used for repeating tasks for GXDevice and related components.
	/// </summary>
    [DataContract(), Serializable]    
    public class GXSchedule : Gurux.Device.Editor.GXSite
    {
        #region Parameters
        List<Object> m_Items, m_ExcludedItems;
        System.DayOfWeek[] m_DayOfWeeks;
        DateTime m_ScheduleEndTime, m_ScheduleStartTime, m_TransactionStartTime, m_TransactionEndTime;
        [DataMember(Name = "UpdateInterval", IsRequired = false, EmitDefaultValue = false)]
        int m_UpdateInterval = 0;
        [DataMember(Name = "Count", IsRequired = false, EmitDefaultValue = false)]
        int m_TransactionCount = 0;
        [DataMember(Name = "RepeatMode", IsRequired = false, EmitDefaultValue = false)]
        ScheduleRepeat m_RepeatMode;
        [DataMember(Name = "Name", IsRequired = false, EmitDefaultValue = false)]
        string m_Name;
        [DataMember(Name = "MaxThreadCount", IsRequired = false, EmitDefaultValue = false)]
        int m_MaxThreadCount;
        [DataMember(Name = "Interval", IsRequired = false, EmitDefaultValue = false)]
        int m_Interval = 0;
        [DataMember(Name = "FailWaitTime", IsRequired = false, EmitDefaultValue = false)]
        int m_FailWaitTime = 0;
        [DataMember(Name = "FailTryCount", IsRequired = false, EmitDefaultValue = false)]
        int m_FailTryCount = 0;
        [DataMember(Name = "DayOfMonth", IsRequired = false, EmitDefaultValue = false)]
        int m_DayOfMonth = 1;
        [DataMember(Name = "ConnectionWaitTime", IsRequired = false, EmitDefaultValue = false)]
        int m_ConnectionWaitTime = 0;
        [DataMember(Name = "ConnectionFailWaitTime", IsRequired = false, EmitDefaultValue = false)]
        int m_ConnectionFailWaitTime = 0;
        [DataMember(Name = "ConnectionFailTryCount", IsRequired = false, EmitDefaultValue = false)]
        int m_ConnectionFailTryCount = 0;
        [DataMember(Name = "ConnectionDelayTime", IsRequired = false, EmitDefaultValue = false)]
        int m_ConnectionDelayTime = 0;
        [DataMember(Name = "Action", IsRequired = false, EmitDefaultValue = false)]
        ScheduleAction m_Action = ScheduleAction.Read;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public GXSchedule()
        {
            m_Interval = m_TransactionCount = 1;
            m_Items = new List<object>();
            m_ExcludedItems = new List<object>();
            m_Action = ScheduleAction.Read;
            m_TransactionStartTime = m_ScheduleStartTime = DateTime.MinValue;
            m_TransactionEndTime = m_ScheduleEndTime = DateTime.MaxValue;
            m_DayOfWeeks = new System.DayOfWeek[0];
            Statistics = new GXScheduleStatistics();
        }

        #region Serializing

        /// <summary>
        /// Override this to made changes before schedule load.
        /// </summary>
        protected override void OnDeserializing(bool designMode)
        {
            m_Interval = m_TransactionCount = 1;
            m_Action = ScheduleAction.Read;
            m_Items = new List<object>();
            m_ExcludedItems = new List<object>();
            m_TransactionStartTime = m_ScheduleStartTime = DateTime.MinValue;
            m_TransactionEndTime = m_ScheduleEndTime = DateTime.MaxValue;
            m_DayOfWeeks = new System.DayOfWeek[0];
            Statistics = new GXScheduleStatistics();
        }

		/// <summary>
		/// Clears SerializedItems
		/// </summary>
        protected override void OnSerialized(bool designMode)
        {
            SerializedExcludedItems = SerializedItems = "";
        }

		/// <summary>
		/// Serializes included and excluded items.
		/// </summary>
        protected override void OnSerializing(bool designMode)
        {
            if (m_RepeatMode != ScheduleRepeat.Month)
            {
                DayOfMonth = 0;
            }
            SerializedItems = "";
            foreach (object it in Items)
            {
                if (it is GXDeviceList)
                {
                    SerializedItems += "0;";
                }
                else if (it is GXDeviceGroup)
                {
                    SerializedItems += ((GXDeviceGroup)it).ID + ";";
                }
                else if (it is GXDevice)
                {
                    SerializedItems += ((GXDevice)it).ID + ";";
                }
                else if (it is GXCategory)
                {
                    SerializedItems += ((GXCategory)it).ID + ";";
                }
                else if (it is GXTable)
                {
                    SerializedItems += ((GXTable)it).ID + ";";
                }
                else if (it is GXProperty)
                {
                    SerializedItems += ((GXProperty)it).ID + ";";
                }
            }

            SerializedExcludedItems = null;
            foreach (object it in ExcludedItems)
            {
                if (it is GXDeviceList)
                {
                    SerializedExcludedItems += "0;";
                }
                else if (it is GXDeviceGroup)
                {
                    SerializedExcludedItems += ((GXDeviceGroup)it).ID + ";";
                }
                else if (it is GXDevice)
                {
                    SerializedExcludedItems += ((GXDevice)it).ID + ";";
                }
                else if (it is GXCategory)
                {
                    SerializedExcludedItems += ((GXCategory)it).ID + ";";
                }
                else if (it is GXTable)
                {
                    SerializedExcludedItems += ((GXTable)it).ID + ";";
                }
                else if (it is GXProperty)
                {
                    SerializedExcludedItems += ((GXProperty)it).ID + ";";
                }
            }
        }

        [DataMember(Name = "Schedule", IsRequired = false, EmitDefaultValue = false)]
        internal string SerializedSchedule
        {
            get
            {
                if (ScheduleStartTime == DateTime.MinValue && ScheduleEndTime == DateTime.MaxValue)
                {
                    return null;
                }
                string str = null;
                if (ScheduleStartTime != DateTime.MinValue)
                {
                    str = ScheduleStartTime.ToUniversalTime().ToString("d", DateTimeFormatInfo.InvariantInfo);
                }
                str += ";";
                if (ScheduleEndTime != DateTime.MaxValue)
                {
                    str += ScheduleEndTime.ToUniversalTime().ToString("d", DateTimeFormatInfo.InvariantInfo);
                }
                return str;
            }
            set
            {
                string[] data = value.Split(new char[] { ';' });
                if (!string.IsNullOrEmpty(data[0]))
                {
                    m_ScheduleStartTime = DateTime.SpecifyKind(DateTime.ParseExact(data[0], "d", DateTimeFormatInfo.InvariantInfo), DateTimeKind.Utc).ToLocalTime();
                }
                if (!string.IsNullOrEmpty(data[1]))
                {
                    m_ScheduleEndTime = DateTime.SpecifyKind(DateTime.ParseExact(data[1], "d", DateTimeFormatInfo.InvariantInfo), DateTimeKind.Utc).ToLocalTime();
                }
            }
        }

        [DataMember(Name = "Transaction", IsRequired = false, EmitDefaultValue = false)]
        internal string SerializedTransaction
        {
            get
            {
                if (TransactionStartTime == DateTime.MinValue && TransactionEndTime == DateTime.MaxValue)
                {
                    return null;
                }
                string str = null;
                if (TransactionStartTime != DateTime.MinValue)
                {
                    str = TransactionStartTime.ToUniversalTime().ToString("T", DateTimeFormatInfo.InvariantInfo);
                }
                str += ";";
                if (TransactionEndTime != DateTime.MaxValue)
                {
                    str += TransactionEndTime.ToUniversalTime().ToString("T", DateTimeFormatInfo.InvariantInfo);
                }
                return str;
            }
            set
            {
				if (value == null)
				{
					m_TransactionStartTime = DateTime.MinValue;
					m_TransactionEndTime = DateTime.MaxValue;
				}
				else
				{
	                string[] data = value.Split(new char[] { ';' });
	                if (!string.IsNullOrEmpty(data[0]))
	                {
	                    m_TransactionStartTime = DateTime.SpecifyKind(DateTime.ParseExact(data[0], "T", DateTimeFormatInfo.InvariantInfo), DateTimeKind.Utc).ToLocalTime();
	                }
	                if (!string.IsNullOrEmpty(data[1]))
	                {
	                    m_TransactionEndTime = DateTime.SpecifyKind(DateTime.ParseExact(data[1], "T", DateTimeFormatInfo.InvariantInfo), DateTimeKind.Utc).ToLocalTime();
	                }
				}
            }
        }           
        #endregion

        #region Parameters

        /// <summary>
        /// Object Identifier.
        /// </summary>        
        [DataMember(Name = "ID", IsRequired = true)]
        public ulong ID
        {
            get;
            internal set;
        }

        ///<summary>
        ///Action is the transaction to be performed, when schedule triggers.
        ///</summary>
        [DefaultValue(ScheduleAction.Read)]
        public ScheduleAction Action 
        {
            get
            {
                return m_Action;
            }
            set
            {
                if (m_Action != value)
                {
                    m_Action = value;
                    NotifyUpdated();
                }
            }
        }

		/// <summary>
		/// Statistics for GXSchedule.
		/// </summary>
        public GXScheduleStatistics Statistics
        {
            get;
            internal set;
        }
                
        ///<summary>
        ///ConnectionDelayTime determines for how long a time (in ms) is waited, before next connection is made.
        ///</summary>
        [DefaultValue(0)]
        public int ConnectionDelayTime 
        {
            get
            {
                return m_ConnectionDelayTime;
            }
            set
            {
                if (m_ConnectionDelayTime != value)
                {
                    m_ConnectionDelayTime = value;
                    NotifyUpdated();
                }
            } 
        }
        
        ///<summary>
        ///How many times failed connection is tried to reconnect.
        ///</summary>
        [DefaultValue(0)]
        public int ConnectionFailTryCount
        {
            get
            {
                return m_ConnectionFailTryCount;
            }
            set
            {
                if (m_ConnectionFailTryCount != value)
                {
                    m_ConnectionFailTryCount = value;
                    NotifyUpdated();
                }
            } 
        }
        
        ///<summary>
        ///ConnectionFailWaitTime determines for how long a time (in ms) is waited,
        ///before next connection is made.
        ///</summary>
        [DefaultValue(0)]
        public int ConnectionFailWaitTime 
        {
            get
            {
                return m_ConnectionFailWaitTime;
            }
            set
            {
                if (m_ConnectionFailWaitTime != value)
                {
                    m_ConnectionFailWaitTime = value;
                    NotifyUpdated();
                }
            } 
        }
        
        ///<summary>
        ///Determines how long a time (in ms) is waited for the connection to be established.
        ///</summary>
        [DefaultValue(0)]
        public int ConnectionWaitTime 
        {
            get
            {
                return m_ConnectionWaitTime;
            }
            set
            {
                if (m_ConnectionWaitTime != value)
                {
                    m_ConnectionWaitTime = value;
                    NotifyUpdated();
                }
            } 
        }
        
        ///<summary>
        ///DayOfMonth is used when RepeatMode is monthly. Determines the day of month,
        ///when the transaction is executed.
        ///</summary>
        [DefaultValue(1)]
        public int DayOfMonth
        {
            get
            {
                return m_DayOfMonth;
            }
            set
            {
                if (m_DayOfMonth != value)
                {
                    m_DayOfMonth = value;
                    NotifyUpdated();
                }
            }  
        }        

        ///<summary>
        ///DayOfWeek is used when RepeatMode is weekly. Determines weekday(s), when
        ///transaction is executed.
        ///</summary>
        ///<remarks>
        ///Multiple days can be specified.
        ///</remarks>        
        public System.DayOfWeek[] DayOfWeeks
        {
            get
            {
                return m_DayOfWeeks;
            }
            set
            {
                if (m_DayOfWeeks == null && value == null)
                {
                    return;
                }                
                if (m_DayOfWeeks != null && value != null)
                {
                    if (m_DayOfWeeks.Length == value.Length)
                    {
                        bool changed = false;
                        for (int pos = 0; pos != m_DayOfWeeks.Length; ++pos)
                        {
                            if (m_DayOfWeeks[pos] != value[pos])
                            {
                                changed = true;
                                break;
                            }
                        }
                        if (!changed)
                        {
                            return;
                        }
                    }
                }                
                m_DayOfWeeks = value;
                NotifyUpdated();            
            }
        }
        [DataMember(Name="DayOfWeeks", IsRequired = false, EmitDefaultValue = false)]
        internal byte SerializedDayOfWeeks
        {
            get
            {
                if (DayOfWeeks == null)
                {
                    return 0;
                }
                byte value = 0;
                foreach (DayOfWeek it in DayOfWeeks)
                {
                    value |= (byte)Math.Pow(2, (int)it);
                }
                return value;
            }
            set
            {
                List<DayOfWeek> list = new List<DayOfWeek>();
                if (value != 0)
                {
                    BitArray arr = new BitArray(new int[] { value });
                    for (int pos = 0; pos != 7; ++pos)
                    {
                        if (arr[pos])
                        {
                            list.Add((DayOfWeek)pos);
                        }
                    }
                }
                m_DayOfWeeks = list.ToArray();
            }
        }

        
        ///<summary>
        ///DeviceGroups are the target GXDeviceGroupCollection of the transaction.
        ///</summary>        
        public List<Object> Items
        {
            get
            {
                return m_Items;
            }
        }

        [DataMember(Name = "Items", IsRequired = false, EmitDefaultValue = false)]
        internal string SerializedItems
        {
            get;
            set;           
        }
                        
        ///<summary>
        ///ExcludedItems are the items, which are not read, even if they are set in target collections.
        ///</summary>        
        public List<Object> ExcludedItems
        {
            get
            {
                return m_ExcludedItems;
            }
        }

        [DataMember(Name = "ExcludedItems", IsRequired = false, EmitDefaultValue = false)]
        internal string SerializedExcludedItems
        {
            get;
            set; 
        }
               
        //
        ///<summary>
        ///ExecuteFailedItemsOnly gets failed items only if the schedule fails.
        ///</summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public bool ExecuteFailedItemsOnly 
        { 
            get; 
            set;
        }
        
        ///<summary>
        ///FailedItems returns collection of items, whose read/write failed.
        ///</summary>        
        public object[] FailedItems 
        { 
            get;
            internal set;
        }
        
        ///<summary>
        ///FailTryCount determines how many times devices are tried to read again if
        ///failed.
        ///</summary>
        [DefaultValue(0)]
        public int FailTryCount 
        {
            get
            {
                return m_FailTryCount;
            }
            set
            {
                if (m_FailTryCount != value)
                {
                    m_FailTryCount = value;
                    NotifyUpdated();
                }
            }  
        }
        
        ///<summary>
        ///FailWaitTime determines for how long a time (in ms) is waited before devices
        ///are tried to read again if failed.
        ///</summary>        
        public int FailWaitTime 
        {
            get
            {
                return m_FailWaitTime;
            }
            set
            {
                if (m_FailWaitTime != value)
                {
                    m_FailWaitTime = value;
                    NotifyUpdated();
                }
            }  
        }
        
        ///<summary>
        ///Interval determines the time between schedule runs.  For example, if the
        ///repeat mode is weekly and interval is 3, the schedule is triggered every three weeks.
        ///</summary>
        ///<remarks>
        ///Default value is 1.
        ///</remarks>        
        public int Interval 
        {
            get
            {
                return m_Interval;
            }
            set
            {
                if (m_Interval != value)
                {
                    m_Interval = value;
                    NotifyUpdated();
                }
            }  
        }                        
        
        ///<summary>
        ///MaxThreadCount is the maximum number of worker threads per Schedule item.
        ///</summary>        
        public int MaxThreadCount 
        {
            get
            {
                return m_MaxThreadCount;
            }
            set
            {
                if (m_MaxThreadCount != value)
                {
                    m_MaxThreadCount = value;
                    NotifyUpdated();
                }
            }
        }

        ///<summary>
        ///Schedule name.
        ///</summary>        
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                if (m_Name != value)
                {
                    m_Name = value;
                    NotifyUpdated();
                }
            }
        }

        ///<summary>
        ///Parent is the parent GXScheduleCollection collection of the GXSchedule.
        ///</summary>
        public GXScheduleCollection Parent
        {
            get;
            set;
        }

        ///<summary>
        ///RepeatMode determines the repeat mode of a GXSchedule.
        ///</summary>        
        public ScheduleRepeat RepeatMode
        {
            get
            {
                return m_RepeatMode;
            }
            set
            {
                if (m_RepeatMode != value)
                {
                    m_RepeatMode = value;
                    NotifyUpdated();
                }
            }
        }       

        ///<summary>
        ///ScheduleEndTime is the time, when the schedule is stopped.
        ///</summary>        
        public DateTime ScheduleEndTime
        {
            get
            {
                return m_ScheduleEndTime;
            }
            set
            {
                if (m_ScheduleEndTime != value)
                {
                    m_ScheduleEndTime = value;
                    NotifyUpdated();
                }
            }
        }

        ///<summary>
        ///ScheduleStartTime is the time, when the schedule is started.
        ///</summary>        
        public DateTime ScheduleStartTime
        {
            get
            {
                return m_ScheduleStartTime;
            }
            set
            {
                if (m_ScheduleStartTime != value)
                {
                    m_ScheduleStartTime = value;
                    NotifyUpdated();
                }
            }
        }

        ///<summary>
        ///TransactionEndTime is the time, when transaction has stopped.
        ///</summary>                
        public DateTime TransactionEndTime
        {
            get
            {
                return m_TransactionEndTime;
            }
            set
            {
                if (m_TransactionEndTime != value)
                {
                    m_TransactionEndTime = value;
                    NotifyUpdated();
                }
            }
        }

        ///<summary>
        ///TransactionStartTime is the time, when transaction is started.
        ///</summary>        
        public DateTime TransactionStartTime
        {
            get
            {
                return m_TransactionStartTime;
            }
            set
            {
                if (m_TransactionStartTime != value)
                {
                    m_TransactionStartTime = value;
                    NotifyUpdated();
                }
            }
        }       

        ///<summary>
        ///Status is the current state of the schedule item.
        ///</summary>
        public ScheduleState Status
        {
            get;
            internal set;
        }

        ///<summary>
		///TransactionCount determines how many times transaction is made, before stopping the current run.
		///</summary>
        public int TransactionCount
        {
            get
            {
                return m_TransactionCount;
            }
            set
            {
                if (m_TransactionCount != value)
                {
                    m_TransactionCount = value;
                    NotifyUpdated();
                }
            }
        }

        ///<summary>
        ///UpdateInterval is the execution interval of the transaction in seconds.
        ///</summary>        
        public int UpdateInterval
        {
            get
            {
                return m_UpdateInterval;
            }
            set
            {
                if (m_UpdateInterval != value)
                {
                    m_UpdateInterval = value;
                    NotifyUpdated();
                }
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// see more: http://www.cronmaker.com/
        /// </remarks>
        /// <returns></returns>
        internal ITrigger GetTrigger()
        {
            string format = null;
            DateTime end;
            ITrigger trigger = null;
            if (this.RepeatMode == ScheduleRepeat.Once)
            {
                trigger = (ITrigger)TriggerBuilder.Create()
                    .StartNow()
                    .EndAt(DateTime.Now)
                    .Build();                
            }
            else if (this.RepeatMode == ScheduleRepeat.Second)
            {
                format = "0/" + Interval.ToString() + " * * * * ?";
            }
            else if (this.RepeatMode == ScheduleRepeat.Minute)
            {
                format = "0 0/" + Interval.ToString() + " * * * ?";
            }
            else if (this.RepeatMode == ScheduleRepeat.Hour)
            {
                format = "0 0 0/" + Interval.ToString() + " * * ?";
            }
            else if (this.RepeatMode == ScheduleRepeat.Day)
            {
                format = string.Format("{3} {2} {1} 1/{0} * ?",
                            Interval,        
                            TransactionStartTime.Hour,
                            TransactionStartTime.Minute, 
                            TransactionStartTime.Second);
            }
            else if (this.RepeatMode == ScheduleRepeat.Week)
            {
                end = DateTime.MaxValue;
                if (ScheduleEndTime != DateTime.MaxValue)
                {
                    end = new DateTime(ScheduleEndTime.Year, ScheduleEndTime.Month, ScheduleEndTime.Day, TransactionEndTime.Hour, TransactionEndTime.Minute, TransactionEndTime.Second);
                }
                List<string> days = new List<string>();
                foreach (DayOfWeek it in this.DayOfWeeks)
                {
                    days.Add(it.ToString().Substring(0, 3).ToUpper());
                }                
                string tmp = string.Format("{3} {2} {1} ? * {0}, *",                                             
                                            string.Join(",", days.ToArray()),
                                            TransactionStartTime.Hour,
                                            TransactionStartTime.Minute, 
                                            TransactionStartTime.Second);
                IScheduleBuilder scheduleBuilder = CronScheduleBuilder.CronSchedule(tmp);
                trigger = (ITrigger)TriggerBuilder.Create()
                .WithIdentity(this.Name)
                .WithSchedule(scheduleBuilder)
                .Build();                               
            }
            else if (this.RepeatMode == ScheduleRepeat.Month)
            {
                string tmp = string.Format("0 {2} {1} {0} 1/{3} ? *",
                                            this.DayOfMonth,
                                            this.TransactionStartTime.Hour,
                                            this.TransactionStartTime.Minute,
                                            Interval);

                IScheduleBuilder scheduleBuilder = CronScheduleBuilder.CronSchedule(tmp);
                trigger = (ITrigger)TriggerBuilder.Create()
                .WithIdentity(this.Name)
                .WithSchedule(scheduleBuilder)
                .Build();                
            }
            if (trigger == null)
            {
                end = DateTime.MaxValue;
                if (ScheduleEndTime != DateTime.MaxValue)
                {
                    end = new DateTime(ScheduleEndTime.Year, ScheduleEndTime.Month, ScheduleEndTime.Day, TransactionEndTime.Hour, TransactionEndTime.Minute, TransactionEndTime.Second);
                }
                IScheduleBuilder scheduleBuilder = CronScheduleBuilder.CronSchedule(format);//.InTimeZone(TimeZoneInfo.Local);
                trigger = (ITrigger)TriggerBuilder.Create()
                .WithIdentity(this.Name)
                .WithSchedule(scheduleBuilder)
                .Build();                                
            }
            return trigger;
        }

        internal DateTime[] GetNextScheduledDates(int repeatCnt)
        {
            ITrigger trigger = GetTrigger();
            List<DateTime> dates = new List<DateTime>();
            DateTimeOffset? dt = DateTime.Now.ToUniversalTime();
            DateTime end = DateTime.MaxValue;
            if (ScheduleEndTime != DateTime.MaxValue)
            {
                end = new DateTime(ScheduleEndTime.Year, ScheduleEndTime.Month, ScheduleEndTime.Day, TransactionEndTime.Hour, TransactionEndTime.Minute, TransactionEndTime.Second);
            }
            do
            {
                dt = trigger.GetFireTimeAfter(dt);
                if (!dt.HasValue || dt > end)
                {
                    break;
                }
                dates.Add(dt.Value.DateTime.ToLocalTime());                
            }
            while (dates.Count < repeatCnt);
            return dates.ToArray();
        }

        ///<summary>
        ///NextScheduledDate returns the next scheduled date.
        ///</summary>
        public DateTime NextScheduledDate 
        {
            get
            {
                DateTime[] dates = GetNextScheduledDates(1);
                if (dates.Length == 0)
                {
                    return DateTime.MinValue;
                }
                return GetNextScheduledDates(1)[0];
            }
        }       
        
        ///<summary>
        ///The ScheduledTransactions is an array of all execution dates of the schedule
        ///item.
        ///</summary>
        public DateTime[] ScheduledTransactions 
        {
            get
            {
                return GetNextScheduledDates(100);
            }
        }             
       
		/// <summary>
		/// Notifies, when an item has changed.
		/// </summary>
        public event ItemUpdatedEventHandler OnUpdated;        

        ///<summary>
        ///OnScheduleItemStateChanged notifies that the state of the GXSchedule has
        ///changed.
		///</summary>
        [Description("OnScheduleItemStateChanged notifies that the state of the GXSchedule has changed.")]
        public event ScheduleItemStateChangedEventHandler OnScheduleItemStateChanged;        

        internal void NotifyChange(ScheduleState status)
        {
            GXScheduleEventArgs e = new GXScheduleEventArgs(this, status);
            if (OnScheduleItemStateChanged != null)
            {
                OnScheduleItemStateChanged(this, e);
            }
            if (Parent != null)
            {
                Parent.NotifyChange(this, e);
            }
        }       

        internal virtual void NotifyUpdated()
        {
            GXScheduleEventArgs e = new GXScheduleEventArgs(this, ScheduleState.Updated);
            if (OnUpdated != null)
            {
                OnUpdated(this, e);
            }
            if (this.Parent != null)
            {
                this.Parent.NotifyUpdated(this, e);
            }
        }

        void Check()
        {
            if (this.Parent == null || this.Parent.Parent == null)
            {
                throw new Exception(Resources.ParentlessScheduleCanTBeActivated);
            }
			this.Parent.Parent.StartSchedules();
        }
        
        ///<summary>
        ///Run runs the schedule item once.
        ///</summary>
        public void Run()
        {
            Check();
            System.Collections.Generic.IDictionary<string, object> data = new Dictionary<string, object>();
            data.Add(new KeyValuePair<string, object>("Target", this));
            JobKey id = new JobKey(this.Name + this.ID.ToString(), this.Name + this.ID.ToString());

            IJobDetail job = JobBuilder.Create(typeof(Gurux.Device.GXScheduleJob))
                .WithIdentity(id)
                .SetJobData(new JobDataMap(data))
                .StoreDurably(true)
                .Build();
            this.Parent.Parent.m_sched.AddJob(job, true);
            this.Parent.Parent.m_sched.TriggerJob(id);
            this.Status = ScheduleState.Run;
            this.NotifyChange(ScheduleState.Start);           
        }
        
        ///<summary>
        ///Start activates the schedule and transaction is started when the schedule triggers.
        ///</summary>
        public void Start()
        {
            Check();
            System.Collections.Generic.IDictionary<string, object> data = new Dictionary<string, object>();
            data.Add(new KeyValuePair<string,object>("Target", this));
            JobKey id = new JobKey(this.Name + this.ID.ToString(), this.Name + this.ID.ToString());
            IJobDetail job = JobBuilder.Create(typeof(Gurux.Device.GXScheduleJob)).WithIdentity(new JobKey(this.Name + this.ID.ToString(), this.Name + this.ID.ToString()))
                .WithIdentity(id)
                .SetJobData(new JobDataMap(data))
                .Build();            
            ITrigger it = GetTrigger();
            this.Parent.Parent.m_sched.ScheduleJob(job, it);
            this.Status = ScheduleState.Run;
            this.NotifyChange(ScheduleState.Start);
        }
        
        ///<summary>
        ///Stop disables the schedule.
        ///</summary>
        public void Stop()
        {
            if ((this.Status & ScheduleState.Run) != 0)
            {
                this.Status = ScheduleState.None;
                this.NotifyChange(ScheduleState.End);
                this.Parent.Parent.m_sched.DeleteJob(new JobKey(this.Name + this.ID.ToString(), this.Name + this.ID.ToString()));
            }
        }
    }
}
