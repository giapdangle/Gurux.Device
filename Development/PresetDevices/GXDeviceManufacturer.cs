using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.ComponentModel;
using System;

namespace Gurux.Device.PresetDevices
{
    /// <summary>
    /// Device manufacturer.
    /// </summary>
    [DataContract()]
    [Serializable]
    public class GXDeviceManufacturer
    {
        GXDeviceModelCollection m_Models;

        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDeviceManufacturer()
        {
            m_Models = new GXDeviceModelCollection(this);            
        }
        
        /// <summary>
        /// Copy Constructor.
        /// </summary>
        public GXDeviceManufacturer(GXDeviceManufacturer item)
        {
            m_Models = new GXDeviceModelCollection(this);
            Name = item.Name;
            Url = item.Url;
            foreach (GXDeviceModel it in item.Models)
            {
                Models.Add(new GXDeviceModel(it));
            }            
        }

        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Manufacturer name.
        /// </summary>
        [DataMember(IsRequired = true)] 
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Parent collection of manufacturer item.
        /// </summary>
        [IgnoreDataMember()]
        [XmlIgnore()]
        public GXDeviceManufacturerCollection Parent
        {
            get;
            internal set;
        }        

        /// <summary>
        /// Manufacturer web address.
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(null)]
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// Models of manufacturer.
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public GXDeviceModelCollection Models
        {
            get
            {
                return m_Models;
            }
            internal set
            {
                m_Models = value;
                if (m_Models != null)
                {
                    m_Models.Parent = this;
                }
            }
        }
    }
}
