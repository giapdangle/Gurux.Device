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
using System.Xml.Serialization;

namespace Gurux.Device
{
    ///<summary>
    ///Determines the status of a GXDevice, category, table or property, the enumeration
    ///tells, if the device is occupied doing something, and what the action is.
    ///</summary>
    [Flags]
    public enum DeviceStates : int
    {
        ///<summary>
        ///Initial state.
        ///</summary>
        [XmlEnum("0")]
        None = 0,
        ///<summary>
        ///Disconnected from a device. 
        ///</summary>
        [XmlEnum("1")]
        Disconnected = 1,        
        ///<summary>
        ///Connected to a device.
        ///</summary>
        [XmlEnum("2")]
        Connected = 2,
        ///<summary>
        ///Monitoring has started.
        ///</summary>
        ///<remarks>
        ///Used for notifies.
        ///</remarks>
        [XmlEnum("4")]
        MonitorStart = 4,
        ///<summary>
        ///Monitoring has ended.
        ///</summary>
        ///<remarks>
        ///Used for notifies.
        ///</remarks>
        [XmlEnum("8")]
        MonitorEnd = 8,
        ///<summary>
        ///Reading has started.
        ///</summary>
        ///<remarks>
        ///Used for notifies.
        ///</remarks>
        [XmlEnum("16")]
        ReadStart = 16,
        ///<summary>
        ///Reading has ended. 
        ///</summary>
        ///<remarks>
        ///Used for notifies.
        ///</remarks>
        [XmlEnum("32")]
        ReadEnd = 32,
        ///<summary>
        ///Writing has started.
        ///</summary>
        ///<remarks>
        ///Used for notifies.
        ///</remarks>
        [XmlEnum("64")]
        WriteStart = 64,
        ///<summary>
        ///Writing has ended.
        ///</summary>
        ///<remarks>
        ///Used for notifies.
        ///</remarks>
        [XmlEnum("128")]
        WriteEnd = 128,
        ///<summary>
        ///Monitoring value(s) of device(s).
        ///</summary>
        [XmlEnum("256")]
        Monitoring = 256,
        ///<summary>
        ///Writing value(s).
        ///</summary>
        [XmlEnum("512")]
        Writing = 512,
        ///<summary>
        ///Reading value(s). 
        ///</summary>
        [XmlEnum("1024")]
        Reading = 1024,
        ///<summary>
        ///An error has occurred in a residing GXProperty, or in the device itself.
        ///</summary>
        [XmlEnum("2048")]
        Error = 2048,        
        ///<summary>
        ///Property value has changed.
        ///</summary>
        [XmlEnum("4096")]
        PropertyChanged = 4096,
        ///<summary>
        ///The error state has been reset by the user.
        ///</summary>
        [XmlEnum("8192")]
        ErrorReset = 8192,        
        ///<summary>
        ///Current transaction is aborted.
        ///</summary>
        [XmlEnum("16384")]
        TransactionAborted = 16384,        
        ///<summary>
        ///Device template is loaded.
        ///</summary>
        [XmlEnum("32768")]
        Loaded = 32768,        
        ///<summary>
        ///Device template is not loaded.
        ///</summary>
        [XmlEnum("65536")]
        Unloaded = 65536,        
        ///<summary>
        ///Scheduling has started.
        ///</summary>
        [XmlEnum("131072")]
        ScheduleStart = 131072,        
        ///<summary>
        ///Scheduling is in progress.
        ///</summary>
        [XmlEnum("262144")]
        Scheduling = 262144,
        ///<summary>
        ///Scheduling has ended.
        ///</summary>
        [XmlEnum("524288")]
        ScheduleEnd = 524288,
        ///<summary>
        ///Connecting to the device.
        ///</summary>
        [XmlEnum("1048576")]
        Connecting = 1048576,
        ///<summary>
        ///Disconnecting from the device.        
        ///</summary>
        [XmlEnum("2097152")]
        Disconnecting = 2097152,
        ///<summary>
        ///Connected to the Media.
        ///</summary>
        [XmlEnum("4194304")]
        MediaConnected = 4194304,
        ///<summary>
        ///Disconnected from the Media.
        ///</summary>
        [XmlEnum("8388608")]
        MediaDisconnected = 8388608,
        ///<summary>
        ///User has changed the media settings, or selected a new media.
        ///</summary>
        [XmlEnum("16777216")]
        MediaSettings = 0x1000000,
        ///<summary>
        ///User has changed the device settings (notified every time).
        ///</summary>
        [XmlEnum("33554432")]
        DeviceSettings = 0x2000000,
        /// <summary>
        /// Keep alive 
        /// </summary>
        [XmlEnum("67108864")]
        Keepalive = 0x4000000,
        /// <summary>
        /// Content of device is updated.
        /// </summary>
        [XmlEnum("134217728")]
        Updated = 0x8000000
    }
}
