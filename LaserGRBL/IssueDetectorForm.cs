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
			f.LblDetected.Text = f.LblDetected.Text + " " + issue.ToString();
			f.ShowDialog();
			f.Dispose();
		}

		private void LL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(@"http://lasergrbl.com/faq#issues");
		}
	}
}
