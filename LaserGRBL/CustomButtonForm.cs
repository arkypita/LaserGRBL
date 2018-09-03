using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
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
			BtnCancel.BackColor = BtnCreate.BackColor = ColorScheme.FormButtonsColor;

			CbEStyles.DataSource = Enum.GetValues(typeof(CustomButton.EnableStyles));
			CbEStyles.SelectedItem = CustomButton.EnableStyles.Always;

			CbByttonType.DataSource = Enum.GetValues(typeof(CustomButton.ButtonTypes));
			CbEStyles.SelectedItem = CustomButton.ButtonTypes.Button;
		}

		public static void CreateAndShowDialog()
		{
			using (CustomButtonForm f = new CustomButtonForm())
				f.ShowDialog();
		}

		internal static void CreateAndShowDialog(CustomButton cb)
		{
			using (CustomButtonForm f = new CustomButtonForm())
				f.ShowDialog(cb);
		}

		private void ShowDialog(CustomButton cb)
		{
			TBGCode.Text = cb.GCode;
			TBGCode2.Text = cb.GCode2;
			BTOpenImage.Image = cb.Image;
			TbToolTip.Text = cb.ToolTip;
			CbEStyles.SelectedItem = cb.EnableStyle;
			CbByttonType.SelectedItem = cb.ButtonType;

			BtnCreate.Text = "Save";
			inedit = cb;

			base.ShowDialog();
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
				cb.ToolTip = TbToolTip.Text;
				cb.EnableStyle = (CustomButton.EnableStyles)CbEStyles.SelectedItem;
				CustomButtons.Add(cb);
			}
			else
			{
				inedit.GCode = TBGCode.Text;
				inedit.GCode2 = TBGCode2.Text;
				inedit.Image = BTOpenImage.Image;
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
				ofd.Filter = "Image file|*.bmp;*.png;*.jpg;*.gif";
				ofd.CheckFileExists = true;
				ofd.Multiselect = false;
				ofd.RestoreDirectory = true;

				if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
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
	}
}
