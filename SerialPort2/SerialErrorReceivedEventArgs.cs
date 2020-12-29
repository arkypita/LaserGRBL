// Copyright © Jason Curl 2012-2016
// Sources at https://github.com/jcurl/SerialPortStream
// Licensed under the Microsoft Public License (Ms-PL)

namespace RJCP.IO.Ports
{
    using System;

    /// <summary>
    /// EventArgs for ErrorReceived.
    /// </summary>
    public class SerialErrorReceivedEventArgs : EventArgs
    {
        private SerialError m_EventType;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="eventType">Event that occurred.</param>
        public SerialErrorReceivedEventArgs(SerialError eventType)
        {
            m_EventType = eventType;
        }

        /// <summary>
        /// The event type for ErrorReceived.
        /// </summary>
        public SerialError EventType
        {
            get { return m_EventType; }
        }
    }
}
