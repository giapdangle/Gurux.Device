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
using System.Runtime.Serialization;
using System.ComponentModel;
using Gurux.Device.Properties;

namespace Gurux.Device
{
	/// <summary>
	/// Contains information about Media Type.
	/// </summary>
    [System.ComponentModel.Editor(typeof(GXMediaTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
    [DataContract()]
    [Serializable]
    public class GXMediaType
    {
        string m_Name;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GXMediaType()
        {

        }

		/// <summary>
		/// Parent collection.
		/// </summary>
        [System.Xml.Serialization.XmlIgnore()]
        public GXMediaTypeCollection Parent
        {
            get;
            internal set;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GXMediaType(string name, string defaultMediaSettings)
        {
            Name = name;
            DefaultMediaSettings = defaultMediaSettings;
        }

        /// <summary>
        /// Object name.
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                bool change = m_Name != value;
                m_Name = value;
                if (change && Parent != null && Parent.Parent != null)
                {
                    Parent.Parent.Dirty = true;
                }
            }
        }

        string m_DefaultMediaSettings;
        /// <summary>
		/// DefaultMediaSettings returns default media settings in XML format. 
		/// </summary>
		/// <remarks>
		/// DefaultMediaSettings are read from a device type template.
		/// </remarks>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string DefaultMediaSettings
        {
            get
            {
                return m_DefaultMediaSettings;
            }
            set
            {
                bool change = m_DefaultMediaSettings != value;
				if (!string.IsNullOrEmpty(value))
				{
					value = value.Replace("\r\n", "");
				}
                m_DefaultMediaSettings = value;
                if (change && Parent != null && Parent.Parent != null)
                {
                    Parent.Parent.Dirty = true;
                }
            }
        }

		/// <summary>
		/// Returns if the media settinsg are "Default" or "Changed".
		/// </summary>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(DefaultMediaSettings))
            {
                return Resources.Default;
            }
            return Resources.Changed;
        }
    }
}
