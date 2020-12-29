namespace RJCP.IO.Ports.Native.Unix
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;

#if !NETSTANDARD15
    [SuppressUnmanagedCodeSecurity]
#endif
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "P/Invoke")]
    internal static class UnsafeNativeMethods
    {
        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern SafeSerialHandle serial_init();

        [DllImport("libnserial.so.1")]
        internal static extern void serial_terminate(IntPtr handle);

        // First available in version 1.1.0 of libnserial
        [DllImport("libnserial.so.1")]
        internal static extern IntPtr serial_getports(SafeSerialHandle handle);

        [DllImport("libnserial.so.1", SetLastError = true, ThrowOnUnmappableChar = true, BestFitMapping = false)]
        internal static extern int serial_setdevicename(SafeSerialHandle handle, [MarshalAs(UnmanagedType.LPStr)] string deviceName);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern IntPtr serial_getdevicename(SafeSerialHandle handle);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_setbaud(SafeSerialHandle handle, int baud);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_getbaud(SafeSerialHandle handle, out int baud);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_setdatabits(SafeSerialHandle handle, int databits);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_getdatabits(SafeSerialHandle handle, out int databits);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_setparity(SafeSerialHandle handle, Parity parity);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_getparity(SafeSerialHandle handle, out Parity parity);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_setstopbits(SafeSerialHandle handle, StopBits stopbits);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_getstopbits(SafeSerialHandle handle, out StopBits stopbits);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_setdiscardnull(SafeSerialHandle handle, [MarshalAs(UnmanagedType.Bool)] bool discardnull);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_getdiscardnull(SafeSerialHandle handle, [MarshalAs(UnmanagedType.Bool)] out bool discardnull);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_setparityreplace(SafeSerialHandle handle, int parityReplace);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_getparityreplace(SafeSerialHandle handle, out int parityReplace);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_settxcontinueonxoff(SafeSerialHandle handle, [MarshalAs(UnmanagedType.Bool)] bool txContinueOnXOff);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_gettxcontinueonxoff(SafeSerialHandle handle, [MarshalAs(UnmanagedType.Bool)] out bool txContinueOnXOff);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_setxofflimit(SafeSerialHandle handle, int xoffLimit);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_getxofflimit(SafeSerialHandle handle, out int xoffLimit);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_setxonlimit(SafeSerialHandle handle, int xonLimit);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_getxonlimit(SafeSerialHandle handle, out int xonLimit);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_sethandshake(SafeSerialHandle handle, Handshake handshake);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_gethandshake(SafeSerialHandle handle, out Handshake handshake);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_open(SafeSerialHandle handle);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_close(SafeSerialHandle handle);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_isopen(SafeSerialHandle handle, [MarshalAs(UnmanagedType.Bool)] out bool isOpen);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_setproperties(SafeSerialHandle handle);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_getproperties(SafeSerialHandle handle);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_getdcd(SafeSerialHandle handle, [MarshalAs(UnmanagedType.Bool)] out bool dcd);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_getri(SafeSerialHandle handle, [MarshalAs(UnmanagedType.Bool)] out bool ri);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_getdsr(SafeSerialHandle handle, [MarshalAs(UnmanagedType.Bool)] out bool dsr);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_getcts(SafeSerialHandle handle, [MarshalAs(UnmanagedType.Bool)] out bool cts);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_setdtr(SafeSerialHandle handle, [MarshalAs(UnmanagedType.Bool)] bool dtr);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_getdtr(SafeSerialHandle handle, [MarshalAs(UnmanagedType.Bool)] out bool dtr);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_setrts(SafeSerialHandle handle, [MarshalAs(UnmanagedType.Bool)] bool rts);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_getrts(SafeSerialHandle handle, [MarshalAs(UnmanagedType.Bool)] out bool rts);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_setbreak(SafeSerialHandle handle, [MarshalAs(UnmanagedType.Bool)] bool breakState);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_getbreak(SafeSerialHandle handle, [MarshalAs(UnmanagedType.Bool)] out bool breakState);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern IntPtr serial_error(SafeSerialHandle handle);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern SerialReadWriteEvent serial_waitforevent(SafeSerialHandle handle, SerialReadWriteEvent rwevent, int timeout);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_abortwaitforevent(SafeSerialHandle handle);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_read(SafeSerialHandle handle, IntPtr buffer, int length);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_write(SafeSerialHandle handle, IntPtr buffer, int length);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern WaitForModemEvent serial_waitformodemevent(SafeSerialHandle handle, WaitForModemEvent mevent);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_abortwaitformodemevent(SafeSerialHandle handle);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_discardinbuffer(SafeSerialHandle handle);

        [DllImport("libnserial.so.1", SetLastError = true)]
        internal static extern int serial_discardoutbuffer(SafeSerialHandle handle);
    }
}

