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
		private string mUrl;
		private bool mClosing;
		public NewVersionForm(string url)
		{
			InitializeComponent();
			mUrl = url;
			mClosing = false;
		}

		internal static void CreateAndShowDialog(Version current, Version latest, string name, string url, Form parent)
		{
			using (NewVersionForm f = new NewVersionForm(url))
			{
				f.LblCurrentVersion.Text = current.ToString(3);
				f.LblLatestVersion.Text = latest.ToString(3);

				DialogResult rv = f.ShowDialog(parent);

				if (rv == DialogResult.OK)
					System.Windows.Forms.Application.Exit(); //exit (spawned process will apply update)
				else if (rv == DialogResult.Abort)
					System.Windows.Forms.MessageBox.Show(Strings.BoxAutoUpdateFailed, Strings.BoxAutoUpdateResult, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void BtnUpdate_Click(object sender, EventArgs e)
		{
			BtnUpdate.Enabled = false;
			BtnCancel.Enabled = false;
			GitHub.DownloadUpdateA(mUrl, dprog, dcomplete); //start download progress
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
		{
			System.Diagnostics.Process.Start(@"https://github.com/arkypita/LaserGRBL/releases/latest");
		}






	}
}

