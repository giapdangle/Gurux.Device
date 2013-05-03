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
using System.Text;

namespace Gurux.Device.Editor
{
	/// <summary>
    /// With GXSettingItemAttribute class is determined, if the property of the object is visible on runtime,
    /// on design time or on both.
	/// </summary>
	public class GXSettingItemAttribute : Attribute
	{
		/// <summary>
        /// Enumerates, when the property is visible.
		/// </summary>
		public enum VisibleType
		{
			/// <summary>
            /// The property is always visible.
			/// </summary>
			All = 3,
			/// <summary>
			/// The property is not visible.
			/// </summary>
			Hidden = 0,
			/// <summary>
            /// The property is visible runtime only.
			/// </summary>
			RunTime = 1,
			/// <summary>
            /// The property is visible design time only.
			/// </summary>
			DesignTime = 2
		}

		bool m_Storable = false;
		VisibleType m_Visible = VisibleType.All;
		string m_Category = string.Empty;
		private Type m_Editor = null;

		/// <summary>
		/// Retrieves or sets the type of the editor.
		/// </summary>
		public Type Editor
		{
			get
			{
				return m_Editor;
			}
			set
			{
				m_Editor = value;
			}
		}

		/// <summary>
		/// Retrieves the category to which the object belongs.
		/// </summary>
        /// <returns>The category to which the object belongs.</returns>
		public string Category
		{
			get
			{
				return m_Category;
			}
		}

		/// <summary>
        /// Retrieves VisibleType of the object. VisibleType indicates, when the object is visible.
		/// </summary>
        /// <seealso cref="VisibleType">VisibleType</seealso>
		public VisibleType Visible
		{
			get
			{
				return m_Visible;
			}
		}

		/// <summary>
		/// Determines if the object is storable.
		/// </summary>
        /// <returns>True, if the object is storable.</returns>
		public bool Storable
		{
			get
			{
				return m_Storable;
			}
		}

		/// <summary>
		/// Initializes a new instance of the GXSettingItemAttribute class. 
		/// </summary>
		/// <param name="visible">Visibility type of the new instance.</param>
		/// <param name="storable">If True, the new instance is storable.</param>
        /// <seealso cref="VisibleType">VisibleType</seealso>
        /// <seealso cref="Storable">Storable</seealso>
		public GXSettingItemAttribute(VisibleType visible, bool storable)
		{
			m_Visible = visible;
			m_Storable = storable;
		}

		/// <summary>
		/// Initializes a new instance of the GXSettingItemAttribute class.
		/// </summary>
        /// <param name="visible">Visibility type of the new instance.</param>
        /// <param name="storable">If True, the new instance is storable.</param>
		/// <param name="category">Name of the category to which the new instance belongs.</param>
        /// <param name="editorType">Type of the editor of the new instance.</param>
        /// <seealso cref="VisibleType">VisibleType</seealso>
        /// <seealso cref="Storable">Storable</seealso>
		public GXSettingItemAttribute(VisibleType visible, bool storable, string category, Type editorType)
		{
			m_Storable = storable;
			m_Visible = visible;
			m_Category = category;
			m_Editor = editorType;
		}

	}
}
