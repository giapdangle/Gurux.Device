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
    /// <summary>
    /// Class is used to serialize device profile settings.
    /// </summary>
    class GXDeviceProfileFormSettings
    {
        /// <summary>
        /// Are custom devices shown.
        /// </summary>
        public bool Custom
        {
            get;
            set;
        }        

        /// <summary>
        /// Are downloadable devices shown.
        /// </summary>
        public bool Download
        {
            get;
            set;
        }

        /// <summary>
        /// Are earlier versions shown.
        /// </summary>
        public bool Earlier
        {
            get;
            set;
        }

        /// <summary>
        /// Search text.
        /// </summary>
        public string SearchText
        {
            get;
            set;
        }
    }
}
