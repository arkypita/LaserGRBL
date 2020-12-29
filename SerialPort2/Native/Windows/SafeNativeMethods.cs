// Copyright © Jason Curl 2012-2016
// Sources at https://github.com/jcurl/SerialPortStream
// Licensed under the Microsoft Public License (Ms-PL)

namespace RJCP.IO.Ports.Native.Windows
{
    using System.Security;

#if !NETSTANDARD15
    [SuppressUnmanagedCodeSecurity]
#endif
    internal static class SafeNativeMethods
    {
    }
}
