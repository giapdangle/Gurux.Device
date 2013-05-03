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

namespace Gurux.Device.Editor
{
    /// <summary>
    /// This attribute is used to convert numeric value to constant string value in property grid.
    /// </summary>
    /// <remarks>
    /// For example; integer value Zero is show as GXNumberEnum value None.
    /// </remarks>
    public class GXNumberEnumeratorConverterAttribute : Attribute
    {
		/// <summary>
		/// The type of the enum describing the number to string conversions.
		/// </summary>
        public Type Items = null;

        /// <summary>
        /// Initializes a new instance of the GXNumberEnumeratorConverterAttribute class.
        /// </summary>
        /// <param name="val">The type of the new instance.</param>
        public GXNumberEnumeratorConverterAttribute(Type val)
        {
            Items = val;
        }
    }
}
