namespace RJCP.IO.Ports.Native
{
    using System;
    using System.Threading;
    using Datastructures;

    /// <summary>
    /// Container structure for properties and methods related to the Native Serial object.
    /// </summary>
    internal interface ISerialBufferSerialData
    {
        /// <summary>
        /// Access the read buffer directly.
        /// </summary>
        /// <value>
        /// The read buffer.
        /// </value>
        CircularBuffer<byte> ReadBuffer { get; }

        /// <summary>
        /// Gets the read buffer offset end, used for giving to native API's.
        /// </summary>
        /// <value>
        /// The read buffer offset end.
        /// </value>
        IntPtr ReadBufferOffsetEnd { get; }

        /// <summary>
        /// Access the write buffer directly.
        /// </summary>
        /// <value>
        /// The write buffer.
        /// </value>
        CircularBuffer<byte> WriteBuffer { get; }

        /// <summary>
        /// Gets the write buffer offset start, used for giving to native API's.
        /// </summary>
        /// <value>
        /// The write buffer offset start.
        /// </value>
        IntPtr WriteBufferOffsetStart { get; }

        /// <summary>
        /// Update the read circular queue indicating data has been received, and is available to the stream object.
        /// </summary>
        /// <param name="count">The count.</param>
        void ReadBufferProduce(int count);

        /// <summary>
        /// Update the write circular queue indicating data has been written, freeing space for more data to write.
        /// </summary>
        /// <param name="count">The count.</param>
        void WriteBufferConsume(int count);

        /// <summary>
        /// Indicates that the write buffer is now empty.
        /// </summary>
        /// <returns><c>true</c> if the write buffer is empty.</returns>
        /// <remarks>
        /// Systems that return immediately after a Write before the write has been sent over the wire
        /// should call this method when the hardware indicates that data is flushed.
        /// <para>Systems that only return from the Write when data is completely written, or that do not
        /// receive events when the hardware buffer is empty, should call this method immediately after
        /// the Write is done.</para><para>Typically Windows systems may return before the hardware buffer is completely empty,
        /// where they notify later of an empty buffer with the EV_TXEMPTY event.</para>
        /// </remarks>
        bool TxEmptyEvent();

        /// <summary>
        /// Gets a value indicating whether this instance uses a pinned buffer.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance uses a pinned buffer; otherwise, <c>false</c>.
        /// </value>
        bool IsPinnedBuffer { get; }

        /// <summary>
        /// Gets the event handle that is signalled when the read buffer is not full.
        /// </summary>
        /// <value>
        /// The event handle that is signalled when the read buffer is not full.
        /// </value>
        WaitHandle ReadBufferNotFull { get; }

        /// <summary>
        /// Gets the event handle that is signalled when data is in the write buffer.
        /// </summary>
        /// <value>
        /// The event handle that is signalled when data is in the write buffer.
        /// </value>
        WaitHandle WriteBufferNotEmpty { get; }

        /// <summary>
        /// Purges the write buffer.
        /// </summary>
        void Purge();

        /// <summary>
        /// Indicates no read/write waits should occur, the device is dead.
        /// </summary>
        void DeviceDead();
    }
}
