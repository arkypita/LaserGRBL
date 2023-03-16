﻿//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaserGRBL
{
    public class MarlinCore : GrblCore
    {
        public MarlinCore(System.Windows.Forms.Control syncroObject, PreviewForm cbform, JogForm jogform) : base(syncroObject, cbform, jogform)
        {
        }

        public override Firmware Type
        { get { return Firmware.Marlin; } }

        // Send "M114\n" to retrieve position
        // Typical response : "X:10.00 Y:0.00 Z:0.00 E:0.00 Count X:1600 Y:0 Z:0"
        protected override void QueryPosition()
        {
            if (MachineStatus != MacStatus.Run)
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
            if (data.Contains("echo:busy:") || data.Contains("echo: busy:"))
            {
                var = MacStatus.Run;
            }

            //try { var = (MacStatus)Enum.Parse(typeof(MacStatus), data); }
            //catch (Exception ex) { Logger.LogException("ParseMachineStatus", ex); }

            if (InProgram && var == MacStatus.Idle) //bugfix for grbl sending Idle on G4
                var = MacStatus.Run;

            if (var == MacStatus.Hold && mHoldByCoolingRequest)
                var = MacStatus.Cooling;
            else if (var == MacStatus.Hold && !mHoldByUserRequest)
                var = MacStatus.AutoHold;

            SetStatus(var);
        }

        public override void RefreshConfig()
        {

        }
		public override void RefreshMachineInfo()
		{
			
		}

        internal override void SendHomingCommand()
        {
            EnqueueCommand(new GrblCommand("G28"));
        }

        public override List<String> getSpindleGcode(SpindleState state, int power)
        {
            List<string> LaserCmd = new List<string>();
            if (mSpindleConfig.pwmMode == GrblCore.PwmMode.Fan)
            {
                LaserCmd.Add(String.Format("G4 P0"));
                if(state == SpindleState.ON)
                    LaserCmd.Add(String.Format("{0} S{1} P{2}", mSpindleConfig.lOn , power, mSpindleConfig.fanId)); //laser on and power to zero
                else
                    LaserCmd.Add(String.Format("{0} P{1}", mSpindleConfig.lOff, mSpindleConfig.fanId)); //laser on and power to zero
                if (state == SpindleState.ON && power > 0)
                    LaserCmd.Add(String.Format("G4 P{0}", mSpindleConfig.dwelltime));
            }
            else
            {
                if (state == SpindleState.ON)
                    LaserCmd.Add(String.Format("{0} S{1}", mSpindleConfig.lOn , power));
                else
                    LaserCmd.Add(String.Format("{0}", mSpindleConfig.lOn));
            }
            return LaserCmd;
        }

        internal override void GrblHoming()
        { if (CanDoHoming) EnqueueCommand(new GrblCommand("G28")); }

        protected override void DetectHang()
        {
            if (mTP.LastIssue == DetectedIssue.Unknown && MachineStatus == MacStatus.Run && InProgram)
            {
                // Marlin does not answer to immediate command
                // So we can not rise an issue if there is too many time before last call of ManageRealTimeStatus
                // We rise the issue if the last response from the board was too long

                // Original line :
                //bool noQueryResponse = debugLastMoveDelay.ElapsedTime > TimeSpan.FromTicks(QueryTimer.Period.Ticks * 10) && debugLastStatusDelay.ElapsedTime > TimeSpan.FromSeconds(5);
                // Marlin version :
                bool noQueryResponse = debugLastMoveOrActivityDelay.ElapsedTime > TimeSpan.FromSeconds(10) && debugLastMoveOrActivityDelay.ElapsedTime > TimeSpan.FromTicks(QueryTimer.Period.Ticks * 10) && debugLastStatusDelay.ElapsedTime > TimeSpan.FromSeconds(5);

                if (noQueryResponse)
                    SetIssue(DetectedIssue.StopResponding);

            }
        }

        public override void FeedHold(bool auto)
        {
            if (CanFeedHold)
            {
                mHoldByUserRequest = !auto;
                GrblCommand stop = null;
                if (Settings.GetObject("Firmware Type", Firmware.Grbl) == Firmware.Marlin && Settings.GetObject("Pwm Selection", GrblCore.PwmMode.Spindle) == GrblCore.PwmMode.Fan)
                {
                    stop = new GrblCommand("M107");
                }
                else
                {
                    stop = new GrblCommand("M5");
                }
                mQueue.Clear(); //flush the queue of item to send
                mQueue.Enqueue(stop); //shut down laser
                mQueue.Enqueue(new GrblCommand("M112"));
            }
        }

        protected override void ManageReceivedLine(string rline)
		{
			if (IsMarlinRealTimeStatusMessage(rline))
				ManageMarlinRealTimeStatus(rline);
			else if(rline.Contains("echo:busy:") || rline.Contains("echo: busy:"))
			{
				SetStatus(MacStatus.Run);
				debugLastStatusDelay.Start();
			}
			else
				base.ManageReceivedLine(rline);
		}

		private bool IsMarlinRealTimeStatusMessage(string rline) => rline.StartsWith("X:");

		private void ManageMarlinRealTimeStatus(string rline)
		{
			try
			{
				debugLastStatusDelay.Start();

				// Remove EOL
				rline = rline.Trim(trimarray); //maybe not necessary (already done)

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

		// LaserGRBL don't ask status to marlin during code execution because there is no immediate command
		// So LaserGRBL has to force the status at the end of programm execution
		protected override void ForceStatusIdle ()
        {
            SetStatus(MacStatus.Idle);
        }

    }

}
