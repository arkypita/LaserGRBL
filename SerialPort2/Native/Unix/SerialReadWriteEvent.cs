// Copyright © Jason Curl 2012-2016
// Sources at https://github.com/jcurl/SerialPortStream
// Licensed under the Microsoft Public License (Ms-PL)

namespace RJCP.IO.Ports.Native.Unix
{
    using System;

    [Flags]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S4070:Non-flags enums should not be marked with \"FlagsAttribute\"",
        Justification = "Enum is flags")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S2346:Flags enumerations zero-value members should be named \"None\"",
        Justification = "Existing name is readable")]
    internal enum SerialReadWriteEvent
    {
        Error = -1,
        NoEvent = 0,
        ReadEvent = 1,
        WriteEvent = 2,
        ReadWriteEvent = ReadEvent + WriteEvent
    }
}
