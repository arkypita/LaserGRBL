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
	public partial class ResumeJobForm : Form
	{
		internal static int CreateAndShowDialog(int exec, int sent, int target)
		{
			ResumeJobForm f = new ResumeJobForm(exec, sent, target);

			int rv = f.ShowDialog() == DialogResult.OK ? f.Position : -1;
			f.Dispose();

			return rv;
		}

		int mExec, mSent;
		private ResumeJobForm(int exec, int sent, int target)
		{
			InitializeComponent();
			mExec = exec;
			mSent = sent;
			LblManaged.Text = exec.ToString();
			LblSent.Text = sent.ToString();
			UdSpecific.Maximum = target;
			UdSpecific.Value = sent;
		}

		public int Position 
		{
			get
			{
				if (RbFromBeginning.Checked)
					return 0;
				if (RbFromSent.Checked)
					return mSent;
				if (RbFromManaged.Checked)
					return mExec;
				if (RbFromSpecific.Checked)
					return (int)UdSpecific.Value;

				return -1;
			}
		}

		private void RbCheckedChanged(object sender, EventArgs e)
		{
			BtnOK.Enabled = Position >= 0;
			UdSpecific.Enabled = RbFromSpecific.Checked;
		}
	}
}
