// Copyright © Jason Curl 2012-2016
// Sources at https://github.com/jcurl/SerialPortStream
// Licensed under the Microsoft Public License (Ms-PL)

namespace RJCP.IO.Ports
{
    using System;

    /// <summary>
    /// EventArgs for DataReceived.
    /// </summary>
    public class SerialDataReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="eventType">Event that occurred.</param>
        public SerialDataReceivedEventArgs(SerialData eventType)
        {
            EventType = eventType;
        }

        /// <summary>
        /// The event type for DataReceived.
        /// </summary>
        public SerialData EventType { get; private set; }
    }
}
