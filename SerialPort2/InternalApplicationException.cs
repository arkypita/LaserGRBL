namespace RJCP.IO.Ports
{
    using System;

    /// <summary>
    /// Indicates an unexpected internal error occurred in the application.
    /// </summary>
    /// <remarks>
    /// The InternalApplicationError indicates a programming error in the application. Such an
    /// error should never occur and always indicates a bug in the application itself.
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S3871:Exception types should be \"public\"",
        Justification = "Exception should never occur, so should be internal")]
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
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public InternalApplicationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
