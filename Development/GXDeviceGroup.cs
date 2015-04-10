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
using Gurux.Device.Editor;
using Gurux.Device.Properties;

namespace Gurux.Device
{
	/// <summary>
	/// A collection of GXDevices. Contains methods and properties related to child GXDevices.
	/// </summary>
    [TypeConverter(typeof(GXTypeConverterNoExpand))]
    [ToolboxItem(false)]
    [GXDataIOSourceAttribute(true, GXDataIOSourceType.DeviceGroup, GXDeviceGroup.AvailableTargets.All)]
    [DataContract()]
    public class GXDeviceGroup : GXSite
    {
        DisabledActions m_DisabledActions;
        string m_Name;
        GXDeviceGroupCollection m_DeviceGroups;
        private GXDeviceCollection m_Devices;

        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDeviceGroup()
        {
            DeviceGroups = new GXDeviceGroupCollection();
            Devices = new GXDeviceCollection();
        }

        /// <summary>
        /// Override this to made changes before device group load.
        /// </summary>
        protected override void OnDeserializing(bool designMode)
        {
            DeviceGroups = new GXDeviceGroupCollection();
            Devices = new GXDeviceCollection();
        }

        /// <summary>
        /// Enumerates what information the class offers for the DataIOSource to use.
        /// </summary>
        /// <seealso cref="GXTable.AvailableTargets">GXTable.AvailableTargets</seealso>
        /// <seealso cref="GXCategory.AvailableTargets">GXCategory.AvailableTargets</seealso>
        /// <seealso cref="GXProperty.AvailableTargets">GXProperty.AvailableTargets</seealso>
        [Flags]
        public enum AvailableTargets : long
        {
			/// <summary>
			/// Everything is targetable.
			/// </summary>
            All = -1,
			/// <summary>
			/// Nothing is targetable.
			/// </summary>
            None = 0x0,
			/// <summary>
			/// Target is a name.
			/// </summary>
            Name = 0x1,
        }

        /// <summary>
        /// Object Identifier.
        /// </summary>
        [Browsable(false), ReadOnly(true), DefaultValue(0)]
        [DataMember(Name = "ID", IsRequired = true)]
        public ulong ID
        {
            get;
            internal set;
        }

		/// <summary>
		/// The name of the GXDeviceGroup.
		/// </summary>
        [DataMember(IsRequired = true)]
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
                NotifyDeviceGroupUpdated(new GXItemEventArgs(this));
            }
        }

		/// <summary>
		/// The parent of the GXDeviceGroup.
		/// </summary>
        public GXDeviceGroupCollection Parent
        {
            get;
            internal set;
        }

		/// <summary>
		/// Child GXDeviceGroups.
		/// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public GXDeviceGroupCollection DeviceGroups
        {
            get
            {
                return m_DeviceGroups;
            }
            internal set
            {
                m_DeviceGroups = value;
                if (m_DeviceGroups != null)
                {
                    m_DeviceGroups.Parent = this;
                }
            }
        }
       
        /// <summary>
        /// Device collection.
        /// </summary>
        /// <remarks>
        /// Do not save this.
        /// </remarks>
        public GXDeviceCollection Devices
        {
            get
            {
                return m_Devices;
            }
            internal set
            {
                m_Devices = value;
                if (m_Devices != null)
                {
                    m_Devices.Parent = this;
                }
            }
        }

        [DataMember(Name = "Devices", IsRequired = false, EmitDefaultValue = false)]
        GXSerializedDevice[] SerializedDevices
        {
            get
            {
                Dictionary<Guid, GXDevice> DeviceProfiles = new Dictionary<Guid,GXDevice>();
                GXSerializedDevice[] devices = new GXSerializedDevice[m_Devices.Count];                                
                int pos = -1;
                foreach (GXDevice it in m_Devices)
                {
                    GXDevice dev = null;
                    if (DeviceProfiles.ContainsKey(it.ProfileGuid))
                    {
                        dev = DeviceProfiles[it.ProfileGuid];
                    }
                    else
                    {
                        string path = it.ProfilePath;
                        dev = GXDevice.Load(path);
                        DeviceProfiles.Add(it.ProfileGuid, dev);
                    }
                    devices[++pos] = new GXSerializedDevice(it, dev);
                }
                return devices;
            }
            set
            {
                foreach (GXSerializedDevice it in value)
                {
                    m_Devices.Add(it.CreateDevice());
                }
            }
        }

        /// <summary>
        /// FindItemByID finds an item using item identification.
        /// </summary>
        /// <param name="id">Item ID</param>
        /// <returns>
        /// Returns any item whose ID matches the given ID.
        /// </returns>
        public virtual object FindItemByID(UInt64 id)
        {
            if (id == 0)
            {
                throw new Exception(Resources.FindItemByIDFailedSearchIDCanTBeZero);
            }
            if (id == this.ID)
            {
                return this;
            }
            object item = null;
            foreach (GXDevice it in m_Devices)
            {
                item = it.FindItemByID(id);
                if (item != null)
                {
                    return item;
                }
            }
            foreach (GXDeviceGroup it in m_DeviceGroups)
            {
                item = it.FindItemByID(id);
                if (item != null)
                {
                    return item;
                }
            }
            return null;
        }

		/// <summary>
		/// Connect all the child devices.
		/// </summary>
        public void Connect()
        {
            foreach (GXDevice it in this.m_Devices)
            {
                it.Connect();
            }   
        }

		/// <summary>
		/// Disconnect all the child devices.
		/// </summary>
        public void Disconnect()
        {
            foreach (GXDevice it in this.m_Devices)
            {
                it.Disconnect();
            }
        }

		/// <summary>
		/// Start monitoring all the child devices.
		/// </summary>
        public void StartMonitoring()
        {
            foreach (GXDevice it in this.m_Devices)
            {
                if ((it.DisabledActions & DisabledActions.Monitor) != 0)
                {
                    it.StartMonitoring();
                }
            }
        }

		/// <summary>
		/// Stop monitoring all the child devices.
		/// </summary>
        public void StopMonitoring()
        {
            foreach (GXDevice it in this.m_Devices)
            {
                it.StopMonitoring();
            }
        }

		/// <summary>
		/// Get devices recursively from this and all the child device groups.
		/// </summary>
        public GXDeviceCollection GetDevicesRecursive()
        {
            GXDeviceCollection devices = new GXDeviceCollection();
			devices.AddRange(this.Devices);
			foreach (GXDeviceGroup dg in this.DeviceGroups)
			{
				devices.AddRange(dg.GetDevicesRecursive());
			}
            return devices;
        }

        /// <summary>
        /// Determines which actions are blocked from use.
        /// </summary>
        [ValueAccess(ValueAccessType.None, ValueAccessType.Edit)]
        [Browsable(false)]
        [DefaultValue(DisabledActions.None)]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public DisabledActions DisabledActions
        {
            get
            {
                return m_DisabledActions;
            }
            set
            {
                m_DisabledActions = value;
            }
        }

		/// <summary>
		/// Read all child GXDevices.
		/// </summary>
        public void Read()
        {
            foreach (GXDevice it in this.m_Devices)
            {
                it.Read();
            }
        }

		/// <summary>
		/// Write all child GXDevices.
		/// </summary>
		public void Write()
        {
            foreach (GXDevice it in this.m_Devices)
            {
                it.Write();
            }
        }

		/// <summary>
		/// Reset all child GXDevices.
		/// </summary>
        public void Reset(ResetTypes type)
        {
            foreach (GXDevice it in this.m_Devices)
            {
                it.Reset(type);
            }
        }

        /// <summary>
        /// Reset statistic value of determined type.
        /// </summary>
        public void ResetStatistic()
        {
            foreach (GXDevice it in this.m_Devices)
            {
                it.Statistics.Reset();
            }
        }

		/// <summary>
		/// Notifies, when an item has changed.
		/// </summary>
        public event ItemUpdatedEventHandler DeviceGroupUpdated;

        internal virtual void NotifyDeviceGroupUpdated(GXItemEventArgs e)
        {
            if (DeviceGroupUpdated != null)
            {
                DeviceGroupUpdated(this, e);
            }
            if (Parent != null)
            {
                Parent.OnDeviceGroupUpdated(this, e);
            }
        }
    }
}
