// Copyright © Jason Curl 2012-2021
// Sources at https://github.com/jcurl/SerialPortStream
// Licensed under the Microsoft Public License (Ms-PL)

namespace RJCP.IO.Ports.Trace
{
    internal static class Log
    {
        public const string SerialPortStream = "IO.Ports.SerialPortStream";
        public const string SerialPortStream_ReadTo = "IO.Ports.SerialPortStream_ReadTo";

        private static readonly object m_RefLock = new object();

        private static LogSource s_Serial;
        private static LogSource s_ReadTo;

        public static LogSource Serial
        {
            get
            {
                lock (m_RefLock) {
                    if (s_Serial == null) s_Serial = new LogSource(SerialPortStream);
                }
                return s_Serial;
            }
        }

        public static LogSource ReadTo
        {
            get
            {
                lock (m_RefLock) {
                    if (s_ReadTo == null) s_ReadTo = new LogSource(SerialPortStream_ReadTo);
                }
                return s_ReadTo;
            }
        }
    }
}
