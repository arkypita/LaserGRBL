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
	public partial class SettingsForm : Form
	{
		public SettingsForm()
		{
			InitializeComponent();

			BackColor = ColorScheme.FormBackColor;
			GB.ForeColor = ForeColor = ColorScheme.FormForeColor;
			BtnCancel.BackColor = BtnSave.BackColor = ColorScheme.FormButtonsColor;

			InitProtocolCB();
			InitStreamingCB();

			CBSupportPWM.Checked = (bool)Settings.GetObject("Support Hardware PWM", true);
            CBLaserMode.Checked = (bool)Settings.GetObject("Laser Mode", false);
			CBProtocol.SelectedItem = Settings.GetObject("ComWrapper Protocol", ComWrapper.WrapperType.UsbSerial);
			CBStreamingMode.SelectedItem = Settings.GetObject("Streaming Mode", GrblCore.StreamingMode.Buffered);
			CbUnidirectional.Checked = (bool)Settings.GetObject("Unidirectional Engraving", false);
		}

		private void InitProtocolCB()
		{
			CBProtocol.BeginUpdate();
			CBProtocol.Items.Add(ComWrapper.WrapperType.UsbSerial);
			CBProtocol.Items.Add(ComWrapper.WrapperType.Telnet);
			CBProtocol.Items.Add(ComWrapper.WrapperType.LaserWebESP8266);
			CBProtocol.EndUpdate();
		}

		private void InitStreamingCB()
		{
			CBStreamingMode.BeginUpdate();
			CBStreamingMode.Items.Add(GrblCore.StreamingMode.Buffered);
			CBStreamingMode.Items.Add(GrblCore.StreamingMode.Synchronous);
			CBStreamingMode.Items.Add(GrblCore.StreamingMode.RepeatOnError);
			CBStreamingMode.EndUpdate();
		}

		internal static void CreateAndShowDialog()
		{
			using (SettingsForm sf = new SettingsForm())
				sf.ShowDialog();
		}

		private void BtnSave_Click(object sender, EventArgs e)
		{
			Settings.SetObject("Support Hardware PWM", CBSupportPWM.Checked);
            Settings.SetObject("Laser Mode", CBLaserMode.Checked);
			Settings.SetObject("ComWrapper Protocol", CBProtocol.SelectedItem);
			Settings.SetObject("Streaming Mode", CBStreamingMode.SelectedItem);
			Settings.SetObject("Unidirectional Engraving", CbUnidirectional.Checked);
			Settings.Save();

			Close();
		}

		private void BtnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void BtnModulationInfo_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(@"http://lasergrbl.com/configuration/#pwm-support");
		}

		private void BtnLaserMode_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(@"http://lasergrbl.com/configuration/#laser-mode");
		}

		private void BtnProtocol_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(@"http://lasergrbl.com/configuration/#protocol");
		}

		private void BtnStreamingMode_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(@"http://lasergrbl.com/configuration/#streaming-mode");
		}
    }
}
