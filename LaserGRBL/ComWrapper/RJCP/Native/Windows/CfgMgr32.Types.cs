namespace RJCP.IO.Ports.Native.Windows
{
    using System;
    using System.Runtime.InteropServices;

    internal static partial class CfgMgr32
    {
        public enum CONFIGRET
        {
            CR_SUCCESS = 0x00000000,
            CR_DEFAULT = 0x00000001,
            CR_OUT_OF_MEMORY = 0x00000002,
            CR_INVALID_POINTER = 0x00000003,
            CR_INVALID_FLAG = 0x00000004,
            CR_INVALID_DEVNODE = 0x00000005,
            CR_INVALID_DEVINST = CR_INVALID_DEVNODE,
            CR_INVALID_RES_DES = 0x00000006,
            CR_INVALID_LOG_CONF = 0x00000007,
            CR_INVALID_ARBITRATOR = 0x00000008,
            CR_INVALID_NODELIST = 0x00000009,
            CR_DEVNODE_HAS_REQS = 0x0000000A,
            CR_DEVINST_HAS_REQS = CR_DEVNODE_HAS_REQS,
            CR_INVALID_RESOURCEID = 0x0000000B,
            CR_DLVXD_NOT_FOUND = 0x0000000C,       // WIN 95 ONLY
            CR_NO_SUCH_DEVNODE = 0x0000000D,
            CR_NO_SUCH_DEVINST = CR_NO_SUCH_DEVNODE,
            CR_NO_MORE_LOG_CONF = 0x0000000E,
            CR_NO_MORE_RES_DES = 0x0000000F,
            CR_ALREADY_SUCH_DEVNODE = 0x00000010,
            CR_ALREADY_SUCH_DEVINST = CR_ALREADY_SUCH_DEVNODE,
            CR_INVALID_RANGE_LIST = 0x00000011,
            CR_INVALID_RANGE = 0x00000012,
            CR_FAILURE = 0x00000013,
            CR_NO_SUCH_LOGICAL_DEV = 0x00000014,
            CR_CREATE_BLOCKED = 0x00000015,
            CR_NOT_SYSTEM_VM = 0x00000016,         // WIN 95 ONLY
            CR_REMOVE_VETOED = 0x00000017,
            CR_APM_VETOED = 0x00000018,
            CR_INVALID_LOAD_TYPE = 0x00000019,
            CR_BUFFER_SMALL = 0x0000001A,
            CR_NO_ARBITRATOR = 0x0000001B,
            CR_NO_REGISTRY_HANDLE = 0x0000001C,
            CR_REGISTRY_ERROR = 0x0000001D,
            CR_INVALID_DEVICE_ID = 0x0000001E,
            CR_INVALID_DATA = 0x0000001F,
            CR_INVALID_API = 0x00000020,
            CR_DEVLOADER_NOT_READY = 0x00000021,
            CR_NEED_RESTART = 0x00000022,
            CR_NO_MORE_HW_PROFILES = 0x00000023,
            CR_DEVICE_NOT_THERE = 0x00000024,
            CR_NO_SUCH_VALUE = 0x00000025,
            CR_WRONG_TYPE = 0x00000026,
            CR_INVALID_PRIORITY = 0x00000027,
            CR_NOT_DISABLEABLE = 0x00000028,
            CR_FREE_RESOURCES = 0x00000029,
            CR_QUERY_VETOED = 0x0000002A,
            CR_CANT_SHARE_IRQ = 0x0000002B,
            CR_NO_DEPENDENT = 0x0000002C,
            CR_SAME_RESOURCES = 0x0000002D,
            CR_NO_SUCH_REGISTRY_KEY = 0x0000002E,
            CR_INVALID_MACHINENAME = 0x0000002F,   // NT ONLY
            CR_REMOTE_COMM_FAILURE = 0x00000030,   // NT ONLY
            CR_MACHINE_UNAVAILABLE = 0x00000031,   // NT ONLY
            CR_NO_CM_SERVICES = 0x00000032,        // NT ONLY
            CR_ACCESS_DENIED = 0x00000033,         // NT ONLY
            CR_CALL_NOT_IMPLEMENTED = 0x00000034,
            CR_INVALID_PROPERTY = 0x00000035,
            CR_DEVICE_INTERFACE_ACTIVE = 0x00000036,
            CR_NO_SUCH_DEVICE_INTERFACE = 0x00000037,
            CR_INVALID_REFERENCE_STRING = 0x00000038,
            CR_INVALID_CONFLICT_LIST = 0x00000039,
            CR_INVALID_INDEX = 0x0000003A,
            CR_INVALID_STRUCTURE_SIZE = 0x0000003B,

            // Custom return codes for this module, not defined in Win32 API
            CR_UNEXPECTED_TYPE = 0x40000001,
            CR_UNEXPECTED_LENGTH = 0x40000002,
        }

        public enum CM_LOCATE_DEVINST
        {
            NORMAL = 0x00000000,
            PHANTOM = 0x00000001,
            CANCELREMOVE = 0x00000002,
            NOVALIDATION = 0x00000004,
            BITS = 0x00000007
        }

        public enum RegDisposition
        {
            OpenAlways = 0,
            OpenExisting = 1
        }

        public enum CM_DRP
        {
            DEVICEDESC = 0x00000001,                  // DeviceDesc REG_SZ property (RW)
            HARDWAREID = 0x00000002,                  // HardwareID REG_MULTI_SZ property (RW)
            COMPATIBLEIDS = 0x00000003,               // CompatibleIDs REG_MULTI_SZ property (RW)
            UNUSED0 = 0x00000004,                     // unused
            SERVICE = 0x00000005,                     // Service REG_SZ property (RW)
            UNUSED1 = 0x00000006,                     // unused
            UNUSED2 = 0x00000007,                     // unused
            CLASS = 0x00000008,                       // Class REG_SZ property (RW)
            CLASSGUID = 0x00000009,                   // ClassGUID REG_SZ property (RW)
            DRIVER = 0x0000000A,                      // Driver REG_SZ property (RW)
            CONFIGFLAGS = 0x0000000B,                 // ConfigFlags REG_DWORD property (RW)
            MFG = 0x0000000C,                         // Mfg REG_SZ property (RW)
            FRIENDLYNAME = 0x0000000D,                // FriendlyName REG_SZ property (RW)
            LOCATION_INFORMATION = 0x0000000E,        // LocationInformation REG_SZ property (RW)
            PHYSICAL_DEVICE_OBJECT_NAME = 0x0000000F, // PhysicalDeviceObjectName REG_SZ property (R)
            CAPABILITIES = 0x00000010,                // Capabilities REG_DWORD property (R)
            UI_NUMBER = 0x00000011,                   // UiNumber REG_DWORD property (R)
            UPPERFILTERS = 0x00000012,                // UpperFilters REG_MULTI_SZ property (RW)
            LOWERFILTERS = 0x00000013,
            BUSTYPEGUID = 0x00000014,                 // Bus Type Guid, GUID, (R)
            LEGACYBUSTYPE = 0x00000015,               // Legacy bus type, INTERFACE_TYPE, (R)
            BUSNUMBER = 0x00000016,                   // Bus Number, DWORD, (R)
            ENUMERATOR_NAME = 0x00000017,             // Enumerator Name REG_SZ property (R)
            SECURITY = 0x00000018,                    // Security - Device override (RW)
            SECURITY_SDS = 0x00000019,                // Security - Device override (RW)
            DEVTYPE = 0x0000001A,                     // Device Type - Device override (RW)
            EXCLUSIVE = 0x0000001B,                   // Exclusivity - Device override (RW)
            CHARACTERISTICS = 0x0000001C,             // Characteristics - Device Override (RW)
            ADDRESS = 0x0000001D,                     // Device Address (R)
            UI_NUMBER_DESC_FORMAT = 0x0000001E,       // UINumberDescFormat REG_SZ property (RW)

            // WinXP and later
            DEVICE_POWER_DATA = 0x0000001F,           // CM_POWER_DATA REG_BINARY property (R)
            REMOVAL_POLICY = 0x00000020,              // CM_DEVICE_REMOVAL_POLICY REG_DWORD (R)
            REMOVAL_POLICY_HW_DEFAULT = 0x00000021,   // CM_DRP_REMOVAL_POLICY_HW_DEFAULT REG_DWORD (R)
            REMOVAL_POLICY_OVERRIDE = 0x00000022,     // CM_DRP_REMOVAL_POLICY_OVERRIDE REG_DWORD (RW)
            INSTALL_STATE = 0x00000023,               // CM_DRP_INSTALL_STATE REG_DWORD (R)

            // Windows 2003 and later
            LOCATION_PATHS = 0x00000024,              // CM_DRP_LOCATION_PATHS REG_MULTI_SZ (R)

            // Windows 7 and later
            BASE_CONTAINERID = 0x00000025             // Base ContainerID REG_SZ property (R)
        }

        public class SafeDevInst : SafeHandle
        {
            private static readonly IntPtr MinusOne = new IntPtr(-1);

            public SafeDevInst() : base(MinusOne, true) { }

            public SafeDevInst(IntPtr newHandle) : base(newHandle, true) { }

            public override bool IsInvalid
            {
                get { return handle == IntPtr.Zero || handle == MinusOne; }
            }

            protected override bool ReleaseHandle()
            {
                // There is no documentation that this handle should be closed. When trying to close through single stepping
                // with a debugger, we get an exception on Windows 10. Further, we see that when querying the root multiple
                // times, the actual value of the handle is constant, thus suggesting that the implementation doesn't need
                // to be freed.

                // So in effect, we use this class to wrap the result of the Win32 API in a type safe manner, but don't need
                // the atomic / clean disposal of the object.

                //return Kernel32.CloseHandle(handle);

                // Also, please note, that it is expected that closing this handle has no effect, as when we return a new
                // handle via DeviceInstance.Handle, we copy it, so if the user closes that copy (which has no effect), it
                // won't affect us here.
                SetHandleAsInvalid();
                return true;
            }
        }
    }
}
