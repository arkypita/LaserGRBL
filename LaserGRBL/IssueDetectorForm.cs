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
	public partial class IssueDetectorForm : Form
	{
		public IssueDetectorForm()
		{
			InitializeComponent();
		}

		internal static void CreateAndShowDialog(GrblCore.DetectedIssue issue)
		{
			IssueDetectorForm f = new IssueDetectorForm();
			f.TxtCause.Text = issue.ToString();
			f.ShowDialog();
			f.Dispose();
		}

		private void LL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(@"http://lasergrbl.com/faq#issues");
		}

		private void BtnOK_Click(object sender, EventArgs e)
		{
			if (CbDoNotShow.Checked)
				Settings.SetObject("Do not show Issue Detector", true);
		}
	}
}
