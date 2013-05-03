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
using Gurux.Device;
using System.Windows.Forms;

namespace Gurux.Device
{
	/// <summary>
	/// Provides an abstract class for finding physical devices. 
	/// Requires protocol specific implementation.
	/// </summary>
	public abstract class GXProtocolSearchAddIn
	{
		private string m_Name;

		/// <summary>
		/// Initializes a new instance of the GXProtocolSearchAddIn class.
		/// </summary>
		/// <param name="name">The name of the addin.</param>
		public GXProtocolSearchAddIn(string name)
		{
			m_Name = name;
		}

		/// <summary>
		/// Search available devices.
		/// </summary>
		/// <returns>True if the the search was success.</returns>
		public abstract bool FindDevices(GXDeviceGroup deviceGroup);
		/// <summary>
		/// Return a wizard form.
		/// </summary>
		public abstract Form GetWizardForm();
		/// <summary>
		/// Return if it is ok to continue.
		/// </summary>
		public abstract bool OkToContinue();
		/// <summary>
		/// Set progress bar for the protocol search addin to use.
		/// </summary>
		public abstract void SetProgressBar(System.Windows.Forms.ProgressBar progressBar);

		/// <summary>
		/// Gets or sets the name of the GXProtocolSearchAddIn object.
		/// </summary>
		public string Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				m_Name = value;
			}
		}
	}
}
