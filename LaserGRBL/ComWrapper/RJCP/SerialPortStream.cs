// Copyright © Jason Curl 2012-2023
// Sources at https://github.com/jcurl/SerialPortStream
// Licensed under the Microsoft Public License (Ms-PL)

// Enable the following define to enable behaviour in the number of bytes similar to the MS
// implementation. It's recommended not to do this, as it's not consistent with the rest of
// this implementation.
//#define DRIVERBUFFEREDBYTES

namespace RJCP.IO.Ports
{
    using System;
    using System.Diagnostics;
    using System.Text;
    using System.IO;
    using System.Threading;
    using Datastructures;
    using Native;
    using Trace;

#if NETSTANDARD1_5
    using System.Runtime.ExceptionServices;
    using Microsoft.Extensions.Logging;
#else
    using System.Runtime.Remoting.Messaging;
#endif

#if NETSTANDARD1_5 || NET45
    using System.Threading.Tasks;
#endif

    /// <summary>
    /// The SerialPortStream is a stream class to communicate with serial port based devices.
    /// </summary>
    /// <remarks>
    /// This implementation is a ground up reimplementation of the Microsoft SerialPort class
    /// but one that is a stream. There are numerous small issues with the Microsoft .NET 4.0
    /// implementation (and assumed earlier) that this class attempts to resolve.
    /// <para>For detailed information about serial port programming, refer to the site:
    /// http://msdn.microsoft.com/en-us/library/ms810467.aspx</para>
    /// <para>When instantiating.</para>
    /// </remarks>
    public partial class SerialPortStream : Stream, ISerialPortStream
    {
        private INativeSerial m_NativeSerial;
        private SerialBuffer m_Buffer;
        private ReadToCache m_ReadTo;
        private readonly LogSource m_Log;

        #region Public constants
        /// <summary>
        /// Indicates that no time out should occur.
        /// </summary>
        public const int InfiniteTimeout = Timeout.Infinite;
        #endregion

        #region Constructor, Port Name, Open
        /// <summary>
        /// Constructor. Create a stream that doesn't connect to any port.
        /// </summary>
        /// <remarks>
        /// This constructor initialises a stream object, but doesn't assign it to any COM port.
        /// The properties then assume default settings. No COM port is opened and queried.
        /// </remarks>
        public SerialPortStream()
        {
            m_Log = Log.Serial;
            m_ReadTo = new ReadToCache(Log.ReadTo);

            Initialize();
        }

        /// <summary>
        /// Constructor. Create a stream that connects to the specified port.
        /// </summary>
        /// <remarks>
        /// This constructor attempts to bind directly to the port given. Properties assume
        /// the settings of the port provided. Exceptions may occur if the port cannot be
        /// opened.
        /// </remarks>
        /// <param name="port">The name of the COM port, such as "COM1" or "COM33".</param>
        public SerialPortStream(string port) : this()
        {
            if (port != null) m_NativeSerial.PortName = port;
        }

        /// <summary>
        /// Constructor. Create a stream that connects to the specified port and sets the initial baud rate.
        /// </summary>
        /// <remarks>
        /// The stream doesn't impose any arbitrary limits on setting the baud rate. It is passed
        /// directly to the driver and it is up to the driver to determine if the baud rate is
        /// settable or not. Normally, a driver will attempt to set a baud rate that is within 5%
        /// of the requested baud rate (but not guaranteed).
        /// </remarks>
        /// <param name="port">The name of the COM port, such as "COM1" or "COM33".</param>
        /// <param name="baud">The baud rate that is passed to the underlying driver.</param>
        public SerialPortStream(string port, int baud)
            : this(port)
        {
            m_NativeSerial.BaudRate = baud;
        }

        /// <summary>
        /// Constructor. Create a stream that connects to the specified port with standard parameters.
        /// </summary>
        /// <remarks>
        /// The stream doesn't impose any arbitrary limits on setting the baud rate. It is passed
        /// directly to the driver and it is up to the driver to determine if the baud rate is
        /// settable or not. Normally, a driver will attempt to set a baud rate that is within 5%
        /// of the requested baud rate (but not guaranteed).
        /// <para>Not all combinations are supported. The driver will interpret the data and indicate
        /// if configuration is possible or not.</para>
        /// </remarks>
        /// <param name="port">The name of the COM port, such as "COM1" or "COM33".</param>
        /// <param name="baud">The baud rate that is passed to the underlying driver.</param>
        /// <param name="data">The number of data bits. This is checked that the driver
        /// supports the data bits provided. The special type 16X is not supported.</param>
        /// <param name="parity">The parity for the data stream.</param>
        /// <param name="stopbits">Number of stop bits.</param>
        public SerialPortStream(string port, int baud, int data, Parity parity, StopBits stopbits)
            : this(port)
        {
            m_NativeSerial.BaudRate = baud;
            m_NativeSerial.DataBits = data;
            m_NativeSerial.Parity = parity;
            m_NativeSerial.StopBits = stopbits;
        }

#if NETSTANDARD1_5
        /// <summary>
        /// Constructor. Create a stream that doesn't connect to any port. Specify the logger.
        /// </summary>
        /// <remarks>
        /// This constructor initialises a stream object, but doesn't assign it to any COM port.
        /// The properties then assume default settings. No COM port is opened and queried.
        /// <para>This method allows to inject a logging instance to help track down and debug
        /// communication on the serial port. Many defects and issues can be traced back to
        /// behaviours of various drivers.</para>
        /// </remarks>
        /// <param name="logger">The logger instance to log to.</param>
        [CLSCompliant(false)]
        public SerialPortStream(ILogger logger)
        {
            m_Log = new LogSource(Log.SerialPortStream, logger);
            m_ReadTo = new ReadToCache();

            Initialize();
        }
#endif

        private void Initialize()
        {
            m_NativeSerial = CreateNativeSerial(m_Log);
            if (m_NativeSerial == null)
                throw new NotSupportedException("SerialPortStream is not supported on this platform");

            InitialiseEvents();
        }

        private static INativeSerial CreateNativeSerial()
        {
            if (Platform.IsUnix()) return new UnixNativeSerial();
            if (Platform.IsWinNT()) return new WinNativeSerial();
            return null;
        }

        private INativeSerial CreateNativeSerial(LogSource log)
        {
            if (Platform.IsUnix()) return new UnixNativeSerial(log);
            if (Platform.IsWinNT()) return new WinNativeSerial(log);
            return null;
        }

        /// <summary>
        /// Get the version of this assembly (or components driving this assembly).
        /// </summary>
        /// <value>The version of the assembly and/or subcomponents.</value>
        public string Version
        {
            get { return m_NativeSerial.Version; }
        }

        /// <summary>
        /// Gets the port for communications, including but not limited to all available COM ports.
        /// </summary>
        /// <remarks>
        /// A list of valid port names can be obtained using the <see cref="GetPortNames"/> method.
        /// <para>
        /// When changing the port name, and the property UpdateOnPortSet is <see langword="true"/>, setting this
        /// property will cause the port to be opened, status read and the port then closed. Thus, you can use this
        /// behaviour to determine the actual settings of the port (which remain constant until a program actually
        /// changes the port settings).
        /// </para>
        /// <para>
        /// Setting this property to itself, while having UpdateOnPortSet to <see langword="true"/> has the effect of
        /// updating the local properties based on the current port settings.
        /// </para>
        /// </remarks>
        public string PortName
        {
            get
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                return m_NativeSerial.PortName;
            }
            set
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Must provide a valid name for a COM port", nameof(value));
                if (m_NativeSerial.IsOpen && value != m_NativeSerial.PortName) throw new InvalidOperationException("Serial Port already opened");

                m_NativeSerial.PortName = value;
            }
        }
        /// <summary>
        /// Update properties based on the current port, overwriting already existing properties.
        /// </summary>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="InvalidOperationException">Serial Port already opened.</exception>
        /// <remarks>
        /// This method opens the serial port and retrieves the current settings from Windows.
        /// These settings are then made available via the various properties, BaudRate, DataBits,
        /// Parity, ParityReplace, Handshake, StopBits, TxContinueOnXoff, DiscardNull, XOnLimit
        /// and XOffLimit.
        /// </remarks>
        public void GetPortSettings()
        {
            if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
            if (m_NativeSerial.IsOpen) throw new InvalidOperationException("Serial Port already opened");
            m_NativeSerial.Open();
            m_NativeSerial.GetPortSettings();
            m_NativeSerial.Close();
        }

        /// <summary>
        /// Opens a new serial port connection.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// This object is already managing a serial port connection.
        /// </exception>
        /// <exception cref="ObjectDisposedException">SerialPortStream is disposed of.</exception>
        /// <remarks>
        /// Opens a connection to the serial port provided by the constructor or the Port property. If this object is
        /// already managing a serial port, this object raises an exception.
        /// <para>
        /// When opening the port, only the settings explicitly applied will be given to the port. That is, if you read
        /// the default BaudRate as 115200, this value will only be applied if you explicitly set it to 115200. Else the
        /// default baud rate of the serial port when its opened will be used.
        /// </para>
        /// <para>
        /// Normally when you instantiate this stream on a COM port, it is opened for a brief time and queried for the
        /// capabilities and default settings. This allows your application to use the settings that were already
        /// available (such as defined by the windows user in the Control Panel, or the last open application). If you
        /// require to open the COM port without briefly opening it to query its status, then you need to instantiate
        /// this object through the default constructor. Set the property UpdateOnPortSet to <see langword="false"/> and
        /// then set the Port property. Provide all the other properties you require then call the <see cref="Open()"/>
        /// method. The port will be opened using the default properties providing you with a consistent environment
        /// (independent of the state of the Operating System or the driver beforehand).
        /// </para>
        /// </remarks>
        public void Open()
        {
            Open(true);
        }

        /// <summary>
        /// Opens a new serial port connection with control if the port settings are initialised or not.
        /// </summary>
        /// <exception cref="ObjectDisposedException">SerialPortStream is disposed of.</exception>
        /// <exception cref="InvalidOperationException">Serial Port already opened</exception>
        /// <remarks>
        /// Opens a connection to the serial port provided by the constructor or the <see cref="PortName"/> property. If
        /// this object is already managing a serial port, this object raises an exception.
        /// <para>
        /// You can override the open so that no communication settings are retrieved or set. This is useful for virtual
        /// COM ports that do not manage state bits (some as some emulated COM ports or USB based communications that
        /// present themselves as a COM port but do not have any underlying physical RS232 implementation).
        /// </para>
        /// <note type="note">
        /// If you use this method to avoid setting parameters for the serial port, instead only to open the serial
        /// port, you should be careful not to set any properties associated with the serial port, as they will set the
        /// communications properties. </note>
        /// </remarks>
        public void OpenDirect()
        {
            Open(false);
        }

        private void Open(bool setCommState)
        {
            if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
            if (m_NativeSerial.IsOpen) throw new InvalidOperationException("Serial Port already opened");

            m_ReadCheckDeviceErrorNotified = false;
            m_NativeSerial.Open();
            try {
                if (setCommState) {
                    m_NativeSerial.SetPortSettings();
                    // Fetch the actual settings and get the capabilities
                    m_NativeSerial.GetPortSettings();
                }

                // Create threads and start working with local buffers
                if (m_Buffer == null) {
                    m_Buffer = m_NativeSerial.CreateSerialBuffer(m_ReadBufferSize, m_WriteBufferSize);
                } else {
                    m_Buffer.Stream.Reset();
                }
                m_NativeSerial.StartMonitor(m_Buffer, PortName);
            } catch {
                m_NativeSerial.Close();
                throw;
            }
        }

        /// <summary>
        /// Gets a value indicating the open or closed status of the SerialPortStream object.
        /// </summary>
        /// <remarks>
        /// The <see cref="IsOpen"/> property tracks whether the port is open for use by the caller, not whether the
        /// port is open by any application on the machine.
        /// </remarks>
        /// <value>
        /// <see langword="true"/> if the serial port is open; otherwise, <see langword="false"/>. The default is
        /// <see langword="false"/>.
        /// </value>
        public bool IsOpen
        {
            get
            {
                lock (m_CloseLock) {
                    if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                    return m_NativeSerial.IsOpen && m_NativeSerial.IsRunning;
                }
            }
        }

        private readonly object m_CloseLock = new object();

        /// <summary>
        /// Closes the port connection, sets the <see cref="IsOpen"/> property to <see langword="false"/>. Does not
        /// dispose the object.
        /// </summary>
        /// <remarks>
        /// This method will clean up the object so far as to close the port. Internal buffers remain active that the
        /// stream can continue to read. Writes will throw an exception.
        /// </remarks>
#if !NETSTANDARD1_5
        public new void Close()
#else
        public void Close()
#endif
        {
            lock (m_CloseLock) {
                if (IsDisposed) return;
                if (m_Buffer != null) m_Buffer.Stream.AbortWait();
                m_NativeSerial.Close();
            }
        }
        #endregion

        #region Computer Configuration and Ports
        /// <summary>
        /// Gets an array of serial port names for the current computer.
        /// </summary>
        /// <returns>An array of serial port names for the current computer.</returns>
        public static string[] GetPortNames()
        {
            using (INativeSerial serial = CreateNativeSerial()) {
                if (serial == null) throw new NotSupportedException("SerialPortStream is not supported on this platform");
                return serial.GetPortNames();
            }
        }

        /// <summary>
        /// Gets an array of serial port names and descriptions for the current computer.
        /// </summary>
        /// <remarks>
        /// This method uses the Windows Management Interface to obtain its information. Therefore,
        /// the list may be different to the list obtained using the <see cref="GetPortNames"/> method which
        /// uses other techniques.
        /// <para>On Windows 7, this method shows to return normal COM ports, but not those
        /// associated with a modem driver.</para>
        /// </remarks>
        /// <returns>An array of serial ports for the current computer.</returns>
        public static PortDescription[] GetPortDescriptions()
        {
            using (INativeSerial serial = CreateNativeSerial()) {
                if (serial == null) throw new NotSupportedException("SerialPortStream is not supported on this platform");
                return serial.GetPortDescriptions();
            }
        }
        #endregion

        #region Reading and Writing Configuration
        /// <summary>
        /// Gets or sets the byte encoding for pre- and post-transmission conversion of text.
        /// </summary>
        /// <remarks>
        /// The encoding is used for encoding string information to byte format when sending
        /// over the serial port, or receiving data via the serial port. It is only used
        /// with the read/write functions that accept strings (and not used for byte based
        /// reading and writing).
        /// </remarks>
        public Encoding Encoding
        {
            get { return m_ReadTo.Encoding; }
            set
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                m_ReadTo.Encoding = value;
            }
        }

        private string m_NewLine = "\n";

        /// <summary>
        /// Gets or sets the value used to interpret the end of a call to the <see cref="ReadLine"/> and
        /// <see cref="WriteLine"/> methods.
        /// </summary>
        /// <remarks>A value that represents the end of a line. The default is a line feed, (NewLine).</remarks>
        /// <exception cref="ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="ArgumentNullException">The <see cref="NewLine"/> cannot be <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">The <see cref="NewLine"/> may not be empty.</exception>
        public string NewLine
        {
            get { return m_NewLine; }
            set
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                if (value == null) throw new ArgumentNullException(nameof(value));
                if (string.IsNullOrEmpty(value)) throw new ArgumentException("Newline may not be empty", nameof(value));
                m_NewLine = value;
            }
        }
        #endregion

        #region Driver Settings
        /// <summary>
        /// Specify the driver In Queue at the time it is opened.
        /// </summary>
        /// <remarks>
        /// This provides the driver a recommended internal input buffer, in bytes.
        /// </remarks>
        public int DriverInQueue
        {
            get
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                return m_NativeSerial.DriverInQueue;
            }
            set
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                if (m_NativeSerial.IsOpen) throw new InvalidOperationException("Serial Port already opened");
                m_NativeSerial.DriverInQueue = value;
            }
        }

        /// <summary>
        /// Specify the driver Out Queue at the time it is opened.
        /// </summary>
        /// <remarks>
        /// This provides the driver a recommended internal output buffer, in bytes.
        /// </remarks>
        public int DriverOutQueue
        {
            get
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                return m_NativeSerial.DriverOutQueue;
            }
            set
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                if (m_NativeSerial.IsOpen) throw new InvalidOperationException("Serial Port already opened");
                m_NativeSerial.DriverOutQueue = value;
            }
        }
        #endregion

        /// <summary>
        /// Gets a value that determines whether the current stream can time out.
        /// </summary>
        /// <returns>A value that determines whether the current stream can time out.</returns>
        public override bool CanTimeout { get { return true; } }

        #region Reading
        /// <summary>
        /// Check if this stream supports reading.
        /// </summary>
        /// <remarks>
        /// Supported so long as the stream is not disposed.
        /// </remarks>
        public override bool CanRead
        {
            get { return !IsDisposed; }
        }

        private int m_ReadTimeout = InfiniteTimeout;

        /// <summary>
        /// Define the time out when reading data from the stream.
        /// </summary>
        /// <remarks>
        /// This defines the time out when data arrives in the buffered memory of this
        /// stream, that is, when the driver indicates that data has arrived to the
        /// application.
        /// <para>Should the user perform a read operation and no data is available
        /// to copy in the buffer, a time out will occur.</para>
        /// <para>Set this property to <see cref="InfiniteTimeout"/> for an infinite time out.</para>
        /// </remarks>
        public override int ReadTimeout
        {
            get { return m_ReadTimeout; }
            set
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                if (value < 0) {
                    m_ReadTimeout = InfiniteTimeout;
                } else {
                    m_ReadTimeout = value;
                }
            }
        }

        private int m_ReadBufferSize = 1048576;

        /// <summary>
        /// Gets or sets the size of the SerialPortStream input buffer.
        /// </summary>
        /// <remarks>
        /// Sets the amount of buffering to use when reading data from the serial port.
        /// Data is read locally into this buffered stream through another port.
        /// <para>The Microsoft implementation uses this to set the buffer size of the
        /// underlying driver. This implementation interprets the ReadBufferSize
        /// differently by setting the local buffer which can be much larger (megabytes)
        /// and independent of the low level driver.</para>
        /// </remarks>
        /// <exception cref="InvalidOperationException">An attempt was used to change
        /// the size of the buffer while the port is open (and therefore buffering is
        /// active).</exception>
        public int ReadBufferSize
        {
            get { return m_ReadBufferSize; }
            set
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                if (m_NativeSerial.IsOpen) throw new InvalidOperationException("Serial Port already opened");
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value), "WriteBufferSize must be greater than zero");

                int newBufferSize = value;
                if (newBufferSize < 1024) newBufferSize = 1024;

                if (m_ReadBufferSize != newBufferSize) {
                    if (m_Buffer != null) {
                        m_Buffer.Dispose();
                        m_Buffer = null;
                    }
                    m_ReadBufferSize = newBufferSize;
                    if (m_RxThreshold > value) m_RxThreshold = value;
                }
            }
        }

        private int m_RxThreshold = 1;

        /// <summary>
        /// Gets or sets the number of bytes in the read buffer before a DataReceived event occurs.
        /// </summary>
        public int ReceivedBytesThreshold
        {
            get
            {
                return m_RxThreshold;
            }
            set
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value), "Must be a positive value (1 or greater)");
                if (value > m_ReadBufferSize)
                    throw new ArgumentOutOfRangeException(nameof(value), "Must be less or equal to the ReadBufferSize");

                // Only raise an event if we think that we wouldn't have received an event otherwise
                int btr = m_NativeSerial.BytesToRead;
                if (btr < m_RxThreshold && btr > value) {
                    m_RxThreshold = value;
                    NativeSerial_DataReceived(this, new SerialDataReceivedEventArgs(SerialData.Chars));
                } else {
                    m_RxThreshold = value;
                }
            }
        }

#if DRIVERBUFFEREDBYTES
        /// <summary>
        /// Gets the number of bytes of data in the receive buffer and in the serial driver.
        /// </summary>
        /// <remarks>
        /// This method returns the number of bytes available in the input read buffer.
        /// Bytes that are cached by the driver itself are also (generally) accounted for.
        /// <para>A small error in determining the number of bytes available to read
        /// may occur, in particularly, an amount less than what is actually available
        /// for reading may be returned.</para>
        /// <para>Why we sometimes don't return the total number of bytes is due to
        /// the asynchronous behaviour of reading from the serial port with consideration
        /// to performance. Particularly, there might be a small time where the number
        /// of bytes in the cached buffer is "x", and the number of bytes in the serial
        /// driver is "y". Normally, the result will be the sum, "X+y". However, there
        /// may be a very small window of opportunity that the size of the buffer will
        /// be "x" and the number of bytes in the serial driver is zero, or less than
        /// "y", because it has just been transferred to the local buffer, but has not
        /// yet been accounted for.</para>
        /// </remarks>
        public int BytesToRead
        {
            get
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                if (m_Buffer == null) return 0;
                lock (m_Buffer.ReadLock) {
                    return m_Buffer.Stream.BytesToRead + m_NativeSerial.BytesToRead;
                }
            }
        }
#else
        /// <summary>
        /// Gets the number of bytes of data in the receive buffer.
        /// </summary>
        /// <remarks>
        /// This method returns the number of bytes available in the input read buffer.
        /// Bytes that are cached by the driver itself are not accounted for, as they
        /// haven't yet been read by the local thread.
        /// <para>This has the effect, that if the local buffer is full (let's say that
        /// it is arbitrarily picked to be 64KB) and the local driver also has buffered
        /// 4KB, only the size of the local buffer is given, so 64KB (instead of the
        /// expected 68KB).</para>
        /// </remarks>
        public int BytesToRead
        {
            get
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                if (m_Buffer == null) return 0;
                return m_Buffer.Stream.BytesToRead;
            }
        }
#endif

        private void ReadCheck(byte[] buffer, int offset, int count)
        {
            if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "Negative offset provided");
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "Negative count provided");
            if (buffer.Length - offset < count) throw new ArgumentException("offset and count exceed buffer boundaries");
        }

        private void ReadCheck(char[] buffer, int offset, int count)
        {
            if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "Negative offset provided");
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "Negative count provided");
            if (buffer.Length - offset < count) throw new ArgumentException("offset and count exceed buffer boundaries");
        }

        private bool m_ReadCheckDeviceErrorNotified;

        private bool ReadCheckDeviceError()
        {
            return ReadCheckDeviceError(false);
        }

        private bool ReadCheckDeviceError(bool immediate)
        {
            if (m_Buffer != null) {
                if (m_NativeSerial.IsOpen && !m_NativeSerial.IsRunning && m_Buffer.Stream.BytesToRead == 0) {
                    if (immediate || m_ReadCheckDeviceErrorNotified) {
                        // This should only happen if the monitoring/buffering threads
                        // have died without explicitly closing the serial port.
                        throw new IOException("Device Error");
                    }

                    // We don't want to raise an exception the first time the issue is detected,
                    // to allow a Read to return zero for end-of-file conditions.
                    m_ReadCheckDeviceErrorNotified = true;

                    // Return true to indicate there was a device error. Next time this function
                    // is called, we raise an exception.
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Reads a sequence of bytes from the current stream and advances the position within the stream by the number
        /// of bytes read.
        /// </summary>
        /// <param name="buffer">
        /// An array of bytes. When this method returns, the buffer contains the specified byte array with the values
        /// between <paramref name="offset" /> and (<paramref name="offset" /> + <paramref name="count" /> - 1) replaced
        /// by the bytes read from the current source.
        /// </param>
        /// <param name="offset">
        /// The zero-based byte offset in <paramref name="buffer" /> at which to begin storing the data read from the
        /// current stream.
        /// </param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="ArgumentNullException"><see langword="null"/> buffer provided.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Negative offset provided, or negative count provided.</exception>
        /// <exception cref="ArgumentException">Offset and count exceed buffer boundaries.</exception>
        /// <exception cref="IOException">Device Error (e.g. device removed).</exception>
        /// <returns>
        /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that
        /// many bytes are not currently available, or zero (0) if the end of the stream has been reached.
        /// </returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            ReadCheck(buffer, offset, count);
            if (m_Buffer == null) return 0;
            if (ReadCheckDeviceError()) return 0;

            if (count == 0) return 0;
            return InternalBlockingRead(buffer, offset, count);
        }

        private int InternalBlockingRead(byte[] buffer, int offset, int count)
        {
            if (m_NativeSerial.IsRunning) {
                bool ready = m_Buffer.Stream.WaitForRead(m_ReadTimeout);
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                if (!ready) {
                    ReadCheckDeviceError();
                    return 0;
                }
            }

            return InternalRead(buffer, offset, count);
        }

        private int InternalRead(byte[] buffer, int offset, int count)
        {
            int bytes = m_Buffer.Stream.Read(buffer, offset, count);
            if (bytes > 0) {
                m_ReadTo.Reset(false);
            }
            return bytes;
        }

#if NETSTANDARD1_5 || NET45
        /// <summary>
        /// Asynchronously reads a sequence of bytes from the current stream and advances the
        /// position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">The buffer to read the data into.</param>
        /// <param name="offset">
        /// The byte offset in <paramref name="buffer"/> at which to begin writing data read from
        /// the stream.
        /// </param>
        /// <param name="count">The maximum number of bytes to read.</param>
        /// <param name="cancellationToken">
        /// Using a cancellation token is not supported and will be ignored.
        /// </param>
        /// <returns>A <see cref="Task{T}"/> representing the asynchronous operation.</returns>
        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            ReadCheck(buffer, offset, count);

            TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
            InternalBeginRead(buffer, offset, count, iar => {
                try {
                    tcs.TrySetResult(InternalEndRead(iar));
                } catch (OperationCanceledException) {
                    tcs.TrySetCanceled();
                } catch (Exception ex) {
                    tcs.TrySetException(ex);
                }
            }, null);
            return tcs.Task;
        }
#endif

#if NET40 || NET45
        /// <summary>
        /// Begins an asynchronous read operation.
        /// </summary>
        /// <param name="buffer">The buffer to read the data into.</param>
        /// <param name="offset">The byte offset in <paramref name="buffer"/> at which to begin writing data read from the stream.</param>
        /// <param name="count">The maximum number of bytes to read.</param>
        /// <param name="callback">An optional asynchronous callback, to be called when the read is complete.</param>
        /// <param name="state">A user-provided object that distinguishes this particular asynchronous read request from other requests.</param>
        /// <returns>An <see cref="IAsyncResult"/> object to be used with <see cref="EndRead"/>.</returns>
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            ReadCheck(buffer, offset, count);

            return InternalBeginRead(buffer, offset, count, callback, state);
        }

        /// <summary>
        /// Waits for the pending asynchronous read to complete.
        /// </summary>
        /// <param name="asyncResult">The reference to the pending asynchronous request to finish.</param>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="IOException">Device Error (e.g. device removed).</exception>
        /// <returns>
        /// The number of bytes read from the stream, between zero (0) and the number of bytes you requested. Streams
        /// return zero (0) only at the end of the stream, otherwise, they should block until at least one byte is
        /// available.
        /// </returns>
        public override int EndRead(IAsyncResult asyncResult)
        {
            if (asyncResult == null) throw new ArgumentNullException(nameof(asyncResult));

            return InternalEndRead(asyncResult);
        }
#endif

#if !NETSTANDARD1_5
        private delegate int ReadDelegate(byte[] buffer, int offset, int count);
#endif

        private IAsyncResult InternalBeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            try {
                // If there was a device error, then an exception is raised, or it immediately returns with zero bytes.
                if (ReadCheckDeviceError()) count = 0;
            } catch (Exception ex) {
                LocalAsync<int> ar = new LocalAsync<int>(state) {
                    Exception = ex,
                    Result = 0,
                    IsCompleted = true,
                    CompletedSynchronously = true
                };
                if (callback != null) callback(ar);
                return ar;
            }

            if (m_Buffer == null || count == 0 || m_Buffer.Stream.WaitForRead(0)) {
                // Data in the buffer, we can return immediately
                LocalAsync<int> ar = new LocalAsync<int>(state);
                if (m_Buffer != null && count > 0) {
                    ar.Result = InternalRead(buffer, offset, count);
                } else {
                    ar.Result = 0;
                }
                ar.IsCompleted = true;
                ar.CompletedSynchronously = true;
                if (callback != null) callback(ar);
                return ar;
            } else {
                // No data in buffer, so we create a thread in the background

#if NETSTANDARD1_5
                TaskCompletionSource<int> tcs = new TaskCompletionSource<int>(state);
                Task<int> task = new Task<int>(() => {
                    int r = InternalBlockingRead(buffer, offset, count);
                    return r;
                });

                task.ContinueWith(t => {
                    // Copy the task result into the returned task.
                    if (t.IsFaulted)
                        tcs.TrySetException(t.Exception.InnerExceptions);
                    else if (t.IsCanceled)
                        tcs.TrySetCanceled();
                    else
                        tcs.TrySetResult(t.Result);

                    // Invoke the user callback if necessary.
                    if (callback != null)
                        callback(tcs.Task);
                });

                task.Start();
                return tcs.Task;
#else
                ReadDelegate read = InternalBlockingRead;
                return read.BeginInvoke(buffer, offset, count, callback, state);
#endif
            }
        }

        private int InternalEndRead(IAsyncResult asyncResult)
        {
            if (asyncResult is LocalAsync<int> localAsync) {
                if (!localAsync.IsCompleted) localAsync.AsyncWaitHandle.WaitOne(Timeout.Infinite);
                localAsync.Dispose();
                if (localAsync.Exception != null) throw localAsync.Exception;
                if (localAsync.Result == 0) ReadCheckDeviceError();
                return localAsync.Result;
            } else {
#if NETSTANDARD1_5
                try {
                    return ((Task<int>)asyncResult).Result;
                } catch (AggregateException ex) {
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                    throw;
                }
#else
                AsyncResult ar = (AsyncResult)asyncResult;
                ReadDelegate caller = (ReadDelegate)ar.AsyncDelegate;
                return caller.EndInvoke(asyncResult);
#endif
            }
        }

        /// <summary>
        /// Synchronously reads one byte from the SerialPort input buffer.
        /// </summary>
        /// <returns>The byte, cast to an <see langword="int"/>, or -1 if the end of the stream has been read.</returns>
        /// <exception cref="IOException">Device Error (e.g. device removed).</exception>
        public override int ReadByte()
        {
            if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
            if (m_Buffer == null) return -1;
            if (ReadCheckDeviceError()) return -1;

            if (m_NativeSerial.IsRunning) {
                if (!m_Buffer.Stream.WaitForRead(m_ReadTimeout)) {
                    ReadCheckDeviceError();
                    return -1;
                }
            }
            int value = m_Buffer.Stream.ReadByte();
            if (value != -1) m_ReadTo.Reset(false);
            return value;
        }

        /// <summary>
        /// Reads a number of characters from the SerialPortStream input buffer and writes
        /// them into an array of characters at a given offset.
        /// </summary>
        /// <param name="buffer">The character array to write the input to.</param>
        /// <param name="offset">Offset into the buffer where to start putting the data.</param>
        /// <param name="count">Maximum number of bytes to read into the buffer.</param>
        /// <returns>The actual number of bytes copied into the buffer, 0 if there was a time out.</returns>
        /// <exception cref="IOException">Device Error (e.g. device removed).</exception>
        /// <remarks>
        /// This function converts the data in the local buffer to characters based on the
        /// encoding defined by the encoding property. The encoder used may buffer data between
        /// calls if characters may require more than one byte of data for its interpretation
        /// as a character.
        /// </remarks>
        public int Read(char[] buffer, int offset, int count)
        {
            ReadCheck(buffer, offset, count);
            if (m_Buffer == null) return 0;
            if (ReadCheckDeviceError()) return 0;

            if (count == 0) return 0;

            TimerExpiry te = new TimerExpiry(m_ReadTimeout);
            int chars;
            do {
                chars = m_ReadTo.Read(m_Buffer, buffer, offset, count);
                if (chars == 0) {
                    if (!m_Buffer.Stream.WaitForRead(te.Timeout)) {
                        ReadCheckDeviceError();
                        return 0;
                    }
                }
            } while (chars == 0 && !te.Expired);
            return chars;
        }

        /// <summary>
        /// Synchronously reads one character from the SerialPortStream input buffer.
        /// </summary>
        /// <returns>The character that was read. -1 indicates no data was available within the time out.</returns>
        public int ReadChar()
        {
            if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
            if (m_Buffer == null) return -1;
            if (ReadCheckDeviceError()) return -1;

            TimerExpiry te = new TimerExpiry(m_ReadTimeout);
            int chars;
            do {
                chars = m_ReadTo.ReadChar(m_Buffer);
                if (chars == -1) {
                    if (!m_Buffer.Stream.WaitForRead(te.Timeout)) {
                        ReadCheckDeviceError();
                        return -1;
                    }
                }
            } while (chars == -1 && !te.Expired);
            return chars;
        }

        /// <summary>
        /// Reads up to the NewLine value in the input buffer.
        /// </summary>
        /// <returns>The contents of the input buffer up to the first occurrence of
        /// a NewLine value.</returns>
        /// <exception cref="TimeoutException">Data was not available in the timeout specified.</exception>
        /// <exception cref="IOException">Device Error (e.g. device removed).</exception>
        /// <exception cref="ObjectDisposedException"/>
        public string ReadLine()
        {
            if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
            return ReadTo(m_NewLine);
        }

        /// <summary>
        /// Reads a string up to the specified <i>text</i> in the input buffer.
        /// </summary>
        /// <remarks>
        /// The ReadTo() function will read text from the byte buffer up to a predetermined limit (1024 characters) when
        /// looking for the string <paramref name="text"/>. If <paramref name="text"/> is not found within this limit,
        /// data is thrown away and more data is read (effectively consuming the earlier bytes).
        /// <para>
        /// This method is provided as compatibility with the Microsoft implementation. There are some important
        /// differences however. This method attempts to fix a minor pathological problem with the Microsoft
        /// implementation. If the string <paramref name="text"/> is not found, the MS implementation may modify the
        /// internal state of the decoder. As a workaround, it pushes all decoded characters back into its internal byte
        /// buffer, which fixes the problem that a second call to the ReadTo() method returns the consistent results,
        /// but a call to Read(byte[], ..) may return data that was not actually transmitted by the DCE. This would
        /// happen in case that an invalid byte sequence was found, converted to a fall back character. The original
        /// byte sequence is removed and replaced with the byte equivalent of the fall back character.
        /// </para>
        /// <para>This method is rather slow, because it tries to preserve the byte buffer in case of failure.</para>
        /// <para>
        /// In case the data cannot be read, an exception is always thrown. So you may assume that if this method
        /// returns, you have valid data.
        /// </para>
        /// </remarks>
        /// <param name="text">The text to indicate where the read operation stops.</param>
        /// <returns>The contents of the input buffer up to the specified <paramref name="text"/>.</returns>
        /// <exception cref="TimeoutException">Data was not available in the timeout specified.</exception>
        /// <exception cref="IOException">Device Error (e.g. device removed).</exception>
        /// <exception cref="ObjectDisposedException"/>
        public string ReadTo(string text)
        {
            if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
            if (string.IsNullOrEmpty(text)) throw new ArgumentException("Parameter text shall not be null or empty", nameof(text));
            if (m_Buffer == null) return null;
            if (ReadCheckDeviceError()) return null;

            TimerExpiry te = new TimerExpiry(m_ReadTimeout);
            bool dataAvailable;
            do {
                if (m_ReadTo.ReadTo(m_Buffer, text, out string line)) return line;
                dataAvailable =
                    m_NativeSerial.IsRunning &&
                    m_ReadTo.ReadToWaitForNewData(m_Buffer, te.RemainingTime());
            } while (dataAvailable && !te.Expired);
            if (!dataAvailable) ReadCheckDeviceError(true);
            throw new TimeoutException();
        }

        /// <summary>
        /// Reads all immediately available bytes.
        /// </summary>
        /// <remarks>
        /// Reads all data in the current buffer. If there is no data available, then no data is returned. This is
        /// different to the Microsoft implementation, that will read all data, and if there is no data, then it waits
        /// for data based on the time outs. This method employs no time outs.
        /// <para>
        /// Because this method returns only the data that is currently in the cached buffer and ignores the data that is
        /// actually buffered by the driver itself, there may be a slight discrepancy between the value returned by
        /// BytesToRead and the actual length of the string returned.
        /// </para>
        /// <para>
        /// This method differs slightly from the Microsoft implementation in that this function doesn't initiate a read
        /// operation, as we have a dedicated thread to reading data that is running independently.
        /// </para>
        /// </remarks>
        /// <returns>The contents of the stream and the input buffer of the <see cref="SerialPortStream"/>.</returns>
        public string ReadExisting()
        {
            if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
            if (m_Buffer == null) return null;
            if (ReadCheckDeviceError()) return null;

            return m_ReadTo.ReadExisting(m_Buffer);
        }

        /// <summary>
        /// Discards data from the serial driver's receive buffer.
        /// </summary>
        /// <remarks>
        /// This function will discard the receive buffer of the <see cref="SerialPortStream"/>.
        /// </remarks>
        public void DiscardInBuffer()
        {
            if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
            if (m_Buffer == null) return;

            lock (m_Buffer.ReadLock) {
                m_Buffer.Stream.DiscardInBuffer();
                if (m_NativeSerial.IsOpen) m_NativeSerial.DiscardInBuffer();
            }
        }
        #endregion

        #region Writing
        /// <summary>
        /// Check if this stream supports writing.
        /// </summary>
        /// <remarks>
        /// Supported so long as the stream is not disposed.
        /// </remarks>
        public override bool CanWrite
        {
            get { return !IsDisposed && m_NativeSerial.IsOpen && m_NativeSerial.IsRunning; }
        }

        private int m_WriteTimeout = InfiniteTimeout;

        /// <summary>
        /// Define the time out when writing data to the local buffer.
        /// </summary>
        /// <remarks>
        /// This defines the time out when writing data to the local buffer. No guarantees are given to when the data
        /// will actually be transferred over to the serial port as this is dependent on the hardware configuration and
        /// flow control.
        /// <para>
        /// When writing data to the stream buffer, a time out will occur if not all data can be written to the local
        /// buffer and the buffer wasn't able to empty itself in the period given by the time out.
        /// </para>
        /// <para>
        /// Naturally then, this depends on the size of the send buffer in use, how much data is already in the buffer,
        /// how fast the data can leave the buffer.
        /// </para>
        /// <para>
        /// In case the data cannot be written to the buffer in the given time out, no data will be written at all.
        /// </para>
        /// </remarks>
        public override int WriteTimeout
        {
            get { return m_WriteTimeout; }
            set
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                if (value < 0) {
                    m_WriteTimeout = InfiniteTimeout;
                } else {
                    m_WriteTimeout = value;
                }
            }
        }

        private int m_WriteBufferSize = 131072;

        /// <summary>
        /// Gets or sets the size of the serial port output buffer.
        /// </summary>
        /// <remarks>
        /// Defines the size of the buffered stream write buffer, used to send data to the serial port. It does not
        /// affect the buffers in the serial port hardware itself.
        /// <para>
        /// The Microsoft implementation uses this to set the buffer size of the underlying driver. This implementation
        /// interprets the WriteBufferSize differently by setting the local buffer which can be much larger (megabytes)
        /// and independent of the low level driver.
        /// </para>
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// An attempt was used to change the size of the buffer while the port is open (and therefore buffering is
        /// active).
        /// </exception>
        public int WriteBufferSize
        {
            get { return m_WriteBufferSize; }
            set
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                if (m_NativeSerial.IsOpen) throw new InvalidOperationException("Serial Port already opened");
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value), "WriteBufferSize must be greater than zero");

                int newBufferSize = value;
                if (newBufferSize < 1024) newBufferSize = 1024;

                if (m_WriteBufferSize != newBufferSize) {
                    if (m_Buffer != null) {
                        m_Buffer.Dispose();
                        m_Buffer = null;
                    }
                    m_WriteBufferSize = newBufferSize;
                }
            }
        }

        /// <summary>
        /// Gets the number of bytes of data in the send buffer.
        /// </summary>
        /// <remarks>
        /// The send buffer includes the serial driver's send buffer as well as internal buffering in the
        /// <see cref="SerialPortStream"/> itself.
        /// </remarks>
        public int BytesToWrite
        {
            get
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                if (m_Buffer == null) return 0;
                lock (m_Buffer.WriteLock) {
                    int bytesToWrite = m_Buffer.Stream.BytesToWrite;
                    int driverBytes = m_NativeSerial.BytesToWrite;
                    return bytesToWrite > driverBytes ? bytesToWrite : driverBytes;
                }
            }
        }

        /// <summary>
        /// Clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Object is disposed, or disposed during flush operation.</exception>
        /// <exception cref="TimeoutException">Flush write time out exceeded.</exception>
        /// <exception cref="InvalidOperationException">Serial Port not opened.</exception>
        /// <exception cref="IOException">Serial Port was closed during the flush operation;
        /// or there was a device error.</exception>
        public override void Flush()
        {
            if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
            if (!m_NativeSerial.IsOpen) throw new InvalidOperationException("Serial Port not opened");

            bool flushed = m_Buffer.Stream.Flush(m_WriteTimeout);
            if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
            if (!m_NativeSerial.IsOpen) throw new IOException("SerialPortStream was closed during write operation");
            WriteCheckDeviceError();
            if (!flushed) {
                throw new TimeoutException("Flush write time out exceeded");
            }
        }

        private bool WriteCheck(byte[] buffer, int offset, int count)
        {
            if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "Negative offset provided");
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "Negative count provided");
            if (buffer.Length - offset < count) throw new ArgumentException("offset and count exceed buffer boundaries");
            if (count == 0) return false;
            if (!m_NativeSerial.IsOpen) throw new InvalidOperationException("Serial Port is closed");
            WriteCheckDeviceError();

            // Check that count is less than the total size of the buffer, else raise
            // an exception immediately that the local buffer is too small.
            if (count > m_WriteBufferSize) {
                throw new InvalidOperationException("Insufficient buffer for the data requested");
            }
            return true;
        }

        private void WriteCheckDeviceError()
        {
            if (m_Buffer != null) {
                if (m_NativeSerial.IsOpen && !m_NativeSerial.IsRunning) {
                    throw new IOException("Device Error");
                }
            }
        }

        /// <summary>
        /// Write the given data into the buffered serial stream for sending over the serial port.
        /// </summary>
        /// <param name="buffer">The buffer containing data to send.</param>
        /// <param name="offset">Offset into the array buffer where data begins.</param>
        /// <param name="count">Number of bytes to copy into the local buffer.</param>
        /// <exception cref="TimeoutException">
        /// Not enough buffer space was made available before the time out expired.
        /// </exception>
        /// <exception cref="ObjectDisposedException">Object is disposed, or disposed during flush
        /// operation.</exception>
        /// <exception cref="ArgumentNullException"><see langword="null"/> buffer was provided.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Negative offset or negative count provided.</exception>
        /// <exception cref="ArgumentException">Offset and count exceed buffer boundaries.</exception>
        /// <exception cref="InvalidOperationException">Serial port not open.</exception>
        /// <exception cref="IOException">
        /// Serial Port was closed during the flush operation; or there was a device error.
        /// </exception>
        /// <remarks>
        /// Data is copied from the array provided into the local stream buffer. It does not guarantee that data will be
        /// sent over the serial port. So long as there is enough local buffer space to accept the write of count bytes,
        /// this function will succeed. In case that the buffered serial stream doesn't have enough data, the function
        /// will wait up to <see cref="WriteTimeout"/> milliseconds for enough buffer data to become available. In case
        /// that there is not enough space before the write time out expires, no data is copied to the local stream and
        /// the function fails with an exception.
        /// <para>
        /// For reliability, this function will only write data to the write buffer if the complete set of data
        /// requested can be written. This implies that the parameter <paramref name="count"/> be less or equal to the
        /// number of bytes that are available in the write buffer. Equivalently, you must make sure that you have a
        /// write buffer with at least <paramref name="count"/> allocated bytes or this function will always raise an
        /// exception.
        /// </para>
        /// </remarks>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!WriteCheck(buffer, offset, count)) return;
            if (count == 0) return;
            InternalBlockingWrite(buffer, offset, count);
        }

        private void InternalBlockingWrite(byte[] buffer, int offset, int count)
        {
            bool ready = m_Buffer.Stream.WaitForWrite(count, m_WriteTimeout);
            if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
            if (!m_NativeSerial.IsOpen) throw new IOException("SerialPortStream was closed during write operation");

            if (!ready) {
                WriteCheckDeviceError();
                throw new TimeoutException("Couldn't write into buffer");
            }
            InternalWrite(buffer, offset, count);
            WriteCheckDeviceError();
        }

        private void InternalWrite(byte[] buffer, int offset, int count)
        {
            m_Buffer.Stream.Write(buffer, offset, count);
        }

#if NETSTANDARD1_5 || NET45
        /// <summary>
        /// Asynchronously writes a sequence of bytes to the current stream, advances the current position within this
        /// stream by the number of bytes written, and monitors cancellation requests.
        /// </summary>
        /// <param name="buffer">The buffer to write data from.</param>
        /// <param name="offset">
        /// The zero-based byte offset in <paramref name="buffer"/> from which to begin copying bytes to the stream.
        /// </param>
        /// <param name="count">The maximum number of bytes to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        /// <exception cref="ArgumentNullException"><see langword="null"/> buffer was provided.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Negative offset or negative count provided.</exception>
        /// <exception cref="ArgumentException">Offset and count exceed buffer boundaries.</exception>
        /// <exception cref="InvalidOperationException">Serial port not open.</exception>
        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            if (!WriteCheck(buffer, offset, count))
                return Task.FromResult((object)null);

            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            InternalBeginWrite(buffer, offset, count, iar => {
                try {
                    InternalEndWrite(iar);
                    tcs.TrySetResult(null);
                } catch (OperationCanceledException) {
                    tcs.TrySetCanceled();
                } catch (Exception ex) {
                    tcs.TrySetException(ex);
                }
            }, null);
            return tcs.Task;
        }
#endif

#if NET40 || NET45
        /// <summary>
        /// Begins an asynchronous write operation.
        /// </summary>
        /// <param name="buffer">The buffer to write data from.</param>
        /// <param name="offset">The byte offset in buffer from which to begin writing.</param>
        /// <param name="count">The maximum number of bytes to write.</param>
        /// <param name="callback">An optional asynchronous callback, to be called when the write is complete.</param>
        /// <param name="state">A user-provided object that distinguishes this particular asynchronous write request from other requests.</param>
        /// <returns>An IAsyncResult that represents the asynchronous write, which could still be pending.</returns>
        /// <exception cref="ArgumentNullException"><see langword="null"/> buffer was provided.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Negative offset or negative count provided.</exception>
        /// <exception cref="ArgumentException">Offset and count exceed buffer boundaries.</exception>
        /// <exception cref="InvalidOperationException">Serial port not open.</exception>
        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            WriteCheck(buffer, offset, count);

            return InternalBeginWrite(buffer, offset, count, callback, state);
        }

        /// <summary>
        /// Ends an asynchronous write operation.
        /// </summary>
        /// <param name="asyncResult">A reference to the outstanding asynchronous I/O request.</param>
        /// <exception cref="TimeoutException">Not enough buffer space was made available
        /// before the time out expired.</exception>
        /// <exception cref="ObjectDisposedException">Object is disposed, or disposed during flush operation.</exception>
        /// <exception cref="IOException">Serial Port was closed during the flush operation;
        /// or there was a device error.</exception>
        /// <remarks>
        /// EndWrite must be called exactly once on every IAsyncResult from BeginWrite.
        /// </remarks>
        public override void EndWrite(IAsyncResult asyncResult)
        {
            if (asyncResult == null) throw new ArgumentNullException(nameof(asyncResult));

            InternalEndWrite(asyncResult);
        }
#endif

#if !NETSTANDARD1_5
        private delegate void WriteDelegate(byte[] buffer, int offset, int count);
#endif

        private IAsyncResult InternalBeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            if (count == 0 || m_Buffer.Stream.WaitForWrite(count, 0)) {
                LocalAsync ar = new LocalAsync(state);
                if (count > 0) {
                    InternalWrite(buffer, offset, count);
                }
                ar.IsCompleted = true;
                ar.CompletedSynchronously = true;
                if (callback != null) callback(ar);
                return ar;
            } else {
#if NETSTANDARD1_5
                TaskCompletionSource<object> tcs = new TaskCompletionSource<object>(state);
                Task task = new Task(() => {
                    InternalBlockingWrite(buffer, offset, count);
                });

                task.ContinueWith(t => {
                    // Copy the task result into the returned task.
                    if (t.IsFaulted)
                        tcs.TrySetException(t.Exception.InnerExceptions);
                    else if (t.IsCanceled)
                        tcs.TrySetCanceled();
                    else
                        tcs.TrySetResult(null);

                    // Invoke the user callback if necessary.
                    if (callback != null)
                        callback(tcs.Task);
                });

                task.Start();
                return tcs.Task;
#else
                WriteDelegate write = InternalBlockingWrite;
                return write.BeginInvoke(buffer, offset, count, callback, state);
#endif
            }
        }

        private void InternalEndWrite(IAsyncResult asyncResult)
        {
            if (asyncResult is LocalAsync localAsync) {
                if (!localAsync.IsCompleted) localAsync.AsyncWaitHandle.WaitOne(Timeout.Infinite);
                localAsync.Dispose();
            } else {
#if NETSTANDARD1_5
                try {
                    ((Task)asyncResult).Wait();
                } catch (AggregateException ex) {
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                    throw;
                }
#else
                AsyncResult ar = (AsyncResult)asyncResult;
                WriteDelegate caller = (WriteDelegate)ar.AsyncDelegate;

                // This will raise any exceptions from the method InternalBlockingWrite
                caller.EndInvoke(asyncResult);
#endif
            }
        }

        /// <summary>
        /// Writes a specified number of characters to the serial port using data from a buffer.
        /// </summary>
        /// <param name="buffer">The buffer containing data to send.</param>
        /// <param name="offset">Offset into the array buffer where data begins.</param>
        /// <param name="count">Number of characters to copy into the local buffer.</param>
        /// <exception cref="TimeoutException">
        /// Not enough buffer space was made available before the time out expired.
        /// </exception>
        /// <exception cref="ObjectDisposedException">Object is disposed, or disposed during flush operation.</exception>
        /// <exception cref="ArgumentNullException"><see langword="null"/> buffer was provided.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Negative offset or negative count provided.</exception>
        /// <exception cref="ArgumentException">Offset and count exceed buffer boundaries.</exception>
        /// <exception cref="InvalidOperationException">Serial port not open.</exception>
        /// <exception cref="IOException">
        /// Serial Port was closed during the flush operation; or there was a device error.
        /// </exception>
        public void Write(char[] buffer, int offset, int count)
        {
            if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "Negative offset provided");
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "Negative count provided");
            if (buffer.Length - offset < count) throw new ArgumentException("offset and count exceed buffer boundaries");
            if (count == 0) return;
            if (!m_NativeSerial.IsRunning) throw new InvalidOperationException("Serial I/O Thread not running");

            byte[] bbuffer = Encoding.GetBytes(buffer, offset, count);
            if (bbuffer.Length > m_WriteBufferSize) throw new InvalidOperationException("Insufficient buffer for the data requested");
            InternalBlockingWrite(bbuffer, 0, bbuffer.Length);
        }

        /// <summary>
        /// Writes the specified string to the serial port.
        /// </summary>
        /// <param name="text">The string for output.</param>
        /// <exception cref="TimeoutException">Not enough buffer space was made available
        /// before the time out expired.</exception>
        /// <exception cref="ObjectDisposedException">Object is disposed, or disposed during flush operation.</exception>
        /// <exception cref="ArgumentNullException"><see langword="null"/> buffer was provided.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Negative offset or negative count provided.</exception>
        /// <exception cref="ArgumentException">Offset and count exceed buffer boundaries.</exception>
        /// <exception cref="InvalidOperationException">Serial port not open.</exception>
        /// <exception cref="IOException">Serial Port was closed during the flush operation;
        /// or there was a device error.</exception>
        public void Write(string text)
        {
            if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (text.Length == 0) return;
            if (!m_NativeSerial.IsRunning) throw new InvalidOperationException("Serial I/O Thread not running");

            byte[] bbuffer = Encoding.GetBytes(text);
            if (bbuffer.Length > m_WriteBufferSize) throw new InvalidOperationException("Insufficient buffer for the data requested");
            InternalBlockingWrite(bbuffer, 0, bbuffer.Length);
        }

        /// <summary>
        /// Writes the specified string and the NewLine value to the output buffer.
        /// </summary>
        /// <param name="text">The string to write to the output buffer.</param>
        /// <exception cref="TimeoutException">Not enough buffer space was made available
        /// before the time out expired.</exception>
        /// <exception cref="ObjectDisposedException">Object is disposed, or disposed during flush operation.</exception>
        /// <exception cref="ArgumentNullException"><see langword="null"/> buffer was provided.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Negative offset or negative count provided.</exception>
        /// <exception cref="ArgumentException">Offset and count exceed buffer boundaries.</exception>
        /// <exception cref="InvalidOperationException">Serial port not open.</exception>
        /// <exception cref="IOException">Serial Port was closed during the flush operation;
        /// or there was a device error.</exception>
        public void WriteLine(string text)
        {
            Write(text + m_NewLine);
        }

        /// <summary>
        /// Discards data from the serial driver's transmit buffer.
        /// </summary>
        /// <remarks>
        /// Clears the local buffer for data not yet sent to the serial port, as well as
        /// attempting to clear the buffers in the driver itself.
        /// </remarks>
        public void DiscardOutBuffer()
        {
            if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
            if (m_Buffer == null) return;

            // Only valid if the serial port is open. If not, then there's nothing
            // to purge.
            if (m_NativeSerial.IsOpen) m_NativeSerial.DiscardOutBuffer();
        }
        #endregion

        #region Modem Information and Serial State
        /// <summary>
        /// Gets the state of the Carrier Detect line for the port.
        /// </summary>
        /// <remarks>
        /// This property can be used to monitor the state of the carrier detection
        /// line for a port. No carrier usually indicates that the receiver has hung
        /// up and the carrier has been dropped.
        /// <para>Windows documentation sometimes refers to the Carrier Detect line
        /// as the RLSD (Receive Line Signal Detect).</para>
        /// </remarks>
        public bool CDHolding
        {
            get
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                return m_NativeSerial.CDHolding;
            }
        }

        /// <summary>
        /// Gets the state of the Clear-to-Send line.
        /// </summary>
        /// <remarks>
        /// The Clear-to-Send (CTS) line is used in Request to Send/Clear to Send
        /// (RTS/CTS) hardware handshaking. The CTS line is queried by a port before
        /// data is sent.
        /// </remarks>
        public bool CtsHolding
        {
            get
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                return m_NativeSerial.CtsHolding;
            }
        }

        /// <summary>
        /// Gets the state of the Data Set Ready (DSR) signal.
        /// </summary>
        /// <remarks>
        /// This property is used in Data Set Ready/Data Terminal Ready (DSR/DTR) handshaking.
        /// The Data Set Ready (DSR) signal is usually sent by a modem to a port to indicate that
        /// it is ready for data transmission or data reception.
        /// </remarks>
        public bool DsrHolding
        {
            get
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                return m_NativeSerial.DsrHolding;
            }
        }

        /// <summary>
        /// Gets the state of the Ring line signal.
        /// </summary>
        /// <remarks>
        /// The ring line is a separate line from a modem that indicates if there is an incoming
        /// call.
        /// </remarks>
        public bool RingHolding
        {
            get
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                return m_NativeSerial.RingHolding;
            }
        }
        #endregion

        #region Serial Configuration Settings
        /// <summary>
        /// Gets or sets the serial baud rate.
        /// </summary>
        /// <remarks>
        /// The stream doesn't impose any arbitrary limits on setting the baud rate. It is passed
        /// directly to the driver and it is up to the driver to determine if the baud rate is
        /// settable or not. Normally, a driver will attempt to set a baud rate that is within 5%
        /// of the requested baud rate (but not guaranteed).
        /// <para>If the serial driver doesn't support setting the baud rate, setting this property
        /// is silently ignored and the baud rate isn't updated.</para>
        /// </remarks>
        public int BaudRate
        {
            get
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                return m_NativeSerial.BaudRate;
            }
            set
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                m_NativeSerial.BaudRate = value;
            }
        }

        /// <summary>
        /// Gets or sets the standard length of data bits per byte.
        /// </summary>
        /// <remarks>
        /// The range of values for this property is from 5 through 8 and 16. A check is made by setting
        /// this property against the advertised capabilities of the driver. That is, a driver lists
        /// its capabilities to say what byte sizes it can support. If the driver cannot support the byte
        /// size requested, an exception is raised.
        /// <para>Not all possible combinations are allowed by all drivers. That implies, that an exception
        /// may be raised for a valid setting of the DataBits property, if the other parameters are not
        /// valid. Such an example might be that 5-bits are only supported with 2 stop bits and not otherwise.
        /// The driver itself will raise an exception to the application in this case.</para>
        /// <para>If the serial driver doesn't support setting the data bits, setting this property is silently ignored
        /// and the number of data bits isn't updated.</para>
        /// </remarks>
        public int DataBits
        {
            get
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                return m_NativeSerial.DataBits;
            }
            set
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                m_NativeSerial.DataBits = value;
            }
        }

        /// <summary>
        /// Gets or sets the standard number of stop bits per byte.
        /// </summary>
        /// <remarks>
        /// Gets or sets the stop bits that should be used when transmitting and receiving data over the serial
        /// port. If the serial driver doesn't support setting the stop bits, setting this property is silently ignored
        /// and the number of stop bits isn't updated.
        /// </remarks>
        public StopBits StopBits
        {
            get
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                return m_NativeSerial.StopBits;
            }
            set
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                if (!Enum.IsDefined(typeof(StopBits), value))
                    throw new ArgumentOutOfRangeException(nameof(value), "Unknown setting for StopBits");

                m_NativeSerial.StopBits = value;
            }
        }

        /// <summary>
        /// Gets or sets the parity-checking protocol.
        /// </summary>
        /// <remarks>
        /// Parity is an error-checking procedure in which the number of 1s must always be the same — either
        /// even or odd — for each group of bits that is transmitted without error. In modem-to-modem
        /// communications, parity is often one of the parameters that must be agreed upon by sending parties
        /// and receiving parties before transmission can take place.
        /// </remarks>
        public Parity Parity
        {
            get
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                return m_NativeSerial.Parity;
            }
            set
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                if (!Enum.IsDefined(typeof(Parity), value))
                    throw new ArgumentOutOfRangeException(nameof(value), "Unknown setting for Parity");

                m_NativeSerial.Parity = value;
            }
        }

        /// <summary>
        /// Gets or sets the byte that replaces invalid bytes in a data stream when a parity error occurs.
        /// </summary>
        /// <remarks>
        /// If the value is set to the nul character, parity replacement is disabled. This property
        /// only has an effect if the Parity property is not Parity.None.
        /// </remarks>
        public byte ParityReplace
        {
            get
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                return m_NativeSerial.ParityReplace;
            }
            set
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                m_NativeSerial.ParityReplace = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether null bytes are ignored when transmitted
        /// between the port and the receive buffer.
        /// </summary>
        /// <remarks>
        /// This value should normally be set to <see langword="false"/>, especially for binary transmissions. Setting
        /// this property to <see langword="true"/> can cause unexpected results for UTF32- and UTF16-encoded bytes.
        /// </remarks>
        public bool DiscardNull
        {
            get
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                return m_NativeSerial.DiscardNull;
            }
            set
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                m_NativeSerial.DiscardNull = value;
            }
        }

        /// <summary>
        /// Gets or sets a value that enables the Data Terminal Ready (DTR) signal
        /// during serial communication.
        /// </summary>
        /// <remarks>
        /// Data Terminal Ready (DTR) is typically enabled during XON/XOFF software handshaking and
        /// Request to Send/Clear to Send (RTS/CTS) hardware handshaking, and modem communications.
        /// </remarks>
        public bool DtrEnable
        {
            get
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                return m_NativeSerial.DtrEnable;
            }
            set
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                m_NativeSerial.DtrEnable = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Request to Send (RTS) signal is
        /// enabled during serial communication.
        /// </summary>
        /// <remarks>
        /// The Request to Transmit (RTS) signal is typically used in Request to Send/Clear
        /// to Send (RTS/CTS) hardware handshaking.
        /// <para>Note, the windows feature RTS_CONTROL_TOGGLE is not supported by this class.</para>
        /// </remarks>
        public bool RtsEnable
        {
            get
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                return m_NativeSerial.RtsEnable;
            }
            set
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                m_NativeSerial.RtsEnable = value;
            }
        }

        /// <summary>
        /// Gets or sets the handshaking protocol for serial port transmission of data.
        /// </summary>
        /// <remarks>
        /// Enables handshaking on the serial port. We support three types of handshaking:
        /// RTS/CTS; XON/XOFF; and DTR/DTS. The Microsoft implementation SerialPort only supports
        /// RTS/CTS and XON/XOFF.
        /// <para>This method is not 100% compatible with the Microsoft implementation. By disabling
        /// RTS flow control, it sets behaviour so that the RTS line is enabled. You must set the
        /// property RtsEnable as appropriate. Although the Microsoft implementation doesn't support
        /// DSR/DTR at all, setting this property will also set the DTR line to enabled. You must
        /// set the property DtrEnable as appropriate.</para>
        /// <para>When enabling DTR flow control, the DsrSensitivity flag is set, so the driver
        /// ignores any bytes received, unless the DSR modem input line is high. Otherwise, if DTR
        /// flow control is disabled, the DSR line is ignored. For more detailed information about
        /// how windows works with flow control, see the site:
        /// http://msdn.microsoft.com/en-us/library/ms810467.aspx. When DTR flow control is specified,
        /// the fOutxDsrFlow is set along with fDsrSensitivity.</para>
        /// <para>Note, the windows feature RTS_CONTROL_TOGGLE is not supported by this class. This
        /// is also not supported by the Microsoft implementation.</para>
        /// </remarks>
        public Handshake Handshake
        {
            get
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                return m_NativeSerial.Handshake;
            }
            set
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                if (!Enum.IsDefined(typeof(Handshake), value))
                    throw new ArgumentOutOfRangeException(nameof(value), "Unknown setting for Handshake");

                m_NativeSerial.Handshake = value;
            }
        }

        /// <summary>
        /// Define the limit of actual bytes in the transmit buffer when XON is sent.
        /// </summary>
        /// <remarks>
        /// The XOFF character (19 or ^S) is sent when the input buffer comes within <see cref="XOffLimit"/> bytes of being full,
        /// and the XON character (17 or ^Q) is sent when the input buffer comes within <see cref="XOnLimit"/> bytes of being empty.
        /// </remarks>
        public int XOnLimit
        {
            get
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                return m_NativeSerial.XOnLimit;
            }
            set
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                m_NativeSerial.XOnLimit = value;
            }
        }

        /// <summary>
        /// Define the limit of free bytes in the buffer before XOFF is sent.
        /// </summary>
        /// <remarks>
        /// The XOFF character (19 or ^S) is sent when the input buffer comes within <see cref="XOffLimit"/> bytes of being full,
        /// and the XON character (17 or ^Q) is sent when the input buffer comes within <see cref="XOnLimit"/> bytes of being empty.
        /// </remarks>
        public int XOffLimit
        {
            get
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                return m_NativeSerial.XOffLimit;
            }
            set
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                m_NativeSerial.XOffLimit = value;
            }
        }

        /// <summary>
        /// Enable or Disable transmit of data when software flow control is enabled.
        /// </summary>
        /// <remarks>
        /// MSDN documentation states this flag as follows:
        /// <para>If this member is <see langword="true"/>, transmission continues after the input buffer
        /// has come within <see cref="XOffLimit"/> bytes of being full and the driver has transmitted
        /// the XoffChar character to stop receiving bytes. If this member is <see langword="false"/>,
        /// transmission does not continue until the input buffer is within XonLim bytes
        /// of being empty and the driver has transmitted the XonChar character to
        /// resume reception.</para>
        /// <para>When the driver buffer fills up and sends the XOFF character (and
        /// software flow control is active), this property defines if the driver should
        /// continue to send data over the serial port or not.</para>
        /// <para>The Microsoft SerialPort implementation doesn't provide this option
        /// (in fact, in .NET 4.0 it doesn't appear to control this at all).</para>
        /// <para>Some DCE devices will resume sending after any character arrives.
        /// The <see cref="TxContinueOnXOff"/> member should be set to <see langword="false"/> when communicating with
        /// a DCE device that resumes sending after any character arrives.</para>
        /// </remarks>
        public bool TxContinueOnXOff
        {
            get
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                return m_NativeSerial.TxContinueOnXOff;
            }
            set
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                m_NativeSerial.TxContinueOnXOff = value;
            }
        }

        /// <summary>
        /// Gets or sets the break signal state.
        /// </summary>
        /// <remarks>
        /// The break signal state occurs when a transmission is suspended and the line is placed in a break state (all
        /// low, no stop bit) until released. To enter a break state, set this property to <see langword="true"/>. If
        /// the port is already in a break state, setting this property again to <see langword="true"/> does not result
        /// in an exception. It is not possible to write to the <see cref="SerialPortStream"/> while
        /// <see cref="BreakState"/> is <see langword="true"/>.
        /// </remarks>
        public bool BreakState
        {
            get
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                if (!m_NativeSerial.IsOpen) throw new InvalidOperationException("Serial Port not opened");
                return m_NativeSerial.BreakState;
            }
            set
            {
                if (IsDisposed) throw new ObjectDisposedException(nameof(SerialPortStream));
                if (!m_NativeSerial.IsOpen) throw new InvalidOperationException("Serial Port not opened");
                m_NativeSerial.BreakState = value;
            }
        }
        #endregion

        #region Seeking
        /// <summary>
        /// This stream is not seekable, so always returns <see langword="false"/>.
        /// </summary>
        public override bool CanSeek
        {
            get { return false; }
        }

        /// <summary>
        /// This stream does not support the Length property.
        /// </summary>
        /// <exception cref="NotSupportedException">This stream doesn't support the Length property.</exception>
        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        /// <summary>
        /// This stream does not support the Position property.
        /// </summary>
        /// <exception cref="NotSupportedException">This stream doesn't support the Position property.</exception>
        public override long Position
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// This stream does not support seeking.
        /// </summary>
        /// <param name="offset">A byte offset relative to the <paramref name="origin" /> parameter.</param>
        /// <param name="origin">A value of type <see cref="SeekOrigin" /> indicating the reference point used to obtain the new position.</param>
        /// <returns>The new position within the current stream.</returns>
        /// <exception cref="NotSupportedException">This stream doesn't support seeking.</exception>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// This stream does not support the SetLength property.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        /// <exception cref="NotSupportedException">This stream doesn't support the SetLength property.</exception>
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }
        #endregion

        #region Event Handling and Abstraction
        private readonly object m_EventLock = new object();
        private readonly ManualResetEvent m_EventProcessing = new ManualResetEvent(false);
        private SerialData m_SerialDataFlags = SerialData.NoData;
        private SerialError m_SerialErrorFlags = SerialError.NoError;
        private SerialPinChange m_SerialPinChange = SerialPinChange.NoChange;

        private void InitialiseEvents()
        {
            m_NativeSerial.DataReceived += NativeSerial_DataReceived;
            m_NativeSerial.ErrorReceived += NativeSerial_ErrorReceived;
            m_NativeSerial.PinChanged += NativeSerial_PinChanged;
        }

        /// <summary>
        /// Occurs when data is received, or the EOF character is detected by the driver.
        /// </summary>
        public event EventHandler<SerialDataReceivedEventArgs> DataReceived;

        /// <summary>
        /// Called when data is received, or the EOF character is detected by the driver.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="SerialDataReceivedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDataReceived(object sender, SerialDataReceivedEventArgs args)
        {
            EventHandler<SerialDataReceivedEventArgs> handler = DataReceived;
            if (handler != null) {
                handler(sender, args);
            }
        }

        /// <summary>
        /// Occurs when an error condition is detected.
        /// </summary>
        public event EventHandler<SerialErrorReceivedEventArgs> ErrorReceived;

        /// <summary>
        /// Called when an error condition is detected.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="SerialErrorReceivedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnCommError(object sender, SerialErrorReceivedEventArgs args)
        {
            EventHandler<SerialErrorReceivedEventArgs> handler = ErrorReceived;
            if (handler != null) {
                handler(sender, args);
            }
        }

        /// <summary>
        /// Occurs when modem pin changes are detected.
        /// </summary>
        public event EventHandler<SerialPinChangedEventArgs> PinChanged;

        /// <summary>
        /// Called when modem pin changes are detected.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="SerialPinChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPinChanged(object sender, SerialPinChangedEventArgs args)
        {
            EventHandler<SerialPinChangedEventArgs> handler = PinChanged;
            if (handler != null) {
                handler(sender, args);
            }
        }

        private void NativeSerial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            lock (m_EventLock) {
                m_SerialDataFlags |= e.EventType;
            }
            CallEvent();
        }

        private void NativeSerial_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            lock (m_EventLock) {
                m_SerialErrorFlags |= e.EventType;
            }
            CallEvent();
        }

        private void NativeSerial_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            lock (m_EventLock) {
                m_SerialPinChange |= e.EventType;
            }
            CallEvent();
        }

        private void CallEvent()
        {
            lock (m_EventLock) {
                if (IsDisposed || m_EventProcessing.WaitOne(0)) return;
                m_EventProcessing.Set();
            }

            ThreadPool.QueueUserWorkItem(HandleEvent);
        }

        private void HandleEvent(object state)
        {
            SerialData serialDataFlags;
            SerialError serialErrorFlags;
            SerialPinChange serialPinChange;

            bool handleEvent;
            do {
                lock (m_EventLock) {
                    if (IsDisposed) return;
                    handleEvent = m_SerialDataFlags != 0 || m_SerialErrorFlags != 0 || m_SerialPinChange != 0;
                    serialDataFlags = m_SerialDataFlags; m_SerialDataFlags = SerialData.NoData;
                    serialErrorFlags = m_SerialErrorFlags; m_SerialErrorFlags = SerialError.NoError;
                    serialPinChange = m_SerialPinChange; m_SerialPinChange = SerialPinChange.NoChange;
                }

                if (handleEvent) {
                    try {
                        if (m_Log.ShouldTrace(TraceEventType.Verbose))
                            m_Log.TraceEvent(TraceEventType.Verbose, "{0}: HandleEvent: {1}; {2}; {3};", m_NativeSerial.PortName, serialDataFlags, serialErrorFlags, serialPinChange);

                        // Received Data
                        bool aboveThreshold = m_Buffer.Stream.BytesToRead >= m_RxThreshold;
                        if (aboveThreshold) {
                            OnDataReceived(this, new SerialDataReceivedEventArgs(serialDataFlags));
                        } else if ((serialDataFlags & SerialData.Eof) != 0) {
                            OnDataReceived(this, new SerialDataReceivedEventArgs(SerialData.Eof));
                        }

                        // Modem Pin States
                        if ((serialPinChange & SerialPinChange.CtsChanged) != 0) {
                            OnPinChanged(this, new SerialPinChangedEventArgs(SerialPinChange.CtsChanged));
                        }
                        if ((serialPinChange & SerialPinChange.Ring) != 0) {
                            OnPinChanged(this, new SerialPinChangedEventArgs(SerialPinChange.Ring));
                        }
                        if ((serialPinChange & SerialPinChange.CDChanged) != 0) {
                            OnPinChanged(this, new SerialPinChangedEventArgs(SerialPinChange.CDChanged));
                        }
                        if ((serialPinChange & SerialPinChange.DsrChanged) != 0) {
                            OnPinChanged(this, new SerialPinChangedEventArgs(SerialPinChange.DsrChanged));
                        }
                        if ((serialPinChange & SerialPinChange.Break) != 0) {
                            OnPinChanged(this, new SerialPinChangedEventArgs(SerialPinChange.Break));
                        }

                        // Error States
                        if ((serialErrorFlags & SerialError.TXFull) != 0) {
                            OnCommError(this, new SerialErrorReceivedEventArgs(SerialError.TXFull));
                        }
                        if ((serialErrorFlags & SerialError.Frame) != 0) {
                            OnCommError(this, new SerialErrorReceivedEventArgs(SerialError.Frame));
                        }
                        if ((serialErrorFlags & SerialError.RXParity) != 0) {
                            OnCommError(this, new SerialErrorReceivedEventArgs(SerialError.RXParity));
                        }
                        if ((serialErrorFlags & SerialError.Overrun) != 0) {
                            OnCommError(this, new SerialErrorReceivedEventArgs(SerialError.Overrun));
                        }
                        if ((serialErrorFlags & SerialError.RXOver) != 0) {
                            OnCommError(this, new SerialErrorReceivedEventArgs(SerialError.RXOver));
                        }
                    } catch (Exception ex) {
                        if (m_Log.ShouldTrace(TraceEventType.Warning)) {
#if DEBUG
                            m_Log.TraceEvent(TraceEventType.Warning, "{0}: HandleEvent: {1};", m_NativeSerial.PortName, ex.ToString());
#else
                            m_Log.TraceEvent(TraceEventType.Warning, "{0}: HandleEvent: {1};", m_NativeSerial.PortName, ex.Message);
#endif
                        }
                        lock (m_EventLock) {
                            handleEvent = false;
                            m_SerialDataFlags = SerialData.NoData;
                            m_SerialErrorFlags = SerialError.NoError;
                            m_SerialPinChange = SerialPinChange.NoChange;
                        }
                    }
                }
            } while (handleEvent);

            lock (m_EventLock) {
                if (IsDisposed) return;
                m_EventProcessing.Reset();
            }
        }
        #endregion

        private volatile bool m_IsDisposed;

        /// <summary>
        /// Indicates if this object has already been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return m_IsDisposed; }
            private set { m_IsDisposed = value; }
        }

        /// <summary>
        /// Clean up all resources managed by this object.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> if the user is disposing this object,
        /// <see langword="false"/> if being cleaned up by the finalizer.</param>
        protected override void Dispose(bool disposing)
        {
            if (IsDisposed) return;

            if (disposing) {
                // Wait for events to finish before we dispose
                IsDisposed = true;
                bool eventRunning = false;
                lock (m_EventLock) {
                    if (m_EventProcessing.WaitOne(0)) {
                        eventRunning = true;
                    }
                }
                if (eventRunning) m_EventProcessing.WaitOne();
                m_EventProcessing.Dispose();

                if (m_Buffer != null) m_Buffer.Stream.AbortWait();

                m_NativeSerial.Dispose();
                m_NativeSerial = null;

                if (m_Buffer != null) {
                    m_Buffer.Dispose();
                    m_Buffer = null;
                    m_ReadTo = null;
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3358:Ternary operators should not be nested",
            Justification = "Code is still readable")]
        public override string ToString()
        {
            if (IsDisposed) return "SerialPortStream: Disposed";
            if (string.IsNullOrWhiteSpace(PortName)) return "SerialPortStream: Invalid configuration";

            char p;
            switch (Parity) {
            case Parity.Even: p = 'E'; break;
            case Parity.Mark: p = 'M'; break;
            case Parity.None: p = 'N'; break;
            case Parity.Odd: p = 'O'; break;
            case Parity.Space: p = 'S'; break;
            default: p = '?'; break;
            }

            string s;
            switch (StopBits) {
            case StopBits.One: s = "1"; break;
            case StopBits.One5: s = "1.5"; break;
            case StopBits.Two: s = "2"; break;
            default: s = "?"; break;
            }

            string dsrStatus;
            try {
                dsrStatus = ((Handshake & Handshake.Dtr) != 0) ?
                    "hs" : (m_NativeSerial.IsOpen ? (DsrHolding ? "on" : "off") : "-");
            } catch (IOException) {
                dsrStatus = "err";
            }

            string ctsStatus;
            try {
                ctsStatus = ((Handshake & Handshake.Rts) != 0) ?
                    "hs" : (m_NativeSerial.IsOpen ? (CtsHolding ? "on" : "off") : "-");
            } catch (IOException) {
                ctsStatus = "err";
            }

            string dtrStatus;
            try {
                dtrStatus = ((Handshake & Handshake.Dtr) != 0) ?
                    "hs" : (m_NativeSerial.IsOpen ? (DtrEnable ? "on" : "off") : "-");
            } catch (IOException) {
                dtrStatus = "err";
            }

            string rtsStatus;
            try {
                rtsStatus = ((Handshake & Handshake.Rts) != 0) ?
                    "hs" : (m_NativeSerial.IsOpen ? (RtsEnable ? "on" : "off") : "-");
            } catch (IOException) {
                rtsStatus = "err";
            }

            return string.Format("{0}:{1},{2},{3},{4},to={5},xon={6},idsr={7},icts={8},odtr={9},orts={10}",
                PortName, BaudRate, DataBits, p, s,
                TxContinueOnXOff ? "on" : "off",
                ((Handshake & Handshake.XOn) != 0) ? "on" : "off",
                dsrStatus, ctsStatus, dtrStatus, rtsStatus);
        }
    }
}
