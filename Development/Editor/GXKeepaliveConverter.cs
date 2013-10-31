using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Gurux.Device.Editor
{
    /// <summary>
    /// This class is used to show parameters of GXKeepalive.
    /// </summary>
    internal class GXKeepaliveConverter : TypeConverter
    {
        /// <summary>
        /// Allows displaying the + symbol in the property grid.
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
        /// <returns>True to find properties of this object.</returns>
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// Loops through keep alive parameters.
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
        /// <param name="value">An Object that specifies the type of array for which to get properties.</param>
        /// <param name="attributes">An array of type Attribute that is used as a filter.</param>
        /// <returns>Collection of properties exposed to this data type.</returns>
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            GXKeepalive target = value as GXKeepalive;
            PropertyDescriptorCollection pds = new PropertyDescriptorCollection(null);
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(value))
            {                
                if (pd.IsBrowsable)
                {
                    //If interval field is not shown.
                    if ((target.Ignore & KeepaliveFieldsIgnored.Interval) != 0 && pd.Name == "Interval")
                    {
                        continue;
                    }
                    //If target field is not shown.
                    if ((target.Ignore & KeepaliveFieldsIgnored.Target) != 0 && pd.Name == "Target")
                    {
                        continue;
                    }
                    //If target field is not shown.
                    if ((target.Ignore & KeepaliveFieldsIgnored.Reset) != 0 && pd.Name == "TransactionResets")
                    {
                        continue;
                    }
                    pds.Add(pd);
                }
            }
            return pds;
        }
    }

}
