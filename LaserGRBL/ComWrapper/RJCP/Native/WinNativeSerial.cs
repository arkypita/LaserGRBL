// Copyright © Jason Curl 2012-2023
// Sources at https://github.com/jcurl/SerialPortStream
// Licensed under the Microsoft Public License (Ms-PL)

namespace RJCP.IO.Ports.Native
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using Microsoft.Win32;
    using Microsoft.Win32.SafeHandles;
    using Trace;
    using Windows;

#if NETSTANDARD1_5
    using System.Reflection;
#endif

    /// <summary>
    /// Windows implementation for a Native Serial connection.
    /// </summary>
    internal class WinNativeSerial : INativeSerial
    {
        private SafeFileHandle m_ComPortHandle;
        private CommProperties m_CommProperties;
        private CommState m_CommState;
        private CommModemStatus m_CommModemStatus;
        private CommOverlappedIo m_CommOverlappedIo;

        private string m_Version;
        private readonly LogSource m_Log;

        public WinNativeSerial() : this(new LogSource()) { }

        public WinNativeSerial(LogSource log)
        {
            m_Log = log;
        }

        /// <summary>
        /// Gets the version of the implementation in use.
        /// </summary>
        /// <value>
        /// The version of the implementation in use.
        /// </value>
        public string Version
        {
            get
            {
                if (m_Version != null) return m_Version;

#if NETSTANDARD1_5
                var assembly = typeof(WinNativeSerial).GetTypeInfo().Assembly;
#else
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
#endif
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                m_Version = fvi.FileVersion;
                return m_Version;
            }
        }

        private string m_PortName;

        /// <summary>
        /// Gets or sets the port device path.
        /// </summary>
        /// <value>
        /// The port device path.
        /// </value>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="InvalidOperationException">Port already open.</exception>
        public string PortName
        {
            get { return m_PortName; }
            set
            {
                if (m_IsDisposed) throw new ObjectDisposedException(nameof(WinNativeSerial));
                if (IsOpen) throw new InvalidOperationException("Port already open");
                m_PortName = value;
            }
        }

        /// <summary>
        /// Gets an array of serial port names for the current computer.
        /// </summary>
        /// <returns>An array of serial port names for the current computer.</returns>
        public string[] GetPortNames()
        {
            using (RegistryKey local = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DEVICEMAP\SERIALCOMM", false)) {
                if (local == null) return new string[0];
                string[] k = local.GetValueNames();
                if (k.Length > 0) {
                    string[] ports = new string[local.ValueCount];
                    for (int i = 0; i < k.Length; i++) {
                        ports[i] = local.GetValue(k[i]) as string;
                    }
                    return ports;
                }
                return new string[0];
            }
        }

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
        public PortDescription[] GetPortDescriptions()
        {
            Dictionary<string, PortDescription> list = new Dictionary<string, PortDescription>();
            using (RegistryKey local = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DEVICEMAP\SERIALCOMM", false)) {
                if (local != null) {
                    string[] k = local.GetValueNames();
                    foreach (string p in k) {
                        string n = local.GetValue(p) as string;
                        list.Add(n, new PortDescription(n, ""));
                    }
                }
            }

            // Use the Windows CfgMgr API to get the device names. This is faster and more reliable than using WMI. It
            // works since Windows XP.
            QueryDevices(list);

            // Get the array and return it
            int i = 0;
            PortDescription[] ports = new PortDescription[list.Count];
            foreach (PortDescription p in list.Values) {
                ports[i++] = p;
            }
            return ports;
        }

        private static void QueryDevices(Dictionary<string, PortDescription> list)
        {
            CfgMgr32.CONFIGRET ret = CfgMgr32.CM_Get_Device_ID_List(null, out string[] instances);
            if (ret != CfgMgr32.CONFIGRET.CR_SUCCESS) return;

            foreach (string device in instances) {
                ret = CfgMgr32.CM_Locate_DevNode(out CfgMgr32.SafeDevInst devInst, device, CfgMgr32.CM_LOCATE_DEVINST.NORMAL);
                if (ret != CfgMgr32.CONFIGRET.CR_SUCCESS) continue;

                using (RegistryKey devKey = GetDeviceKey(devInst)) {
                    if (devKey != null) {
                        if (devKey.GetValue("PortName") is string portName &&
                            list.TryGetValue(portName, out PortDescription port)) {
                            port.Description = GetDeviceProperty(devInst, CfgMgr32.CM_DRP.FRIENDLYNAME) ?? string.Empty;
                            if (string.IsNullOrEmpty(port.Description))
                                port.Description = GetDeviceProperty(devInst, CfgMgr32.CM_DRP.DEVICEDESC) ?? string.Empty;
                            port.Manufacturer = GetDeviceProperty(devInst, CfgMgr32.CM_DRP.MFG) ?? string.Empty;
                        }
                    }
                }
            }
        }

        private static string GetDeviceProperty(CfgMgr32.SafeDevInst devInst, CfgMgr32.CM_DRP deviceProperty)
        {
            CfgMgr32.CONFIGRET ret = CfgMgr32.CM_Get_DevNode_Registry_Property(
                devInst, deviceProperty, out int _, out string buffer);
            if (ret != CfgMgr32.CONFIGRET.CR_SUCCESS)
                return string.Empty;

            return buffer;
        }

        private static RegistryKey GetDeviceKey(CfgMgr32.SafeDevInst devInst)
        {
            CfgMgr32.CONFIGRET ret = CfgMgr32.CM_Open_DevNode_Key(
                devInst, Kernel32.REGSAM.KEY_READ, 0, CfgMgr32.RegDisposition.OpenExisting,
                out SafeRegistryHandle key, 0);
            if (ret != CfgMgr32.CONFIGRET.CR_SUCCESS)
                return null;
            if (key.IsInvalid || key.IsClosed)
                return null;

            return RegistryKey.FromHandle(key);
        }

        private int m_Baud = 115200;

        /// <summary>
        /// Gets or sets the baud rate.
        /// </summary>
        /// <value>
        /// The baud rate.
        /// </value>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="ArgumentOutOfRangeException">Baud rate must be positive.</exception>
        public int BaudRate
        {
            get { return m_Baud; }
            set
            {
                if (m_IsDisposed) throw new ObjectDisposedException(nameof(WinNativeSerial));
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "Baud rate must be positive");

                m_Baud = value;
                if (IsOpen) SetPortSettings();
            }
        }

        private int m_DataBits = 8;

        /// <summary>
        /// Gets or sets the data bits.
        /// </summary>
        /// <value>
        /// The data bits.
        /// </value>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="ArgumentOutOfRangeException">May only be 5, 6, 7, 8 or 16.</exception>
        public int DataBits
        {
            get { return m_DataBits; }
            set
            {
                if (m_IsDisposed) throw new ObjectDisposedException(nameof(WinNativeSerial));
                if ((value < 5 || value > 8) && value != 16) {
                    throw new ArgumentOutOfRangeException(nameof(value), "May only be 5, 6, 7, 8 or 16");
                }
                m_DataBits = value;
                if (IsOpen) SetPortSettings();
            }
        }

        private Parity m_Parity = Parity.None;

        /// <summary>
        /// Gets or sets the parity.
        /// </summary>
        /// <value>
        /// The parity.
        /// </value>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="ArgumentOutOfRangeException">Unknown value for Parity.</exception>
        public Parity Parity
        {
            get { return m_Parity; }
            set
            {
                if (m_IsDisposed) throw new ObjectDisposedException(nameof(WinNativeSerial));
                if (!Enum.IsDefined(typeof(Parity), value)) {
                    throw new ArgumentOutOfRangeException(nameof(value), "Unknown value for Parity");
                }
                m_Parity = value;
                if (IsOpen) SetPortSettings();
            }
        }

        private StopBits m_StopBits = StopBits.One;

        /// <summary>
        /// Gets or sets the stop bits.
        /// </summary>
        /// <value>
        /// The stop bits.
        /// </value>
        /// <exception cref="ArgumentOutOfRangeException">Unknown value for Stop Bits.</exception>
        public StopBits StopBits
        {
            get { return m_StopBits; }
            set
            {
                if (m_IsDisposed) throw new ObjectDisposedException(nameof(WinNativeSerial));
                if (!Enum.IsDefined(typeof(StopBits), value)) {
                    throw new ArgumentOutOfRangeException(nameof(value), "Unknown value for Stop Bits");
                }
                m_StopBits = value;
                if (IsOpen) SetPortSettings();
            }
        }

        private bool m_DiscardNull;

        /// <summary>
        /// Gets or sets a value if null bytes should be discarded or not.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if null bytes should be discarded; otherwise, <see langword="false"/>.
        /// </value>
        public bool DiscardNull
        {
            get { return m_DiscardNull; }
            set
            {
                if (m_IsDisposed) throw new ObjectDisposedException(nameof(WinNativeSerial));
                m_DiscardNull = value;
                if (IsOpen) SetPortSettings();
            }
        }

        private byte m_ParityReplace;

        /// <summary>
        /// Gets or sets the parity replace byte.
        /// </summary>
        /// <value>
        /// The byte to use on parity errors.
        /// </value>
        /// <exception cref="ArgumentOutOfRangeException">Must be a byte value from 0 to 255.</exception>
        public byte ParityReplace
        {
            get { return m_ParityReplace; }
            set
            {
                if (m_IsDisposed) throw new ObjectDisposedException(nameof(WinNativeSerial));
                m_ParityReplace = value;
                if (IsOpen) SetPortSettings();
            }
        }

        private bool m_TxContinueOnXOff;

        /// <summary>
        /// Gets or sets a value indicating whether transmission should still
        /// be sent when the input buffer is full and if the XOff character has been sent.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if transmission should continue after the input buffer
        /// is within <see cref="XOffLimit" /> bytes of being full and the driver has sent the
        /// XOff character; otherwise, <see langword="false"/> that transmission should stop
        /// and only continue when the input buffer is within <see cref="XOnLimit" /> bytes of
        /// being empty and the driver has sent the XOn character.
        /// </value>
        public bool TxContinueOnXOff
        {
            get { return m_TxContinueOnXOff; }
            set
            {
                if (m_IsDisposed) throw new ObjectDisposedException(nameof(WinNativeSerial));
                m_TxContinueOnXOff = value;
                if (IsOpen) SetPortSettings();
            }
        }

        private int m_XOffLimit = 512;

        /// <summary>
        /// Gets or sets the XOff limit input when the XOff character should be sent.
        /// </summary>
        /// <value>
        /// The XOff buffer limit.
        /// </value>
        /// <exception cref="ArgumentOutOfRangeException">XOffLimit must be positive.</exception>
        public int XOffLimit
        {
            get { return m_XOffLimit; }
            set
            {
                if (m_IsDisposed) throw new ObjectDisposedException(nameof(WinNativeSerial));
                if (value < 0) {
                    throw new ArgumentOutOfRangeException(nameof(value), "XOffLimit must be positive");
                }
                m_XOffLimit = value;
                if (IsOpen) SetPortSettings();
            }
        }

        private int m_XOnLimit = 2048;

        /// <summary>
        /// Gets or sets the XOn limit when the input buffer is below when the XOn character should be sent.
        /// </summary>
        /// <value>
        /// The XOn buffer limit.
        /// </value>
        /// <exception cref="ArgumentOutOfRangeException">XOnLimit must be positive.</exception>
        public int XOnLimit
        {
            get { return m_XOnLimit; }
            set
            {
                if (m_IsDisposed) throw new ObjectDisposedException(nameof(WinNativeSerial));
                if (value < 0) {
                    throw new ArgumentOutOfRangeException(nameof(value), "XOnLimit must be positive");
                }
                m_XOnLimit = value;
                if (IsOpen) SetPortSettings();
            }
        }

        private bool m_BreakState;

        /// <summary>
        /// Gets or sets the break state of the serial port.
        /// </summary>
        /// <value>
        ///   <see langword="true"/> if in the break state; otherwise, <see langword="false"/>.
        /// </value>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="InvalidOperationException">Port not open.</exception>
        public bool BreakState
        {
            get
            {
                if (m_IsDisposed) throw new ObjectDisposedException(nameof(WinNativeSerial));
                if (!IsOpen) return false;
                return m_BreakState;
            }
            set
            {
                if (m_IsDisposed) throw new ObjectDisposedException(nameof(WinNativeSerial));
                if (!IsOpen) throw new InvalidOperationException("Port not open");
                if (value) {
                    m_CommModemStatus.SetCommBreak();
                } else {
                    m_CommModemStatus.ClearCommBreak();
                }
                m_BreakState = value;
            }
        }

        private int m_DriverInQueue = 4096;

        /// <summary>
        /// Gets or sets the driver input queue size.
        /// </summary>
        /// <value>
        /// The driver input queue size.
        /// </value>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="ArgumentOutOfRangeException">value must be a positive integer.</exception>
        /// <remarks>
        /// This method is typically available with Windows API only.
        /// </remarks>
        public int DriverInQueue
        {
            get { return m_DriverInQueue; }
            set
            {
                if (m_IsDisposed) throw new ObjectDisposedException(nameof(WinNativeSerial));
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "value must be a positive integer");
                m_DriverInQueue = value;

                if (IsOpen) {
                    Kernel32.SetupComm(m_ComPortHandle, m_DriverInQueue, m_DriverOutQueue);
                }
            }
        }

        private int m_DriverOutQueue = 2048;

        /// <summary>
        /// Gets or sets the driver output queue size.
        /// </summary>
        /// <value>
        /// The driver output queue size.
        /// </value>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="ArgumentOutOfRangeException">value must be a positive integer.</exception>
        /// <remarks>
        /// This method is typically available with Windows API only.
        /// </remarks>
        public int DriverOutQueue
        {
            get { return m_DriverOutQueue; }
            set
            {
                if (m_IsDisposed) throw new ObjectDisposedException(nameof(WinNativeSerial));
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "value must be a positive integer");
                m_DriverOutQueue = value;

                if (IsOpen) {
                    Kernel32.SetupComm(m_ComPortHandle, m_DriverInQueue, m_DriverOutQueue);
                }
            }
        }

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
                if (m_IsDisposed) throw new ObjectDisposedException(nameof(WinNativeSerial));
                if (!IsOpen) return 0;
                return m_CommOverlappedIo.BytesToRead;
            }
        }

        /// <summary>
        /// Gets the number of bytes in the output buffer of the driver still to write (not any managed buffers).
        /// </summary>
        /// <value>
        /// The number of bytes in the driver queue for writing. If this value is not supported, zero is returned.
        /// </value>
        /// <exception cref="ObjectDisposedException"/>
        public int BytesToWrite
        {
            get
            {
                if (m_IsDisposed) throw new ObjectDisposedException(nameof(WinNativeSerial));
                if (!IsOpen) return 0;
                return m_CommOverlappedIo.BytesToWrite;
            }
        }

        /// <summary>
        /// Gets the state of the Carrier Detect pin on the serial port.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if carrier detect pin is active; otherwise, <see langword="false"/>.
        /// </value>
        /// <exception cref="ObjectDisposedException"/>
        public bool CDHolding
        {
            get
            {
                if (m_IsDisposed) throw new ObjectDisposedException(nameof(WinNativeSerial));
                if (!IsOpen) return false;
                m_CommModemStatus.GetCommModemStatus();
                return m_CommModemStatus.Rlsd;
            }
        }

        /// <summary>
        /// Gets the state of the Clear To Send pin on the serial port.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the clear to send pin is active; otherwise, <see langword="false"/>.
        /// </value>
        /// <exception cref="ObjectDisposedException"/>
        public bool CtsHolding
        {
            get
            {
                if (m_IsDisposed) throw new ObjectDisposedException(nameof(WinNativeSerial));
                if (!IsOpen) return false;
                m_CommModemStatus.GetCommModemStatus();
                return m_CommModemStatus.Cts;
            }
        }

        /// <summary>
        /// Gets the state of the Data Set Ready pin on the serial port.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if data set ready pin is active; otherwise, <see langword="false"/>.
        /// </value>
        /// <exception cref="ObjectDisposedException"/>
        public bool DsrHolding
        {
            get
            {
                if (m_IsDisposed) throw new ObjectDisposedException(nameof(WinNativeSerial));
                if (!IsOpen) return false;
                m_CommModemStatus.GetCommModemStatus();
                return m_CommModemStatus.Dsr;
            }
        }

        /// <summary>
        /// Gets the state of the Ring Indicator pin on the serial port.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if ring indicator state is active; otherwise, <see langword="false"/>.
        /// </value>
        /// <exception cref="ObjectDisposedException"/>
        public bool RingHolding
        {
            get
            {
                if (m_IsDisposed) throw new ObjectDisposedException(nameof(WinNativeSerial));
                if (!IsOpen) return false;
                m_CommModemStatus.GetCommModemStatus();
                return m_CommModemStatus.Ring;
            }
        }

        private bool m_DtrEnable = true;

        /// <summary>
        /// Gets or sets the Data Terminal Ready pin of the serial port.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if data terminal pin is active; otherwise, <see langword="false"/>.
        /// </value>
        /// <exception cref="ObjectDisposedException"/>
        /// <remarks>
        /// This pin only has an effect if handshaking for DTR/DTS is disabled. Reading from this
        /// pin returns the state that should be requested. Setting this pin then changing the
        /// handshake mode to disabled results in the DTR pin being set as per this property.
        /// </remarks>
        public bool DtrEnable
        {
            get { return m_DtrEnable; }
            set
            {
                if (m_IsDisposed) throw new ObjectDisposedException(nameof(WinNativeSerial));
                m_DtrEnable = value;
                if (IsOpen && (m_Handshake & Handshake.Dtr) == 0) SetDtrPortSettings(true);
            }
        }

        private bool m_RtsEnable = true;

        /// <summary>
        /// Gets or sets the Request To Send pin of the serial port.
        /// </summary>
        /// <value>
        ///   <see langword="true"/> if RTS is enabled; otherwise, <see langword="false"/>.
        /// </value>
        /// <exception cref="ObjectDisposedException"/>
        /// <remarks>
        /// This pin only has an effect if the handshaking for RTS/CTS is disabled. Reading from this
        /// pin returns the state that should be requested. Setting this pin then changing the
        /// handshake mode to disabled results in the RTS pin being set as per this property.
        /// </remarks>
        public bool RtsEnable
        {
            get { return m_RtsEnable; }
            set
            {
                if (m_IsDisposed) throw new ObjectDisposedException(nameof(WinNativeSerial));
                m_RtsEnable = value;
                if (IsOpen && (m_Handshake & Handshake.Rts) == 0) SetRtsPortSettings(true);
            }
        }

        private Handshake m_Handshake = Handshake.None;

        /// <summary>
        /// Gets or sets the handshake to use on the serial port.
        /// </summary>
        /// <value>
        /// The handshake mode to use on the serial port.
        /// </value>
        /// <exception cref="ArgumentOutOfRangeException">Unknown value for Handshake.</exception>
        public Handshake Handshake
        {
            get { return m_Handshake; }
            set
            {
                if (m_IsDisposed) throw new ObjectDisposedException(nameof(WinNativeSerial));
                if (!Enum.IsDefined(typeof(Handshake), value)) {
                    throw new ArgumentOutOfRangeException(nameof(value), "Unknown value for Handshake");
                }
                m_Handshake = value;
                if (IsOpen) SetPortSettings();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the serial port has been opened.
        /// </summary>
        /// <value>
        ///   <see langword="true"/> if this instance is open; otherwise, <see langword="false"/>.
        /// </value>
        public bool IsOpen
        {
            get
            {
                if (m_IsDisposed) return false;
                SafeFileHandle handle = m_ComPortHandle;
                return handle != null && !handle.IsClosed && !handle.IsInvalid;
            }
        }

        /// <summary>
        /// Discards the input queue buffer of the driver.
        /// </summary>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="InvalidOperationException">Port not open.</exception>
        public void DiscardInBuffer()
        {
            if (m_IsDisposed) throw new ObjectDisposedException(nameof(WinNativeSerial));
            if (!IsOpen) throw new InvalidOperationException("Port not open");

            Kernel32.PurgeComm(m_ComPortHandle, Kernel32.PurgeFlags.PURGE_RXABORT |
                Kernel32.PurgeFlags.PURGE_RXCLEAR);
        }

        public void DiscardOutBuffer()
        {
            if (m_IsDisposed) throw new ObjectDisposedException(nameof(WinNativeSerial));
            if (!IsOpen) throw new InvalidOperationException("Port not open");

            m_CommOverlappedIo.DiscardOutBuffer();
        }

        /// <summary>
        /// Gets the port settings and updates the properties of the object.
        /// </summary>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="InvalidOperationException">Port not open.</exception>
        public void GetPortSettings()
        {
            if (m_IsDisposed) throw new ObjectDisposedException(nameof(WinNativeSerial));
            if (!IsOpen) throw new InvalidOperationException("Port not open");

            m_CommState.GetCommState();
            m_Baud = m_CommState.BaudRate;
            m_DataBits = m_CommState.ByteSize;
            m_Parity = m_CommState.Parity;
            m_StopBits = m_CommState.StopBits;
            m_TxContinueOnXOff = m_CommState.TxContinueOnXOff;
            m_XOnLimit = m_CommState.XonLim;
            m_XOffLimit = m_CommState.XoffLim;
            m_DiscardNull = m_CommState.Null;

            // Get the Error Char. Only if it is zero and parity is enabled, we change it to 126
            // This is because the interface can't provide for an error char of zero and active.
            // This is a limitation of the API as taken over from the System.IO.Ports.SerialPort
            // implementation by Microsoft.
            m_ParityReplace = m_CommState.ErrorChar;
            if (m_ParityReplace == 0 && m_CommState.ErrorCharEnabled && m_Parity != Parity.None) {
                m_ParityReplace = 126;
            }

            m_Handshake = Handshake.None;
            if (m_CommState.RtsControl == RtsControl.Handshake) m_Handshake |= Handshake.Rts;
            if (m_CommState.DtrControl == DtrControl.Handshake) m_Handshake |= Handshake.Dtr;
            if (m_CommState.InX) m_Handshake |= Handshake.XOn;

            // We don't support RTS_CONTROL_TOGGLE
            switch (m_CommState.RtsControl) {
            case RtsControl.Disable:
            case RtsControl.Toggle:
                m_RtsEnable = false;
                break;
            case RtsControl.Enable:
            case RtsControl.Handshake:
                m_RtsEnable = true;
                break;
            }
            m_DtrEnable = m_CommState.DtrControl != DtrControl.Disable;
        }

        /// <summary>
        /// Writes the settings of the serial port as set in this object.
        /// </summary>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="InvalidOperationException">Port not open.</exception>
        public void SetPortSettings()
        {
            if (m_IsDisposed) throw new ObjectDisposedException(nameof(WinNativeSerial));
            if (!IsOpen) throw new InvalidOperationException("Port not open");

            // Binary mode must always be set per MSDN
            m_CommState.Binary = true;
            m_CommState.AbortOnError = false;
            m_CommState.EventChar = 26;
            m_CommState.EofChar = 26;
            m_CommState.XOnChar = 17;
            m_CommState.XOffChar = 19;
            m_CommState.BaudRate = m_Baud;
            m_CommState.ByteSize = m_DataBits;
            m_CommState.StopBits = m_StopBits;
            m_CommState.Null = m_DiscardNull;
            m_CommState.XonLim = m_XOnLimit;
            m_CommState.XoffLim = m_XOffLimit;
            m_CommState.TxContinueOnXOff = m_TxContinueOnXOff;

            m_CommState.ParityEnable = true;
            m_CommState.Parity = m_Parity;
            switch (m_Parity) {
            case Parity.None:
                m_CommState.ErrorCharEnabled = false;
                m_CommState.ErrorChar = 0;
                break;
            case Parity.Even:
            case Parity.Odd:
            case Parity.Space:
            case Parity.Mark:
                if (m_ParityReplace == 0) {
                    m_CommState.ErrorCharEnabled = false;
                    m_CommState.ErrorChar = 0;
                } else {
                    m_CommState.ErrorCharEnabled = true;
                    m_CommState.ErrorChar = m_ParityReplace;
                }
                break;
            default:
                throw new InternalApplicationException("Unknown Parity");
            }

            SetRtsPortSettings(false);
            SetDtrPortSettings(false);
            if ((m_Handshake & Handshake.XOn) != 0) {
                m_CommState.InX = true;
                m_CommState.OutX = true;
            } else {
                m_CommState.InX = false;
                m_CommState.OutX = false;
            }

            m_CommState.SetCommState();

            if (m_CommState.RtsControl != RtsControl.Handshake) m_CommModemStatus.SetRts(m_RtsEnable);
            if (m_CommState.DtrControl != DtrControl.Handshake) m_CommModemStatus.SetDtr(m_DtrEnable);
        }

        private void SetRtsPortSettings(bool immediate)
        {
            if ((m_Handshake & Handshake.Rts) != 0) {
                m_CommState.OutCtsFlow = true;
                m_CommState.RtsControl = RtsControl.Handshake;
            } else {
                m_CommState.OutCtsFlow = false;
                m_CommState.RtsControl = m_RtsEnable ? RtsControl.Enable : RtsControl.Disable;
            }

            if (immediate) {
                m_CommState.SetCommState();
                if (m_CommState.RtsControl != RtsControl.Handshake) m_CommModemStatus.SetRts(m_RtsEnable);
            }
        }

        private void SetDtrPortSettings(bool immediate)
        {
            if ((m_Handshake & Handshake.Dtr) != 0) {
                m_CommState.OutDsrFlow = true;
                m_CommState.DsrSensitivity = true;
                m_CommState.DtrControl = DtrControl.Handshake;
            } else {
                m_CommState.OutDsrFlow = false;
                m_CommState.DsrSensitivity = false;
                m_CommState.DtrControl = m_DtrEnable ? DtrControl.Enable : DtrControl.Disable;
            }

            if (immediate) {
                m_CommState.SetCommState();
                if (m_CommState.DtrControl != DtrControl.Handshake) m_CommModemStatus.SetDtr(m_DtrEnable);
            }
        }

        /// <summary>
        /// Opens the serial port specified by <see cref="PortName" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="InvalidOperationException">
        /// Port must first be set;
        /// or
        /// Serial Port currently open.
        /// </exception>
        /// <exception cref="IOException">Wrong file type.</exception>
        /// <remarks>
        /// Opening the serial port does not set any settings (such as baud rate, etc.). On the windows implementation,
        /// it only sets the internal driver input and output queue.
        /// </remarks>
        public void Open()
        {
            if (m_IsDisposed) throw new ObjectDisposedException(nameof(WinNativeSerial));
            if (string.IsNullOrWhiteSpace(PortName)) throw new InvalidOperationException("Port must first be set");
            if (IsOpen) throw new InvalidOperationException("Serial Port currently open");

            m_ComPortHandle = Kernel32.CreateFile(@"\\.\" + PortName,
                Kernel32.FileAccess.GENERIC_READ | Kernel32.FileAccess.GENERIC_WRITE,
                Kernel32.FileShare.FILE_SHARE_NONE,
                IntPtr.Zero,
                Kernel32.CreationDisposition.OPEN_EXISTING,
                Kernel32.FileAttributes.FILE_FLAG_OVERLAPPED,
                IntPtr.Zero);
            if (m_ComPortHandle.IsInvalid) WinIOError();

            Kernel32.FileType t = Kernel32.GetFileType(m_ComPortHandle);
            bool validOverride = false;
            if (t != Kernel32.FileType.FILE_TYPE_CHAR && t != Kernel32.FileType.FILE_TYPE_UNKNOWN) {
                foreach (string port in GetPortNames()) {
#if NETSTANDARD1_5
                    if (port.Equals(PortName, StringComparison.OrdinalIgnoreCase)) {
                        validOverride = true;
                        break;
                    }
#else
                    if (port.Equals(PortName, StringComparison.InvariantCultureIgnoreCase)) {
                        validOverride = true;
                        break;
                    }
#endif
                }

                if (!validOverride) {
                    m_ComPortHandle.Dispose();
                    m_ComPortHandle = null;
                    throw new IOException(string.Format("Wrong file type: {0}", PortName));
                }
            }

            // Set the default parameters
            Kernel32.SetupComm(m_ComPortHandle, m_DriverInQueue, m_DriverOutQueue);

            m_CommState = new CommState(m_ComPortHandle);
            m_CommProperties = new CommProperties(m_ComPortHandle);
            m_CommModemStatus = new CommModemStatus(m_ComPortHandle);
            m_CommOverlappedIo = new CommOverlappedIo(m_ComPortHandle, m_Log);
            RegisterEvents();
        }

        /// <summary>
        /// Closes the serial port.
        /// </summary>
        /// <exception cref="ObjectDisposedException"/>
        /// <remarks>
        /// Closing the serial port invalidates actions that can be done to the serial port,
        /// but it does not prevent the serial port from being reopened
        /// </remarks>
        public void Close()
        {
            if (m_IsDisposed) throw new ObjectDisposedException(nameof(WinNativeSerial));
            if (IsOpen) {
                SafeFileHandle handle = m_ComPortHandle;
                m_ComPortHandle = null;
                m_CommOverlappedIo.Dispose();
                m_CommOverlappedIo = null;
                m_CommState = null;
                m_CommModemStatus = null;
                handle.Dispose();
            }
        }

        /// <summary>
        /// Creates the serial buffer suitable for monitoring.
        /// </summary>
        /// <param name="readBuffer">The read buffer size to allocate.</param>
        /// <param name="writeBuffer">The write buffer size to allocate.</param>
        /// <returns>A serial buffer object that can be given to <see cref="StartMonitor"/></returns>
        public SerialBuffer CreateSerialBuffer(int readBuffer, int writeBuffer)
        {
            return new SerialBuffer(readBuffer, writeBuffer, true);
        }

        /// <summary>
        /// Start the monitor thread, that will watch over the serial port.
        /// </summary>
        /// <param name="buffer">The buffer structure that should be used to read data into
        /// and write data from.</param>
        /// <param name="name">The name of the thread to use.</param>
        public void StartMonitor(SerialBuffer buffer, string name)
        {
            if (m_IsDisposed) throw new ObjectDisposedException(nameof(WinNativeSerial));
            if (!IsOpen) throw new InvalidOperationException("Serial Port not open");
            m_CommOverlappedIo.Start(buffer, name);
        }

        /// <summary>
        /// Gets a value indicating whether the thread for monitoring the serial port is running.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this instance is running; otherwise, <see langword="false"/>.
        /// </value>
        /// <remarks>
        /// This property differs slightly from <see cref="IsOpen" />, as this returns status if
        /// the monitoring thread for reading/writing data is actually running. If the thread is
        /// not running for whatever reason, we can expect no data updates in the buffer provided
        /// to <see cref="StartMonitor(SerialBuffer, string)" />.
        /// </remarks>
        public bool IsRunning
        {
            get
            {
                if (m_IsDisposed || !IsOpen) return false;
                return m_CommOverlappedIo.IsRunning;
            }
        }

        private void WinIOError()
        {
            int e = Marshal.GetLastWin32Error();

            switch (e) {
            case 2:
            case 3:
                throw new IOException(string.Format("Port not found: {0}", PortName), e);
            case 5:
                throw new UnauthorizedAccessException(string.Format("Access Denied: {0}", PortName));
            case 32:
                throw new IOException(string.Format("Sharing violation: {0}", PortName), e);
            case 206:
                throw new PathTooLongException(string.Format("Path too long: {0}", PortName));
            }
            throw new IOException(string.Format("Unknown error 0x{0}: {1}", e.ToString("X"), PortName), e);
        }

        #region Event Handling
        private void RegisterEvents()
        {
            m_CommOverlappedIo.CommEvent += CommOverlappedIo_CommEvent;
            m_CommOverlappedIo.CommErrorEvent += CommOverlappedIo_CommErrorEvent;
        }

        private const Kernel32.SerialEventMask c_DataFlags =
            Kernel32.SerialEventMask.EV_RXFLAG |
            Kernel32.SerialEventMask.EV_RXCHAR;

        private const Kernel32.SerialEventMask c_PinFlags =
            Kernel32.SerialEventMask.EV_CTS |
            Kernel32.SerialEventMask.EV_RING |
            Kernel32.SerialEventMask.EV_RLSD |
            Kernel32.SerialEventMask.EV_DSR |
            Kernel32.SerialEventMask.EV_BREAK;

        private const Kernel32.ComStatErrors c_ErrorFlags =
            Kernel32.ComStatErrors.CE_TXFULL |
            Kernel32.ComStatErrors.CE_FRAME |
            Kernel32.ComStatErrors.CE_RXPARITY |
            Kernel32.ComStatErrors.CE_OVERRUN |
            Kernel32.ComStatErrors.CE_RXOVER;

        private void CommOverlappedIo_CommEvent(object sender, CommEventArgs e)
        {
            SerialData dataFlags = (SerialData)(e.EventType & c_DataFlags);
            if (dataFlags != 0) {
                OnDataReceived(this, new SerialDataReceivedEventArgs(dataFlags));
            }

            SerialPinChange pinFlags = (SerialPinChange)(e.EventType & c_PinFlags);
            if (pinFlags != 0) {
                OnPinChanged(this, new SerialPinChangedEventArgs(pinFlags));
            }
        }

        private void CommOverlappedIo_CommErrorEvent(object sender, CommErrorEventArgs e)
        {
            SerialError errorFlags = (SerialError)(e.EventType & c_ErrorFlags);
            if (errorFlags != 0) {
                OnCommError(this, new SerialErrorReceivedEventArgs(errorFlags));
            }
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
        #endregion

        #region IDisposable Support
        private bool m_IsDisposed;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
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
            if (m_IsDisposed) return;

            if (disposing) {
                if (IsOpen) Close();
            }

            // Note: the SafeFileHandle will close the object itself when finalising, so
            // we don't need to do it here. It would be different if we managed the handle
            // with an IntPtr however.
            m_IsDisposed = true;
        }
        #endregion
    }
}
