﻿using System;
using NServiceBus.ObjectBuilder;
using NServiceBus.ObjectBuilder.Common;

namespace NServiceBus.ObjectBuilder.Common.Config
{
    /// <summary>
    /// Utility configuration class for implementers of IContainer.
    /// </summary>
    public static class ConfigureCommon
    {
        /// <summary>
        /// Sets the Builder property of the given Configure object to an instance of CommonObjectBuilder.
        /// Then, the given builder object is inserted in the relevant place of the builder chain.
        /// Finally, the given actions are performed on the instance of CommonObjectBuilder.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="container"></param>
        public static void With(Configure config, IContainer container)
        {
            var b = new CommonObjectBuilder { Container = container, Synchronized = SyncConfig.Synchronize };

            config.Builder = b;
            config.Configurer = b;

            var cfg = config.Configurer.ConfigureComponent<CommonObjectBuilder>(ComponentCallModelEnum.Singleton)
                .ConfigureProperty(c => c.Container, container);

            SyncConfig.MarkConfigured();
        }
    }
}
