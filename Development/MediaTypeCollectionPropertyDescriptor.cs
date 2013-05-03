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
using Gurux.Device.Editor;

namespace Gurux.Device
{
    /// <summary>
    /// A helper class for showing a media type in the property grid.
    /// </summary>
    /// <remarks>
    /// The MediaTypeCollectionPropertyDescriptor is used to set default settings for the media type.
    /// <br />
    /// Example: The port number in TCP/IP protocol.
    /// </remarks>
    internal class MediaTypeCollectionPropertyDescriptor : GXCollectionPropertyDescriptor
    {
        public MediaTypeCollectionPropertyDescriptor(GXMediaTypeCollection coll, int idx)
            : base(idx, coll)
        {
        }

        GXMediaTypeCollection List
        {
            get
            {
                return (GXMediaTypeCollection)m_List;
            }
        }

        public override string DisplayName
        {
            get
            {
                if (Index >= List.Count)
                {
                    return string.Empty;
                }
                return List[Index].Name;
            }
        }

        public override string Description
        {
            get
            {
                if (Index >= List.Count)
                {
                    return string.Empty;
                }
                return List[Index].Name + "Media properties";
            }
        }

        public override string Name
        {
            get
            {
                if (Index >= List.Count)
                {
                    return string.Empty;
                }
                return List[Index].Name.ToString();
            }
        }

        public override void ResetValue(object component)
        {
            List[Index].DefaultMediaSettings = string.Empty;
        }
    }
}
