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
		internal static int CreateAndShowDialog(int exec, int sent, int target, GrblCore.DetectedIssue issue)
		{
			ResumeJobForm f = new ResumeJobForm(exec, sent, target, issue);

			int rv = f.ShowDialog() == DialogResult.OK ? f.Position : -1;
			f.Dispose();

			return rv;
		}

		int mExec, mSent, mSomeLine;
		private ResumeJobForm(int exec, int sent, int target, GrblCore.DetectedIssue issue)
		{
			InitializeComponent();
			mSomeLine = Math.Max(0, exec - 17);
			mExec = exec;
			mSent = sent;
			LblSomeLines.Text = mSomeLine.ToString();
			LblSent.Text = sent.ToString();
			UdSpecific.Maximum = sent;
			UdSpecific.Value = sent;
			RbSomeLines.Enabled = LblSomeLines.Enabled = mSomeLine > 0;

			 TxtCause.Text = issue.ToString();

			if (issue == GrblCore.DetectedIssue.StopMoving || issue == GrblCore.DetectedIssue.StopResponding || issue == GrblCore.DetectedIssue.UnexpectedReset || issue == GrblCore.DetectedIssue.ManualReset)
			{
				//all this causes indicate a situation where grbl does not execute the content of buffers (both planned and rx)
				//so restart from some line (17 lines) before the last command in planned buffer

				if (RbSomeLines.Enabled)
					RbSomeLines.Checked = true;
				else
					RbFromBeginning.Checked = true;
			}
			else if (issue == GrblCore.DetectedIssue.ManualDisconnect || issue == GrblCore.DetectedIssue.UnexpectedDisconnect)
			{
				//if issue is a disconnect all sent lines could be already executed
				//so restart from sent
				RbFromSent.Checked = true;
			}

		}

		public int Position 
		{
			get
			{
				if (RbFromBeginning.Checked)
					return 0;
				if (RbSomeLines.Checked)
					return mSomeLine;
				if (RbFromSent.Checked)
					return mSent;
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
