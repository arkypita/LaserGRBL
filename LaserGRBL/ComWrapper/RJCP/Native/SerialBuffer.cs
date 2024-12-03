// Copyright © Jason Curl 2012-2023
// Sources at https://github.com/jcurl/SerialPortStream
// Licensed under the Microsoft Public License (Ms-PL)

namespace RJCP.IO.Ports.Native
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using Datastructures;

    /// <summary>
    /// Manages two buffers, for reading and writing, between a Stream and a Native Serial object.
    /// </summary>
    /// <remarks>
    /// This class is used to help implement streams with time out functionality when the actual
    /// object that reads and writes data doesn't support streams, but simply reads and writes
    /// data using low level APIs.
    /// <para>It is to be expected that this object is only used by one reader and one writer. The
    /// reader and writer may be different threads. That is, the properties under SerialData are
    /// accessed only by one thread, the properties under StreamData are accessed only by one
    /// single (but may be different to SerialData) thread.</para>
    /// </remarks>
    internal class SerialBuffer : IDisposable, ISerialBufferSerialData, ISerialBufferStreamData
    {
        private CircularBuffer<byte> m_ReadBuffer;
        private readonly GCHandle m_ReadHandle;
        private readonly object m_ReadLock = new object();
        private readonly ManualResetEvent m_ReadBufferNotEmptyEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent m_ReadBufferNotFullEvent = new ManualResetEvent(true);
        private readonly ManualResetEvent m_ReadEvent = new ManualResetEvent(false);
        private readonly AutoResetEvent m_AbortReadEvent = new AutoResetEvent(false);

        private CircularBuffer<byte> m_WriteBuffer;
        private readonly GCHandle m_WriteHandle;
        private readonly object m_WriteLock = new object();
        private readonly ManualResetEvent m_WriteBufferNotFullEvent = new ManualResetEvent(true);
        private readonly ManualResetEvent m_WriteBufferNotEmptyEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent m_TxEmptyEvent = new ManualResetEvent(true);
        private readonly AutoResetEvent m_AbortWriteEvent = new AutoResetEvent(false);

        private readonly ManualResetEvent m_DeviceDead = new ManualResetEvent(false);

        private readonly bool m_Pinned;

        #region ISerialBufferSerialData
        /// <summary>
        /// Access to properties and methods specific to the native serial object.
        /// </summary>
        /// <value>
        /// Access to properties and methods specific to the native serial object.
        /// </value>
        public ISerialBufferSerialData Serial { get { return this; } }

        CircularBuffer<byte> ISerialBufferSerialData.ReadBuffer { get { return m_ReadBuffer; } }

        IntPtr ISerialBufferSerialData.ReadBufferOffsetEnd
        {
            get
            {
                return !m_Pinned ?
                    IntPtr.Zero :
                    m_ReadHandle.AddrOfPinnedObject() + m_ReadBuffer.End;
            }
        }

        CircularBuffer<byte> ISerialBufferSerialData.WriteBuffer { get { return m_WriteBuffer; } }

        IntPtr ISerialBufferSerialData.WriteBufferOffsetStart
        {
            get
            {
                return !m_Pinned ?
                    IntPtr.Zero :
                    m_WriteHandle.AddrOfPinnedObject() + m_WriteBuffer.Start;
            }
        }

        void ISerialBufferSerialData.ReadBufferProduce(int count)
        {
            lock (m_ReadLock) {
                m_ReadBuffer.Produce(count);
                m_ReadBufferNotEmptyEvent.Set();
                m_ReadEvent.Set();
                if (m_ReadBuffer.Free == 0) {
                    m_ReadBufferNotFullEvent.Reset();
                }
            }
        }

        void ISerialBufferSerialData.WriteBufferConsume(int count)
        {
            lock (m_WriteLock) {
                m_WriteBuffer.Consume(count);
                m_WriteBufferNotFullEvent.Set();
                if (m_WriteBuffer.Length == 0) {
                    m_WriteBufferNotEmptyEvent.Reset();
                }
            }
        }

        bool ISerialBufferSerialData.TxEmptyEvent()
        {
            lock (m_WriteLock) {
                if (m_WriteBuffer.Length == 0) {
                    m_TxEmptyEvent.Set();
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets the event handle that is signalled when the read buffer is not full.
        /// </summary>
        /// <value>
        /// The event handle that is signalled when the read buffer is not full.
        /// </value>
        WaitHandle ISerialBufferSerialData.ReadBufferNotFull
        {
            get { return m_ReadBufferNotFullEvent; }
        }

        /// <summary>
        /// Gets the event handle that is signalled when data is in the write buffer.
        /// </summary>
        /// <value>
        /// The event handle that is signalled when data is in the write buffer.
        /// </value>
        WaitHandle ISerialBufferSerialData.WriteBufferNotEmpty
        {
            get { return m_WriteBufferNotEmptyEvent; }
        }

        /// <summary>
        /// Purges the write buffer.
        /// </summary>
        void ISerialBufferSerialData.Purge()
        {
            lock (m_WriteLock) {
                m_WriteBuffer.Reset();
                m_WriteBufferNotEmptyEvent.Reset();
                m_WriteBufferNotFullEvent.Set();
                m_TxEmptyEvent.Set();
            }
        }

        /// <summary>
        /// Indicates no read/write waits should occur, the device is dead.
        /// </summary>
        void ISerialBufferSerialData.DeviceDead()
        {
            m_DeviceDead.Set();
        }
        #endregion

        #region ISerialBufferStreamData
        /// <summary>
        /// Access to properties and methods specific to the native serial object.
        /// </summary>
        /// <value>
        /// Access to properties and methods specific to the native serial object.
        /// </value>
        public ISerialBufferStreamData Stream { get { return this; } }

        private readonly WaitHandle[] m_BufferStreamWaitForReadHandles;
        private readonly WaitHandle[] m_BufferStreamWaitForReadCountHandles;

        bool ISerialBufferStreamData.WaitForRead(int timeout)
        {
            m_AbortReadEvent.Reset();
            int triggered = WaitHandle.WaitAny(m_BufferStreamWaitForReadHandles, timeout);
            switch (triggered) {
            case WaitHandle.WaitTimeout:
                return false;
            case 0:
                // Data is available to read
                return true;
            case 1:
                // Someone aborted the wait.
                return false;
            case 2:
                // Monitoring thread died. No point waiting any longer.
                return false;
            }
            throw new InternalApplicationException("Unexpected code flow");
        }

        bool ISerialBufferStreamData.WaitForRead(int count, int timeout)
        {
            if (count == 0) return true;
            lock (m_ReadLock) {
                if (count > m_ReadBuffer.Capacity) return false;
            }

            m_AbortReadEvent.Reset();
            TimerExpiry timer = new TimerExpiry(timeout);
            do {
                lock (m_ReadLock) {
                    if (m_ReadBuffer.Length >= count) return true;
                    m_ReadEvent.Reset();
                }

                int triggered = WaitHandle.WaitAny(m_BufferStreamWaitForReadCountHandles, timer.RemainingTime());
                switch (triggered) {
                case WaitHandle.WaitTimeout:
                    break;
                case 0:
                    // Someone aborted the wait.
                    return false;
                case 1:
                    // Data is available to read.
                    return true;
                case 2:
                    // Monitoring thread died. No point waiting any longer.
                    return false;
                }
            } while (!timer.Expired);
            return false;
        }

        int ISerialBufferStreamData.Read(byte[] buffer, int offset, int count)
        {
            lock (m_ReadLock) {
                int bytes = m_ReadBuffer.MoveTo(buffer, offset, count);
                if (m_ReadBuffer.Length == 0) {
                    m_ReadBufferNotEmptyEvent.Reset();
                }
                m_ReadBufferNotFullEvent.Set();
                return bytes;
            }
        }

        void ISerialBufferStreamData.ReadConsume(int count)
        {
            lock (m_ReadLock) {
                m_ReadBuffer.Consume(count);
                if (m_ReadBuffer.Length == 0) {
                    m_ReadBufferNotEmptyEvent.Reset();
                }
                m_ReadBufferNotFullEvent.Set();
            }
        }

        int ISerialBufferStreamData.Read(char[] buffer, int offset, int count, Decoder decoder)
        {
            lock (m_ReadLock) {
                decoder.Convert(m_ReadBuffer, buffer, offset, count, false, out int bu, out int cu, out bool complete);
                if (m_ReadBuffer.Length == 0) {
                    m_ReadBufferNotEmptyEvent.Reset();
                }
                m_ReadBufferNotFullEvent.Set();
                return cu;
            }
        }

        int ISerialBufferStreamData.ReadByte()
        {
            lock (m_ReadLock) {
                if (m_ReadBuffer.Length == 0) return -1;
                int v = m_ReadBuffer[0];
                m_ReadBuffer.Consume(1);
                if (m_ReadBuffer.Length == 0) {
                    m_ReadBufferNotEmptyEvent.Reset();
                }
                m_ReadBufferNotFullEvent.Set();
                return v;
            }
        }

        int ISerialBufferStreamData.BytesToRead
        {
            get
            {
                lock (m_ReadLock) {
                    return m_ReadBuffer.Length;
                }
            }
        }

        void ISerialBufferStreamData.DiscardInBuffer()
        {
            lock (m_ReadLock) {
                m_ReadBuffer.Consume(m_ReadBuffer.Length);
                m_ReadBufferNotFullEvent.Set();
                m_ReadBufferNotEmptyEvent.Reset();
            }
        }

        private readonly WaitHandle[] m_BufferStreamWaitForWriteCountHandles;

        bool ISerialBufferStreamData.WaitForWrite(int count, int timeout)
        {
            if (count == 0) return true;
            lock (m_WriteLock) {
                if (count > m_WriteBuffer.Capacity) return false;
            }

            m_AbortWriteEvent.Reset();
            TimerExpiry timer = new TimerExpiry(timeout);
            do {
                lock (m_WriteLock) {
                    if (m_WriteBuffer.Free >= count) return true;
                    m_WriteBufferNotFullEvent.Reset();
                }
                int triggered = WaitHandle.WaitAny(m_BufferStreamWaitForWriteCountHandles, timer.RemainingTime());
                switch (triggered) {
                case WaitHandle.WaitTimeout:
                    break;
                case 0:
                    // The internal thread died.
                    return false;
                case 1:
                    // Someone aborted the wait.
                    return false;
                case 2:
                    // Data is available to write. Loop to the beginning to see if there is now enough data to write.
                    break;
                }
            } while (!timer.Expired);
            return false;
        }

        void ISerialBufferStreamData.AbortWait()
        {
            m_AbortWriteEvent.Set();
            m_AbortReadEvent.Set();
        }

        int ISerialBufferStreamData.Write(byte[] buffer, int offset, int count)
        {
            lock (m_WriteLock) {
                int bytes = m_WriteBuffer.Append(buffer, offset, count);
                m_WriteBufferNotEmptyEvent.Set();
                m_TxEmptyEvent.Reset();
                if (m_WriteBuffer.Free == 0) {
                    m_WriteBufferNotFullEvent.Reset();
                }
                OnWriteEvent(this, new EventArgs());
                return bytes;
            }
        }

        int ISerialBufferStreamData.BytesToWrite
        {
            get
            {
                lock (m_WriteLock) {
                    return m_WriteBuffer.Length;
                }
            }
        }

        private readonly WaitHandle[] m_BufferStreamFlushHandles;

        bool ISerialBufferStreamData.Flush(int timeout)
        {
            // This manual reset event is always set every time data is removed from the buffer
            m_AbortWriteEvent.Reset();
            int triggered = WaitHandle.WaitAny(m_BufferStreamFlushHandles, timeout);
            switch (triggered) {
            case WaitHandle.WaitTimeout:
                return false;
            case 0:
                // The internal thread died.
                return false;
            case 1:
                // Someone aborted the wait.
                return false;
            case 2:
                // Data is available to write
                return true;
            }
            throw new InternalApplicationException("Unexpected code flow");
        }

        void ISerialBufferStreamData.Reset()
        {
            m_ReadBuffer.Reset();
            m_WriteBuffer.Reset();
            m_ReadBufferNotEmptyEvent.Reset();
            m_ReadBufferNotFullEvent.Set();
            m_ReadEvent.Reset();
            m_AbortReadEvent.Reset();
            m_WriteBufferNotFullEvent.Set();
            m_WriteBufferNotEmptyEvent.Reset();
            m_AbortWriteEvent.Reset();
            m_TxEmptyEvent.Set();
            m_DeviceDead.Reset();
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SerialBuffer"/> class, where the buffer is not pinned.
        /// </summary>
        /// <param name="readBuffer">The read buffer size in bytes.</param>
        /// <param name="writeBuffer">The write buffer size in bytes.</param>
        /// <remarks>
        /// Allocates buffer space for reading and writing (accessible via the <see cref="Serial"/> and <see cref="Stream"/>
        /// properties). The buffers are not pinned, meaning they should not be used for native methods (unless pinned explicitly).
        /// </remarks>
        public SerialBuffer(int readBuffer, int writeBuffer) : this(readBuffer, writeBuffer, false) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerialBuffer"/> class.
        /// </summary>
        /// <param name="readBuffer">The read buffer size in bytes.</param>
        /// <param name="writeBuffer">The write buffer size in bytes.</param>
        /// <param name="pinned">if set to <see langword="true"/> then the read and write buffers are pinned.</param>
        /// <remarks>
        /// Allocates buffer space for reading and writing (accessible via the <see cref="Serial"/> and <see cref="Stream"/>
        /// properties). If specified, the buffers can be pinned. THis causes the buffers to not move for the duration of
        /// the program. The usual warnings apply with pinning buffers, you must be careful to avoid memory problems as
        /// the GC will not be able to reallocate or free space. Note, you must pin the buffers if you intend to use
        /// overlapped I/O.
        /// </remarks>
        public SerialBuffer(int readBuffer, int writeBuffer, bool pinned)
        {
            if (pinned) {
                byte[] read = new byte[readBuffer];
                m_ReadHandle = GCHandle.Alloc(read, GCHandleType.Pinned);
                m_ReadBuffer = new CircularBuffer<byte>(read, 0);

                byte[] write = new byte[writeBuffer];
                m_WriteHandle = GCHandle.Alloc(write, GCHandleType.Pinned);
                m_WriteBuffer = new CircularBuffer<byte>(write, 0);
            } else {
                m_ReadBuffer = new CircularBuffer<byte>(readBuffer);
                m_WriteBuffer = new CircularBuffer<byte>(writeBuffer);
            }
            m_Pinned = pinned;

            // Allocate these conditions once, to reduce the load on the GC

            m_BufferStreamWaitForReadHandles = new WaitHandle[] {
                m_ReadBufferNotEmptyEvent,
                m_AbortReadEvent,
                m_DeviceDead
            };

            m_BufferStreamWaitForReadCountHandles = new WaitHandle[] {
                m_AbortReadEvent,
                m_ReadEvent,
                m_DeviceDead,
            };

            m_BufferStreamWaitForWriteCountHandles = new WaitHandle[] {
                m_DeviceDead,
                m_AbortWriteEvent,
                m_WriteBufferNotFullEvent
            };

            m_BufferStreamFlushHandles = new WaitHandle[] {
                m_DeviceDead,
                m_AbortWriteEvent,
                m_TxEmptyEvent
            };
        }

        /// <summary>
        /// Object to use for locking access to the byte read buffer.
        /// </summary>
        /// <value>
        /// The object to use for locking access to the byte read buffer.
        /// </value>
        public object ReadLock { get { return m_ReadLock; } }

        /// <summary>
        /// Object to use for locking access to the byte write buffer.
        /// </summary>
        /// <value>
        /// The object to use for locking access to the byte write buffer.
        /// </value>
        public object WriteLock { get { return m_WriteLock; } }

        private void OnWriteEvent(object sender, EventArgs args)
        {
            EventHandler handler = WriteEvent;
            if (handler != null) {
                handler(sender, args);
            }
        }

        /// <summary>
        /// Event raised when data is available to write. This could be used to
        /// abort an existing connection.
        /// </summary>
        public event EventHandler WriteEvent;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources;
        /// <see langword="false"/> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            // This is a sealed class, so we have "private void" instead of "protected virtual"
            if (disposing) {
                if (m_Pinned) {
                    // Dispose managed objects here.
                    m_ReadHandle.Free();
                    m_WriteHandle.Free();
                }
                m_ReadBufferNotEmptyEvent.Dispose();
                m_ReadBufferNotFullEvent.Dispose();
                m_ReadEvent.Dispose();
                m_WriteBufferNotFullEvent.Dispose();
                m_WriteBufferNotEmptyEvent.Dispose();
                m_TxEmptyEvent.Dispose();
                m_AbortWriteEvent.Dispose();
                m_AbortReadEvent.Dispose();
                m_DeviceDead.Dispose();
                m_ReadBuffer = null;
                m_WriteBuffer = null;
            }
        }
    }
}
