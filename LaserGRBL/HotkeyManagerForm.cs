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

			BackColor = ColorScheme.FormBackColor;
			GB.ForeColor = ForeColor = ColorScheme.FormForeColor;
			DGV.BackgroundColor = SystemColors.Control;
			DGV.ForeColor = SystemColors.ControlText;
			BtnSave.BackColor = BtnCancel.BackColor = ColorScheme.FormButtonsColor;


			mLocalList = core.HotKeys.GetEditList();
			DGV.DataSource = mLocalList;
			ComputeErrors();
		}

		internal static void CreateAndShowDialog(GrblCore core)
		{
			using (HotkeyManagerForm sf = new HotkeyManagerForm(core))
				sf.ShowDialog();
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
