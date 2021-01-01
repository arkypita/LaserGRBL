//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

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
			System.Diagnostics.Debug.WriteLine(rline);
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
				rline = rline.Substring(1, rline.Length - 2);
				string[] arr = rline.Split("|".ToCharArray());

				for (int i = 0; i < arr.Length; i++)
				{
					if (arr[i].StartsWith("VSta:"))
						ParseVSta(arr[i]);
					else if (arr[i].StartsWith("SBuf:"))
						ParseSBuf(arr[i]);
					//else if (arr[i].StartsWith("LTC:"))
					//	;// ParseLTC(arr[i]);
				}
			}
			catch (Exception ex)
			{
				Logger.LogMessage("VigoStatus", "Ex on [{0}] message", rline);
				Logger.LogException("VigoStatus", ex);
			}
		}

		int mOldSta = 0;
		private void ParseVSta(string p)
		{
			string data = p.Substring(5, p.Length - 5);
			int mCurSta = (int)ParseFloat(data);

			if (mOldSta == 2 && mCurSta == 0)
				injob = false;

			mOldSta = mCurSta;
		}

		private bool injob = false;
		private int mOldReceived = 0;
		private int mOldManaged = 0;
		private int mOldError = 0;
		private void ParseSBuf(string p)
		{
			string data = p.Substring(5, p.Length - 5);
			string[] xyz = data.Split(",".ToCharArray());
			int mCurReceived = (int)ParseFloat(xyz[0]);
			int mCurManaged = (int)ParseFloat(xyz[1]);
			int mCurError = (int)ParseFloat(xyz[2]);

			if (mOldReceived > mCurReceived)
				mOldReceived = mOldManaged = mOldError = 0; //emergency reset

			for (int i = mOldManaged; i < mCurManaged; i++)
				ManageCommandResponse("ok");
			for (int i = mOldError; i < mCurError; i++)
				ManageCommandResponse("error:99");

			mOldReceived = mCurReceived;
			mOldManaged = mCurManaged;
			mOldError = mCurError; 
		}

		protected override void OnJobBegin()
		{
			injob = true;
			EnqueueCommand(new GrblCommand(">Ofk9gsd8IKjKBahP0OGS9BrhZCPeWCBALCbyGf", 0, true));
			base.OnJobBegin();
		}

		protected override void OnJobEnd()
		{
			base.OnJobEnd();
			EnqueueCommand(new GrblCommand("G0X0Y0M5", 0, true));
			EnqueueCommand(new GrblCommand(">NPredMjdFaeaJajf6OHy:hkRUygcpBXwtV", 0, true));

			//injob = false; //tanto sembra che a questo messaggio che segue non risponda e invece si resetta!
		}

		protected override void SendToSerial(GrblCommand tosend)
		{
			if (injob)
			{
				mUsedBuffer += tosend.SerialData.Length;
				com.Write(tosend.SerialData); //invio dei dati alla linea di comunicazione
			}
			else
			{
				//mUsedBuffer += tosend.SerialData.Length;
				com.Write(tosend.SerialData); //invio dei dati alla linea di comunicazione
				tosend.SetResult("ok", false);
			}
		}

	}

}
