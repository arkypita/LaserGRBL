//Copyright (c) 2023 Alexandre Besnier - https://github.com/Varamil/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using LaserGRBL.PSHelper;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LaserGRBL
{
	/// <summary>
	/// Description of ConvertSizeAndOptionForm.
	/// </summary>
	public partial class ChangeScaleForm : Form
	{
		GrblCore mCore;

		bool loading = false;

		internal static void CreateAndShowDialog(GrblCore core, Form parent)
        {
            using (ChangeScaleForm f = new ChangeScaleForm(core))
            {
                f.ShowDialogForm(parent);
            }
        }

        private ChangeScaleForm(GrblCore core)
		{
			InitializeComponent();
			mCore = core;

			BackColor = ColorScheme.FormBackColor;
			GbLaser.ForeColor = GbSpeed.ForeColor = ForeColor = ColorScheme.FormForeColor;
			BtnCancel.BackColor = BtnCreate.BackColor = ColorScheme.FormButtonsColor;

		}

		public void ShowDialogForm(Form parent)
        {
			loading = true;
			DIscaleX.CurrentValue = 1;
			DIscaleY.CurrentValue = 1;

			DIsizeX.CurrentValue = (float)mCore.LoadedFile.Range.MovingRange.Width;
			DIsizeY.CurrentValue = (float)mCore.LoadedFile.Range.MovingRange.Height;

			ChKeepRatio.Checked = true;
			ChKeepRatio.Enabled = !mCore.LoadedFile.IsG2G3();

			DIscaleY.Enabled = ChKeepRatio.Enabled && !ChKeepRatio.Checked;
			DIsizeY.Enabled = ChKeepRatio.Enabled && !ChKeepRatio.Checked;

			loading = false;

			if (ShowDialog(parent) == DialogResult.OK)
			{
				mCore.ChangeScale(DIscaleX.CurrentValue, DIscaleY.CurrentValue);
			}
		}

        private void DIscaleX_CurrentValueChanged(object sender, float OldValue, float NewValue, bool ByUser)
        {
			if (loading) return;

			loading = true;
			DIsizeX.CurrentValue = DIscaleX.CurrentValue * (float)mCore.LoadedFile.Range.MovingRange.Width;
			loading = false;

			if (!DIscaleY.Enabled)
				DIscaleY.CurrentValue = DIscaleX.CurrentValue;			
		}

		private void DIscaleY_CurrentValueChanged(object sender, float OldValue, float NewValue, bool ByUser)
		{
			if (loading) return;

			loading = true;
			DIsizeY.CurrentValue = DIscaleY.CurrentValue * (float)mCore.LoadedFile.Range.MovingRange.Height;
			loading = false;

		}

		private void DIsizeX_CurrentValueChanged(object sender, float OldValue, float NewValue, bool ByUser)
        {
			if (loading) return;

			loading = true;
			DIscaleX.CurrentValue = DIsizeX.CurrentValue / (float)mCore.LoadedFile.Range.MovingRange.Width;
			loading = false;

			if (!DIsizeY.Enabled)
				DIsizeY.CurrentValue = DIsizeX.CurrentValue;			
		}

		private void DIsizeY_CurrentValueChanged(object sender, float OldValue, float NewValue, bool ByUser)
		{
			if (loading) return;

			loading = true;
			DIscaleY.CurrentValue = DIsizeY.CurrentValue / (float)mCore.LoadedFile.Range.MovingRange.Height;
			loading = false;
		}

        private void ChKeepRatio_CheckedChanged(object sender, EventArgs e)
        {
			if (loading) return;

			if (ChKeepRatio.Checked)
            {
				//lock
				loading = true;
				DIscaleY.CurrentValue = DIscaleX.CurrentValue ;
				DIsizeY.CurrentValue = DIscaleY.CurrentValue * (float)mCore.LoadedFile.Range.MovingRange.Height;

				DIscaleY.Enabled = false;
				DIsizeY.Enabled = false;
				loading = false;

			} else
            {
				//unlock
				DIscaleY.Enabled = true;
				DIsizeY.Enabled = true;
			}
		}

        private void BtnCancel_Click(object sender, EventArgs e)
        {
			DialogResult = DialogResult.Cancel;
			Close();
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
			DialogResult = DialogResult.OK;
			Close();
        }
    }
}
