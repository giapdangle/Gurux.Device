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
using System.IO;
using System.Xml;
using Gurux.Common;

namespace Gurux.Device
{
	/// <summary>
	/// A collection of GXDeviceTypes.
	/// </summary>
    [CollectionDataContract()]
    [Serializable]
    public class GXDeviceProfileCollection : GenericList<GXDeviceProfile>
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

        protected override void OnBeforeItemAdded(object sender, GenericItemEventArgs<GXDeviceProfile> e)
        {
            if (e.Item.Parent == null)
            {
                e.Item.Parent = this;
            }
        }

        /// <summary>
        /// Remove item ID and parent. 
        /// </summary>
        protected override void OnBeforeItemRemoved(object sender, GenericItemEventArgs<GXDeviceProfile> e)
        {
            if (e.Item.Parent == this)
            {                
                e.Item.Parent = null;
            }
        }


        /// <summary>
        /// Get devices by publish date.
        /// </summary>
        /// <param name="dt">Lower publish date.</param>
        /// <returns>Collection of published devices.</returns>
        public GXDeviceProfileCollection GetDevicesByPublishDate(DateTime dt)
        {
            GXDeviceProfileCollection items = new GXDeviceProfileCollection();
            foreach (GXDeviceProfile it in this)
            {
                if (it.Date > dt)
                {
                    items.Add(it);
                }
            }
            return items;
        }      

        /// <summary>
        /// Find device profile by protocol name and profile name.
        /// </summary>
        /// <param name="protocol">Protocol name.</param>
        /// <param name="profile">Profile name.</param>
        /// <returns>Found device profile or null if not found.</returns>
        public GXDeviceProfile Find(string protocol, string profile)
        {
            foreach (GXDeviceProfile type in this)
            {
                if (string.Compare(protocol, type.Protocol, true) == 0 &&
                    string.Compare(profile, type.Name, true) == 0)
                {
                    return type;
                }
            }
            return null;
        }

        /// <summary>
        /// Find device profile.
        /// </summary>
        /// <param name="guid">Guid to find</param>
        /// <returns>Device profile.</returns>
        public GXDeviceProfile Find(Guid guid)
        {
            foreach (GXDeviceProfile type in this)
            {
                if (type.Guid == guid)
                {
                    return type;
                }
            }
            return null;
        }

        /// <summary>
        /// Find device profile(s) by target Guid.
        /// </summary>
        /// <param name="guid">Guid to find</param>
        /// <returns>Device profiles.</returns>
        /// <remarks>
        /// There might be several versions from same profile
        /// </remarks>
        public GXDeviceProfile[] FindProfiles(Guid guid)
        {
            List<GXDeviceProfile> items = new List<GXDeviceProfile>();
            foreach (GXDeviceProfile it in this)
            {
                if (it.ProfileGuid == guid)
                {
                    items.Add(it);
                }
            }
            return items.ToArray();
        }             
    }
}
