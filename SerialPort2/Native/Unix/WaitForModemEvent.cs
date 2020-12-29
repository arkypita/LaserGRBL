// Copyright © Jason Curl 2012-2016
// Sources at https://github.com/jcurl/SerialPortStream
// Licensed under the Microsoft Public License (Ms-PL)

namespace RJCP.IO.Ports.Native.Unix
{
    using System;

    [Flags]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S4070:Non-flags enums should not be marked with \"FlagsAttribute\"",
        Justification = "P/Invoke")]
    internal enum WaitForModemEvent
    {
        Error = -1,
        None = 0,
        DataCarrierDetect = 1,
        RingIndicator = 2,
        DataSetReady = 4,
        ClearToSend = 8
    }
}

