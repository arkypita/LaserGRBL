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
	public partial class RunFromPositionForm : Form
	{


		bool mAllowH;

		internal static int CreateAndShowDialog(Form parent, int total, bool allowHoming, out bool homing)
		{
			RunFromPositionForm f = new RunFromPositionForm(total, allowHoming);

			int rv = f.ShowDialog(parent) == DialogResult.OK ? f.Position : -1;
			homing = f.DoHoming;
			f.Dispose();
			return rv;
		}

		private RunFromPositionForm(int total, bool allowHoming)
		{
			InitializeComponent();
			mAllowH = allowHoming;
			UdSpecific.Maximum = total+1;
			UdSpecific.Value = 1;
			
			RbFromSpecific.Checked = true;

			CbRedoHoming.Visible = allowHoming;
			CbRedoHoming.Checked = false;
		}


	

		public bool DoHoming
		{ get { return CbRedoHoming.Checked; } }

		public int Position 
		{
			get
			{
				return (int)UdSpecific.Value -1;
			}
		}

		private void BtnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.Cancel;
			Close();
		}

		private void BtnOK_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.OK;
			Close();
		}
	}
}
