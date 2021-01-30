using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaserGRBL.WiFiDiscovery
{
	public partial class DiscoveryForm : Form
	{
		String RV;

		Task T;
		CancellationTokenSource C;

		public DiscoveryForm()
		{
			InitializeComponent();

			ComWrapper.WrapperType currentWrapper = Settings.GetObject("ComWrapper Protocol", ComWrapper.WrapperType.UsbSerial);
			if (currentWrapper == ComWrapper.WrapperType.Telnet)
				UdPort.Value = 23;
			else if (currentWrapper == ComWrapper.WrapperType.LaserWebESP8266)
				UdPort.Value = 81;
			else
				UdPort.Value = 666;

		}

		private void BtnScan_Click(object sender, EventArgs e)
		{
			StartScan();
		}

		private void StartScan()
		{
			if (T == null)
			{
				Cursor = Cursors.WaitCursor;
				LV.Items.Clear();
				BtnScan.Visible = false;
				BtnStop.Visible = true;
				BtnStop.Enabled = true;
				UdPort.Enabled = false;

				T = Task.Factory.StartNew(() => { ScanAsync(); });
			}
		}

		private void ScanAsync()
		{
			C = new CancellationTokenSource();
			IPAddressHelper.ScanIP(ScanOutput, ScanProgress, C.Token, (int)UdPort.Value); //bloccante
			OnScanEnd();
		}

		void OnScanEnd()
		{
			if (InvokeRequired)
			{
				BeginInvoke((MethodInvoker)(() => { OnScanEnd(); }));
			}
			else
			{
				if (C.IsCancellationRequested)
					LblProgress.Text = "Scan aborted";
				else
					LblProgress.Text = "Scan finished";

				BtnScan.Visible = true;
				BtnStop.Visible = false;
				BtnStop.Enabled = true;
				UdPort.Enabled = true;

				T = null;
				C = null;

				Cursor = Cursors.Default;
			}
		}

		void ScanOutput(IPAddressHelper.ScanResult result)
		{
			if (InvokeRequired)
			{
				BeginInvoke((MethodInvoker)(() => { ScanOutput(result); }));
			}
			else
			{
				LV.BeginUpdate();
				LV.Items.Add(new ResultItem(result));
				LV.EndUpdate();
			}
		}

		void ScanProgress(int fase, int count, int total)
		{
			if (InvokeRequired)
			{
				BeginInvoke((MethodInvoker)(() => { ScanProgress(fase, count, total); }));
			}
			else
			{
				if (fase == 0)
					LblProgress.Text = $"Fast scan: {count}/{total}";
				if (fase == 1)
					LblProgress.Text = $"Deep scan: {count}/{total}";
			}
		}

		private void BtnStop_Click(object sender, EventArgs e)
		{
			LblProgress.Text = "Abort scan...";
			StopScan();
		}

		private void StopScan()
		{
			try
			{
				BtnStop.Enabled = false;
				Cursor = Cursors.WaitCursor;
				C?.Cancel();
			}
			catch { }
		}

		internal static string CreateAndShowDialog(Form parent)
		{
			String RV;
			using (DiscoveryForm F = new DiscoveryForm())
			{
				F.ShowDialog(parent);
				RV = F.RV;
			}
			return RV;
		}

		private void LV_SelectedIndexChanged(object sender, EventArgs e)
		{
			BtnConnect.Enabled = LV.SelectedItems.Count == 1;
		}

		private void BtnConnect_Click(object sender, EventArgs e)
		{
			if (LV.SelectedItems.Count == 1)
				ReturnItem((LV.SelectedItems[0] as ResultItem).RI);
		}

		private void ReturnItem(IPAddressHelper.ScanResult result)
		{
			if (result != null)
			{
				ComWrapper.WrapperType currentWrapper = Settings.GetObject("ComWrapper Protocol", ComWrapper.WrapperType.UsbSerial);
				if (currentWrapper == ComWrapper.WrapperType.Telnet)
					RV = $"{result.IP}:{result.Port}";
				else if (currentWrapper == ComWrapper.WrapperType.LaserWebESP8266)
					RV = $"ws://{result.IP}:{result.Port}/";
				Close();
			}
		}

		private void DiscoveryForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			StopScan();
		}

		private void UdPort_ValueChanged(object sender, EventArgs e)
		{
			ChConnection.Text = $"Connection (Port {UdPort.Value})";
		}

		private void LV_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			ListView senderList = (ListView)sender;
			ResultItem clickedItem = senderList.HitTest(e.Location).Item as ResultItem;
			if (clickedItem != null)
				ReturnItem(clickedItem.RI);
		}
	}

	internal class ResultItem : ListViewItem
	{
		public IPAddressHelper.ScanResult RI;

		public ResultItem(IPAddressHelper.ScanResult result)
		{
			RI = result;
			RI.Update += OnUpdate;

			Text = result.IP.ToString();
			SubItems.Add("");
			SubItems.Add("");
			SubItems.Add("");
			SubItems.Add("");
			DoRefresh();
		}

		private void OnUpdate(object sender, EventArgs e)
		{
			if (ListView.InvokeRequired)
				ListView.BeginInvoke((MethodInvoker)(() => OnUpdate(sender, e)));
			else
			{
				ListView.BeginUpdate();
				DoRefresh();
				ListView.EndUpdate();
			}
		}

		private void DoRefresh()
		{
			SubItems[1].Text = RI.Ping;
			SubItems[2].Text = RI.HostName;
			SubItems[3].Text = RI.MAC;
			SubItems[4].Text = RI.Telnet;
		}
	}
}
