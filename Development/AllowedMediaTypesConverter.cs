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
using System.Linq;
using System.Text;
using Gurux.Device.Editor;
using System.ComponentModel;
using Gurux.Device.Properties;

namespace Gurux.Device
{
    /// <summary>
    /// This is a special type converter which will be associated with the MediaTypeCollection class.
    /// It converts an MediaType object to a string representation for use in a property grid.
    /// </summary>
    internal class AllowedMediaTypesConverter : ExpandableObjectConverter
    {
        /// <summary>
        /// Allows displaying the + symbol in the property grid.
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
        /// <returns>True to find properties of this object.</returns>
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return GXDesigner.Visibility == UserLevelType.Experienced;
        }

        /// <summary>
        /// Converts given value object to the specified destination type.
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
        /// <param name="culture">Culture info.</param>
        /// <param name="value">Value to convert.</param>
        /// <param name="destType">Destination type.</param>
        /// <returns>Converted value.</returns>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType)
        {
            if (destType == typeof(string) && value is GXMediaTypeCollection)
            {
                string data = "";
                if (((GXMediaTypeCollection)value).Count == 0)
                {
                    return Resources.AllMedias;
                }
                foreach (GXMediaType type in ((GXMediaTypeCollection)value))
                {
                    data += type.Name + ", ";
                }
                if (data.Length > 1)
                {
                    data = data.Remove(data.Length - 2, 2);
                }
                return data;
            }
            return base.ConvertTo(context, culture, value, destType);
        }

        /// <summary>
        /// Loops through all device types and adds them to the property list.
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
        /// <param name="value">An Object that specifies the type of array for which to get properties.</param>
        /// <param name="attributes">An array of type Attribute that is used as a filter.</param>
        /// <returns>Collection of properties exposed to this data type.</returns>
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            GXMediaTypeCollection list = (GXMediaTypeCollection)value;
            PropertyDescriptorCollection pds = new PropertyDescriptorCollection(null);
            for (int pos = 0; pos < list.Count; ++pos)
            {
                PropertyDescriptor pd = new MediaTypeCollectionPropertyDescriptor(list, pos);
                pds.Add(pd);
            }
            return pds;
        }
    }
}
