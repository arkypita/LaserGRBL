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
	public partial class NewVersionForm : Form
	{
		private string mDownloadUrl;
		private string mPageUrl;
		private bool mClosing;
		private NewVersionForm()
		{
			InitializeComponent();
			mClosing = false;
		}

		internal static void CreateAndShowDialog(Version current, GitHub.OnlineVersion available, Form parent)
		{
			using (NewVersionForm f = new NewVersionForm())
			{
				f.mDownloadUrl = available.DownloadUrl;
				f.mPageUrl = available.HtmlUrl;
				f.LblCurrentVersion.Text = current.ToString(3);
				f.LblLatestVersion.Text = available.Version.ToString(3);
				if (available.IsPreRelease)
					f.LblLatestVersion.Text = f.LblLatestVersion.Text + " pre-release";

				DialogResult rv = f.ShowDialog(parent);

				if (rv == DialogResult.OK)
				{
					UsageStats.DoNotSendNow = true; //do not send stats now, will be sent on next start
					Application.Exit(); //exit (spawned process will apply update)
				}
				else if (rv == DialogResult.Abort)
				{
					MessageBox.Show(Strings.BoxAutoUpdateFailed, Strings.BoxAutoUpdateResult, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void BtnUpdate_Click(object sender, EventArgs e)
		{
			BtnUpdate.Enabled = false;
			BtnCancel.Enabled = false;
			GitHub.DownloadUpdateA(mDownloadUrl, dprog, dcomplete); //start download progress
		}

		private void dprog(object sender, System.Net.DownloadProgressChangedEventArgs e)
		{
			if (InvokeRequired)
			{
				Invoke(new System.Net.DownloadProgressChangedEventHandler(dprog), sender, e);
			}
			else
			{
				if (mClosing)
					return;

				PB.Value = e.ProgressPercentage;
			}
		}


		private void dcomplete(object sender, AsyncCompletedEventArgs e) //on download end (good or error)
		{
			if (InvokeRequired)
			{
				Invoke(new System.ComponentModel.AsyncCompletedEventHandler(dcomplete), e);
			}
			else
			{
				if (mClosing)
					return;

				if (e.Error == null && !e.Cancelled && GitHub.ApplyUpdateEXE())
					DialogResult = System.Windows.Forms.DialogResult.OK;
				else
					DialogResult = System.Windows.Forms.DialogResult.Abort;

				GitHub.Updating = DialogResult == System.Windows.Forms.DialogResult.OK;
				Close();
			}
		}

		private void NewVersionForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			mClosing = true;
		}

		private void BtnWebsite_Click(object sender, EventArgs e)
		{Tools.Utils.OpenLink(mPageUrl);}

	}
}

