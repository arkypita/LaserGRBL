// Copyright © Jason Curl 2012-2016
// Sources at https://github.com/jcurl/SerialPortStream
// Licensed under the Microsoft Public License (Ms-PL)

namespace RJCP.IO.Ports.Native.Windows
{
    using System;

    internal class CommErrorEventArgs : EventArgs
    {
        private NativeMethods.ComStatErrors m_EventType;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="eventType"><see cref="NativeMethods.ComStatErrors"/> result.</param>
        public CommErrorEventArgs(NativeMethods.ComStatErrors eventType)
        {
            m_EventType = eventType;
        }

        /// <summary>
        /// <see cref="NativeMethods.ComStatErrors"/> result.
        /// </summary>
        public NativeMethods.ComStatErrors EventType
        {
            get { return m_EventType; }
        }
    }
}
