using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaserGRBL.PSHelper
{
	public partial class PSEditorForm : Form
	{
		public PSEditorForm()
		{
			InitializeComponent();
			source.DataSource = GrblCore.MaterialDB.Materials;
			TbNewElement.Text = GrblCore.MaterialDB.GetNewCount().ToString();
		}

		public static void CreateAndShowDialog()
		{
			using (PSEditorForm f = new PSEditorForm())
				f.ShowDialog();
		}

		private void PSEditorForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			GrblCore.MaterialDB.SaveChanges();
		}

		private void BtnImport_Click(object sender, EventArgs e)
		{
			GrblCore.MaterialDB.ImportServer();
			TbNewElement.Text = GrblCore.MaterialDB.GetNewCount().ToString();
		}
	}
}
