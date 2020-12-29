// Copyright © Jason Curl 2012-2017
// Sources at https://github.com/jcurl/SerialPortStream
// Licensed under the Microsoft Public License (Ms-PL)

namespace RJCP.IO.Ports.Native.Unix
{
    using System;
    using System.Runtime.InteropServices;

    internal class SerialUnix : INativeSerialDll
    {
        [ThreadStatic]
        private static int m_ErrNo;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S2696:Instance members should not write to \"static\" fields",
            Justification = "P/Invoke and this is ThreadStatic. Same behaviour as libc")]
        public int errno
        {
            get { return m_ErrNo; }
            set { m_ErrNo = value; }
        }

        public string serial_version()
        {
            IntPtr version = SafeNativeMethods.serial_version();
            return Marshal.PtrToStringAnsi(version);
        }

        public SafeSerialHandle serial_init()
        {
            SafeSerialHandle result = UnsafeNativeMethods.serial_init();
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public void serial_terminate(SafeSerialHandle handle)
        {
            handle.Dispose();
        }

        public PortDescription[] serial_getports(SafeSerialHandle handle)
        {
            // The portdesc is an array of two string pointers, where the last element is zero
            IntPtr portdesc;
#if !NETSTANDARD15
            portdesc = UnsafeNativeMethods.serial_getports(handle);
#else
            try {
                portdesc = UnsafeNativeMethods.serial_getports(handle);
            } catch (Exception ex) {
                // Ugly hack as .NET Standard doesn't have EntryPointNotFoundException, so if we
                // get any exception, we raise this one instead. To remove if it ever becomes
                // available.
                throw new EntryPointNotFoundException(ex.Message, ex);
            }
#endif
            errno = Marshal.GetLastWin32Error();
            if (portdesc.Equals(IntPtr.Zero)) return null;

            // Get the number of ports in the system.
            int portNum = 0;
            IntPtr portName;
            do {
                portName = Marshal.ReadIntPtr(portdesc, portNum * 2 * IntPtr.Size);
                if (portName != IntPtr.Zero) portNum++;
            } while (portName != IntPtr.Zero);

            // Copy them into our struct
            PortDescription[] ports = new PortDescription[portNum];
            for (int i = 0; i < portNum; i++) {
                IntPtr portPtr = Marshal.ReadIntPtr(portdesc, i * 2 * IntPtr.Size);
                string port = Marshal.PtrToStringAnsi(portPtr);
                IntPtr descPtr = Marshal.ReadIntPtr(portdesc, i * 2 * IntPtr.Size + IntPtr.Size);
                string desc;
                if (descPtr.Equals(IntPtr.Zero)) {
                    desc = string.Empty;
                } else {
                    desc = Marshal.PtrToStringAnsi(descPtr);
                }
                ports[i] = new PortDescription(port, desc);
            }

            return ports;
        }

        public int serial_setdevicename(SafeSerialHandle handle, string deviceName)
        {
            int result = UnsafeNativeMethods.serial_setdevicename(handle, deviceName);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public string serial_getdevicename(SafeSerialHandle handle)
        {
            IntPtr deviceName = UnsafeNativeMethods.serial_getdevicename(handle);
            errno = Marshal.GetLastWin32Error();
            if (deviceName.Equals(IntPtr.Zero)) return null;
            return Marshal.PtrToStringAnsi(deviceName);
        }

        public int serial_setbaud(SafeSerialHandle handle, int baud)
        {
            int result = UnsafeNativeMethods.serial_setbaud(handle, baud);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_getbaud(SafeSerialHandle handle, out int baud)
        {
            int result = UnsafeNativeMethods.serial_getbaud(handle, out baud);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_setdatabits(SafeSerialHandle handle, int databits)
        {
            int result = UnsafeNativeMethods.serial_setdatabits(handle, databits);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_getdatabits(SafeSerialHandle handle, out int databits)
        {
            int result = UnsafeNativeMethods.serial_getdatabits(handle, out databits);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_setparity(SafeSerialHandle handle, Parity parity)
        {
            int result = UnsafeNativeMethods.serial_setparity(handle, parity);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_getparity(SafeSerialHandle handle, out Parity parity)
        {
            int result = UnsafeNativeMethods.serial_getparity(handle, out parity);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_setstopbits(SafeSerialHandle handle, StopBits stopbits)
        {
            int result = UnsafeNativeMethods.serial_setstopbits(handle, stopbits);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_getstopbits(SafeSerialHandle handle, out StopBits stopbits)
        {
            int result = UnsafeNativeMethods.serial_getstopbits(handle, out stopbits);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_setdiscardnull(SafeSerialHandle handle, bool discardNull)
        {
            int result = UnsafeNativeMethods.serial_setdiscardnull(handle, discardNull);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_getdiscardnull(SafeSerialHandle handle, out bool discardNull)
        {
            int result = UnsafeNativeMethods.serial_getdiscardnull(handle, out discardNull);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_setparityreplace(SafeSerialHandle handle, int parityReplace)
        {
            int result = UnsafeNativeMethods.serial_setparityreplace(handle, parityReplace);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_getparityreplace(SafeSerialHandle handle, out int parityReplace)
        {
            int result = UnsafeNativeMethods.serial_getparityreplace(handle, out parityReplace);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_settxcontinueonxoff(SafeSerialHandle handle, bool txContinueOnXOff)
        {
            int result = UnsafeNativeMethods.serial_settxcontinueonxoff(handle, txContinueOnXOff);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_gettxcontinueonxoff(SafeSerialHandle handle, out bool txContinueOnXOff)
        {
            int result = UnsafeNativeMethods.serial_gettxcontinueonxoff(handle, out txContinueOnXOff);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_setxofflimit(SafeSerialHandle handle, int xoffLimit)
        {
            int result = UnsafeNativeMethods.serial_setxofflimit(handle, xoffLimit);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_getxofflimit(SafeSerialHandle handle, out int xoffLimit)
        {
            int result = UnsafeNativeMethods.serial_getxofflimit(handle, out xoffLimit);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_setxonlimit(SafeSerialHandle handle, int xonLimit)
        {
            int result = UnsafeNativeMethods.serial_setxonlimit(handle, xonLimit);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_getxonlimit(SafeSerialHandle handle, out int xonLimit)
        {
            int result = UnsafeNativeMethods.serial_getxonlimit(handle, out xonLimit);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_sethandshake(SafeSerialHandle handle, Handshake handshake)
        {
            int result = UnsafeNativeMethods.serial_sethandshake(handle, handshake);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_gethandshake(SafeSerialHandle handle, out Handshake handshake)
        {
            int result = UnsafeNativeMethods.serial_gethandshake(handle, out handshake);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_open(SafeSerialHandle handle)
        {
            int result = UnsafeNativeMethods.serial_open(handle);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_close(SafeSerialHandle handle)
        {
            int result = UnsafeNativeMethods.serial_close(handle);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_isopen(SafeSerialHandle handle, out bool isOpen)
        {
            int result = UnsafeNativeMethods.serial_isopen(handle, out isOpen);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_setproperties(SafeSerialHandle handle)
        {
            int result = UnsafeNativeMethods.serial_setproperties(handle);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_getproperties(SafeSerialHandle handle)
        {
            int result = UnsafeNativeMethods.serial_getproperties(handle);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_getdcd(SafeSerialHandle handle, out bool dcd)
        {
            int result = UnsafeNativeMethods.serial_getdcd(handle, out dcd);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_getri(SafeSerialHandle handle, out bool ri)
        {
            int result = UnsafeNativeMethods.serial_getri(handle, out ri);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_getdsr(SafeSerialHandle handle, out bool dsr)
        {
            int result = UnsafeNativeMethods.serial_getdsr(handle, out dsr);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_getcts(SafeSerialHandle handle, out bool cts)
        {
            int result = UnsafeNativeMethods.serial_getcts(handle, out cts);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_setdtr(SafeSerialHandle handle, bool dtr)
        {
            int result = UnsafeNativeMethods.serial_setdtr(handle, dtr);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_getdtr(SafeSerialHandle handle, out bool dtr)
        {
            int result = UnsafeNativeMethods.serial_getdtr(handle, out dtr);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_setrts(SafeSerialHandle handle, bool rts)
        {
            int result = UnsafeNativeMethods.serial_setrts(handle, rts);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_getrts(SafeSerialHandle handle, out bool rts)
        {
            int result = UnsafeNativeMethods.serial_getrts(handle, out rts);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_setbreak(SafeSerialHandle handle, bool breakState)
        {
            int result = UnsafeNativeMethods.serial_setbreak(handle, breakState);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_getbreak(SafeSerialHandle handle, out bool breakState)
        {
            int result = UnsafeNativeMethods.serial_getbreak(handle, out breakState);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public string serial_error(SafeSerialHandle handle)
        {
            IntPtr errorString = UnsafeNativeMethods.serial_error(handle);
            errno = Marshal.GetLastWin32Error();
            if (errorString.Equals(IntPtr.Zero)) return null;
            return Marshal.PtrToStringAnsi(errorString);
        }

        public SerialReadWriteEvent serial_waitforevent(SafeSerialHandle handle, SerialReadWriteEvent rwevent, int timeout)
        {
            SerialReadWriteEvent result = UnsafeNativeMethods.serial_waitforevent(handle, rwevent, timeout);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_abortwaitforevent(SafeSerialHandle handle)
        {
            int result = UnsafeNativeMethods.serial_abortwaitforevent(handle);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_read(SafeSerialHandle handle, IntPtr data, int length)
        {
            int result = UnsafeNativeMethods.serial_read(handle, data, length);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_write(SafeSerialHandle handle, IntPtr data, int length)
        {
            int result = UnsafeNativeMethods.serial_write(handle, data, length);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public WaitForModemEvent serial_waitformodemevent(SafeSerialHandle handle, WaitForModemEvent mevent)
        {
            WaitForModemEvent result = UnsafeNativeMethods.serial_waitformodemevent(handle, mevent);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_abortwaitformodemevent(SafeSerialHandle handle)
        {
            int result = UnsafeNativeMethods.serial_abortwaitformodemevent(handle);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_discardinbuffer(SafeSerialHandle handle)
        {
            int result = UnsafeNativeMethods.serial_discardinbuffer(handle);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public int serial_discardoutbuffer(SafeSerialHandle handle)
        {
            int result = UnsafeNativeMethods.serial_discardoutbuffer(handle);
            errno = Marshal.GetLastWin32Error();
            return result;
        }

        public SysErrNo netfx_errno(int errno)
        {
#if !NETSTANDARD15
            return (SysErrNo)SafeNativeMethods.netfx_errno(errno);
#else
            try {
                return (SysErrNo)SafeNativeMethods.netfx_errno(errno);
            } catch (Exception ex) {
                // Ugly hack as .NET Standard doesn't have EntryPointNotFoundException, so if we
                // get any exception, we raise this one instead. To remove if it ever becomes
                // available.
                throw new EntryPointNotFoundException(ex.Message, ex);
            }
#endif
        }

        public string netfx_errstring(int errno)
        {
#if !NETSTANDARD15
            IntPtr strerror = SafeNativeMethods.netfx_errstring(errno);
#else
            IntPtr strerror;
            try {
                strerror = SafeNativeMethods.netfx_errstring(errno);
            } catch (Exception ex) {
                // Ugly hack as .NET Standard doesn't have EntryPointNotFoundException, so if we
                // get any exception, we raise this one instead. To remove if it ever becomes
                // available.
                throw new EntryPointNotFoundException(ex.Message, ex);
            }
#endif
            return Marshal.PtrToStringAnsi(strerror);
        }
    }
}
