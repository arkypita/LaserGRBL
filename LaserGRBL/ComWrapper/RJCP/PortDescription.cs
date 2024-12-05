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
        /// Gets or sets the manufacturer of the driver if available.
        /// </summary>
        /// <value>The manufacturer of the driver if available.</value>
        public string Manufacturer { get; set; }

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

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(Manufacturer)) {
                return Description;
            }
            return $"{Description} [{Manufacturer}]";
        }
    }
}
