// Copyright © Jason Curl 2012-2018
// Sources at https://github.com/jcurl/SerialPortStream
// Licensed under the Microsoft Public License (Ms-PL)

namespace RJCP.IO.Ports.Trace
{
    using System.Diagnostics;

    internal static class Log
    {
        private static readonly object m_RefLock = new object();
        private static int s_RefCounter;

        private static LogSource s_Serial;
        private static LogSource s_ReadTo;

        /// <summary>
        /// Adds a reference to using the tracing objects.
        /// </summary>
        /// <remarks>
        /// When you add a reference to this object, you should remove the reference with the
        /// <see cref="Close"/> method. When all references are gone, the trace objects are
        /// also closed.
        /// </remarks>
        public static void Open()
        {
            lock (m_RefLock) {
                if (s_RefCounter == 0) {
                    s_Serial = new LogSource("IO.Ports.SerialPortStream");
                    s_ReadTo = new LogSource("IO.Ports.SerialPortStream_ReadTo");
                }
                s_RefCounter++;
            }
        }

        /// <summary>
        /// Closes trace instances.
        /// </summary>
        public static void Close()
        {
            lock (m_RefLock) {
                s_RefCounter--;
                if (s_RefCounter == 0) {
                    if (s_ReadTo != null) {
                        s_ReadTo.Dispose();
                        s_ReadTo = null;
                    }
                    if (s_Serial != null) {
                        s_Serial.Dispose();
                        s_Serial = null;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the trace source object for serial port logging.
        /// </summary>
        /// <value>
        /// The trace source object for serial port logging.
        /// </value>
        public static TraceSource Serial
        {
            get { return s_Serial.TraceSource; }
        }

        /// <summary>
        /// Tests is tracing is enabled for the level requested.
        /// </summary>
        /// <param name="eventType">Type of the event that should be traced.</param>
        /// <returns><see langword="true"/> if tracing is enabled, <see langword="false"/> otherwise.</returns>
        public static bool SerialTrace(TraceEventType eventType)
        {
            return s_Serial.ShouldTrace(eventType);
        }

        /// <summary>
        /// Gets the trace source object for the ReadTo line implementation.
        /// </summary>
        /// <value>
        /// The trace source object for the ReadTo line implementation.
        /// </value>
        public static TraceSource ReadTo
        {
            get { return s_ReadTo.TraceSource; }
        }

        /// <summary>
        /// Tests is tracing is enabled for the level requested.
        /// </summary>
        /// <param name="eventType">Type of the event that should be traced.</param>
        /// <returns><see langword="true"/> if tracing is enabled, <see langword="false"/> otherwise.</returns>
        public static bool ReadToTrace(TraceEventType eventType)
        {
            return s_ReadTo.ShouldTrace(eventType);
        }
    }
}
