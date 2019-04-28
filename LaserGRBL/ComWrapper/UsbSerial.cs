using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaserGRBL.ComWrapper
{
	class UsbSerial : IComWrapper
	{
		private System.IO.Ports.SerialPort com = new System.IO.Ports.SerialPort();
		private string mPortName;
		private int mBaudRate;
        ComLogger ComLog = new ComLogger("comlog.txt");

        public void Configure(params object[] param)
		{
			mPortName = (string)param[0];
			mBaudRate = (int)param[1];
		}

		public void Open()
		{
			if (!com.IsOpen)
			{
				try
				{
					com.DataBits = 8;
					com.Parity = System.IO.Ports.Parity.None;
					com.StopBits = System.IO.Ports.StopBits.One;
					com.Handshake = System.IO.Ports.Handshake.None;
					com.PortName = mPortName;
					com.BaudRate = mBaudRate;
					com.NewLine = "\n";
					com.WriteTimeout = 1000; //se si blocca in write

					com.DtrEnable = (bool)Settings.GetObject("HardReset Grbl On Connect", false);
					com.RtsEnable = (bool)Settings.GetObject("HardReset Grbl On Connect", false);

					ComLog.Log("com", string.Format("Open {0} @ {1} baud {2}", com.PortName.ToUpper(), com.BaudRate, GetResetDiagnosticString()));
					Logger.LogMessage("OpenCom", "Open {0} @ {1} baud {2}", com.PortName.ToUpper(), com.BaudRate, GetResetDiagnosticString());

					com.Open();
					com.DiscardOutBuffer();
					com.DiscardInBuffer();
				}
				catch (System.IO.IOException ioex)
				{
					if (char.IsDigit(mPortName[mPortName.Length - 1]) && char.IsDigit(mPortName[mPortName.Length - 2])) //two digits port like COM23
					{
						//FIX https://github.com/arkypita/LaserGRBL/issues/31

						try
						{ 
							com.PortName = mPortName.Substring(0, mPortName.Length - 1); //remove last digit and try again

                            ComLog.Log("com", string.Format("Open {0} @ {1} baud {2}", com.PortName.ToUpper(), com.BaudRate, GetResetDiagnosticString()));
							Logger.LogMessage("OpenCom", "Retry opening {0} as {1} (issue #31)", mPortName.ToUpper(), com.PortName.ToUpper());

							com.Open();
							com.DiscardOutBuffer();
							com.DiscardInBuffer();
						}
						catch
						{
							throw ioex; //throw the original ex - not the new one!
						}
					}
					else
					{
						
					}
				}
			}
		}

		private string GetResetDiagnosticString()
		{
			bool rts = (bool)Settings.GetObject("HardReset Grbl On Connect", false);
			bool dtr = (bool)Settings.GetObject("HardReset Grbl On Connect", false);
			bool soft = (bool)Settings.GetObject("Reset Grbl On Connect", false);

			string rv = "";

			if (dtr) rv += "DTR, ";
			if (rts) rv += "RTS, ";
			if (soft) rv += "Ctrl-X, ";

			return rv.Trim(", ".ToCharArray());
		}

		public void Close(bool auto)
		{
			if (com.IsOpen)
			{
                ComLog.Log("com", string.Format("Close {0} [{1}]", com.PortName.ToUpper(), auto ? "CORE" : "USER"));
				Logger.LogMessage("CloseCom", "Close {0} [{1}]", com.PortName.ToUpper(), auto ? "CORE" : "USER");
                try { com.DiscardOutBuffer(); } catch { }
                try { com.DiscardInBuffer(); } catch { }
                try { com.Close(); } catch { }
			}
		}

		public bool IsOpen
		{ get { return com.IsOpen; } }

		public void Write(byte b)
		{
            ComLog.Log("tx", b);
			com.Write(new byte[] { b }, 0, 1);
		}

        public void Write(byte[] arr)
        {
            ComLog.Log("tx", arr);
            com.Write(arr, 0, arr.Length);
        }

        public void Write(string text)
		{
            ComLog.Log("tx", text);
			com.Write(text);
		}

		public string ReadLineBlocking()
		{
			if (GrblCore.WriteComLog)
			{
				string rv = com.ReadLine();
                ComLog.Log("rx", rv);
				return rv;
			}

			else
			{
				return com.ReadLine();
			}


		} //la lettura della com è bloccante per natura

		public bool HasData()
		{ return com.BytesToRead > 0; }

	}
}
