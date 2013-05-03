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
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.ComponentModel;

namespace Gurux.Device.Editor
{
	/// <summary>
	/// An UITypeEditor used with GXDataIOSource.
	/// </summary>
    public class UITextTypeEditor : UITypeEditor
    {
        /// <summary>
        /// Shows a dropdown icon in the property editor
        /// </summary>
        /// <param name="context">The context of the editing control</param>
        /// <returns>Returns <c>UITypeEditorEditStyle.DropDown</c></returns>
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            if (System.ComponentModel.TypeDescriptor.GetAttributes(context.Instance)[typeof(GXDataIOSourceAttribute)] == null)
            {
                return UITypeEditorEditStyle.None;
            }            
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
            GXDataIOSource source = value as GXDataIOSource;            
            GXDataIOSourceAttribute comp = System.ComponentModel.TypeDescriptor.GetAttributes(source.Parent)[typeof(GXDataIOSourceAttribute)] as GXDataIOSourceAttribute;
            GXDevice device = source.Parent.Site.GetService(typeof(GXDevice)) as GXDevice;            
            GXDataIOSourceDialog dlg;
            if (device == null)
            {
                GXDeviceList list = source.Parent.Site.GetService(typeof(GXDeviceList)) as GXDeviceList;
                dlg = new GXDataIOSourceDialog((GXDataIOSource)value, comp, list);
            }
            else
            {
                dlg = new GXDataIOSourceDialog((GXDataIOSource)value, comp, device);
            }            
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                object oldTarget = source.Target;
                source.Target = dlg.Target;
                source.Action = dlg.TargetType;
                string name = "Target";
                if (source.Parent.Site != null)
                {
                    System.ComponentModel.Design.IComponentChangeService ccsChanger = (System.ComponentModel.Design.IComponentChangeService)source.Parent.Site.GetService(typeof(System.ComponentModel.Design.IComponentChangeService));
                    if (ccsChanger != null)
                    {
                        MemberDescriptor md = TypeDescriptor.GetProperties(source).Find(name, true);
                        if (md == null)
                        {
                            System.Diagnostics.Debug.WriteLine(string.Format("GXSite NotifyChange failed. Propertys '{0}' name '{1}' is unknown", source.Parent.Site.Name, name));
                        }
                        ccsChanger.OnComponentChanging(source.Parent.Site.Component, md);
                        ccsChanger.OnComponentChanged(source.Parent.Site.Component, md, oldTarget, dlg.Target);
                    }
                }
            }
            return value;
        }
       
    }
}
