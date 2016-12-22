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
		Image mOriginal;
		bool mIgnoreEvent;
		string mFileName;


		double scaleX = 1.0; //square ratio
		
		
		private RasterToLaserForm(GrblFile file, string filename)
		{
			InitializeComponent();
			mFile = file;
			mFileName = filename;

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

			scaleX = (double)mOriginal.Width / (double)mOriginal.Height;
			
			CbMode.SuspendLayout();
			CbMode.Items.Add(ImageTransform.Formula.SimpleAverage);
			CbMode.Items.Add(ImageTransform.Formula.WeightAverage);
			CbMode.SelectedItem = ImageTransform.Formula.SimpleAverage;
			CbMode.ResumeLayout();

			RefreshPreview();
			RefreshSizes();
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

		private void RefreshSizes()
		{
			const double milimetresPerInch = 25.4;
			
			int H = (int)(mOriginal.Height / mOriginal.VerticalResolution * milimetresPerInch);
			int W = (int)(H * scaleX);
			
			TbSizeW.Text = W.ToString();
			TbSizeH.Text = H.ToString();
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
		
		void GoodInput(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            	e.Handled = true;
		}
		
		void TbSizeWTextChanged(object sender, EventArgs e)
		{
			if (!mIgnoreEvent)
			{
				mIgnoreEvent = true;
				try
				{
					int W = int.Parse(TbSizeW.Text);
					int H = (int)(W / scaleX);
				
					TbSizeW.Text = W.ToString();
					TbSizeH.Text = H.ToString();
				}
				catch {}
				mIgnoreEvent = false;
			}
		}
		void TbSizeHTextChanged(object sender, EventArgs e)
		{
			if (!mIgnoreEvent)
			{
				mIgnoreEvent = true;
				try
				{
					int H = int.Parse(TbSizeH.Text);
					int W = (int)(H * scaleX);
				
					TbSizeW.Text = W.ToString();
					TbSizeH.Text = H.ToString();
				}
				catch {}
				mIgnoreEvent = false;
			}
		}
		void BtnCreateClick(object sender, EventArgs e)
		{
			int H = int.Parse(TbSizeH.Text) * (int)UDQuality.Value;
			int W = (int)(H * scaleX);
			
			int oX = int.Parse(TbOffsetX.Text);
			int oY = int.Parse(TbOffsetY.Text);
			
			int f =  int.Parse(TbSpeed.Text);
			
			using (Bitmap bmp = new Bitmap(W, H))
			{
				using (Graphics g = Graphics.FromImage(bmp))
				{
					g.DrawImage(PbConverted.Image, 0, 0, W, H);
					mFile.LoadImage(bmp, mFileName, (int)UDQuality.Value, oX, oY, f);
				}
			}
			
			Close();
		}
		

		

	}
}
