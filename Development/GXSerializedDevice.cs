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
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using Gurux.Device.Editor;

namespace Gurux.Device
{
    [DataContract(Name="GXDevice")]
    internal class GXSerializedDevice
    {
        GXSerializedDevice()
        {            
        }

        void SaveParameters(object target, ulong id, List<GXParameter> parameters, GXDevice deviceTemplate)
        {
            object templateTarget = deviceTemplate.FindItemByID(id & 0xFFFF);
            PropertyDescriptorCollection templateProperties = TypeDescriptor.GetProperties(templateTarget);
            object value, templateValue;
            foreach (PropertyDescriptor it in TypeDescriptor.GetProperties(target))
            {               
                //If value is not stored.
                DataMemberAttribute dm = it.Attributes[typeof(DataMemberAttribute)] as DataMemberAttribute;
                if (dm == null)
                {
                    continue;
                }
                ValueAccessAttribute va = it.Attributes[typeof(ValueAccessAttribute)] as ValueAccessAttribute;
                if (va == null || va.RunTime != ValueAccessType.Edit)
                {
                    continue;
                }
                //Ignore default values.
                value = it.GetValue(target);
                templateValue = templateProperties[it.Name].GetValue(templateTarget);
                if (!object.Equals(templateValue, value))
                {
                    parameters.Add(new GXParameter(id, it.Name, value, it.PropertyType));
                }                    
            }
        }

        public GXSerializedDevice(GXDevice device, GXDevice deviceTemplate)
        {
            this.ID = device.ID;
            this.ProtocolName = device.ProtocolName;
            this.DeviceType = device.DeviceType;
            this.Name = device.Name;
            this.Guid = device.Guid;
            this.MediaType = device.GXClient.MediaType;
            this.MediaSettings = device.GXClient.MediaSettings;
            this.ResendCount = device.ResendCount;
            this.WaitTime = device.WaitTime;
            this.UpdateInterval = device.UpdateInterval;
            if (!string.IsNullOrEmpty(device.Manufacturer))
            {
                this.Manufacturer = device.Manufacturer;
                this.Model = device.Model;
                this.Version = device.Version;
                this.PresetName = device.PresetName;
            }

            List<GXParameter> parameters = new List<GXParameter>();
            SaveParameters(device, device.ID, parameters, deviceTemplate);
            foreach (GXCategory cat in device.Categories)
            {
                SaveParameters(cat, cat.ID, parameters, deviceTemplate);
                foreach (GXProperty prop in cat.Properties)
                {
                    SaveParameters(prop, prop.ID, parameters, deviceTemplate);
                }
            }
            foreach (GXTable table in device.Tables)
            {
                SaveParameters(table, table.ID, parameters, deviceTemplate);
            }
            Parameters = parameters.ToArray();
        }       
 
        public GXDevice CreateDevice()
        {
            GXDevice device;
            if (!string.IsNullOrEmpty(Manufacturer))
            {
                device = GXDevice.Create(this.Manufacturer, this.Model, this.Version, this.PresetName, this.Name);
            }
            else
            {
                device = GXDevice.Create(this.ProtocolName, this.DeviceType, this.Name);
            }
            device.ID = this.ID;
            Gurux.Common.IGXMedia media = device.GXClient.SelectMedia(this.MediaType);
            device.ResendCount = this.ResendCount;
            device.WaitTime = this.WaitTime;
            device.UpdateInterval = this.UpdateInterval;
            device.Manufacturer = this.Manufacturer;
            device.Model = this.Model;
            device.Version = this.Version;
            if (media != null)
            {
                media.Settings = this.MediaSettings;
            }
            device.GXClient.AssignMedia(media);
            object value;
            foreach (GXParameter it in Parameters)
            {
                try
                {
                    object target = device.FindItemByID(it.ID);
                    if (target == null)
                    {
                    //TODO: Show warning that parameter can't found.
                    }
                    else
                    {
                        System.Reflection.PropertyInfo pi = target.GetType().GetProperty(it.Name);
                        if (pi != null)
                        {
                            if (pi.PropertyType.IsEnum)
                            {
                                value = Enum.Parse(pi.PropertyType, it.Value);
                            }
                            else
                            {
                                if (pi.PropertyType == typeof(object) && it.Type != null)
                                {                                    
                                    Type type = Type.GetType(it.Type);
                                    value = Convert.ChangeType(it.Value, type);
                                }
                                else
                                {
                                    value = Convert.ChangeType(it.Value, pi.PropertyType);
                                }
                            }
                            pi.SetValue(target, value, null);
                        }
                    }
                }
                catch (Exception Ex)
                {
					if (device.GXClient != null && device.GXClient.Trace >= System.Diagnostics.TraceLevel.Error)
					{
						Gurux.Common.GXCommon.TraceWriteLine(Ex.Message);
					}
                }
            }
            return device;
        }        

        [DataMember(Name = "ID", IsRequired = true)]
        public ulong ID = 0;

        /// <summary>
        /// Retrieves or sets the name of the protocol of the device.
        /// </summary>
        [ReadOnly(true), Category("Design"), Description("Retrieves or sets the name of the protocol of the device.")]
        [DataMember(IsRequired = true)]
        public string ProtocolName
        {
            get;
            set;
        }
        
        /// <summary>
        /// Preset name of the device.
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string PresetName
        {
            get;
            set;
        }

        /// <summary>
        /// Retrieves or sets the DeviceType of the device. DeviceType is defined in device template.
        /// </summary>
        [Browsable(false), ReadOnly(true), Category("Design"), Description("Retrieves or sets the DeviceType of the device. DeviceType is defined in device template.")]
        [DataMember(IsRequired = true)]
        public string DeviceType
        {
            get;
            set;
        }

        /// <summary>
        /// Each device has creation Guid that is set when new device template is created.
        /// </summary>
        [DataMember(IsRequired = true)]
        [ReadOnly(true), Browsable(false)]
        public Guid Guid
        {
            get;
            internal set;
        }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        internal string MediaType
        {
            get;
            set;
        }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        internal string MediaSettings
        {
            get;
            set;
        }    

        [DefaultValue(null), DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string Name
        {
            get;
            set;
        }

        [DefaultValue(0), DataMember(IsRequired = false, EmitDefaultValue = false)]
        public int ResendCount
        {
            get;
            set;
        }

        [DefaultValue(1000), DataMember(IsRequired = false, EmitDefaultValue = false)]
        public int WaitTime
        {
            get;
            set;
        }

        [DefaultValue(0), DataMember(IsRequired = false, EmitDefaultValue = false)]
        public int UpdateInterval
        {
            get;
            set;
        }

        [DefaultValue(null), DataMember(IsRequired = false, EmitDefaultValue = false)]
        public GXParameter[] Parameters
        {
            get;
            set;
        }
        [DefaultValue(null), DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string Manufacturer
        {
            get;
            set;
        }
        [DefaultValue(null), DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string Model
        {
            get;
            set;
        }
        [DefaultValue(null), DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string Version
        {
            get;
            set;
        }        
    }
}
