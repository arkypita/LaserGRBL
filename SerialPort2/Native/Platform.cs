namespace RJCP.IO.Ports.Native
{
    using System;
#if NETSTANDARD15
    using System.Runtime.InteropServices;
#endif

    internal static class Platform
    {
        public static bool IsUnix()
        {
#if NETSTANDARD15
            return
                RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
                RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
#else
            int p = (int)Environment.OSVersion.Platform;
            return (p == 4 || p == 8 || p == 128);
#endif
        }

        public static bool IsWinNT()
        {
#if NETSTANDARD15
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#else
            int p = (int)Environment.OSVersion.Platform;
            return (p == (int)PlatformID.Win32NT);
#endif
        }
    }
}
