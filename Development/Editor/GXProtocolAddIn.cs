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
using System.Windows.Forms;
using System.Collections;
using Gurux.Device;
using System.ComponentModel;
using Gurux.Communication;
using System.Collections.Generic;

namespace Gurux.Device.Editor
{   
	/// <summary>
	/// GXProtocolAddIn is base class for protocol addins.
	/// A protocol addin is an extension to GXDeviceEditor that implements protocol specific features.
	/// Protocol specific features include communication messages, custom wizard pages for configuration and 
	/// importing settings from a file of a physical device.
	/// </summary>
    [Serializable]
    public abstract class GXProtocolAddIn
	{		
		/// <summary>
		/// Defines item possibilities for a protocol addin.
		/// </summary>
		[Flags]
		public enum VisibilityItems
		{
			/// <summary>
			/// No items
			/// </summary>
			None = 0x0,
			/// <summary>
			/// GXCategories
			/// </summary>
			Categories = 0x1,
			/// <summary>
			/// Tables
			/// </summary>
			Tables = 0x4,
			/// <summary>
			/// GXDevice
			/// </summary>
			Device = 0x8,
			/// <summary>
			/// GXProperties
			/// </summary>
			Properties = 0x10
		}

		/// <summary>
		/// Defines functionality possibilities for a protocol addin.
		/// </summary>
		[Flags]
		public enum Functionalities
		{
			/// <summary>
			/// None
			/// </summary>
			None = 0x0,
			/// <summary>
			/// Add
			/// </summary>
			Add = 0x01,
			/// <summary>
			/// Edit
			/// </summary>
			Edit = 0x02,
			/// <summary>
			/// Remove
			/// </summary>
			Remove = 0x4
		}

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the GXProtocolAddIn class.
		/// </summary>
		/// <param name="name">Name of the protocol</param>
		/// <param name="importFromFile">Enable importing from a file</param>
		/// <param name="importFromDevice">Enable importing from a device</param>
		/// <param name="export">Enable exporting GXDevice to a file</param>
		public GXProtocolAddIn(string name, bool importFromFile, bool importFromDevice, bool export)
		{
            WizardAvailable = VisibilityItems.None;
			Name = name;
			ImportFromFileEnabled = importFromFile;
			ImportFromDeviceEnabled = importFromDevice;			
		}        
        
		#endregion

		#region Public Properties

		/// <summary>
		/// Returns available functionalities for the protocol.
		/// </summary>
		public virtual VisibilityItems ItemVisibility
		{
			get
			{
				return VisibilityItems.Categories | VisibilityItems.Tables;
			}
		}

		/// <summary>
		/// Returns available functionalities for the target.
		/// </summary>
		public virtual Functionalities GetFunctionalities(object target)
		{
			return Functionalities.Add | Functionalities.Edit | Functionalities.Remove;
		}

		/// <summary>
		/// Get Type of the overloaded GXDevice
		/// </summary>
        public abstract Type GetDeviceType();

		/// <summary>
		/// Get Types of the overloaded GXProperties
		/// </summary>
		public abstract Type[] GetPropertyTypes(object parent);

		/// <summary>
		/// Get Types of the overloaded GXCategories
		/// </summary>
		public virtual Type[] GetCategoryTypes(object parent)
        {
            return new Type[]{typeof(GXCategory)};
        }

		/// <summary>
		/// Get Types of the overloaded GXTables
		/// </summary>
		public virtual Type[] GetTableTypes(object parent)
        {
            return new Type[]{typeof(GXTable)};
        }

		/// <summary>
		/// Get extra Types for serialization.
		/// </summary>
		public virtual Type[] GetExtraTypes(object parent)
        {
            return null;
        }

        /// <summary>
		/// Get custom UI.
		/// </summary>
		public virtual Form GetCustomUI(object target)
        {
            return null;
        }

        /// <summary>
        /// Initialize protocol AddIn in import.
        /// </summary>
        public virtual void InitializeAfterImport(GXDevice device)
        {

        }

		/// <summary>
		/// 
		/// </summary>
		public virtual string[] GetAdditionalExportItems()
		{
			return null;
		}

		/// <summary>
		/// Gets or sets the name of the protocol.
		/// </summary>
		public string Name
		{
			get;
			set;
		}
		
		/// <summary>
        /// Determines what functionalities are provided with wizard user interface.
        /// </summary>
        public VisibilityItems WizardAvailable
		{
            get;
            set;
        }

        /// <summary>
        /// Return true if wizard is available.
        /// </summary>
        /// <param name="target">The item queried for wizard functionality.</param>
        /// <returns>True if wizard is available, otherwise false.</returns>
        public bool IsWizardAvailable(object target)
        {
            if (target is GXDevice)
            {               
                return (WizardAvailable & VisibilityItems.Device) != 0;
            }
            if (target is GXCategory)
            {               
                return (WizardAvailable & VisibilityItems.Categories) != 0;
            }
            if (target is GXTable)
            {               
                return (WizardAvailable & VisibilityItems.Tables) != 0;
            }
            if (target is GXProperty)
            {               
                return (WizardAvailable & VisibilityItems.Properties) != 0;
            }
            return false;
        }

        /// <summary>
        /// Determines if data importing from file is possible.
        /// </summary>
		public bool ImportFromFileEnabled
		{
            get;
            private set;
        }

        /// <summary>
		/// Gets or sets if importing of data is possible from a physical device.
        /// </summary>
        public bool ImportFromDeviceEnabled
		{
            get;
            private set;
        }

        /// <summary>
		/// Gets or sets the filter for file types.
        /// </summary>
		public string FileTypesFilter
		{
            get;
            set;
        }

		#endregion

		#region Public Methods

		/// <summary>
		/// Creates a new instance of all protocol addin GXCategory Types.
		/// </summary>
        public GXCategory[] GetCategories(object parent)
        {
			Type[] types = this.GetCategoryTypes(parent);
            GXCategory[] categories = new GXCategory[types.Length];
            for (int pos = 0; pos != types.Length; ++pos)
            {
                categories[pos] = Activator.CreateInstance(types[pos]) as GXCategory;
            }
            return categories;
        }

		/// <summary>
		/// Creates a new instance of all protocol addin GXTable Types.
		/// </summary>
        public GXTable[] GetTables(object parent)
        {
			Type[] types = this.GetTableTypes(parent);
            GXTable[] tables = new GXTable[types.Length];
            for (int pos = 0; pos != types.Length; ++pos)
            {
                tables[pos] = Activator.CreateInstance(types[pos]) as GXTable;
            }
            return tables;
        }

		/// <summary>
		/// Creates a new instance of all protocol addin GXProperty Types.
		/// </summary>
        public GXProperty[] GetProperties(object parent)
        {
			Type[] types = this.GetPropertyTypes(parent);
            GXProperty[] properties = new GXProperty[types.Length];
            for (int pos = 0; pos != types.Length; ++pos)
            {
                properties[pos] = Activator.CreateInstance(types[pos]) as GXProperty;
            }
            return properties;
        }


        /// <summary>
        /// Notify progress.
        /// </summary>
        protected void Progress(int value, int maximum)
        {
            if (OnProgress != null)
            {
                OnProgress(value, maximum);
            }
        }

		/// <summary>
		/// GXProtocolAddin component sends progress notifications using this method.
		/// </summary>
		public delegate void ProgressEventHandler(int value, int maximum);
		/// <summary>
		/// GXProtocolAddin component sends trace notifications using this method.
		/// </summary>
		public delegate void TraceEventHandler(string value);
		/// <summary>
		/// GXProtocolAddin component sends progress notifications using this method.
		/// </summary>
        public event ProgressEventHandler OnProgress;
		/// <summary>
		/// GXProtocolAddin component sends trace notifications using this method.
		/// </summary>
		public event TraceEventHandler OnTrace;


        /// <summary>
        /// Write trace.
        /// </summary>
        /// <param name="value"></param>
        protected void Trace(string value)
        {
            if (OnTrace != null)
            {
                OnTrace(value);
            }
        }

        /// <summary>
        /// Write trace line.
        /// </summary>
        /// <param name="value"></param>
        protected void TraceLine(string value)
        {
            if (OnTrace != null)
            {
                OnTrace(value + Environment.NewLine);
            }
        }


		/// <summary>
		/// Imports properties from the device.
		/// </summary>
		/// <param name="addinPages">Custom pages created by the protocol addin.</param>
		/// <param name="device">The source GXDevice.</param>
		/// <param name="media">A media connection to the device.</param>
		/// <returns>True if everything went fine, otherwise false.</returns>
        public virtual void ImportFromDevice(Control[] addinPages, GXDevice device, Gurux.Common.IGXMedia media)
		{
			throw new NotImplementedException("ImportFromDevice");
		}

		/// <summary>
		/// Imports properties from the file.
		/// </summary>
		/// <param name="device">A GXDevice that is the target of import.</param>
		/// <param name="fileNames">An array of file names.</param>
		/// <returns>True if everything went fine, otherwise false.</returns>
		public virtual bool ImportFromFile(GXDevice device, List<string> fileNames)
		{
            throw new NotImplementedException("ImportFromFile");
		}

        /// <summary>
        /// Export settings.
        /// </summary>
        /// <param name="device">A GXDevice that is the source of export.</param>
		/// <param name="trace">A TextBox to trace the process.</param>
		/// <returns>True if everything went fine, otherwise false.</returns>
		public virtual bool Export(GXDevice device, TextBox trace)
		{
            throw new NotImplementedException("Export");
		}
		      

		/// <summary>
		/// Modifies the wizard by adding protocol specific pages.
		/// </summary>
        /// <param name="source">The associated object.</param>
		/// <param name="type">The Type of the property page</param>
        /// <param name="pages">The collection of tab pages.</param>
        public virtual void ModifyWizardPages(object source, GXPropertyPageType type, List<Control> pages)
		{        
            
		}      
		#endregion
	}
}
