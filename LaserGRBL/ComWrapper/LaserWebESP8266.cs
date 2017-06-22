using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LaserGRBL.ComWrapper
{
	class LaserWebESP8266 : IComWrapper
	{
		private string mAddress;
		private int mPort;

		private System.Net.Sockets.TcpClient cln;
		BinaryWriter bwriter;
		StreamReader sreader;
		StreamWriter swriter;

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

				cln = new System.Net.Sockets.TcpClient();
				Logger.LogMessage("OpenCom", "Open {0}:{1}", mAddress, mPort);
				cln.Connect(mAddress, mPort);

				Stream cst = cln.GetStream();
				bwriter = new BinaryWriter(cst);
				sreader = new StreamReader(cst, Encoding.ASCII);
				swriter = new StreamWriter(cst, Encoding.ASCII);
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
				}
				catch { }

				cln = null;
				bwriter = null;
				sreader = null;
				swriter = null;
			}
		}

		public bool IsOpen
		{
			get { return cln != null && cln.Connected; }
		}

		public void Write(byte b)
		{
			bwriter.Write(b);
			bwriter.Flush();
		}

		public void WriteLine(string text)
		{
			swriter.WriteLine(text);
			swriter.Flush();
		}

		public string ReadLine()
		{
			return sreader.ReadLine();
		}
	}
}
