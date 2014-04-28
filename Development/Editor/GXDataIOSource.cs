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
using System.ComponentModel;
using System.Globalization;
using System.Drawing.Design;
using Gurux.Device.Properties;

namespace Gurux.Device.Editor
{
	/// <summary>
	/// The data source of the control.
	/// </summary>
	/// <remarks>
	/// A control can use GXProperties, GXCategories or GXTables as its data source.
	/// The DataIOSource also defines how the source of data is read.
	/// For example, a control can monitor the time when a value was last read, instead of the actual value.
	/// </remarks>
	[TypeConverter(typeof(GXDataIOSourceTypeConverter)), Serializable]
	public class GXDataIOSource : System.Runtime.Serialization.ISerializable
	{
		/// <summary>
		/// Initializes a new instance of the GXDataIOSource class.
		/// </summary>
		public GXDataIOSource()
		{
			UseUIValue = true;
		}

		/// <summary>
		/// Initializes a new instance of the GXDataIOSource class.
		/// </summary>
		public GXDataIOSource(Component parent)
		{
			UseUIValue = true;
			Parent = parent;
		}

		#region ISerializable Members

		/// <summary>
		/// Serialize constructor.
		/// </summary>
		/// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the data.</param>
		/// <param name="context">The associated System.Runtime.Serialization.StreamingContext.</param>
		public GXDataIOSource(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			UseUIValue = true;
			Serialize(info, context);
		}

		/// <summary>
		/// Serialize the datasource.
		/// </summary>
		protected void Serialize(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			m_TargetID = info.GetUInt64("ID");
			this.Action = info.GetInt64("Action");
			try
			{
				if (m_TargetID != 0)
				{
					this.UseUIValue = info.GetBoolean("UseUIValue");
				}
				else
				{
					this.UseUIValue = true;
				}
			}
			catch
			{
				this.UseUIValue = true;
			}
		}

		void System.Runtime.Serialization.ISerializable.GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			if (Target is GXProperty)
			{
				GXProperty prop = Target as GXProperty;
				info.AddValue("ID", prop.ID);
			}
			else if (Target is GXCategory)
			{
				GXCategory cat = Target as GXCategory;
				info.AddValue("ID", cat.ID);
			}
			else if (Target is GXTable)
			{
				GXTable table = Target as GXTable;
				info.AddValue("ID", table.ID);
			}
			else if (Target is GXDevice)
			{
				GXDevice device = Target as GXDevice;
				info.AddValue("ID", device.ID);
			}
			else if (Target is GXDeviceGroup)
			{
				GXDeviceGroup group = Target as GXDeviceGroup;
				info.AddValue("ID", group.ID);
			}
			else if (Target is GXDeviceList)
			{
				info.AddValue("ID", 0);
			}
			else
			{
				info.AddValue("ID", m_TargetID);
			}
			info.AddValue("Action", Convert.ToInt64(this.Action));
			info.AddValue("UseUIValue", this.UseUIValue);
		}

		/// <summary>
		/// Causes the component to validate its state.
		/// </summary>
		/// <remarks>The framework calls this method when it needs the component to validate its state. 
		/// If an error has occurred in the component, it is added to GXTaskCollection list.</remarks>
		/// <param name="tasks">GXTaskCollection collection where new tasks are added.</param>
		public void Validate(GXTaskCollection tasks)
		{
			tasks.RemoveUnusedTasks(Parent, false);
			if (Target == null)
			{
				GXDataIOSourceAttribute att = (GXDataIOSourceAttribute)TypeDescriptor.GetAttributes(this.Parent)[typeof(GXDataIOSourceAttribute)];
				if (att == null || !att.DataIOSourceCanBeUnknown)
				{
					tasks.Add(new GXTask(Parent, "DataIOSource", Resources.DataIOSourceTargetIsUnknown));
				}
			}
		}

		#endregion

		/// <summary>
		/// Determines whether an UI value (True) or a device value (False) is used.
		/// </summary>
		[Description("Determines whether an UI value (True) or a device value (False) is used."), CategoryAttribute("Behavior")]
		public bool UseUIValue
		{
			get;
			set;
		}

		ulong m_TargetID = ulong.MaxValue;

		/// <summary>
		/// ID of the GXDataIOSource target.
		/// </summary>
		[Browsable(false)]
		public ulong TargetID
		{
			get
			{
				return this.m_TargetID;
			}
		}

		/// <summary>
		/// Action
		/// </summary>
		[TypeConverter(typeof(GXDataIOSourceActionConverter))]
		[Editor(typeof(GXDataIOSourceEditor), typeof(System.Drawing.Design.UITypeEditor))]
		[RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All)]
		public object Action
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets target (property, category or a table) that this GXDataIOSource instance is bind to.
		/// </summary>
		[Browsable(false)]
		public object Target
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the parent object as a Control.
		/// </summary>
		/// <remarks>
		/// If the parent object is not a Control the property returns null;
		/// </remarks>
		[Browsable(false)]
		public IComponent Parent
		{
			get;
			internal set;
		}

		/// <summary>
		/// Returns the name of the Target.
		/// </summary>
		public override string ToString()
		{
			if (Target is GXDevice)
			{
				return ((GXDevice)Target).DeviceProfile;
			}
			else if (Target is GXCategory)
			{
				return ((GXCategory)Target).DisplayName;
			}
			else if (Target is GXTable)
			{
				return ((GXTable)Target).DisplayName;
			}
			else if (Target is GXProperty)
			{
				return ((GXProperty)Target).DisplayName;
			}
			else if (Target is GXDeviceList)
			{
				return ((GXDeviceList)Target).Name;
			}
			else if (Target is GXDeviceGroup)
			{
				return ((GXDeviceGroup)Target).Name;
			}
			return Resources.Empty;
		}

		/// <summary>
		/// Returns default text of the Target member.
		/// </summary>
		/// <param name="status">Describes the state of the property. The integer value of the Gurux.Device.PropertyStates enum.</param>
		/// <returns>The default text</returns>
		public string GetDefaultText(int status)
		{
			if (Target is GXProperty)
			{
				GXProperty.AvailableTargets action = (GXProperty.AvailableTargets)Convert.ToInt64(Action);
				GXProperty prop = Target as GXProperty;
				if (action == GXProperty.AvailableTargets.Name)
				{
					return prop.DisplayName;
				}
				if (action == GXProperty.AvailableTargets.Value)
				{
					//Return if value has not change.
					if ((status & (int)Gurux.Device.PropertyStates.ValueChanged) == 0)
					{
						if ((status & (int)Gurux.Device.PropertyStates.ErrorChanged) != 0)
						{
							return null;
						}
					}
					return Convert.ToString(prop.GetValue(UseUIValue));
				}
				if (action == GXProperty.AvailableTargets.LastRead)
				{
					//Return if value has not change.
					if ((status & (int)Gurux.Device.PropertyStates.ValueChanged) == 0)
					{
						return "";
					}
					return prop.ReadTime.ToString();
				}

				if (action == GXProperty.AvailableTargets.LastWrite)
				{
					//Return if value has not change.
					if ((status & (int)Gurux.Device.PropertyStates.ValueChanged) == 0)
					{
						return "";
					}
					return prop.WriteTime.ToString();
				}
				if (action == GXProperty.AvailableTargets.MinimumValue)
				{
					//Return if value has not change.
					if ((status & (int)Gurux.Device.PropertyStates.MinChanged) == 0)
					{
						return "";
					}
					return Convert.ToString(prop.Statistics.Minimun);
				}
				if (action == GXProperty.AvailableTargets.MaximumValue)
				{
					//Return if value has not change.
					if ((status & (int)Gurux.Device.PropertyStates.MaxChanged) == 0)
					{
						return "";
					}
					return Convert.ToString(prop.Statistics.Maximum);
				}
				if (action == GXProperty.AvailableTargets.AvarageValue)
				{
					//Return if value has not change.
					if ((status & (int)Gurux.Device.PropertyStates.AverageChanged) == 0)
					{
						return "";
					}
					return Convert.ToString(prop.Statistics.Average);
				}
				if (action == GXProperty.AvailableTargets.Unit)
				{
					//Return if value has not change.
					if ((status & (int)Gurux.Device.PropertyStates.ValueChanged) == 0)
					{
						return "";
					}
					return prop.Unit;
				}
			}
			else if (Target is GXCategory)
			{
				GXCategory.AvailableTargets action = (GXCategory.AvailableTargets)Convert.ToInt64(Action);
				GXCategory cat = Target as GXCategory;
				if (action == GXCategory.AvailableTargets.Name)
				{
					return cat.Name;
				}
			}
			else if (Target is GXTable)
			{
				GXTable.AvailableTargets action = (GXTable.AvailableTargets)Convert.ToInt64(Action);
				GXTable table = Target as GXTable;
				if (action == GXTable.AvailableTargets.Name)
				{
					return table.DisplayName;
				}
			}
			else if (Target is GXDevice)
			{
				GXDevice device = Target as GXDevice;
				GXDevice.AvailableTargets action = (GXDevice.AvailableTargets)Convert.ToInt64(Action);
				switch (action)
				{
					case GXDevice.AvailableTargets.Name:
						return device.Name;
					case GXDevice.AvailableTargets.DeviceType:
						return device.DeviceProfile;
					case GXDevice.AvailableTargets.ProtocolName:
						return device.DeviceProfile;
					case GXDevice.AvailableTargets.UpdateInterval:
						return device.UpdateInterval.ToString();
					case GXDevice.AvailableTargets.ResendWaitTime:
						if (device != null)
						{
							return device.WaitTime.ToString();
						}
						else
						{
							return device.Name;
						}
					case GXDevice.AvailableTargets.ResendCount:
						if (device != null)
						{
							return device.ResendCount.ToString();
						}
						else
						{
							return device.Name;
						}
					case GXDevice.AvailableTargets.Description:
						if (device != null)
						{
							return device.Description;
						}
						else
						{
							return device.Name;
						}
				}
			}
			else if (Target is GXDeviceGroup)
			{
				GXDeviceGroup.AvailableTargets action = (GXDeviceGroup.AvailableTargets)Convert.ToInt64(Action);
				GXDeviceGroup group = Target as GXDeviceGroup;
				if (action == GXDeviceGroup.AvailableTargets.Name)
				{
					return group.Name;
				}
			}
			else if (Target is GXDeviceList)
			{
				GXDeviceList.AvailableTargets action = (GXDeviceList.AvailableTargets)Convert.ToInt64(Action);
				GXDeviceList list = Target as GXDeviceList;
				if (action == GXDeviceList.AvailableTargets.Name)
				{
					return list.Name;
				}
			}
			if (this.Parent is System.Windows.Forms.Control)
			{
				return ((System.Windows.Forms.Control)this.Parent).Text;
			}
			return null;
		}

		/// <summary>
		/// Returns true if the associated item is used, i.e a control uses this item as a source.
		/// </summary>
		/// <param name="target">The item checked for availability.</param>
		/// <returns>True if the associated item is in use.</returns>
		public bool IsInUse(object target)
		{
			return Target == target;
		}

		/// <summary>
		/// Update DataIOSource string to component after load.
		/// </summary>
		public void UpdateDataIOSource(object target)
		{
			if (m_TargetID == ulong.MaxValue)
			{
				return;
			}
			GXDevice device = target as GXDevice;
			if (device != null)
			{
				this.Target = device.FindItemByID(device.ID + m_TargetID);
			}
			else if (target is GXDeviceList)
			{
				GXDeviceList list = target as GXDeviceList;
				this.Target = list.FindItemByID(m_TargetID);
			}
			else if (target is GXDeviceGroup)
			{
				GXDeviceGroup group = target as GXDeviceGroup;
				this.Target = group.FindItemByID(m_TargetID);
			}
			if (this.Target is GXProperty)
			{
				this.Action = (GXProperty.AvailableTargets)this.Action;
			}
			else if (this.Target is GXCategory)
			{
				this.Action = (GXCategory.AvailableTargets)this.Action;
			}
			else if (this.Target is GXTable)
			{
				this.Action = (GXTable.AvailableTargets)this.Action;
			}
			else if (this.Target is GXDevice)
			{
				this.Action = (GXDevice.AvailableTargets)this.Action;
			}
			else if (this.Target is GXDeviceGroup)
			{
				this.Action = (GXDevice.AvailableTargets)this.Action;
			}
			else if (this.Target is GXDeviceList)
			{
				this.Action = (GXDevice.AvailableTargets)this.Action;
			}
		}
	}
}