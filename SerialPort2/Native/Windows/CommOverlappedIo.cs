// Copyright © Jason Curl 2012-2018
// Sources at https://github.com/jcurl/SerialPortStream
// Licensed under the Microsoft Public License (Ms-PL)

//#define STRESSTEST
#define PL2303_WORKAROUNDS

namespace RJCP.IO.Ports.Native.Windows
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using Datastructures;
    using Microsoft.Win32.SafeHandles;
    using Trace;

    internal class CommOverlappedIo : IDisposable
    {
        #region Local variables
        /// <summary>
        /// Handle to the already opened COM Port.
        /// </summary>
        private readonly SafeFileHandle m_ComPortHandle;

        /// <summary>
        /// The OverlappedIoThread.
        /// </summary>
        private Thread m_Thread;

        /// <summary>
        /// Read and Write buffers that are pinned.
        /// </summary>
        private SerialBuffer m_Buffer;

        /// <summary>
        /// Event to abort OverlappedIoThread (for OverlappedIoThread).
        /// </summary>
        private readonly ManualResetEvent m_StopRunning = new ManualResetEvent(false);

        /// <summary>
        /// Overlapped I/O for WaitCommEvent() finished (for OverlappedIoThread).
        /// </summary>
        private readonly ManualResetEvent m_SerialCommEvent = new ManualResetEvent(false);

        /// <summary>
        /// Overlapped I/O for ReadFile() finished (for OverlappedIoThread).
        /// </summary>
        private readonly ManualResetEvent m_ReadEvent = new ManualResetEvent(false);

        /// <summary>
        /// Overlapped I/O for WriteFile() finished (for OverlappedIoThread).
        /// </summary>
        private readonly ManualResetEvent m_WriteEvent = new ManualResetEvent(false);

        /// <summary>
        /// Triggered to indicate to purge data in the write buffer by the OverlappedIO thread.
        /// </summary>
        private readonly AutoResetEvent m_WriteClearEvent = new AutoResetEvent(false);

        /// <summary>
        /// Triggered when the purge is complete from the OverlappedIO thread.
        /// </summary>
        private readonly AutoResetEvent m_WriteClearDoneEvent = new AutoResetEvent(false);

        /// <summary>
        /// Used by the OverlappedIO thread to finalise a purge of the write buffer if a write
        /// operation was previously pending.
        /// </summary>
        private bool m_PurgePending;

        /// <summary>
        /// Indicates if the OverlappedIO thread is running.
        /// </summary>
        private volatile bool m_IsRunning;

        /// <summary>
        /// Indicates that there is a byte available for reading.
        /// </summary>
        /// <remarks>
        /// The WaitCommEvent() method indicates if a byte has been received (EV_RXCHAR). This
        /// is cleared when the read operation is finished.
        /// </remarks>
        private bool m_ReadByteAvailable;

        /// <summary>
        /// Indicates that there is a EOF character that has arrived.
        /// </summary>
        /// <remarks>
        /// The WaitCommEvent() method indicates if a byte has been received (RX_CHAR) covered
        /// by the variable m_ReadByteAvailable. If m_ReadByteAvailable, this indicates if a
        /// the EOF character has arrived (EV_RXFLAG).
        /// </remarks>
        private bool m_ReadByteEof;

        private string m_Name;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CommOverlappedIo"/> class.
        /// </summary>
        /// <param name="handle">The serial port handle.</param>
        public CommOverlappedIo(SafeFileHandle handle)
        {
            m_ComPortHandle = handle;
        }
        #endregion

        #region Buffer Management
        /// <summary>
        /// Gets the number of bytes in the driver queue still to be read.
        /// </summary>
        /// <value>
        /// The bytes to read.
        /// </value>
        public int BytesToRead
        {
            get
            {
                if (!IsRunning) return 0;

                if (GetReceiveStats(out uint bytesInRecvQueue, out _)) {
                    return (int)bytesInRecvQueue;
                }
                return 0;
            }
        }

        /// <summary>
        /// Gets the status of bytes received by the serial provider that haven't been read yet. If there is
        /// a failure in obtaining the information, zero is returned.
        /// </summary>
        /// <param name="bytesInRecvQueue">Output indicating number of bytes in queue but not read by ReadFile.</param>
        /// <param name="eofReceived">Output indicating whether an EOF character was received.</param>
        /// <returns>true if the stats were received, otherwise false.</returns>
        /// <remarks>
        /// Getting this information has the side effect of processing and clearing any serial port
        /// errors and firing CommErrorEvent.
        /// </remarks>
        private bool GetReceiveStats(out uint bytesInRecvQueue, out bool eofReceived)
        {
            bytesInRecvQueue = 0;
            eofReceived = false;
            lock (m_Buffer.ReadLock) {
                NativeMethods.COMSTAT comStat = new NativeMethods.COMSTAT();
                bool result = UnsafeNativeMethods.ClearCommError(m_ComPortHandle, out NativeMethods.ComStatErrors cErr, ref comStat);
                if (!result) {
                    int w32err = Marshal.GetLastWin32Error();
                    int hr = Marshal.GetHRForLastWin32Error();
                    if (Log.SerialTrace(System.Diagnostics.TraceEventType.Error))
                        Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Error, 0,
                            "{0}: SerialThread: BytesInSerialQueue: ClearCommError() error {1}", m_Name, w32err);
                    Marshal.ThrowExceptionForHR(hr);
                    return false;
                }
                if (cErr != 0) OnCommErrorEvent(new CommErrorEventArgs(cErr));
                bytesInRecvQueue = comStat.cbInQue;
                eofReceived = ((comStat.Flags & NativeMethods.ComStatFlags.Eof) == NativeMethods.ComStatFlags.Eof);
                return true;
            }
        }

        /// <summary>
        /// Gets the number bytes to of data in the transmit buffer.
        /// </summary>
        /// <value>
        /// The number of bytes in the transmit buffer.
        /// </value>
        public int BytesToWrite
        {
            get
            {
                NativeMethods.COMSTAT comStat = new NativeMethods.COMSTAT();
                bool result = UnsafeNativeMethods.ClearCommError(m_ComPortHandle, out _, ref comStat);
                if (result) return (int)comStat.cbOutQue;
                return 0;
            }
        }

        /// <summary>
        /// Discards data from the serial driver's transmit buffer.
        /// </summary>
        /// <remarks>
        /// This function will discard the receive buffer of the SerialPortStream.
        /// </remarks>
        public void DiscardOutBuffer()
        {
            if (!IsRunning) {
                lock (m_Buffer.WriteLock) {
                    m_Buffer.Serial.Purge();
                }
            } else {
                m_WriteClearEvent.Set();
                m_WriteClearDoneEvent.WaitOne(Timeout.Infinite);
            }
        }
        #endregion

        #region Thread Control
        /// <summary>
        /// Start the I/O thread.
        /// </summary>
        public void Start(SerialBuffer buffer, string name)
        {
            m_Buffer = buffer;
            m_Name = name;
            m_IsRunning = true;
            try {
                // Set the time outs
                NativeMethods.COMMTIMEOUTS timeouts = new NativeMethods.COMMTIMEOUTS() {
                    // We read only the data that is buffered
#if PL2303_WORKAROUNDS
                    // Time out if data hasn't arrived in 10ms, or if the read takes longer than 100ms in total
                    ReadIntervalTimeout = 10,
                    ReadTotalTimeoutConstant = 100,
                    ReadTotalTimeoutMultiplier = 0,
#else
                    // Non-asynchronous behaviour
                    ReadIntervalTimeout = System.Threading.Timeout.Infinite,
                    ReadTotalTimeoutConstant = 0,
                    ReadTotalTimeoutMultiplier = 0,
#endif
                    // We have no time outs when writing
                    WriteTotalTimeoutMultiplier = 0,
                    WriteTotalTimeoutConstant = 500
                };

                bool result = UnsafeNativeMethods.SetCommTimeouts(m_ComPortHandle, ref timeouts);
                if (!result) throw new IOException("Couldn't set CommTimeouts", Marshal.GetLastWin32Error());

                m_Thread = new Thread(new ThreadStart(OverlappedIoThread)) {
                    Name = "SerialPortStream_" + m_Name,
                    IsBackground = true
                };
                m_Thread.Start();
            } catch {
                m_IsRunning = false;
                throw;
            }
        }

        /// <summary>
        /// Cancel pending I/O, stop the I/O thread, wait and then return.
        /// </summary>
        private void Stop()
        {
            if (m_Thread != null) {
                if (Log.SerialTrace(System.Diagnostics.TraceEventType.Verbose))
                    Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0, "{0}: OverlappedIO: Stopping Thread", m_Name);
                m_StopRunning.Set();
                if (Log.SerialTrace(System.Diagnostics.TraceEventType.Verbose))
                    Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0, "{0}: OverlappedIO: Waiting for Thread", m_Name);
                m_Thread.Join();
                if (Log.SerialTrace(System.Diagnostics.TraceEventType.Verbose))
                    Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0, "{0}: OverlappedIO: Thread Stopped", m_Name);
                m_Thread = null;
            }
        }

        /// <summary>
        /// Test if the I/O thread is running.
        /// </summary>
        public bool IsRunning { get { return m_IsRunning; } }
        #endregion

        private const NativeMethods.SerialEventMask maskRead =
            NativeMethods.SerialEventMask.EV_BREAK |
            NativeMethods.SerialEventMask.EV_CTS |
            NativeMethods.SerialEventMask.EV_DSR |
            NativeMethods.SerialEventMask.EV_ERR |
            NativeMethods.SerialEventMask.EV_RING |
            NativeMethods.SerialEventMask.EV_RLSD |
            NativeMethods.SerialEventMask.EV_RXCHAR |
            NativeMethods.SerialEventMask.EV_TXEMPTY |
            NativeMethods.SerialEventMask.EV_EVENT1 |
            NativeMethods.SerialEventMask.EV_EVENT2 |
            NativeMethods.SerialEventMask.EV_PERR |
            NativeMethods.SerialEventMask.EV_RX80FULL |
            NativeMethods.SerialEventMask.EV_RXFLAG;

        private const NativeMethods.SerialEventMask maskReadPending =
            NativeMethods.SerialEventMask.EV_BREAK |
            NativeMethods.SerialEventMask.EV_CTS |
            NativeMethods.SerialEventMask.EV_DSR |
            NativeMethods.SerialEventMask.EV_ERR |
            NativeMethods.SerialEventMask.EV_RING |
            NativeMethods.SerialEventMask.EV_RLSD |
            NativeMethods.SerialEventMask.EV_TXEMPTY |
            NativeMethods.SerialEventMask.EV_EVENT1 |
            NativeMethods.SerialEventMask.EV_EVENT2 |
            NativeMethods.SerialEventMask.EV_PERR;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Runtime.InteropServices.SafeHandle.DangerousGetHandle")]
        private void OverlappedIoThread()
        {
            try {
                OverlappedIoThreadMainLoop();
            } catch (Exception ex) {
                if (Log.SerialTrace(System.Diagnostics.TraceEventType.Error))
                    Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Error, 0,
                    "{0}: SerialThread: Died from {1}", m_Name, ex.Message);
            } finally {
                m_IsRunning = false;

                // Clear the write buffer. Anything that's still in the driver serial buffer will continue to write. The I/O was cancelled
                // so no need to purge the actual driver itself.
                m_Buffer.Serial.Purge();

                // We must notify the stream that any blocking waits should abort.
                m_Buffer.Serial.DeviceDead();
            }
        }

        private void OverlappedIoThreadMainLoop()
        {
            // WaitCommEvent
            bool serialCommPending = false;
            bool serialCommError = false;
            m_SerialCommEvent.Reset();
            NativeOverlapped serialCommOverlapped = new NativeOverlapped();
#if NETSTANDARD15
            serialCommOverlapped.EventHandle = m_SerialCommEvent.GetSafeWaitHandle().DangerousGetHandle();
#else
            serialCommOverlapped.EventHandle = m_SerialCommEvent.SafeWaitHandle.DangerousGetHandle();
#endif
            // ReadFile
            bool readPending = false;
            m_ReadEvent.Reset();
            NativeOverlapped readOverlapped = new NativeOverlapped();
#if NETSTANDARD15
            readOverlapped.EventHandle = m_ReadEvent.GetSafeWaitHandle().DangerousGetHandle();
#else
            readOverlapped.EventHandle = m_ReadEvent.SafeWaitHandle.DangerousGetHandle();
#endif
            // WriteFile
            bool writePending = false;
            m_WriteEvent.Reset();
            NativeOverlapped writeOverlapped = new NativeOverlapped();
            m_ReadByteAvailable = false;
#if NETSTANDARD15
            writeOverlapped.EventHandle = m_WriteEvent.GetSafeWaitHandle().DangerousGetHandle();
#else
            writeOverlapped.EventHandle = m_WriteEvent.SafeWaitHandle.DangerousGetHandle();
#endif
            // SEt up the types of serial events we want to see.
            UnsafeNativeMethods.SetCommMask(m_ComPortHandle, maskRead);

            bool result;
            NativeMethods.SerialEventMask commEventMask = 0;

            bool running = true;
            uint bytes;
            ReusableList<WaitHandle> handles = new ReusableList<WaitHandle>(2, 7);

            while (running) {
                handles.Clear();
                handles.Add(m_StopRunning);
                handles.Add(m_WriteClearEvent);

#if PL2303_WORKAROUNDS
                // - - - - - - - - - - - - - - - - - - - - - - - - -
                // PROLIFIC PL23030 WORKAROUND
                // - - - - - - - - - - - - - - - - - - - - - - - - -
                // If we have a read pending, we don't request events
                // for reading data. To do so will result in errors.
                // Have no idea why.
                if (readPending) {
                    UnsafeNativeMethods.SetCommMask(m_ComPortHandle, maskReadPending);
                } else {
                    UnsafeNativeMethods.SetCommMask(m_ComPortHandle, maskRead);

                    // While the comm event mask was set to ignore read events, data could have been written
                    // to the input queue. Check for that and if there are bytes waiting or EOF was received,
                    // set the appropriate flags.
                    if (GetReceiveStats(out uint bytesInQueue, out bool eofReceived) && (bytesInQueue > 0 || eofReceived)) {
                        // Tell DoReadEvent that there is data pending
                        m_ReadByteAvailable = true;
                        m_ReadByteEof |= eofReceived;
                    }
                }
#else
                UnsafeNativeMethods.SetCommMask(m_ComPortHandle, maskRead);
#endif

                // commEventMask is on the stack, and is therefore fixed
                if (!serialCommError) {
                    try {
                        if (!serialCommPending)
                            serialCommPending = DoWaitCommEvent(out commEventMask, ref serialCommOverlapped);
                        if (serialCommPending)
                            handles.Add(m_SerialCommEvent);
                    } catch (IOException) {
                        // Some devices, such as the Arduino Uno with a CH340 on board don't support an overlapped
                        // WaitCommEvent. So if that occurs, we remember it and don't use it again. The Windows error
                        // returned was 87 (ERROR_INVALID_PARAMETER) was returned in that case. GetReceiveStats() did
                        // work, so we can still know of data pending by polling. But we won't get any other events,
                        // such as TX_EMPTY.
                        if (Log.SerialTrace(System.Diagnostics.TraceEventType.Warning))
                            Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Warning, 0,
                                "{0}: SerialThread: Not processing WaitCommEvent events", m_Name);
                        serialCommError = true;
                    }
                }

                if (!readPending) {
                    if (!m_Buffer.Serial.ReadBufferNotFull.WaitOne(0)) {
                    if (Log.SerialTrace(System.Diagnostics.TraceEventType.Verbose))
                        Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0, "{0}: SerialThread: Read Buffer Full", m_Name);
                    handles.Add(m_Buffer.Serial.ReadBufferNotFull);
                    } else {
                        readPending = DoReadEvent(ref readOverlapped);
                    }
                }
                if (readPending) handles.Add(m_ReadEvent);

                if (!writePending) {
                    if (!m_Buffer.Serial.WriteBufferNotEmpty.WaitOne(0)) {
                        handles.Add(m_Buffer.Serial.WriteBufferNotEmpty);
                    } else {
                        writePending = DoWriteEvent(ref writeOverlapped);
                    }
                }
                if (writePending) handles.Add(m_WriteEvent);

                // We wait up to 100ms, in case we're not actually pending on anything. Normally, we should always be
                // pending on a Comm event. Just in case this is not so (and is a theoretical possibility), we will
                // slip out of this WaitAny() after 100ms and then restart the loop, effectively polling every 100ms in
                // worst case.
                WaitHandle[] whandles = handles.ToArray();
                int ev = WaitHandle.WaitAny(whandles, 100);

                if (ev != WaitHandle.WaitTimeout) {
                    if (whandles[ev] == m_StopRunning) {
                        if (Log.SerialTrace(System.Diagnostics.TraceEventType.Verbose))
                            Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0, "{0}: SerialThread: Thread closing", m_Name);
                        result = UnsafeNativeMethods.CancelIo(m_ComPortHandle);
                        if (!result) {
                            int win32Error = Marshal.GetLastWin32Error();
                            int hr = Marshal.GetHRForLastWin32Error();
                            if (Log.SerialTrace(System.Diagnostics.TraceEventType.Warning))
                                Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Warning, 0,
                                    "{0}: SerialThread: CancelIo error {1}", m_Name, win32Error);
                            Marshal.ThrowExceptionForHR(hr);
                        }
                        running = false;
                    } else if (whandles[ev] == m_SerialCommEvent) {
                        result = UnsafeNativeMethods.GetOverlappedResult(m_ComPortHandle, ref serialCommOverlapped, out bytes, true);
                        if (!result) {
                            int win32Error = Marshal.GetLastWin32Error();
                            int hr = Marshal.GetHRForLastWin32Error();
                            if (Log.SerialTrace(System.Diagnostics.TraceEventType.Error))
                                Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Error, 0,
                                    "{0}: SerialThread: Overlapped WaitCommEvent() error {1}", m_Name, win32Error);
                            Marshal.ThrowExceptionForHR(hr);
                        }
                        ProcessWaitCommEvent(commEventMask);
                        serialCommPending = false;
                    } else if (whandles[ev] == m_ReadEvent) {
                        result = UnsafeNativeMethods.GetOverlappedResult(m_ComPortHandle, ref readOverlapped, out bytes, true);
                        if (!result) {
                            int win32Error = Marshal.GetLastWin32Error();
                            int hr = Marshal.GetHRForLastWin32Error();
                            // Should never get ERROR_IO_PENDING, as this method is only called when the event is triggered.
                            if (win32Error != WinError.ERROR_OPERATION_ABORTED || bytes > 0) {
                                if (Log.SerialTrace(System.Diagnostics.TraceEventType.Error))
                                    Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Error, 0,
                                        "{0}: SerialThread: Overlapped ReadFile() error {1} bytes {2}", m_Name, win32Error, bytes);
                                Marshal.ThrowExceptionForHR(hr);
                            } else {
                                // ERROR_OPERATION_ABORTED may be caused by CancelIo or PurgeComm
                                if (Log.SerialTrace(System.Diagnostics.TraceEventType.Verbose))
                                    Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0,
                                        "{0}: SerialThread: Overlapped ReadFile() error {1} bytes {2}", m_Name, win32Error, bytes);
                            }
                        } else {
                            ProcessReadEvent(bytes);
                        }
                        readPending = false;
                    } else if (whandles[ev] == m_Buffer.Serial.ReadBufferNotFull) {
                        // The read buffer is no longer full. We just loop back to the beginning to test if we
                        // should read or not.
                    } else if (whandles[ev] == m_WriteEvent) {
                        result = UnsafeNativeMethods.GetOverlappedResult(m_ComPortHandle, ref writeOverlapped, out bytes, true);
                        if (!result) {
                            int win32Error = Marshal.GetLastWin32Error();
                            int hr = Marshal.GetHRForLastWin32Error();
                            // Should never get ERROR_IO_PENDING, as this method is only called when the event is triggered.
                            if (win32Error != WinError.ERROR_OPERATION_ABORTED || bytes > 0) {
                                if (Log.SerialTrace(System.Diagnostics.TraceEventType.Error))
                                    Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Error, 0,
                                        "{0}: SerialThread: Overlapped WriteFile() error {1} bytes {2}", m_Name, win32Error, bytes);
                                Marshal.ThrowExceptionForHR(hr);
                            } else {
                                // ERROR_OPERATION_ABORTED may be caused by CancelIo or PurgeComm
                                if (Log.SerialTrace(System.Diagnostics.TraceEventType.Verbose))
                                    Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0,
                                        "{0}: SerialThread: Overlapped WriteFile() error {1} bytes {2}", m_Name, win32Error, bytes);
                            }
                        } else {
                            ProcessWriteEvent(bytes);
                        }
                        writePending = false;
                    } else if (whandles[ev] == m_Buffer.Serial.WriteBufferNotEmpty) {
                        // The write buffer is no longer empty. We just loop back to the beginning to test if we
                        // should write or not.
                    } else if (whandles[ev] == m_WriteClearEvent) {
                        if (writePending) {
                            if (Log.SerialTrace(System.Diagnostics.TraceEventType.Verbose))
                                Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0, "{0}: SerialThread: PurgeComm() write pending", m_Name);
                            m_PurgePending = true;
                            result = UnsafeNativeMethods.PurgeComm(m_ComPortHandle,
                                NativeMethods.PurgeFlags.PURGE_TXABORT | NativeMethods.PurgeFlags.PURGE_TXCLEAR);
                            if (!result) {
                                int win32Error = Marshal.GetLastWin32Error();
                                int hr = Marshal.GetHRForLastWin32Error();
                                if (win32Error != WinError.ERROR_OPERATION_ABORTED) {
                                    if (Log.SerialTrace(System.Diagnostics.TraceEventType.Error))
                                        Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Error, 0,
                                        "{0}: SerialThread: PurgeComm() error {1}", m_Name, win32Error);
                                    Marshal.ThrowExceptionForHR(hr);
                                } else {
                                    if (Log.SerialTrace(System.Diagnostics.TraceEventType.Verbose))
                                        Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0,
                                            "{0}: SerialThread: PurgeComm() error {1}", m_Name, win32Error);
                                }
                            }
                        } else {
                            lock (m_Buffer.WriteLock) {
                                if (Log.SerialTrace(System.Diagnostics.TraceEventType.Verbose))
                                    Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0, "{0}: SerialThread: Purged", m_Name);
                                m_Buffer.Serial.Purge();
                                m_WriteClearDoneEvent.Set();
                            }
                        }
                    }
                }

#if STRESSTEST
                SerialTrace.TraceSer.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0, "{0}: STRESSTEST SerialThread: Stress Test Delay of 1000ms", m_Name);
                System.Threading.Thread.Sleep(1000);
                NativeMethods.ComStatErrors commStateErrors = new NativeMethods.ComStatErrors();
                NativeMethods.COMSTAT commStat = new NativeMethods.COMSTAT();
                result = UnsafeNativeMethods.ClearCommError(m_ComPortHandle, out commStateErrors, out commStat);
                if (result) {
                    SerialTrace.TraceSer.TraceEvent(System.Diagnostics.TraceEventType.Information, 0,
                        "{0}: STRESSTEST SerialThread: ClearCommError errors={1}", m_Name, commStateErrors);
                    SerialTrace.TraceSer.TraceEvent(System.Diagnostics.TraceEventType.Information, 0,
                        "{0}: STRESSTEST SerialThread: ClearCommError stats flags={1}, InQueue={2}, OutQueue={3}", m_Name, commStat.Flags, commStat.cbInQue, commStat.cbOutQue);
                } else {
                    SerialTrace.TraceSer.TraceEvent(System.Diagnostics.TraceEventType.Warning, 0,
                        "{0}: STRESSTEST SerialThread: ClearCommError error: {1}", m_Name, Marshal.GetLastWin32Error());
                }
#endif
            }
        }

        /// <summary>
        /// Check if we should execute WaitCommEvent() and get the result if immediately available.
        /// </summary>
        /// <remarks>
        /// This function abstracts the Win32 API WaitCommEvent(). It assumes overlapped I/O.
        /// Therefore, when calling this function, you should ensure that the parameter <c>mask</c>
        /// and <c>overlap</c> are pinned for the duration of the overlapped I/O. Any easy way to
        /// do this is to allocate the variables on the stack and then pass them by reference to
        /// this function.
        /// <para>You should not call this function if a pending I/O operation for WaitCommEvent()
        /// is still open. It is an error otherwise.</para>
        /// </remarks>
        /// <param name="mask">The mask value if information is available immediately.</param>
        /// <param name="overlap">The overlap structure to use.</param>
        /// <returns>If the operation is pending or not.</returns>
        private bool DoWaitCommEvent(out NativeMethods.SerialEventMask mask, ref NativeOverlapped overlap)
        {
            bool result = UnsafeNativeMethods.WaitCommEvent(m_ComPortHandle, out mask, ref overlap);
            if (!result) {
                int w32err = Marshal.GetLastWin32Error();
                int hr = Marshal.GetHRForLastWin32Error();
                if (w32err != WinError.ERROR_IO_PENDING) {
                    if (Log.SerialTrace(System.Diagnostics.TraceEventType.Error))
                        Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Error, 0,
                            "{0}: SerialThread: DoWaitCommEvent: Result: {1}", m_Name, w32err);
                    throw new IOException("WaitCommEvent overlapped exception", hr);
                }
            } else {
                ProcessWaitCommEvent(mask);
            }
            return !result;
        }

        /// <summary>
        /// Do work based on the mask event that has occurred.
        /// </summary>
        /// <param name="mask">The mask that was provided.</param>
        private void ProcessWaitCommEvent(NativeMethods.SerialEventMask mask)
        {
            if (Log.SerialTrace(System.Diagnostics.TraceEventType.Verbose)) {
                if (mask != 0) {
                    Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0,
                        "{0}: SerialThread: ProcessWaitCommEvent: {1}", m_Name, mask);
                }
            }

            // Reading a character
            if ((mask & NativeMethods.SerialEventMask.EV_RXCHAR) != 0) {
                m_ReadByteAvailable = true;
            }
            if ((mask & NativeMethods.SerialEventMask.EV_RXFLAG) != 0) {
                m_ReadByteAvailable = true;
                m_ReadByteEof = true;
            }

            // We don't raise an event for characters immediately, but only after the read operation
            // is complete.
            OnCommEvent(new CommEventArgs(mask & ~(NativeMethods.SerialEventMask.EV_RXCHAR | NativeMethods.SerialEventMask.EV_RXFLAG)));

            if ((mask & NativeMethods.SerialEventMask.EV_TXEMPTY) != 0) {
                lock (m_Buffer.WriteLock) {
                    if (m_Buffer.Serial.TxEmptyEvent()) {
                        if (Log.SerialTrace(System.Diagnostics.TraceEventType.Verbose))
                            Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0,
                                "{0}: SerialThread: ProcessWaitCommEvent: TX-BUFFER empty", m_Name);
                    }
                }
            }

            if ((mask & (NativeMethods.SerialEventMask.EV_RXCHAR | NativeMethods.SerialEventMask.EV_ERR)) != 0) {
                bool result = UnsafeNativeMethods.ClearCommError(m_ComPortHandle, out NativeMethods.ComStatErrors comErr, IntPtr.Zero);
                if (!result) {
                    int w32err = Marshal.GetLastWin32Error();
                    int hr = Marshal.GetHRForLastWin32Error();
                    if (Log.SerialTrace(System.Diagnostics.TraceEventType.Verbose))
                        Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0,
                            "{0}: SerialThread: ClearCommError: WINERROR {1}", m_Name, w32err);
                    Marshal.ThrowExceptionForHR(hr);
                } else {
                    comErr = (NativeMethods.ComStatErrors)((int)comErr & 0x10F);
                    if (comErr != 0) {
                        OnCommErrorEvent(new CommErrorEventArgs(comErr));
                    }
                }
            }
        }

        /// <summary>
        /// Check if we should ReadFile() and process the data if serial data is immediately.
        /// available.
        /// </summary>
        /// <remarks>
        /// This function should be called if there is no existing pending read operation. It
        /// will check if there is data to read (indicated by the variable m_ReadByteAvailable,
        /// which is set by ProcessWaitCommEvent()) and then issue a ReadFile(). If the result
        /// indicates that asynchronous I/O is happening, <b>true</b> is returned. Else this
        /// function automatically calls ProcessReadEvent() with the number of bytes read. If
        /// an asynchronous operation is pending, then you should wait on the event in the
        /// overlapped structure not call this function until GetOverlappedResult() has
        /// been called.
        /// </remarks>
        /// <param name="overlap">The overlap structure to use for reading.</param>
        /// <returns>If the operation is pending or not.</returns>
        private bool DoReadEvent(ref NativeOverlapped overlap)
        {
            // If WaitCommEvent() hasn't been called, there's no data
            if (!m_ReadByteAvailable) return false;

            // Read Buffer is full, so can't write into it.
            if (!m_Buffer.Serial.ReadBufferNotFull.WaitOne(0)) return false;

            // As C# can't convert an offset in the array to a pointer, we have to do
            // our own marshalling with (IntPtr)ReadBufferOffsetEnd that is the address
            // at the end of the read buffer.
            IntPtr bufPtr;
            uint bufLen;
            lock (m_Buffer.ReadLock) {
                bufPtr = m_Buffer.Serial.ReadBufferOffsetEnd;
                bufLen = (uint)m_Buffer.Serial.ReadBuffer.WriteLength;
            }

            bool result = UnsafeNativeMethods.ReadFile(m_ComPortHandle, bufPtr, bufLen, out uint bufRead, ref overlap);
            int w32err = Marshal.GetLastWin32Error();
            int hr = Marshal.GetHRForLastWin32Error();
            if (Log.SerialTrace(System.Diagnostics.TraceEventType.Verbose))
                Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0,
                    "{0}: SerialThread: DoReadEvent: ReadFile({1}, {2}, {3}) == {4}", m_Name,
                    m_ComPortHandle.DangerousGetHandle(), bufPtr, bufLen, result);
            if (!result) {
                if (w32err == WinError.ERROR_OPERATION_ABORTED) {
                    if (Log.SerialTrace(System.Diagnostics.TraceEventType.Information))
                        Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Information, 0,
                            "{0}: SerialThread: DoReadEvent: ReadFile() error {1}", m_Name, w32err);
                    return false;
                }
                if (w32err != WinError.ERROR_IO_PENDING) {
                    if (Log.SerialTrace(System.Diagnostics.TraceEventType.Error))
                        Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Error, 0,
                            "{0}: SerialThread: DoReadEvent: ReadFile() error {1}", m_Name, w32err);
                    // In case an unexpected error occurs here, we kill the thread and indicate an error.
                    // One reason for this happening is a device error, or the device is removed from the
                    // system.
                    Marshal.ThrowExceptionForHR(hr);
                }
            } else {
                // MS Documentation for ReadFile() says that the 'bufRead' parameter should be NULL.
                // However, in the case that the COMMTIMEOUTS is set up so that no wait is required
                // (see COMMTIMEOUTS in Win32 API), this function will actually not perform an
                // asynchronous I/O operation and return the number of bytes copied in bufRead.
                ProcessReadEvent(bufRead);
            }
            return !result;
        }

        /// <summary>
        /// Produce the number of bytes read in the buffer.
        /// </summary>
        /// <remarks>
        /// If the number of bytes read is zero, this function should also be called, as it indicates
        /// that there are no more bytes pending. The recommendation from MS documentation for reading
        /// from the serial port indicates to read the buffer data, until a result of zero is given,
        /// which indicates to wait for the next receiving character.
        /// </remarks>
        /// <param name="bytes">Number of bytes read.</param>
        private void ProcessReadEvent(uint bytes)
        {
            if (Log.SerialTrace(System.Diagnostics.TraceEventType.Verbose))
                Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0, "{0}: SerialThread: ProcessReadEvent: {1} bytes", m_Name, bytes);
            if (bytes == 0) {
                m_ReadByteAvailable = false;
            } else {
                lock (m_Buffer.ReadLock) {
                    if (Log.SerialTrace(System.Diagnostics.TraceEventType.Verbose)) {
                        Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0,
                            "{0}: SerialThread: ProcessReadEvent: End={1}; Bytes={2}", m_Name,
                            m_Buffer.Serial.ReadBuffer.End, bytes);
                    }
                    m_Buffer.Serial.ReadBufferProduce((int)bytes);
                }

                OnCommEvent(new CommEventArgs((m_ReadByteEof ? NativeMethods.SerialEventMask.EV_RXFLAG : 0) | NativeMethods.SerialEventMask.EV_RXCHAR));
                m_ReadByteEof = false;
            }
        }

        /// <summary>
        /// Check if we should WriteFile() and update buffers if serial data is immediately cached by driver.
        /// </summary>
        /// <remarks>
        /// This function should be called if there is no existing pending write operation. If
        /// the result indicates that asynchronous I/O is happening, <b>true</b> is returned.
        /// Else this function automatically calls ProcessWriteEvent() with the number of bytes
        /// written. If an asynchronous operation is pending, then you should wait on the event
        /// in the overlapped structure not call this function until GetOverlappedResult()
        /// has been called.
        /// </remarks>
        /// <param name="overlap">The overlap structure to use for writing.</param>
        /// <returns>If the operation is pending or not.</returns>
        private bool DoWriteEvent(ref NativeOverlapped overlap)
        {
            IntPtr bufPtr;
            uint bufLen;
            lock (m_Buffer.WriteLock) {
                bufPtr = m_Buffer.Serial.WriteBufferOffsetStart;
                bufLen = (uint)m_Buffer.Serial.WriteBuffer.ReadLength;
            }

            bool result = UnsafeNativeMethods.WriteFile(m_ComPortHandle, bufPtr, bufLen, out uint bufWrite, ref overlap);
            int win32err = Marshal.GetLastWin32Error();
            int hr = Marshal.GetHRForLastWin32Error();
            if (Log.SerialTrace(System.Diagnostics.TraceEventType.Verbose))
                Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0,
                    "{0}: SerialThread: DoWriteEvent: WriteFile({1}, {2}, {3}, ...) == {4}", m_Name,
                    m_ComPortHandle.DangerousGetHandle(), bufPtr, bufLen, result);
            if (!result) {
                if (win32err == WinError.ERROR_OPERATION_ABORTED) {
                    if (Log.SerialTrace(System.Diagnostics.TraceEventType.Information))
                        Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Information, 0,
                            "{0}: SerialThread: DoWriteEvent: WriteFile() error {1}", m_Name, win32err);
                    return false;
                }
                if (win32err != WinError.ERROR_IO_PENDING) {
                    if (Log.SerialTrace(System.Diagnostics.TraceEventType.Error))
                        Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Error, 0,
                            "{0}: SerialThread: DoWriteEvent: WriteFile() error {1}", m_Name, win32err);
                    // In case an unexpected error occurs here, we kill the thread and indicate an error.
                    // One reason for this happening is a device error, or the device is removed from the
                    // system.
                    Marshal.ThrowExceptionForHR(hr);
                }
            } else {
                ProcessWriteEvent(bufWrite);
            }
            return !result;
        }

        /// <summary>
        /// Consume the number of bytes written from the write buffer.
        /// </summary>
        /// <param name="bytes">Number of bytes written to the driver.</param>
        private void ProcessWriteEvent(uint bytes)
        {
            if (m_PurgePending) {
                m_PurgePending = false;
                if (Log.SerialTrace(System.Diagnostics.TraceEventType.Verbose))
                    Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0,
                        "{0}: SerialThread: ProcessWriteEvent: {1} bytes - Purged", m_Name, bytes);
                lock (m_Buffer.WriteLock) {
                    m_Buffer.Serial.Purge();
                    m_WriteClearDoneEvent.Set();
                }
            } else {
                if (Log.SerialTrace(System.Diagnostics.TraceEventType.Verbose))
                    Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0,
                        "{0}: SerialThread: ProcessWriteEvent: {1} bytes", m_Name, bytes);
                if (bytes != 0) {
                    lock (m_Buffer.WriteLock) {
                        m_Buffer.Serial.WriteBufferConsume((int)bytes);
                        if (m_Buffer.Serial.TxEmptyEvent()) {
                            // We set the TxEmptyEvent always in v2.x. In release 1.x we only set it
                            // if we received a TX_EMPTY. If we ever change this code to check for TX_EMPTY
                            // first, ensure to check the variable SerialCommError in the main loop, as
                            // that will tell us if we ever expect to get a TX_EMPTY. Given some weird
                            // behaviour of serial ports, not checking is probably the safest, even if not
                            // 100% correct.
                            if (Log.SerialTrace(System.Diagnostics.TraceEventType.Verbose))
                                Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0,
                                    "{0}: SerialThread: ProcessWriteEvent: TX-BUFFER empty", m_Name);
                        }
                    }
                }
            }
        }

        #region Event Handling
        public event EventHandler<CommEventArgs> CommEvent;

        protected virtual void OnCommEvent(CommEventArgs args)
        {
            if (args.EventType == 0) return;

            if (Log.SerialTrace(System.Diagnostics.TraceEventType.Verbose))
                Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0,
                    "{0}: CommEvent: {1}", m_Name, args.EventType.ToString());

            EventHandler<CommEventArgs> handler = CommEvent;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public event EventHandler<CommErrorEventArgs> CommErrorEvent;

        protected virtual void OnCommErrorEvent(CommErrorEventArgs args)
        {
            if (args.EventType == 0) return;

            if (Log.SerialTrace(System.Diagnostics.TraceEventType.Verbose))
                Log.Serial.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0,
                    "{0}: CommErrorEvent: {1}", m_Name, args.EventType.ToString());

            EventHandler<CommErrorEventArgs> handler = CommErrorEvent;
            if (handler != null)
            {
                handler(this, args);
            }
        }
        #endregion

        #region IDisposable Support
        private bool m_IsDisposed; // To detect redundant calls

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (m_IsDisposed) return;

            if (disposing) {
                Stop();
                m_StopRunning.Close();
                m_SerialCommEvent.Close();
                m_ReadEvent.Close();
                m_WriteEvent.Close();
                m_WriteClearEvent.Close();
                m_WriteClearDoneEvent.Close();
                CommErrorEvent = null;
                CommEvent = null;
            }
            m_IsDisposed = true;
        }
        #endregion
    }
}
