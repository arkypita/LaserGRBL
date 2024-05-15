//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using LaserGRBL.Icons;
using LaserGRBL.UserControls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LaserGRBL
{
    public partial class CustomButtonForm : Form
	{
		CustomButton inedit = null;

		private CustomButtonForm()
		{
			InitializeComponent();

			BackColor = ColorScheme.FormBackColor;
			ForeColor = ColorScheme.FormForeColor;
			LblDescription.LinkColor = ColorScheme.LinkColor;
            IconsMgr.PrepareButton(BtnCreate, "mdi-checkbox-marked");
            IconsMgr.PrepareButton(BtnCancel, "mdi-close-box");

			if (!IconsMgr.LegacyIcons)
			{
				LblCaption.Text = Strings.MdiIconCaption;
                LblCaption.ForeColor = ColorScheme.LinkColor;
                LblCaption.Click += LblCaption_Click;
				LblCaption.Cursor = Cursors.Hand;
				BTOpenImage.Visible = false;
				LblImage.Visible = false;
			}

            CbEStyles.DataSource = Enum.GetValues(typeof(CustomButton.EnableStyles));
			CbEStyles.SelectedItem = CustomButton.EnableStyles.Always;

			CbByttonType.DataSource = Enum.GetValues(typeof(CustomButton.ButtonTypes));
			CbEStyles.SelectedItem = CustomButton.ButtonTypes.Button;

            ThemeMgr.SetTheme(this);
        }

        private void LblCaption_Click(object sender, EventArgs e)
        {
            Tools.Utils.OpenLink("https://pictogrammers.com/library/mdi/");
        }

        public static void CreateAndShowDialog(Form parent)
		{
			using (CustomButtonForm f = new CustomButtonForm())
				f.ShowDialog(parent);
		}

		internal static void CreateAndShowDialog(Form parent, CustomButton cb)
		{
			using (CustomButtonForm f = new CustomButtonForm())
				f.ShowDialog(parent, cb);
		}

		private void ShowDialog(Form parent, CustomButton cb)
		{
			TBGCode.Text = cb.GCode;
			TBGCode2.Text = cb.GCode2;
			BTOpenImage.Image = cb.Image;
			TbCaption.Text = cb.Caption;
			TbToolTip.Text = cb.ToolTip;
			CbEStyles.SelectedItem = cb.EnableStyle;
			CbByttonType.SelectedItem = cb.ButtonType;

			BtnCreate.Text = "Save";
			inedit = cb;

			base.ShowDialog(parent);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			CbByttonType_SelectedIndexChanged(this, null);
		}

		private void BtnCreate_Click(object sender, EventArgs e)
		{
			if (TBGCode.Text.Trim().Length == 0)
			{
				System.Windows.Forms.MessageBox.Show(Strings.BoxCustomButtonPleaseGCodeText, Strings.BoxCustomButtonPleaseToolTipTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
				TBGCode.Focus();
				return;
			}

			if (TBGCode2.Visible && TBGCode2.Text.Trim().Length == 0)
			{
				System.Windows.Forms.MessageBox.Show(Strings.BoxCustomButtonPleaseGCodeText, Strings.BoxCustomButtonPleaseToolTipTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
				TBGCode2.Focus();
				return;
			}

			if (TbToolTip.Text.Trim().Length == 0)
			{
				System.Windows.Forms.MessageBox.Show(Strings.BoxCustomButtonPleaseToolTipText, Strings.BoxCustomButtonPleaseToolTipTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
				TbToolTip.Focus();
				return;
			}

			if (inedit == null)
			{
				CustomButton cb = new CustomButton();
				cb.ButtonType = (CustomButton.ButtonTypes)CbByttonType.SelectedItem;
				cb.GCode = TBGCode.Text;
				cb.GCode2 = TBGCode2.Text;
				cb.Image = BTOpenImage.Image;
				cb.Caption = TbCaption.Text;
				cb.ToolTip = TbToolTip.Text;
				cb.EnableStyle = (CustomButton.EnableStyles)CbEStyles.SelectedItem;
				CustomButtons.Add(cb);
			}
			else
			{
				inedit.GCode = TBGCode.Text;
				inedit.GCode2 = TBGCode2.Text;
				inedit.Image = BTOpenImage.Image;
				inedit.Caption = TbCaption.Text;
				inedit.ToolTip = TbToolTip.Text;
				inedit.EnableStyle = (CustomButton.EnableStyles)CbEStyles.SelectedItem;
				inedit.ButtonType = (CustomButton.ButtonTypes)CbByttonType.SelectedItem;
			}

			CustomButtons.SaveFile();

			Close();
		}

		private void BtnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void BTOpenImage_Click(object sender, EventArgs e)
		{
			using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
			{
				ofd.Filter = "Image file|*.bmp;*.png;*.jpg;*.jpeg;*.gif";
				ofd.CheckFileExists = true;
				ofd.Multiselect = false;
				ofd.RestoreDirectory = true;

				System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.DialogResult.Cancel;
				try
				{
					dialogResult = ofd.ShowDialog(this);
				}
				catch (System.Runtime.InteropServices.COMException)
				{
					ofd.AutoUpgradeEnabled = false;
					dialogResult = ofd.ShowDialog(this);
				}


				if (dialogResult == System.Windows.Forms.DialogResult.OK)
				{
					using (System.Drawing.Image newicon = Bitmap.FromFile(ofd.FileName))
						BTOpenImage.Image = LaserGRBL.RasterConverter.ImageTransform.ResizeImage(newicon, new Size(48, 48), false, System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic);
				}
			}
		}

		private void CbByttonType_SelectedIndexChanged(object sender, EventArgs e)
		{
			SuspendLayout();
			LblGCode2.Visible = TBGCode2.Visible = ((CustomButton.ButtonTypes)CbByttonType.SelectedItem) != CustomButton.ButtonTypes.Button;
			tableLayoutPanel3.SetColumnSpan(TBGCode, TBGCode2.Visible ? 1 : 2);
			ResumeLayout();
		}

		private void LblDescription_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string link = (sender as LinkLabel).Tag.ToString();
			if (!string.IsNullOrEmpty(link))
			{Tools.Utils.OpenLink(link);}
		}
	}
}
