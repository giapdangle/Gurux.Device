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
using System.Xml.Serialization;
using Gurux.Device.PresetDevices;
using Gurux.Device;

namespace Gurux.Device
{
	/// <summary>
	/// Basic information about a GXDevice type.
	/// </summary>
    [DataContract()]
    [Serializable]
    public class GXDeviceProfile
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDeviceProfile()
        {
        }

        /// <summary>
        /// Copy Constructor.
        /// </summary>
        public GXDeviceProfile(GXDeviceProfile item)
        {
            Protocol = item.Protocol;
            Name = item.Name;
            PresetName = item.PresetName;
            Description = item.Description;
            this.DeviceGuid = item.DeviceGuid;
        }

        /// <summary>
        /// Returns device type parent collection.
        /// </summary>
        [XmlIgnore()]
        virtual public GXDeviceProfileCollection Parent
        {
            get;
            internal set;
        }

        /// <summary>
		/// Device profile protocol name.
		/// </summary>
        [DataMember(IsRequired = true)]
		public string Protocol
        {
            get;
            set;
        }
        
        /// <summary>
		/// Path is the file path of the device template.
		/// </summary>
        public virtual string Path
        {
            get
            {
                if (DeviceGuid == Guid.Empty)
                {
                    return GXDevice.GetDeviceProfilesPath(Protocol, Name);
                }
                return GXDevice.GetDeviceProfilesPath(Protocol, DeviceGuid);
            }
        }

		/// <summary>
		/// The name of the device type.
		/// </summary>
        [DataMember(IsRequired = true)]
        public string Name
        {
            get;
            set;
        }  

        /// <summary>
		/// The preset name of the device profile.
		/// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string PresetName
        {
            get;
            set;
        }

        /// <summary>
        /// Description.
        /// </summary>
        [DefaultValue(null)]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Device profile guid.
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public Guid DeviceGuid
        {
            get;
            set;
        }

        public override string ToString()
        {
            return Protocol + "_" + Name;
        }
    }
}
