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
using System.Drawing.Design;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace Gurux.Device
{
    /// <summary>
    /// Shows Media depended editor, and current user settings.
    /// </summary>
    public class GXMediaTypeEditor : UITypeEditor
    {
        /// <summary>
        /// Shows a dropdown icon in property grid.
        /// </summary>
        /// <param name="context">The context of the editing control.</param>
        /// <returns>Returns <c>UITypeEditorEditStyle.DropDown</c></returns>
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        /// <summary>
        /// Overrides the method used to provide basic behaviour for selecting editor.
        /// Shows our custom control for editing the value.
        /// </summary>
        /// <param name="context">The context of the editing control</param>
        /// <param name="provider">A valid service provider</param>
        /// <param name="value">The current value of the object to edit</param>
        /// <returns>The new value of the object</returns>
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {                       
            if (context == null || context.Instance == null || provider == null)
            {
                return base.EditValue(context, provider, value);
            }            
            //Show macro dialog.
            GXMediaType type = (GXMediaType)value;
            Gurux.Communication.GXClient cl = new Gurux.Communication.GXClient();
            Gurux.Common.IGXMedia media = cl.SelectMedia(type.Name) as Gurux.Common.IGXMedia;
            if (media != null)
            {
                media.Settings = type.DefaultMediaSettings;
                if (media.Properties(null))
                {
                    type.DefaultMediaSettings = media.Settings;
                }
            }
            return value;
        }        
    }
}
