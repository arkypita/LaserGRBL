// Copyright © Jason Curl 2012-2016
// Sources at https://github.com/jcurl/SerialPortStream
// Licensed under the Microsoft Public License (Ms-PL)

namespace RJCP.IO.Ports
{
    using System;

    /// <summary>
    /// Handshaking mode to use.
    /// </summary>
    [Flags]
    public enum Handshake
    {
        /// <summary>
        /// No handshaking.
        /// </summary>
        None = 0,

        /// <summary>
        /// Software handshaking.
        /// </summary>
        XOn = 1,

        /// <summary>
        /// Hardware handshaking (RTS/CTS).
        /// </summary>
        Rts = 2,

        /// <summary>
        /// Hardware handshaking (DTR/DSR) (uncommon).
        /// </summary>
        Dtr = 4,

        /// <summary>
        /// RTS and Software handshaking.
        /// </summary>
        RtsXOn = Rts | XOn,

        /// <summary>
        /// DTR and Software handshaking (uncommon).
        /// </summary>
        DtrXOn = Dtr | XOn,

        /// <summary>
        /// Hardware handshaking with RTS/CTS and DTR/DSR (uncommon).
        /// </summary>
        DtrRts = Dtr | Rts,

        /// <summary>
        /// Hardware handshaking with RTS/CTS and DTR/DSR and Software handshaking (uncommon).
        /// </summary>
        DtrRtsXOn = Dtr | Rts | XOn
    }
}
