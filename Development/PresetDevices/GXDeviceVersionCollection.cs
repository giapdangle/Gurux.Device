using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Gurux.Device.PresetDevices
{
    /// <summary>
    /// List of device versions.
    /// </summary>
    [CollectionDataContract()]
    public class GXDeviceVersionCollection : GenericList<GXDeviceVersion>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDeviceVersionCollection()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDeviceVersionCollection(GXDeviceModel parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Sets parent.
        /// </summary>
        protected override void OnBeforeItemAdded(object sender, GenericItemEventArgs<GXDeviceVersion> e)
        {
            e.Item.Parent = this;
        }

        /// <summary>
        /// Device model name.
        /// </summary>
        [XmlIgnore()]
        [IgnoreDataMember()]
        public GXDeviceModel Parent
        {
            get;
            internal set;
        }

        /// <summary>
        /// Find manufacturer model version number by name.
        /// </summary>
        /// <param name="versionName">Version number of the device.</param>
        /// <returns>Found version number item.</returns>
        public GXDeviceVersion Find(string versionName)
        {
            foreach (GXDeviceVersion version in this)
            {
                if (string.Compare(versionName, version.Name, true) == 0)
                {
                    return version;
                }
            }
            return null;
        }

        /// <summary>
        /// Find device version by guid.
        /// </summary>
        /// <param name="version">device version.</param>
        /// <returns>Found device version item.</returns>
        public GXDeviceVersion Find(GXDeviceVersion version)
        {
            foreach (GXDeviceVersion v in this)
            {
                if (v.Name == version.Name)
                {
                    return v;
                }
            }
            return null;
        }
    }
}
