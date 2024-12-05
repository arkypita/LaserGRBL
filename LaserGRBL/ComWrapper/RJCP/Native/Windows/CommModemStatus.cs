// Copyright © Jason Curl 2012-2023
// Sources at https://github.com/jcurl/SerialPortStream
// Licensed under the Microsoft Public License (Ms-PL)

namespace RJCP.IO.Ports.Native.Windows
{
    using System.IO;
    using System.Runtime.InteropServices;
    using Microsoft.Win32.SafeHandles;

    /// <summary>
    /// Abstracts the Win32 API GetCommModemStatus().
    /// </summary>
    internal sealed class CommModemStatus
    {
        private readonly SafeFileHandle m_ComPortHandle;
        private Kernel32.ModemStat m_ModemStatus;

        internal CommModemStatus(SafeFileHandle handle)
        {
            m_ComPortHandle = handle;
        }

        public void GetCommModemStatus()
        {
            if (!Kernel32.GetCommModemStatus(m_ComPortHandle, out Kernel32.ModemStat s)) {
                throw new IOException("Unable to get serial port modem state", Marshal.GetLastWin32Error());
            }

            m_ModemStatus = s;
        }

        public bool Cts { get { return (m_ModemStatus & Kernel32.ModemStat.MS_CTS_ON) != 0; } }

        public bool Dsr { get { return (m_ModemStatus & Kernel32.ModemStat.MS_DSR_ON) != 0; } }

        public bool Ring { get { return (m_ModemStatus & Kernel32.ModemStat.MS_RING_ON) != 0; } }

        public bool Rlsd { get { return (m_ModemStatus & Kernel32.ModemStat.MS_RLSD_ON) != 0; } }

        public void ClearCommBreak()
        {
            if (!Kernel32.ClearCommBreak(m_ComPortHandle)) {
                throw new IOException("Unable to clear the serial break state", Marshal.GetLastWin32Error());
            }
        }

        public void SetCommBreak()
        {
            if (!Kernel32.SetCommBreak(m_ComPortHandle)) {
                throw new IOException("Unable to set the serial break state", Marshal.GetLastWin32Error());
            }
        }

        public void SetDtr(bool value)
        {
            if (!Kernel32.EscapeCommFunction(m_ComPortHandle, value ? Kernel32.ExtendedFunctions.SETDTR : Kernel32.ExtendedFunctions.CLRDTR)) {
                throw new IOException("Unable to set DTR state explicitly", Marshal.GetLastWin32Error());
            }
        }

        public void SetRts(bool value)
        {
            if (!Kernel32.EscapeCommFunction(m_ComPortHandle, value ? Kernel32.ExtendedFunctions.SETRTS : Kernel32.ExtendedFunctions.CLRRTS)) {
                throw new IOException("Unable to set RTS state explicitly", Marshal.GetLastWin32Error());
            }
        }
    }
}
