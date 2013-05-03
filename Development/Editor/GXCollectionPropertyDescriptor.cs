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

namespace Gurux.Device.Editor
{
	/// <summary>
	/// This helper class is used to show collection in property grid. 
	/// </summary>
	public class GXCollectionPropertyDescriptor : PropertyDescriptor
	{
		/// <summary>
		/// Internal list of properties.
		/// </summary>
		protected System.Collections.IList m_List = null;
		/// <summary>
		/// Index of the current instance.
		/// </summary>
		protected int Index = -1;

		/// <summary>
		/// Initializes a new instance of the GXCollectionPropertyDescriptor class.
		/// </summary>
		/// <param name="idx">Index of the new instance.</param>
        /// <param name="list">Collection list.</param>
		public GXCollectionPropertyDescriptor(int idx, System.Collections.IList list) :
			base("#" + idx.ToString(), null)
		{
			m_List = list;
			this.Index = idx;
		}

		/// <summary>
		/// Can always reset the value.
		/// </summary>
		public override bool CanResetValue(object component)
		{
			return true;
		}

		/// <summary>
		/// Returns the type of the component.
		/// </summary>
		public override Type ComponentType
		{
			get
			{
				return m_List.GetType();
			}
		}

		/// <summary>
		/// Returns value of property of the component in at position Index.
		/// </summary>
		public override object GetValue(object component)
		{
			if (m_List.Count <= Index)
			{
				return string.Empty;
			}
			return m_List[Index];
		}

		/// <summary>
		/// Never true.
		/// </summary>
		public override bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Type of property of the component at position Index.
		/// </summary>
		public override Type PropertyType
		{
			get
			{
				return m_List[Index].GetType();
			}
		}

		/// <summary>
		/// Always true.
		/// </summary>
		public override bool ShouldSerializeValue(object component)
		{
			return true;
		}

		/// <summary>
		/// Not used, just for compiler
		/// </summary>
		public override void ResetValue(object component)
		{
			//Not used, just for compiler
		}

		/// <summary>
		/// Not used, just for compiler
		/// </summary>
		public override void SetValue(object component, object value)
		{
			//Not used, just for compiler
		}
	}
}
