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

namespace Gurux.Device.Editor
{
    /// <summary>
    /// GXLocalizedNameAttribute is a custom attribute class for localized name texts.
    /// </summary>
    public class GXLocalizedNameAttribute : Attribute
    {
        string m_ResourceStr = string.Empty;

        /// <summary>
        /// Gets the localized name text.
        /// </summary>
        public string ResourceStr
        {
            get
            {
                return m_ResourceStr;
            }
        }

        /// <summary>
        /// Initializes a new instance of the GXLocalizedNameAttribute.
        /// </summary>
        /// <param name="txt">Localized resource name.</param>
        public GXLocalizedNameAttribute(string txt)
        {
            m_ResourceStr = txt;
        }
    }
}