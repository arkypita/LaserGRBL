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
		int logcnt = 0;

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
			if (GrblCore.WriteComLog) log("com", string.Format("Open {0} {1}", mAddress, GetResetDiagnosticString()));

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
					if (GrblCore.WriteComLog) log("com", string.Format("Close {0} [{1}]", mAddress, auto ? "CORE" : "USER"));
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
				if (GrblCore.WriteComLog) log("tx", string.Format("[0x{0:X}]", b));
				cln.Send(new string((char)b, 1));
			}
		}

		public void Write(string text)
		{
			if (IsOpen)
			{
				if (GrblCore.WriteComLog) log("tx", text);
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

			if (GrblCore.WriteComLog) log("rx", rv);
			return rv;
		}

		public bool HasData()
		{ return IsOpen && buffer.Count > 0; }

		private void log(string operation, string line)
		{
			line = line?.Replace("\r", "\\r");
			line = line?.Replace("\n", "\\n");
			try { System.IO.File.AppendAllText(System.IO.Path.Combine(GrblCore.DataPath, string.Format("socketlog.txt", operation)), string.Format("{0:00000000}\t{1:00000}\t{2}\t{3}\r\n", Tools.TimingBase.TimeFromApplicationStartup().TotalMilliseconds, logcnt++, operation, line)); }
			catch { }
		}
	}
}
