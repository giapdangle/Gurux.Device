using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gurux.Common.JSon;
using Gurux.Common;

namespace Gurux.Device.PresetDevices
{
    /// <summary>
    /// Download selected template from Gurux server.
    /// </summary>
    public class GXDownloadRequest : IGXRequest<GXDownloadResponse>
    {
        /// <summary>
        /// Profile to download.
        /// </summary>
        public Guid Profile
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="profile">Device profile to download.</param>
        public GXDownloadRequest(GXDeviceProfile profile)
        {
            Profile = profile.ProfileGuid;
        }
    }
}
