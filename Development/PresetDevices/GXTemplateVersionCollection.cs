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
    public class GXTemplateVersionCollection : GenericList<GXTemplateVersion>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public GXTemplateVersionCollection()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public GXTemplateVersionCollection(GXPublishedDeviceType parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Sets parent.
        /// </summary>
        protected override void OnBeforeItemAdded(object sender, GenericItemEventArgs<GXTemplateVersion> e)
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
        public new void Add(GXTemplateVersion item)
        {
            GXTemplateVersion it = item as GXTemplateVersion;
            bool found = false;
            foreach(GXTemplateVersion ver in this)
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
        public GXPublishedDeviceType Parent
        {
            get;
            internal set;
        }

        /// <summary>
        /// Find template version by version number.
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public GXTemplateVersion Find(int version)
        {
            foreach (GXTemplateVersion it in this)
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
        public GXTemplateVersion Find(GXTemplateVersion version)
        {
            foreach (GXTemplateVersion v in this)
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
