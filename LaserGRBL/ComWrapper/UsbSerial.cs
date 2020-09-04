﻿//Copyright (c) 2016-2020 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.IO.Ports;

namespace LaserGRBL.ComWrapper
{
	class UsbSerial : IComWrapper
	{
		private MySerial com = null;

		private string mPortName;
		private int mBaudRate;

        public void Configure(params object[] param)
		{
			mPortName = (string)param[0];
			mBaudRate = (int)param[1];
		}

		public void Open()
		{
			if (!IsOpen)
			{
				try { Close(true); }
				catch { }

				try
				{
					com = new MySerial();
					com.DataBits = 8;
					com.Parity = System.IO.Ports.Parity.None;
					com.StopBits = System.IO.Ports.StopBits.One;
					com.Handshake = System.IO.Ports.Handshake.None;
					com.PortName = mPortName;
					com.BaudRate = mBaudRate;
					com.NewLine = "\n";
					com.WriteTimeout = 1000; //se si blocca in write

					com.DtrEnable = Settings.GetObject("HardReset Grbl On Connect", false);
					com.RtsEnable = Settings.GetObject("HardReset Grbl On Connect", false);

					if (Settings.GetObject("Firmware Type", Firmware.Grbl) == Firmware.Marlin)
						com.DtrEnable = true;

					ComLogger.Log("com", string.Format("Open {0} @ {1} baud {2}", com.PortName.ToUpper(), com.BaudRate, GetResetDiagnosticString()));
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

                            ComLogger.Log("com", string.Format("Open {0} @ {1} baud {2}", com.PortName.ToUpper(), com.BaudRate, GetResetDiagnosticString()));
							Logger.LogMessage("OpenCom", "Retry opening {0} as {1} (issue #31)", mPortName.ToUpper(), com.PortName.ToUpper());

							com.Open();
							com.DiscardOutBuffer();
							com.DiscardInBuffer();
						}
						catch
						{
							com = null;
							throw ioex; //throw the original ex - not the new one!
						}
					}
					else
					{
						com = null;
					}
				}
			}
		}

		private string GetResetDiagnosticString()
		{
			bool rts = Settings.GetObject("HardReset Grbl On Connect", false);
			bool dtr = Settings.GetObject("HardReset Grbl On Connect", false);
			bool soft = Settings.GetObject("Reset Grbl On Connect", false);

			string rv = "";

			if (dtr) rv += "DTR, ";
			if (rts) rv += "RTS, ";
			if (soft) rv += "Ctrl-X, ";

			return rv.Trim(", ".ToCharArray());
		}

		public void Close(bool auto)
		{
			if (com != null && com.IsOpen)
			{
				ComLogger.Log("com", string.Format("Close {0} [{1}]", com.PortName.ToUpper(), auto ? "CORE" : "USER"));
				Logger.LogMessage("CloseCom", "Close {0} [{1}]", com.PortName.ToUpper(), auto ? "CORE" : "USER");
				try { com.DiscardOutBuffer(); } catch { }
				try { com.DiscardInBuffer(); } catch { }
				try { com.Close(); } catch { }
				try { com.Dispose(); } catch { }
			}
			com = null;
		}

		public bool IsOpen
		{ get { return com != null && com.IsOpen; } }

		public void Write(byte b)
		{
			if (com != null)
			{
				ComLogger.Log("tx", b);
				com.Write(new byte[] { b }, 0, 1);
			}
		}

        public void Write(byte[] arr)
        {
			if (com != null)
			{
				ComLogger.Log("tx", arr);
				com.Write(arr, 0, arr.Length);
			}
        }

        public void Write(string text)
		{
			if (com != null)
			{
				ComLogger.Log("tx", text);
				com.Write(text);
			}
		}

		public string ReadLineBlocking()  //la lettura della com è bloccante per natura
		{
			if (com != null)
			{
				if (ComLogger.Enabled)
				{
					string rv = com.ReadLine();
					ComLogger.Log("rx", rv);
					return rv;
				}
				else
				{
					return com.ReadLine();
				}
			}
			else
			{
				throw new System.IO.IOException("Cannot read from COM port, port is closed!");
			}
		}

		public bool HasData()
		{ return com != null && com.BytesToRead > 0; }

	}


    //se MySerial non dovesse bastare a calmare il problema riportato qui: https://github.com/arkypita/LaserGRBL/issues/990
    //provare i seguenti suggerimenti:
    //- Always call Close() followed by Dispose().
    //- Never reuse a SerialPort object, always create a new one when a port needs to be reopened
    internal class MySerial : SerialPort
	{
		protected override void Dispose(bool disposing)
		{
			try
			{
				Type mytype = typeof(System.IO.Ports.SerialPort);
				System.Reflection.FieldInfo field = mytype.GetField("internalSerialStream", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
				System.IO.Stream stream = field.GetValue(this) as System.IO.Stream;

				if (stream != null)
				{
					try { stream.Dispose(); }
					catch { }
				}
			}
			catch { }

			base.Dispose(disposing);
		}
	}
}
