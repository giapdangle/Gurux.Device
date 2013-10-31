using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Gurux.Device
{
    /// <summary>
    /// Enumerates not needed keepalive fields.
    /// </summary>
    [DataContract()]
    [Flags]
    public enum KeepaliveFieldsIgnored : int
    {
        /// <summary>
        /// Nothing is hidden.
        /// </summary>
        [EnumMember(Value = "0")]
        None = 0,
        /// <summary>
        /// Interval is not needed.
        /// </summary>
        [EnumMember(Value = "1")]
        Interval = 0x1,
        /// <summary>
        /// Target is not needed.
        /// </summary>
        [EnumMember(Value = "2")]
        Target = 0x2,
        /// <summary>
        /// TransactionResets is not needed.
        /// </summary>        
        [EnumMember(Value = "4")]
        Reset = 0x4
    }
}
