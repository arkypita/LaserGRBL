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
	public partial class GrblConfig : Form
	{
		public GrblConfig(GrblCore core)
		{
			InitializeComponent();

			BackColor = ColorScheme.FormBackColor;
			GB.ForeColor = ForeColor = ColorScheme.FormForeColor;
			DGV.BackgroundColor = SystemColors.Control;
			DGV.ForeColor = SystemColors.ControlText;
			BtnCancel.BackColor = BtnSave.BackColor = ColorScheme.FormButtonsColor;

			core.ReadConfig();

			BS.DataSource = core.GrblConfiguration;
		}

		internal static void CreateAndShowDialog(GrblCore core)
		{
			using (GrblConfig sf = new GrblConfig(core))
				sf.ShowDialog();
		}

		private void BtnSave_Click(object sender, EventArgs e)
		{

			Settings.Save();

			Close();
		}

		private void BtnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}


	}


}
