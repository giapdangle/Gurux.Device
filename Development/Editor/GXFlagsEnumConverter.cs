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
using System.Reflection;
using System.Windows.Forms;
using Gurux.Device.Properties;

namespace Gurux.Device.Editor
{
	/// <summary>
	/// Flags enumeration type converter.
	/// </summary>
	public class GXFlagsEnumConverter : EnumConverter
	{
		/// <summary>
		/// This class represents an enumeration field in the property grid.
		/// </summary>
		protected class EnumFieldDescriptor : SimplePropertyDescriptor
		{
			#region Fields
			/// <summary>
			/// Stores the context which the enumeration field descriptor was created in.
			/// </summary>
			ITypeDescriptorContext m_Context;
			#endregion

			#region Methods
			/// <summary>
			/// Creates an instance of the enumeration field descriptor class.
			/// </summary>
			/// <param name="componentType">The type of the enumeration.</param>
			/// <param name="name">The name of the enumeration field.</param>
			/// <param name="context">The current context.</param>
			public EnumFieldDescriptor(Type componentType, string name, ITypeDescriptorContext context)
				: base(componentType, name, typeof(bool))
			{
				m_Context = context;
			}

			/// <summary>
			/// Retrieves the value of the enumeration field.
			/// </summary>
			/// <param name="component">
			/// The instance of the enumeration type which to retrieve the field value for.
			/// </param>
			/// <returns>
			/// True if the enumeration field is included to the enumeration; 
			/// otherwise, False.
			/// </returns>
			public override object GetValue(object component)
			{
				return ((int)component & (int)Enum.Parse(ComponentType, Name)) != 0;
			}

			/// <summary>
			/// Sets the value of the enumeration field.
			/// </summary>
			/// <param name="component">
			/// The instance of the enumeration type which to set the field value to.
			/// </param>
			/// <param name="value">
			/// True if the enumeration field should included to the enumeration; 
			/// otherwise, False.
			/// </param>
			public override void SetValue(object component, object value)
			{
				bool myValue = (bool)value;
				int myNewValue;
				if (myValue)
				{
					myNewValue = ((int)component) | (int)Enum.Parse(ComponentType, Name);
				}
				else
				{
					myNewValue = ((int)component) & ~(int)Enum.Parse(ComponentType, Name);
				}


				FieldInfo myField = component.GetType().GetField("value__", BindingFlags.Instance | BindingFlags.Public);
				myField.SetValue(component, myNewValue);
				m_Context.PropertyDescriptor.SetValue(m_Context.Instance, component);
			}

			/// <summary>
			/// Retrieves a value indicating whether the enumeration 
			/// field is set to a non-default value.
			/// </summary>
			public override bool ShouldSerializeValue(object component)
			{
				return (bool)GetValue(component) != GetDefaultValue();
			}

			/// <summary>
			/// Resets the enumeration field to its default value.
			/// </summary>
			public override void ResetValue(object component)
			{
				SetValue(component, GetDefaultValue());
			}

			/// <summary>
			/// Retrieves a value indicating whether the enumeration 
			/// field can be reset to the default value.
			/// </summary>
			public override bool CanResetValue(object component)
			{
				return ShouldSerializeValue(component);
			}

			/// <summary>
			/// Retrieves the enumerations field�s default value.
			/// </summary>
			private bool GetDefaultValue()
			{
				object myDefaultValue = null;
				string myPropertyName = m_Context.PropertyDescriptor.Name;
				Type myComponentType = m_Context.PropertyDescriptor.ComponentType;

				// Get DefaultValueAttribute
				DefaultValueAttribute myDefaultValueAttribute = (DefaultValueAttribute)Attribute.GetCustomAttribute(
					myComponentType.GetProperty(myPropertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic),
					typeof(DefaultValueAttribute));
				if (myDefaultValueAttribute != null)
				{
					myDefaultValue = myDefaultValueAttribute.Value;
				}

				if (myDefaultValue != null)
				{
					return ((int)myDefaultValue & (int)Enum.Parse(ComponentType, Name)) != 0;
				}
				return false;
			}
			#endregion

			#region Properties

			/// <summary>
            /// A collection of attributes.
			/// </summary>
			public override AttributeCollection Attributes
			{
				get
				{
					return new AttributeCollection(new Attribute[] { RefreshPropertiesAttribute.Repaint });
				}
			}
			#endregion
		}

		#region Methods
		/// <summary>
		/// Creates an instance of the FlagsEnumConverter class.
		/// </summary>
		/// <param name="type">The type of the enumeration.</param>
		public GXFlagsEnumConverter(Type type)
			: base(type)
		{
		}

		/// <summary>
		/// Retrieves the property descriptors for the enumeration fields. 
		/// These property descriptors will be used by the property grid 
		/// to show separate enumeration fields.
		/// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
        /// <param name="value">An Object that specifies the type of array for which to get properties.</param>
        /// <param name="attributes">An array of type Attribute that is used as a filter.</param>
        /// <returns>Collection of properties exposed to this data type.</returns>
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			return GetProperties2(context, value, attributes, false);
		}

		PropertyDescriptorCollection GetProperties2(ITypeDescriptorContext context, object value, Attribute[] attributes, bool all)
		{
			if (context != null)
			{
				Type myType = value.GetType();
				string[] myNames = Enum.GetNames(myType);
				Array myValues = Enum.GetValues(myType);
				if (myNames != null)
				{
					PropertyDescriptorCollection myCollection = new PropertyDescriptorCollection(null);
					for (int i = 0; i < myNames.Length; i++)
					{
						if (all || ((int)myValues.GetValue(i) != 0 && myNames[i] != Resources.All))
							myCollection.Add(new EnumFieldDescriptor(myType, myNames[i], context));
					}
					return myCollection;
				}
			}
			return base.GetProperties(context, value, attributes);
		}

        /// <summary>
        /// Allows displaying the + symbol in the property grid.
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
        /// <returns>True to find properties of this object.</returns>
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			if (context != null)
			{
				return true;
			}
			return base.GetPropertiesSupported(context);
		}

		/// <summary>
		/// Returns whether this object supports a standard set of values that can be picked from a list.
		/// </summary>
		/// <returns>False</returns>
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return false;
		}

		/// <summary>
		/// Converts given value object to the specified destination type.
		/// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
        /// <param name="culture">Culture info.</param>
        /// <param name="destinationType">Destination type.</param>
        /// <param name="value">Value to convert.</param>
		/// <returns>Converted value.</returns>
		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				string str = "";
				bool all = true;
				EnumFieldDescriptor allIndex = null, noneIndex = null;
				PropertyDescriptorCollection props = GetProperties2(context, value, null, true);
				foreach (EnumFieldDescriptor it in props)
				{
					if (((int)Enum.Parse(it.ComponentType, it.Name)) == 0)
					{
						noneIndex = it;
						continue;
					}
					if (it.Name == "All")
					{
						allIndex = it;
						continue;
					}
					if (((int)value == (int)Enum.Parse(it.ComponentType, it.Name)))
					{
						str = it.DisplayName;
						break;
					}
					object val = it.GetValue(value);
					if (val is bool && ((bool)val) == true)
					{
						str += it.DisplayName + " ,";
					}
					else
					{
						all = false;
					}
				}
				if (all && allIndex != null)
				{
					return allIndex.DisplayName;
				}
				if (str == "" && noneIndex != null)
				{
					return noneIndex.DisplayName;
				}
				int index;
				if ((index = str.LastIndexOf(',')) != -1)
				{
					str = str.Remove(index - 1);
				}
				return str;
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
		#endregion
	}
}
