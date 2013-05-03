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
using System.Reflection;
using System.Resources;

namespace Gurux.Device.Editor
{
    /// <summary>
    /// GXToolboxItemAttribute is a custom attribute class for Gurux toolbox items.
    /// </summary>
    /// <remarks>
    /// Toolbox items are controls that are in the toolbox.
    /// </remarks>
    public class GXToolboxItemAttribute : Attribute
    {
        private string m_Name;
        private string m_ItemName;
        private int m_Index;
        private int m_TabIndex;
        /// <summary>
        /// Gets or sets a value indicating if the GXToolBoxTab object is enabled.
        /// </summary>
        public bool Enabled = true;
        private Type m_ItemType;

		/// <summary>
		/// Initializes the attribute.
		/// </summary>
        protected void Init(int index, Type itemType, Type resourceType, string itemName, string name, int tabIndex, bool enabled)
        {
            this.Enabled = enabled;
            System.Resources.ResourceManager resources = new ResourceManager(resourceType.Namespace + ".Properties.Resources", resourceType.Assembly);
            string tmpName = resources.GetString(itemName);
            if (tmpName == null || tmpName.Trim().Length == 0)
            {
                m_ItemName = itemName;
            }
            else
            {
                m_ItemName = tmpName;
            }
            tmpName = resources.GetString(name);
            if (tmpName == null || tmpName.Trim().Length == 0)
            {
                m_Name = name;
            }
            else
            {
                m_Name = tmpName;
            }
            m_Index = index;
            m_TabIndex = tabIndex;
            m_ItemType = itemType;
        }

        /// <summary>
        /// Initializes a new instance of the GXToolBoxTab class.
        /// </summary>
        /// <param name="index">The zero-based index of the toolbox item.</param>
        /// <param name="itemType">The type of the item.</param>
		/// <param name="resourceType">The resource type of the toolbox item.</param>
		/// <param name="itemName">The resource name of the toolbox item.</param>
        /// <param name="name">The resource name of the toolbox tab</param>
        /// <param name="tabIndex">The index of the tab.</param>
        public GXToolboxItemAttribute(int index, Type itemType, Type resourceType, string itemName, string name, int tabIndex)
        {
            Init(index, itemType, resourceType, itemName, name, tabIndex, true);
        }

        /// <summary>
        /// Initializes a new instance of the GXToolBoxTab class.
        /// </summary>
        /// <param name="index">The zero-based index of the toolbox item.</param>
        /// <param name="itemType">The type of the item.</param>
		/// <param name="resourceType">The resource type of the toolbox item.</param>
		/// <param name="itemName">The resource name of the toolbox item.</param>
        /// <param name="name">The resource name of the toolbox tab</param>
        /// <param name="tabIndex">The index of the tab.</param>
        /// <param name="enabled">Is the item enabled.</param>
        public GXToolboxItemAttribute(int index, Type itemType, Type resourceType, string itemName, string name, int tabIndex, bool enabled)
        {
            Init(index, itemType, resourceType, itemName, name, tabIndex, enabled);
        }

        /// <summary>
        /// Gets the index of the tool box tab in a tool box.
        /// </summary>
        public int Index
        {
            get
            {
                return m_Index;
            }
        }

        /// <summary>
        /// Gets the name of the GXToolBoxTab object.
        /// </summary>
		public string Name
        {
            get
            {
                return m_Name;
            }
        }

        /// <summary>
        /// Gets the type information of the item.
        /// </summary>
        public Type ItemType
        {
            get
            {
                return m_ItemType;
            }
        }

        /// <summary>
        /// Gets the name of the item.
        /// </summary>
		public virtual string ItemName
        {
            get
            {
                return m_ItemName;
            }
			set
			{
				m_ItemName = value;
			}
        }

        /// <summary>
        /// Gets the tab index of the item.
        /// </summary>
        public int TabIndex
        {
            get
            {
                return m_TabIndex;
            }
        }
    }
}
