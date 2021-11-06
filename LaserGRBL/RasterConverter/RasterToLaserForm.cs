//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Threading;

namespace LaserGRBL.RasterConverter
{
	public partial class RasterToLaserForm : Form
	{
		GrblCore mCore;
		ImageProcessor IP;
		bool preventClose;
		bool supportPWM = Settings.GetObject("Support Hardware PWM", true);

		private RasterToLaserForm(GrblCore core, string filename, bool append)
		{
			InitializeComponent();
			mCore = core;

			UDQuality.Maximum = UDFillingQuality.Maximum = GetMaxQuality();

			BackColor = ColorScheme.FormBackColor;
			GbCenterlineOptions.ForeColor = GbConversionTool.ForeColor = GbLineToLineOptions.ForeColor = GbParameters.ForeColor = GbVectorizeOptions.ForeColor = ForeColor = ColorScheme.FormForeColor;
			BtnCancel.BackColor = BtnCreate.BackColor = ColorScheme.FormButtonsColor;

			IP = new ImageProcessor(core, filename, GetImageSize(), append);
			//PbOriginal.Image = IP.Original;
			ImageProcessor.PreviewReady += OnPreviewReady;
			ImageProcessor.PreviewBegin += OnPreviewBegin;
			ImageProcessor.GenerationComplete += OnGenerationComplete;

			LblGrayscale.Visible = CbMode.Visible = !IP.IsGrayScale;

			CbResize.SuspendLayout();
			CbResize.AddItem(InterpolationMode.HighQualityBicubic);
			CbResize.AddItem(InterpolationMode.NearestNeighbor);
			CbResize.ResumeLayout();

			CbDither.SuspendLayout();
			foreach (ImageTransform.DitheringMode formula in Enum.GetValues(typeof(ImageTransform.DitheringMode)))
				CbDither.Items.Add(formula);
			CbDither.SelectedIndex = 0;
			CbDither.ResumeLayout();
			CbDither.SuspendLayout();

			CbMode.SuspendLayout();
			foreach (ImageTransform.Formula formula in Enum.GetValues(typeof(ImageTransform.Formula)))
				CbMode.AddItem(formula);
			CbMode.SelectedIndex = 0;
			CbMode.ResumeLayout();

			CbDirections.SuspendLayout();
			foreach (ImageProcessor.Direction direction in Enum.GetValues(typeof(ImageProcessor.Direction)))
				if (GrblFile.RasterFilling(direction))
					CbDirections.AddItem(direction, true);
			CbDirections.SelectedIndex = 0;
			CbDirections.ResumeLayout();

			CbFillingDirection.SuspendLayout();
			CbFillingDirection.AddItem(ImageProcessor.Direction.None);
			foreach (ImageProcessor.Direction direction in Enum.GetValues(typeof(ImageProcessor.Direction)))
				if (GrblFile.VectorFilling(direction))
					CbFillingDirection.AddItem(direction);
			foreach (ImageProcessor.Direction direction in Enum.GetValues(typeof(ImageProcessor.Direction)))
				if (GrblFile.RasterFilling(direction))
					CbFillingDirection.AddItem(direction);
			CbFillingDirection.SelectedIndex = 0;
			CbFillingDirection.ResumeLayout();

			RbLineToLineTracing.Visible = supportPWM;

			LoadSettings();
			RefreshVE();
		}

		private decimal GetMaxQuality()
		{
			return Settings.GetObject("Raster Hi-Res", false) ? 50 : 20;
		}

		private Size GetImageSize()
		{
			return new Size(PbConverted.Size.Width - 20, PbConverted.Size.Height - 20);
		}

		void OnPreviewBegin()
		{
			preventClose = true;

			if (InvokeRequired)
			{
				Invoke(new ImageProcessor.PreviewBeginDlg(OnPreviewBegin));
			}
			else
			{
				WT.Enabled = true;
				BtnCreate.Enabled = false;
			}
		}
		void OnPreviewReady(Image img)
		{
			if (InvokeRequired)
			{
				Invoke(new ImageProcessor.PreviewReadyDlg(OnPreviewReady), img);
			}
			else
			{
				Image old_orig = PbOriginal.Image;
				Image old_conv = PbConverted.Image;
				PbOriginal.Image = CreatePaper(IP.Original);
				PbConverted.Image = CreatePaper(img);


				if (old_conv != null)
					old_conv.Dispose();

				if (old_orig != null)
					old_orig.Dispose();

				WT.Enabled = false;
				WB.Visible = false;
				WB.Running = false;
				BtnCreate.Enabled = true;
				preventClose = false;
			}
		}

		private static Image CreatePaper(Image img)
		{
			Image newimage = new Bitmap(img.Width + 6, img.Height + 6);
			using (Graphics g = Graphics.FromImage(newimage))
			{
				g.Clear(Color.Transparent);
				g.FillRectangle(Brushes.Gray, 6, 6, img.Width + 2, img.Height + 2); //ombra
				g.FillRectangle(Brushes.White, 0, 0, img.Width + 2, img.Height + 2); //pagina
				g.DrawRectangle(Pens.LightGray, 0, 0, img.Width + 1, img.Height + 1); //bordo
				g.DrawImage(img, 1, 1); //disegno
			}
			return newimage;
		}

		void WTTick(object sender, EventArgs e)
		{
			WT.Enabled = false;
			WB.Visible = true;
			WB.Running = true;
		}

		internal static void CreateAndShowDialog(GrblCore core, string filename, Form parent, bool append)
		{
			using (RasterToLaserForm f = new RasterToLaserForm(core, filename, append))
				f.ShowDialog(parent);
		}

		void GoodInput(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
				e.Handled = true;
		}

		void BtnCreateClick(object sender, EventArgs e)
		{
			if (IP.SelectedTool == ImageProcessor.Tool.Vectorize && GrblFile.TimeConsumingFilling(IP.FillingDirection) && IP.FillingQuality > 2
			&& System.Windows.Forms.MessageBox.Show(this, $"Using { GrblCore.TranslateEnum(IP.FillingDirection)} with quality > 2 line/mm could be very time consuming with big image. Continue?", "Warning",  MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.OK)
				return;				
			
			using (ConvertSizeAndOptionForm f = new ConvertSizeAndOptionForm(mCore))
			{
				f.ShowDialog(this, IP);
				if (f.DialogResult == DialogResult.OK)
				{
					preventClose = true;
					Cursor = Cursors.WaitCursor;
					SuspendLayout();
					TCOriginalPreview.SelectedIndex = 0;
					FlipControl.Enabled = false;
					BtnCreate.Enabled = false;
					WB.Visible = true;
					WB.Running = true;
					FormBorderStyle = FormBorderStyle.FixedSingle;
					TlpLeft.Enabled = false;
					MaximizeBox = false;
					ResumeLayout();

					StoreSettings();

					IP.GenerateGCode(); //processo asincrono che ritorna con l'evento "OnGenerationComplete"
				}
			}
		}


		void OnGenerationComplete(Exception ex)
		{
			if (InvokeRequired)
			{
				BeginInvoke(new ImageProcessor.GenerationCompleteDlg(OnGenerationComplete), ex);
			}
			else
			{

				try
				{
					if (IP != null)
					{
						if (IP.SelectedTool == ImageProcessor.Tool.Dithering)
							mCore.UsageCounters.Dithering++;
						else if (IP.SelectedTool == ImageProcessor.Tool.Line2Line)
							mCore.UsageCounters.Line2Line++;
						else if (IP.SelectedTool == ImageProcessor.Tool.Vectorize)
							mCore.UsageCounters.Vectorization++;
						else if (IP.SelectedTool == ImageProcessor.Tool.Centerline)
							mCore.UsageCounters.Centerline++;
						else if (IP.SelectedTool == ImageProcessor.Tool.NoProcessing)
							mCore.UsageCounters.Passthrough++;

						Cursor = Cursors.Default;

						if (ex != null && !(ex is ThreadAbortException))
							MessageBox.Show(ex.Message);

						preventClose = false;
						WT.Enabled = false;

						ImageProcessor P = IP;
						IP = null;
						P?.Dispose();
					}
				}
				finally { Close(); }
			}
		}


		private void StoreSettings()
		{
			Settings.SetObject("GrayScaleConversion.RasterConversionTool", RbLineToLineTracing.Checked ? ImageProcessor.Tool.Line2Line : RbDithering.Checked ? ImageProcessor.Tool.Dithering : RbCenterline.Checked ? ImageProcessor.Tool.Centerline : ImageProcessor.Tool.Vectorize);

			Settings.SetObject("GrayScaleConversion.Line2LineOptions.Direction", (ImageProcessor.Direction)CbDirections.SelectedItem);
			Settings.SetObject("GrayScaleConversion.Line2LineOptions.Quality", UDQuality.Value);
			Settings.SetObject("GrayScaleConversion.Line2LineOptions.Preview", CbLinePreview.Checked);

			Settings.SetObject("GrayScaleConversion.VectorizeOptions.SpotRemoval.Enabled", CbSpotRemoval.Checked);
			Settings.SetObject("GrayScaleConversion.VectorizeOptions.SpotRemoval.Value", UDSpotRemoval.Value);
			Settings.SetObject("GrayScaleConversion.VectorizeOptions.Smooting.Enabled", CbSmoothing.Checked);
			Settings.SetObject("GrayScaleConversion.VectorizeOptions.Smooting.Value", UDSmoothing.Value);
			Settings.SetObject("GrayScaleConversion.VectorizeOptions.Optimize.Enabled", CbOptimize.Checked);
			Settings.SetObject("GrayScaleConversion.VectorizeOptions.UseAdaptiveQuality.Enabled", CbAdaptiveQuality.Checked);
			Settings.SetObject("GrayScaleConversion.VectorizeOptions.Optimize.Value", UDOptimize.Value);
			Settings.SetObject("GrayScaleConversion.VectorizeOptions.DownSample.Enabled", CbDownSample.Checked);
			Settings.SetObject("GrayScaleConversion.VectorizeOptions.DownSample.Value", UDDownSample.Value);
			Settings.SetObject("GrayScaleConversion.VectorizeOptions.FillingDirection", (ImageProcessor.Direction)CbFillingDirection.SelectedItem);
			Settings.SetObject("GrayScaleConversion.VectorizeOptions.FillingQuality", UDFillingQuality.Value);
			Settings.SetObject("GrayScaleConversion.VectorizeOptions.OptimizeFast.Enabled", CbOptimizeFast.Checked);

			Settings.SetObject("GrayScaleConversion.DitheringOptions.DitheringMode", (ImageTransform.DitheringMode)CbDither.SelectedItem);

			Settings.SetObject("GrayScaleConversion.Parameters.Interpolation", (InterpolationMode)CbResize.SelectedItem);
			Settings.SetObject("GrayScaleConversion.Parameters.Mode", (ImageTransform.Formula)CbMode.SelectedItem);
			Settings.SetObject("GrayScaleConversion.Parameters.R", TBRed.Value);
			Settings.SetObject("GrayScaleConversion.Parameters.G", TBGreen.Value);
			Settings.SetObject("GrayScaleConversion.Parameters.B", TBBlue.Value);
			Settings.SetObject("GrayScaleConversion.Parameters.Brightness", TbBright.Value);
			Settings.SetObject("GrayScaleConversion.Parameters.Contrast", TbContrast.Value);
			Settings.SetObject("GrayScaleConversion.Parameters.Threshold.Enabled", CbThreshold.Checked);
			Settings.SetObject("GrayScaleConversion.Parameters.Threshold.Value", TbThreshold.Value);
			Settings.SetObject("GrayScaleConversion.Parameters.WhiteClip", TBWhiteClip.Value);

			Settings.SetObject("GrayScaleConversion.VectorizeOptions.BorderSpeed", IP.BorderSpeed);
			Settings.SetObject("GrayScaleConversion.Gcode.Speed.Mark", IP.MarkSpeed);

			Settings.SetObject("GrayScaleConversion.Gcode.LaserOptions.LaserOn", IP.LaserOn);
			Settings.SetObject("GrayScaleConversion.Gcode.LaserOptions.LaserOff", IP.LaserOff);
			Settings.SetObject("GrayScaleConversion.Gcode.LaserOptions.PowerMin", IP.MinPower);
			Settings.SetObject("GrayScaleConversion.Gcode.LaserOptions.PowerMax", IP.MaxPower);

			Settings.SetObject("GrayScaleConversion.Gcode.Offset.X", IP.TargetOffset.X);
			Settings.SetObject("GrayScaleConversion.Gcode.Offset.Y", IP.TargetOffset.Y);

			Settings.SetObject("GrayScaleConversion.Gcode.ImageSize.W", IP.TargetSize.Width);
			Settings.SetObject("GrayScaleConversion.Gcode.ImageSize.H", IP.TargetSize.Height);

			Settings.SetObject("GrayScaleConversion.VectorizeOptions.LineThreshold.Enabled", IP.UseLineThreshold);
			Settings.SetObject("GrayScaleConversion.VectorizeOptions.LineThreshold.Value", IP.LineThreshold);
			Settings.SetObject("GrayScaleConversion.VectorizeOptions.CornerThreshold.Enabled", IP.UseCornerThreshold);
			Settings.SetObject("GrayScaleConversion.VectorizeOptions.CornerThreshold.Value", IP.CornerThreshold);
		}

		private void LoadSettings()
		{
			if ((IP.SelectedTool = Settings.GetObject("GrayScaleConversion.RasterConversionTool", ImageProcessor.Tool.Line2Line)) == ImageProcessor.Tool.Line2Line)
				RbLineToLineTracing.Checked = true;
			else if ((IP.SelectedTool = Settings.GetObject("GrayScaleConversion.RasterConversionTool", ImageProcessor.Tool.Line2Line)) == ImageProcessor.Tool.Dithering)
				RbDithering.Checked = true;
			else if ((IP.SelectedTool = Settings.GetObject("GrayScaleConversion.RasterConversionTool", ImageProcessor.Tool.Line2Line)) == ImageProcessor.Tool.Centerline)
				RbCenterline.Checked = true;
			else
				RbVectorize.Checked = true;

			CbDirections.SelectedItem = IP.LineDirection = Settings.GetObject("GrayScaleConversion.Line2LineOptions.Direction", ImageProcessor.Direction.Horizontal);
			UDQuality.Value = IP.Quality = Math.Min(UDQuality.Maximum, Settings.GetObject("GrayScaleConversion.Line2LineOptions.Quality", 3.0m));
			CbLinePreview.Checked = IP.LinePreview = Settings.GetObject("GrayScaleConversion.Line2LineOptions.Preview", false);

			CbSpotRemoval.Checked = IP.UseSpotRemoval = Settings.GetObject("GrayScaleConversion.VectorizeOptions.SpotRemoval.Enabled", false);
			UDSpotRemoval.Value = IP.SpotRemoval = Settings.GetObject("GrayScaleConversion.VectorizeOptions.SpotRemoval.Value", 2.0m);
			CbSmoothing.Checked = IP.UseSmoothing = Settings.GetObject("GrayScaleConversion.VectorizeOptions.Smooting.Enabled", false);
			UDSmoothing.Value = IP.Smoothing = Settings.GetObject("GrayScaleConversion.VectorizeOptions.Smooting.Value", 1.0m);
			CbOptimize.Checked = IP.UseOptimize = Settings.GetObject("GrayScaleConversion.VectorizeOptions.Optimize.Enabled", false);
			CbAdaptiveQuality.Checked = IP.UseAdaptiveQuality = Settings.GetObject("GrayScaleConversion.VectorizeOptions.UseAdaptiveQuality.Enabled", false);
			UDOptimize.Value = IP.Optimize = Settings.GetObject("GrayScaleConversion.VectorizeOptions.Optimize.Value", 0.2m);
			CbDownSample.Checked = IP.UseDownSampling = Settings.GetObject("GrayScaleConversion.VectorizeOptions.DownSample.Enabled", false);
			UDDownSample.Value = IP.DownSampling = Settings.GetObject("GrayScaleConversion.VectorizeOptions.DownSample.Value", 2.0m);
			CbOptimizeFast.Checked = IP.OptimizeFast = Settings.GetObject("GrayScaleConversion.VectorizeOptions.OptimizeFast.Enabled", false);

			CbFillingDirection.SelectedItem = IP.FillingDirection = Settings.GetObject("GrayScaleConversion.VectorizeOptions.FillingDirection", ImageProcessor.Direction.None);
			UDFillingQuality.Value = IP.FillingQuality = Math.Min(UDFillingQuality.Maximum, Settings.GetObject("GrayScaleConversion.VectorizeOptions.FillingQuality", 3.0m));

			CbResize.SelectedItem = IP.Interpolation = Settings.GetObject("GrayScaleConversion.Parameters.Interpolation", InterpolationMode.HighQualityBicubic);
			CbMode.SelectedItem = IP.Formula = Settings.GetObject("GrayScaleConversion.Parameters.Mode", ImageTransform.Formula.SimpleAverage);
			TBRed.Value = IP.Red = Settings.GetObject("GrayScaleConversion.Parameters.R", 100);
			TBGreen.Value = IP.Green = Settings.GetObject("GrayScaleConversion.Parameters.G", 100);
			TBBlue.Value = IP.Blue = Settings.GetObject("GrayScaleConversion.Parameters.B", 100);
			TbBright.Value = IP.Brightness = Settings.GetObject("GrayScaleConversion.Parameters.Brightness", 100);
			TbContrast.Value = IP.Contrast = Settings.GetObject("GrayScaleConversion.Parameters.Contrast", 100);
			CbThreshold.Checked = IP.UseThreshold = Settings.GetObject("GrayScaleConversion.Parameters.Threshold.Enabled", false);
			TbThreshold.Value = IP.Threshold = Settings.GetObject("GrayScaleConversion.Parameters.Threshold.Value", 50);
			TBWhiteClip.Value = IP.WhiteClip = Settings.GetObject("GrayScaleConversion.Parameters.WhiteClip", 5);

			CbDither.SelectedItem = Settings.GetObject("GrayScaleConversion.DitheringOptions.DitheringMode", ImageTransform.DitheringMode.FloydSteinberg);

			CbLineThreshold.Checked = IP.UseLineThreshold = Settings.GetObject("GrayScaleConversion.VectorizeOptions.LineThreshold.Enabled", true);
			TBLineThreshold.Value = IP.LineThreshold = Settings.GetObject("GrayScaleConversion.VectorizeOptions.LineThreshold.Value", 10);

			CbCornerThreshold.Checked = IP.UseCornerThreshold = Settings.GetObject("GrayScaleConversion.VectorizeOptions.CornerThreshold.Enabled", true);
			TBCornerThreshold.Value = IP.CornerThreshold = Settings.GetObject("GrayScaleConversion.VectorizeOptions.CornerThreshold.Value", 110);

			if (RbLineToLineTracing.Checked && !supportPWM)
				RbDithering.Checked = true;
		}

		void OnRGBCBDoubleClick(object sender, EventArgs e)
		{ ((UserControls.ColorSlider)sender).Value = 100; }

		void OnThresholdDoubleClick(object sender, EventArgs e)
		{ ((UserControls.ColorSlider)sender).Value = 50; }

		private void CbMode_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (IP != null)
			{
				IP.Formula = (ImageTransform.Formula)CbMode.SelectedItem;

				SuspendLayout();
				TBRed.Visible = TBGreen.Visible = TBBlue.Visible = (IP.Formula == ImageTransform.Formula.Custom && !IP.IsGrayScale);
				LblRed.Visible = LblGreen.Visible = LblBlue.Visible = (IP.Formula == ImageTransform.Formula.Custom && !IP.IsGrayScale);
				ResumeLayout();
			}
		}

		private void TBRed_ValueChanged(object sender, EventArgs e)
		{ if (IP != null) IP.Red = TBRed.Value; }

		private void TBGreen_ValueChanged(object sender, EventArgs e)
		{ if (IP != null) IP.Green = TBGreen.Value; }

		private void TBBlue_ValueChanged(object sender, EventArgs e)
		{ if (IP != null) IP.Blue = TBBlue.Value; }

		private void TbBright_ValueChanged(object sender, EventArgs e)
		{ if (IP != null) IP.Brightness = TbBright.Value; }

		private void TbContrast_ValueChanged(object sender, EventArgs e)
		{ if (IP != null) IP.Contrast = TbContrast.Value; }

		private void CbThreshold_CheckedChanged(object sender, EventArgs e)
		{
			if (IP != null)
			{
				IP.UseThreshold = CbThreshold.Checked;
				RefreshVE();
			}
		}

		private void RefreshVE()
		{
			GbParameters.Enabled = !RbNoProcessing.Checked;
			GbVectorizeOptions.Visible = RbVectorize.Checked;
			GbCenterlineOptions.Visible = RbCenterline.Checked;
			GbLineToLineOptions.Visible = RbLineToLineTracing.Checked || RbDithering.Checked;
			GbPassthrough.Visible = RbNoProcessing.Checked;
			GbLineToLineOptions.Text = RbLineToLineTracing.Checked ? Strings.Line2LineOptions : Strings.DitheringOptions;

			CbThreshold.Visible = !RbDithering.Checked;
			TbThreshold.Visible = !RbDithering.Checked && CbThreshold.Checked;

			LblDitherMode.Visible = CbDither.Visible = RbDithering.Checked;
		}

		private void TbThreshold_ValueChanged(object sender, EventArgs e)
		{ if (IP != null) IP.Threshold = TbThreshold.Value; }

		private void RbLineToLineTracing_CheckedChanged(object sender, EventArgs e)
		{
			if (IP != null)
			{
				if (RbLineToLineTracing.Checked)
					IP.SelectedTool = ImageProcessor.Tool.Line2Line;
				RefreshVE();
			}
		}

		private void RbNoProcessing_CheckedChanged(object sender, EventArgs e)
		{
			if (IP != null)
			{
				if (RbNoProcessing.Checked)
					IP.SelectedTool = ImageProcessor.Tool.NoProcessing;
				RefreshVE();
			}
		}

		private void RbCenterline_CheckedChanged(object sender, EventArgs e)
		{
			if (IP != null)
			{
				if (RbCenterline.Checked)
					IP.SelectedTool = ImageProcessor.Tool.Centerline;
				RefreshVE();
			}
		}

		private void RbVectorize_CheckedChanged(object sender, EventArgs e)
		{
			if (IP != null)
			{
				if (RbVectorize.Checked)
					IP.SelectedTool = ImageProcessor.Tool.Vectorize;
				RefreshVE();
			}
		}

		private void UDQuality_ValueChanged(object sender, EventArgs e)
		{ if (IP != null) IP.Quality = UDQuality.Value; }

		private void CbLinePreview_CheckedChanged(object sender, EventArgs e)
		{ if (IP != null) IP.LinePreview = CbLinePreview.Checked; }

		private void UDSpotRemoval_ValueChanged(object sender, EventArgs e)
		{ if (IP != null) IP.SpotRemoval = (int)UDSpotRemoval.Value; }

		private void CbSpotRemoval_CheckedChanged(object sender, EventArgs e)
		{
			if (IP != null)
				IP.UseSpotRemoval = CbSpotRemoval.Checked;
			UDSpotRemoval.Enabled = CbSpotRemoval.Checked;
		}

		private void UDSmoothing_ValueChanged(object sender, EventArgs e)
		{ if (IP != null) IP.Smoothing = UDSmoothing.Value; }

		private void CbSmoothing_CheckedChanged(object sender, EventArgs e)
		{
			if (IP != null) IP.UseSmoothing = CbSmoothing.Checked;
			UDSmoothing.Enabled = CbSmoothing.Checked;
		}

		private void UDOptimize_ValueChanged(object sender, EventArgs e)
		{ if (IP != null) IP.Optimize = UDOptimize.Value; }

		private void CbOptimize_CheckedChanged(object sender, EventArgs e)
		{
			if (IP != null) IP.UseOptimize = CbOptimize.Checked;
			UDOptimize.Enabled = CbOptimize.Checked;
		}

		private void RasterToLaserForm_Load(object sender, EventArgs e)
		{ if (IP != null) IP.Resume(); }

		void RasterToLaserFormFormClosing(object sender, FormClosingEventArgs e)
		{
			if (preventClose)
			{
				e.Cancel = true;
			}
			else
			{
				ImageProcessor.PreviewReady -= OnPreviewReady;
				ImageProcessor.PreviewBegin -= OnPreviewBegin;
				ImageProcessor.GenerationComplete -= OnGenerationComplete;
				if (IP != null) IP.Dispose();
			}
		}

		void CbDirectionsSelectedIndexChanged(object sender, EventArgs e)
		{ if (IP != null) IP.LineDirection = (ImageProcessor.Direction)CbDirections.SelectedItem; }

		void CbResizeSelectedIndexChanged(object sender, EventArgs e)
		{
			if (IP != null)
			{
				IP.Interpolation = (InterpolationMode)CbResize.SelectedItem;
				//PbOriginal.Image = IP.Original;
			}
		}
		void BtRotateCWClick(object sender, EventArgs e)
		{
			if (IP != null)
			{
				IP.RotateCW();
				//PbOriginal.Image = IP.Original;
			}
		}
		void BtRotateCCWClick(object sender, EventArgs e)
		{
			if (IP != null)
			{
				IP.RotateCCW();
				//PbOriginal.Image = IP.Original;
			}
		}
		void BtFlipHClick(object sender, EventArgs e)
		{
			if (IP != null)
			{
				IP.FlipH();
				//PbOriginal.Image = IP.Original;
			}
		}
		void BtFlipVClick(object sender, EventArgs e)
		{
			if (IP != null)
			{
				IP.FlipV();
				//PbOriginal.Image = IP.Original;
			}
		}

		void BtnRevertClick(object sender, EventArgs e)
		{
			if (IP != null)
			{
				IP.Revert();
				//PbOriginal.Image = IP.Original;
			}
		}

		private void CbFillingDirection_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (IP != null)
			{
				IP.FillingDirection = (ImageProcessor.Direction)CbFillingDirection.SelectedItem;
				BtnFillingQualityInfo.Visible = LblFillingLineLbl.Visible = LblFillingQuality.Visible = UDFillingQuality.Visible = ((ImageProcessor.Direction)CbFillingDirection.SelectedItem != ImageProcessor.Direction.None);
			}
		}

		private void UDFillingQuality_ValueChanged(object sender, EventArgs e)
		{
			if (IP != null)
				IP.FillingQuality = UDFillingQuality.Value;
		}


		bool isDrag = false;
		Rectangle imageRectangle;
		Rectangle theRectangle = new Rectangle(new Point(0, 0), new Size(0, 0));
		Point sP;
		Point eP;

		void PbConvertedMouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && Cropping)
			{
				int left = (PbConverted.Width - PbConverted.Image.Width) / 2;
				int top = (PbConverted.Height - PbConverted.Image.Height) / 2;
				int right = PbConverted.Width - left;
				int bottom = PbConverted.Height - top;

				imageRectangle = new Rectangle(left, top, PbConverted.Image.Width, PbConverted.Image.Height);

				if ((e.X >= left && e.Y >= top) && (e.X <= right && e.Y <= bottom))
				{
					isDrag = true;
					sP = e.Location;
					eP = e.Location;
				}
			}

		}
		void PbConvertedMouseMove(object sender, MouseEventArgs e)
		{
			if (isDrag)
			{
				//erase old rectangle
				ControlPaint.DrawReversibleFrame(theRectangle, this.BackColor, FrameStyle.Dashed);

				eP = e.Location;

				//limit eP to image rectangle
				int left = (PbConverted.Width - PbConverted.Image.Width) / 2;
				int top = (PbConverted.Height - PbConverted.Image.Height) / 2;
				int right = PbConverted.Width - left;
				int bottom = PbConverted.Height - top;
				eP.X = Math.Min(Math.Max(eP.X, left), right);
				eP.Y = Math.Min(Math.Max(eP.Y, top), bottom);

				theRectangle = new Rectangle(PbConverted.PointToScreen(sP), new Size(eP.X - sP.X, eP.Y - sP.Y));

				// Draw the new rectangle by calling DrawReversibleFrame
				ControlPaint.DrawReversibleFrame(theRectangle, this.BackColor, FrameStyle.Dashed);
			}
		}

		void PbConvertedMouseUp(object sender, MouseEventArgs e)
		{
			// If the MouseUp event occurs, the user is not dragging.
			if (isDrag)
			{
				isDrag = false;

				//erase old rectangle
				ControlPaint.DrawReversibleFrame(theRectangle, this.BackColor, FrameStyle.Dashed);


				int left = (PbConverted.Width - PbConverted.Image.Width) / 2;
				int top = (PbConverted.Height - PbConverted.Image.Height) / 2;

				Rectangle CropRect = new Rectangle(Math.Min(sP.X, eP.X) - left,
													 Math.Min(sP.Y, eP.Y) - top,
													 Math.Abs(eP.X - sP.X),
													 Math.Abs(eP.Y - sP.Y));

				//Rectangle CropRect = new Rectangle(p.X-left, p.Y-top, orientedRect.Width, orientedRect.Height);

				IP.CropImage(CropRect, PbConverted.Image.Size);

				//PbOriginal.Image = IP.Original;

				// Reset the rectangle.
				theRectangle = new Rectangle(0, 0, 0, 0);
				Cropping = false;
				Cursor.Clip = new Rectangle();
				UpdateCropping();
			}
		}

		bool Cropping;
		void BtnCropClick(object sender, EventArgs e)
		{
			Cropping = !Cropping;
			UpdateCropping();
		}

		void UpdateCropping()
		{
			if (Cropping)
				BtnCrop.BackColor = Color.Orange;
			else
				BtnCrop.BackColor = DefaultBackColor;
		}
		void BtnCancelClick(object sender, EventArgs e)
		{
			try
			{
				ImageProcessor P = IP;
				IP = null;
				P?.Dispose();
			}
			finally{ Close(); }
		}

		private void RbDithering_CheckedChanged(object sender, EventArgs e)
		{
			if (IP != null)
			{
				if (RbDithering.Checked)
					IP.SelectedTool = ImageProcessor.Tool.Dithering;
				RefreshVE();
			}
		}

		private void CbDownSample_CheckedChanged(object sender, EventArgs e)
		{
			if (IP != null)
			{
				IP.UseDownSampling = CbDownSample.Checked;
				UDDownSample.Enabled = CbDownSample.Checked;
			}
		}

		private void UDDownSample_ValueChanged(object sender, EventArgs e)
		{
			if (IP != null)
				IP.DownSampling = UDDownSample.Value;
		}

		private void CbOptimizeFast_CheckedChanged(object sender, EventArgs e)
		{
			if (IP != null)
			{
				IP.OptimizeFast = CbOptimizeFast.Checked;
			}
		}

		private void PbConverted_Resize(object sender, EventArgs e)
		{
			try
			{
				if (IP != null)
					IP.FormResize(GetImageSize());
			}
			catch (System.ArgumentException ex)
			{
				//Catching this exception https://github.com/arkypita/LaserGRBL/issues/1288
			}
		}

		private void CbDither_SelectedIndexChanged(object sender, EventArgs e)
		{ if (IP != null) IP.DitheringMode = (ImageTransform.DitheringMode)CbDither.SelectedItem; }

		private void BtnQualityInfo_Click(object sender, EventArgs e)
		{
			UDQuality.Value = Math.Min(UDQuality.Maximum, (decimal)ResolutionHelperForm.CreateAndShowDialog(this, mCore, (double)UDQuality.Value));
			//Tools.Utils.OpenLink(@"https://lasergrbl.com/usage/raster-image-import/setting-reliable-resolution/");
		}

		private void BtnFillingQualityInfo_Click(object sender, EventArgs e)
		{
			UDFillingQuality.Value = Math.Min(UDFillingQuality.Maximum, (decimal)ResolutionHelperForm.CreateAndShowDialog(this, mCore, (double)UDFillingQuality.Value));
			//Tools.Utils.OpenLink(@"https://lasergrbl.com/usage/raster-image-import/setting-reliable-resolution/");
		}

		private void TBWhiteClip_ValueChanged(object sender, EventArgs e)
		{ if (IP != null) IP.WhiteClip = TBWhiteClip.Value; }

		private void TBWhiteClip_MouseDown(object sender, MouseEventArgs e)
		{ if (IP != null) IP.Demo = true; }

		private void TBWhiteClip_MouseUp(object sender, MouseEventArgs e)
		{ if (IP != null) IP.Demo = false; }

		private void BtnReverse_Click(object sender, EventArgs e)
		{
			if (IP != null)
			{
				IP.Invert();
				//PbOriginal.Image = IP.Original;
			}
		}

		private void CbUseLineThreshold_CheckedChanged(object sender, EventArgs e)
		{
			if (IP != null) IP.UseLineThreshold = CbLineThreshold.Checked;
			TBLineThreshold.Enabled = CbLineThreshold.Checked;
		}

		private void CbCornerThreshold_CheckedChanged(object sender, EventArgs e)
		{
			if (IP != null) IP.UseCornerThreshold = CbCornerThreshold.Checked;
			TBCornerThreshold.Enabled = CbCornerThreshold.Checked;
		}

		private void TBLineThreshold_ValueChanged(object sender, EventArgs e)
		{ if (IP != null) IP.LineThreshold = (int)TBLineThreshold.Value; }

		private void TBCornerThreshold_ValueChanged(object sender, EventArgs e)
		{ if (IP != null) IP.CornerThreshold = (int)TBCornerThreshold.Value; }

		private void TBCornerThreshold_DoubleClick(object sender, EventArgs e)
		{ TBCornerThreshold.Value = 110; }

		private void TBLineThreshold_DoubleClick(object sender, EventArgs e)
		{ TBLineThreshold.Value = 10; }

		private void CbAdaptiveQuality_CheckedChanged(object sender, EventArgs e)
		{ if (IP != null) IP.UseAdaptiveQuality = CbAdaptiveQuality.Checked; }

		private void BtnAdaptiveQualityInfo_Click(object sender, EventArgs e)
		{ Tools.Utils.OpenLink(@"https://lasergrbl.com/usage/raster-image-import/vectorization-tool/#adaptive-quality"); }

		private void BtnAutoTrim_Click(object sender, EventArgs e)
		{
			if (IP != null)
				IP.AutoTrim();
		}

		private void RbCenterline_Click(object sender, EventArgs e)
		{
			if (!Tools.OSHelper.Is64BitProcess)
			{
				MessageBox.Show(Strings.WarnCenterline64bit, Strings.WarnMessageBoxHeader, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				//RbVectorize.Checked = true;
			}
		}

		private void RbLineToLineTracing_Click(object sender, EventArgs e)
		{
			if (!supportPWM)
			{
				MessageBox.Show(Strings.WarnLine2LinePWM, Strings.WarnMessageBoxHeader, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				//RbDithering.Checked = true;
			}
		}

	}
}
