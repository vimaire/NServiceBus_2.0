using System;
using System.Collections.Generic;

namespace NServiceBus.ObjectBuilder
{
    /// <summary>
    /// Used to instantiate types, so that all configured dependencies
    /// and property values are set.
    /// An abstraction on top of dependency injection frameworks.
    /// </summary>
    public interface IBuilder
    {
        /// <summary>
        /// Creates an instance of the given type, injecting it with all defined dependencies.
        /// </summary>
        /// <param name="typeToBuild"></param>
        /// <returns></returns>
        object Build(Type typeToBuild);

        /// <summary>
        /// Creates an instance of the given type, injecting it with all defined dependencies.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Build<T>();

        /// <summary>
        /// For each type that is compatible with T, an instance is created with all dependencies injected, and yeilded to the caller.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> BuildAll<T>();

        /// <summary>
        /// For each type that is compatible with the given type, an instance is created with all dependencies injected.
        /// </summary>
        /// <param name="typeToBuild"></param>
        /// <returns></returns>
        IEnumerable<object> BuildAll(Type typeToBuild);

        /// <summary>
        /// Builds an instance of the defined type injecting it with all defined dependencies
        /// and invokes the given action on the instance.
        /// </summary>
        /// <param name="typeToBuild"></param>
        /// <param name="action"></param>
        void BuildAndDispatch(Type typeToBuild, Action<object> action);
    }
}
