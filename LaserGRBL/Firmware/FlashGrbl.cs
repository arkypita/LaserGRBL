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
	public partial class FlashGrbl : Form
	{
		private const string SELECT = "--- add custom firmware ---";

		public int retval = int.MinValue;
		private object[] baudRates = { 4800, 9600, 19200, 38400, 57600, 115200, 230400, 460800, 921600 };
		public FlashGrbl()
		{
			InitializeComponent();

			InitCBFirmware();
			InitCbBaudRate();
			InitPortCB();

			BtnOK.Enabled = CbFirmware.SelectedIndex >= 0 && CbTarget.SelectedIndex >= 0;
		}

		private void InitCbBaudRate()
		{
			CbBaudRate.BeginUpdate();
			CbBaudRate.Items.AddRange(baudRates);
			CbBaudRate.EndUpdate();
			CbBaudRate.SelectedItem = 115200;
		}

		private void InitCBFirmware()
		{
			CbFirmware.BeginUpdate();
			CbFirmware.Items.Clear();
			foreach (string filename in System.IO.Directory.GetFiles(".\\Firmware\\"))
			{
				if (filename.ToLower().EndsWith(".hex"))
					CbFirmware.Items.Add(new Firmware(System.IO.Path.GetFullPath(filename)));
			}
			CbFirmware.Items.Add(SELECT);
			CbFirmware.SelectedIndexChanged += CbFirmware_SelectedIndexChanged;
			CbFirmware.EndUpdate();
		}

		private void InitPortCB() //Availabe Ports combo box
		{
			string currentport = CbPort.SelectedItem as string;
			CbPort.BeginUpdate();
			CbPort.Items.Clear();

			foreach (string portname in System.IO.Ports.SerialPort.GetPortNames())
			{
				string purgename = portname;

				//FIX https://github.com/arkypita/LaserGRBL/issues/31

				if (!char.IsDigit(purgename[purgename.Length - 1]))
					purgename = purgename.Substring(0, purgename.Length - 1);

				CbPort.Items.Add(purgename);
			}

			if (currentport != null && CbPort.Items.Contains(currentport))
				CbPort.SelectedItem = currentport;
			else if (CbPort.Items.Count > 0)
				CbPort.SelectedIndex = CbPort.Items.Count - 1;
			CbPort.EndUpdate();
		}

		private class Firmware
		{
			public string path;

			public Firmware(string path)
			{
				this.path = path;
			}

			public override string ToString()
			{
				string fname = System.IO.Path.GetFileNameWithoutExtension(path);

				try
				{
					string[] arr = fname.Split(new char[] { '-' });

					string order = arr[0];
					string grblversion = arr[1];
					string source = arr[2];
					string date = arr[3];

					date = DateTime.ParseExact(date, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToShortDateString();

					return $"{grblversion} {source} [{date}]";
				}
				catch
				{ return fname; }
			}
		}

		private void BtnOK_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Are you sure to flash a new firmware?", "Confirm request!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.OK)
			{
				try
				{
					retval = 1;

					string com = CbPort.Text;
					string firmware = (CbFirmware.SelectedItem as Firmware).path;

					System.Diagnostics.Process pProcess = new System.Diagnostics.Process();

					//strCommand is path and file name of command to run
					pProcess.StartInfo.FileName = ".\\Firmware\\avrdude.exe";

					//strCommandParameters are parameters to pass to program
					pProcess.StartInfo.Arguments = $"-patmega328p -b{CbBaudRate.SelectedItem} -P{com} -carduino -Uflash:w:\"{firmware}\":i";

					//Start the process
					pProcess.Start();

					//Wait for process to finish
					pProcess.WaitForExit();

					retval = pProcess.ExitCode; //zero se ok
				}
				catch { }
				finally { DialogResult = DialogResult.OK; }
			}
			else
			{ DialogResult = DialogResult.Cancel; }
		}

		private void CbTarget_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (CbTarget.SelectedIndex == 0)
				CbBaudRate.SelectedItem = 115200;
			else if (CbTarget.SelectedIndex == 1)
				CbBaudRate.SelectedItem = 57600;

			BtnOK.Enabled = CbFirmware.SelectedIndex >= 0 && CbTarget.SelectedIndex >= 0;
		}

		private void BtnTarget_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(@"http://lasergrbl.com/usage/flash/#target");
		}

		private void BtnFirmware_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(@"http://lasergrbl.com/usage/flash/#firmware");
		}

		private void CbFirmware_SelectedIndexChanged(object sender, EventArgs e)
		{
			if ((CbFirmware.SelectedItem as string) == SELECT)
			{
				CbFirmware.SelectedIndexChanged -= CbFirmware_SelectedIndexChanged;
				int index = -1;
				using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
				{
					ofd.FileName = null;
					ofd.Filter = "Precompiled .hex firmware|*.hex";
					ofd.CheckFileExists = true;
					ofd.Multiselect = false;
					ofd.RestoreDirectory = true;
					if (ofd.ShowDialog() == DialogResult.OK)
					{
						try
						{
							string src = ofd.FileName;
							string dst = $".\\Firmware\\{System.IO.Path.GetFileName(ofd.FileName)}";
							bool confirm = !System.IO.File.Exists(dst) || MessageBox.Show("A firmware file with this name already exists.\r\nOverwrite?", "Confirm overwrite", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK;
							if (confirm)
							{
								System.IO.File.Copy(src, dst, true);

								InitCBFirmware();

								foreach (object o in CbFirmware.Items)
								{
									if (o is Firmware)
									{
										if (System.IO.Path.GetFileName((o as Firmware).path).ToLower() == System.IO.Path.GetFileName(dst).ToLower())
											index = CbFirmware.Items.IndexOf(o);
									}
								}
							}
						}
						catch { }
					}
				}
				CbFirmware.SelectedIndex = index;
				CbFirmware.SelectedIndexChanged += CbFirmware_SelectedIndexChanged;
			}

			BtnOK.Enabled = CbFirmware.SelectedIndex >= 0 && CbTarget.SelectedIndex >= 0;
		}
	}
}
