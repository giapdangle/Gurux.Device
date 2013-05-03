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
	/// An attribute describign packet parser and packet handler for a protocol addin.
	/// </summary>
    public class GXCommunicationAttribute : Attribute
    {
		/// <summary>
		/// Type of the packet parser used by the attributed protocol addin.
		/// </summary>
        public Type PacketParserType
        {
            get;
            internal set;
        }

		/// <summary>
		/// Type of the packet handler used by the attributed protocol addin.
		/// </summary>
        public Type PacketHandlerType
        {
            get;
            internal set;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="packetHandlerType"></param>
        public GXCommunicationAttribute(Type packetHandlerType)
        {
            PacketHandlerType = packetHandlerType;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="packetHandlerType"></param>
        /// <param name="packetParserType"></param>
        public GXCommunicationAttribute(Type packetHandlerType, Type packetParserType)
        {
            PacketHandlerType = packetHandlerType;
            PacketParserType = packetParserType;
        }
    }
}
