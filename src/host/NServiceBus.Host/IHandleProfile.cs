﻿namespace NServiceBus
{
    /// <summary>
    /// Generic abstraction for code which will be called when the given profile is active.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHandleProfile<T> : IHandleProfile where T : IProfile {}

    /// <summary>
    /// Abstraction for code which will be called when the given profile is active.
    /// Implementors should implement IHandleProfile{T} rather than IHandleProfile.
    /// </summary>
    public interface IHandleProfile
    {
        /// <summary>
        /// Called when a given profile is activated.
        /// </summary>
        void ProfileActivated();
    }
}
