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
	public partial class LaserUsage : Form
	{
		private GrblCore mCore;
		private List<LaserLifeHandler.LaserLifeCounter> mList;

		private LaserUsage(GrblCore core)
		{
			InitializeComponent();
			mCore = core;
		}

		internal static void CreateAndShowDialog(GrblCore core)
		{
			using (LaserUsage sf = new LaserUsage(core))
				sf.ShowDialog(FormsHelper.MainForm);
		}

		private void LaserUsage_Load(object sender, EventArgs e)
		{
			RefreshList(true);
			RefreshButtons();
		}

		private void RefreshList(bool select = false)
		{
			try
			{
				mList = LaserLifeHandler.GetListClone();

				LVLasers.BeginUpdate();
				LVLasers.Items.Clear();

				string lastguid = Settings.GetObject<string>("Last laser used", null);
				foreach (LaserLifeHandler.LaserLifeCounter llc in mList)
				{
					LaserLifeCounterLVI lvi = new LaserLifeCounterLVI(llc);
				
					if (select && !string.IsNullOrEmpty(lastguid) && llc.Guid == lastguid)
						lvi.Selected = true;

					LVLasers.Items.Add(lvi);
				}
				LVLasers.EndUpdate();
			}
			catch { }
		}

		private void BtnClose_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void LVLasers_SelectedIndexChanged(object sender, EventArgs e)
		{
			RefreshButtons();
			Preview.LifeCounter = GetSelected();
		}

		private void RefreshButtons()
		{
			LaserLifeHandler.LaserLifeCounter selected = GetSelected();

			BtnAddNew.Enabled = true;
			BtnRemove.Enabled = selected != null;
			BtnEdit.Enabled = selected != null;

			BtnMark.Visible = selected == null || selected.DeathDate == null;
			BtnUnmark.Visible = !BtnMark.Visible;

			BtnMark.Enabled = BtnMark.Visible && selected != null && selected.DeathDate == null;
			BtnUnmark.Enabled = BtnUnmark.Visible && selected != null && selected.DeathDate != null;
		}

		private LaserLifeHandler.LaserLifeCounter GetSelected()
		{
			return LVLasers.SelectedItems.Count == 1 ? (LVLasers.SelectedItems[0] as LaserLifeCounterLVI).Parent : null;
		}

		private class LaserLifeCounterLVI : ListViewItem
		{
			public LaserLifeHandler.LaserLifeCounter Parent;
			public LaserLifeCounterLVI(LaserLifeHandler.LaserLifeCounter llc) : base()
			{
				Parent = llc;

				this.Text = llc.Name;
				this.SubItems.Add(string.IsNullOrWhiteSpace(llc.Brand) ? LaserLifeHandler.LaserLifeCounter.DEF_BRAND : llc.Brand);
				this.SubItems.Add(string.IsNullOrWhiteSpace(llc.Model) ? LaserLifeHandler.LaserLifeCounter.DEF_MODEL : llc.Model);

				this.SubItems.Add($"{llc.TimeInRun.TotalHours:0.0} h");
				this.SubItems.Add($"{llc.TimeUsageNormalizedPower.TotalHours:0.0} h");
				this.SubItems.Add($"{llc.StressTime.TotalHours:0.0} h");
				this.SubItems.Add($"{Math.Round(llc.AveragePowerFactor * 100, 0)} %");

				this.SubItems.Add(llc.PurchaseDate.HasValue ? llc.PurchaseDate.Value.ToShortDateString() : "");
				this.SubItems.Add(llc.MonitoringDate.HasValue ? llc.MonitoringDate.Value.ToShortDateString() : "");
				this.SubItems.Add(llc.DeathDate.HasValue ? llc.DeathDate.Value.ToShortDateString() : "");
			}
		}

		private void BtnAddNew_Click(object sender, EventArgs e)
		{
			LaserLifeHandler.LaserLifeCounter llc = LaserLifeHandler.LaserLifeCounter.CreateNew();
			if (LaserLifeEdit.CreateAndShowDialog(this, mCore, llc))
			{
				LaserLifeHandler.Add(llc);
				RefreshList();
			}
			RefreshButtons();
		}

		private void BtnEdit_Click(object sender, EventArgs e)
		{
			LaserLifeHandler.LaserLifeCounter selected = GetSelected();
			if (selected != null)
			{
				if (LaserLifeEdit.CreateAndShowDialog(this, mCore, selected))
				{
					LaserLifeHandler.Edit(selected);
					RefreshList();
				}
			}
			RefreshButtons();
		}

		private void BtnRemove_Click(object sender, EventArgs e)
		{
			LaserLifeHandler.LaserLifeCounter selected = GetSelected();
			if (selected != null)
			{
				if (MessageBox.Show(this, selected.HasWorked() ? "Are you sure you want to delete this laser and his history?\r\nIf your lasers has stopped working please use \"Mark as death\" button instead of delete." : "Are you sure you want to delete this laser?", "Delete data?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{ 
					string rv = LaserLifeHandler.Delete(selected);

					if (rv != null)
						MessageBox.Show(this, rv, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

					RefreshList();
				}
			}
			RefreshButtons();
		}

		private void BtnMark_Click(object sender, EventArgs e)
		{
			LaserLifeHandler.LaserLifeCounter selected = GetSelected();
			if (selected != null)
			{
				if (MessageBox.Show(this, "Are you sure your laser is death?", "Mark as death?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					LaserLifeHandler.Death(selected);
					RefreshList();
				}
			}
			RefreshButtons();
		}

		private void BtnReport_Click(object sender, EventArgs e)
		{
			RefreshButtons();
		}

		private void LVLasers_DoubleClick(object sender, EventArgs e)
		{
			if (BtnEdit.Enabled)
				BtnEdit.PerformClick();
		}

		private void BtnUnmark_Click(object sender, EventArgs e)
		{
			LaserLifeHandler.LaserLifeCounter selected = GetSelected();
			if (selected != null)
			{
				if (MessageBox.Show(this, "Restore this laser?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					LaserLifeHandler.UnDeath(selected);
					RefreshList();
				}
			}
			RefreshButtons();
		}

		private void LaserUsage_Shown(object sender, EventArgs e)
		{
			LVLasers.Focus();
		}
	}
}
