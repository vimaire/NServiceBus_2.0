using System;
using System.Web.UI;

namespace NServiceBus
{
    /// <summary>
    /// Objects of this interface are returned from calling IBus.Send.
    /// The interface allows the caller to register for a callback when a response
    /// is received to their original call to IBus.Send.
    /// </summary>
    public interface ICallback
    {
        /// <summary>
        /// Registers a callback to be invoked when a response arrives to the message sent.
        /// </summary>
        /// <param name="callback">The callback to invoke.</param>
        /// <param name="state">State that will be passed to the callback method.</param>
        /// <returns>An IAsyncResult useful for integration with ASP.NET async tasks.</returns>
        IAsyncResult Register(AsyncCallback callback, object state);

        /// <summary>
        /// Registers a callback to be invoked when a response arrives to the message sent.
        /// Not designed for web scenarios.
        /// </summary>
        /// <param name="callback"></param>
        void Register(Action<int> callback);

        /// <summary>
        /// Registers a callback to be invoked when a response arrives to the message sent.
        /// The return code is casted to the given enumerated type - T.
        /// </summary>
        /// <typeparam name="T">An enumeration type.</typeparam>
        /// <param name="callback"></param>
        void Register<T>(Action<T> callback);

        /// <summary>
        /// Registers a callback to be invoked when a response arrives to the message sent.
        /// Designed specifically for web scenarios so that you don't need to write PageAsyncTasks.
        /// Only works when the callback is on the Page object itself.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        void RegisterWebCallback(Action<int> callback, object state);

        /// <summary>
        /// Registers a callback to be invoked when a response arrives to the message sent.
        /// Designed specifically for web scenarios so that you don't need to write PageAsyncTasks.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <param name="page"></param>
        void RegisterWebCallback(Action<int> callback, object state, Page page);

        /// <summary>
        /// Registers a callback to be invoked when a response arrives to the message sent.
        /// Designed specifically for web scenarios so that you don't need to write PageAsyncTasks.
        /// Only works when the callback is on the Page object itself.
        /// </summary>
        /// <typeparam name="T">An enumeration type.</typeparam>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        void RegisterWebCallback<T>(Action<T> callback, object state);

        /// <summary>
        /// Registers a callback to be invoked when a response arrives to the message sent.
        /// Designed specifically for web scenarios so that you don't need to write PageAsyncTasks.
        /// </summary>
        /// <typeparam name="T">An enumeration type.</typeparam>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <param name="page"></param>
        void RegisterWebCallback<T>(Action<T> callback, object state, Page page);

    }
}
