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
	/// This attribute is used to determine the visibility of the property, according to user feature level. 
	/// </summary>
	public class GXUserLevelAttribute : Attribute
	{
		/// <summary>
        /// The type of user level determines visibility of the property.
		/// </summary>
        /// <seealso cref="UserLevelType">UserLevelType</seealso>
		public UserLevelType Type;

		/// <summary>
		/// Initializes a new instance of the GXUserLevelAttribute class.
		/// </summary>
		/// <param name="type">User level type of the new instance.</param>
        /// <seealso cref="UserLevelType">UserLevelType</seealso>
		public GXUserLevelAttribute(UserLevelType type)
		{
			Type = type;
		}
	}
}
