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

namespace LaserGRBL.GrblEmulator
{
	public partial class EmulatorUI : Form
	{
		private static EmulatorUI istance;
		private bool canclose = false;

		private const int CP_NOCLOSE_BUTTON = 0x200;
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams myCp = base.CreateParams;
				myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
				return myCp;
			}
		}

		public static void ShowUI(string initmessage)
		{
			if (istance == null)
			{
				istance = new EmulatorUI(initmessage);
				istance.Show();
			}
		}

		public static void HideUI()
		{
			if (istance != null)
			{
				istance.canclose = true;
				istance.Hide();
				istance.Dispose();
				istance = null;
			}
		}

		public EmulatorUI(string initmessage)
		{
			InitializeComponent();
			Grblv11Emulator.EmulatorMessage += Grblv11Emulator_EmulatorMessage;

			Grblv11Emulator_EmulatorMessage(initmessage);
		}

		StringBuilder sb = new StringBuilder();

		void Grblv11Emulator_EmulatorMessage(string message)
		{
			if (message == null)
			{
				if (InvokeRequired)
					Invoke(new Grblv11Emulator.SendMessage(ManageClearMessage), new object [] {null});
				else
					ManageClearMessage(null);
			}
			else
			{
				lock (sb)
				{ sb.AppendLine(message); }
			}
		}


		void ManageClearMessage(string message)
		{
			lock (sb)
			{ sb.Length = 0; }
			RTB.Text = "";
		}

		private void RT_Tick(object sender, EventArgs e)
		{
			string buff = null;
			lock (sb)
			{
				if (sb.Length > 0)
				{
					buff = sb.ToString();
					sb.Length = 0;
				}
			}

			if (buff != null)
			{
				RTB.Text = RTB.Text + buff;

				if (RTB.TextLength > 10000)
					RTB.Text = RTB.Text.Substring(RTB.Text.Length - 10000);

				RTB.SelectionStart = RTB.TextLength;
				RTB.ScrollToCaret();
			}
		}

		private void EmulatorUI_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = !canclose;
		}
	}
}
