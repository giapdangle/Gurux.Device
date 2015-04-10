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

namespace Gurux.Device
{
    public class GXSelectDeviceProfileEventArgs
    {
        /// <summary>
        /// Selected device or device profile.
        /// </summary>
        public object Target
        {
            get;
            set;
        }

        /// <summary>
        /// Can user select new item if target is given.
        /// </summary>
        public bool Edit
        {
            get;
            set;
        }

        /// <summary>
        /// Is thread used when user is selected new target and apply changes.
        /// </summary>
        public bool UseThread
        {
            get;
            set;
        }

        /// <summary>
        /// Action that is done when user accept new item.
        /// </summary>
        public ProfileSelectEventHandler OnInitialize
        {
            get;
            set;
        }

        /// <summary>
        /// Device profiles that are shown. If null default device profiles are shown.
        /// </summary>
        public GXDeviceProfileCollection DeviceProfiles
        {
            get;
            private set;
        }

        /// <summary>
        /// Custom parameters.
        /// </summary>
        public object[] Parameters
        {
            get;
            set;
        }

        /// <summary>
        /// Action that is done when user accept new item.
        /// </summary>
        public ProfileSelectEventHandler OnSelect
        {
            get;
            set;
        }

        /// <summary>
        /// Serializated settings for UI.
        /// </summary>
        public string Settings
        {
            get;
            set;
        }

        /// <summary>
        /// Server where new device profiles are downloaded.
        /// </summary>
        public string DownloadServer
        {
            get;
            set;
        }

        /// <summary>
        /// UserName.
        /// </summary>
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// UserName.
        /// </summary>
        public string Password
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public GXSelectDeviceProfileEventArgs(object target, ProfileSelectEventHandler select)
        {
            DeviceProfiles = new GXDeviceProfileCollection();
            Target = target;
            OnSelect = select;
            Edit = true;
        }
    }
}
