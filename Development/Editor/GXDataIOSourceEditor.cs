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

namespace Gurux.Device.Editor
{
    internal class GXDataIOSourceEditor : UITypeEditor
    {
        System.Windows.Forms.Design.IWindowsFormsEditorService m_EdSvc = null;
        System.Windows.Forms.ListBox m_List;

        /// <summary>
        /// Shows a dropdown icon in the property editor
        /// </summary>
        /// <remarks>
        /// Search attributes and shpw dropdown list if there are more than one attribute to shown.
        /// </remarks>
        /// <param name="context">The context of the editing control</param>
        /// <returns>Returns <c>UITypeEditorEditStyle.DropDown</c></returns>
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            if (context == null || context.Instance == null)
            {
                return base.GetEditStyle(context);
            }
            if (context.Instance is GXDataIOSource)
            {
                GXDataIOSource source = context.Instance as GXDataIOSource;
                GXDataIOSourceAttribute att = TypeDescriptor.GetAttributes(source.Parent)[typeof(GXDataIOSourceAttribute)] as GXDataIOSourceAttribute;
                long values;
                Array types;
                if (source.Target == null)
                {
                    values = (long)att.SupportedProperties;
                    types = Enum.GetValues(att.SupportedProperties.GetType());
                }
                else
                {
                    GXDataIOSourceAttribute targetAttributes = TypeDescriptor.GetAttributes(source.Target)[typeof(GXDataIOSourceAttribute)] as GXDataIOSourceAttribute;
                    if (att.SupportedProperties == null)
                    {
                        values = (long)targetAttributes.SupportedProperties;
                    }
                    else
                    {
                        values = ((long)att.SupportedProperties & (long)targetAttributes.SupportedProperties);
                    }
                    types = Enum.GetValues(targetAttributes.SupportedProperties.GetType());
                }
                int cnt = 0;
                foreach (object it in types)
                {
                    long val = Convert.ToInt64(it);
                    if (((val & values) == val || values == -1) && val != 0)
                    {
                        ++cnt;
                    }
                }
                return cnt == 1 ? UITypeEditorEditStyle.None : UITypeEditorEditStyle.DropDown;
            }
            return UITypeEditorEditStyle.DropDown;
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
            if ((m_EdSvc = (System.Windows.Forms.Design.IWindowsFormsEditorService)provider.GetService(typeof(System.Windows.Forms.Design.IWindowsFormsEditorService))) == null)
            {
                return value;
            }
            // Create a CheckedListBox and populate it with all the propertylist values
            m_List = new System.Windows.Forms.ListBox();
            m_List.BorderStyle = System.Windows.Forms.BorderStyle.None;
            GXDataIOSource source = (GXDataIOSource)context.Instance;
            GXDataIOSourceAttribute att = TypeDescriptor.GetAttributes(source.Parent)[typeof(GXDataIOSourceAttribute)] as GXDataIOSourceAttribute;
            List<Enum> tmp = new List<Enum>();
            Array SourceTypes = Enum.GetValues(value.GetType());
            Array TargetTypes = null;
            if (att.SupportedProperties != null)
            {
                long values = (long)att.SupportedProperties;
                foreach (object it in Enum.GetValues(att.SupportedProperties.GetType()))
                {
                    long val = Convert.ToInt64(it);
                    if ((val & values) == val && val != 0)
                    {
                        tmp.Add((Enum)it);
                    }
                }
                TargetTypes = tmp.ToArray();
            }

            foreach (object it in SourceTypes)
            {
                if (TargetTypes == null)
                {
                    long val = Convert.ToInt64(it);
                    if (val > 0)
                    {
                        m_List.Items.Add(Enum.GetName(value.GetType(), it));
                    }
                }
                else
                {
                    foreach (Enum it2 in TargetTypes)
                    {
                        if (string.Compare(Enum.GetName(value.GetType(), it), it2.ToString()) == 0)
                        {
                            m_List.Items.Add(Enum.GetName(value.GetType(), it));
                            break;
                        }
                    }
                }
            }
            if (value != null)
            {
                m_List.SelectedIndex = m_List.Items.IndexOf(value.ToString());
            }
            m_List.SelectedIndexChanged += new System.EventHandler(this.OnSelectedIndexChanged);
            // Show Listbox as a DropDownControl. This methods returns only when the dropdowncontrol is closed
            m_EdSvc.DropDownControl(m_List);
            if (m_List.SelectedItem != null)
            {
                return Enum.Parse(value.GetType(), m_List.SelectedItem.ToString());
            }
            return value;
        }

        /// <summary>
        /// Close wnd when user selects new item.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        private void OnSelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (m_EdSvc != null)
            {
                m_EdSvc.CloseDropDown();
            }
        }
    }
}
