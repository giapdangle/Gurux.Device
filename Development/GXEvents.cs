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
using Gurux.Device.Editor;

namespace Gurux.Device
{
	/// <summary>
	/// Contains collection of GXTables ans GXCategories that function as device event listeners.
	/// </summary>
    [Serializable()]
    public class GXEvents
    {
        GXCategoryCollection m_Categories = null;
		GXTableCollection m_Tables = null;

		/// <summary>
		/// Initializes a new instance of the GXNotifies class.
		/// </summary>
        public GXEvents()
		{
			Categories = new GXCategoryCollection();
			Tables = new GXTableCollection();
		}

		/// <summary>
		/// Retrieves notification categories.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
        public GXCategoryCollection Categories
		{
			get
			{
				return m_Categories;
			}
            internal set
            {
                m_Categories = value;
                if (m_Categories != null)
                {
                    m_Categories.Parent = this;
                }
            }
		}

		/// <summary>
        /// Retrieves notification tables.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
        public GXTableCollection Tables
		{
			get
			{
				return m_Tables;
			}
            internal set
            {
                m_Tables = value;
                if (m_Tables != null)
                {
                    m_Tables.Parent = this;
                }
            }
		}

		/// <summary>
		/// Clears notification tables and categories.
		/// </summary>
		public void Clear()
		{
			m_Categories.Clear();
			m_Tables.Clear();
		}

        /// <summary>
        /// Retrieves a string representation of the value of the instance in the GXNotifies class.
        /// </summary>
        /// <returns>A string representation of the value of the instance.</returns>
		public override string ToString()
		{
			return "Notifies";
		}        		

        /// <summary>
        /// Checks if the properties in notifies are valid. 
        /// </summary>
        /// <param name="tasks">Collection of tasks.</param>
		/// <param name="designMode"></param>
        public void Validate(bool designMode, GXTaskCollection tasks)
		{
            m_Categories.Validate(designMode, tasks);
            m_Tables.Validate(designMode, tasks);
		}
    }
}
