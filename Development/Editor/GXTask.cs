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
using System.Xml;
using System.Windows.Forms;

namespace Gurux.Device.Editor
{
	/// <summary>
    /// GXTask represents an item of a task. 
    /// Tasks are to fix, in order to make the device work properly.
	/// </summary>
	public class GXTask
	{
		/// <summary>
        /// Source of the task item.
		/// </summary>
		public object Source = null;

		/// <summary>
        /// The property that causes the task.
		/// </summary>
		public string FailedProperty = null;

		/// <summary>
		/// Retrieves or sets a string that describes the task item.
		/// </summary>
		public string Description = null;

		/// <summary>
        /// Determines if some of the properties have changed.
		/// </summary>
		public bool Dirty = true;

		/// <summary>
		/// Initializes a new instance of the GXTask class.
		/// </summary>
		public GXTask()
		{

		}

		/// <summary>
		/// Parent collection.
		/// </summary>
        public GXTaskCollection Parent
        {
            get;
            internal set;
        }

		/// <summary>
		/// Initializes a new instance of the GXTask class.
		/// </summary>
		/// <param name="source">If the task is added by user, source of the task is null.</param>
        /// <param name="prop">The property that causes the task.</param>
		/// <param name="description">Description for the new task.</param>
		public GXTask(object source, string prop, string description)
		{
			Source = source;
			FailedProperty = prop;
			Description = description;
		}        
	}
}
