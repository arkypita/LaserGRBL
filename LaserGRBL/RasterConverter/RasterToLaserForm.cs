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
			mScaled = ImageTransform.ResizeImage(mOriginal, scaleSize.Width, scaleSize.Height);

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

					if (RbLineToLineTracing.Checked)
						PreviewLineByLine(th);
					else if (RbVectorize.Checked)
						PreviewVector(th);

				}
			}
		}



		private void PreviewVector(Bitmap th)
		{
			using (Graphics g = Graphics.FromImage(th))
				g.Clear(Color.White);

			PbConverted.SuspendLayout();
			if (mConverted != null)
				mConverted.Dispose();
			mConverted = th;
			PbConverted.Image = mConverted;
			PbConverted.ResumeLayout();
		}

		private void PreviewLineByLine(Bitmap th)
		{
			if (CbLinePreview.Checked)
			{
				using (Graphics g = Graphics.FromImage(th))
				{
					g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

					int mod = 7 - (int)UDQuality.Value / 3;
					for (int Y = 0; Y < th.Height; Y++)
					{
						using (Pen mark = new Pen(Color.FromArgb(0, 255, 255, 255), 1F))
						{
							using (Pen nomark = new Pen(Color.FromArgb(255, 255, 255, 255), 1F))
							{
								if (Y % mod == 0)
									g.DrawLine(mark, 0, Y, th.Width, Y);
								else
									g.DrawLine(nomark, 0, Y, th.Width, Y);
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

			using (Bitmap res = ImageTransform.ResizeImage(mOriginal, W, H))
			{
				using (Bitmap gs = ImageTransform.GrayScale(res, TBRed.Value / 100.0F, TBGreen.Value / 100.0F, TBBlue.Value / 100.0F, (ImageTransform.Formula)CbMode.SelectedItem))
				{
					using (Bitmap bc = ImageTransform.BrightnessContrast(gs, -((100 - TbBright.Value) / 100.0F), (TbContrast.Value / 100.0F)))
					{
						using (Bitmap th = ImageTransform.Threshold(bc, TbThreshold.Value / 100.0F, CbThreshold.Checked))
						{
							using (Bitmap killed = ImageTransform.KillAlfa(th))
							{
								mFile.LoadImage(killed, mFileName, (int)UDQuality.Value, IIOffsetX.CurrentValue, IIOffsetY.CurrentValue, IISpeed.CurrentValue);
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

		private static bool IsOdd(int value)
		{ return value % 2 != 0; }

		private void OnGeneratorChange(object sender, EventArgs e)
		{
			GbLineToLineOptions.Visible = RbLineToLineTracing.Checked;
			GbVectorizeOptions.Visible = RbVectorize.Checked;
			RefreshPreview();
		}

		private void UDQuality_ValueChanged(object sender, EventArgs e)
		{
			RefreshPreview();
		}


	}
}
