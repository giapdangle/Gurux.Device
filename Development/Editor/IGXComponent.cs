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
using System.ComponentModel;

namespace Gurux.Device.Editor
{
    /// <summary>
    /// Defines an interface common for all user interface controls used in GXDeviceEditor and GXDirector.
    /// Using the IGXComponent interface, GXDeviceEditor and GXDirector framework communicates with user controls.
    /// </summary>
    public interface IGXComponent
    {
		/// <summary>
		/// The GXDataIOSource of the components. Provides value or text from a GXObject (ex. GXProperty) for the component to display.
		/// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        GXDataIOSource DataIOSource
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the GXDevice object associated with the component.
        /// </summary>
        /// <param name="dev">Gurux device component.</param>
        void SetDevice(object dev);

        /// <summary>
        /// Causes the component to validate its state.
        /// </summary>
        /// <remarks>The framework calls this method when it needs the component to validate its state. 
        /// If an error has occurred in the component, it is added to GXTasks list.</remarks>
        /// <param name="tasks">GXTasks collection where new tasks are added.</param>
        void Validate(GXTaskCollection tasks);

        /// <summary>
        /// The framework calls this method before the component is loaded.
        /// </summary>
        void BeforeLoad(object target);

        /// <summary>
        /// The framework calls this method after the component is loaded.
        /// </summary>
        void AfterLoad(object target);

        /// <summary>
        /// Returns true if the component has changed and it needs to be saved.
        /// </summary>
        /// <returns>Does the component needs to be saved.</returns>
        bool IsDirty();

        /// <summary>
        /// Start or stop listening events of the GXDevice object.
        /// </summary>
        /// <param name="listen">Start or stop listening events of the GXDevice object.</param>
        void StartListenEvents(bool listen);

        /// <summary>
        /// Causes the component to clear its data.
        /// </summary>
        /// <remarks>
        /// The framework calls this method when the component is about to be destroyed.
        /// </remarks>
        void ClearComponentData();
    }
}
