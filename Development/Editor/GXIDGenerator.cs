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

namespace Gurux.Device.Editor
{
	/// <summary>
	/// Generates a unique ID for each object of the device.
	/// </summary>
    internal class GXIDGenerator : List<ulong>
	{
        static ulong m_LastFoundFreeTemplateID = 1;
        static ulong m_LastFoundFreeID = 0x10000;

		/// <summary>
        /// Generates a unique ID. 
        /// </summary>
        /// <returns>The generated unique ID for template.</returns>
        internal ulong GenerateTemplateID()
		{
            for (ulong pos = m_LastFoundFreeTemplateID; pos < 0x10000; ++pos)
			{
				if (!this.Contains(pos))
				{
					Add(pos);
                    m_LastFoundFreeTemplateID = pos;
					return pos;
				}
			}
            m_LastFoundFreeTemplateID = 1;
            throw new Exception("Invalid Gurux Device template ID.");
		}

        /// <summary>
        /// Generates a unique ID. 
        /// </summary>
        /// <returns>The generated unique ID.</returns>
        internal ulong GenerateID()
        {
            for (ulong pos = m_LastFoundFreeID; pos < ulong.MaxValue; pos += 0x10000)
            {
                if (!this.Contains(pos))
                {
                    Add(pos);
                    m_LastFoundFreeID = pos;
                    return pos;
                }
            }
            m_LastFoundFreeID = 1;
            throw new Exception("Invalid Gurux Device template ID.");
        }	
	}
}
