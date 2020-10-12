//Copyright (c) 2016-2020 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaserGRBL
{
    public class VigoCore : GrblCore
    {
        public VigoCore(System.Windows.Forms.Control syncroObject, PreviewForm cbform, JogForm jogform) : base(syncroObject, cbform, jogform)
        {

        }

        public override Firmware Type
        { get { return Firmware.VigoWork; } }

		protected override void QueryPosition()
		{
			SendImmediate(0x88, true);
		}

		public override int BufferSize => 127;

		protected override void ManageReceivedLine(string rline)
		{
			if (IsVigoStatusMessage(rline))
				ManageVigoStatus(rline);
			else
				base.ManageReceivedLine(rline);
		}

		protected virtual bool IsVigoStatusMessage(string rline)
		{
			return rline.StartsWith("<VSta") && rline.EndsWith(">");
		}

		protected virtual void ManageVigoStatus(string rline)
		{
			try
			{
				//<VSta:2|SBuf:5,1,0|LTC:4095>

				rline = rline.Substring(1, rline.Length - 2);
				string[] arr = rline.Split("|".ToCharArray());

				for (int i = 0; i < arr.Length; i++)
				{
					if (arr[i].StartsWith("VSta:"))
						;// ParseVSta(arr[i]);
					else if (arr[i].StartsWith("SBuf:"))
						ParseSBuf(arr[i]);
					else if (arr[i].StartsWith("LTC:"))
						;// ParseLTC(arr[i]);
				}
				System.Diagnostics.Debug.WriteLine(rline);
			}
			catch (Exception ex)
			{
				Logger.LogMessage("VigoStatus", "Ex on [{0}] message", rline);
				Logger.LogException("VigoStatus", ex);
			}
		}

		private int mOldReceived = 0;
		private int mOldManaged = 0;
		private int mOldFoo = 0;
		private void ParseSBuf(string p)
		{
			string wco = p.Substring(5, p.Length - 5);
			string[] xyz = wco.Split(",".ToCharArray());
			int mReceived = (int)ParseFloat(xyz[1]);

			if (mReceived != mOldReceived)
			{
				for (int i = mOldReceived; i < mReceived; i++)
					ManageCommandResponse("ok");
				mOldReceived = mReceived;
			}
		}

		//protected override void ParseMachineStatus(string data)
		//{
		//    MacStatus var = MacStatus.Disconnected;

		//    if (data.Contains("ok"))
		//        var = MacStatus.Idle;

		//    //try { var = (MacStatus)Enum.Parse(typeof(MacStatus), data); }
		//    //catch (Exception ex) { Logger.LogException("ParseMachineStatus", ex); }

		//    if (InProgram && var == MacStatus.Idle) //bugfix for grbl sending Idle on G4
		//        var = MacStatus.Run;

		//    if (var == MacStatus.Hold && !mHoldByUserRequest)
		//        var = MacStatus.Cooling;

		//    SetStatus(var);
		//}


		//protected override void ManageRealTimeStatus(string rline)
		//{
		//    try
		//    {
		//        debugLastStatusDelay.Start();

		//        // Remove EOL
		//        rline = rline.Trim(trimarray);

		//        // Marlin M114 response : 
		//        // X:10.00 Y:0.00 Z:0.00 E:0.00 Count X:1600 Y:0 Z:0
		//        // Split by space
		//        string[] arr = rline.Split(" ".ToCharArray());

		//        if (arr.Length > 0)
		//        {
		//            // Force update of status 
		//            ParseMachineStatus("ok");

		//            // Retrieve position from data send by marlin
		//            float x = float.Parse(arr[0].Split(":".ToCharArray())[1], System.Globalization.NumberFormatInfo.InvariantInfo);
		//            float y = float.Parse(arr[1].Split(":".ToCharArray())[1], System.Globalization.NumberFormatInfo.InvariantInfo);
		//            float z = float.Parse(arr[2].Split(":".ToCharArray())[1], System.Globalization.NumberFormatInfo.InvariantInfo);
		//            SetMPosition(new GPoint(x, y, z));
		//        }
		//    }
		//    catch (Exception ex)
		//    {
		//        Logger.LogMessage("ManageRealTimeStatus", "Ex on [{0}] message", rline);
		//        Logger.LogException("ManageRealTimeStatus", ex);
		//    }
		//}

		//protected override void DetectHang()
		//{
		//    if (mTP.LastIssue == DetectedIssue.Unknown && MachineStatus == MacStatus.Run && InProgram)
		//    {
		//        // Marlin does not answer to immediate command
		//        // So we can not rise an issue if there is too many time before last call of ManageRealTimeStatus
		//        // We rise the issue if the last response from the board was too long

		//        // Original line :
		//        //bool noQueryResponse = debugLastMoveDelay.ElapsedTime > TimeSpan.FromTicks(QueryTimer.Period.Ticks * 10) && debugLastStatusDelay.ElapsedTime > TimeSpan.FromSeconds(5);
		//        // Marlin version :
		//        bool noQueryResponse = debugLastMoveDelay.ElapsedTime > TimeSpan.FromSeconds(10) && debugLastMoveDelay.ElapsedTime > TimeSpan.FromTicks(QueryTimer.Period.Ticks * 10) && debugLastStatusDelay.ElapsedTime > TimeSpan.FromSeconds(5);

		//        if (noQueryResponse)
		//            SetIssue(DetectedIssue.StopResponding);

		//    }
		//}

		//// Return true if message received start with X:
		//protected override bool IsRealtimeStatusMessage(string rline)
		//{
		//    return rline.StartsWith("X:");
		//}

		//// LaserGRBL don't ask status to marlin during code execution because there is no immediate command
		//// So LaserGRBL has to force the status at the end of programm execution
		//protected override void ForceStatusIdle ()
		//{
		//    SetStatus(MacStatus.Idle);
		//}

	}

}
