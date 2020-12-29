// Copyright © Jason Curl 2012-2016
// Sources at https://github.com/jcurl/SerialPortStream
// Licensed under the Microsoft Public License (Ms-PL)

namespace RJCP.IO.Ports
{
    /// <summary>
    /// A class containing information about a serial port.
    /// </summary>
    public class PortDescription
    {
        /// <summary>
        /// The name of the port
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// Description about the serial port.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="port">The name of the port.</param>
        /// <param name="description">Description about the serial port.</param>
        public PortDescription(string port, string description)
        {
            Port = port;
            Description = description;
        }
    }
}
