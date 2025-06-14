#nullable enable
using System;
using System.Collections.Generic;

namespace Mediator
{
    /// <summary>
    /// Cache for handler type resolution
    /// </summary>
    public class HandlerCache
    {
        private readonly Dictionary<Type, Type> _requestHandlers = new Dictionary<Type, Type>();

        /// <summary>
        /// Adds a request handler type to the cache
        /// </summary>
        /// <param name="requestType">Type of the request</param>
        /// <param name="handlerType">Type of the handler</param>
        public void AddRequestHandler(Type requestType, Type responseType, Type handlerType)
        {
            _requestHandlers[requestType] = handlerType;
        }


        /// <summary>
        /// Gets the handler type for a request
        /// </summary>
        /// <param name="requestType">Type of the request</param>
        /// <returns>Handler type /// or null</returns>
        public Type? GetRequestHandlerType(Type requestType, Type responseType)
        {
            return _requestHandlers.TryGetValue(requestType, out var handlerType)
                ? handlerType
                : null;
        }
    }
}