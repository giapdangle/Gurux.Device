using System;
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
    /// Obsolete.
    /// </summary>
    class GXPublishedDeviceType : GXDeviceProfile
    {

    }

    /// <summary>
    /// Device version.
    /// </summary>
    [DataContract()]
    [Serializable]
    public class GXDeviceVersion
    {
        GXDeviceProfileCollection m_Profiles;

        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDeviceVersion()
        {
            Profiles = new GXDeviceProfileCollection(this);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDeviceVersion(string name)
        {
            Name = name;
            m_Profiles = new GXDeviceProfileCollection(this);
        }
        
        /// <summary>
        /// Copy Constructor.
        /// </summary>
        public GXDeviceVersion(GXDeviceVersion item)
        {
            Name = item.Name;
            Description = item.Description;
            m_Profiles = new GXDeviceProfileCollection(this);
            foreach (GXDeviceProfile it in item.Profiles)
            {
                m_Profiles.Add(it);
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
        public GXDeviceProfileCollection Profiles
        {
            get
            {
                return m_Profiles;
            }
            internal set
            {
                m_Profiles = value;
                if (m_Profiles != null)
                {
                    m_Profiles.Parent = this;
                }
            }
        }       
    }
}
