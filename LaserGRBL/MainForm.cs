using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaserGRBL
{
	public partial class MainForm : Form
	{
		private object[] baudRates = { 4800, 9600, 19200, 38400, 57600, 115200, 230400 };
		private GrblFile mLoadedFile;
		private GrblCom mCom;
		
		public MainForm()
		{
			InitializeComponent();

			//build main communication object
			mCom = new GrblCom();
			mCom.MachineStatusChanged += OnMachineStatus;
			
			//assign to command log and 2D preview panel
			CmdLog.SetCom(mCom);
			Preview.SetCom(mCom);
			
			//File sending progress bar
			PB.Bars.Add(new LaserGRBL.UserControls.DoubleProgressBar.Bar(Color.LightSkyBlue));
			PB.Bars.Add(new LaserGRBL.UserControls.DoubleProgressBar.Bar(Color.Pink));
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			//initialize combo box
			InitSpeedCB();
			InitPortCB();
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

		
		private void BtnConnectDisconnect_Click(object sender, EventArgs e)
		{
			if (mCom.MachineStatus == GrblCom.MacStatus.Disconnected && CBSpeed.SelectedItem != null && CBPort.SelectedItem != null)
			{
				try{mCom.OpenCom((string)CBPort.SelectedItem, (int)CBSpeed.SelectedItem);}
				catch { }
			}
			else
			{
				try{mCom.CloseCom();}
				catch { }
			}

			TimerUpdate();
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			mCom.CloseCom();
		}

		private void BtnOpen_Click(object sender, EventArgs e)
		{
			string filename = null;
			using (System.Windows.Forms.OpenFileDialog ofd = new OpenFileDialog())
			{
				ofd.Filter = "GCODE Files|*.nc;*.gcode";
				ofd.CheckFileExists = true;
				ofd.Multiselect = false;
				ofd.RestoreDirectory = true;
				if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
					filename = ofd.FileName;
			}

			if (filename != null)
			{
				Cursor = Cursors.WaitCursor;
				long start = Tools.HiResTimer.TotalMilliseconds;
				mLoadedFile = new GrblFile(filename);
				Preview.Program = mLoadedFile;
				long elapsed = Tools.HiResTimer.TotalMilliseconds - start;

				RefreshButtonEnabled();
				TbFileName.Text = filename;
				TTTFile.Text = filename;
				TTTLines.Text = mLoadedFile.Count.ToString();
				TTTLoadedIn.Text = elapsed.ToString() + " ms";
				TTTEstimated.Text = Tools.Utils.TimeSpanToString(mLoadedFile.EstimatedTime, Tools.Utils.TimePrecision.Second, Tools.Utils.TimePrecision.Millisecond);
				Cursor = Cursors.Default;
			}
		}


		void OnMachineStatus(GrblCom.MacStatus status)
		{
			if (InvokeRequired)
				BeginInvoke(new GrblCom.dlgOnMachineStatus(OnMachineStatus), status);
			else
				RefreshButtonEnabled();
		}
		void BtnRunProgramClick(object sender, EventArgs e)
		{
			mCom.EnqueueProgram(mLoadedFile);
		}
		
		void RefreshButtonEnabled()
		{
			/*
			Idle: All systems are go, no motions queued, and it's ready for anything.
			Run: Indicates a cycle is running.
			Hold: A feed hold is in process of executing, or slowing down to a stop. After the hold is complete, Grbl will remain in Hold and wait for a cycle start to resume the program.
			Door: (New in v0.9i) This compile-option causes Grbl to feed hold, shut-down the spindle and coolant, and wait until the door switch has been closed and the user has issued a cycle start. Useful for OEM that need safety doors.
			Home: In the middle of a homing cycle. NOTE: Positions are not updated live during the homing cycle, but they'll be set to the home position once done.
			Alarm: This indicates something has gone wrong or Grbl doesn't know its position. This state locks out all G-code commands, but allows you to interact with Grbl's settings if you need to. '$X' kill alarm lock releases this state and puts Grbl in the Idle state, which will let you move things again. As said before, be cautious of what you are doing after an alarm.
			Check: Grbl is in check G-code mode. It will process and respond to all G-code commands, but not motion or turn on anything. Once toggled off with another '$C' command, Grbl will reset itself.
			*/

			BtnConnectDisconnect.UseAltImage = mCom.IsOpen;
			MnFileOpen.Enabled = BtnOpen.Enabled = true;
			MnFileSend.Enabled = BtnRunProgram.Enabled = mLoadedFile != null && mLoadedFile.Count > 0 && mCom.IsOpen && mCom.MachineStatus == GrblCom.MacStatus.Idle;
			MnExportConfig.Enabled = MnImportConfig.Enabled = mCom.IsOpen && mCom.MachineStatus == GrblCom.MacStatus.Idle;

			bool old = TxtManualCommand.Enabled;
			TxtManualCommand.Enabled = mCom.IsOpen && mCom.MachineStatus != GrblCom.MacStatus.Disconnected;
			if (old == false && TxtManualCommand.Enabled == true)
				TxtManualCommand.Focus();
			
			CBPort.Enabled = !mCom.IsOpen;
			CBSpeed.Enabled = !mCom.IsOpen;
			
			MnGrblReset.Enabled = BtnReset.Enabled = mCom.IsOpen && mCom.MachineStatus != GrblCom.MacStatus.Disconnected;
			BtnGoHome.Enabled = mCom.IsOpen && (mCom.MachineStatus == GrblCom.MacStatus.Idle || mCom.MachineStatus == GrblCom.MacStatus.Alarm);
			BtnStop.Enabled = mCom.IsOpen && mCom.MachineStatus == GrblCom.MacStatus.Run;
			BtnResume.Enabled = mCom.IsOpen && (mCom.MachineStatus == GrblCom.MacStatus.Door || mCom.MachineStatus == GrblCom.MacStatus.Hold);
		}

		private void BtnGoHomeClick(object sender, EventArgs e)
		{
			mCom.EnqueueCommand(new GrblCommand("$H"));
		}

		private void BtnResetClick(object sender, EventArgs e)
		{
			mCom.GrblReset();
		}
		void BtnStopClick(object sender, EventArgs e)
		{
			mCom.SafetyDoor();
		}
		void BtnResumeClick(object sender, EventArgs e)
		{
			mCom.CycleStartResume();
		}

		//private bool mOldCom = false;
		private void UpdateTimer_Tick(object sender, EventArgs e)
		{
			CmdLog.TimerUpdate();
			Preview.TimerUpdate();
			TimerUpdate();
			
//			if (mOldCom != mCom.IsOpen) {
//				mOldCom = mCom.IsOpen;
//				
//				if (!mOldCom)
//					mCom.OnComClosing();
//			}
		}

		private void TimerUpdate()
		{
			if (!mCom.IsOpen && System.IO.Ports.SerialPort.GetPortNames().Length != CBPort.Items.Count)
				InitPortCB();
			
			TTTStatus.Text = mCom.MachineStatus.ToString();
			PB.Maximum = mCom.ProgramTarget;
			PB.Bars[0].Value = mCom.ProgramSent;
			PB.Bars[1].Value = mCom.ProgramExecuted;

			string val = Tools.Utils.TimeSpanToString(mCom.ProgramTime, Tools.Utils.TimePrecision.Second, Tools.Utils.TimePrecision.Second);

			if (val != "now")
				PB.PercString = val;
			else if (mCom.InProgram)
				PB.PercString = "0 sec";
			else
				PB.PercString = "";
			
			if (mCom.InProgram)
				TTTEstimated.Text = Tools.Utils.TimeSpanToString(mCom.ProjectedTime, Tools.Utils.TimePrecision.Second, Tools.Utils.TimePrecision.Millisecond);
			else if (mLoadedFile != null)
				TTTEstimated.Text = Tools.Utils.TimeSpanToString(mLoadedFile.EstimatedTime, Tools.Utils.TimePrecision.Second, Tools.Utils.TimePrecision.Millisecond);

			if (mCom.InProgram)
				TTLEstimated.Text = "Projected Time:";
			else if (mLoadedFile != null)
				TTLEstimated.Text = "Estimated Time:";
			
			RefreshButtonEnabled();
			PB.Invalidate();
		}
		void MnExportConfigClick(object sender, EventArgs e)
		{
			string filename = null;
			using (System.Windows.Forms.SaveFileDialog ofd = new SaveFileDialog())
			{
				ofd.Filter = "GCODE Files|*.nc";
				ofd.AddExtension = true;
				ofd.RestoreDirectory = true;
				if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
					filename = ofd.FileName;
			}

			if (filename != null)
			{mCom.ExportConfig(filename);}
			
		}
		void MnImportConfigClick(object sender, EventArgs e)
		{
			string filename = null;
			using (System.Windows.Forms.OpenFileDialog ofd = new OpenFileDialog())
			{
				ofd.Filter = "GCODE Files|*.nc;*.gcode";
				ofd.CheckFileExists = true;
				ofd.Multiselect = false;
				ofd.RestoreDirectory = true;
				if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
					filename = ofd.FileName;
			}
			
			mCom.ImportConfig(filename);
		}
		void TxtManualCommandCommandEntered(string command)
		{
			mCom.EnqueueCommand(new GrblCommand(command));
		}
		void ExitToolStripMenuItemClick(object sender, EventArgs e)
		{
			Close();
		}

		
		
		
		
	}

	
}
