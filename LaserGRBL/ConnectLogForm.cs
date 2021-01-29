//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Drawing;
using System.Windows.Forms;

namespace LaserGRBL
{
	/// <summary>
	/// Description of ConnectLogForm.
	/// </summary>
	public partial class ConnectLogForm : System.Windows.Forms.UserControl
	{
		private object[] baudRates = { 4800, 9600, 19200, 38400, 57600, 115200, 230400, 460800, 921600 };
		public ComWrapper.WrapperType currentWrapper;

		GrblCore Core;
		private string mLoadedFileName;

		public ConnectLogForm()
		{
			currentWrapper = Settings.GetObject("ComWrapper Protocol", ComWrapper.WrapperType.UsbSerial);
			InitializeComponent();
		}

		public void SetCore(GrblCore core)
		{
			Core = core;
			Core.OnFileLoaded += OnFileLoaded;
			Core.OnLoopCountChange += OnLoopCountChanged;

			CmdLog.SetCom(core);
			
			PB.Bars.Add(new LaserGRBL.UserControls.DoubleProgressBar.Bar(Color.LightSkyBlue));
			PB.Bars.Add(new LaserGRBL.UserControls.DoubleProgressBar.Bar(Color.Pink));

			InitSpeedCB();
			InitPortCB();

			RestoreConf();

			TimerUpdate();
		}

		void OnLoopCountChanged(decimal current)
		{
			if (InvokeRequired)
			{
				Invoke(new GrblCore.dlgOnLoopCountChange(OnLoopCountChanged), current);
			}
			else
			{
				if (UDLoopCounter.Value != current)
					UDLoopCounter.Value = current;
			}
		}

		private void RestoreConf()
		{
			CBSpeed.SelectedItem = Settings.GetObject("Serial Speed", 115200);

			if (currentWrapper == ComWrapper.WrapperType.Telnet)
				TxtAddress.Text = Settings.GetObject("Telnet Address", "127.0.0.1:23");	
			else if (currentWrapper == ComWrapper.WrapperType.LaserWebESP8266)
				TxtAddress.Text = Settings.GetObject("Websocket URL", "ws://127.0.0.1:81/"); 
		}

		void OnFileLoaded(long elapsed, string filename)
		{
			if (InvokeRequired)
			{
				Invoke(new GrblFile.OnFileLoadedDlg(OnFileLoaded), elapsed, filename);
			}
			else
			{
				mLoadedFileName = filename;
				TbFileName.Text = System.IO.Path.GetFileName(filename);
			}
		}

		private void InitSpeedCB() //Baud Rates combo box
		{
			CBSpeed.BeginUpdate();
			CBSpeed.Items.AddRange(baudRates);
			CBSpeed.EndUpdate();
		}

		private void InitPortCB() //Availabe Ports combo box
		{
			string currentport = CBPort.SelectedItem as string;
			CBPort.BeginUpdate();
			CBPort.Items.Clear();

			foreach (string portname in System.IO.Ports.SerialPort.GetPortNames())
			{
				string purgename = portname;

				//FIX https://github.com/arkypita/LaserGRBL/issues/31

				if (!char.IsDigit(purgename[purgename.Length - 1]))
					purgename = purgename.Substring(0, purgename.Length - 1);

				CBPort.Items.Add(purgename);
			}

			if (currentport != null && CBPort.Items.Contains(currentport))
				CBPort.SelectedItem = currentport;
			else if (CBPort.Items.Count > 0)
				CBPort.SelectedIndex = CBPort.Items.Count -1;
			CBPort.EndUpdate();
		}

		//private static System.Text.RegularExpressions.Regex ComRX = new System.Text.RegularExpressions.Regex(@"(?'wholecom'(?:^|[ (])COM(?'comno'\d+)(?:[) ]|$))", System.Text.RegularExpressions.RegexOptions.Compiled);
		//private System.Collections.Generic.SortedDictionary<int, string> GetPortDictionary()
		//{
		//	System.Collections.Generic.SortedDictionary<int, string> rv = new System.Collections.Generic.SortedDictionary<int, string>();
		//
		//	try //add using managment object
		//	{
		//		using (System.Management.ManagementObjectSearcher searcher = new System.Management.ManagementObjectSearcher(@"\\.\root\cimv2", "SELECT * FROM Win32_PnPEntity"))
		//		{
		//			System.Management.ManagementObjectCollection moc = searcher.Get();
		//			foreach (System.Management.ManagementObject mo in moc)
		//			{
		//				string caption = (string)mo["Caption"];
		//				if (caption != null && ComRX.IsMatch(caption))
		//				{
		//					System.Text.RegularExpressions.Match m = ComRX.Match(caption);
		//					if (m != null && m.Groups["comno"] != null)
		//					{
		//						int no = int.Parse(m.Groups["comno"].Value);
		//						string wholecom = m.Groups["wholecom"].Value;
		//						if (!rv.ContainsKey(no))
		//							rv.Add(int.Parse(m.Groups["comno"].Value), caption.Replace(wholecom, "").Trim());
		//					}
		//				}
		//			}
		//
		//		}
		//	}
		//	catch { }
		//
		//	try //add using SerialPort.GetPortNames 
		//	{
		//		foreach (string dirty in System.IO.Ports.SerialPort.GetPortNames())
		//		{
		//			string comno = dirty;
		//			if (!char.IsDigit(comno[comno.Length - 1]))
		//				comno = comno.Substring(0, comno.Length - 1);
		//
		//			string caption = comno;
		//			if (caption != null && ComRX.IsMatch(caption))
		//			{
		//				System.Text.RegularExpressions.Match m = ComRX.Match(caption);
		//				if (m != null && m.Groups["comno"] != null)
		//				{
		//					int no = int.Parse(m.Groups["comno"].Value);
		//					if (!rv.ContainsKey(no))
		//						rv.Add(int.Parse(m.Groups["comno"].Value), "Generic COM Port");
		//				}
		//			}
		//		}
		//	}
		//	catch { }
		//
		//	return rv;
		//}


		void BtnConnectDisconnectClick(object sender, EventArgs e)
		{
			if (Core.MachineStatus == GrblCore.MacStatus.Disconnected)
				Core.OpenCom();
			else if (!(Core.InProgram && System.Windows.Forms.MessageBox.Show(Strings.DisconnectAnyway, Strings.WarnMessageBoxHeader, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes))
				Core.CloseCom(true);

			TimerUpdate();
		}


		void ApplyConfig()
		{
			if ((currentWrapper == ComWrapper.WrapperType.UsbSerial || currentWrapper == ComWrapper.WrapperType.UsbSerial2) && CBPort.Text != null && CBSpeed.SelectedItem != null)
				Core.Configure(currentWrapper, CBPort.Text, (int)CBSpeed.SelectedItem);
			else if (currentWrapper == ComWrapper.WrapperType.Telnet || currentWrapper == ComWrapper.WrapperType.LaserWebESP8266)
				Core.Configure(currentWrapper, (string)TxtAddress.Text);
			else if (currentWrapper == ComWrapper.WrapperType.Emulator)
				Core.Configure(currentWrapper);
		}

		void BtnOpenClick(object sender, EventArgs e)
		{
			Core.OpenFile(ParentForm);
		}

		void BtnRunProgramClick(object sender, EventArgs e)
		{
			Core.RunProgram(ParentForm);
		}
		void TxtManualCommandCommandEntered(string command)
		{
			Core.EnqueueCommand(new GrblCommand(command, 0, true));
		}
		
		public void TimerUpdate()
		{
			SuspendLayout();

			if (!Core.IsConnected && System.IO.Ports.SerialPort.GetPortNames().Length != CBPort.Items.Count)
				InitPortCB();
			
			PB.Maximum = Core.ProgramTarget;
			PB.Bars[0].Value = Core.ProgramSent;
			PB.Bars[1].Value = Core.ProgramExecuted;

			string val = Tools.Utils.TimeSpanToString(Core.ProgramTime, Tools.Utils.TimePrecision.Minute, Tools.Utils.TimePrecision.Second, " ,", true);

			if (val != "now")
				PB.PercString = val;
			else if (Core.InProgram)
				PB.PercString = "0 sec";
			else
				PB.PercString = "";
			
			PB.Invalidate();



			/*
			Idle: All systems are go, no motions queued, and it's ready for anything.
			Run: Indicates a cycle is running.
			Hold: A feed hold is in process of executing, or slowing down to a stop. After the hold is complete, Grbl will remain in Hold and wait for a cycle start to resume the program.
			Door: (New in v0.9i) This compile-option causes Grbl to feed hold, shut-down the spindle and coolant, and wait until the door switch has been closed and the user has issued a cycle start. Useful for OEM that need safety doors.
			Home: In the middle of a homing cycle. NOTE: Positions are not updated live during the homing cycle, but they'll be set to the home position once done.
			Alarm: This indicates something has gone wrong or Grbl doesn't know its position. This state locks out all G-code commands, but allows you to interact with Grbl's settings if you need to. '$X' kill alarm lock releases this state and puts Grbl in the Idle state, which will let you move things again. As said before, be cautious of what you are doing after an alarm.
			Check: Grbl is in check G-code mode. It will process and respond to all G-code commands, but not motion or turn on anything. Once toggled off with another '$C' command, Grbl will reset itself.
			*/

			TT.SetToolTip(BtnConnectDisconnect, Core.IsConnected ? Strings.BtnDisconnectTT : Strings.BtnConnectTT);
			
			BtnConnectDisconnect.UseAltImage = Core.IsConnected;
			BtnRunProgram.Enabled = Core.CanSendFile;
            BtnRunProgram.Visible = !Core.CanAbortProgram;
            BtnAbortProgram.Visible = Core.CanAbortProgram;
            BtnOpen.Enabled = Core.CanLoadNewFile;

			bool old = TxtManualCommand.Enabled;
			TxtManualCommand.Enabled = Core.CanSendManualCommand;
			//if (old == false && TxtManualCommand.Enabled == true)
			//	TxtManualCommand.Focus();

			//CBProtocol.Enabled = !Core.IsOpen;
			CBPort.Enabled = !Core.IsConnected;
			CBSpeed.Enabled = !Core.IsConnected;
			TxtAddress.Enabled = !Core.IsConnected;

			CmdLog.TimerUpdate();

			if (!Core.IsConnected)
			{
				ComWrapper.WrapperType actualWrapper = Settings.GetObject("ComWrapper Protocol", ComWrapper.WrapperType.UsbSerial);
				if (actualWrapper != currentWrapper)
				{
					currentWrapper = actualWrapper;
					UpdateConf();
				}
			}

			ResumeLayout();
		}

		private void CBPort_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateConf();
		}

		private void CBPort_TextChanged(object sender, EventArgs e)
		{
			UpdateConf();
		}

		private void CBSpeed_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateConf();
		}

		
		private void UpdateConf()
		{
			tableLayoutPanel4.SuspendLayout();
			CBPort.Visible = CBSpeed.Visible = LblComPort.Visible = LblBaudRate.Visible = (currentWrapper == ComWrapper.WrapperType.UsbSerial || currentWrapper == ComWrapper.WrapperType.UsbSerial2);
			TxtAddress.Visible = LblAddress.Visible = (currentWrapper == ComWrapper.WrapperType.Telnet || currentWrapper == ComWrapper.WrapperType.LaserWebESP8266);
			LblAddress.Text = (currentWrapper == ComWrapper.WrapperType.Telnet ? "IP:PORT" : "Socket URL");
			TxtEmulator.Visible = LblEmulator.Visible = (currentWrapper == ComWrapper.WrapperType.Emulator);
			tableLayoutPanel4.ResumeLayout();

			if (CBSpeed.SelectedItem != null)
				Settings.SetObject("Serial Speed", CBSpeed.SelectedItem);

			if (TxtAddress.Text != "")
			{
				if (currentWrapper == ComWrapper.WrapperType.Telnet)
					Settings.SetObject("Telnet Address", TxtAddress.Text);
				else if (currentWrapper == ComWrapper.WrapperType.LaserWebESP8266)
					Settings.SetObject("Websocket URL", TxtAddress.Text);
			}

			ApplyConfig();

			Settings.Save();
		}

		private void CBProtocol_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateConf();
		}

		private void TxtHostName_TextChanged(object sender, EventArgs e)
		{
			UpdateConf();
		}

		private void ITcpPort_CurrentValueChanged(object sender, int NewValue, bool ByUser)
		{
			UpdateConf();
		}

		private void UDLoopCounter_ValueChanged(object sender, EventArgs e)
		{
			Core.LoopCount = UDLoopCounter.Value;
		}

		internal void OnColorChange()
		{
			CmdLog.Invalidate();
		}

		private void TxtManualCommand_Enter(object sender, EventArgs e)
		{
			Core.SuspendHK = true;
		}

		private void TxtManualCommand_Leave(object sender, EventArgs e)
		{
			Core.SuspendHK = false;
		}

		private void TbFileName_MouseEnter(object sender, EventArgs e)
		{
			if (mLoadedFileName != null)
				TT.Show(mLoadedFileName, TbFileName, 5000);
		}

		private void TbFileName_MouseLeave(object sender, EventArgs e)
		{
			TT.Hide(TbFileName);
		}

        private void BtnAbortProgram_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Strings.BoxAbortProgramConfirm, Strings.WarnMessageBoxHeader, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                Core.AbortProgram();
        }

		internal void ConfigFromDiscovery(string config)
		{
			if (TxtAddress.Visible && TxtAddress.Enabled && config != null)
			{
				TxtAddress.Text = config;
				Application.DoEvents();

				if (BtnConnectDisconnect.Enabled && Core.MachineStatus == GrblCore.MacStatus.Disconnected)
					BtnConnectDisconnectClick(null, null);
			}
		}
	}
}
