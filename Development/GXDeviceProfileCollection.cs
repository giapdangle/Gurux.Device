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

namespace Gurux.Device
{
	/// <summary>
	/// A collection of GXDeviceTypes.
	/// </summary>
    [CollectionDataContract()]
    [Serializable]
    public class GXDeviceProfileCollection : List<GXDeviceProfile>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDeviceProfileCollection()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDeviceProfileCollection(object parent)
        {
            Parent = parent;
        }

		/// <summary>
		/// String indexer using GXDeviceType.name.
		/// </summary>
        public GXDeviceProfile this[string name]
        {
            get
            {
                foreach (GXDeviceProfile it in this)
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
                foreach (GXDeviceProfile it in this)
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
        public object Parent
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
        public new void Add(GXDeviceProfile item)
        {
            GXDeviceProfile it = item as GXDeviceProfile;
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
        public GXDeviceProfile Find(string presetName)
        {
            foreach (GXDeviceProfile type in this)
            {
                if (string.Compare(presetName, type.PresetName, true) == 0)
                {
                    return type;
                }
            }
            return null;
        }

        public GXDeviceProfile Find(string protocolName, string deviceType)
        {
            foreach (GXDeviceProfile type in this)
            {
                if (string.Compare(protocolName, type.Protocol, true) == 0 &&
                    string.Compare(deviceType, type.Name, true) == 0)
                {
                    return type;
                }
            }
            return null;
        }
    }
}