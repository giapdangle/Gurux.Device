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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Gurux.Device.Properties;

namespace Gurux.Device.Editor
{
    /// <summary>
    /// This dialog is used to shown DataIOSource targets or table columns.
    /// </summary>
    public partial class GXDataIOSourceDialog : Form
    {
        GXDataIOSourceAttribute SourceAttributes;
		/// <summary>
		/// The target of the dialog.
		/// </summary>
        public object Target;
		/// <summary>
		/// Type of the target of the dialog.
		/// </summary>
        public object TargetType;

		/// <summary>
		/// Constructor. Initializes the dialog and populates the tree.
		/// </summary>
        public GXDataIOSourceDialog(GXDataIOSource dataIOSource, GXDataIOSourceAttribute sourceAttributes, GXDeviceList list)
        {
            InitializeComponent();
			PopulateTree(dataIOSource, sourceAttributes, list);
        }

		/// <summary>
		/// Constructor. Initializes the dialog and populates the tree.
		/// </summary>
		public GXDataIOSourceDialog(GXDataIOSource dataIOSource, GXDataIOSourceAttribute sourceAttributes, GXDevice device)
        {
            InitializeComponent();
			PopulateTree(dataIOSource, sourceAttributes, device);
        }

        /// <summary>
        /// If editor is used to shown table columns.
        /// </summary>
        public GXDataIOSourceDialog(GXProperty target, GXTable table)
        {
            InitializeComponent();
            PopulateTree(target, table);
        }

        private void PopulateTree(GXProperty target, GXTable table)
        {
            TreeNode propNode = new TreeNode(Resources.None);            
            this.DataSourceTree.Nodes.Add(propNode);    
            TreeNode selNode = null;
            foreach (GXProperty prop in table.Columns)
            {
                propNode = new TreeNode(prop.DisplayName, 3, 3);
                propNode.Tag = prop;
                this.DataSourceTree.Nodes.Add(propNode);                
                if (target == prop)
                {
                    selNode = propNode;
                }
            }
            if (selNode != null)
            {
                this.DataSourceTree.SelectedNode = selNode;
            }
            else
            {
                this.DataSourceTree.SelectedNode = this.DataSourceTree.Nodes[0];
            }
        }

		private void PopulateTree(GXDataIOSource dataIOSource, GXDataIOSourceAttribute sourceAttributes, object root)
		{
			GXDevice device = null;
			GXDeviceList devList = null;
			if (root is GXDevice)
			{
				device = root as GXDevice;
			}
			else if (root is GXDeviceList)
			{
				devList = root as GXDeviceList;
			}            

			object target = dataIOSource.Target;
			SourceAttributes = sourceAttributes;
			long UsedDataSources = -1;
			if (SourceAttributes != null)
			{
				UsedDataSources = (long)SourceAttributes.UsedDataSources;
			}
			TreeNode selNode = null;
			if (device != null)
			{
				TreeNode deviceNode;
				CreateDeviceNode(device, target, UsedDataSources, out deviceNode, ref selNode);
				this.DataSourceTree.Nodes.Add(deviceNode);
			}
			else if (devList != null)
			{
				TreeNode listNode = new TreeNode(devList.Name, 0, 0);
				listNode.Tag = devList;
				this.DataSourceTree.Nodes.Add(listNode);

				CreateDeviceGroupNodes(devList.DeviceGroups, listNode, target, ref selNode, UsedDataSources);
			}

			if (selNode != null)
			{
				this.DataSourceTree.SelectedNode = selNode;
			}
			else
			{
				this.DataSourceTree.SelectedNode = this.DataSourceTree.Nodes[0];
			}
		}

		private void CreateDeviceGroupNodes(GXDeviceGroupCollection deviceGroups, TreeNode parentNode, object target, ref TreeNode selNode, long UsedDataSources)
		{
			foreach (GXDeviceGroup devGroup in deviceGroups)
			{
				TreeNode groupNode = new TreeNode(devGroup.Name, 0, 0);
				groupNode.Tag = devGroup;
				if (target == devGroup)
				{
					selNode = groupNode;
				}
				parentNode.Nodes.Add(groupNode);
				if (devGroup.DeviceGroups.Count > 0)
				{
					CreateDeviceGroupNodes(devGroup.DeviceGroups, groupNode, target, ref selNode, UsedDataSources);
				}
				foreach (GXDevice device in devGroup.Devices)
				{
					TreeNode deviceNode;
					CreateDeviceNode(device, target, UsedDataSources, out deviceNode, ref selNode);
					deviceNode.Text = device.Name;
					groupNode.Nodes.Add(deviceNode);
				}
			}
		}

		private static void CreateDeviceNode(GXDevice device, object target, long UsedDataSources, out TreeNode deviceNode, ref TreeNode selNode)
		{
			deviceNode = new TreeNode(GXDeviceList.DeviceProfiles.Find(device.ProfileGuid).Name, 0, 0);
			deviceNode.Tag = device;
			if (target == device)
			{
				selNode = deviceNode;
			}

			if ((UsedDataSources & (long)GXDataIOSourceType.Category) != 0 ||
				(UsedDataSources & (long)GXDataIOSourceType.Property) != 0)
			{
				foreach (GXCategory cat in device.Categories)
				{
					TreeNode catNode = new TreeNode(cat.DisplayName, 1, 1);
					catNode.Tag = cat;
					if (target == cat)
					{
						selNode = catNode;
					}

					if ((UsedDataSources & (long)GXDataIOSourceType.Category) != 0 ||
						cat.Properties.Count != 0)
					{
						deviceNode.Nodes.Add(catNode);
					}
					if ((UsedDataSources & (long)GXDataIOSourceType.Property) != 0)
					{
						foreach (GXProperty prop in cat.Properties)
						{
							TreeNode propNode = new TreeNode(prop.DisplayName, 3, 3);
							propNode.Tag = prop;
							catNode.Nodes.Add(propNode);
							if (target == prop)
							{
								selNode = propNode;
							}
						}
					}
				}
			}
			if ((UsedDataSources & (long)GXDataIOSourceType.Table) != 0)
			{
				foreach (GXTable table in device.Tables)
				{
					TreeNode tableNode = new TreeNode(table.DisplayName, 2, 2);
					tableNode.Tag = table;
					if (target == table)
					{
						selNode = tableNode;
					}
					deviceNode.Nodes.Add(tableNode);
				}
			}
		}

        private void DataSourceTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (SourceAttributes == null)
            {
                OKBtn.Enabled = true;
            }
            else
            {
                object target = e.Node.Tag;
                if (target is GXDevice && (SourceAttributes.UsedDataSources & GXDataIOSourceType.Device) != 0)
                {
                    OKBtn.Enabled = true;
                }
                else if (target is GXCategory && (SourceAttributes.UsedDataSources & GXDataIOSourceType.Category) != 0)
                {
                    OKBtn.Enabled = true;
                }
                else if (target is GXTable && (SourceAttributes.UsedDataSources & GXDataIOSourceType.Table) != 0)
                {
                    OKBtn.Enabled = true;
                }
                else if (target is GXProperty && (SourceAttributes.UsedDataSources & GXDataIOSourceType.Property) != 0)
                {
                    OKBtn.Enabled = true;
                }
                else if (target is GXDeviceGroup && (SourceAttributes.UsedDataSources & GXDataIOSourceType.DeviceGroup) != 0)
                {
                    OKBtn.Enabled = true;
                }
                else if (target is GXDeviceList && (SourceAttributes.UsedDataSources & GXDataIOSourceType.DeviceList) != 0)
                {
                    OKBtn.Enabled = true;
                }
                else
                {
                    OKBtn.Enabled = false;
                }
            }
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            //If editor is used to shown table columns.
            if (SourceAttributes == null)
            {
                Target = DataSourceTree.SelectedNode.Tag;
                return;
            }
            //If target is changed.
            if (Target != DataSourceTree.SelectedNode.Tag)
            {
                GXDataIOSourceAttribute att = TypeDescriptor.GetAttributes(DataSourceTree.SelectedNode.Tag)[typeof(GXDataIOSourceAttribute)] as GXDataIOSourceAttribute;
                long values;
                Array types;
                if (SourceAttributes.SupportedProperties == null)
                {
                    values = (long)att.SupportedProperties;
                    types = Enum.GetValues(att.SupportedProperties.GetType());
                }
                else
                {
                    values = ((long)att.SupportedProperties & (long)SourceAttributes.SupportedProperties);
                    types = Enum.GetValues(SourceAttributes.SupportedProperties.GetType());
                }                                
                foreach (object it in types)
                {
                    long val = Convert.ToInt64(it);
                    if (((val & values) == val || values == -1)&& val != 0)
                    {
                        TargetType = Enum.GetValues(att.SupportedProperties.GetType()).GetValue(val);
                        break;
                    }
                }
                Target = DataSourceTree.SelectedNode.Tag;
            }                        
        }
    }
}
