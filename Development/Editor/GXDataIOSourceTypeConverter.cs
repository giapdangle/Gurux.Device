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
using System.ComponentModel;

namespace Gurux.Device.Editor
{
    /// <summary>
    /// GXDataIOSourceBaseTypeConverter is used to show text types in property grid.
    /// </summary>
    internal class GXDataIOSourceTypeConverter : TypeConverter
    {

        /// <summary>
        /// Allows us to display the + symbol near the property name
        /// </summary>
        /// <param name="context">An System.ComponentModel.ITypeDescriptorContext that provides a format context.</param>
        /// <returns>true if System.ComponentModel.TypeConverter.GetProperties(System.Object) should be called to find the properties of this object; otherwise, false.</returns>
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance is IGXComponent)
            {
                GXDataIOSource source = (GXDataIOSource)((IGXComponent)context.Instance).DataIOSource;
                if (source.Target != null)
                {
					return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add text properties to the collection.
        /// </summary>
        /// <param name="context">An System.ComponentModel.ITypeDescriptorContext that provides a format context.</param>
        /// <param name="value">An System.Object that specifies the type of array for which to get properties.</param>
        /// <param name="attributes">An array of type System.Attribute that is used as a filter.</param>
        /// <returns>true if System.ComponentModel.TypeConverter.GetProperties(System.Object) should be called to find the properties of this object; otherwise, false.</returns>
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            PropertyDescriptorCollection pds = new PropertyDescriptorCollection(null);
            GXDataIOSource source = (GXDataIOSource)value;
            if (source == null || source.Target == null)
            {
                return pds;
            }
            PropertyDescriptorCollection m_Pdc = TypeDescriptor.GetProperties(source);
            foreach (PropertyDescriptor pd in m_Pdc)
            {
                if (pd.IsBrowsable)
                {
                    //Show UseUIValue only for property if Action is Value based...
                    if (pd.Name == "UseUIValue")
                    {
                        if (source.Target is GXProperty)
                        {
                            GXProperty.AvailableTargets action = (GXProperty.AvailableTargets)source.Action;
                            if (!(action == GXProperty.AvailableTargets.Value || action == GXProperty.AvailableTargets.AvarageValue || action == GXProperty.AvailableTargets.MinimumValue || action == GXProperty.AvailableTargets.MaximumValue))
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    pds.Add(new GXDataIOSourceDescriptor(pd));
                }
            }
            return pds;
        }
    }
}
