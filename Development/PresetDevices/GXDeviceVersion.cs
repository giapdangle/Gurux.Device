﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gurux.Device;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.ComponentModel;
using Gurux.Device.PresetDevices;

namespace Gurux.Device.PresetDevices
{
    /// <summary>
    /// Device version.
    /// </summary>
    [DataContract()]
    public class GXDeviceVersion
    {
        GXPublishedDeviceTypeCollection m_Templates;

        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDeviceVersion()
        {
            Templates = new GXPublishedDeviceTypeCollection(this);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDeviceVersion(string name)
        {
            Name = name;
            m_Templates = new GXPublishedDeviceTypeCollection(this);
        }

        /// <summary>
        /// Copy Constructor.
        /// </summary>
        public GXDeviceVersion(GXDeviceVersion item)
        {
            Name = item.Name;
            Description = item.Description;
            m_Templates = new GXPublishedDeviceTypeCollection(this);
            foreach (GXPublishedDeviceType it in item.Templates)
            {
                m_Templates.Add(new GXPublishedDeviceType(it));
            }
        }

        /// <summary>
        /// Device version parent.
        /// </summary>
        [XmlIgnore()]
        [IgnoreDataMember()]
        public GXDeviceVersionCollection Parent
        {
            get;
            internal set;
        }

        public override string ToString()
        {
            return Name;
        }
        
        /// <summary>
        /// Version name.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Version description.
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(null)]
        public string Description
        {
            get;
            set;
        } 

        /// <summary>
        /// Supported device templates.
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public GXPublishedDeviceTypeCollection Templates
        {
            get
            {
                return m_Templates;
            }
            internal set
            {
                m_Templates = value;
                if (m_Templates != null)
                {
                    m_Templates.Parent = this;
                }
            }
        }
        
        /// <summary>
        /// Download status.
        /// </summary>
        [DefaultValue(DownloadStates.None)]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public DownloadStates Status
        {
            get;
            set;
        }
    }
}