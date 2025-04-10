using System;

namespace Mediator.Exceptions
{
    /// <summary>
    /// Exception thrown when a request handler is not found
    /// </summary>
    public sealed class HandlerNotFoundException : Exception
    {

        /// <summary>
        /// Gets the type of the request that caused the error
        /// </summary>
        public Type RequestType { get; }

        /// <summary>
        /// Initializes a new instance of the exception
        /// </summary>
        /// <param name="requestType">Type of the request that caused the error</param>
        public HandlerNotFoundException(Type requestType)
            : base($"Handler not found for request type {requestType.Name}")
        {
            RequestType = requestType;
        }


    }
}