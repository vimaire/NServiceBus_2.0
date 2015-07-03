﻿using System;
using NServiceBus.Unicast.Transport;
using System.Collections.Specialized;

namespace NServiceBus.Gateway
{
    public class HeaderMapper
    {
        public static void Map(NameValueCollection from, TransportMessage to)
        {
            to.Id = from[NServiceBus + Id];
            to.IdForCorrelation = from[NServiceBus + IdForCorrelation];
            to.CorrelationId = from[NServiceBus + CorrelationId];

            bool recoverable;
            bool.TryParse(from[NServiceBus + Recoverable], out recoverable);
            to.Recoverable = recoverable;

            TimeSpan timeToBeReceived;
            TimeSpan.TryParse(from[NServiceBus + TimeToBeReceived], out timeToBeReceived);
            to.TimeToBeReceived = timeToBeReceived;

            to.WindowsIdentityName = from[NServiceBus + WindowsIdentityName];

            to.Headers = new System.Collections.Generic.List<HeaderInfo>();
            foreach (string header in from.Keys)
                if (header.Contains(NServiceBus + Header))
                    to.Headers.Add(new HeaderInfo { Key = header.Replace(NServiceBus + Header, ""), Value = from[header] });
        }

        public static void Map(TransportMessage from, NameValueCollection to)
        {
            to[NServiceBus + Id] = from.Id;
            to[NServiceBus + IdForCorrelation] = from.IdForCorrelation;
            to[NServiceBus + CorrelationId] = from.CorrelationId;
            to[NServiceBus + Recoverable] = from.Recoverable.ToString();
            to[NServiceBus + TimeToBeReceived] = from.TimeToBeReceived.ToString();
            to[NServiceBus + WindowsIdentityName] = from.WindowsIdentityName;

            to[NServiceBus + ReturnAddress] = from.ReturnAddress;
            to[NServiceBus + Header + ReturnAddress] = from.ReturnAddress;

            var header = from.Headers.Find(hi => hi.Key == ReturnAddress);
            if (header != null)
                to[NServiceBus + Header + RouteTo] = header.Value;

            from.Headers.ForEach(info => to[NServiceBus + Header + info.Key] = info.Value);
        }

        private const string NServiceBus = "NServiceBus.";
        private const string Id = "Id";
        private const string IdForCorrelation = "IdForCorrelation";
        private const string CorrelationId = "CorrelationId";
        private const string Recoverable = "Recoverable";
        private const string ReturnAddress = "ReturnAddress";
        private const string TimeToBeReceived = "TimeToBeReceived";
        private const string WindowsIdentityName = "WindowsIdentityName";
        private const string Header = "Header.";

        public const string RouteTo = "RouteTo";
    }

    public static class Headers
    {
        public const string ContentMd5Key = "Content-MD5";
        public const string FromKey = "From";
    }
}
