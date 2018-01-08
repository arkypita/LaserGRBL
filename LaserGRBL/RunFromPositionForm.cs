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
	public partial class RunFromPositionForm : Form
	{


		bool mAllowH;

		internal static int CreateAndShowDialog(int total, bool allowHoming, out bool homing)
		{
			RunFromPositionForm f = new RunFromPositionForm(total, allowHoming);

			int rv = f.ShowDialog() == DialogResult.OK ? f.Position : -1;
			homing = f.DoHoming;
			f.Dispose();
			return rv;
		}

		private RunFromPositionForm(int total, bool allowHoming)
		{
			InitializeComponent();
			mAllowH = allowHoming;
			UdSpecific.Maximum = total+1;
			UdSpecific.Value = 1;
			
			RbFromSpecific.Checked = true;

			CbRedoHoming.Visible = allowHoming;
			CbRedoHoming.Checked = false;
		}


	

		public bool DoHoming
		{ get { return CbRedoHoming.Checked; } }

		public int Position 
		{
			get
			{
				return (int)UdSpecific.Value -1;
			}
		}

		private void BtnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.Cancel;
			Close();
		}

		private void BtnOK_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.OK;
			Close();
		}
	}
}
