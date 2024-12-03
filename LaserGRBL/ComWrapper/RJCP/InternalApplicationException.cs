namespace RJCP.IO.Ports
{
    using System;
#if NETFRAMEWORK
    using System.Runtime.Serialization;
#endif

    /// <summary>
    /// Indicates an unexpected internal error occurred in the application.
    /// </summary>
    /// <remarks>
    /// The InternalApplicationError indicates a programming error in the application. Such an
    /// error should never occur and always indicates a bug in the application itself.
    /// </remarks>
#if NETFRAMEWORK
    [Serializable]
#endif
    internal class InternalApplicationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InternalApplicationException"/> class.
        /// </summary>
        public InternalApplicationException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalApplicationException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InternalApplicationException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalApplicationException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a <see langword="null"/> reference (Nothing in
        /// Visual Basic) if no inner exception is specified.
        /// </param>
        public InternalApplicationException(string message, Exception innerException) : base(message, innerException) { }

#if NETFRAMEWORK
        /// <summary>
        /// Initializes a new instance of the <see cref="InternalApplicationException"/> class.
        /// </summary>
        /// <param name="info">
        /// The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext"/> that contains contextual information about the source or destination.
        /// </param>
        protected InternalApplicationException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
#endif
    }
}
