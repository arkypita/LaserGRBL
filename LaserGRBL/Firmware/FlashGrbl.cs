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
		public FlashGrbl()
		{
			InitializeComponent();

			foreach (string filename in System.IO.Directory.GetFiles(".\\Firmware\\"))
			{
				if (filename.ToLower().EndsWith(".hex"))
				{
					string path = System.IO.Path.GetFullPath(filename);
					string file = System.IO.Path.GetFileName(filename);
					CbFirmware.Items.Add(file);
					//ToolStripItem item = flashGrblFirmwareToolStripMenuItem.DropDownItems.Add(file);
					//item.Tag = path;
					//item.Click += UploadFirmware;
				}
			}


		}

		private void BtnOK_Click(object sender, EventArgs e)
		{
			//string com = "COM3";
			//string firmware = ((sender as ToolStripMenuItem)?.Tag as string);
			//System.Diagnostics.Process.Start(".\\Firmware\\avrdude.exe", $"-patmega328p -b115200 -P{com} -carduino -Uflash:w:{firmware}:i");
		}
	}
}
