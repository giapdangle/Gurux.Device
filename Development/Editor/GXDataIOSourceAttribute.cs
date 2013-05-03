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

namespace Gurux.Device.Editor
{

	/// <summary>
	/// This attribute is used to tell GXDataIOSource what data sources are available for the component.
	/// Types of data sources are also listed.
	/// </summary>
	public class GXDataIOSourceAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the GXDataIOSourceAttribute class.
		/// </summary>
		/// <param name="unknown">Can the data source be unknown.</param>
		public GXDataIOSourceAttribute(bool unknown)
		{
			DataIOSourceCanBeUnknown = unknown;
		}

		/// <summary>
		/// Initializes a new instance of the GXDataIOSourceAttribute class.
		/// </summary>
		/// <param name="sources">Available data sources.</param>
		public GXDataIOSourceAttribute(int sources)
		{
			SupportedDataSources = (GXDataIOSourceType)sources;
		}

		/// <summary>
		/// Initializes a new instance of the GXDataIOSourceAttribute class.
		/// </summary>
		/// <param name="unknown">Can the data source be unknown. (Not selected).</param>
		/// <param name="supportedDataSources">Available data sources.</param>
		/// <param name="supportedProperties">Used data sources.</param>
		public GXDataIOSourceAttribute(bool unknown, GXDataIOSourceType supportedDataSources, object supportedProperties)
		{
			DataIOSourceCanBeUnknown = unknown;
			this.SupportedDataSources = supportedDataSources;
			this.SupportedProperties = supportedProperties;
		}

		/// <summary>
		/// Initializes a new instance of the GXDataIOSourceAttribute class.
		/// </summary>
        /// <param name="unknown">Can the data source be unknown. (Not selected).</param>
		/// <param name="supportedDataSources">Available data sources.</param>
		/// <param name="supportedProperties">Used data sources.</param>
		/// <param name="usedDataSources">DataSources to use.</param>
        /// <param name="usedProperties">Properties to use.</param>
        public GXDataIOSourceAttribute(bool unknown, GXDataIOSourceType supportedDataSources, object supportedProperties, GXDataIOSourceType usedDataSources, int usedProperties)
		{
			DataIOSourceCanBeUnknown = unknown;
			this.SupportedDataSources = supportedDataSources;
			this.UsedDataSources = usedDataSources;
			this.SupportedProperties = supportedProperties;
			this.UsedProperties = usedProperties;
		}

		/// <summary>
		/// Available data sources.
		/// </summary>
		/// <remarks>
        /// By default, no data sources are available.
		/// </remarks>
		public GXDataIOSourceType SupportedDataSources = GXDataIOSourceType.None;
		/// <summary>
		/// Used data sources.
		/// </summary>
		/// <remarks>
		/// By default, all data sources are used.
		/// </remarks>
		public GXDataIOSourceType UsedDataSources = GXDataIOSourceType.All;

		/// <summary>
		/// Properties that the component supports.
		/// </summary>
        public object SupportedProperties;
		/// <summary>
		/// Properties that the component requires.
		/// </summary>
		public int UsedProperties = 0;

		/// <summary>
		/// Determines if the datasource can be unknown. 
		/// </summary>
        /// <remarks>If this is True, no warning is shown.</remarks>
		public bool DataIOSourceCanBeUnknown = false;
	}
}
