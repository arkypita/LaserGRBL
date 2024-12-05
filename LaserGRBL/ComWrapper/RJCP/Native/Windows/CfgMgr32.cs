namespace RJCP.IO.Ports.Native.Windows
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.Win32.SafeHandles;

    internal static partial class CfgMgr32
    {
        private const int MaxLengthStack = 128;

        [DllImport("cfgmgr32.dll", SetLastError = false, CharSet = CharSet.Unicode, ExactSpelling = true, EntryPoint = "CM_Get_Device_ID_List_SizeW")]
        private static extern CONFIGRET CM_Get_Device_ID_List_Size(out int length, string filter, int flags);

        [DllImport("cfgmgr32.dll", SetLastError = false, CharSet = CharSet.Unicode, ExactSpelling = true, EntryPoint = "CM_Get_Device_ID_ListW")]
        private static extern CONFIGRET CM_Get_Device_ID_List(string filter, [Out] char[] buffer, int length, int flags);

        public static unsafe CONFIGRET CM_Get_Device_ID_List(string filter, out string[] buffer)
        {
            CONFIGRET ret = CM_Get_Device_ID_List_Size(out int length, filter, 0);
            if (ret != CONFIGRET.CR_SUCCESS) {
                buffer = null;
                return ret;
            }

            char[] blob = new char[length];
            ret = CM_Get_Device_ID_List(filter, blob, length, 0);
            if (ret != CONFIGRET.CR_SUCCESS) {
                buffer = null;
                return ret;
            }
            buffer = Marshalling.GetMultiSz(blob, length).ToArray();
            return ret;
        }

        [DllImport("cfgmgr32.dll", SetLastError = false, CharSet = CharSet.Unicode, ExactSpelling = true, EntryPoint = "CM_Locate_DevNodeW")]
        public static extern CONFIGRET CM_Locate_DevNode(out SafeDevInst devInst, string devInstId, CM_LOCATE_DEVINST flags);

        [DllImport("cfgmgr32.dll", SetLastError = false, ExactSpelling = true, EntryPoint = "CM_Open_DevNode_Key")]
        public static extern CONFIGRET CM_Open_DevNode_Key(SafeDevInst devInst, Kernel32.REGSAM samDesired, int hardwareProfile, RegDisposition disposition, out SafeRegistryHandle device, int flags);

        // We must set CharSet so when we query the length for strings, we get the correct value.
        [DllImport("cfgmgr32.dll", SetLastError = false, CharSet = CharSet.Unicode, ExactSpelling = true, EntryPoint = "CM_Get_DevNode_Registry_PropertyW")]
        private static extern CONFIGRET CM_Get_DevNode_Registry_Property(SafeDevInst devInst, CM_DRP property, out int dataType, IntPtr buffer, ref int bufferLen, int flags);

        [DllImport("cfgmgr32.dll", SetLastError = false, CharSet = CharSet.Unicode, ExactSpelling = true, EntryPoint = "CM_Get_DevNode_Registry_PropertyW")]
        private static extern CONFIGRET CM_Get_DevNode_Registry_Property(SafeDevInst devInst, CM_DRP property, out int dataType, out int buffer, ref int bufferLen, int flags);

        [DllImport("cfgmgr32.dll", SetLastError = false, CharSet = CharSet.Unicode, ExactSpelling = true, EntryPoint = "CM_Get_DevNode_Registry_PropertyW")]
        private static extern CONFIGRET CM_Get_DevNode_Registry_Property(SafeDevInst devInst, CM_DRP property, out int dataType, [Out] char[] buffer, ref int bufferLen, int flags);

        [DllImport("cfgmgr32.dll", SetLastError = false, CharSet = CharSet.Unicode, ExactSpelling = true, EntryPoint = "CM_Get_DevNode_Registry_PropertyW")]
        private static unsafe extern CONFIGRET CM_Get_DevNode_Registry_Property(SafeDevInst devInst, CM_DRP property, out int dataType, char* buffer, ref int bufferLen, int flags);

        public static unsafe CONFIGRET CM_Get_DevNode_Registry_Property(SafeDevInst devInst, CM_DRP property, out int dataType, out string buffer)
        {
            int length = 0;
            CONFIGRET ret = CM_Get_DevNode_Registry_Property(devInst, property, out dataType, IntPtr.Zero, ref length, 0);
            if (ret != CONFIGRET.CR_SUCCESS && ret != CONFIGRET.CR_BUFFER_SMALL) {
                buffer = string.Empty;
                return ret;
            }

            if (length <= 0) {
                buffer = string.Empty;
                return CONFIGRET.CR_UNEXPECTED_LENGTH;
            }

            Kernel32.REG_DATATYPE regDataType = (Kernel32.REG_DATATYPE)dataType;
            if (regDataType != Kernel32.REG_DATATYPE.REG_SZ) {
                buffer = string.Empty;
                return CONFIGRET.CR_UNEXPECTED_TYPE;
            }

            if (length % 2 == 1) length++;
            int bloblen = length / 2;

            if (length <= MaxLengthStack) {
                char* blob = stackalloc char[bloblen];
                ret = CM_Get_DevNode_Registry_Property(devInst, property, out _, blob, ref length, 0);
                if (ret != CONFIGRET.CR_SUCCESS) {
                    buffer = string.Empty;
                    return ret;
                }

                // Subtract one for the NUL at the end.
                if (blob[bloblen - 1] == (char)0) bloblen--;
                buffer = new string(blob, 0, bloblen);
            } else {
                // NETSTANDARD 1.5 doesn't have ArrayPool.

                char[] blob = new char[bloblen];
                ret = CM_Get_DevNode_Registry_Property(devInst, property, out _, blob, ref length, 0);
                if (ret != CONFIGRET.CR_SUCCESS) {
                    buffer = string.Empty;
                    return ret;
                }

                // Subtract one for the NUL at the end.
                if (blob[bloblen - 1] == (char)0) bloblen--;
                buffer = new string(blob, 0, bloblen);
            }
            return ret;
        }
    }
}
