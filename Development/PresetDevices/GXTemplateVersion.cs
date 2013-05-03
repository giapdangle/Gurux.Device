using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Gurux.Device.PresetDevices
{
    /// <summary>
    /// Device template version.
    /// </summary>
    [DataContract()]
    public class GXTemplateVersion
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public GXTemplateVersion()
        {
            this.Guid = System.Guid.NewGuid();
        }

        /// <summary>
        /// Copy Constructor.
        /// </summary>
        public GXTemplateVersion(GXTemplateVersion item)
        {
            Publisher = item.Publisher;
            Anynomous = item.Anynomous;
            Version = item.Version;
            Date = item.Date;
            Status = item.Status;
            this.Guid = item.Guid;
        }

        public override string ToString()
        {
            return VersionToString(Version);
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

        /// <summary>
        /// Parent template version type.
        /// </summary>
        [XmlIgnore()]
        [IgnoreDataMember()]
        public GXTemplateVersionCollection Parent
        {
            get;
            internal set;
        }

        /// <summary>y
        /// Template Guid.
        /// </summary>
        [DataMember(IsRequired = true)]
        public System.Guid Guid
        {
            get;
            internal set;
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
        /// Template version number.
        /// </summary>
        [DefaultValue(0)]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public int Version
        {
            get;
            set;
        }

        /// <summary>
        /// Template published date.
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public DateTime Date
        {
            get;
            set;
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
