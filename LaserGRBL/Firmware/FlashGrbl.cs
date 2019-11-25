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
		public int retval = int.MinValue;
		private object[] baudRates = { 4800, 9600, 19200, 38400, 57600, 115200, 230400, 460800, 921600 };
		public FlashGrbl()
		{
			InitializeComponent();


			foreach (string filename in System.IO.Directory.GetFiles(".\\Firmware\\"))
			{
				if (filename.ToLower().EndsWith(".hex"))
					CbFirmware.Items.Add(new Firmware(System.IO.Path.GetFullPath(filename)));
			}
			CbFirmware.SelectedIndex = 0;


			CbBaudRate.BeginUpdate();
			CbBaudRate.Items.AddRange(baudRates);
			CbBaudRate.EndUpdate();
			CbBaudRate.SelectedItem = 115200;

			InitPortCB();

			CbTarget.SelectedIndex = 0;
			//CbTarget.Items.Add(new Arduino("Arduino UNO", "atmega328p"));
			//CbTarget.Items.Add(new Arduino("Arduino Nano", "atmega328p"));
			//CbTarget.SelectedIndex = 0;
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
				string[] arr = fname.Split(new char[] { '-' });

				string order = arr[0];
				string grblversion = arr[1];
				string source = arr[2];
				string date = arr[3];

				date = DateTime.ParseExact(date, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToShortDateString();

				return $"{grblversion} {source} [{date}]";
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
					pProcess.StartInfo.Arguments = $"-patmega328p -b{CbBaudRate.SelectedItem} -P{com} -carduino -Uflash:w:{firmware}:i";

					//Start the process
					pProcess.Start();

					//Wait for process to finish
					pProcess.WaitForExit();

					retval = pProcess.ExitCode; //zero se ok
				}
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
		}
	}
}
