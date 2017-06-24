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
		private int mPort;
		private WebSocket cln;

		private Queue<string> buffer = new Queue<string>();

		public void Configure(params object[] param)
		{
			mAddress = (string)param[0];
			mPort = (int)param[1];
		}

		public void Open()
		{
			if (cln == null)
			{
				if (string.IsNullOrEmpty(mAddress))
					throw new MissingFieldException("Missing HostName");
				else if (mPort == 0)
					throw new MissingFieldException("Missing Port");

				buffer.Clear();
				cln = new WebSocketSharp.WebSocket(string.Format("ws://{0}:{1}", mAddress, mPort));
				Logger.LogMessage("OpenCom", "Open {0}:{1}", mAddress, mPort);
				cln.OnMessage += cln_OnMessage;
				cln.Connect();

			}
			else
			{
				throw new InvalidOperationException("Port already opened");
			}
		}

		public void Close(bool auto)
		{
			if (cln != null)
			{
				try
				{
					Logger.LogMessage("CloseCom", "Close {0} [{1}]", mAddress, auto ? "CORE" : "USER");
					cln.Close();
					cln.OnMessage -= cln_OnMessage;
					buffer.Clear();
				}
				catch { }

				cln = null;
			}
		}

		public bool IsOpen
		{
			get { return cln != null && cln.IsConnected; }
		}

		public void Write(byte b)
		{
			if (IsOpen)
				cln.Send(new string((char)b,1));
		}

		public void WriteLine(string text)
		{
			if (IsOpen)
				cln.Send(text + "\r\n");
		}

		void cln_OnMessage(object sender, MessageEventArgs e)
		{
			foreach (string line in e.Data.Split(new string[] {"\n", "\r\n"}, StringSplitOptions.RemoveEmptyEntries))
				buffer.Enqueue(line); 
		}

		public string ReadLine()
		{
			string rv = null;
			while (IsOpen && rv == null) //wait for disconnect or data
			{
				if (buffer.Count > 0)
					rv = buffer.Dequeue();
				else
					System.Threading.Thread.Sleep(10);
			}

			return rv;
		}
	}
}
