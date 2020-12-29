namespace RJCP.IO.Ports
{
    using System;
    using System.IO;
    using System.Text;
#if NET45
    using System.Threading;
    using System.Threading.Tasks;
#endif

    /// <summary>
    /// Interface for the <see cref="SerialPortStream"/>, which can be used for mocking in unit tests.
    /// </summary>
    public interface ISerialPortStream : IDisposable
    {
        /// <summary>
        /// Get the version of this assembly (or components driving this assembly).
        /// </summary>
        /// <value>The version of the assembly and/or subcomponents.</value>
        string Version { get; }

        /// <summary>
        /// Gets the port for communications, including but not limited to all available COM ports.
        /// </summary>
        /// <remarks>
        /// A list of valid port names can be obtained using the GetPortNames method.
        /// <para>
        /// When changing the port name, and the property UpdateOnPortSet is <b>true</b>, setting this property will
        /// cause the port to be opened, status read and the port then closed. Thus, you can use this behaviour to
        /// determine the actual settings of the port (which remain constant until a program actually changes the port settings).
        /// </para>
        /// <para>
        /// Setting this property to itself, while having UpdateOnPortSet to <b>true</b> has the effect of updating the
        /// local properties based on the current port settings.
        /// </para>
        /// </remarks>
        string PortName { get; set; }

        /// <summary>
        /// Gets a value indicating the open or closed status of the SerialPortStream object.
        /// </summary>
        /// <remarks>
        /// The IsOpen property tracks whether the port is open for use by the caller, not whether the port is open by
        /// any application on the machine.
        /// </remarks>
        /// <value>True if the serial port is open; otherwise, false. The default is false.</value>
        bool IsOpen { get; }

        /// <summary>
        /// Gets or sets the byte encoding for pre- and post-transmission conversion of text.
        /// </summary>
        /// <remarks>
        /// The encoding is used for encoding string information to byte format when sending over the serial port, or
        /// receiving data via the serial port. It is only used with the read/write functions that accept strings (and
        /// not used for byte based reading and writing).
        /// </remarks>
        Encoding Encoding { get; set; }

        /// <summary>
        /// Gets or sets the value used to interpret the end of a call to the ReadLine and WriteLine methods.
        /// </summary>
        /// <remarks>A value that represents the end of a line. The default is a line feed, (NewLine).</remarks>
        string NewLine { get; set; }

        /// <summary>
        /// Specify the driver In Queue at the time it is opened.
        /// </summary>
        /// <remarks>
        /// This provides the driver a recommended internal input buffer, in bytes.
        /// </remarks>
        int DriverInQueue { get; set; }

        /// <summary>
        /// Specify the driver Out Queue at the time it is opened.
        /// </summary>
        /// <remarks>
        /// This provides the driver a recommended internal output buffer, in bytes.
        /// </remarks>
        int DriverOutQueue { get; set; }

        /// <summary>
        /// Gets a value that determines whether the current stream can time out.
        /// </summary>
        /// <returns>A value that determines whether the current stream can time out.</returns>
        bool CanTimeout { get; }

        /// <summary>
        /// Check if this stream supports reading.
        /// </summary>
        /// <remarks>
        /// Supported so long as the stream is not disposed.
        /// </remarks>
        bool CanRead { get; }

        /// <summary>
        /// Define the time out when reading data from the stream.
        /// </summary>
        /// <remarks>
        /// This defines the time out when data arrives in the buffered memory of this stream, that is, when the driver
        /// indicates that data has arrived to the application.
        /// <para>
        /// Should the user perform a read operation and no data is available to copy in the buffer, a time out will occur.
        /// </para>
        /// <para>Set this property to <see cref="SerialPortStream.InfiniteTimeout"/> for an infinite time out.</para>
        /// </remarks>
        int ReadTimeout { get; set; }

        /// <summary>
        /// Gets or sets the size of the SerialPortStream input buffer.
        /// </summary>
        /// <remarks>
        /// Sets the amount of buffering to use when reading data from the serial port. Data is read locally into this
        /// buffered stream through another port.
        /// <para>
        /// The Microsoft implementation uses this to set the buffer size of the underlying driver. This implementation
        /// interprets the ReadBufferSize differently by setting the local buffer which can be much larger (megabytes)
        /// and independent of the low level driver.
        /// </para>
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// An attempt was used to change the size of the buffer while the port is open (and therefore buffering is active).
        /// </exception>
        int ReadBufferSize { get; set; }

        /// <summary>
        /// Gets or sets the number of bytes in the read buffer before a DataReceived event occurs.
        /// </summary>
        int ReceivedBytesThreshold { get; set; }

        /// <summary>
        /// Gets the number of bytes of data in the receive buffer.
        /// </summary>
        /// <remarks>
        /// This method returns the number of bytes available in the input read buffer. Bytes that are cached by the
        /// driver itself are not accounted for, as they haven't yet been read by the local thread.
        /// <para>
        /// This has the effect, that if the local buffer is full (let's say that it is arbitrarily picked to be 64KB)
        /// and the local driver also has buffered 4KB, only the size of the local buffer is given, so 64KB (instead of
        /// the expected 68KB).
        /// </para>
        /// </remarks>
        int BytesToRead { get; }

        /// <summary>
        /// Check if this stream supports writing.
        /// </summary>
        /// <remarks>
        /// Supported so long as the stream is not disposed.
        /// </remarks>
        bool CanWrite { get; }

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
        int WriteTimeout { get; set; }

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
        /// An attempt was used to change the size of the buffer while the port is open (and therefore buffering is active).
        /// </exception>
        int WriteBufferSize { get; set; }

        /// <summary>
        /// Gets the number of bytes of data in the send buffer.
        /// </summary>
        /// <remarks>
        /// The send buffer includes the serial driver's send buffer as well as internal buffering in the
        /// SerialPortStream itself.
        /// </remarks>
        int BytesToWrite { get; }

        /// <summary>
        /// Gets the state of the Carrier Detect line for the port.
        /// </summary>
        /// <remarks>
        /// This property can be used to monitor the state of the carrier detection line for a port. No carrier usually
        /// indicates that the receiver has hung up and the carrier has been dropped.
        /// <para>
        /// Windows documentation sometimes refers to the Carrier Detect line as the RLSD (Receive Line Signal Detect).
        /// </para>
        /// </remarks>
        bool CDHolding { get; }

        /// <summary>
        /// Gets the state of the Clear-to-Send line.
        /// </summary>
        /// <remarks>
        /// The Clear-to-Send (CTS) line is used in Request to Send/Clear to Send (RTS/CTS) hardware handshaking. The CTS
        /// line is queried by a port before data is sent.
        /// </remarks>
        bool CtsHolding { get; }

        /// <summary>
        /// Gets the state of the Data Set Ready (DSR) signal.
        /// </summary>
        /// <remarks>
        /// This property is used in Data Set Ready/Data Terminal Ready (DSR/DTR) handshaking. The Data Set Ready (DSR)
        /// signal is usually sent by a modem to a port to indicate that it is ready for data transmission or data reception.
        /// </remarks>
        bool DsrHolding { get; }

        /// <summary>
        /// Gets the state of the Ring line signal.
        /// </summary>
        /// <remarks>The ring line is a separate line from a modem that indicates if there is an incoming call.</remarks>
        bool RingHolding { get; }

        /// <summary>
        /// Gets or sets the serial baud rate.
        /// </summary>
        /// <remarks>
        /// The stream doesn't impose any arbitrary limits on setting the baud rate. It is passed directly to the driver
        /// and it is up to the driver to determine if the baud rate is settable or not. Normally, a driver will attempt
        /// to set a baud rate that is within 5% of the requested baud rate (but not guaranteed).
        /// <para>
        /// If the serial driver doesn't support setting the baud rate, setting this property is silently ignored and the
        /// baud rate isn't updated.
        /// </para>
        /// </remarks>
        int BaudRate { get; set; }

        /// <summary>
        /// Gets or sets the standard length of data bits per byte.
        /// </summary>
        /// <remarks>
        /// The range of values for this property is from 5 through 8 and 16. A check is made by setting this property
        /// against the advertised capabilities of the driver. That is, a driver lists its capabilities to say what byte
        /// sizes it can support. If the driver cannot support the byte size requested, an exception is raised.
        /// <para>
        /// Not all possible combinations are allowed by all drivers. That implies, that an exception may be raised for a
        /// valid setting of the DataBits property, if the other parameters are not valid. Such an example might be that
        /// 5-bits are only supported with 2 stop bits and not otherwise. The driver itself will raise an exception to
        /// the application in this case.
        /// </para>
        /// <para>
        /// If the serial driver doesn't support setting the data bits, setting this property is silently ignored and the
        /// number of data bits isn't updated.
        /// </para>
        /// </remarks>
        int DataBits { get; set; }

        /// <summary>
        /// Gets or sets the standard number of stop bits per byte.
        /// </summary>
        /// <remarks>
        /// Gets or sets the stop bits that should be used when transmitting and receiving data over the serial port. If
        /// the serial driver doesn't support setting the stop bits, setting this property is silently ignored and the
        /// number of stop bits isn't updated.
        /// </remarks>
        StopBits StopBits { get; set; }

        /// <summary>
        /// Gets or sets the parity-checking protocol.
        /// </summary>
        /// <remarks>
        /// Parity is an error-checking procedure in which the number of 1s must always be the same — either even or odd
        /// — for each group of bits that is transmitted without error. In modem-to-modem communications, parity is often
        /// one of the parameters that must be agreed upon by sending parties and receiving parties before transmission
        /// can take place.
        /// </remarks>
        Parity Parity { get; set; }

        /// <summary>
        /// Gets or sets the byte that replaces invalid bytes in a data stream when a parity error occurs.
        /// </summary>
        /// <remarks>
        /// If the value is set to the null character, parity replacement is disabled. This property only has an effect
        /// if the Parity property is not Parity.None.
        /// </remarks>
        byte ParityReplace { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether null bytes are ignored when transmitted between the port and the
        /// receive buffer.
        /// </summary>
        /// <remarks>
        /// This value should normally be set to false, especially for binary transmissions. Setting this property to
        /// true can cause unexpected results for UTF32- and UTF16-encoded bytes.
        /// </remarks>
        bool DiscardNull { get; set; }

        /// <summary>
        /// Gets or sets a value that enables the Data Terminal Ready (DTR) signal during serial communication.
        /// </summary>
        /// <remarks>
        /// Data Terminal Ready (DTR) is typically enabled during XON/XOFF software handshaking and Request to Send/Clear
        /// to Send (RTS/CTS) hardware handshaking, and modem communications.
        /// </remarks>
        bool DtrEnable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Request to Send (RTS) signal is enabled during serial communication.
        /// </summary>
        /// <remarks>
        /// The Request to Transmit (RTS) signal is typically used in Request to Send/Clear to Send (RTS/CTS) hardware handshaking.
        /// <para>Note, the windows feature RTS_CONTROL_TOGGLE is not supported by this class.</para>
        /// </remarks>
        bool RtsEnable { get; set; }

        /// <summary>
        /// Gets or sets the handshaking protocol for serial port transmission of data.
        /// </summary>
        /// <remarks>
        /// Enables handshaking on the serial port. We support three types of handshaking: RTS/CTS; XON/XOFF; and
        /// DTR/DTS. The Microsoft implementation SerialPort only supports RTS/CTS and XON/XOFF.
        /// <para>
        /// This method is not 100% compatible with the Microsoft implementation. By disabling RTS flow control, it sets
        /// behaviour so that the RTS line is enabled. You must set the property RtsEnable as appropriate. Although the
        /// Microsoft implementation doesn't support DSR/DTR at all, setting this property will also set the DTR line to
        /// enabled. You must set the property DtrEnable as appropriate.
        /// </para>
        /// <para>
        /// When enabling DTR flow control, the DsrSensitivity flag is set, so the driver ignores any bytes received,
        /// unless the DSR modem input line is high. Otherwise, if DTR flow control is disabled, the DSR line is ignored.
        /// For more detailed information about how windows works with flow control, see the site:
        /// http://msdn.microsoft.com/en-us/library/ms810467.aspx. When DTR flow control is specified, the fOutxDsrFlow
        /// is set along with fDsrSensitivity.
        /// </para>
        /// <para>
        /// Note, the windows feature RTS_CONTROL_TOGGLE is not supported by this class. This is also not supported by
        /// the Microsoft implementation.
        /// </para>
        /// </remarks>
        Handshake Handshake { get; set; }

        /// <summary>
        /// Define the limit of actual bytes in the transmit buffer when XON is sent.
        /// </summary>
        /// <remarks>
        /// The XOFF character (19 or ^S) is sent when the input buffer comes within <see cref="XOffLimit"/> bytes of
        /// being full, and the XON character (17 or ^Q) is sent when the input buffer comes within
        /// <see cref="XOnLimit"/> bytes of being empty.
        /// </remarks>
        int XOnLimit { get; set; }

        /// <summary>
        /// Define the limit of free bytes in the buffer before XOFF is sent.
        /// </summary>
        /// <remarks>
        /// The XOFF character (19 or ^S) is sent when the input buffer comes within <see cref="XOffLimit"/> bytes of
        /// being full, and the XON character (17 or ^Q) is sent when the input buffer comes within
        /// <see cref="XOnLimit"/> bytes of being empty.
        /// </remarks>
        int XOffLimit { get; set; }

        /// <summary>
        /// Enable or Disable transmit of data when software flow control is enabled.
        /// </summary>
        /// <remarks>
        /// MSDN documentation states this flag as follows:
        /// <para>
        /// If this member is TRUE, transmission continues after the input buffer has come within <see cref="XOffLimit"/>
        /// bytes of being full and the driver has transmitted the XoffChar character to stop receiving bytes. If this
        /// member is FALSE, transmission does not continue until the input buffer is within XonLim bytes of being empty
        /// and the driver has transmitted the XonChar character to resume reception.
        /// </para>
        /// <para>
        /// When the driver buffer fills up and sends the XOFF character (and software flow control is active), this
        /// property defines if the driver should continue to send data over the serial port or not.
        /// </para>
        /// <para>
        /// The Microsoft SerialPort implementation doesn't provide this option (in fact, in .NET 4.0 it doesn't appear
        /// to control this at all).
        /// </para>
        /// <para>
        /// Some DCE devices will resume sending after any character arrives. The <see cref="TxContinueOnXOff"/> member
        /// should be set to FALSE when communicating with a DCE device that resumes sending after any character arrives.
        /// </para>
        /// </remarks>
        bool TxContinueOnXOff { get; set; }

        /// <summary>
        /// Gets or sets the break signal state.
        /// </summary>
        /// <remarks>
        /// The break signal state occurs when a transmission is suspended and the line is placed in a break state (all
        /// low, no stop bit) until released. To enter a break state, set this property to true. If the port is already
        /// in a break state, setting this property again to true does not result in an exception. It is not possible to
        /// write to the SerialPortStream while BreakState is true.
        /// </remarks>
        bool BreakState { get; set; }

        /// <summary>
        /// This stream is not seekable, so always returns false.
        /// </summary>
        bool CanSeek { get; }

        /// <summary>
        /// This stream does not support the Length property.
        /// </summary>
        /// <exception cref="NotSupportedException">This stream doesn't support the Length property.</exception>
        long Length { get; }

        /// <summary>
        /// This stream does not support the Position property.
        /// </summary>
        /// <exception cref="NotSupportedException">This stream doesn't support the Position property.</exception>
        long Position { get; set; }

        /// <summary>
        /// Indicates if this object has already been disposed.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Update properties based on the current port, overwriting already existing properties.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException"/>
        /// <exception cref="System.InvalidOperationException">Serial Port already opened.</exception>
        /// <remarks>
        /// This method opens the serial port and retrieves the current settings from Windows. These settings are then
        /// made available via the various properties, BaudRate, DataBits, Parity, ParityReplace, Handshake, StopBits,
        /// TxContinueOnXoff, DiscardNull, XOnLimit and XOffLimit.
        /// </remarks>
        void GetPortSettings();

        /// <summary>
        /// Opens a new serial port connection.
        /// </summary>
        /// <exception cref="InvalidOperationException">This object is already managing a serial port connection.</exception>
        /// <exception cref="System.ObjectDisposedException">SerialPortStream is disposed of.</exception>
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
        /// this object through the default constructor. Set the property UpdateOnPortSet to false and then set the Port
        /// property. Provide all the other properties you require then call the Open() method. The port will be opened
        /// using the default properties providing you with a consistent environment (independent of the state of the
        /// Operating System or the driver beforehand).
        /// </para>
        /// </remarks>
        void Open();

        /// <summary>
        /// Opens a new serial port connection with control if the port settings are initialised or not.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">SerialPortStream is disposed of.</exception>
        /// <exception cref="System.InvalidOperationException">Serial Port already opened</exception>
        /// <remarks>
        /// Opens a connection to the serial port provided by the constructor or the Port property. If this object is
        /// already managing a serial port, this object raises an exception.
        /// <para>
        /// You can override the open so that no communication settings are retrieved or set. This is useful for virtual
        /// COM ports that do not manage state bits (some as some emulated COM ports or USB based communications that
        /// present themselves as a COM port but do not have any underlying physical RS232 implementation).
        /// </para>
        /// <note type="note">If you use this method to avoid setting parameters for the serial port, instead only to
        /// open the serial port, you should be careful not to set any properties associated with the serial port, as
        /// they will set the communications properties.</note>
        /// </remarks>
        void OpenDirect();

        /// <summary>
        /// Closes the port connection, sets the IsOpen property to false. Does not dispose the object.
        /// </summary>
        /// <remarks>
        /// This method will clean up the object so far as to close the port. Internal buffers remain active that the
        /// stream can continue to read. Writes will throw an exception.
        /// </remarks>
        void Close();

        /// <summary>
        /// Reads a sequence of bytes from the current stream and advances the position within the stream by the number
        /// of bytes read.
        /// </summary>
        /// <param name="buffer">
        /// An array of bytes. When this method returns, the buffer contains the specified byte array with the values
        /// between <paramref name="offset"/> and ( <paramref name="offset"/>
        /// + <paramref name="count"/> - 1) replaced by the bytes read from the current source.
        /// </param>
        /// <param name="offset">
        /// The zero-based byte offset in <paramref name="buffer"/> at which to begin storing the data read from the
        /// current stream.
        /// </param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="ArgumentNullException">Null buffer provided.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Negative offset provided, or negative count provided.</exception>
        /// <exception cref="ArgumentException">Offset and count exceed buffer boundaries.</exception>
        /// <exception cref="IOException">Device Error (e.g. device removed).</exception>
        /// <returns>
        /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that
        /// many bytes are not currently available, or zero (0) if the end of the stream has been reached.
        /// </returns>
        int Read(byte[] buffer, int offset, int count);

        /// <summary>
        /// Synchronously reads one byte from the SerialPort input buffer.
        /// </summary>
        /// <returns>The byte, cast to an Int32, or -1 if the end of the stream has been read.</returns>
        /// <exception cref="IOException">Device Error (e.g. device removed).</exception>
        int ReadByte();

        /// <summary>
        /// Reads a number of characters from the SerialPortStream input buffer and writes them into an array of
        /// characters at a given offset.
        /// </summary>
        /// <param name="buffer">The character array to write the input to.</param>
        /// <param name="offset">Offset into the buffer where to start putting the data.</param>
        /// <param name="count">Maximum number of bytes to read into the buffer.</param>
        /// <returns>The actual number of bytes copied into the buffer, 0 if there was a time out.</returns>
        /// <exception cref="IOException">Device Error (e.g. device removed).</exception>
        /// <remarks>
        /// This function converts the data in the local buffer to characters based on the encoding defined by the
        /// encoding property. The encoder used may buffer data between calls if characters may require more than one
        /// byte of data for its interpretation as a character.
        /// </remarks>
        int Read(char[] buffer, int offset, int count);

        /// <summary>
        /// Synchronously reads one character from the SerialPortStream input buffer.
        /// </summary>
        /// <returns>The character that was read. -1 indicates no data was available
        /// within the time out.</returns>
        int ReadChar();

        /// <summary>
        /// Reads up to the NewLine value in the input buffer.
        /// </summary>
        /// <returns>The contents of the input buffer up to the first occurrence of
        /// a NewLine value.</returns>
        /// <exception cref="TimeoutException">Data was not available in the timeout specified.</exception>
        /// <exception cref="IOException">Device Error (e.g. device removed).</exception>
        /// <exception cref="ObjectDisposedException"/>
        string ReadLine();

        /// <summary>
        /// Reads a string up to the specified <i>text</i> in the input buffer.
        /// </summary>
        /// <remarks>
        /// The ReadTo() function will read text from the byte buffer up to a predetermined limit (1024 characters) when
        /// looking for the string <i>text</i>. If <i>text</i> is not found within this limit, data is thrown away and
        /// more data is read (effectively consuming the earlier bytes).
        /// <para>
        /// This method is provided as compatibility with the Microsoft implementation. There are some important
        /// differences however. This method attempts to fix a minor pathological problem with the Microsoft
        /// implementation. If the string <i>text</i> is not found, the MS implementation may modify the internal state
        /// of the decoder. As a workaround, it pushes all decoded characters back into its internal byte buffer, which
        /// fixes the problem that a second call to the ReadTo() method returns the consistent results, but a call to
        /// Read(byte[], ..) may return data that was not actually transmitted by the DCE. This would happen in case that
        /// an invalid byte sequence was found, converted to a fall back character. The original byte sequence is removed
        /// and replaced with the byte equivalent of the fall back character.
        /// </para>
        /// <para>This method is rather slow, because it tries to preserve the byte buffer in case of failure.</para>
        /// <para>
        /// In case the data cannot be read, an exception is always thrown. So you may assume that if this method
        /// returns, you have valid data.
        /// </para>
        /// </remarks>
        /// <param name="text">The text to indicate where the read operation stops.</param>
        /// <returns>The contents of the input buffer up to the specified <i>text</i>.</returns>
        /// <exception cref="TimeoutException">Data was not available in the timeout specified.</exception>
        /// <exception cref="IOException">Device Error (e.g. device removed).</exception>
        /// <exception cref="ObjectDisposedException"/>
        string ReadTo(string text);

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
        /// <returns>The contents of the stream and the input buffer of the SerialPortStream.</returns>
        string ReadExisting();

        /// <summary>
        /// Discards data from the serial driver's receive buffer.
        /// </summary>
        /// <remarks>
        /// This function will discard the receive buffer of the SerialPortStream.
        /// </remarks>
        void DiscardInBuffer();

        /// <summary>
        /// Clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">Object is disposed, or disposed during flush operation.</exception>
        /// <exception cref="System.TimeoutException">Flush write time out exceeded.</exception>
        /// <exception cref="System.InvalidOperationException">Serial Port not opened.</exception>
        /// <exception cref="System.IO.IOException">
        /// Serial Port was closed during the flush operation; or there was a device error.
        /// </exception>
        void Flush();

        /// <summary>
        /// Write the given data into the buffered serial stream for sending over the serial port.
        /// </summary>
        /// <param name="buffer">The buffer containing data to send.</param>
        /// <param name="offset">Offset into the array buffer where data begins.</param>
        /// <param name="count">Number of bytes to copy into the local buffer.</param>
        /// <exception cref="System.TimeoutException">
        /// Not enough buffer space was made available before the time out expired.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">Object is disposed, or disposed during flush operation.</exception>
        /// <exception cref="System.ArgumentNullException">NULL buffer was provided.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Negative offset or negative count provided.</exception>
        /// <exception cref="System.ArgumentException">Offset and count exceed buffer boundaries.</exception>
        /// <exception cref="System.InvalidOperationException">Serial port not open.</exception>
        /// <exception cref="System.IO.IOException">
        /// Serial Port was closed during the flush operation; or there was a device error.
        /// </exception>
        /// <remarks>
        /// Data is copied from the array provided into the local stream buffer. It does not guarantee that data will be
        /// sent over the serial port. So long as there is enough local buffer space to accept the write of count bytes,
        /// this function will succeed. In case that the buffered serial stream doesn't have enough data, the function
        /// will wait up to <see cref="SerialPortStream.WriteTimeout"/> milliseconds for enough buffer data to become
        /// available. In case that there is not enough space before the write time out expires, no data is copied to the
        /// local stream and the function fails with an exception.
        /// <para>
        /// For reliability, this function will only write data to the write buffer if the complete set of data requested
        /// can be written. This implies that the parameter <b>count</b> be less or equal to the number of bytes that are
        /// available in the write buffer. Equivalently, you must make sure that you have a write buffer with at least
        /// <b>count</b> allocated bytes or this function will always raise an exception.
        /// </para>
        /// </remarks>
        void Write(byte[] buffer, int offset, int count);

        /// <summary>
        /// Writes a specified number of characters to the serial port using data from a buffer.
        /// </summary>
        /// <param name="buffer">The buffer containing data to send.</param>
        /// <param name="offset">Offset into the array buffer where data begins.</param>
        /// <param name="count">Number of characters to copy into the local buffer.</param>
        /// <exception cref="System.TimeoutException">
        /// Not enough buffer space was made available before the time out expired.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">Object is disposed, or disposed during flush operation.</exception>
        /// <exception cref="System.ArgumentNullException">NULL buffer was provided.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Negative offset or negative count provided.</exception>
        /// <exception cref="System.ArgumentException">Offset and count exceed buffer boundaries.</exception>
        /// <exception cref="System.InvalidOperationException">Serial port not open.</exception>
        /// <exception cref="System.IO.IOException">
        /// Serial Port was closed during the flush operation; or there was a device error.
        /// </exception>
        void Write(char[] buffer, int offset, int count);

        /// <summary>
        /// Writes the specified string to the serial port.
        /// </summary>
        /// <param name="text">The string for output.</param>
        /// <exception cref="System.TimeoutException">
        /// Not enough buffer space was made available before the time out expired.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">Object is disposed, or disposed during flush operation.</exception>
        /// <exception cref="System.ArgumentNullException">NULL buffer was provided.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Negative offset or negative count provided.</exception>
        /// <exception cref="System.ArgumentException">Offset and count exceed buffer boundaries.</exception>
        /// <exception cref="System.InvalidOperationException">Serial port not open.</exception>
        /// <exception cref="System.IO.IOException">
        /// Serial Port was closed during the flush operation; or there was a device error.
        /// </exception>
        void Write(string text);

        /// <summary>
        /// Writes the specified string and the NewLine value to the output buffer.
        /// </summary>
        /// <param name="text">The string to write to the output buffer.</param>
        /// <exception cref="System.TimeoutException">
        /// Not enough buffer space was made available before the time out expired.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">Object is disposed, or disposed during flush operation.</exception>
        /// <exception cref="System.ArgumentNullException">NULL buffer was provided.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Negative offset or negative count provided.</exception>
        /// <exception cref="System.ArgumentException">Offset and count exceed buffer boundaries.</exception>
        /// <exception cref="System.InvalidOperationException">Serial port not open.</exception>
        /// <exception cref="System.IO.IOException">
        /// Serial Port was closed during the flush operation; or there was a device error.
        /// </exception>
        void WriteLine(string text);

        /// <summary>
        /// Discards data from the serial driver's transmit buffer.
        /// </summary>
        /// <remarks>
        /// Clears the local buffer for data not yet sent to the serial port, as well as attempting to clear the buffers
        /// in the driver itself.
        /// </remarks>
        void DiscardOutBuffer();

        /// <summary>
        /// This stream does not support seeking.
        /// </summary>
        /// <param name="offset">A byte offset relative to the <paramref name="origin"/> parameter.</param>
        /// <param name="origin">
        /// A value of type <see cref="T:System.IO.SeekOrigin"/> indicating the reference point used to obtain the new position.
        /// </param>
        /// <returns>The new position within the current stream.</returns>
        /// <exception cref="System.NotSupportedException">This stream doesn't support seeking.</exception>
        long Seek(long offset, SeekOrigin origin);

        /// <summary>
        /// This stream does not support the SetLength property.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        /// <exception cref="System.NotSupportedException">This stream doesn't support the SetLength property.</exception>
        void SetLength(long value);

        /// <summary>
        /// Occurs when data is received, or the EOF character is detected by the driver.
        /// </summary>
        event EventHandler<SerialDataReceivedEventArgs> DataReceived;

        /// <summary>
        /// Occurs when an error condition is detected.
        /// </summary>
        event EventHandler<SerialErrorReceivedEventArgs> ErrorReceived;

        /// <summary>
        /// Occurs when modem pin changes are detected.
        /// </summary>
        event EventHandler<SerialPinChangedEventArgs> PinChanged;

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        string ToString();

        /// <summary>
        /// Writes a byte to the current position in the stream and advances the position within the stream by one byte.
        /// </summary>
        /// <param name="value">The byte to write to the stream.</param>
        void WriteByte(byte value);

        /// <summary>
        /// Reads the bytes from the current stream and writes them to another stream.
        /// </summary>
        /// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
        void CopyTo(Stream destination);

        /// <summary>
        /// Reads the bytes from the current stream and writes them to another stream, using a specified buffer size.
        /// </summary>
        /// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
        /// <param name="bufferSize">
        /// The size of the buffer. This value must be greater than zero. The default size is 81920.
        /// </param>
        void CopyTo(Stream destination, int bufferSize);

#if NET45
        /// <summary>
        /// Asynchronously reads the bytes from the current stream and writes them to another stream.
        /// </summary>
        /// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
        /// <returns>A task that represents the asynchronous copy operation.</returns>
        Task CopyToAsync(Stream destination);

        /// <summary>
        /// Asynchronously reads the bytes from the current stream and writes them to another stream, using a specified
        /// buffer size.
        /// </summary>
        /// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
        /// <param name="bufferSize">
        /// The size, in bytes, of the buffer. This value must be greater than zero. The default size is 81920.
        /// </param>
        /// <returns>A task that represents the asynchronous copy operation.</returns>
        Task CopyToAsync(Stream destination, int bufferSize);

        /// <summary>
        /// Asynchronously reads the bytes from the current stream and writes them to another stream, using a specified
        /// buffer size and cancellation token.
        /// </summary>
        /// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
        /// <param name="bufferSize">
        /// The size, in bytes, of the buffer. This value must be greater than zero. The default size is 81920.
        /// </param>
        /// <param name="cancellationToken">
        /// The token to monitor for cancellation requests. The default value is
        /// <see cref="System.Threading.CancellationToken.None"/> .
        /// </param>
        /// <returns>A task that represents the asynchronous copy operation.</returns>
        Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously clears all buffers for this stream and causes any buffered data to be written to the
        /// underlying device.
        /// </summary>
        /// <returns>A task that represents the asynchronous flush operation.</returns>
        Task FlushAsync();

        /// <summary>
        /// Asynchronously clears all buffers for this stream, causes any buffered data to be written to the underlying
        /// device, and monitors cancellation requests.
        /// </summary>
        /// <param name="cancellationToken">
        /// The token to monitor for cancellation requests. The default value is
        /// <see cref="System.Threading.CancellationToken.None"/> .
        /// </param>
        /// <returns>A task that represents the asynchronous copy operation.</returns>
        Task FlushAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously reads a sequence of bytes from the current stream and advances the position within the stream
        /// by the number of bytes read.
        /// </summary>
        /// <param name="buffer">The buffer to write the data into.</param>
        /// <param name="offset">
        /// The byte offset in <paramref name="buffer"/> at which to begin writing data from the stream.
        /// </param>
        /// <param name="count">The maximum number of bytes to read.</param>
        /// <returns>
        /// TA task that represents the asynchronous read operation. The value of the TResult parameter contains the
        /// total number of bytes read into the buffer. The result value can be less than the number of bytes requested
        /// if the number of bytes currently available is less than the requested number, or it can be 0 (zero) if the
        /// end of the stream has been reached.
        /// </returns>
        Task<int> ReadAsync(byte[] buffer, int offset, int count);

        /// <summary>
        /// Asynchronously reads a sequence of bytes from the current stream, advances the position within the stream by
        /// the number of bytes read, and monitors cancellation requests.
        /// </summary>
        /// <param name="buffer">The buffer to write the data into.</param>
        /// <param name="offset">
        /// The byte offset in <paramref name="buffer"/> at which to begin writing data from the stream.
        /// </param>
        /// <param name="count">The maximum number of bytes to read.</param>
        /// <param name="cancellationToken">
        /// The token to monitor for cancellation requests. The default value is
        /// <see cref="System.Threading.CancellationToken.None"/> .
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The value of the TResult parameter contains the total
        /// number of bytes read into the buffer. The result value can be less than the number of bytes requested if the
        /// number of bytes currently available is less than the requested number, or it can be 0 (zero) if the end of
        /// the stream has been reached.
        /// </returns>
        Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken);

        /// <summary>
        /// Writes the asynchronous.
        /// </summary>
        /// <param name="buffer">The buffer to write data from.</param>
        /// <param name="offset">
        /// The zero-based byte offset in <paramref name="buffer"/> from which to begin copying bytes to the stream.
        /// </param>
        /// <param name="count">The maximum number of bytes to write.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        Task WriteAsync(byte[] buffer, int offset, int count);

        /// <summary>
        /// Asynchronously writes a sequence of bytes to the current stream, advances the current position within this
        /// stream by the number of bytes written, and monitors cancellation requests.
        /// </summary>
        /// <param name="buffer">The buffer to write data from.</param>
        /// <param name="offset">
        /// The zero-based byte offset in <paramref name="buffer"/> from which to begin copying bytes to the stream.
        /// </param>
        /// <param name="count">The maximum number of bytes to write.</param>
        /// <param name="cancellationToken">
        /// The token to monitor for cancellation requests. The default value is
        /// <see cref="System.Threading.CancellationToken.None"/> .
        /// </param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken);
#endif
    }
}