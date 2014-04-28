using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace Gurux.Device.PresetDevices
{
    /// <summary>
    /// List of device versions.
    /// </summary>
    [CollectionDataContract()]
    [Serializable]
    public class GXDeviceProfileVersionCollection : GenericList<GXDeviceProfileVersion>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDeviceProfileVersionCollection()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDeviceProfileVersionCollection(GXPublishedDeviceProfile parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Sets parent.
        /// </summary>
        protected override void OnBeforeItemAdded(object sender, GenericItemEventArgs<GXDeviceProfileVersion> e)
        {
            e.Item.Parent = this;
        }

        /// <summary>
        /// Add the specified value.
        /// </summary>
        /// <param name='item'>
        /// Value.
        /// </param>
        /// <remarks>
        /// Mono needs this. Do not remove!
        /// </remarks>
        public new void Add(GXDeviceProfileVersion item)
        {
            GXDeviceProfileVersion it = item as GXDeviceProfileVersion;
            bool found = false;
            foreach(GXDeviceProfileVersion ver in this)
            {
                if (ver.Version == item.Version)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                if (it.Parent == null)
                {
                    it.Parent = this;
                }
                base.Add(it);
            }
        }

        /// <summary>
        /// Parent Device type.
        /// </summary>
        [XmlIgnore()]
        [IgnoreDataMember()]
        public GXPublishedDeviceProfile Parent
        {
            get;
            internal set;
        }

        /// <summary>
        /// Find template version by version number.
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public GXDeviceProfileVersion Find(int version)
        {
            foreach (GXDeviceProfileVersion it in this)
            {
                if (it.Version == version)
                {
                    return it;
                }
            }
            return null;
        }

        /// <summary>
        /// Find template version by guid.
        /// </summary>
        /// <param name="version">device version.</param>
        /// <returns>Found device version item.</returns>
        public GXDeviceProfileVersion Find(GXDeviceProfileVersion version)
        {
            foreach (GXDeviceProfileVersion v in this)
            {
                if (v.Guid == version.Guid)
                {
                    return v;
                }
            }
            return null;
        }

    }
}
