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
    /// GXLocalizedDescriptionAttribute is a custom attribute class for localize object's property description.
	/// </summary>
	public class GXLocalizedDescriptionAttribute : Attribute
	{
		string m_ResourceStr = string.Empty;

		/// <summary>
        /// Gets the localized description text.
		/// </summary>
		public string ResourceStr
		{
			get
			{
				return m_ResourceStr;
			}
		}

		/// <summary>
		/// Initializes a new instance of the GXLocalizedDescriptionAttribute class.
		/// </summary>
		/// <param name="txt">Localized description for the new instance.</param>
		public GXLocalizedDescriptionAttribute(string txt)
		{
			m_ResourceStr = txt;
		}
	}
}
