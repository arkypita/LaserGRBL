namespace System
{
    /// <summary>
    /// The exception that is thrown when an attempt to load a class fails due to the absence of an entry method.
    /// </summary>
    /// <remarks>
    /// This method is intended as a drop in replacement for the equivalent exception in .NET.
    /// Unfortunately, .NET Standard 1.6 and earlier do not contain this exception, even though
    /// P/Invoke on Windows certainly raises this exception.
    /// </remarks>
    internal class EntryPointNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntryPointNotFoundException"/> class.
        /// </summary>
        public EntryPointNotFoundException()
            : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntryPointNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public EntryPointNotFoundException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntryPointNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a <see langword="null"/> reference (Nothing in
        /// Visual Basic) if no inner exception is specified.
        /// </param>
        public EntryPointNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
