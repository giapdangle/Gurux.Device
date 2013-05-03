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

namespace Gurux.Device.Editor
{
	/// <summary>
	/// A collection of GXValuesItems.
	/// </summary>
    public class GXValueItemCollection : GenericList<GXValueItem>
    {
		/// <summary>
		/// Converts value from physical device to human readable user interface value.
		/// </summary>
        public object DeviceValueToUIValue(object value, bool forcePresetValues)
        {
            string str = Convert.ToString(value);
            foreach (GXValueItem it in this)
            {
                if (string.Compare(Convert.ToString(it.DeviceValue), str, true) == 0)
                {
                    return it.UIValue;
                }
            }
            if (!forcePresetValues)
            {
                throw new Exception("Invalid value. Forced preset value not found.");
            }
            return value;
        }

		/// <summary>
		/// Converts human readable user interface value value understood by physical device.
		/// </summary>
        public object UIValueToDeviceValue(object value, bool forcePresetValues)
        {
            string str = Convert.ToString(value);
            foreach (GXValueItem it in this)
            {
                if (string.Compare(Convert.ToString(it.UIValue), str, true) == 0)
                {
                    return it.DeviceValue;
                }
            }
            if (!forcePresetValues)
            {
                throw new Exception("Invalid value. Forced preset value not found.");
            }
            return value;
        }

		/// <summary>
		/// Finds a GXValueItem by device value.
		/// </summary>
        public GXValueItem FindByDeviceValue(object value, bool forcePresetValues)
        {
            string str = Convert.ToString(value);
            foreach (GXValueItem it in this)
            {
                if (string.Compare(Convert.ToString(it.DeviceValue), str, true) == 0)
                {
                    return it;
                }
            }
            if (!forcePresetValues)
            {
                throw new Exception("Invalid value. Forced preset value not found.");
            }
            return null;
        }

		/// <summary>
		/// Finds a GXValueItem by user interface value.
		/// </summary>
        public GXValueItem FindByUIValue(object value, bool forcePresetValues)
        {
            string str = Convert.ToString(value);
            foreach (GXValueItem it in this)
            {
                if (string.Compare(Convert.ToString(it.UIValue), str, true) == 0)
                {
                    return it;
                }
            }
            if (!forcePresetValues)
            {
                throw new Exception("Invalid value. Forced preset value not found.");
            }
            return null;
        }
    }
}
