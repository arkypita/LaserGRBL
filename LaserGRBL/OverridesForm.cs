using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaserGRBL
{
	public partial class OverridesForm : UserControls.DockingManager.DockContent
	{
		GrblCom ComPort;

		public OverridesForm(GrblCom com)
		{
			InitializeComponent();
			ComPort = com;
			TimerUpdate();
		}

		public void TimerUpdate()
		{
			SuspendLayout();
			foreach (Control ctr in tlp1.Controls)
					ctr.Enabled = ComPort.JogEnabled;
			ResumeLayout();
		}

		private void TbRapid_ValueChanged(object sender, EventArgs e)
		{
			if (TbRapid.Value == 0)
				LblRapid.Text = "Rapid [0.25x]";
			else if (TbRapid.Value == 1)
				LblRapid.Text = "Rapid [0.50x]";
			else if (TbRapid.Value == 2)
				LblRapid.Text = "Rapid [1.00x]";

			ComPort.SetRapidOverride(TbRapid.Value);
		}

		private void TbSpeed_ValueChanged(object sender, EventArgs e)
		{
			LblSpeed.Text = string.Format("Speed [{0:0.00}x]", TbSpeed.Value / 100.0);
			ComPort.SetSpeedOverride(TbSpeed.Value);
		}

		private void TbPower_ValueChanged(object sender, EventArgs e)
		{
			LblPower.Text = string.Format("Power [{0:0.00}x]", TbPower.Value / 100.0);
			ComPort.SetPowerOverride(TbPower.Value);
		}

	}
}


