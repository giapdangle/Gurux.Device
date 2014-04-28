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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Gurux.Device.Properties;

namespace Gurux.Device.Editor
{
	/// <summary>
	/// This class is used to show a text editor in property grid.
	/// </summary>
	internal class GXUITextEditor : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button CancelBtn;
		private System.Windows.Forms.Button OKBtn;
		public System.Windows.Forms.TextBox EditText;
		private System.ComponentModel.Container m_Components = null;

		/// <summary>
		/// Initializes a new instance of the GXUITextEditor class.
		/// </summary>
		/// <param name="text">The text to be modified.</param>
		/// <param name="readOnly">True, if the text is not editable.</param>
		public GXUITextEditor(object text, bool readOnly)
		{
			InitializeComponent();

			if (text != null)
			{
				EditText.Text = text.ToString();
			}
			EditText.ReadOnly = readOnly;
			OKBtn.Enabled = !readOnly;
		}

        /// <summary> 
        /// Cleans up any resources being used.
        /// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (m_Components != null)
				{
					m_Components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		private void InitializeComponent()
		{
			this.EditText = new System.Windows.Forms.TextBox();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.OKBtn = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// EditText
			// 
			this.EditText.AcceptsReturn = true;
			this.EditText.AcceptsTab = true;
			this.EditText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
				| System.Windows.Forms.AnchorStyles.Left)
				| System.Windows.Forms.AnchorStyles.Right)));
			this.EditText.Location = new System.Drawing.Point(0, 0);
			this.EditText.Multiline = true;
			this.EditText.Name = "EditText";
			this.EditText.Size = new System.Drawing.Size(296, 232);
			this.EditText.TabIndex = 0;
			this.EditText.Text = "";
			// 
			// CancelBtn
			// 
			this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Location = new System.Drawing.Point(208, 240);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(80, 24);
			this.CancelBtn.TabIndex = 1;
			this.CancelBtn.Text = Resources.Cancel;
			// 
			// OKBtn
			// 
			this.OKBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OKBtn.Location = new System.Drawing.Point(112, 240);
			this.OKBtn.Name = "OKBtn";
			this.OKBtn.Size = new System.Drawing.Size(80, 24);
			this.OKBtn.TabIndex = 2;
			this.OKBtn.Text = "OK";
			// 
			// GXUITextEditor
			// 
			this.AcceptButton = this.OKBtn;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(296, 269);
			this.ControlBox = false;
			this.Controls.Add(this.OKBtn);
			this.Controls.Add(this.CancelBtn);
			this.Controls.Add(this.EditText);
			this.Name = "GXUITextEditor";
			this.ShowInTaskbar = false;
			this.Text = Resources.TextEditor;
			this.ResumeLayout(false);

		}
		#endregion
	}
}
