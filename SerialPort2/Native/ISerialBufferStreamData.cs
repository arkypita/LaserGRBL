namespace RJCP.IO.Ports.Native
{
    using System.Text;

    /// <summary>
    /// Container structure for properties and methods related to the Stream object.
    /// </summary>
    internal interface ISerialBufferStreamData
    {
        /// <summary>
        /// Waits up to a specified time out for data to be available to read.
        /// </summary>
        /// <param name="timeout">The time out in milliseconds.</param>
        /// <returns><c>true</c> if data is available to read in time; <c>false</c> otherwise.</returns>
        bool WaitForRead(int timeout);

        /// <summary>
        /// Waits up to a specified time out for data to be available to read.
        /// </summary>
        /// <param name="count">The number of bytes that should be in the read buffer.</param>
        /// <param name="timeout">The time out in milliseconds.</param>
        /// <returns><c>true</c> if data is available to read in time; <c>false</c> otherwise.</returns>
        bool WaitForRead(int count, int timeout);

        /// <summary>
        /// Reads data received by the Serial object, copying it into a buffer and reducing the read buffer size.
        /// </summary>
        /// <param name="buffer">The buffer to copy data into.</param>
        /// <param name="offset">The offset where to copy data into..</param>
        /// <param name="count">The number of bytes to copy.</param>
        /// <returns>Number of bytes actually read from the queue.</returns>
        int Read(byte[] buffer, int offset, int count);

        /// <summary>
        /// Consume bytes from the incoming buffer.
        /// </summary>
        /// <param name="count">The number of bytes to discard at the beginning of the read byte buffer.</param>
        void ReadConsume(int count);

        /// <summary>
        /// Reads data received by the serial object, converted to characters using the specified decoder.
        /// </summary>
        /// <param name="buffer">The character buffer to read into.</param>
        /// <param name="offset">The offset into <paramref name="buffer"/>.</param>
        /// <param name="count">The number of characters to write into <paramref name="buffer"/>.</param>
        /// <param name="decoder">The decoder to use for the conversion.</param>
        /// <returns>The number of characters read.</returns>
        /// <remarks>
        /// This method has no input checks that it is internal
        /// </remarks>
        int Read(char[] buffer, int offset, int count, Decoder decoder);

        /// <summary>
        /// Reads a single byte from the input queue.
        /// </summary>
        /// <returns>The byte, cast to an Int32, or -1 if the end of the stream has been read.</returns>
        int ReadByte();

        /// <summary>
        /// Gets the number of bytes in the internal read buffer only.
        /// </summary>
        /// <remarks>
        /// This value is independent of the actual number of bytes in the serial port
        /// hardware buffer. It will only return that which is currently obtained by
        /// the I/O thread.
        /// </remarks>
        int BytesToRead { get; }

        /// <summary>
        /// Discards data from the receive buffer.
        /// </summary>
        void DiscardInBuffer();

        /// <summary>
        /// Waits up to a specified time out for enough data to be free in the write buffer.
        /// </summary>
        /// <param name="count">The number of bytes required to be free.</param>
        /// <param name="timeout">The time out in milliseconds.</param>
        /// <returns><c>true</c> if <paramref name="count"/> bytes are available for writing to the buffer;
        /// <c>false</c> if there is not enough buffer available within the <paramref name="timeout"/>
        /// parameter. If <paramref name="count"/> is larger than the capacity of the buffer, <c>false</c>
        /// is returned immediately.</returns>
        bool WaitForWrite(int count, int timeout);

        /// <summary>
        /// Aborts the wait for write.
        /// </summary>
        void AbortWait();

        /// <summary>
        /// Puts data into the write buffer for the Serial object to send.
        /// </summary>
        /// <param name="buffer">The buffer to copy data from.</param>
        /// <param name="offset">The offset where to copy the data from.</param>
        /// <param name="count">The number of bytes to copy.</param>
        /// <returns>Number of bytes actually written to the queue.</returns>
        int Write(byte[] buffer, int offset, int count);

        /// <summary>
        /// Gets the number of bytes in the internal write buffer only.
        /// </summary>
        /// <remarks>
        /// This value is independent of the actual number of bytes in the serial port
        /// hardware buffer. It will only return that which is currently not completely written
        /// by the I/O thread.
        /// </remarks>
        int BytesToWrite { get; }

        /// <summary>
        /// Waits for all data in the write buffer to be written with notification also by the serial buffer.
        /// </summary>
        /// <param name="timeout">The time out in milliseconds.</param>
        /// <returns><c>true</c> if the data was flushed within the specified time out; <c>false</c> otherwise.</returns>
        bool Flush(int timeout);

        /// <summary>
        /// Reset the buffer to an initial state ready for a new connection.
        /// </summary>
        /// <param name="clearBuffer">Set to <c>true</c> to reset the contents of the buffers.</param>
        /// <remarks>
        /// This should only be closed when the serial port is closed.
        /// </remarks>
        void Reset(bool clearBuffer);
    }
}
