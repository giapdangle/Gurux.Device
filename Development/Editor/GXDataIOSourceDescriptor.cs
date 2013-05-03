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
using System.Windows.Forms;
using Gurux.Device.Editor;

namespace Gurux.Device.Editor
{
    /// <summary>
	/// Displays properties of a DataIOSource in the property grid.
	/// </summary>
	internal class GXDataIOSourceDescriptor : PropertyDescriptor
	{
		PropertyDescriptor m_PropDescriptor;

		public GXDataIOSourceDescriptor(PropertyDescriptor propDescriptor)
			: base(propDescriptor.Name, null)
		{
			m_PropDescriptor = propDescriptor;
		}

		public override AttributeCollection Attributes
		{
			get
			{
				return m_PropDescriptor.Attributes;
			}
		}

		public override object GetValue(object component)
		{
            return m_PropDescriptor.GetValue(component);
		}

		public override bool CanResetValue(object component)
		{
			return false;
		}

		public override void ResetValue(object component)
		{
		}

		public override Type ComponentType
		{
			get
			{
				return m_PropDescriptor.PropertyType;
			}
		}

		public override bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public override Type PropertyType
		{
			get
			{
				return m_PropDescriptor.PropertyType;
			}
		}

		public void NotifyChange(System.ComponentModel.ISite site, MemberDescriptor md, object oldValue, object newValue)
		{
			//If value has not change.
			if (oldValue == newValue || site == null)
			{
				return;
			}

			System.ComponentModel.Design.IDesignerHost dhDesigner = (System.ComponentModel.Design.IDesignerHost)site.GetService(typeof(System.ComponentModel.Design.IDesignerHost));
			System.ComponentModel.Design.IComponentChangeService ccsChanger = (System.ComponentModel.Design.IComponentChangeService)dhDesigner.GetService(typeof(System.ComponentModel.Design.IComponentChangeService));
			if (dhDesigner != null && ccsChanger != null)
			{
				ccsChanger.OnComponentChanging(site.Component, md);
				ccsChanger.OnComponentChanged(site.Component, md, oldValue, newValue);
			}
		}

		public override void SetValue(object component, object value)
		{
			//Notify if value has changed.
			object oldVal = m_PropDescriptor.GetValue(component);
			if (((GXDataIOSource)component).Parent is Component)
			{
				NotifyChange(((GXDataIOSource)component).Parent.Site, m_PropDescriptor, oldVal, value);
			}
			else //Control
			{
				NotifyChange(((GXDataIOSource)component).Parent.Site, m_PropDescriptor, oldVal, value);
			}
			m_PropDescriptor.SetValue(component, value);
		}

		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}
	}    
}