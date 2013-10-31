using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;

namespace Gurux.Device.Editor
{
    internal class GXKeepaliveEditor : System.Drawing.Design.UITypeEditor
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
            foreach (GXCategory cat in (context.Instance as GXKeepalive).Parent.Categories)
            {
                foreach (GXProperty prop in cat.Properties)
                {
                    m_List.Items.Add(prop);
                }
            }
            if (value != null)
            {
                m_List.SelectedIndex = m_List.Items.IndexOf(value);
            }
            m_List.SelectedIndexChanged += new System.EventHandler(this.OnSelectedIndexChanged);
            // Show Listbox as a DropDownControl. This methods returns only when the dropdowncontrol is closed
            m_EdSvc.DropDownControl(m_List);
            (context.Instance as GXKeepalive).Target = m_List.SelectedItem;
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
