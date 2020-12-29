// Copyright © Jason Curl 2012-2016
// Sources at https://github.com/jcurl/SerialPortStream
// Licensed under the Microsoft Public License (Ms-PL)

namespace RJCP.IO.Ports.Native.Windows
{
    using System;
    using System.Collections.Specialized;
    using System.Runtime.InteropServices;

    internal static class NativeMethods
    {
        [Flags]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S2344:Enumeration type names should not have \"Flags\" or \"Enum\" suffixes",
            Justification = "P/Invoke")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S2346:Flags enumerations zero-value members should be named \"None\"",
            Justification = "P/Invoke")]
        public enum DcbFlags
        {
            Binary = 0x0001,
            Parity = 0x0002,
            OutxCtsFlow = 0x0004,
            OutxDsrFlow = 0x0008,
            DtrControlMask = 0x0030,
            DtrControlDisable = 0x0000,
            DtrControlEnable = 0x0010,
            DtrControlHandshake = 0x0020,
            DsrSensitivity = 0x0040,
            TxContinueOnXOff = 0x0080,
            OutX = 0x0100,
            InX = 0x0200,
            ErrorChar = 0x0400,
            Null = 0x0800,
            RtsControlMask = 0x3000,
            RtsControlDisable = 0x0000,
            RtsControlEnable = 0x1000,
            RtsControlHandshake = 0x2000,
            RtsControlToggle = 0x3000,
            AbortOnError = 0x4000
        }

        /// <summary>
        /// Defines the control setting for a serial communications device
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed",
            Justification = "P/Invoke")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase",
            Justification = "P/Invoke")]
        public struct DCB
        {
            /// <summary>
            /// Length of the structure in bytes
            /// </summary>
            public int DCBLength;

            /// <summary>
            /// Baud rate at which the communications device operates
            /// </summary>
            public uint BaudRate;

            /// <summary>
            /// Various flags that define operation
            /// </summary>
            [MarshalAs(UnmanagedType.U4)]
            public DcbFlags Flags;


            /// <summary>
            /// Not currently used
            /// </summary>
            private ushort wReserved;

            /// <summary>
            /// Transmit XON threshold
            /// </summary>
            public ushort XonLim;

            /// <summary>
            /// Transmit XOFF threshold
            /// </summary>
            public ushort XoffLim;

            /// <summary>
            /// Number of bits in the bytes transmitted and received
            /// </summary>
            public byte ByteSize;

            /// <summary>
            /// The parity scheme to be used
            /// </summary>
            public byte Parity;

            /// <summary>
            /// Number of stop bits to use
            /// </summary>
            public byte StopBits;

            /// <summary>
            /// Value of the XON character for both transmission and reception
            /// </summary>
            public byte XonChar;

            /// <summary>
            /// Value of the XOFF character for both transmission and reception
            /// </summary>
            public byte XoffChar;

            /// <summary>
            /// Value of the character to use when replacing bytes with a parity error
            /// </summary>
            public byte ErrorChar;

            /// <summary>
            /// Character to use to signal end of data
            /// </summary>
            public byte EofChar;

            /// <summary>
            /// Character to use to signal an event
            /// </summary>
            public byte EvtChar;

            /// <summary>
            /// Reserved; do not use
            /// </summary>
            private ushort wReserved1;
        }

        [Flags]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S4070:Non-flags enums should not be marked with \"FlagsAttribute\"",
            Justification = "P/Invoke")]
        public enum FileAccess
        {
            #region Standard Access Rights
            /// <summary>
            /// The caller can delete the object
            /// </summary>
            DELETE = 0x10000,

            /// <summary>
            /// Read access to the owner, group and discretionary access control list (DACL) of the security descriptor,
            /// not including the information in the system access control list (SACL).
            /// </summary>
            READ_CONTROL = 0x20000,

            /// <summary>
            /// Write access to the DACL, to change information for the object
            /// </summary>
            WRITE_DAC = 0x40000,

            /// <summary>
            /// The right to change the owner in the object's security descriptor
            /// </summary>
            WRITE_OWNER = 0x80000,

            /// <summary>
            /// The caller can perform a wait operation on the object
            /// </summary>
            /// <remarks>
            /// This enables a thread to wait until the object is in the signalled state. Some object
            /// types do not support this access right.
            /// </remarks>
            SYNCHRONIZE = 0x100000,

            /// <summary>
            /// Combines DELETE, READ_CONTROL, WRITE_DAC, and WRITE_OWNER access
            /// </summary>
            STANDARD_RIGHTS_REQUIRED = 0xF0000,

            /// <summary>
            /// Includes READ_CONTROL, which is the right to read information in the file or directory
            /// object's security descriptor. This does not include the information in the SACL
            /// </summary>
            STANDARD_RIGHTS_READ = READ_CONTROL,

            /// <summary>
            /// Same as STANDARD_RIGHTS_READ, READ_CONTROL
            /// </summary>
            STANDARD_RIGHTS_WRITE = READ_CONTROL,

            /// <summary>
            /// Same as READ_CONTROL
            /// </summary>
            STANDARD_RIGHTS_EXECUTE = READ_CONTROL,

            /// <summary>
            /// Combines DELETE, READ_CONTROL, WRITE_DAC, WRITE_OWNER and SYNCHRONIZE access
            /// </summary>
            STANDARD_RIGHTS_ALL = 0x1F0000,
            #endregion

            #region Other Rights
            /// <summary>
            /// Access System Security.
            /// </summary>
            /// <remarks>
            /// It is used to indicate access to a system access control list (SACL). This type of access
            /// requires the calling process to have the SE_SECURITY_NAME (Manage auditing and security log)
            /// privilege. If this flag is set in the access mask of an audit access ACE (successful or
            /// unsuccessful access), the SACL access will be audited.
            /// </remarks>
            ACCESS_SYSTEM_SECURITY = 0x01000000,

            /// <summary>
            /// Maximum Allowed
            /// </summary>
            MAXIMUM_ALLOWED = 0x02000000,

            /// <summary>
            /// Read, write and execute access
            /// </summary>
            GENERIC_ALL = 0x10000000,

            /// <summary>
            /// Execute access
            /// </summary>
            GENERIC_EXECUTE = 0x20000000,

            /// <summary>
            /// Write access
            /// </summary>
            GENERIC_WRITE = 0x40000000,

            /// <summary>
            /// Read access
            /// </summary>
            GENERIC_READ = unchecked((int)0x80000000),
            #endregion

            #region Specific Rights File, Pipe and Directory
            /// <summary>
            /// The right to read the corresponding file data
            /// </summary>
            FILE_READ_DATA = 0x0001,

            /// <summary>
            /// The right to list the contents of the directory
            /// </summary>
            FILE_LIST_DIRECTORY = 0x0001,

            /// <summary>
            /// Read data from a pipe. Always has SYNCHRONIZE access.
            /// </summary>
            PIPE_ACCESS_INBOUND = 0x0001,

            /// <summary>
            /// the right to write data to the file
            /// </summary>
            FILE_WRITE_DATA = 0x0002,

            /// <summary>
            /// The right to create a file in the directory
            /// </summary>
            FILE_ADD_FILE = 0x0002,

            /// <summary>
            /// Write data to a pipe. Always has SYNCHRONIZE access
            /// </summary>
            PIPE_ACCESS_OUTBOUND = 0x0002,

            /// <summary>
            /// Allow read/write access to the pipe. Always has SYNCHRONIZE access
            /// </summary>
            PIPE_ACCESS_DUPLEX = 0x0003,

            /// <summary>
            /// For a file object, the right to append data to the file
            /// </summary>
            /// <remarks>
            /// For local files, write operations will not overwrite existing data
            /// if this flag is specified without FILE_WRITE_DATA.
            /// </remarks>
            FILE_APPEND_DATA = 0x0004,

            /// <summary>
            /// The right to create a subdirectory
            /// </summary>
            /// <remarks>
            /// For local directories, write operations will not overwrite existing
            /// data if this flag is specified without FILE_ADD_SUBDIRECTORY.
            /// </remarks>
            FILE_ADD_SUBDIRECTORY = 0x0004,

            /// <summary>
            /// The right to create a pipe
            /// </summary>
            FILE_CREATE_PIPE_INSTANCE = 0x0004,

            /// <summary>
            /// The right to read extended file attributes
            /// </summary>
            FILE_READ_EA = 0x0008,

            /// <summary>
            /// The right to write extended file attributes
            /// </summary>
            FILE_WRITE_EA = 0x0010,

            /// <summary>
            /// For a native code file, the right to execute the file
            /// </summary>
            /// <remarks>
            /// This access right given to scripts may cause the script to be executable,
            /// depending on the script interpreter.
            /// </remarks>
            FILE_EXECUTE = 0x0020,

            /// <summary>
            /// The right to traverse the directory
            /// </summary>
            /// <remarks>
            /// By default, users are assigned the BYPASS_TRAVERSE_CHECKING privilege, which
            /// ignores the FILE_TRAVERSE access right. See the remarks in "File Security and Access Rights"
            /// for more information
            /// </remarks>
            FILE_TRAVERSE = 0x0020,

            /// <summary>
            /// The right to delete a directory and all the files it contains, including read-only files
            /// </summary>
            FILE_DELETE_CHILD = 0x0040,

            /// <summary>
            /// The right to read file attributes, for file, directory and pipe
            /// </summary>
            FILE_READ_ATTRIBUTES = 0x0080,

            /// <summary>
            /// FThe right to write file attributes
            /// </summary>
            FILE_WRITE_ATTRIBUTES = 0x0100,

            /// <summary>
            /// All possible access rights for a file
            /// </summary>
            FILE_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0x1FF,

            /// <summary>
            /// Provide generic read access to a file
            /// </summary>
            FILE_GENERIC_READ = STANDARD_RIGHTS_READ | FILE_READ_DATA | FILE_READ_ATTRIBUTES | FILE_READ_EA | SYNCHRONIZE,

            /// <summary>
            /// Provide generic write access to a file
            /// </summary>
            FILE_GENERIC_WRITE = STANDARD_RIGHTS_WRITE | FILE_WRITE_DATA | FILE_WRITE_ATTRIBUTES | FILE_WRITE_EA | FILE_APPEND_DATA | SYNCHRONIZE,

            /// <summary>
            /// Provide generic execute access to a file
            /// </summary>
            FILE_GENERIC_EXECUTE = STANDARD_RIGHTS_EXECUTE | FILE_READ_ATTRIBUTES | FILE_EXECUTE | SYNCHRONIZE,
            #endregion

            SPECIFIC_RIGHTS_ALL = 0xFFFF,
        }

        [Flags]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S2346:Flags enumerations zero-value members should be named \"None\"",
            Justification = "P/Invoke")]
        public enum FileShare
        {
            /// <summary>
            /// Prevents other processes from opening a file or device if they request to delete,
            /// read or write access.
            /// </summary>
            FILE_SHARE_NONE = 0x00000000,

            /// <summary>
            /// Enables subsequent open operations on an object to request read access.
            /// </summary>
            /// <remarks>
            /// Enables subsequent open operations on an object to request read access.
            /// Otherwise, other processes cannot open the object if they request read access.
            /// If this flag is not specified, but the object has been opened for read access, the function fails.
            /// </remarks>
            FILE_SHARE_READ = 0x00000001,

            /// <summary>
            /// Enables subsequent open operations on an object to request write access.
            /// </summary>
            /// <remarks>
            /// Enables subsequent open operations on an object to request write access.
            /// Otherwise, other processes cannot open the object if they request write access.
            /// If this flag is not specified, but the object has been opened for write access, the function fails.
            /// </remarks>
            FILE_SHARE_WRITE = 0x00000002,

            /// <summary>
            /// Enables subsequent open operations on an object to request delete access.
            /// </summary>
            /// <remarks>
            /// Enables subsequent open operations on an object to request delete access.
            /// Otherwise, other processes cannot open the object if they request delete access.
            /// If this flag is not specified, but the object has been opened for delete access, the function fails.
            /// </remarks>
            FILE_SHARE_DELETE = 0x00000004
        }

        public enum CreationDisposition
        {
            /// <summary>
            /// Creates a new file. The function fails if a specified file exists.
            /// </summary>
            CREATE_NEW = 1,

            /// <summary>
            /// Creates a new file, always.
            /// </summary>
            /// <remarks>
            /// If a file exists, the function overwrites the file, clears the existing attributes, combines the specified file attributes,
            /// and flags with FILE_ATTRIBUTE_ARCHIVE, but does not set the security descriptor that the SECURITY_ATTRIBUTES structure specifies.
            /// </remarks>
            CREATE_ALWAYS = 2,

            /// <summary>
            /// Opens a file. The function fails if the file does not exist.
            /// </summary>
            OPEN_EXISTING = 3,

            /// <summary>
            /// Opens a file, always.
            /// </summary>
            /// <remarks>
            /// If a file does not exist, the function creates a file as if dwCreationDisposition is CREATE_NEW.
            /// </remarks>
            OPEN_ALWAYS = 4,

            /// <summary>
            /// Opens a file and truncates it so that its size is 0 (zero) bytes. The function fails if the file does not exist.
            /// </summary>
            /// <remarks>
            /// The calling process must open the file with the GENERIC_WRITE access right.
            /// </remarks>
            TRUNCATE_EXISTING = 5
        }

        [Flags]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S4070:Non-flags enums should not be marked with \"FlagsAttribute\"",
            Justification = "P/Invoke")]
        public enum FileAttributes
        {
            /// <summary>
            /// A file that is read-only
            /// </summary>
            /// <remarks>
            /// Applications can read the file, but cannot write to it or delete
            /// it. This attribute is not honoured on directories. For more information,
            /// see "You cannot view or change the Read-only or the System attributes
            /// of folders in Windows Server 2003, in Windows XP, in Windows Vista or
            /// in Windows 7".
            /// </remarks>
            FILE_ATTRIBUTE_READONLY = 0x00000001,

            /// <summary>
            /// The file or directory is hidden.
            /// </summary>
            /// <remarks>
            /// It is not included in an ordinary directory listing.
            /// </remarks>
            FILE_ATTRIBUTE_HIDDEN = 0x00000002,

            /// <summary>
            /// A file or directory that the operating system uses a part of, or uses exclusively.
            /// </summary>
            FILE_ATTRIBUTE_SYSTEM = 0x00000004,

            /// <summary>
            /// The handle that identifies a directory
            /// </summary>
            FILE_ATTRIBUTE_DIRECTORY = 0x00000010,

            /// <summary>
            /// A file or directory that is an archive file or directory.
            /// </summary>
            /// <remarks>
            /// Applications typically use this attribute to mark files for backup or removal.
            /// </remarks>
            FILE_ATTRIBUTE_ARCHIVE = 0x00000020,

            /// <summary>
            /// This value is reserved for system use
            /// </summary>
            FILE_ATTRIBUTE_DEVICE = 0x00000040,

            /// <summary>
            /// A file that does not have other attributes set
            /// </summary>
            /// <remarks>
            /// This attribute is valid only when used alone
            /// </remarks>
            FILE_ATTRIBUTE_NORMAL = 0x00000080,

            /// <summary>
            /// A file that is being used for temporary storage
            /// </summary>
            /// <remarks>
            /// File systems avoid writing data back to mass storage if sufficient
            /// cache memory is available, because typically, an application deletes
            /// a temporary file after the handle is closed. In that scenario, the
            /// system can entirely avoid writing the data. Otherwise, the data is
            /// written after the handle is closed.
            /// </remarks>
            FILE_ATTRIBUTE_TEMPORARY = 0x00000100,

            /// <summary>
            /// A file that is a sparse file
            /// </summary>
            FILE_ATTRIBUTE_SPARSE_FILE = 0x00000200,

            /// <summary>
            /// A file or directory that has an associated reparse point, or a file that is a symbolic link
            /// </summary>
            FILE_ATTRIBUTE_REPARSE_POINT = 0x00000400,

            /// <summary>
            /// A file or directory that is compressed.
            /// </summary>
            /// <remarks>
            /// For a file, all of the data in the file is compressed. For a directory,
            /// compression is the default for newly created files and subdirectories.
            /// </remarks>
            FILE_ATTRIBUTE_COMPRESSED = 0x00000800,

            /// <summary>
            /// The data of a file is not available immediately
            /// </summary>
            /// <remarks>
            /// This attribute indicates that the file data is physically moved to offline
            /// storage. This attribute is used by Remote Storage, which is the hierarchical
            /// storage management software. Applications should not arbitrarily change this attribute.
            /// </remarks>
            FILE_ATTRIBUTE_OFFLINE = 0x00001000,

            /// <summary>
            /// The file or directory is not to be indexed by the content indexing service
            /// </summary>
            FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x00002000,

            /// <summary>
            /// A file or directory that is encrypted
            /// </summary>
            /// <remarks>
            /// For a file, all data streams in the file are encrypted. For a directory,
            /// encryption is the default for newly created files and subdirectories.
            /// </remarks>
            FILE_ATTRIBUTE_ENCRYPTED = 0x00004000,

            /// <summary>
            /// This value is reserved for system use
            /// </summary>
            FILE_ATTRIBUTE_VIRTUAL = 0x00010000,

            /// <summary>
            /// If you attempt to create multiple instances of a pipe with this flag,
            /// creation of the first instance succeeds, but creation of the next
            /// instance fails with ERROR_ACCESS_DENIED.
            /// </summary>
            /// <remarks>
            /// Windows 2000: This flag is not supported until Windows 2000 SP2 and Windows XP
            /// </remarks>
            FILE_FLAG_FIRST_PIPE_INSTANCE = 0x00080000,

            /// <summary>
            /// The file data is requested, but it should continue to be located in
            /// remote storage. It should not be transported back to local storage.
            /// This flag is for use by remote storage systems.
            /// </summary>
            FILE_FLAG_OPEN_NO_RECALL = 0x00100000,

            /// <summary>
            /// Normal reparse point processing will not occur.
            /// </summary>
            /// <remarks>
            /// CreateFile will attempt to open the reparse point. When a file is opened,
            /// a file handle is returned, whether or not the filter
            /// that controls the reparse point is operational. This flag cannot be used
            /// with the CREATE_ALWAYS flag. If the file is not a reparse point, then this
            /// flag is ignored.
            /// </remarks>
            FILE_FLAG_OPEN_REPARSE_POINT = 0x00200000,

            /// <summary>
            /// Access will occur according to POSIX rules
            /// </summary>
            /// <remarks>
            /// This includes allowing multiple files with names, differing only in case,
            /// for file systems that support that naming. Use care when using this option,
            /// because files created with this flag may not be accessible by applications
            /// that are written for MS-DOS or 16-bit Windows.
            /// </remarks>
            FILE_FLAG_POSIX_SEMANTICS = 0x01000000,

            /// <summary>
            /// The file is being opened or created for a backup or restore operation
            /// </summary>
            /// <remarks>
            /// The system ensures that the calling process overrides file security
            /// checks when the process has SE_BACKUP_NAME and SE_RESTORE_NAME privileges.
            /// For more information, see "Changing Privileges in a Token".
            /// </remarks>
            FILE_FLAG_BACKUP_SEMANTICS = 0x02000000,

            /// <summary>
            /// The file is to be deleted immediately after all of its handles are closed,
            /// which includes the specified handle and any other open or duplicated handles.
            /// </summary>
            /// <remarks>
            /// If there are existing open handles to a file, the call fails unless they were
            /// all opened with the FILE_SHARE_DELETE share mode. Subsequent open requests for
            /// the file fail, unless the FILE_SHARE_DELETE share mode is specified.
            /// </remarks>
            FILE_FLAG_DELETE_ON_CLOSE = 0x04000000,

            /// <summary>
            /// Access is intended to be sequential from beginning to end
            /// </summary>
            /// <remarks>
            /// The system can use this as a hint to optimize file caching. This flag
            /// should not be used if read-behind (that is, reverse scans) will be used.
            /// This flag has no effect if the file system does not support cached I/O
            /// and FILE_FLAG_NO_BUFFERING. For more information, see the Caching Behaviour
            /// section of CreateFile().
            /// </remarks>
            FILE_FLAG_SEQUENTIAL_SCAN = 0x08000000,

            /// <summary>
            /// Access is intended to be random
            /// </summary>
            /// <remarks>
            /// The system can use this as a hint to optimize file caching. This flag
            /// has no effect if the file system does not support cached I/O and
            /// FILE_FLAG_NO_BUFFERING. For more information, see the Caching Behaviour
            /// section of CreateFile()
            /// </remarks>
            FILE_FLAG_RANDOM_ACCESS = 0x10000000,

            /// <summary>
            /// The file or device is being opened with no system caching for data reads
            /// and writes. This flag does not affect hard disk caching or memory mapped
            /// files.
            /// </summary>
            /// <remarks>
            /// There are strict requirements for successfully working with files opened
            /// with CreateFile using the FILE_FLAG_NO_BUFFERING flag, for details see
            /// "File Buffering".
            /// </remarks>
            FILE_FLAG_NO_BUFFERING = 0x20000000,

            /// <summary>
            /// The file or device is being opened or created for asynchronous I/O.
            /// </summary>
            /// <remarks>
            /// When subsequent I/O operations are completed on this handle, the event
            /// specified in the OVERLAPPED structure will be set to the signalled state.
            /// If this flag is specified, the file can be used for simultaneous read
            /// and write operations. If this flag is not specified, then I/O operations
            /// are serialized, even if the calls to the read and write functions specify
            /// an OVERLAPPED structure. For information about considerations when using
            /// a file handle created with this flag, see the Synchronous and Asynchronous
            /// I/O Handles section of CreateFile().
            /// </remarks>
            FILE_FLAG_OVERLAPPED = 0x40000000,

            /// <summary>
            /// Write operations will not go through any intermediate cache, they will go directly to disk.
            /// </summary>
            /// <remarks>
            /// For additional information, see the Caching Behaviour section of CreateFile().
            /// </remarks>
            FILE_FLAG_WRITE_THROUGH = unchecked((int)0x80000000),
        }

        /// <summary>
        /// The file type of the specified file
        /// </summary>
        public enum FileType
        {
            /// <summary>
            /// Either the type of the specified file is unknown, or the function failed
            /// </summary>
            FILE_TYPE_UNKNOWN = 0x0000,

            /// <summary>
            /// The specified file is a disk file.
            /// </summary>
            FILE_TYPE_DISK = 0x0001,

            /// <summary>
            /// The specified file is a character file, typically an LPT device or a console
            /// </summary>
            FILE_TYPE_CHAR = 0x0002,

            /// <summary>
            /// The specified file is a socket, a named pipe, or an anonymous pipe.
            /// </summary>
            FILE_TYPE_PIPE = 0x0003,

            /// <summary>
            /// Unused
            /// </summary>
            FILE_TYPE_REMOTE = 0x8000
        }

        /// <summary>
        /// Specifies possible baud rates in the GetCommProperties method
        /// </summary>
        [Flags]
        public enum MaxBaud
        {
            /// <summary>
            /// 75 bps
            /// </summary>
            BAUD_075 = 0x00000001,

            /// <summary>
            /// 110 bps
            /// </summary>
            BAUD_110 = 0x00000002,

            /// <summary>
            /// 134.5 bps
            /// </summary>
            BAUD_134_5 = 0x00000004,

            /// <summary>
            /// 150 bps
            /// </summary>
            BAUD_150 = 0x00000008,

            /// <summary>
            /// 300 bps
            /// </summary>
            BAUD_300 = 0x00000010,

            /// <summary>
            /// 600 bps
            /// </summary>
            BAUD_600 = 0x00000020,

            /// <summary>
            /// 1200 bps
            /// </summary>
            BAUD_1200 = 0x00000040,

            /// <summary>
            /// 1800 bps
            /// </summary>
            BAUD_1800 = 0x00000080,

            /// <summary>
            /// 2400 bps
            /// </summary>
            BAUD_2400 = 0x00000100,

            /// <summary>
            /// 4800 bps
            /// </summary>
            BAUD_4800 = 0x00000200,

            /// <summary>
            /// 7200 bps
            /// </summary>
            BAUD_7200 = 0x00000400,

            /// <summary>
            /// 9600 bps
            /// </summary>
            BAUD_9600 = 0x00000800,

            /// <summary>
            /// 14400 bps
            /// </summary>
            BAUD_14400 = 0x00001000,

            /// <summary>
            /// 19200 bps
            /// </summary>
            BAUD_19200 = 0x00002000,

            /// <summary>
            /// 38400 bps
            /// </summary>
            BAUD_38400 = 0x00004000,

            /// <summary>
            /// 56K bps
            /// </summary>
            BAUD_56K = 0x00008000,

            /// <summary>
            /// 57600 bps
            /// </summary>
            BAUD_57600 = 0x00040000,

            /// <summary>
            /// 115200 bps
            /// </summary>
            BAUD_115200 = 0x00020000,

            /// <summary>
            /// 128K bps
            /// </summary>
            BAUD_128K = 0x00010000,

            /// <summary>
            /// Programmable baud rate
            /// </summary>
            BAUD_USER = 0x10000000
        }

        /// <summary>
        /// Communications Provider Type
        /// </summary>
        public enum ProvSubType
        {
            /// <summary>
            /// Unspecified
            /// </summary>
            PST_UNSPECIFIED = 0x00000000,

            /// <summary>
            /// RS-232 serial port
            /// </summary>
            PST_RS232 = 0x00000001,

            /// <summary>
            /// Parallel port
            /// </summary>
            PST_PARALLELPORT = 0x00000002,

            /// <summary>
            /// RS-422 port
            /// </summary>
            PST_RS422 = 0x00000003,

            /// <summary>
            /// RS-423 port
            /// </summary>
            PST_RS423 = 0x00000004,

            /// <summary>
            /// RS-449 port
            /// </summary>
            PST_RS449 = 0x00000005,

            /// <summary>
            /// Modem Device
            /// </summary>
            PST_MODEM = 0x00000006,

            /// <summary>
            /// FAX Device
            /// </summary>
            PST_FAX = 0x00000021,

            /// <summary>
            /// Scanner device
            /// </summary>
            PST_SCANNER = 0x00000022,

            /// <summary>
            /// Unspecified network bridge
            /// </summary>
            PST_NETWORK_BRIDGE = 0x00000100,

            /// <summary>
            /// LAT protocol
            /// </summary>
            PST_LAT = 0x00000101,

            /// <summary>
            /// TCP/IP Telnet Protocol
            /// </summary>
            PST_TCPIP_TELNET = 0x00000102,

            /// <summary>
            /// X.25 standards
            /// </summary>
            PST_X25 = 0x00000103
        }

        /// <summary>
        /// Bit mask of capabilities in the GetCommProperties method
        /// </summary>
        [Flags]
        public enum ProvCapabilities
        {
            /// <summary>
            /// DTR (data-terminal-ready)/DSR (data-set-ready) supported
            /// </summary>
            PCF_DTRDSR = 0x0001,

            /// <summary>
            /// RTS (request-to-send)/CTS (clear-to-send) supported
            /// </summary>
            PCF_RTSCTS = 0x0002,

            /// <summary>
            /// RLSD (receive-line-signal-detect) supported
            /// </summary>
            PCF_RLSD = 0x0004,

            /// <summary>
            /// Parity checking supported
            /// </summary>
            PCF_PARITY_CHECK = 0x0008,

            /// <summary>
            /// XON/XOFF flow control supported
            /// </summary>
            PCF_XONXOFF = 0x0010,

            /// <summary>
            /// Settable XON/XOFF supported
            /// </summary>
            PCF_SETXCHAR = 0x0020,

            /// <summary>
            /// The total (elapsed) time-outs supported
            /// </summary>
            PCF_TOTALTIMEOUTS = 0x0040,

            /// <summary>
            /// Interval time-outs supported
            /// </summary>
            PCF_INTTIMEOUTS = 0x0080,

            /// <summary>
            /// Special character support provided
            /// </summary>
            PCF_SPECIALCHARS = 0x0100,

            /// <summary>
            /// Special 16-bit mode supported
            /// </summary>
            PCF_16BITMODE = 0x0200,
        }

        /// <summary>
        /// Communication parameters that can be changed
        /// </summary>
        [Flags]
        public enum SettableParams
        {
            /// <summary>
            /// Parity
            /// </summary>
            SP_PARITY = 0x0001,

            /// <summary>
            /// Baud rate
            /// </summary>
            SP_BAUD = 0x0002,

            /// <summary>
            /// Data bits
            /// </summary>
            SP_DATABITS = 0x0004,

            /// <summary>
            /// Stop bits
            /// </summary>
            SP_STOPBITS = 0x0008,

            /// <summary>
            /// Handshaking (flow control)
            /// </summary>
            SP_HANDSHAKING = 0x0010,

            /// <summary>
            /// Parity Checking
            /// </summary>
            SP_PARITY_CHECK = 0x0020,

            /// <summary>
            /// RLSD (receive-line-signal-detect)
            /// </summary>
            SP_RLSD = 0x0040,
        }

        /// <summary>
        /// Number of data bits that can be set
        /// </summary>
        [Flags]
        public enum SettableData
        {
            /// <summary>
            /// 5 data bits
            /// </summary>
            DATABITS_5 = 0x0001,

            /// <summary>
            /// 6 data bits
            /// </summary>
            DATABITS_6 = 0x0002,

            /// <summary>
            /// 7 data bits
            /// </summary>
            DATABITS_7 = 0x0004,

            /// <summary>
            /// 8 data bits
            /// </summary>
            DATABITS_8 = 0x0008,

            /// <summary>
            /// 16 data bits
            /// </summary>
            DATABITS_16 = 0x0010,

            /// <summary>
            /// Special wide path through serial hardware lines
            /// </summary>
            DATABITS_16X = 0x0020
        }

        /// <summary>
        /// The stop bit and parity settings
        /// </summary>
        [Flags]
        public enum SettableStopParity
        {
            /// <summary>
            /// 1 stop bit
            /// </summary>
            STOPBITS_10 = 0x00010000,

            /// <summary>
            /// 1.5 stop bits
            /// </summary>
            STOPBITS_15 = 0x00020000,

            /// <summary>
            /// 2 stop bits
            /// </summary>
            STOPBITS_20 = 0x00040000,

            /// <summary>
            /// No parity
            /// </summary>
            PARITY_NONE = 0x01000000,

            /// <summary>
            /// Odd parity
            /// </summary>
            PARITY_ODD = 0x02000000,

            /// <summary>
            /// Even parity
            /// </summary>
            PARITY_EVEN = 0x04000000,

            /// <summary>
            /// Mark parity
            /// </summary>
            PARITY_MARK = 0x08000000,

            /// <summary>
            /// Space parity
            /// </summary>
            PARITY_SPACE = 0x10000000
        }

        /// <summary>
        /// Set wProvSpec1 to COMMPROP_INITIALIZED to indicate that wPacketLength
        /// is valid before a call to GetCommProperties().
        /// </summary>
        public const uint COMMPROP_INITIALIZED = 0xE73CF52E;

        /// <summary>
        /// Contains information about a communications driver
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed",
            Justification = "P/Invoke")]
        public struct CommProp
        {
            /// <summary>
            /// The size of the entire data packet, regardless of the amount of data requested, in bytes
            /// </summary>
            public ushort wPacketLength;

            /// <summary>
            /// The version of the structure
            /// </summary>
            public ushort wPacketVersion;

            /// <summary>
            /// A bit mask indicating which services are implemented by this provider. The SP_SERIALCOMM
            /// value is always specified for communications providers, including modem providers
            /// </summary>
            public uint dwServiceMask;

            /// <summary>
            /// Reserved; do not use
            /// </summary>
            private uint dwReserved1;

            /// <summary>
            /// The maximum size of the driver's internal output buffer, in bytes. A value of zero indicates
            /// that no maximum value is imposed by the serial provider.
            /// </summary>
            public uint dwMaxTxQueue;

            /// <summary>
            /// The maximum size of the driver's internal input buffer, in bytes. A value of zero indicates
            /// that no maximum value is imposed by the serial provider
            /// </summary>
            public uint dwMaxRxQueue;

            /// <summary>
            /// The maximum allowable baud rate, in bits per second (bps)
            /// </summary>
            public BitVector32 dwMaxBaud;                    // MaxBaud

            /// <summary>
            /// The communications-provider type
            /// </summary>
            [MarshalAs(UnmanagedType.U4)]
            public ProvSubType dwProvSubType;

            /// <summary>
            /// A bit mask indicating the capabilities offered by the provider
            /// </summary>
            public BitVector32 dwProvCapabilities;           // ProvCapabilities

            /// <summary>
            /// The communications parameter that can be changed
            /// </summary>
            public BitVector32 dwSettableParams;             // SettableParams

            /// <summary>
            /// The baud rates that can be used
            /// </summary>
            public BitVector32 dwSettableBaud;               // MaxBaud

            /// <summary>
            /// The number of data bits that can be set, stop and parity bits in the second half
            /// </summary>
            public BitVector32 dwSettableDataStopParity;     // SettableData, SettableStopParity

            /// <summary>
            /// The size of the driver's internal output buffer, in bytes.
            /// A value of zero indicates that the value is unavailable
            /// </summary>
            public uint dwCurrentTxQueue;

            /// <summary>
            /// The size of the driver's internal input buffer, in bytes.
            /// A value of zero indicates that the value is unavailable
            /// </summary>
            public uint dwCurrentRxQueue;

            /// <summary>
            /// Any provider-specific data
            /// </summary>
            /// <remarks>
            /// Applications should ignore this member unless they have
            /// detailed information about the format of the data required
            /// by the provider. Set this member to COMMPROP_INITIALIZED
            /// before calling the GetCommProperties function to indicate
            /// that the wPacketLength member is already valid
            /// </remarks>
            public uint dwProvSpec1;

            /// <summary>
            /// Any provider-specific data
            /// </summary>
            /// <remarks>
            /// Applications should ignore this member unless they have
            /// detailed information about the format of the data required
            /// by the provider.
            /// </remarks>
            public uint dwProvSpec2;

            /// <summary>
            /// Any provider-specific data
            /// </summary>
            /// <remarks>
            /// Applications should ignore this member unless they
            /// have detailed information about the format of the data required
            /// by the provider.
            /// </remarks>
            public char wcProvChar;
        }

        /// <summary>
        /// Current state of the modem control-register values
        /// </summary>
        [Flags]
        public enum ModemStat
        {
            /// <summary>
            /// The CTS (clear-to-send) signal is on
            /// </summary>
            MS_CTS_ON = 0x0010,

            /// <summary>
            /// The DSR (data-set-ready) signal is on
            /// </summary>
            MS_DSR_ON = 0x0020,

            /// <summary>
            /// The ring indicator signal is on
            /// </summary>
            MS_RING_ON = 0x0040,

            /// <summary>
            /// The RLSD (receive-line-signal-detect) signal is on
            /// </summary>
            MS_RLSD_ON = 0x0080
        }

        /// <summary>
        /// The extended function to be performed
        /// </summary>
        public enum ExtendedFunctions
        {
            /// <summary>
            /// Causes transmission to act as if an XOFF character has been received
            /// </summary>
            SETXOFF = 1,

            /// <summary>
            /// Causes transmission to act as if an XON character has been received.
            /// </summary>
            SETXON = 2,

            /// <summary>
            /// Sends the RTS (request-to-send) signal
            /// </summary>
            SETRTS = 3,

            /// <summary>
            /// Clears the RTS (request-to-send) signal
            /// </summary>
            CLRRTS = 4,

            /// <summary>
            /// Sends the DTR (data-terminal-ready) signal
            /// </summary>
            SETDTR = 5,

            /// <summary>
            /// Clears the DTR (data-terminal-ready) signal
            /// </summary>
            CLRDTR = 6,

            /// <summary>
            /// Suspends character transmission and places the transmission line in a break
            /// state until the ClearCommBreak function is called (or EscapeCommFunction is
            /// called with the CLRBREAK extended function code). The SETBREAK extended
            /// function code is identical to the SetCommBreak function. Note that this
            /// extended function does not flush data that has not been transmitted.
            /// </summary>
            SETBREAK = 8,

            /// <summary>
            /// Restores character transmission and places the transmission line in a non-break
            /// state. The CLRBREAK extended function code is identical to the ClearCommBreak function
            /// </summary>
            CLRBREAK = 9
        }

        [Flags]
        public enum SerialEventMask
        {
            /// <summary>
            /// A character was received and placed in the input buffer
            /// </summary>
            EV_RXCHAR = 0x0001,

            /// <summary>
            /// The event character was received and placed in the input buffer.
            /// The event character is specified in the device's DCB structure,
            /// which is applied to a serial port by using the SetCommState function.
            /// </summary>
            EV_RXFLAG = 0x0002,

            /// <summary>
            /// The last character in the output buffer was sent
            /// </summary>
            EV_TXEMPTY = 0x0004,

            /// <summary>
            /// The CTS (clear-to-send) signal changed state
            /// </summary>
            EV_CTS = 0x0008,

            /// <summary>
            /// The DSR (data-set-ready) signal changed state
            /// </summary>
            EV_DSR = 0x0010,

            /// <summary>
            /// The RLSD (receive-line-signal-detect) signal changed state
            /// </summary>
            EV_RLSD = 0x0020,

            /// <summary>
            /// A break was detected on input
            /// </summary>
            EV_BREAK = 0x0040,

            /// <summary>
            /// A line-status error occurred. Line-status errors are CE_FRAME, CE_OVERRUN, and CE_RXPARITY
            /// </summary>
            EV_ERR = 0x0080,

            /// <summary>
            /// A ring indicator was detected
            /// </summary>
            EV_RING = 0x0100,

            /// <summary>
            /// A printer error occurred
            /// </summary>
            EV_PERR = 0x0200,

            /// <summary>
            /// The receive buffer is 80 percent full
            /// </summary>
            EV_RX80FULL = 0x0400,

            /// <summary>
            /// An event of the first provider-specific type occurred
            /// </summary>
            EV_EVENT1 = 0x0800,

            /// <summary>
            /// An event of the second provider-specific type occurred
            /// </summary>
            EV_EVENT2 = 0x1000
        }

        [StructLayout(LayoutKind.Sequential)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase",
            Justification = "P/Invoke")]
        public struct COMMTIMEOUTS
        {
            public int ReadIntervalTimeout;
            public int ReadTotalTimeoutMultiplier;
            public int ReadTotalTimeoutConstant;
            public int WriteTotalTimeoutMultiplier;
            public int WriteTotalTimeoutConstant;
        }

        [Flags]
        public enum ComStatErrors
        {
            CE_RXOVER = 0x0001,
            CE_OVERRUN = 0x0002,
            CE_RXPARITY = 0x0004,
            CE_FRAME = 0x0008,
            CE_BREAK = 0x0010,
            CE_TXFULL = 0x0100,
            CE_PTO = 0x0200,
            CE_IOE = 0x0400,
            CE_DNS = 0x0800,
            CE_OOP = 0x1000,
            CE_MODE = 0x8000
        }

        [Flags]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S2344:Enumeration type names should not have \"Flags\" or \"Enum\" suffixes",
            Justification = "P/Invoke")]
        public enum ComStatFlags
        {
            CtsHold = 0x01,
            DsrHold = 0x02,
            RlsdHold = 0x04,
            XoffHold = 0x08,
            XoffSent = 0x10,
            Eof = 0x20,
            Txim = 0x40
        }

        [StructLayout(LayoutKind.Sequential)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase",
            Justification = "P/Invoke")]
        public struct COMSTAT
        {
            [MarshalAs(UnmanagedType.U4)]
            public ComStatFlags Flags;
            public UInt32 cbInQue;
            public UInt32 cbOutQue;
        }

        [Flags]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S2344:Enumeration type names should not have \"Flags\" or \"Enum\" suffixes",
            Justification = "P/Invoke")]
        public enum PurgeFlags
        {
            PURGE_TXABORT = 0x0001,
            PURGE_RXABORT = 0x0002,
            PURGE_TXCLEAR = 0x0004,
            PURGE_RXCLEAR = 0x0008
        }
    }
}
