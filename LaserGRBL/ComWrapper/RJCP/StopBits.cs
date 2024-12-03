// Copyright © Jason Curl 2012-2016
// Sources at https://github.com/jcurl/SerialPortStream
// Licensed under the Microsoft Public License (Ms-PL)

namespace RJCP.IO.Ports
{
    /// <summary>
    /// Number of stop bits to use.
    /// </summary>
    public enum StopBits
    {
        /// <summary>
        /// One stop bit.
        /// </summary>
        One = 0,

        /// <summary>
        /// 1.5 stop bits.
        /// </summary>
        One5 = 1,

        /// <summary>
        /// Two stop bits.
        /// </summary>
        Two = 2
    }
}
