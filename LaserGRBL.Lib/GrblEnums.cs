//Copyright (c) 2016-2020 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

namespace LaserGRBL
{
    public enum CommandStatus
    {
        Queued,
        WaitingResponse,
        ResponseGood,
        ResponseBad,
        InvalidResponse
    }

    public enum DetectedIssue
    {
        Unknown = 0,
        ManualReset = -1,
        ManualDisconnect = -2,
        ManualAbort = -3,
        StopResponding = 1,
        //StopMoving = 2, 
        UnexpectedReset = 3,
        UnexpectedDisconnect = 4,
    }

    public enum MacStatus
    {
        Unknown,
        Disconnected,
        Connecting,
        Idle,
        Run,
        Hold,
        Door,
        Home,
        Alarm,
        Check,
        Jog,
        Queue,
        Cooling
    }

    public enum JogDirection
    {
        None,
        Abort,
        Home,
        N,
        S,
        W,
        E,
        NW,
        NE,
        SW,
        SE,
        Zup,
        Zdown
    }

    public enum StreamingMode
    {
        Buffered,
        Synchronous,
        RepeatOnError
    }

    public enum MessageType
    {
        Startup,
        Config,
        Alarm,
        Feedback,
        Position,
        Others
    }

    public enum Firmware
    {
        Grbl,
        Smoothie,
        Marlin
    }
}
