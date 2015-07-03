﻿namespace NServiceBus.Host.Internal.ProfileHandlers
{
    /// <summary>
    /// Installs and starts MSMQ if necessary.
    /// </summary>
    public class InstallMsmqProfileHandler : IHandleProfile<InstallMsmq>
    {
        void IHandleProfile.ProfileActivated()
        {
            Utils.MsmqInstallation.StartMsmqIfNecessary();
        }
    }
}
