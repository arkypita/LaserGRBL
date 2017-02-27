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
		List<CustomButton> buttons;
		CustomButton inedit = null;

		private CustomButtonForm()
		{
			InitializeComponent();

			buttons = (List<CustomButton>)Settings.GetObject("Custom Buttons", new List<CustomButton>());
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
				buttons.Add(cb);
			}
			else
			{
				inedit.GCode = TBGCode.Text;
				inedit.Image = BTOpenImage.Image;
				inedit.ToolTip = TbToolTip.Text;
			}
			Settings.SetObject("Custom Buttons", buttons);

			Settings.Save();

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
						BTOpenImage.Image = LaserGRBL.RasterConverter.ImageTransform.ResizeImage(newicon, new Size(42, 42), false, System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic);
				}
			}
		}


	}
}
