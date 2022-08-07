using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaserGRBL.WiFiConfigurator
{
	public partial class OrturWiFiConfigPrompt : Form
	{
		public OrturWiFiConfigPrompt(GrblCore core)
		{
			InitializeComponent();
			LblPrompt.Text = String.Format(LblPrompt.Text, core.GrblVersion?.MachineName);
		}

		private void CbDontShow_CheckedChanged(object sender, EventArgs e)
		{
			Settings.SetObject("Suppress Ortur WiFI Message", CbDontShow.Checked);
		}
	}
}
