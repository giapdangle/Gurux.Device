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
using System.Collections;
using System.ComponentModel;

namespace Gurux.Device.Editor
{

	/// <summary>
	/// Is the property shown only in the design mode (with the Device Editor) or only in the run time mode.
	/// </summary>
	public class GXDesigner : System.Windows.Forms.Design.ControlDesigner
	{
		/// <summary>
		/// User Level affecting displayed items.
		/// </summary>
		static public UserLevelType Visibility = UserLevelType.Beginner;

		/// <summary>
		/// If the designer is serializing at the moment.
		/// </summary>
		static public bool Serializing = false;

		/// <summary>
        /// Removes unnecessary designtime properties and check user visibility level.
		/// </summary>
        /// <param name="properties">Collection of component properties.</param>
		static public void RemoveDesignTimeProperties(IDictionary properties)
		{
			//If serializing don't remove any items.
			if (Serializing)
			{
				return;
			}
			//Copy data to temp array so foreach can be used.
			System.Collections.Hashtable arr = new Hashtable(properties);
			foreach (DictionaryEntry elem in arr)
			{
				PropertyDescriptor it = (PropertyDescriptor)elem.Value;
				//Check user's visibility rights
				GXUserLevelAttribute vAtt = (GXUserLevelAttribute)it.Attributes[typeof(GXUserLevelAttribute)];
				if (vAtt != null)
				{
					//If user don't have rights to see this property.
					if (Visibility < vAtt.Type)
					{
						properties.Remove(elem.Key);
						continue;
					}
				}
				GXSettingItemAttribute att = (GXSettingItemAttribute)it.Attributes[typeof(GXSettingItemAttribute)];
				if (it.Name == "Modifiers" ||
					it.Name == "CausesValidation" ||
					it.Name == "AccessibleDescription" ||
					it.Name == "AccessibleName" ||
					it.Name == "AccessibleRole" ||
					it.Name == "DataBindings" ||
					it.Name == "Tag" ||
					it.Name == "AllowDrop" ||
					it.Name == "ContextMenu" ||
					it.Name == "DialogResult" ||
					it.Name == "DrawMode" ||
					it.Name == "DockPadding" ||
					it.Name == "ContextMenuStrip" ||
					it.PropertyType == typeof(System.Windows.Forms.ImeMode) ||
					att != null && (att.Visible & GXSettingItemAttribute.VisibleType.DesignTime) == 0)
				{
					properties.Remove(elem.Key);
				}
			}
		}

		/// <summary>
		/// Removes properties that are visible only at runtime, not in design time.
		/// </summary>
		/// <param name="properties">Collection of component properties.</param>
		protected override void PreFilterProperties(IDictionary properties)
		{
			RemoveDesignTimeProperties(properties);
			base.PreFilterProperties(properties);
		}
	}
}
