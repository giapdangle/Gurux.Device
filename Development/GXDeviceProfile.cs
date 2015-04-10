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
            Description = item.Description;
            this.Guid = Guid;
            this.ProfileGuid = item.ProfileGuid;
            this.DeviceManufacturer = item.DeviceManufacturer;
            this.DeviceModel = item.DeviceModel;
            this.DeviceVersion = item.DeviceVersion;
            this.Publisher = item.Publisher;
            this.Anynomous = item.Anynomous;
            this.Version = item.Version;
            this.Date = item.Date;
        }

        /// <summary>
        /// Copy Constructor.
        /// </summary>
        public GXDeviceProfile(Guid guid, Guid profileGuid)
        {
            this.Guid = guid;
            this.ProfileGuid = profileGuid;
        }

        /// <summary>
        /// Returns device profile parent collection.
        /// </summary>
        [XmlIgnore()]
        public GXDeviceProfileCollection Parent
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
		/// The name of the device type.
		/// </summary>
        [DataMember(IsRequired = true)]
        public string Name
        {
            get;
            set;
        }  

        /// <summary>
        /// Device manufacturer name.
		/// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string DeviceManufacturer
        {
            get;
            set;
        }

        /// <summary>
        /// Device model name.
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string DeviceModel
        {
            get;
            set;
        }

        /// <summary>
        /// Device version name.
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string DeviceVersion
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
        /// <remarks>
        /// Profile Guid is generated when device profile is created.
        /// Device profile do not change when new version is made.
        /// </remarks>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public Guid ProfileGuid
        {
            get;
            set;
        }

        /// <summary>
        /// Device main Guid.
        /// </summary>
        /// <remarks>
        /// New guid is generated when new version is created.
        /// </remarks>
        [DataMember(IsRequired = true)]
        public System.Guid Guid
        {
            get;            
            set;
        }

        /// <summary>
        /// Publisher's Gurux community name of this version.
        /// </summary>
        [DefaultValue(null)]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string Publisher
        {
            get;
            set;
        }

        /// <summary>
        /// Is device publised as anynomous.
        /// </summary>
        [DefaultValue(false)]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public bool Anynomous
        {
            get;
            set;
        }

        /// <summary>
        /// Profile version number.
        /// </summary>
        [DefaultValue(0)]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public int Version
        {
            get;
            set;
        }

        /// <summary>
        /// Profile published date.
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public DateTime Date
        {
            get;
            set;
        }

        /// <summary>
        /// Give version as string.
        /// </summary>
        /// <returns></returns>
        public string VersionToString()
        {
            return VersionToString(Version);
        }

        /// <summary>
        /// Path is the file path of the device template.
        /// </summary>
        public virtual string Path
        {
            get
            {
                return GXDevice.GetDeviceProfilePath(Protocol, Guid);
            }
        }

        /// <summary>
        /// Converts version number to string.
        /// </summary>
        /// <param name="version"></param>
        /// <returns>Version number as a string.</returns>
        static string VersionToString(int version)
        {
            return ((version >> 24) & 0xFF).ToString() + "." +
                    ((version >> 16) & 0xFF).ToString() +
                    "." + ((version >> 8) & 0xFF).ToString() + "." +
                    (version & 0xFF).ToString();
        }

        public override string ToString()
        {
            return Protocol + "_" + Name + "_" + VersionToString(Version);
        }

    }
}
