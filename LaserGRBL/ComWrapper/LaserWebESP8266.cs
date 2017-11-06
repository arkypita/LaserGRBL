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
			cln.OnMessage += cln_OnMessage;
			cln.Connect();
		}

		public void Close(bool auto)
		{
			if (cln != null)
			{
				try
				{
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
		{if (IsOpen) cln.Send(new string((char)b,1));}

		public void Write(string text)
		{if (IsOpen) cln.Send(text);}

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

			return rv;
		}

		public bool HasData()
		{ return IsOpen && buffer.Count > 0; }
	}
}
