using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace LaserGRBL.ComWrapper
{
	class Emulator : IComWrapper
	{
		private LaserGRBL.GrblEmulator.Grblv11Emulator emu;
		private bool opened;
		private bool closing;
		private Queue<string> buffer = new Queue<string>();

		public Emulator()
		{
			emu = new GrblEmulator.Grblv11Emulator(SendData);
		}

		private void SendData(string message)
		{
			foreach (string line in message.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries))
				buffer.Enqueue(line); 
		}

		public void Configure(params object[] param)
		{
			
		}

		public void Open()
		{
			if (!opened)
			{
				GrblEmulator.EmulatorUI.ShowUI("Grbl Emulator v1.1#");

				closing = false;
				emu.OpenCom();
				buffer.Clear();
				opened = true;
			}


		}

		public void Close(bool auto)
		{
			if (opened)
			{
				closing = true;
				emu.CloseCom();
				buffer.Clear();
				opened = false;
			}
		}

		public bool IsOpen
		{
			get { return opened; }
		}

		public void Write(byte b)
		{
			emu.ManageMessage(new byte[] { b });
		}

		public void Write(string text)
		{
			emu.ManageMessage(Encoding.UTF8.GetBytes(text));
		}

		public string ReadLineBlocking()
		{
			string rv = null;
			while (IsOpen && rv == null) //wait for disconnect or data
			{
				if (closing)
					throw new System.IO.EndOfStreamException("Connection closed");
				if (buffer.Count > 0)
					rv = buffer.Dequeue();
				else
					System.Threading.Thread.Sleep(10);
			}

			return rv;
		}

		public bool HasData()
		{ return IsOpen && buffer.Count > 0; }
	}

}
