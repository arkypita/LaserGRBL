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
		private GrblCore Core;
		private ConnectLogForm ConnectionForm;
		private PreviewForm PreviewForm;
		private JogForm JogForm;

		public MainForm()
		{
			InitializeComponent();

			SplashScreenForm f = new SplashScreenForm();
			f.ShowDialog();
			f.Dispose();

			//build main communication object
			Core = new GrblCore(this);
			Core.MachineStatusChanged += OnMachineStatus;
			Core.OnFileLoaded += OnFileLoaded;
			Core.OnOverrideChange += ComPort_OnOverrideChange;
			ComPort_OnOverrideChange();
			
						
			PreviewForm = new PreviewForm(Core);
			ConnectionForm = new ConnectLogForm(Core);
			JogForm = new JogForm(Core);
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			if (System.IO.File.Exists("LaserGRBL.Docking.xml"))
			{
				DockArea.LoadFromXml("LaserGRBL.Docking.xml", new LaserGRBL.UserControls.DockingManager.DeserializeDockContent(this.GetContentFromPersistString));
			}
			else
			{
				PreviewForm.Show(DockArea, LaserGRBL.UserControls.DockingManager.DockState.Document);
				ConnectionForm.Show(DockArea, LaserGRBL.UserControls.DockingManager.DockState.DockLeft);
				JogForm.Show(ConnectionForm.Pane, LaserGRBL.UserControls.DockingManager.DockAlignment.Bottom, 0.2);
			}
			
			UpdateTimer.Enabled = true;
		}

		private LaserGRBL.UserControls.DockingManager.IDockContent GetContentFromPersistString(string persistString)
		{
			if (persistString == typeof(ConnectLogForm).ToString())
				return ConnectionForm;
			else if (persistString == typeof(PreviewForm).ToString())
				return PreviewForm;
			else if (persistString == typeof(JogForm).ToString())
				return JogForm;
			else
				return null;
		}

		void OnFileLoaded(long elapsed, string filename)
		{
			TimerUpdate();
			//TTTFile.Text = System.IO.Path.GetFileName(filename);
			TTLines.Text = String.Format("Lines: {0}", Core.LoadedFile.Count);
			//TTTLoadedIn.Text = elapsed.ToString() + " ms";
			TTTEstimated.Text = Tools.Utils.TimeSpanToString(Core.LoadedFile.EstimatedTime, Tools.Utils.TimePrecision.Second, Tools.Utils.TimePrecision.Second);
		}
		
		void OnMachineStatus()
		{
			TimerUpdate();
		}
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			DockArea.SaveAsXml("LaserGRBL.Docking.xml");
			Core.CloseCom();
		}
		

		private void UpdateTimer_Tick(object sender, EventArgs e)
		{
			TimerUpdate();
			ConnectionForm.TimerUpdate();
			PreviewForm.TimerUpdate();
			JogForm.TimerUpdate();
		}
		
		private void TimerUpdate()
		{
			SuspendLayout();
			TTStatus.Text = string.Format("Status: {0}", Core.MachineStatus);

			if (Core.InProgram)
				TTTEstimated.Text = Tools.Utils.TimeSpanToString(Core.ProjectedTime, Tools.Utils.TimePrecision.Minute, Tools.Utils.TimePrecision.Second);
			else
				TTTEstimated.Text = Tools.Utils.TimeSpanToString(Core.LoadedFile.EstimatedTime, Tools.Utils.TimePrecision.Minute, Tools.Utils.TimePrecision.Second);

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
					TTStatus.BackColor = Color.Red;
					TTStatus.ForeColor = Color.White;
					break;
				case GrblCore.MacStatus.Door:
				case GrblCore.MacStatus.Hold: 					
					TTStatus.BackColor = Color.DarkOrange;
					TTStatus.ForeColor = Color.Black;
					break;
				case GrblCore.MacStatus.Jog:
				case GrblCore.MacStatus.Run:
				case GrblCore.MacStatus.Check:
					TTStatus.BackColor = Color.LightGreen;
					TTStatus.ForeColor = Color.Black;
					break;
				default:
					TTStatus.BackColor = DefaultBackColor;
					TTStatus.ForeColor = DefaultForeColor;
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
			Core.OpenFile();
		}

		private void MnFileSend_Click(object sender, EventArgs e)
		{
			Core.EnqueueProgram();
		}

		private void MnGrblReset_Click(object sender, EventArgs e)
		{
			Core.GrblReset();
		}

		private void joggingToolStripMenuItem_Click(object sender, EventArgs e)
		{
			JogForm.Show(DockArea);
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
			Core.CloseCom();
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
		
		
	}
}
