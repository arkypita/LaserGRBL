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
		RollingBuffer rb = new RollingBuffer(30);
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

		void Grblv11Emulator_EmulatorMessage(string message)
		{
			if (message == null)
				rb.Clear();
			else
				rb.Add(message);
		}


		private void RT_Tick(object sender, EventArgs e)
		{
			string buff = "";

			string[] arr = rb.ToArray();

			foreach (string s in arr)
				buff = buff + s + "\r\n";

			RTB.Text = buff;

				
			RTB.SelectionStart = RTB.TextLength;
			RTB.ScrollToCaret();
		}

		private void EmulatorUI_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = !canclose;
		}
	}


	public class RollingBuffer //one reader, one writer... no need to sync?!
	{
		private string[] buffer;
		private int head;
		private int tail;
		private int count;
		private int capacity;

		public RollingBuffer(int size)
		{
			lock(this)
			{ 
				buffer = new string[size];
				capacity = size;
				head = 0;
				tail = 0;
				count = 0;
			}
		}

		public void Add(string item)
		{
			//lock (this)
			//{ 
				buffer[head] = item;
				head = (head + 1) % capacity;

				if (count == capacity)
				{
					tail = (tail + 1) % capacity; // Overwrite oldest element
				}
				else
				{
					count++;
				}
			//}
		}

		public string[] ToArray()
		{
			//lock(this)
			//{ 
				string[] result = new string[count];
				for (int i = 0; i < count; i++)
						result[i] = buffer[(tail + i) % capacity];
				return result;
			//}
		}

		internal void Clear()
		{
			//lock (this)
			//{
				head = 0;
				tail = 0;
				count = 0;
			//}
		}

		public int Count
		{
			get { return count; }
		}

		public int Capacity
		{
			get { return capacity; }
		}
	}
}
