﻿using System;
using System.Collections.Generic;

namespace NServiceBus
{
    /// <summary>
    /// Used by ConfigUnicastBus to indicate the order in which
    /// handler types are to run.
    /// 
    /// Not thread safe.
    /// </summary>
    /// <typeparam name="T">The type which will run first.</typeparam>
    public class First<T>
    {
        /// <summary>
        /// Specifies the type which will run next.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <returns></returns>
        public static First<T> Then<K>()
        {
            var instance = new First<T>();

            instance.AndThen<T>();
            instance.AndThen<K>();

            return instance;
        }

        /// <summary>
        /// Returns the ordered list of types specified.
        /// </summary>
        public IEnumerable<Type> Types
        {
            get { return types; }
        }

        /// <summary>
        /// Specifies the type which will run next
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <returns></returns>
        public First<T> AndThen<K>()
        {
            if (!types.Contains(typeof(K)))
                types.Add(typeof(K));

            return this;
        }

        private IList<Type> types = new List<Type>();
    }
}
