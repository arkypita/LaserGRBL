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
			ComPort = new GrblCom();
			ComPort.MachineStatusChanged += OnMachineStatus;
			ComPort.OnFileLoaded += OnFileLoaded;
			
			ConnectionForm = new ConnectLogForm(ComPort);
			ConnectionForm.Show(DockArea);
			PreviewForm = new PreviewForm(ComPort);
			PreviewForm.Show(DockArea);
			JogForm = new JogForm(ComPort);
			JogForm.Show(DockArea);
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

	}
}
