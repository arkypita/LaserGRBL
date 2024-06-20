//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using LaserGRBL.Icons;
using LaserGRBL.UserControls;
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
	public partial class HotkeyManagerForm : Form
	{
		private List<HotKeysManager.HotKey> mLocalList = null;
		GrblCore mCore = null;
		public HotkeyManagerForm(GrblCore core)
		{
			InitializeComponent();
			mCore = core;
			ThemeMgr.SetTheme(this, true);
            IconsMgr.PrepareButton(BtnSave, "mdi-checkbox-marked");
            IconsMgr.PrepareButton(BtnCancel, "mdi-close-box");

            BackColor = ColorScheme.FormBackColor;
			GB.ForeColor = ForeColor = ColorScheme.FormForeColor;
            GB.BackColor = ColorScheme.FormBackColor;
			DGV.AutoGenerateColumns = false;
            DGV.EnableHeadersVisualStyles = false;
            DGV.BackgroundColor = ColorScheme.FormBackColor; //SystemColors.Control;
			DGV.ForeColor = ColorScheme.FormForeColor; //SystemColors.ControlText;
            DGV.DefaultCellStyle.BackColor = ColorScheme.FormBackColor;
            DGV.ColumnHeadersDefaultCellStyle.BackColor = ColorScheme.FormBackColor;
            DGV.ColumnHeadersDefaultCellStyle.ForeColor = ColorScheme.FormForeColor;
            DGV.RowHeadersDefaultCellStyle.BackColor = ColorScheme.FormBackColor;
            DGV.RowHeadersDefaultCellStyle.ForeColor = ColorScheme.FormForeColor;
            BtnSave.BackColor = BtnCancel.BackColor = ColorScheme.FormButtonsColor;


			mLocalList = core.HotKeys.GetEditList();
			DGV.DataSource = mLocalList;
            ComputeErrors();
		}

		internal static void CreateAndShowDialog(Form parent, GrblCore core)
		{
			using (HotkeyManagerForm sf = new HotkeyManagerForm(core))
				sf.ShowDialog(parent);
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			mLastkeyData = Keys.None;
			base.OnKeyUp(e);
		}

		Keys mLastkeyData = Keys.None;
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData != mLastkeyData)
			{
				mLastkeyData = keyData;
				HotKeysManager.HotKey hk = DGV.CurrentRow.DataBoundItem as HotKeysManager.HotKey;

				if (HotKeysManager.HotKey.ValidShortcut(mLastkeyData))
					SetNewValue(hk, mLastkeyData);
				else if (mLastkeyData == Keys.Delete)
					SetNewValue(hk, Keys.None);
			}
			else
			{
				return base.ProcessCmdKey(ref msg, keyData);
			}
			return false;
		}

		private void SetNewValue(HotKeysManager.HotKey hk, Keys value)
		{
			hk.SetShortcut(value);
			DGV.Refresh();
			ComputeErrors();
		}

		private void ComputeErrors()
		{
			List<DataGridViewRow> inerror = new List<DataGridViewRow>();
			List<DataGridViewRow> noerror = new List<DataGridViewRow>();
			foreach (DataGridViewRow ra in DGV.Rows)
			{
				HotKeysManager.HotKey a = ra.DataBoundItem as HotKeysManager.HotKey;

				foreach (DataGridViewRow rb in DGV.Rows)
				{
					HotKeysManager.HotKey b = rb.DataBoundItem as HotKeysManager.HotKey;
					if (!ReferenceEquals(a, b) && a.Combination != Keys.None && a.Combination == b.Combination)
						inerror.Add(ra);
				}

				if (!inerror.Contains(ra))
					noerror.Add(ra);
			}

			foreach (DataGridViewRow row in noerror)
				row.ErrorText = null;
			foreach (DataGridViewRow row in inerror)
				row.ErrorText = "Duplicated value!";

			BtnSave.Enabled = inerror.Count == 0;
		}

		private void BtnSave_Click(object sender, EventArgs e)
		{
			mCore.WriteHotkeys(mLocalList);
		}
	}
}
