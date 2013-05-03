using System.Runtime.Serialization;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Gurux.Device.PresetDevices
{
    /// <summary>
    /// Published device type and data.
    /// </summary>
    [DataContract()]
    public class GXPublishedDeviceType : GXDeviceType
    {
        GXTemplateVersionCollection m_Versions;

        /// <summary>
        /// Constructor.
        /// </summary>
        public GXPublishedDeviceType()
        {
            m_Versions = new GXTemplateVersionCollection(this);
            this.Guid = System.Guid.NewGuid();
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        public GXPublishedDeviceType(GXDeviceType type)
        {
            Protocol = type.Protocol;
            Name = type.Name;
            PresetName = type.PresetName;
            Description = type.Description;
            Versions = new GXTemplateVersionCollection(this);
            GXPublishedDeviceType tmp = type as GXPublishedDeviceType;
            if (tmp != null)
            {
                if (tmp.Versions != null)
                {
                    Versions = new GXTemplateVersionCollection(this);
                    foreach (GXTemplateVersion it in tmp.Versions)
                    {
                        Versions.Add(new GXTemplateVersion(it));
                    }
                }
                this.Guid = tmp.Guid;
                this.DeviceGuid = tmp.DeviceGuid;
            }
            else
            {
                this.Guid = System.Guid.NewGuid();
            }
        }

        /// <summary>
        /// Device type Guid.
        /// </summary>
        [DataMember(IsRequired = true)]
        public System.Guid Guid
        {
            get;
            internal set;
        }

        /// <summary>
        /// Device Guid.
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public System.Guid DeviceGuid
        {
            get;
            set;
        }  

        /// <summary>
        /// Returns device type parent collection.
        /// </summary>
        [XmlIgnore()]
        new public GXPublishedDeviceTypeCollection Parent
        {
            get;
            internal set;
        }

        /// <summary>
        /// Path is the file path of the device template.
        /// </summary>
        public override string Path
        {
            get
            {
                if (Status == DownloadStates.Add)
                {
                    return GXDevice.GetDeviceTemplatePath(Protocol, Name);
                }
                return GXDevice.GetDeviceTemplatePath(DeviceGuid);
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

        /// <summary>
        /// Collection of Device template versions.
        /// </summary>
        [DefaultValue(null)]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GXTemplateVersionCollection Versions
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
        /// Returns info from the published device template.
        /// </summary>
        /// <param name="manufacturer"></param>
        /// <param name="model"></param>
        /// <param name="version"></param>
        /// <param name="presetName"></param>
        public void GetInfo(out string manufacturer, out string model, out string version, out string presetName)
        {
            presetName = this.PresetName;
            GXDeviceVersion ver = this.Parent.Parent;
            version = ver.Name;
            GXDeviceModel mdl = ver.Parent.Parent;
            model = mdl.Name;
            GXDeviceManufacturer m = mdl.Parent.Parent;
            manufacturer = m.Name;
        }

        public override string ToString()
        {
            if (Versions.Count == 0)
            {
                return Protocol + "_" + PresetName;
            }
            return Protocol + "_" + PresetName + "_" + Versions[Versions.Count - 1].ToString();
        }

    }
}
