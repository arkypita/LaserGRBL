using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Tools
{
    public class OSHelper
    {
        public static bool Is64BitProcess = (IntPtr.Size == 8);
        public static bool Is64BitOperatingSystem = Is64BitProcess || InternalCheckIsWow64();

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process([In] IntPtr hProcess, [Out] out bool wow64Process);

        private static bool InternalCheckIsWow64()
        {
            if ((Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1) || Environment.OSVersion.Version.Major >= 6)
            {
                try
                {
                    using (Process p = Process.GetCurrentProcess())
                    {
                        bool retVal;
                        if (!IsWow64Process(p.Handle, out retVal))
                            return false;
                        return retVal;
                    }
                }
                catch { return false; }
            }
            else
            {
                return false;
            }
        }

    }
}
