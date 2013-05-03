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

namespace Gurux.Device
{
    [DataContract()]
    internal class GXParameter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GXParameter(ulong id, string name, object value, Type type)
        {
            this.ID = id;
            this.Name = name;
            if (value != null)
            {
                this.Value = Convert.ToString(value);
                if (type == typeof(object))
                {
                    Type = value.GetType().FullName;
                }
            }
        }

        [DataMember(IsRequired = true)]
        public ulong ID
        {
            get;
            set;
        }

        /// <summary>
        /// Parameter name.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Parameter value.
        /// </summary>
        [DataMember(Name = "Value", IsRequired = false, EmitDefaultValue = false)]
        public string Value
        {
            get;
            set;
        }

        /// <summary>
        /// Parameter value.
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string Type
        {
            get;
            set;
        }
    }
}
