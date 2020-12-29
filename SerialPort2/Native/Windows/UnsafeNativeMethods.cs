// Copyright © Jason Curl 2012-2016
// Sources at https://github.com/jcurl/SerialPortStream
// Licensed under the Microsoft Public License (Ms-PL)

namespace RJCP.IO.Ports.Native.Windows
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;
    using Microsoft.Win32.SafeHandles;

#if !NETSTANDARD15
    [SuppressUnmanagedCodeSecurity]
#endif
    internal static class UnsafeNativeMethods
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern SafeFileHandle CreateFile(
            string lpFileName,
            [MarshalAs(UnmanagedType.U4)] NativeMethods.FileAccess dwDesiredAccess,
            [MarshalAs(UnmanagedType.U4)] NativeMethods.FileShare dwShareMode,
            IntPtr lpSecurityAttributes,
            [MarshalAs(UnmanagedType.U4)] NativeMethods.CreationDisposition dwCreationDisposition,
            [MarshalAs(UnmanagedType.U4)] NativeMethods.FileAttributes dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool WriteFile(SafeFileHandle hFile, IntPtr lpBuffer,
            uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten,
            [In] ref System.Threading.NativeOverlapped lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ReadFile(SafeFileHandle hFile, [Out] IntPtr lpBuffer, uint nNumberOfBytesToRead,
            out uint lpNumberOfBytesRead, [In] ref System.Threading.NativeOverlapped lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetOverlappedResult(SafeFileHandle hFile,
           [In] ref System.Threading.NativeOverlapped lpOverlapped,
           out uint lpNumberOfBytesTransferred,
           [MarshalAs(UnmanagedType.Bool)] bool bWait);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCommProperties(SafeFileHandle hFile, ref NativeMethods.CommProp lpCommProp);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCommModemStatus(SafeFileHandle hFile,
            [MarshalAs(UnmanagedType.U4)] out NativeMethods.ModemStat lpModemStat);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCommMask(SafeFileHandle hFile,
            [MarshalAs(UnmanagedType.U4)] out NativeMethods.SerialEventMask lpEvtMask);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetCommMask(SafeFileHandle hFile,
            [MarshalAs(UnmanagedType.U4)] NativeMethods.SerialEventMask dwEvtMask);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool WaitCommEvent(SafeFileHandle hFile,
            [MarshalAs(UnmanagedType.U4)] out NativeMethods.SerialEventMask lpEvtMask,
            ref System.Threading.NativeOverlapped lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCommTimeouts(SafeFileHandle hFile,
            [In, Out] ref NativeMethods.COMMTIMEOUTS lpCommTimeouts);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetCommTimeouts(SafeFileHandle hFile,
            [In] ref NativeMethods.COMMTIMEOUTS lpCommTimeouts);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ClearCommError(
            [In] SafeFileHandle hFile,
            [MarshalAs(UnmanagedType.U4)] out NativeMethods.ComStatErrors lpErrors,
            [In, Out] ref NativeMethods.COMSTAT lpStat
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ClearCommError(
            [In] SafeFileHandle hFile,
            [MarshalAs(UnmanagedType.U4)] out NativeMethods.ComStatErrors lpErrors,
            IntPtr lpStat
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PurgeComm(SafeFileHandle hFile, [MarshalAs(UnmanagedType.U4)] NativeMethods.PurgeFlags dwFlags);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCommState(SafeFileHandle hFile, ref NativeMethods.DCB lpDCB);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetCommState(SafeFileHandle hFile, [In] ref NativeMethods.DCB lpDCB);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CancelIo(SafeFileHandle hFile);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.U4)]
        public static extern NativeMethods.FileType GetFileType(SafeFileHandle hFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetCommBreak(SafeFileHandle hFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ClearCommBreak(SafeFileHandle hFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EscapeCommFunction(SafeFileHandle hFile,
            [MarshalAs(UnmanagedType.U4)] NativeMethods.ExtendedFunctions dwFunc);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetupComm(SafeFileHandle hFile, int dwInQueue, int dwOutQueue);
    }
}
