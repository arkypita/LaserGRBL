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

			InitProtocolCB();

			CBSupportPWM.Checked = (bool)Settings.GetObject("Support Hardware PWM", true);
            CBLaserMode.Checked = (bool)Settings.GetObject("Laser Mode", false);
			CBProtocol.SelectedItem = Settings.GetObject("ComWrapper Protocol", ComWrapper.WrapperType.UsbSerial);
		}

		private void InitProtocolCB()
		{
			CBProtocol.BeginUpdate();
			CBProtocol.Items.Add(ComWrapper.WrapperType.UsbSerial);
			CBProtocol.Items.Add(ComWrapper.WrapperType.Telnet);
			CBProtocol.Items.Add(ComWrapper.WrapperType.LaserWebESP8266);
			CBProtocol.EndUpdate();
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

		private void CBProtocol_SelectedIndexChanged(object sender, EventArgs e)
		{
			//if (((ComWrapper.WrapperType)CBProtocol.SelectedItem) == ComWrapper.WrapperType.LaserWebESP8266)
			//{
			//	System.Windows.Forms.MessageBox.Show("LaserWeb ESP8266 compatible protocol is under development.\r\nWill be available in a future version.");
			//	CBProtocol.SelectedItem = Settings.GetObject("ComWrapper Protocol", ComWrapper.WrapperType.UsbSerial);
			//}
		}
    }
}
