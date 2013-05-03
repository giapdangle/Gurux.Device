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
using System.Windows.Forms;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.ComponentModel;

namespace Gurux.Device.Editor
{
	/// <summary>
	/// This class is used to show a text editor in property grid.
	/// </summary>
    /// <remarks>
    /// The purpose of the text editor is to ease writing, and editing, multi-line texts.
    /// </remarks>
	public class UITextEditor : UITypeEditor
	{
		/// <summary>
		/// Shows a three-dots in the property grid.
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
		/// <param name="context">The context of the editing control.</param>
		/// <param name="provider">A valid service provider.</param>
		/// <param name="value">The current value of the object to edit.</param>
		/// <returns>The new value of the object.</returns>
		public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			bool ReadOnly = false;
			ReadOnlyAttribute att = (ReadOnlyAttribute)context.PropertyDescriptor.Attributes[typeof(ReadOnlyAttribute)];
			if (att != null)
			{
				ReadOnly = att.IsReadOnly;
			}
			GXUITextEditor dlg = new GXUITextEditor(value, ReadOnly);
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				return dlg.EditText.Text;
			}
			return value;
		}

	}
}
