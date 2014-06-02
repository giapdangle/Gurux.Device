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
	/// <summary>
	/// Interface describing GXPacketHandler.
	/// </summary>
    public interface IGXPacketHandler
    {
        /// <summary>
        /// Parent component.
        /// </summary>
        object Parent
        {
            get;
            set;
        }

        ///<summary>
        /// Connect to the meter.
        ///</summary>
        ///<remarks>
        ///Initialize all packet parset settings here.
        ///</remarks>
        void Connect(object sender);

        ///<summary>
        /// Disconnect from the meter.
        ///</summary>
        ///<remarks>
        ///Make cleanup here.
        ///</remarks>
        void Disconnect(object sender);

		/// <summary>
		/// Called to prepare GXPacket for a send command.
		/// </summary>
        void ExecuteSendCommand(object sender, string command, Gurux.Communication.GXPacket packet);

		/// <summary>
		/// Called to parse received data.
		/// </summary>
        void ExecuteParseCommand(object sender, string command, Gurux.Communication.GXPacket[] packets);

		/// <summary>
		/// Called to verify if the transaction is complete.
		/// </summary>
        bool IsTransactionComplete(object sender, string command, Gurux.Communication.GXPacket packet);

        /// <summary>
        /// Called when event is received from the meter.
        /// </summary>
        void ExecuteNotifyCommand(object sender, string command, Gurux.Communication.GXPacket packet, byte[] data, string senderInfo);

        ///<summary>
        /// Convert UI value to device value.
        ///</summary>
        object UIValueToDeviceValue(GXProperty sender, object value);

        ///<summary>
        /// Convert device value to UI value.
        ///</summary>
        object DeviceValueToUIValue(GXProperty sender, object value);
    }
}
