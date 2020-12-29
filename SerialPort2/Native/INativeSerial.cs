// Copyright © Jason Curl 2012-2016
// Sources at https://github.com/jcurl/SerialPortStream
// Licensed under the Microsoft Public License (Ms-PL)

namespace RJCP.IO.Ports.Native
{
    using System;

    /// <summary>
    /// Interface for accessing serial based streams.
    /// </summary>
    internal interface INativeSerial : IDisposable
    {
        /// <summary>
        /// Gets the version of the implementation in use.
        /// </summary>
        /// <value>
        /// The version of the implementation in use.
        /// </value>
        string Version { get; }

        /// <summary>
        /// Gets or sets the port device path.
        /// </summary>
        /// <value>
        /// The port device path.
        /// </value>
        string PortName { get; set; }

        /// <summary>
        /// Gets an array of serial port names for the current computer.
        /// </summary>
        /// <returns>An array of serial port names for the current computer.</returns>
        string[] GetPortNames();

        /// <summary>
        /// Gets an array of serial port names and descriptions for the current computer.
        /// </summary>
        /// <remarks>
        /// This method uses the Windows Management Interface to obtain its information. Therefore,
        /// the list may be different to the list obtained using the GetPortNames() method which
        /// uses other techniques.
        /// <para>On Windows 7, this method shows to return normal COM ports, but not those
        /// associated with a modem driver.</para>
        /// </remarks>
        /// <returns>An array of serial ports for the current computer.</returns>
        PortDescription[] GetPortDescriptions();

        /// <summary>
        /// Gets or sets the baud rate.
        /// </summary>
        /// <value>
        /// The baud rate.
        /// </value>
        int BaudRate { get; set; }

        /// <summary>
        /// Gets or sets the data bits.
        /// </summary>
        /// <value>
        /// The data bits.
        /// </value>
        int DataBits { get; set; }

        /// <summary>
        /// Gets or sets the parity.
        /// </summary>
        /// <value>
        /// The parity.
        /// </value>
        Parity Parity { get; set; }

        /// <summary>
        /// Gets or sets the stop bits.
        /// </summary>
        /// <value>
        /// The stop bits.
        /// </value>
        StopBits StopBits { get; set; }

        /// <summary>
        /// Gets or sets a value if null bytes should be discarded or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if null bytes should be discarded; otherwise, <c>false</c>.
        /// </value>
        bool DiscardNull { get; set; }

        /// <summary>
        /// Gets or sets the parity replace byte.
        /// </summary>
        /// <value>
        /// The byte to use on parity errors.
        /// </value>
        byte ParityReplace { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether transmission should still
        /// be sent when the input buffer is full and if the XOff character has been sent.
        /// </summary>
        /// <value>
        ///   <c>true</c> if transmission should continue after the input buffer
        ///   is within <see cref="XOffLimit"/> bytes of being full and the driver has sent the
        ///   XOff character; otherwise, <c>false</c> that transmission should stop
        ///   and only continue when the input buffer is within <see cref="XOnLimit"/> bytes of
        ///   being empty and the driver has sent the XOn character.
        /// </value>
        bool TxContinueOnXOff { get; set; }

        /// <summary>
        /// Gets or sets the XOff limit input when the XOff character should be sent.
        /// </summary>
        /// <value>
        /// The XOff buffer limit.
        /// </value>
        int XOffLimit { get; set; }

        /// <summary>
        /// Gets or sets the XOn limit when the input buffer is below when the XOn character should be sent.
        /// </summary>
        /// <value>
        /// The XOn buffer limit.
        /// </value>
        int XOnLimit { get; set; }

        /// <summary>
        /// Gets or sets the break state of the serial port.
        /// </summary>
        /// <value>
        ///   <c>true</c> if in the break state; otherwise, <c>false</c>.
        /// </value>
        bool BreakState { get; set; }

        /// <summary>
        /// Gets or sets the driver input queue size.
        /// </summary>
        /// <value>
        /// The driver input queue size.
        /// </value>
        /// <remarks>
        /// This method is typically available with Windows API only.
        /// </remarks>
        int DriverInQueue { get; set; }

        /// <summary>
        /// Gets or sets the driver output queue size.
        /// </summary>
        /// <value>
        /// The driver output queue size.
        /// </value>
        /// <remarks>
        /// This method is typically available with Windows API only.
        /// </remarks>
        int DriverOutQueue { get; set; }

        /// <summary>
        /// Gets the number of bytes in the input queue of the driver not yet read (not any managed buffers).
        /// </summary>
        /// <value>
        /// The number of bytes in the driver queue for reading. If this value is not supported, zero is returned.
        /// </value>
        int BytesToRead { get; }

        /// <summary>
        /// Gets the number of bytes in the output buffer of the driver still to write (not any managed buffers).
        /// </summary>
        /// <value>
        /// The number of bytes in the driver queue for writing. If this value is not supported, zero is returned.
        /// </value>
        int BytesToWrite { get; }

        /// <summary>
        /// Gets the state of the Carrier Detect pin on the serial port.
        /// </summary>
        /// <value>
        ///   <c>true</c> if carrier detect pin is active; otherwise, <c>false</c>.
        /// </value>
        bool CDHolding { get; }

        /// <summary>
        /// Gets the state of the Clear To Send pin on the serial port.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the clear to send pin is active; otherwise, <c>false</c>.
        /// </value>
        bool CtsHolding { get; }

        /// <summary>
        /// Gets the state of the Data Set Ready pin on the serial port.
        /// </summary>
        /// <value>
        ///   <c>true</c> if data set ready pin is active; otherwise, <c>false</c>.
        /// </value>
        bool DsrHolding { get; }

        /// <summary>
        /// Gets the state of the Ring Indicator pin on the serial port.
        /// </summary>
        /// <value>
        ///   <c>true</c> if ring indicator state is active; otherwise, <c>false</c>.
        /// </value>
        bool RingHolding { get; }

        /// <summary>
        /// Gets or sets the Data Terminal Ready pin of the serial port.
        /// </summary>
        /// <value>
        ///   <c>true</c> if data terminal pin is active; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This pin only has an effect if handshaking for DTR/DTS is disabled.
        /// </remarks>
        bool DtrEnable { get; set; }

        /// <summary>
        /// Gets or sets the Request To Send pin of the serial port.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [RTS enable]; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This pin only has an effect if the handshaking for RTS/CTS is disabled.
        /// </remarks>
        bool RtsEnable { get; set; }

        /// <summary>
        /// Gets or sets the handshake to use on the serial port.
        /// </summary>
        /// <value>
        /// The handshake mode to use on the serial port.
        /// </value>
        Handshake Handshake { get; set; }

        /// <summary>
        /// Gets a value indicating whether the serial port has been opened.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is open; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This property only indicates if the port has been opened and that the
        /// internal handle is valid.
        /// </remarks>
        bool IsOpen { get; }

        /// <summary>
        /// Discards the input queue buffer of the driver.
        /// </summary>
        void DiscardInBuffer();

        /// <summary>
        /// Discards the output queue buffer of the driver.
        /// </summary>
        void DiscardOutBuffer();

        /// <summary>
        /// Gets the port settings and updates the properties of the object.
        /// </summary>
        void GetPortSettings();

        /// <summary>
        /// Writes the settings of the serial port as set in this object.
        /// </summary>
        void SetPortSettings();

        /// <summary>
        /// Opens the serial port specified by <see cref="PortName"/>.
        /// </summary>
        /// <remarks>
        /// Opening the serial port does not set any settings (such as baud rate, etc.)
        /// </remarks>
        void Open();

        /// <summary>
        /// Closes the serial port.
        /// </summary>
        /// <remarks>
        /// Closing the serial port invalidates actions that can be done to the serial port,
        /// but it does not prevent the serial port from being reopened
        /// </remarks>
        void Close();

        /// <summary>
        /// Creates the serial buffer suitable for monitoring.
        /// </summary>
        /// <param name="readBuffer">The read buffer size to allocate.</param>
        /// <param name="writeBuffer">The write buffer size to allocate.</param>
        /// <returns>A serial buffer object that can be given to <see cref="StartMonitor"/></returns>
        SerialBuffer CreateSerialBuffer(int readBuffer, int writeBuffer);

        /// <summary>
        /// Start the monitor thread, that will watch over the serial port.
        /// </summary>
        /// <param name="buffer">The buffer structure that should be used to read data into
        /// and write data from.</param>
        /// <param name="name">The name of the thread to use.</param>
        void StartMonitor(SerialBuffer buffer, string name);

        /// <summary>
        /// Gets a value indicating whether the thread for monitoring the serial port is running.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is running; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This property differs slightly from <see cref="IsOpen"/>, as this returns status if
        /// the monitoring thread for reading/writing data is actually running. If the thread is
        /// not running for whatever reason, we can expect no data updates in the buffer provided
        /// to <see cref="StartMonitor(SerialBuffer, string)"/>.
        /// </remarks>
        bool IsRunning { get; }

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
    }
}
