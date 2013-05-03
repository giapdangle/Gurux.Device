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
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.Drawing.Design;

namespace Gurux.Device.Editor
{
	/// <summary>
	/// This enum is used to show None in value Zero in property grid. 
	/// </summary>
	public enum GXNumberEnum
	{
		/// <summary>
		/// None.
		/// </summary>
		None = 0
	}	

	/// <summary>
	/// Use this converter to use selected names with numbers (Zero = 0).
	/// </summary>	
	public class GXNumberEnumeratorConverter : StringConverter
	{
		/// <summary>
        /// Checks if this converter can convert the object from given type to the type of this converter.
		/// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
        /// <param name="t">The type to convert from.</param>
        /// <returns>True, if the converter can convert the object from given type.</returns>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type t)
		{
			if (t == typeof(string))
			{
				return true;
			}
			return base.CanConvertFrom(context, t);
		}

		/// <summary>
        /// Checks if this converter can convert the object to given type.
		/// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
        /// <param name="destinationType">The type to convert to.</param>
        /// <returns>True, if the converter can convert the object to given type.</returns>
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return base.CanConvertTo(context, destinationType);
		}


		/// <summary>
		/// Checks if the object supports a standard set of values that can be picked from a list. 
		/// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
		/// <returns>True, if the object supports a standard set of values.</returns>
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			GXNumberEnumeratorConverterAttribute att = (GXNumberEnumeratorConverterAttribute)context.PropertyDescriptor.Attributes[typeof(GXNumberEnumeratorConverterAttribute)];
			if (att != null)
			{
				return true;
			}
			return base.GetStandardValuesSupported(context);
		}

		/// <summary>
		/// Retrieves standard values to be shown in a combobox.
		/// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
		/// <returns>Collection of standard values.</returns>
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			System.Collections.ArrayList arr = new System.Collections.ArrayList();
			GXNumberEnumeratorConverterAttribute att = (GXNumberEnumeratorConverterAttribute)context.PropertyDescriptor.Attributes[typeof(GXNumberEnumeratorConverterAttribute)];
			if (att != null)
			{
				Type type = ((GXNumberEnumeratorConverterAttribute)att).Items;
				foreach (object val in Enum.GetValues(type))
				{
					arr.Add(val.ToString());
				}
			}
			return new StandardValuesCollection(arr);
		}

		/// <summary>
		/// Checks if the user can type in a value that is not in the drop-down list.
		/// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
		/// <returns>False.</returns>
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		/// <summary>
        /// Converts given value to the type of this converter.
		/// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
        /// <param name="info">The culture info.</param>
        /// <param name="value">Object to convert.</param>
        /// <returns>Converted type.</returns>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo info, object value)
		{
			try
			{
				if (value is string)
				{
					double tmp = 0;
					bool number = double.TryParse(value.ToString(), System.Globalization.NumberStyles.Number, NumberFormatInfo.InvariantInfo, out tmp);
					if (number)
					{
						Type type = context.PropertyDescriptor.PropertyType;
						return Convert.ChangeType(tmp, type);
					}
					else
					{
						//If value is empty return default value if found.
						if (value == null || ((string)value).Length == 0)
						{
							System.ComponentModel.DefaultValueAttribute att = (System.ComponentModel.DefaultValueAttribute)context.PropertyDescriptor.Attributes[typeof(System.ComponentModel.DefaultValueAttribute)];
							if (att != null)
							{
								return att.Value;
							}
						}
						GXNumberEnumeratorConverterAttribute numAtt = (GXNumberEnumeratorConverterAttribute)context.PropertyDescriptor.Attributes[typeof(GXNumberEnumeratorConverterAttribute)];
						if (numAtt != null)
						{
							object tmp1 = Enum.Parse(((GXNumberEnumeratorConverterAttribute)numAtt).Items, value.ToString(), true);
							return tmp1;
							//return Enum.Parse(((GXNumberEnumeratorConverterAttribute) numAtt).Items, value.ToString(), true);
						}
					}
				}
				return base.ConvertFrom(context, info, value);
			}
			catch (Exception Ex)
			{
				MessageBox.Show(Ex.Message);
			}
			return null;
		}

		/// <summary>
        /// Converts given object to given type.
		/// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
        /// <param name="culture">Culture info.</param>
        /// <param name="value">The object to convert.</param>
        /// <param name="destType">The type to convert to.</param>
        /// <returns>Converted type of the object.</returns>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
		{
			try
			{
				if (destType == typeof(string) && context != null)
				{
					//Check if the value is an enum, a text or an integer.
					double v = 0;
					bool number = double.TryParse(value.ToString(), System.Globalization.NumberStyles.Number, NumberFormatInfo.InvariantInfo, out v);

					GXNumberEnumeratorConverterAttribute att = (GXNumberEnumeratorConverterAttribute)context.PropertyDescriptor.Attributes[typeof(GXNumberEnumeratorConverterAttribute)];
					if (att != null)
					{
						string tmp = null;
						if (!number)//If enum value.
						{
							tmp = Enum.Parse(((GXNumberEnumeratorConverterAttribute)att).Items, value.ToString(), true).ToString();
						}
						else
						{
							//Convert int to enum value if any.
							tmp = Enum.GetName(((GXNumberEnumeratorConverterAttribute)att).Items, Convert.ChangeType(v, context.PropertyDescriptor.PropertyType));
							//If value is not enum value.
							if (tmp == null)
							{
								tmp = value.ToString();
							}
						}
						return tmp;
					}
				}
				return base.ConvertTo(context, culture, 0, destType);
			}
			catch (Exception Ex)
			{
				MessageBox.Show(Ex.Message);
			}
			return null;
		}
	}
}
