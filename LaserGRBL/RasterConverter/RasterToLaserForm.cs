using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaserGRBL.RasterConverter
{
	public partial class RasterToLaserForm : Form
	{
		GrblFile mFile;
		string mFilename;
		Image mOriginal;
		bool mRefresh;

		private RasterToLaserForm(GrblFile file, string filename)
		{
			InitializeComponent();

			mOriginal = Image.FromFile(filename);
			PbOriginal.Image = mOriginal;
			if (mOriginal.Size.Width > PbOriginal.Size.Width || mOriginal.Size.Height > PbOriginal.Size.Height)
				PbOriginal.SizeMode = PictureBoxSizeMode.Zoom;
			else
				PbOriginal.SizeMode = PictureBoxSizeMode.Normal;
			if (mOriginal.Size.Width > PbConverted.Size.Width || mOriginal.Size.Height > PbConverted.Size.Height)
				PbConverted.SizeMode = PictureBoxSizeMode.Zoom;
			else
				PbConverted.SizeMode = PictureBoxSizeMode.Normal;

			CbMode.SuspendLayout();
			CbMode.Items.Add(ImageTransform.Formula.SimpleAverage);
			CbMode.Items.Add(ImageTransform.Formula.WeightAverage);
			CbMode.SelectedItem = ImageTransform.Formula.SimpleAverage;
			CbMode.ResumeLayout();

			RefreshPreview();
		}

		internal static void CreateAndShowDialog(GrblFile file, string filename)
		{
			RasterToLaserForm f = new RasterToLaserForm(file, filename);
			f.ShowDialog();
			f.Dispose();
		}

		private void RefreshPreview()
		{
			Image img = mOriginal;
			img = ImageTransform.GrayScale(mOriginal, (ImageTransform.Formula)CbMode.SelectedItem);
			img = ImageTransform.BrightnessContrast(img, -((100 - TbBright.Value) / 100.0F), (TbContrast.Value / 100.0F));
			PbConverted.Image = img;
		}

	

		private void CbMode_SelectedIndexChanged(object sender, EventArgs e)
		{
			RefreshPreview();
		}

		private void TbBright_Scroll(object sender, EventArgs e)
		{
			RefreshPreview();
		}

		private void TbContrast_Scroll(object sender, EventArgs e)
		{
			RefreshPreview();
		}

	}
}
