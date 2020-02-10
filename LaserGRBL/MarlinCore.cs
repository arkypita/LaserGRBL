using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaserGRBL
{
    public class MarlinCore : GrblCore
    {
        public MarlinCore(System.Windows.Forms.Control syncroObject, PreviewForm cbform) : base(syncroObject, cbform)
        {
        }

        public override Firmware Type
        { get { return Firmware.Marlin; } }

        // Send "M114\n" to retrieve position
        // Typical response : "X:10.00 Y:0.00 Z:0.00 E:0.00 Count X:1600 Y:0 Z:0"
        protected override void QueryPosition()
        {
            com.Write("M114\n");
        }

        //protected override void ParseF(string p)
        //{
        //    string sfs = p.Substring(2, p.Length - 2);
        //    string[] fs = sfs.Split(",".ToCharArray());
        //    SetFS(ParseFloat(fs[0]), ParseFloat(fs[1]));
        //}

        public override StreamingMode CurrentStreamingMode => StreamingMode.Synchronous;
        public override bool UIShowGrblConfig => false;
        public override bool UIShowUnlockButtons => false;
        public override bool SupportTrueJogging => false;
        internal override void SendUnlockCommand() { } //do nothing (should not be called because UI does not show unlock button)
        protected override void SendBoardResetCommand() { }

        protected override void ParseMachineStatus(string data)
        {
            MacStatus var = MacStatus.Disconnected;

            if (data.Contains("ok"))
                var = MacStatus.Idle;

            //try { var = (MacStatus)Enum.Parse(typeof(MacStatus), data); }
            //catch (Exception ex) { Logger.LogException("ParseMachineStatus", ex); }

            if (InProgram && var == MacStatus.Idle) //bugfix for grbl sending Idle on G4
                var = MacStatus.Run;

            if (var == MacStatus.Hold && !mHoldByUserRequest)
                var = MacStatus.Cooling;

            SetStatus(var);
        }

        public override void RefreshConfig()
        {

        }

        protected override void ManageRealTimeStatus(string rline)
        {
            try
            {
                debugLastStatusDelay.Start();

                // Remove EOL
                rline = rline.Trim(trimarray);

                // Marlin M114 response : 
                // X:10.00 Y:0.00 Z:0.00 E:0.00 Count X:1600 Y:0 Z:0
                // Split by space
                string[] arr = rline.Split(" ".ToCharArray());

                if (arr.Length > 0)
                {
                    // Force update of status 
                    ParseMachineStatus("ok");

                    // Retrieve position from data send by marlin
                    float x = float.Parse(arr[0].Split(":".ToCharArray())[1], System.Globalization.NumberFormatInfo.InvariantInfo);
                    float y = float.Parse(arr[1].Split(":".ToCharArray())[1], System.Globalization.NumberFormatInfo.InvariantInfo);
                    float z = float.Parse(arr[2].Split(":".ToCharArray())[1], System.Globalization.NumberFormatInfo.InvariantInfo);
                    SetMPosition(new GPoint(x, y, z));
                }
            }
            catch (Exception ex)
            {
                Logger.LogMessage("ManageRealTimeStatus", "Ex on [{0}] message", rline);
                Logger.LogException("ManageRealTimeStatus", ex);
            }
        }

        ////public override void SendImmediate(byte b, bool mute = false)
        ////{
        ////    try
        ////    {
        ////        if (!mute) Logger.LogMessage("SendImmediate", "Send Immediate Command [0x{0:X}]", b);

        ////        lock (this)
        ////        { if (com.IsOpen) com.Write(new byte[] { b, 10 }); }
        ////    }
        ////    catch (Exception ex)
        ////    { Logger.LogException("SendImmediate", ex); }
        ////}
    }

}
