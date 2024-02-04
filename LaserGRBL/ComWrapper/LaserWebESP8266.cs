//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using WebSocketSharp;

namespace LaserGRBL.ComWrapper
{
	class LaserWebESP8266 : IComWrapper
	{
		private string mRemainder;
		private string mAddress;
		private WebSocket cln;
		private Queue<string> buffer = new Queue<string>();

		public void Configure(params object[] param)
		{
			mAddress = (string)param[0];
		}

		public void Open()
		{
			if (cln != null)
				Close(true);


			if (string.IsNullOrEmpty(mAddress))
				throw new MissingFieldException("Missing Address");

			buffer.Clear();
			cln = new WebSocketSharp.WebSocket(mAddress);

			Logger.LogMessage("OpenCom", "Open {0}", mAddress);
            ComLogger.Log("com", string.Format("Open {0} {1}", mAddress, GetResetDiagnosticString()));

			cln.OnMessage += cln_OnMessage;
			cln.Connect();
		}

		private string GetResetDiagnosticString()
		{
			//bool rts = Settings.GetObject("HardReset Grbl On Connect", false);
			//bool dtr = Settings.GetObject("HardReset Grbl On Connect", false);
			bool soft = Settings.GetObject("Reset Grbl On Connect", false);

			string rv = "";

			//if (dtr) rv += "DTR, ";
			//if (rts) rv += "RTS, ";
			if (soft) rv += "Ctrl-X, ";

			return rv.Trim(", ".ToCharArray());
		}

		public void Close(bool auto)
		{
			if (cln != null)
			{
				try
				{
					ComLogger.Log("com", string.Format("Close {0} [{1}]", mAddress, auto ? "CORE" : "USER"));
					Logger.LogMessage("CloseCom", "Close {0} [{1}]", mAddress, auto ? "CORE" : "USER");
					cln.OnMessage -= cln_OnMessage;

					cln.Close();
				}
				catch { }

				buffer.Clear();
				cln = null;
			}
		}

		public bool IsOpen
		{get { return cln != null && cln.IsConnected; }}

		public void Write(byte b)
		{
			if (IsOpen)
			{
				ComLogger.Log("tx", b);
				cln.Send(new string((char)b, 1));
			}
		}

        public void Write(byte[] arr)
        {
            if (IsOpen)
            {
                ComLogger.Log("tx", arr);
                cln.Send(arr);
            }
        }

        public void Write(string text)
		{
			if (IsOpen)
			{
                ComLogger.Log("tx", text);
				cln.Send(text);
			}
		}

		void cln_OnMessage(object sender, MessageEventArgs e)
		{
			var data = e.Data;
			if (data == null)
				data = Encoding.Default.GetString(e.RawData);
			var lastItemComplete = data.EndsWith("\n") || data.EndsWith("\r");

			var items = data.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < items.Length; i++)
			{
				var item = items[i];
				if(i == items.Length-1 && !lastItemComplete)
				{
					mRemainder = item;
					break;
				}
				if (mRemainder != null)
				{
					item = mRemainder + item;
					mRemainder = null;
				}
				buffer.Enqueue(item);
			} 
		}

		public string ReadLineBlocking()
		{
			string rv = null;
			while (IsOpen && rv == null) //wait for disconnect or data
			{
				if (buffer.Count > 0)
					rv = buffer.Dequeue();
				else
					System.Threading.Thread.Sleep(1);
			}

            ComLogger.Log("rx", rv);
			return rv;
		}

		public bool HasData()
		{ return IsOpen && buffer.Count > 0; }

	}
}
