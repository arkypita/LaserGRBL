//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

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

		internal static void CreateAndShowDialog(Form parent, GrblCore.DetectedIssue issue)
		{
			IssueDetectorForm f = new IssueDetectorForm();
			f.TxtCause.Text = issue.ToString();
			f.ShowDialog(parent);
			f.Dispose();
		}

		private void LL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Tools.Utils.OpenLink(@"https://lasergrbl.com/faq#issues");
		}

		private void BtnOK_Click(object sender, EventArgs e)
		{
			if (CbDoNotShow.Checked)
				Settings.SetObject("Do not show Issue Detector", true);
		}
	}
}
