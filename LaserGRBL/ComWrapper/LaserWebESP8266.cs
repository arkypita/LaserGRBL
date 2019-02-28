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

		private string mAddress;
		private WebSocket cln;
        ComLogger ComLog = new ComLogger("socketlog.txt");

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
            ComLog.Log("com", string.Format("Open {0} {1}", mAddress, GetResetDiagnosticString()));

			cln.OnMessage += cln_OnMessage;
			cln.Connect();
		}

		private string GetResetDiagnosticString()
		{
			//bool rts = (bool)Settings.GetObject("HardReset Grbl On Connect", false);
			//bool dtr = (bool)Settings.GetObject("HardReset Grbl On Connect", false);
			bool soft = (bool)Settings.GetObject("Reset Grbl On Connect", false);

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
                    ComLog.Log("com", string.Format("Close {0} [{1}]", mAddress, auto ? "CORE" : "USER"));
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
                ComLog.Log("tx", b);
				cln.Send(new string((char)b, 1));
			}
		}

        public void Write(byte[] arr)
        {
            if (IsOpen)
            {
                ComLog.Log("tx", arr);
                cln.Send(arr);
            }
        }

        public void Write(string text)
		{
			if (IsOpen)
			{
                ComLog.Log("tx", text);
				cln.Send(text);
			}
		}

		void cln_OnMessage(object sender, MessageEventArgs e)
		{
			foreach (string line in e.Data.Split(new string[] {"\n"}, StringSplitOptions.RemoveEmptyEntries))
				buffer.Enqueue(line); 
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

            ComLog.Log("rx", rv);
			return rv;
		}

		public bool HasData()
		{ return IsOpen && buffer.Count > 0; }

	}
}
