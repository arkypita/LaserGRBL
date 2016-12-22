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
		Image mScaled;
		Image mConverted;

		bool mIgnoreEvent;
		string mFileName;


		double scaleX = 1.0; //square ratio


		private RasterToLaserForm(GrblFile file, string filename)
		{
			InitializeComponent();
			mFile = file;
			mFileName = filename;

			mOriginal = Image.FromFile(filename);
			scaleX = (double)mOriginal.Width / (double)mOriginal.Height;

			Size scaleSize = CalculateResizeToFit(mOriginal.Size, PbConverted.Size);
			mScaled = ResizeImage(mOriginal, scaleSize.Width, scaleSize.Height);

			PbOriginal.Image = mScaled;

			CbMode.SuspendLayout();
			CbMode.Items.Add(ImageTransform.Formula.SimpleAverage);
			CbMode.Items.Add(ImageTransform.Formula.WeightAverage);
			CbMode.SelectedItem = ImageTransform.Formula.SimpleAverage;
			CbMode.ResumeLayout();

			RefreshPreview();
			RefreshSizes();
		}

		private static Size CalculateResizeToFit(Size imageSize, Size boxSize)
		{
			// TODO: Check for arguments (for null and <=0)
			var widthScale = boxSize.Width / (double)imageSize.Width;
			var heightScale = boxSize.Height / (double)imageSize.Height;
			var scale = Math.Min(widthScale, heightScale);
			return new Size(
				(int)Math.Round((imageSize.Width * scale)),
				(int)Math.Round((imageSize.Height * scale))
				);
		}

		internal static void CreateAndShowDialog(GrblFile file, string filename)
		{
			RasterToLaserForm f = new RasterToLaserForm(file, filename);
			f.ShowDialog();
			f.Dispose();
		}

		private void RefreshPreview()
		{
			TbThreshold.Enabled = CbThreshold.Checked;

			using (Bitmap gs = ImageTransform.GrayScale(mScaled, TBRed.Value / 100.0F, TBGreen.Value / 100.0F, TBBlue.Value / 100.0F, (ImageTransform.Formula)CbMode.SelectedItem))
			{
				using (Bitmap bc = ImageTransform.BrightnessContrast(gs, -((100 - TbBright.Value) / 100.0F), (TbContrast.Value / 100.0F)))
				{
					Bitmap th = ImageTransform.Threshold(bc, TbThreshold.Value / 100.0F, CbThreshold.Checked);

					if (CbLinePreview.Checked)
					{
						using (Graphics g = Graphics.FromImage(th))
						{
							g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
							for (int Y = 0; Y < bc.Height; Y += 3)
							{
								using (Pen P = new Pen(Color.FromArgb(150, 255, 255, 255), 1F))
								{
									using (Pen P1 = new Pen(Color.FromArgb(100, 255, 255, 255), 1F))
									{
										if (IsOdd(Y))
											g.DrawLine(P, 0, Y, bc.Width, Y);
										else
											g.DrawLine(P1, 0, Y, bc.Width, Y);
									}
								}
							}
						}
					}

					PbConverted.SuspendLayout();
					if (mConverted != null)
						mConverted.Dispose();
					mConverted = th;
					PbConverted.Image = mConverted;
					PbConverted.ResumeLayout();
				}
			}
		}

		private void RefreshSizes()
		{
			const double milimetresPerInch = 25.4;

			int H = (int)(mOriginal.Height / mOriginal.VerticalResolution * milimetresPerInch);
			int W = (int)(H * scaleX);

			IISizeW.CurrentValue = W;
			IISizeH.CurrentValue = H;
		}


		private void OnSelectorChange(object sender, EventArgs e)
		{
			RefreshPreview();
		}

		void GoodInput(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
				e.Handled = true;
		}

		void BtnCreateClick(object sender, EventArgs e)
		{
			int H = IISizeH.CurrentValue * (int)UDQuality.Value;
			int W = IISizeW.CurrentValue * (int)UDQuality.Value;

			using (Bitmap res = ResizeImage(mOriginal, W, H))
			{
				using (Bitmap gs = ImageTransform.GrayScale(res, TBRed.Value / 100.0F, TBGreen.Value / 100.0F, TBBlue.Value / 100.0F, (ImageTransform.Formula)CbMode.SelectedItem))
				{
					using (Bitmap bc = ImageTransform.BrightnessContrast(gs, -((100 - TbBright.Value) / 100.0F), (TbContrast.Value / 100.0F)))
					{
						using (Bitmap th = ImageTransform.Threshold(bc, TbThreshold.Value / 100.0F, CbThreshold.Checked))
						{
							using (Bitmap killed = KillAlfa(th))
							{
								mFile.LoadImage(killed, mFileName, (int)UDQuality.Value, IIOffsetX.CurrentValue, IIOffsetY.CurrentValue, IISpeed.CurrentValue);
								killed.Save("test.jpg");
							}
						}
					}
				}
			}

			Close();
		}

		private void IISizeW_CurrentValueChanged(object sender, int NewValue, bool ByUser)
		{
			if (ByUser)
				IISizeH.CurrentValue = (int)(NewValue / scaleX);
		}

		private void IISizeH_CurrentValueChanged(object sender, int NewValue, bool ByUser)
		{
			if (ByUser)
				IISizeW.CurrentValue = (int)(NewValue * scaleX);
		}

		private static Bitmap ResizeImage(Image image, int width, int height)
		{
			Rectangle destRect = new Rectangle(0, 0, width, height);
			Bitmap destImage = new Bitmap(width, height);
			
			destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			using (Graphics g = Graphics.FromImage(destImage))
			{
				g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
				g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
				g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

				using (var wrapMode = new System.Drawing.Imaging.ImageAttributes())
				{
					wrapMode.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
					g.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
				}
			}

			return destImage;
		}

		private static bool IsOdd(int value)
		{ return value % 2 != 0; }

		private static Bitmap KillAlfa(Image image)
		{
			Bitmap destImage = new Bitmap(image.Width, image.Height);
			destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			using (Graphics g = Graphics.FromImage(destImage))
			{
				g.Clear(Color.White);
				g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
				g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
				g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
				g.DrawImage(image,0, 0);
			}

			return destImage;
		}


	}
}
