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

			CBSupportPWM.Checked = (bool)Settings.GetObject("Support Hardware PWM", true);
		}

		internal static void CreateAndShowDialog()
		{
			using (SettingsForm sf = new SettingsForm())
				sf.ShowDialog();
		}

		private void BtnSave_Click(object sender, EventArgs e)
		{
			Settings.SetObject("Support Hardware PWM", CBSupportPWM.Checked);
			Settings.Save();

			Close();
		}

		private void BtnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void BtnModulationInfo_Click(object sender, EventArgs e)
		{

		}
	}
}
