//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL$
//
// Version:         $Revision$,
//                  $Date$
//                  $Author$
//
// Copyright (c) Gurux Ltd
//
//---------------------------------------------------------------------------
//
//  DESCRIPTION
//
// This file is a part of Gurux Device Framework.
//
// Gurux Device Framework is Open Source software; you can redistribute it
// and/or modify it under the terms of the GNU General Public License 
// as published by the Free Software Foundation; version 2 of the License.
// Gurux Device Framework is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of 
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details.
//
// This code is licensed under the GNU General Public License v2. 
// Full text may be retrieved at http://www.gnu.org/licenses/gpl-2.0.txt
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Gurux.Device.PresetDevices
{
	/// <summary>
	/// A collection of GXDeviceTypes.
	/// </summary>
    [CollectionDataContract()]
    public class GXPublishedDeviceTypeCollection : GenericList<GXPublishedDeviceType>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public GXPublishedDeviceTypeCollection()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public GXPublishedDeviceTypeCollection(GXDeviceVersion parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Sets parent.
        /// </summary>
        protected override void OnBeforeItemAdded(object sender, GenericItemEventArgs<GXPublishedDeviceType> e)
        {
            e.Item.Parent = this;
        }

		/// <summary>
		/// String indexer using GXDeviceType.name.
		/// </summary>
        public GXPublishedDeviceType this[string name]
        {
            get
            {
                foreach (GXPublishedDeviceType it in this)
                {
                    if (string.Compare(it.Name, name, true) == 0)
                    {
                        return it;                        
                    }
                }
                return null;
            }
            set
            {
                int pos = 0;
                foreach (GXPublishedDeviceType it in this)
                {
                    if (string.Compare(it.Name, name, true) == 0)
                    {
                        this[pos] = value;
                        break;
                    }
                    ++pos;
                }
            }
        }

        /// <summary>
        /// Returns device type collection parent collection.
        /// </summary>
        [XmlIgnore()]
        [IgnoreDataMember()]
        public GXDeviceVersion Parent
        {
            get;
            internal set;
        }

        /// <summary>
        /// Add the specified value.
        /// </summary>
        /// <param name='item'>
        /// added item.
        /// </param>
        /// <remarks>
        /// Mono needs this. Do not remove!
        /// </remarks>
        public new void Add(GXPublishedDeviceType item)
        {
            GXPublishedDeviceType it = item as GXPublishedDeviceType;
            if (it.Parent == null)
            {
                it.Parent = this;
            }
            base.Add(it);
        }


        /// <summary>
        /// Find device template by preset name.
        /// </summary>
        /// <param name="publishedName">Name of preset device template.</param>
        /// <returns>Found device template item.</returns>
        public GXPublishedDeviceType Find(string presetName)
        {
            foreach (GXPublishedDeviceType type in this)
            {
                if (string.Compare(presetName, type.PresetName, true) == 0)
                {
                    return type;
                }
            }
            return null;
        }

        /// <summary>
        /// Find device template by guid.
        /// </summary>
        /// <param name="manufacturer">Name of the manufacturer.</param>
        /// <returns>Found manufacturer item.</returns>
        public GXPublishedDeviceType Find(GXPublishedDeviceType type)
        {
            foreach (GXPublishedDeviceType dt in this)
            {
                if (dt.Guid == type.Guid)
                {
                    return dt;
                }
            }
            return null;
        }
    }
}