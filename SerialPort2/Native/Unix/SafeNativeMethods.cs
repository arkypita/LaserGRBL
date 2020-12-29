namespace RJCP.IO.Ports.Native.Unix
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;

#if !NETSTANDARD15
    [SuppressUnmanagedCodeSecurity]
#endif
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "P/Invoke")]
    internal static class SafeNativeMethods
    {
        [DllImport("libnserial.so.1")]
        internal static extern IntPtr serial_version();

        // First available in version 1.1.0 of libnserial
        [DllImport("libnserial.so.1")]
        internal static extern int netfx_errno(int errno);

        // First available in version 1.1.0 of libnserial
        [DllImport("libnserial.so.1")]
        internal static extern IntPtr netfx_errstring(int errno);
    }
}

