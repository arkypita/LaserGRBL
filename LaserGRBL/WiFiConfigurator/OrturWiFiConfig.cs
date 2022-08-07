using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace LaserGRBL.WiFiConfigurator
{
	public partial class OrturWiFiConfig : Form
	{
		private GrblCore Core;
		private string mWaitMsg;
		private Stopwatch mWaitTime = new Stopwatch();

		public OrturWiFiConfig(GrblCore core)
		{
			InitializeComponent();
			Core = core;
			mWaitMsg = LblWaitConnection.Text;
			FindSSID();
			FindPass();
		}

		private void FindPass()
		{
			string pwd = GrblCore.Configuration.WiFi_Pwd;
			TxtPassword.Text = pwd != null ? pwd : "" ;
			TxtPassword.Focus();
		}

		private void FindSSID()
		{

			string ssid = GrblCore.Configuration.WiFi_SSID;
			if (string.IsNullOrWhiteSpace(ssid))
			{
				Process P = new Process() { StartInfo = { FileName = "netsh.exe", Arguments = "wlan show interfaces", UseShellExecute = false, RedirectStandardOutput = true, CreateNoWindow = true } };
				P.Start();

				string output = P.StandardOutput.ReadToEnd();
				string line = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(l => l.Contains("SSID") && !l.Contains("BSSID"));
				if (line == null) TxtSSID.Text = "";
				else TxtSSID.Text = line.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[1].TrimStart();
			}
			else
			{
				TxtSSID.Text = ssid;
			}
		}

		private void PbRevealPwd_MouseDown(object sender, MouseEventArgs e)
		{
			TxtPassword.UseSystemPasswordChar = false;
		}

		private void PbRevealPwd_MouseUp(object sender, MouseEventArgs e)
		{
			TxtPassword.UseSystemPasswordChar = true;
		}

		private void OnTextChanges(object sender, EventArgs e)
		{
			BtnWrite.Enabled = !string.IsNullOrWhiteSpace(TxtSSID.Text) && !string.IsNullOrWhiteSpace(TxtPassword.Text);
		}

		private void BtnWrite_Click(object sender, EventArgs e)
		{
			WaitMode(true);
			Core.WriteWiFiConfig(TxtSSID.Text, TxtPassword.Text);
		}


		private void WaitMode(bool v)
		{
			TxtSSID.Enabled = TxtPassword.Enabled = PbRevealPwd.Enabled = BtnWrite.Enabled = !v;
			TimWait.Enabled = LblWaitConnection.Visible = v;

			if (v) LblWaitConnection.Text = mWaitMsg;

			if (v) mWaitTime.Restart();
			else mWaitTime.Stop();
		}

		private void OrturWiFiConfig_FormClosing(object sender, FormClosingEventArgs e)
		{
			WaitMode(false);
		}

		private void TimWait_Tick(object sender, EventArgs e)
		{
			if (mWaitTime.Elapsed.TotalSeconds > 1)
				LblWaitConnection.Text = mWaitMsg + $" ({(int)mWaitTime.Elapsed.TotalSeconds})";
			else
				LblWaitConnection.Text = mWaitMsg;

			if (Core.DetectedIP != null)
			{
				WaitMode(false);
				if (MessageBox.Show(this, $"Detected machine IP {Core.DetectedIP}\r\nDo you want to disconnect USB and connect via WiFi?", "Configuration success!", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
					DialogResult = DialogResult.OK;
			}
			else if (mWaitTime.Elapsed.TotalSeconds >= 10)
			{
				WaitMode(false);
				MessageBox.Show(this, "Unable to establish a connection: machine not connected to the WiFi network or IP address not assigned correctly. Please retry or proceed with a manual configuration.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		private void BtnHelp_Click(object sender, EventArgs e)
		{
			Tools.Utils.OpenLink(@"https://lasergrbl.com/ortur-manuals/#configure-wifi");
		}
	}
}
