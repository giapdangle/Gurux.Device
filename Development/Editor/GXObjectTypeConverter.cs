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
using System.Reflection;

namespace Gurux.Device.Editor
{
	/// <summary>
	/// TypeConverter that is used with GXObjects such as GXDevice and GXCategory.
	/// </summary>
    public class GXObjectTypeConverter : TypeConverter
    {
        static public bool DesignMode
        {
            get;
            set;
        }

        /// <summary>
        /// Allows us to display the + symbol near the property name
        /// </summary>
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

		/// <summary>
		/// Get properties from a given object.
		/// </summary>
        public static PropertyDescriptorCollection GetProperties(object value, bool designMode)
        {
            PropertyDescriptorCollection pds = new PropertyDescriptorCollection(null);
            PropertyDescriptorCollection m_Pdc = TypeDescriptor.GetProperties(value);
            foreach (PropertyDescriptor pd in m_Pdc)
            {
                if (!pd.IsBrowsable)
                {
                    continue;
                }
                //Check user access visibly.
                GXUserLevelAttribute ua = pd.Attributes[typeof(GXUserLevelAttribute)] as GXUserLevelAttribute;
                if (ua != null && ua.Type > GXDesigner.Visibility)
                {
                    continue;
                }
                ValueAccessAttribute att = pd.Attributes[typeof(ValueAccessAttribute)] as ValueAccessAttribute;
                if (att != null)
                {
                    if (designMode)
                    {
                        if (att.Design == ValueAccessType.None)
                        {
                            continue;
                        }                        
                        if (att.Design == ValueAccessType.Show)
                        {
                            bool contains = false;
                            foreach (Attribute it in pd.Attributes)
                            {
                                if (it.GetType() == typeof(ReadOnlyAttribute))
                                {
                                    contains = true;
                                    break;
                                }
                            }
                            if (contains)
                            {                                
                                ReadOnlyAttribute attr = pd.Attributes[typeof(ReadOnlyAttribute)] as ReadOnlyAttribute;								
                                FieldInfo field = attr.GetType().GetField("isReadOnly", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.CreateInstance);
								if (field != null)
								{
                                	field.SetValue(attr, true, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.CreateInstance, null, null);
								}
                            }
                        }
                    }
                    else 
                    {
                        if (att.RunTime == ValueAccessType.None)
                        {
                            continue;
                        }
                        if (att.RunTime == ValueAccessType.Show)
                        {
                            bool contains = false;
                            foreach (Attribute it in pd.Attributes)
                            {
                                if (it.GetType() == typeof(ReadOnlyAttribute))
                                {
                                    contains = true;
                                    break;
                                }
                            }
                            if (contains)
                            {
                                ReadOnlyAttribute attr = pd.Attributes[typeof(ReadOnlyAttribute)] as ReadOnlyAttribute;
                                FieldInfo field = attr.GetType().GetField("isReadOnly", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.CreateInstance);
                                if (field != null)
                                {
                                    field.SetValue(attr, true, BindingFlags.NonPublic | BindingFlags.Instance, null, null);
                                }
                            }
                        }
                    }                    
                }				
                pds.Add(pd);                
            }
            return pds;
        }

        /// <summary>
        /// Hide base properties.
        /// </summary>
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            bool designMode;
            if (System.Environment.OSVersion.Platform == PlatformID.Unix)
            {
                designMode = DesignMode || context.GetService(typeof(System.ComponentModel.Design.IDesignerHost)) != null;
            }
            else
            {
                designMode = DesignMode || context.Container is System.ComponentModel.Design.IDesignerHost;
            }			
			return GetProperties(value, designMode);
        }
    }
}
