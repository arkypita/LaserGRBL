using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaserGRBL
{
    public class SmoothieCore : GrblCore
    {
        public SmoothieCore(System.Windows.Forms.Control syncroObject, PreviewForm cbform) : base(syncroObject, cbform)
        {
        }

        public override Firmware Type
        { get { return Firmware.Smoothie; } }

        protected override void InitializeBoard()
        {
            SendImmediate(10); //send a new line
            base.InitializeBoard();
        }

        protected override void ParseF(string p)
        {
            string sfs = p.Substring(2, p.Length - 2);
            string[] fs = sfs.Split(",".ToCharArray());
            SetFS(ParseFloat(fs[0]), ParseFloat(fs[1]));
        }

        public override StreamingMode CurrentStreamingMode => StreamingMode.Synchronous;

        public override bool UIShowGrblConfig => false;
        public override bool UIShowUnlockButtons => false;

        public override bool SupportTrueJogging => false;

        internal override void SendUnlockCommand() { } //do nothing (should not be called because UI does not show unlock button)
        protected override void SendBoardResetCommand() { com.Write("reset\r\n"); } //is it possible to write directly without push into queue??? please verify!!

        //public override void SendImmediate(byte b, bool mute = false)
        //{
        //    try
        //    {
        //        if (!mute) Logger.LogMessage("SendImmediate", "Send Immediate Command [0x{0:X}]", b);

        //        lock (this)
        //        { if (com.IsOpen) com.Write(new byte[] { b, 10 }); }
        //    }
        //    catch (Exception ex)
        //    { Logger.LogException("SendImmediate", ex); }
        //}
    }

}
