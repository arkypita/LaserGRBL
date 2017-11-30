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
			BTOpenImage.Image = cb.Image;
			TbToolTip.Text = cb.ToolTip;
			CbEStyles.SelectedItem = cb.EnableStyle;

			BtnCreate.Text = "Save";
			inedit = cb;

			base.ShowDialog();
		}

		private void BtnCreate_Click(object sender, EventArgs e)
		{
			if (inedit == null)
			{
				CustomButton cb = new CustomButton();
				cb.GCode = TBGCode.Text;
				cb.Image = BTOpenImage.Image;
				cb.ToolTip = TbToolTip.Text;
				cb.EnableStyle = (CustomButton.EnableStyles)CbEStyles.SelectedItem;
				CustomButtons.Add(cb);
			}
			else
			{
				inedit.GCode = TBGCode.Text;
				inedit.Image = BTOpenImage.Image;
				inedit.ToolTip = TbToolTip.Text;
				inedit.EnableStyle = (CustomButton.EnableStyles)CbEStyles.SelectedItem;
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


	}
}
