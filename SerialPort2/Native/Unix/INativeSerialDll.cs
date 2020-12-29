// Copyright © Jason Curl 2012-2017
// Sources at https://github.com/jcurl/SerialPortStream
// Licensed under the Microsoft Public License (Ms-PL)

namespace RJCP.IO.Ports.Native.Unix
{
    using System;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "P/Invoke")]
    internal interface INativeSerialDll
    {
        string serial_version();

        SafeSerialHandle serial_init();
        void serial_terminate(SafeSerialHandle handle);

        PortDescription[] serial_getports(SafeSerialHandle handle);

        int serial_setdevicename(SafeSerialHandle handle, string deviceName);
        string serial_getdevicename(SafeSerialHandle handle);

        int serial_setbaud(SafeSerialHandle handle, int baud);
        int serial_getbaud(SafeSerialHandle handle, out int baud);
        int serial_setdatabits(SafeSerialHandle handle, int databits);
        int serial_getdatabits(SafeSerialHandle handle, out int databits);
        int serial_setparity(SafeSerialHandle handle, Parity parity);
        int serial_getparity(SafeSerialHandle handle, out Parity parity);
        int serial_setstopbits(SafeSerialHandle handle, StopBits stopbits);
        int serial_getstopbits(SafeSerialHandle handle, out StopBits stopbits);
        int serial_setdiscardnull(SafeSerialHandle handle, bool discardNull);
        int serial_getdiscardnull(SafeSerialHandle handle, out bool discardNull);
        int serial_setparityreplace(SafeSerialHandle handle, int parityReplace);
        int serial_getparityreplace(SafeSerialHandle handle, out int parityReplace);
        int serial_settxcontinueonxoff(SafeSerialHandle handle, bool txContinueOnXOff);
        int serial_gettxcontinueonxoff(SafeSerialHandle handle, out bool txContinueOnXOff);
        int serial_setxofflimit(SafeSerialHandle handle, int xoffLimit);
        int serial_getxofflimit(SafeSerialHandle handle, out int xoffLimit);
        int serial_setxonlimit(SafeSerialHandle handle, int xonLimit);
        int serial_getxonlimit(SafeSerialHandle handle, out int xonLimit);
        int serial_sethandshake(SafeSerialHandle handle, Handshake handshake);
        int serial_gethandshake(SafeSerialHandle handle, out Handshake handshake);

        int serial_open(SafeSerialHandle handle);
        int serial_close(SafeSerialHandle handle);
        int serial_isopen(SafeSerialHandle handle, out bool isOpen);

        int serial_setproperties(SafeSerialHandle handle);
        int serial_getproperties(SafeSerialHandle handle);

        int serial_getdcd(SafeSerialHandle handle, out bool dcd);
        int serial_getri(SafeSerialHandle handle, out bool ri);
        int serial_getdsr(SafeSerialHandle handle, out bool dsr);
        int serial_getcts(SafeSerialHandle handle, out bool cts);
        int serial_setdtr(SafeSerialHandle handle, bool dtr);
        int serial_getdtr(SafeSerialHandle handle, out bool dtr);
        int serial_setrts(SafeSerialHandle handle, bool rts);
        int serial_getrts(SafeSerialHandle handle, out bool rts);
        int serial_setbreak(SafeSerialHandle handle, bool breakState);
        int serial_getbreak(SafeSerialHandle handle, out bool breakState);

        string serial_error(SafeSerialHandle handle);
        int errno { get; }

        SysErrNo netfx_errno(int errno);
        string netfx_errstring(int errno);

        WaitForModemEvent serial_waitformodemevent(SafeSerialHandle handle, WaitForModemEvent mevent);
        int serial_abortwaitformodemevent(SafeSerialHandle handle);
        SerialReadWriteEvent serial_waitforevent(SafeSerialHandle handle, SerialReadWriteEvent rwevent, int timeout);
        int serial_abortwaitforevent(SafeSerialHandle handle);
        int serial_read(SafeSerialHandle handle, IntPtr data, int length);
        int serial_write(SafeSerialHandle handle, IntPtr data, int length);
        int serial_discardinbuffer(SafeSerialHandle handle);
        int serial_discardoutbuffer(SafeSerialHandle handle);
    }
}
