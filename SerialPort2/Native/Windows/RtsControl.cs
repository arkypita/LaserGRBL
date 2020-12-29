// Copyright © Jason Curl 2012-2016
// Sources at https://github.com/jcurl/SerialPortStream
// Licensed under the Microsoft Public License (Ms-PL)

namespace RJCP.IO.Ports.Native.Windows
{
    /// <summary>
    /// RTS (Request to Send) to use.
    /// </summary>
    internal enum RtsControl
    {
        /// <summary>
        /// Disable RTS line.
        /// </summary>
        Disable = 0,

        /// <summary>
        /// Enable the RTS line.
        /// </summary>
        Enable = 1,

        /// <summary>
        /// RTS Handshaking.
        /// </summary>
        Handshake = 2,

        /// <summary>
        /// RTS Toggling.
        /// </summary>
        Toggle = 3
    }
}
