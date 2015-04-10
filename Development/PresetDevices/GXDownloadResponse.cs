using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gurux.Device.PresetDevices
{
    /// <summary>
    /// Get downloaded profile.
    /// </summary>
    public class GXDownloadResponse
    {
        /// <summary>
        /// Profile data.
        /// </summary>
        [DataMember(IsRequired = true)]
        public byte[] Data
        {
            get;
            set;
        }

        /// <summary>
        /// Downloaded profile.
        /// </summary>
        public GXDeviceProfile Profile
        {
            get;
            set;
        }
    }
}
