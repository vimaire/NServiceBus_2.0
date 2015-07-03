﻿using System;
using System.Collections.Generic;

namespace NServiceBus
{
    /// <summary>
    /// Contains out-of-band information on the logical message.
    /// </summary>
    public interface IMessageContext
    {
        /// <summary>
        /// Returns the Id of the message.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Returns the address of the endpoint that sent this message.
        /// </summary>
        string ReturnAddress { get; }

        /// <summary>
        /// Gets the list of key/value pairs found in the header of the message.
        /// </summary>
        IDictionary<string, string> Headers { get; }
    }
}
