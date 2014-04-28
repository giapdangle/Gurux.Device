using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Collections;

namespace Gurux.Device.PresetDevices
{
    /// <summary>
    /// List of device models
    /// </summary>
    [CollectionDataContract()]
    [Serializable]
    public class GXDeviceModelCollection : GenericList<GXDeviceModel>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDeviceModelCollection()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parent"></param>
        public GXDeviceModelCollection(GXDeviceManufacturer parent)
        {
            Parent = parent;
        }
        
        /// <summary>
		/// Sets parent.
		/// </summary>
        protected override void OnBeforeItemAdded(object sender, GenericItemEventArgs<GXDeviceModel> e)
        {
            e.Item.Parent = this;
        }

        /// <summary>
        /// Find manufacturer model by name.
        /// </summary>
        /// <param name="modelName">Model of the manufacturer.</param>
        /// <returns>Found model item.</returns>
        public GXDeviceModel Find(string modelName)
        {
            foreach (GXDeviceModel model in this)
            {
                if (string.Compare(modelName, model.Name, true) == 0)
                {
                    return model;
                }
            }
            return null;
        }

        /// <summary>
        /// Find manufacturer model by guid.
        /// </summary>
        /// <param name="manufacturer">Name of the manufacturer.</param>
        /// <returns>Found manufacturer item.</returns>
        public GXDeviceModel Find(GXDeviceModel model)
        {
            foreach (GXDeviceModel m in this)
            {
                if (m.Name == model.Name)
                {
                    return m;
                }
            }
            return null;
        }

        /// <summary>
        /// Parent of device manufacturer collection.
        /// </summary>
        [XmlIgnore()]
        [IgnoreDataMember()]
        public GXDeviceManufacturer Parent
        {
            get;
            internal set;
        }
    }
}
