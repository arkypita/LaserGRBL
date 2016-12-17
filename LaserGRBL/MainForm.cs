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
		private GrblCom ComPort;
		private ConnectLogForm ConnectionForm;
		private PreviewForm PreviewForm;
		private JogForm JogForm;

		public MainForm()
		{
			InitializeComponent();

			//build main communication object
			ComPort = new GrblCom(this);
			ComPort.MachineStatusChanged += OnMachineStatus;
			ComPort.OnFileLoaded += OnFileLoaded;
			ComPort.OnOverrideChange += ComPort_OnOverrideChange;
			ComPort_OnOverrideChange();
			
						
			PreviewForm = new PreviewForm(ComPort);
			ConnectionForm = new ConnectLogForm(ComPort);
			JogForm = new JogForm(ComPort);
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			if (System.IO.File.Exists("Docking.xml"))
			{
				DockArea.LoadFromXml("Docking.xml", new LaserGRBL.UserControls.DockingManager.DeserializeDockContent(this.GetContentFromPersistString));
			}
			else
			{
				PreviewForm.Show(DockArea, LaserGRBL.UserControls.DockingManager.DockState.Document);
				ConnectionForm.Show(DockArea, LaserGRBL.UserControls.DockingManager.DockState.DockLeft);
				JogForm.Show(ConnectionForm.Pane, LaserGRBL.UserControls.DockingManager.DockAlignment.Bottom, 0.2);
			}
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
			TTLines.Text = String.Format("Lines: {0}", ComPort.LoadedFile.Count);
			//TTTLoadedIn.Text = elapsed.ToString() + " ms";
			TTTEstimated.Text = Tools.Utils.TimeSpanToString(ComPort.LoadedFile.EstimatedTime, Tools.Utils.TimePrecision.Second, Tools.Utils.TimePrecision.Second);
		}
		
		void OnMachineStatus()
		{
			TimerUpdate();
		}
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			DockArea.SaveAsXml("Docking.xml");
			ComPort.CloseCom();
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
			TTStatus.Text = string.Format("Status: {0}", ComPort.MachineStatus);

			if (ComPort.InProgram)
				TTTEstimated.Text = Tools.Utils.TimeSpanToString(ComPort.ProjectedTime, Tools.Utils.TimePrecision.Second, Tools.Utils.TimePrecision.Second);
			else
				TTTEstimated.Text = Tools.Utils.TimeSpanToString(ComPort.LoadedFile.EstimatedTime, Tools.Utils.TimePrecision.Second, Tools.Utils.TimePrecision.Second);

			if (ComPort.InProgram)
				TTLEstimated.Text = "Projected Time:";
			else
				TTLEstimated.Text = "Estimated Time:";
			
			MnFileOpen.Enabled = true;
			MnFileSend.Enabled = ComPort.HasProgram && ComPort.IsOpen && ComPort.MachineStatus == GrblCom.MacStatus.Idle; 
			MnExportConfig.Enabled = MnImportConfig.Enabled = ComPort.IsOpen && ComPort.MachineStatus == GrblCom.MacStatus.Idle;
			MnGrblReset.Enabled = ComPort.IsOpen && ComPort.MachineStatus != GrblCom.MacStatus.Disconnected;
			
			TTOvG0.Visible = ComPort.SupportOverride;
			TTOvG1.Visible = ComPort.SupportOverride;
			TTOvS.Visible = ComPort.SupportOverride;
			spacer.Visible = ComPort.SupportOverride;
			
			
			switch (ComPort.MachineStatus)
			{
				//Disconnected, Connecting, Idle, *Run, *Hold, *Door, Home, *Alarm, *Check, *Jog
					
				case GrblCom.MacStatus.Alarm:
					TTStatus.BackColor = Color.Red;
					TTStatus.ForeColor = Color.White;
					break;
				case GrblCom.MacStatus.Door:
				case GrblCom.MacStatus.Hold: 					
					TTStatus.BackColor = Color.DarkOrange;
					TTStatus.ForeColor = Color.Black;
					break;
				case GrblCom.MacStatus.Jog:
				case GrblCom.MacStatus.Run:
				case GrblCom.MacStatus.Check:
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
			{ComPort.ExportConfig(filename);}
			
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
			
			ComPort.ImportConfig(filename);
		}

		void ExitToolStripMenuItemClick(object sender, EventArgs e)
		{
			Close();
		}

		private void MnFileOpen_Click(object sender, EventArgs e)
		{
			ComPort.OpenFile();
		}

		private void MnFileSend_Click(object sender, EventArgs e)
		{
			ComPort.EnqueueProgram();
		}

		private void MnGrblReset_Click(object sender, EventArgs e)
		{
			ComPort.GrblReset();
		}

		private void joggingToolStripMenuItem_Click(object sender, EventArgs e)
		{
			JogForm.Show(DockArea);
		}
		
		void ComPort_OnOverrideChange()
		{
			SuspendLayout();
			TTOvG0.Text = string.Format("G0 [{0:0.00}x]", ComPort.OverrideG0 / 100.0);
			TTOvG0.BackColor = ComPort.OverrideG0 > 100 ? Color.LightPink : (ComPort.OverrideG0 < 100 ? Color.LightBlue : SystemColors.Control) ;
			TTOvG1.Text = string.Format("G1 [{0:0.00}x]", ComPort.OverrideG1 / 100.0);
			TTOvG1.BackColor = ComPort.OverrideG1 > 100 ? Color.LightPink : (ComPort.OverrideG1 < 100 ? Color.LightBlue : SystemColors.Control) ;
			TTOvS.Text = string.Format("S [{0:0.00}x]", ComPort.OverrideS / 100.0);
			TTOvS.BackColor = ComPort.OverrideS > 100 ? Color.LightPink : (ComPort.OverrideS < 100 ? Color.LightBlue : SystemColors.Control) ;
			ResumeLayout();
		}
		void TTOvClick(object sender, EventArgs e)
		{
			GetOvMenu().Show(Cursor.Position);
		}

		
		
		internal virtual System.Windows.Forms.ContextMenuStrip GetOvMenu()
		{
			System.Windows.Forms.ContextMenuStrip CM = new System.Windows.Forms.ContextMenuStrip();
			CM.Items.Add(new ToolStripTraceBarItem(ComPort, 0));
			CM.Items.Add(new ToolStripTraceBarItem(ComPort, 1));
			CM.Items.Add(new ToolStripTraceBarItem(ComPort, 2));
			CM.Width = 150;

			return CM;
		}
		
		/// <summary>
		/// Adds trackbar to toolstrip stuff
		/// </summary>
		[System.Windows.Forms.Design.ToolStripItemDesignerAvailability(System.Windows.Forms.Design.ToolStripItemDesignerAvailability.ToolStrip | System.Windows.Forms.Design.ToolStripItemDesignerAvailability.StatusStrip)]
		public class ToolStripTraceBarItem : System.Windows.Forms.ToolStripControlHost
		{
			public ToolStripTraceBarItem(GrblCom com, int function): base(new UserControls.LabelTB(com, function))
			{
				Control.Dock = System.Windows.Forms.DockStyle.Fill;
			}
		}
		
		
	}
}
