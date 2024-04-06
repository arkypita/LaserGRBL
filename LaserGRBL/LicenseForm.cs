//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using LaserGRBL.Icons;
using LaserGRBL.UserControls;
using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace LaserGRBL
{
    public partial class LicenseForm : Form
	{
		public LicenseForm()
		{
			InitializeComponent();
			BackColor = ColorScheme.FormBackColor;
			ForeColor = ColorScheme.FormForeColor;
            richTextBox1.BackColor = ColorScheme.LogBackColor;
            richTextBox1.ForeColor = ColorScheme.FormForeColor;
            richTextBox2.BackColor = ColorScheme.LogBackColor;
            richTextBox2.ForeColor = ColorScheme.FormForeColor;
			ThemeMgr.SetTheme(this, true);
			IconsMgr.PrepareButton(BtnContinue, "mdi-checkbox-marked");
			IconsMgr.PrepareButton(BtnDonate, "mdi-heart");
        }

		private void BtnContinue_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void BtnDonate_Click(object sender, EventArgs e)
		{
			Tools.Utils.OpenLink(@"https://lasergrbl.com/donate");
		}

		internal static void CreateAndShowDialog(Form parent)
		{
			using (LicenseForm f = new LicenseForm())
			{
				f.Icon = parent.Icon;
				f.ShowDialog(parent);
			}
		}

		private void RTBLinkClick(object sender, LinkClickedEventArgs e)
		{
			Tools.Utils.OpenLink(e.LinkText);
		}
	}
}
