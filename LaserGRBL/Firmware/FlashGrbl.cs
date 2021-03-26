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
	public partial class FlashGrbl : Form
	{
		private const string SELECT = "--- select custom firmware ---";

		public int retval = int.MinValue;
		private object[] baudRates = { 4800, 9600, 19200, 38400, 57600, 115200, 230400, 460800, 921600 };
		public FlashGrbl()
		{
			InitializeComponent();

			InitCBFirmware();
			InitCbBaudRate();
			InitPortCB();

			UpdateBtnOK();
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
			string FirmwareFolder = System.IO.Path.Combine(GrblCore.ExePath, "Firmware\\");
			foreach (string filename in System.IO.Directory.GetFiles(FirmwareFolder))
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
					pProcess.StartInfo.FileName = System.IO.Path.Combine(GrblCore.ExePath, "Firmware\\avrdude.exe");

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

			UpdateBtnOK();
		}

		private void BtnTarget_Click(object sender, EventArgs e)
		{
			Tools.Utils.OpenLink(@"https://lasergrbl.com/usage/flash/#target");
		}

		private void BtnFirmware_Click(object sender, EventArgs e)
		{
			Tools.Utils.OpenLink(@"https://lasergrbl.com/usage/flash/#firmware");
		}

		private void CbFirmware_SelectedIndexChanged(object sender, EventArgs e)
		{
			if ((CbFirmware.SelectedItem as string) == SELECT)
			{
				using (OpenFileDialog ofd = new OpenFileDialog())
				{
					ofd.FileName = null;
					ofd.Filter = "Precompiled .hex firmware|*.hex";
					ofd.CheckFileExists = true;
					ofd.Multiselect = false;
					ofd.RestoreDirectory = true;

					System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.DialogResult.Cancel;
					try
					{
						dialogResult = ofd.ShowDialog(this);
					}
					catch (System.Runtime.InteropServices.COMException)
					{
						ofd.AutoUpgradeEnabled = false;
						dialogResult = ofd.ShowDialog(this);
					}


					if (dialogResult == DialogResult.OK && System.IO.File.Exists(ofd.FileName))
						CbFirmware.SelectedIndex = AddOrSelect(ofd.FileName);
					else
						CbFirmware.SelectedIndex = -1;
				}
			}

			UpdateBtnOK();
		}

		private int AddOrSelect(string path)
		{
			for (int i = 0; i < CbFirmware.Items.Count; i++)
				if (CbFirmware.Items[i] is Firmware && ((Firmware)CbFirmware.Items[i]).path.ToLower() == path.ToLower())
					return i;

			return CbFirmware.Items.Add(new Firmware(path));
		}

		private void UpdateBtnOK()
		{
			BtnOK.Enabled = CbPort.Text.Trim().Length > 0 && CbFirmware.SelectedIndex >= 0 && CbTarget.SelectedIndex >= 0;
		}

		private void CbPort_TextChanged(object sender, EventArgs e)
		{
			UpdateBtnOK();
		}
	}
}
