/*
 * Created by SharpDevelop.
 * User: Diego
 * Date: 05/12/2016
 * Time: 23:41
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace LaserGRBL
{
	/// <summary>
	/// Description of ConnectLogForm.
	/// </summary>
	public partial class ConnectLogForm : LaserGRBL.UserControls.DockingManager.DockContent
	{
		private object[] baudRates = { 4800, 9600, 19200, 38400, 57600, 115200, 230400 };
		
		GrblCore Core;
		
		public ConnectLogForm(GrblCore core)
		{
			InitializeComponent();
			Core = core;
			Core.OnFileLoaded += ComPort_OnFileLoaded;
			CmdLog.SetCom(core);
			
			PB.Bars.Add(new LaserGRBL.UserControls.DoubleProgressBar.Bar(Color.LightSkyBlue));
			PB.Bars.Add(new LaserGRBL.UserControls.DoubleProgressBar.Bar(Color.Pink));
			
			InitSpeedCB();
			InitPortCB();
			TimerUpdate();
		}

		void ComPort_OnFileLoaded(long elapsed, string filename)
		{
			TbFileName.Text = filename;
		}

		private void InitSpeedCB() //Baud Rates combo box
		{
			CBSpeed.BeginUpdate();
			CBSpeed.Items.AddRange(baudRates);
			CBSpeed.SelectedItem = 115200;
			CBSpeed.EndUpdate();
		}

		private void InitPortCB() //Availabe Ports combo box
		{
			string currentport = CBPort.SelectedItem as string;
			CBPort.BeginUpdate();
			CBPort.Items.Clear();
			CBPort.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
			if (currentport != null && CBPort.Items.Contains(currentport))
				CBPort.SelectedItem = currentport;
			else if (CBPort.Items.Count > 0)
				CBPort.SelectedIndex = CBPort.Items.Count -1;
			CBPort.EndUpdate();
		}
		
		void BtnConnectDisconnectClick(object sender, EventArgs e)
		{
			if (Core.MachineStatus == GrblCore.MacStatus.Disconnected && CBSpeed.SelectedItem != null && CBPort.SelectedItem != null)
			{
				try{Core.OpenCom();}
				catch { }
			}
			else
			{
				try{Core.CloseCom();}
				catch { }
			}

			TimerUpdate();
		}
		
		void BtnOpenClick(object sender, EventArgs e)
		{
			Core.OpenFile();
		}

		void BtnRunProgramClick(object sender, EventArgs e)
		{
			Core.EnqueueProgram();
		}
		void TxtManualCommandCommandEntered(string command)
		{
			Core.EnqueueCommand(new GrblCommand(command));
		}
		
		public void TimerUpdate()
		{
			SuspendLayout();

			if (!Core.IsOpen && System.IO.Ports.SerialPort.GetPortNames().Length != CBPort.Items.Count)
				InitPortCB();
			
			PB.Maximum = Core.ProgramTarget;
			PB.Bars[0].Value = Core.ProgramSent;
			PB.Bars[1].Value = Core.ProgramExecuted;

			string val = Tools.Utils.TimeSpanToString(Core.ProgramTime, Tools.Utils.TimePrecision.Minute, Tools.Utils.TimePrecision.Second);

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

			BtnConnectDisconnect.UseAltImage = Core.IsOpen;
			BtnRunProgram.Enabled = Core.CanSendFile;
			BtnOpen.Enabled = Core.CanLoadNewFile;

			bool old = TxtManualCommand.Enabled;
			TxtManualCommand.Enabled = Core.CanSendManualCommand;
			if (old == false && TxtManualCommand.Enabled == true)
				TxtManualCommand.Focus();
			
			CBPort.Enabled = !Core.IsOpen;
			CBSpeed.Enabled = !Core.IsOpen;

			CmdLog.TimerUpdate();

			ResumeLayout();
		}

		private void CBPort_SelectedIndexChanged(object sender, EventArgs e)
		{Core.PortName = (string)CBPort.SelectedItem;}

		private void CBSpeed_SelectedIndexChanged(object sender, EventArgs e)
		{Core.BaudRate = (int)CBSpeed.SelectedItem;}
	}
}
