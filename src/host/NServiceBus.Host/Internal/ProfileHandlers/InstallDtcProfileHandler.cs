﻿namespace NServiceBus.Host.Internal.ProfileHandlers
{
    /// <summary>
    /// Installs the distributed transaction coordinator.
    /// </summary>
    public class InstallDtcProfileHandler : IHandleProfile<InstallDtc>
    {
        void IHandleProfile.ProfileActivated()
        {
            Utils.DtcUtil.StartDtcIfNecessary();
        }
    }
}
