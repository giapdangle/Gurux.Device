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
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Gurux.Device.Editor
{
	/// <summary>
	/// GXValueItem contains a Device value and an User Interface value. Ex. 0 and "Off" or 1 and "On".
	/// </summary>
    [DataContract()]
    [Serializable]
    public class GXValueItem
    {
		/// <summary>
		/// Default constructor
		/// </summary>
        public GXValueItem()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GXValueItem(object uiValue, object deviceValue)
        {
            UIValue = uiValue;
            DeviceValue = deviceValue;
        }

		/// <summary>
		/// DeviceValue is the actual value received from the physical device.
		/// </summary>
        [DefaultValue(null), DataMember(IsRequired = false, EmitDefaultValue = false)]
        [ValueAccess(ValueAccessType.Edit, ValueAccessType.None)]       
        [TypeConverter(typeof(StringConverter))]
        public object DeviceValue
        {
            get;
            set;
        }

		/// <summary>
		/// UIValue is the value displayed on User Interface.
		/// </summary>
        [DefaultValue(null), DataMember(IsRequired = false, EmitDefaultValue = false)]
        [ValueAccess(ValueAccessType.Edit, ValueAccessType.None)]        
        [TypeConverter(typeof(StringConverter))]
        public object UIValue
        {
            get;
            set;
        }

		/// <summary>
		/// Returns string version of both values.
		/// </summary>
        public override string ToString()
        {
            return Convert.ToString(UIValue) + " [" + Convert.ToString(DeviceValue) + "]";
        }
    }
}
