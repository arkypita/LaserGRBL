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
		private OverridesForm OvForm;
		
		public MainForm()
		{
			InitializeComponent();

			//build main communication object
			ComPort = new GrblCom();
			ComPort.MachineStatusChanged += OnMachineStatus;
			ComPort.OnFileLoaded += OnFileLoaded;
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			PreviewForm = new PreviewForm(ComPort);
			ConnectionForm = new ConnectLogForm(ComPort);
			JogForm = new JogForm(ComPort);
			OvForm = new OverridesForm(ComPort);

			//if (System.IO.File.Exists("Docking.xml"))
			//{
			//	DockArea.LoadFromXml("Docking.xml", new LaserGRBL.UserControls.DockingManager.DeserializeDockContent(this.GetContentFromPersistString));
			//}
			//else
			//{
				PreviewForm.Show(DockArea, LaserGRBL.UserControls.DockingManager.DockState.Document);
				ConnectionForm.Show(DockArea, LaserGRBL.UserControls.DockingManager.DockState.DockLeft);
				OvForm.Show(ConnectionForm.Pane, LaserGRBL.UserControls.DockingManager.DockAlignment.Bottom, 0.2);
				JogForm.Show(OvForm.Pane, null);
				
			//}
		}

		private LaserGRBL.UserControls.DockingManager.IDockContent GetContentFromPersistString(string persistString)
		{
			if (persistString == typeof(ConnectLogForm).ToString())
			{
				ConnectionForm.Show(DockArea);
				return ConnectionForm;
			}
			else if (persistString == typeof(PreviewForm).ToString())
			{
				PreviewForm.Show(DockArea);
				return PreviewForm;
			}
			else if (persistString == typeof(JogForm).ToString())
			{
				JogForm.Show(DockArea);
				return JogForm;
			}
			else if (persistString == typeof(OverridesForm).ToString())
			{
				OvForm.Show(DockArea);
				return OvForm;
			}
			else
			{
				return null;
			}
		}

		void OnFileLoaded(long elapsed, string filename)
		{
			TimerUpdate();
			TTTFile.Text = System.IO.Path.GetFileName(filename);
			TTTLines.Text = ComPort.LoadedFile.Count.ToString();
			TTTLoadedIn.Text = elapsed.ToString() + " ms";
			TTTEstimated.Text = Tools.Utils.TimeSpanToString(ComPort.LoadedFile.EstimatedTime, Tools.Utils.TimePrecision.Second, Tools.Utils.TimePrecision.Second);
		}
		
		void OnMachineStatus(GrblCom.MacStatus status)
		{
			if (InvokeRequired)
				BeginInvoke(new GrblCom.dlgOnMachineStatus(OnMachineStatus), status);
			else
				TimerUpdate();
		}
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			//DockArea.SaveAsXml("Docking.xml");
			ComPort.CloseCom();
		}
		

		private void UpdateTimer_Tick(object sender, EventArgs e)
		{
			TimerUpdate();
			ConnectionForm.TimerUpdate();
			PreviewForm.TimerUpdate();
			JogForm.TimerUpdate();
			OvForm.TimerUpdate();
		}
		
		private void TimerUpdate()
		{
			SuspendLayout();
			TTTStatus.Text = ComPort.MachineStatus.ToString();

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

		private void overridesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OvForm.Show(DockArea);
		}

	}
}
