using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gurux.Device.PresetDevices
{
    /// <summary>
    /// Download state.
    /// </summary>
    [DataContract()]
    [Flags]
    public enum DownloadStates
    {
        /// <summary>
        /// Version is update to installed one.
        /// </summary>
        [EnumMember(Value = "0")]
        None = 0x0,
        /// <summary>
        /// Item is marked as downloaded.
        /// </summary>
        [EnumMember(Value = "2")]
        Add = 0x2,
        /// <summary>
        /// Item is removed.
        /// </summary>
        [EnumMember(Value = "4")]
        Remove = 0x4,
        /// <summary>
        /// Item is installed.
        /// </summary>
        [EnumMember(Value = "8")]
        Installed = 0x8
    }
}