// Copyright © Jason Curl 2012-2016
// Sources at https://github.com/jcurl/SerialPortStream
// Licensed under the Microsoft Public License (Ms-PL)

namespace RJCP.IO.Ports
{
    /// <summary>
    /// The type of parity to use.
    /// </summary>
    public enum Parity
    {
        /// <summary>
        /// No parity.
        /// </summary>
        None = 0,

        /// <summary>
        /// Odd parity.
        /// </summary>
        Odd = 1,

        /// <summary>
        /// Even parity.
        /// </summary>
        Even = 2,

        /// <summary>
        /// Mark parity.
        /// </summary>
        Mark = 3,

        /// <summary>
        /// Space parity.
        /// </summary>
        Space = 4
    }
}
