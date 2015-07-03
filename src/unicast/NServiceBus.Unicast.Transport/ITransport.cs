using System;
using System.Collections.Generic;

namespace NServiceBus.Unicast.Transport
{
	/// <summary>
	/// Defines the basic functionality of a transport to be used by NServiceBus.
	/// </summary>
    public interface ITransport : IDisposable
    {
		/// <summary>
		/// Starts the transport.
		/// </summary>
        void Start();

        /// <summary>
        /// Gets the number of worker threads currently running in the transport.
        /// </summary>
        int NumberOfWorkerThreads { get; }

        /// <summary>
        /// Changes the number of worker threads running in the transport.
        /// This may stop active worker threads; those threads will finish
        /// processing their current message and then exit.
        /// </summary>
        /// <param name="targetNumberOfWorkerThreads">
        /// The requested number of active worker threads after
        /// the necessary threads have been stopped or started.
        /// </param>
	    void ChangeNumberOfWorkerThreads(int targetNumberOfWorkerThreads);

		/// <summary>
		/// Sends a message to the specified destination.
		/// </summary>
		/// <param name="m">The message to send.</param>
		/// <param name="destination">The address to send the message to.</param>
        void Send(TransportMessage m, string destination);

		/// <summary>
		/// Re-queues a message for processing at another time.
		/// </summary>
		/// <param name="m">The message to process later.</param>
        void ReceiveMessageLater(TransportMessage m);

        /// <summary>
        /// Access the underlying technology to get the number of unhandled messages.
        /// </summary>
        /// <returns>The number of pending messages.</returns>
	    int GetNumberOfPendingMessages();

		/// <summary>
		/// Gets the address at which the transport receives messages.
		/// </summary>
        string Address { get; }

		/// <summary>
		/// Raised when a message is received at the transport's <see cref="Address"/>.
		/// </summary>
        event EventHandler<TransportMessageReceivedEventArgs> TransportMessageReceived;

        /// <summary>
        /// Raised when a message is available but before <see cref="TransportMessageReceived"/> is raised.
        /// </summary>
	    event EventHandler StartedMessageProcessing;

        /// <summary>
        /// Raised after message processing was completed, even in case of an exception in message processing.
        /// </summary>
        event EventHandler FinishedMessageProcessing;

        /// <summary>
        /// Raised if an exception was encountered at any point in the processing - including
        /// when the transaction commits.
        /// </summary>
	    event EventHandler FailedMessageProcessing;

        /// <summary>
        /// Causes the current message being handled to return to the queue.
        /// </summary>
	    void AbortHandlingCurrentMessage();
    }
}
