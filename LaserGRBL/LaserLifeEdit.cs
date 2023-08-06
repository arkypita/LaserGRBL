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
	public partial class LaserLifeEdit : Form
	{
		LaserLifeHandler.LaserLifeCounter mTarget;
		public LaserLifeEdit(GrblCore core, LaserLifeHandler.LaserLifeCounter llc)
		{
			InitializeComponent();

			mTarget = llc;

			DateTime Today = DateTime.Today;

			if (llc.Name != null) TbLaserName.Text = llc.Name;
			if (llc.Brand != null) TbBrand.Text = llc.Brand;
			if (llc.Model != null) TbModel.Text = llc.Model;
			if (llc.OpticalPower != null) DiPower.CurrentValue = (float)(llc.OpticalPower.Value);

			if (llc.PurchaseDate.HasValue)
				DtpPurchaseDate.Value = llc.PurchaseDate.Value;
			else
				DtpPurchaseDate.Value = Today;

			try
			{
				DtpPurchaseDate.MaxDate = Today.AddMonths(1);
				DtpPurchaseDate.MinDate = new DateTime(2015, 1, 1);
			}
			catch { }
			TbLaserName_TextChanged(null, null);
		}

		internal static bool CreateAndShowDialog(Form parent, GrblCore core, LaserLifeHandler.LaserLifeCounter llc)
		{
			using (LaserLifeEdit sf = new LaserLifeEdit(core, llc))
				return sf.ShowDialog(parent) == DialogResult.OK;
		}

		private void BtnSave_Click(object sender, EventArgs e)
		{
			mTarget.Name = TbLaserName.Text.Trim();
			mTarget.PurchaseDate = DtpPurchaseDate.Value.Date;
			mTarget.Brand = string.IsNullOrWhiteSpace(TbBrand.Text) ? null : TbBrand.Text.Trim();
			mTarget.Model = string.IsNullOrWhiteSpace(TbModel.Text) ? null : TbModel.Text.Trim();
			mTarget.OpticalPower = DiPower.CurrentValue == 0 ? (double?)null : (double?)DiPower.CurrentValue;
		}

		private void DiPower_CurrentValueChanged(object sender, float OldValue, float NewValue, bool ByUser)
		{
			if (NewValue > 200)
				DiPower.CurrentValue = NewValue / 1000;
		}

		private void TbLaserName_TextChanged(object sender, EventArgs e)
		{
			BtnSave.Enabled = !String.IsNullOrWhiteSpace(TbLaserName.Text);
		}
	}
}
