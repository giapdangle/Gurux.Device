using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Gurux.Device.Editor
{
	/// <summary>
	/// Editor for table columns in PropertyGrid
	/// </summary>
    public class GXTableColumnEditor : UITypeEditor
    {
        /// <summary>
        /// Shows a dropdown icon in the property editor
        /// </summary>
        /// <param name="context">The context of the editing control</param>
        /// <returns>Returns <c>UITypeEditorEditStyle.DropDown</c></returns>
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            if (context.Instance is GXTable && ((GXTable)context.Instance).Columns.Count != 0)
            {
                return UITypeEditorEditStyle.Modal;
            }
            return UITypeEditorEditStyle.None;
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
            GXProperty target = value as GXProperty;
            GXDataIOSourceDialog dlg = new GXDataIOSourceDialog(target, context.Instance as GXTable);            
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                return dlg.Target;                
            }
            return value;
        }          
    }
}
