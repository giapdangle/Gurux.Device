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

namespace Gurux.Device.Editor
{
	/// <summary>
	/// An attribute describing visibility and serialization of properties on classes.
	/// </summary>
    public class ValueAccessAttribute : Attribute
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="design">Design time accessibility.</param>
        /// <param name="runTime">Run time accessibility.</param>
        public ValueAccessAttribute(ValueAccessType design, ValueAccessType runTime)
        {
            this.Design = design;
            this.RunTime = runTime;            
        }

        /// <summary>
        /// Is parameter's value accessibility at the runtime.
        /// </summary>
        public ValueAccessType RunTime
        {
            get;
            internal set;
        }

        /// <summary>
        /// Is parameter's value accessibility at the designtime.
        /// </summary>
        public ValueAccessType Design
        {
            get;
            internal set;
        }       
    }
}
