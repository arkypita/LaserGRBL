using System;
using System.Drawing;
using System.Windows.Forms;

namespace LaserGRBL
{
	public partial class MainForm : Form
	{
		private GrblCore Core;

		public MainForm()
		{
			InitializeComponent();

			splitContainer1.FixedPanel = FixedPanel.Panel1;
			splitContainer1.SplitterDistance = (int)Settings.GetObject("MainForm Splitter Position", 260);
			autoUpdateToolStripMenuItem.Checked = (bool)Settings.GetObject("Auto Update", true);

			if (System.Threading.Thread.CurrentThread.Name == null)
				System.Threading.Thread.CurrentThread.Name = "Main Thread";
			
			using (SplashScreenForm f = new SplashScreenForm())
				f.ShowDialog();

			//build main communication object
			Core = new GrblCore(this);
			Core.MachineStatusChanged += OnMachineStatus;
			Core.OnFileLoaded += OnFileLoaded;
			Core.OnOverrideChange += ComPort_OnOverrideChange;

			ComPort_OnOverrideChange();

			PreviewForm.SetCore(Core);
			ConnectionForm.SetCore(Core);
			JogForm.SetCore(Core);

			GitHub.NewVersion += GitHub_NewVersion;
		}

		void GitHub_NewVersion(Version current, Version latest, string name, string url)
		{
			if (InvokeRequired)
			{
				Invoke(new GitHub.NewVersionDlg(GitHub_NewVersion), current, latest, name, url);
			}
			else
			{
				NewVersionForm.CreateAndShowDialog(current, latest, name, url, this);
			}
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			UpdateTimer.Enabled = true;
			GitHub.CheckVersion();
		}

		void OnFileLoaded(long elapsed, string filename)
		{
			if (InvokeRequired)
			{
				Invoke(new GrblFile.OnFileLoadedDlg(OnFileLoaded), elapsed, filename);
			}
			else
			{
				TimerUpdate();
				//TTTFile.Text = System.IO.Path.GetFileName(filename);
				TTTLines.Text = Core.LoadedFile.Count.ToString();
				//TTTLoadedIn.Text = elapsed.ToString() + " ms";
				TTTEstimated.Text = Tools.Utils.TimeSpanToString(Core.LoadedFile.EstimatedTime, Tools.Utils.TimePrecision.Second, Tools.Utils.TimePrecision.Second, " ,", true);
			}
		}
		
		void OnMachineStatus()
		{
			TimerUpdate();
		}
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			Core.CloseCom(false);
		}
		

		private void UpdateTimer_Tick(object sender, EventArgs e)
		{
			TimerUpdate();
			ConnectionForm.TimerUpdate();
			PreviewForm.TimerUpdate();
			
			JogForm.Enabled = Core.JogEnabled;
		}
		
		private void TimerUpdate()
		{
			SuspendLayout();
			TTTStatus.Text = Core.MachineStatus.ToString();

			if (Core.InProgram)
				TTTEstimated.Text = Tools.Utils.TimeSpanToString(Core.ProjectedTime, Tools.Utils.TimePrecision.Minute, Tools.Utils.TimePrecision.Second, " ,", true);
			else
				TTTEstimated.Text = Tools.Utils.TimeSpanToString(Core.LoadedFile.EstimatedTime, Tools.Utils.TimePrecision.Minute, Tools.Utils.TimePrecision.Second, " ,", true);

			if (Core.InProgram)
				TTLEstimated.Text = "Projected Time:";
			else
				TTLEstimated.Text = "Estimated Time:";

			MnFileOpen.Enabled = Core.CanLoadNewFile;
			MnSaveProgram.Enabled = Core.HasProgram;
			MnFileSend.Enabled = Core.CanSendFile; 
			MnExportConfig.Enabled = Core.CanImportExport;
			MnImportConfig.Enabled = Core.CanImportExport;
			MnGrblReset.Enabled = Core.CanResetGrbl;

			MnConnect.Visible = !Core.IsOpen;
			MnDisconnect.Visible = Core.IsOpen;

			MnGoHome.Enabled = Core.CanGoHome;
			MnUnlock.Enabled = Core.CanGoHome;
			
			TTOvG0.Visible = Core.SupportOverride;
			TTOvG1.Visible = Core.SupportOverride;
			TTOvS.Visible = Core.SupportOverride;
			spacer.Visible = Core.SupportOverride;
			
			
			switch (Core.MachineStatus)
			{
				//Disconnected, Connecting, Idle, *Run, *Hold, *Door, Home, *Alarm, *Check, *Jog
					
				case GrblCore.MacStatus.Alarm:
					TTTStatus.BackColor = Color.Red;
					TTTStatus.ForeColor = Color.White;
					break;
				case GrblCore.MacStatus.Door:
				case GrblCore.MacStatus.Hold:
					TTTStatus.BackColor = Color.DarkOrange;
					TTTStatus.ForeColor = Color.Black;
					break;
				case GrblCore.MacStatus.Jog:
				case GrblCore.MacStatus.Run:
				case GrblCore.MacStatus.Check:
					TTTStatus.BackColor = Color.LightGreen;
					TTTStatus.ForeColor = Color.Black;
					break;
				default:
					TTTStatus.BackColor = DefaultBackColor;
					TTTStatus.ForeColor = DefaultForeColor;
					break;
					

					
			}
			ResumeLayout();
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
			{Core.ExportConfig(filename);}
			
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
			
			Core.ImportConfig(filename);
		}

		void ExitToolStripMenuItemClick(object sender, EventArgs e)
		{
			Close();
		}

		private void MnFileOpen_Click(object sender, EventArgs e)
		{
			Core.OpenFile(this);
		}

		private void MnFileSend_Click(object sender, EventArgs e)
		{
			Core.EnqueueProgram();
		}

		private void MnGrblReset_Click(object sender, EventArgs e)
		{
			Core.GrblReset();
		}

		void ComPort_OnOverrideChange()
		{
			SuspendLayout();
			TTOvG0.Text = string.Format("G0 [{0:0.00}x]", Core.OverrideG0 / 100.0);
			TTOvG0.BackColor = Core.OverrideG0 > 100 ? Color.LightPink : (Core.OverrideG0 < 100 ? Color.LightBlue : SystemColors.Control) ;
			TTOvG1.Text = string.Format("G1 [{0:0.00}x]", Core.OverrideG1 / 100.0);
			TTOvG1.BackColor = Core.OverrideG1 > 100 ? Color.LightPink : (Core.OverrideG1 < 100 ? Color.LightBlue : SystemColors.Control) ;
			TTOvS.Text = string.Format("S [{0:0.00}x]", Core.OverrideS / 100.0);
			TTOvS.BackColor = Core.OverrideS > 100 ? Color.LightPink : (Core.OverrideS < 100 ? Color.LightBlue : SystemColors.Control) ;
			ResumeLayout();
		}
		void TTOvClick(object sender, EventArgs e)
		{
			GetOvMenu().Show(Cursor.Position,ToolStripDropDownDirection.AboveLeft);
		}

		
		
		internal virtual System.Windows.Forms.ContextMenuStrip GetOvMenu()
		{
			System.Windows.Forms.ContextMenuStrip CM = new System.Windows.Forms.ContextMenuStrip();
			CM.Items.Add(new ToolStripTraceBarItem(Core, 2));
			CM.Items.Add(new ToolStripTraceBarItem(Core, 1));
			CM.Items.Add(new ToolStripTraceBarItem(Core, 0));
			CM.Width = 150;

			return CM;
		}
		
		/// <summary>
		/// Adds trackbar to toolstrip stuff
		/// </summary>
		[System.Windows.Forms.Design.ToolStripItemDesignerAvailability(System.Windows.Forms.Design.ToolStripItemDesignerAvailability.ToolStrip | System.Windows.Forms.Design.ToolStripItemDesignerAvailability.StatusStrip)]
		public class ToolStripTraceBarItem : System.Windows.Forms.ToolStripControlHost
		{
			public ToolStripTraceBarItem(GrblCore core, int function)
				: base(new UserControls.LabelTB(core, function))
			{
				Control.Dock = System.Windows.Forms.DockStyle.Fill;
			}
		}

		private void MnGoHome_Click(object sender, EventArgs e)
		{
			Core.EnqueueCommand(new GrblCommand("$H"));
		}

		private void MnUnlock_Click(object sender, EventArgs e)
		{
			Core.EnqueueCommand(new GrblCommand("$X"));
		}

		private void MnConnect_Click(object sender, EventArgs e)
		{
			Core.OpenCom();
		}

		private void MnDisconnect_Click(object sender, EventArgs e)
		{
			Core.CloseCom(false);
		}
		void MnSaveProgramClick(object sender, EventArgs e)
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
			{Core.SaveProgram(filename);}
		}

		private void MNEnglish_Click(object sender, EventArgs e)
		{
			SetLanguage(new System.Globalization.CultureInfo("en"));
		}

		private void MNItalian_Click(object sender, EventArgs e)
		{
			SetLanguage(new System.Globalization.CultureInfo("it"));
		}

		private void MNSpanish_Click(object sender, EventArgs e)
		{
			SetLanguage(new System.Globalization.CultureInfo("es"));
		}

		private void SetLanguage(System.Globalization.CultureInfo ci)
		{
			if (ci != null)
				Settings.SetObject("User Language", ci);
			else
				Settings.DeleteObject("User Language");

			Settings.Save();

			if (System.Windows.Forms.MessageBox.Show("Require application restart, restart now?", "Restart required", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
				Application.Restart();
		}

		private void helpOnLineToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(@"http://lasergrbl.com/usage/");
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(@"http://lasergrbl.com/");
		}

		private void autoUpdateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			autoUpdateToolStripMenuItem.Checked = !autoUpdateToolStripMenuItem.Checked;
			Settings.SetObject("Auto Update", autoUpdateToolStripMenuItem.Checked);
			Settings.Save();

			if (autoUpdateToolStripMenuItem.Checked)
				GitHub.CheckVersion();
		}

		private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
		{
			Settings.SetObject("MainForm Splitter Position", splitContainer1.SplitterDistance);
			Settings.Save();
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SettingsForm.CreateAndShowDialog();
		}

		private void openSessionLogToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (System.IO.File.Exists("sessionlog.txt"))
				System.Diagnostics.Process.Start("sessionlog.txt");
		}

		private void toolStripMenuItem4_DropDownOpening(object sender, EventArgs e)
		{
			openSessionLogToolStripMenuItem.Enabled = System.IO.File.Exists("sessionlog.txt");
		}

		private void MNFrench_Click(object sender, EventArgs e)
		{
			SetLanguage(new System.Globalization.CultureInfo("fr"));
		}

		private void MNGerman_Click(object sender, EventArgs e)
		{
			SetLanguage(new System.Globalization.CultureInfo("de"));
		}

		private void MNDanish_Click(object sender, EventArgs e)
		{
			SetLanguage(new System.Globalization.CultureInfo("da"));
		}

		private void MNBrazilian_Click(object sender, EventArgs e)
		{
			SetLanguage(new System.Globalization.CultureInfo("pt-BR"));
		}
	}
}
