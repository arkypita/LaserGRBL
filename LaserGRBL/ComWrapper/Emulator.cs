//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

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
                ComLogger.Log("com", "Open Emulator");
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
                ComLogger.Log("com", String.Format("Close Emulator [{0}]", auto ? "CORE" : "USER"));
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
            ComLogger.Log("tx", b);
            emu.ManageMessage(new byte[] { b });
		}

        public void Write(byte[] arr)
        {
            ComLogger.Log("tx", arr);
            emu.ManageMessage(arr);
        }

        public void Write(string text)
		{
            ComLogger.Log("tx", text);
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

            if (rv != null)
                ComLogger.Log("rx", rv);
            return rv;
		}

		public bool HasData()
		{ return IsOpen && buffer.Count > 0; }
	}

}
