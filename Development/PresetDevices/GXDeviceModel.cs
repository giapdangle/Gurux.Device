using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Gurux.Device.PresetDevices
{
    /// <summary>
    /// Device model.
    /// </summary>
    [DataContract()]
    public class GXDeviceModel
    {
        GXDeviceVersionCollection m_Versions;

        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDeviceModel()
        {
            Versions = new GXDeviceVersionCollection(this);
            Description = null;
        }

        /// <summary>
        /// Copy Constructor.
        /// </summary>
        public GXDeviceModel(GXDeviceModel item)
        {
            Versions = new GXDeviceVersionCollection(this);
            Name = item.Name;
            Description = item.Description;
            foreach (GXDeviceVersion it in item.Versions)
            {
                Versions.Add(new GXDeviceVersion(it));
            }
        }       

        /// <summary>
        /// Parent collection of device model.
        /// </summary>
        [IgnoreDataMember()]
        [XmlIgnore()]
        public GXDeviceModelCollection Parent
        {
            get;
            internal set;
        }

        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Device model name.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Model description
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue("")]
        public string Description
        {
            get;
            set;
        }

        [DataMember(IsRequired = true)]
        public GXDeviceVersionCollection Versions
        {
            get
            {
                return m_Versions;
            }
            internal set
            {
                m_Versions = value;
                if (m_Versions != null)
                {
                    m_Versions.Parent = this;
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
