using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Threading;

namespace LaserGRBL.UI.Forms.RasterConverter
{
	public partial class RasterToLaserForm : Form
	{
		Core.RasterToGcode.PreviewGenerator PG;
		bool preventClose;

		bool supportPWM = (bool)Settings.GetObject("Support Hardware PWM", true);
	
		private RasterToLaserForm(GrblCore core, string filename)
		{
			InitializeComponent();

			BackColor = ColorScheme.FormBackColor;
			//GbConversionTool.ForeColor = GbLineToLineOptions.ForeColor = GbParameters.ForeColor = GbVectorizeOptions.ForeColor = ForeColor = ColorScheme.FormForeColor;
			BtnCancel.BackColor = BtnCreate.BackColor = ColorScheme.FormButtonsColor;

			PG = new Core.RasterToGcode.PreviewGenerator(core, PbConverted, filename);
			GS.Start(PG.Configuration, PG.IsGrayScale);
			ST.Start(PG.Configuration);

			PbOriginal.Image = PG.OriginalImage;
			//
			
			//CbInterpolation.SuspendLayout();
			//CbInterpolation.AddItem(InterpolationMode.HighQualityBicubic);
			//CbInterpolation.AddItem(InterpolationMode.NearestNeighbor);
			//CbInterpolation.ResumeLayout();

			System.Windows.Forms.Application.Idle += Application_Idle;






			//RbLineToLineTracing.Visible = supportPWM;

			LoadSettings();
			RefreshVE();
		}


		public LaserGRBL.Core.RasterToGcode.Configuration Conf
		{ get { return PG.Configuration; } }

		//void OnPreviewBegin()
		//{
		//	preventClose = true;
				
		//	if (InvokeRequired)
		//	{
		//		Invoke(new Core.RasterToGcode.PreviewGenerator.PreviewBeginDlg(OnPreviewBegin));
		//	}
		//	else
		//	{
		//		WT.Enabled = true;
		//		BtnCreate.Enabled = false;				
		//	}
		//}
		//void OnPreviewReady(Image img)
		//{
		//	if (InvokeRequired)
		//	{
		//		Invoke(new Core.RasterToGcode.PreviewGenerator.PreviewReadyDlg(OnPreviewReady), img);
		//	}
		//	else
		//	{
		//		Image old = PbConverted.Image;
		//		PbOriginal.Image = PG.Original;
		//		PbConverted.Image = img.Clone() as Image;
		//		if (old != null)
		//			old.Dispose();
		//		WT.Enabled = false;
		//		WB.Visible = false;
		//		WB.Running = false;
		//		BtnCreate.Enabled = true;
		//		preventClose = false;
		//	}
		//}

		//void OnGenerationComplete(Exception ex)
		//{
		//	if (InvokeRequired)
		//	{
		//		BeginInvoke(new Core.RasterToGcode.PreviewGenerator.GenerationCompleteDlg(OnGenerationComplete), ex);
		//	}
		//	else
		//	{
		//		Cursor = Cursors.Default;
		//		if (ex != null)
		//			System.Windows.Forms.MessageBox.Show(ex.Message);
		//		preventClose = false;
		//		WT.Enabled = false;
		//		PG.Dispose();
		//		Close();
		//	}
		//}
		
		void WTTick(object sender, EventArgs e)
		{
			WT.Enabled = false;
			WB.Visible = true;
			WB.Running = true;
		}
		
		internal static void CreateAndShowDialog(GrblCore core, string filename, Form parent)
		{
			using (RasterToLaserForm f = new RasterToLaserForm(core, filename))
				f.ShowDialog(parent);
		}

		void GoodInput(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
				e.Handled = true;
		}

		void BtnCreateClick(object sender, EventArgs e)
		{
			using (ConvertSizeAndOptionForm f = new ConvertSizeAndOptionForm())
			{
				f.ShowDialog(PG);
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
					ResumeLayout();
			
					StoreSettings();
		
					//Core.RasterToGcode.PreviewGenerator targetProcessor = PG.Clone() as Core.RasterToGcode.PreviewGenerator;
					//PG.GenerateGCode();
				}
			}
		}

		private void StoreSettings()
		{
//			Settings.SetObject("GrayScaleConversion.RasterConversionTool", RbLineToLineTracing.Checked ? Core.RasterToGcode.PreviewGenerator.Tool.Line2Line : RbDithering.Checked ? Core.RasterToGcode.PreviewGenerator.Tool.Dithering : Core.RasterToGcode.PreviewGenerator.Tool.Vectorize);
			
//			Settings.SetObject("GrayScaleConversion.Line2LineOptions.Direction", (Core.RasterToGcode.ConversionTool.EngravingDirection)CbDirections.SelectedItem);
//			Settings.SetObject("GrayScaleConversion.Line2LineOptions.Quality", UDQuality.Value);
//			Settings.SetObject("GrayScaleConversion.Line2LineOptions.Preview", CbLinePreview.Checked);

//			Settings.SetObject("GrayScaleConversion.VectorizeOptions.SpotRemoval.Enabled", CbSpotRemoval.Checked);
//			Settings.SetObject("GrayScaleConversion.VectorizeOptions.SpotRemoval.Value", UDSpotRemoval.Value);
//			Settings.SetObject("GrayScaleConversion.VectorizeOptions.Smooting.Enabled", CbSmoothing.Checked);
//			Settings.SetObject("GrayScaleConversion.VectorizeOptions.Smooting.Value", UDSmoothing.Value);
//			Settings.SetObject("GrayScaleConversion.VectorizeOptions.Optimize.Enabled", CbOptimize.Checked);
//			Settings.SetObject("GrayScaleConversion.VectorizeOptions.Optimize.Value", UDOptimize.Value);
//			Settings.SetObject("GrayScaleConversion.VectorizeOptions.DownSample.Enabled", CbDownSample.Checked);
//			Settings.SetObject("GrayScaleConversion.VectorizeOptions.DownSample.Value", UDDownSample.Value);
////			Settings.SetObject("GrayScaleConversion.VectorizeOptions.ShowDots.Enabled", CbShowDots.Checked);
////			Settings.SetObject("GrayScaleConversion.VectorizeOptions.ShowImage.Enabled", CbShowImage.Checked);
//			Settings.SetObject("GrayScaleConversion.VectorizeOptions.FillingDirection", (Core.RasterToGcode.ConversionTool.EngravingDirection)CbFillingDirection.SelectedItem);
//			Settings.SetObject("GrayScaleConversion.VectorizeOptions.FillingQuality", UDFillingQuality.Value);

//			Settings.SetObject("GrayScaleConversion.DitheringOptions.DitheringMode", (Core.RasterToGcode.ImageTransform.DitheringMode)CbDither.SelectedItem);

//			Settings.SetObject("GrayScaleConversion.Parameters.Interpolation", (InterpolationMode)CbResize.SelectedItem);
//			Settings.SetObject("GrayScaleConversion.Parameters.Mode", (Core.RasterToGcode.ImageTransform.Formula)CbMode.SelectedItem);
//			Settings.SetObject("GrayScaleConversion.Parameters.R", TBRed.Value);
//			Settings.SetObject("GrayScaleConversion.Parameters.G", TBGreen.Value);
//			Settings.SetObject("GrayScaleConversion.Parameters.B", TBBlue.Value);
//			Settings.SetObject("GrayScaleConversion.Parameters.Brightness", TbBright.Value);
//			Settings.SetObject("GrayScaleConversion.Parameters.Contrast", TbContrast.Value);
//			Settings.SetObject("GrayScaleConversion.Parameters.Threshold.Enabled", CbThreshold.Checked);
//			Settings.SetObject("GrayScaleConversion.Parameters.Threshold.Value", TbThreshold.Value);
//			Settings.SetObject("GrayScaleConversion.Parameters.WhiteClip", TBWhiteClip.Value);

//			Settings.SetObject("GrayScaleConversion.VectorizeOptions.BorderSpeed", PG.BorderSpeed);
//			Settings.SetObject("GrayScaleConversion.Gcode.Speed.Mark", PG.MarkSpeed);
//			Settings.SetObject("GrayScaleConversion.Gcode.Speed.Travel", PG.TravelSpeed);

//			Settings.SetObject("GrayScaleConversion.Gcode.LaserOptions.LaserOn", PG.LaserOn);
//			Settings.SetObject("GrayScaleConversion.Gcode.LaserOptions.LaserOff", PG.LaserOff);
//			Settings.SetObject("GrayScaleConversion.Gcode.LaserOptions.PowerMin", PG.MinPower);
//			Settings.SetObject("GrayScaleConversion.Gcode.LaserOptions.PowerMax", PG.MaxPower);

//			Settings.SetObject("GrayScaleConversion.Gcode.Offset.X", PG.TargetOffset.X);
//			Settings.SetObject("GrayScaleConversion.Gcode.Offset.Y", PG.TargetOffset.Y);
//			Settings.SetObject("GrayScaleConversion.Gcode.BiggestDimension", Math.Max(PG.TargetSize.Width, PG.TargetSize.Height));


//			Settings.Save(); // Saves settings in application configuration file
		}

		private void LoadSettings()
		{
			//if ((PG.SelectedTool = (Core.RasterToGcode.PreviewGenerator.Tool)Settings.GetObject("GrayScaleConversion.RasterConversionTool", Core.RasterToGcode.PreviewGenerator.Tool.Line2Line)) == Core.RasterToGcode.PreviewGenerator.Tool.Line2Line)
			//	RbLineToLineTracing.Checked = true;
			//else if ((PG.SelectedTool = (Core.RasterToGcode.PreviewGenerator.Tool)Settings.GetObject("GrayScaleConversion.RasterConversionTool", Core.RasterToGcode.PreviewGenerator.Tool.Line2Line)) == Core.RasterToGcode.PreviewGenerator.Tool.Dithering)
			//	RbDithering.Checked = true;
			//else
			//	RbVectorize.Checked = true;

			//CbDirections.SelectedItem = PG.LineDirection = (Core.RasterToGcode.ConversionTool.EngravingDirection)Settings.GetObject("GrayScaleConversion.Line2LineOptions.Direction", Core.RasterToGcode.ConversionTool.EngravingDirection.Horizontal);
			//UDQuality.Value = (decimal)(PG.Quality = Convert.ToDouble(Settings.GetObject("GrayScaleConversion.Line2LineOptions.Quality", 3.0)));
			//CbLinePreview.Checked = PG.LinePreview = (bool)Settings.GetObject("GrayScaleConversion.Line2LineOptions.Preview", false);

			//CbSpotRemoval.Checked = PG.UseSpotRemoval = (bool)Settings.GetObject("GrayScaleConversion.VectorizeOptions.SpotRemoval.Enabled", false);
			//UDSpotRemoval.Value = PG.SpotRemoval = (decimal)Settings.GetObject("GrayScaleConversion.VectorizeOptions.SpotRemoval.Value", 2.0m);
			//CbSmoothing.Checked = PG.UseSmoothing = (bool)Settings.GetObject("GrayScaleConversion.VectorizeOptions.Smooting.Enabled", false);
			//UDSmoothing.Value = PG.Smoothing = (decimal)Settings.GetObject("GrayScaleConversion.VectorizeOptions.Smooting.Value", 1.0m);
			//CbOptimize.Checked = PG.UseOptimize = (bool)Settings.GetObject("GrayScaleConversion.VectorizeOptions.Optimize.Enabled", false);
			//UDOptimize.Value = PG.Optimize = (decimal)Settings.GetObject("GrayScaleConversion.VectorizeOptions.Optimize.Value", 0.2m);
			//CbDownSample.Checked = PG.UseDownSampling = (bool)Settings.GetObject("GrayScaleConversion.VectorizeOptions.DownSample.Enabled", false);
			//UDDownSample.Value = PG.DownSampling = (decimal)Settings.GetObject("GrayScaleConversion.VectorizeOptions.DownSample.Value", 2.0m);

			////CbShowDots.Checked = IP.ShowDots = (bool)Settings.GetObject("GrayScaleConversion.VectorizeOptions.ShowDots.Enabled", false);
			////CbShowImage.Checked = IP.ShowImage = (bool)Settings.GetObject("GrayScaleConversion.VectorizeOptions.ShowImage.Enabled", true);
			//CbFillingDirection.SelectedItem = PG.FillingDirection = (Core.RasterToGcode.ConversionTool.EngravingDirection)Settings.GetObject("GrayScaleConversion.VectorizeOptions.FillingDirection", Core.RasterToGcode.ConversionTool.EngravingDirection.None);
			//UDFillingQuality.Value = (decimal)(PG.FillingQuality = Convert.ToDouble(Settings.GetObject("GrayScaleConversion.VectorizeOptions.FillingQuality", 3.0)));

			//CbResize.SelectedItem = PG.Interpolation = (InterpolationMode)Settings.GetObject("GrayScaleConversion.Parameters.Interpolation", InterpolationMode.HighQualityBicubic);
			//CbMode.SelectedItem = PG.Formula = (Core.RasterToGcode.ImageTransform.Formula)Settings.GetObject("GrayScaleConversion.Parameters.Mode", Core.RasterToGcode.ImageTransform.Formula.SimpleAverage);
			//TBRed.Value = PG.Red = (int)Settings.GetObject("GrayScaleConversion.Parameters.R", 100);
			//TBGreen.Value = PG.Green = (int)Settings.GetObject("GrayScaleConversion.Parameters.G", 100);
			//TBBlue.Value = PG.Blue = (int)Settings.GetObject("GrayScaleConversion.Parameters.B", 100);
			//TbBright.Value = PG.Brightness = (int)Settings.GetObject("GrayScaleConversion.Parameters.Brightness", 100);
			//TbContrast.Value = PG.Contrast = (int)Settings.GetObject("GrayScaleConversion.Parameters.Contrast", 100);
			//CbThreshold.Checked = PG.UseThreshold = (bool)Settings.GetObject("GrayScaleConversion.Parameters.Threshold.Enabled", false);
			//TbThreshold.Value = PG.Threshold = (int)Settings.GetObject("GrayScaleConversion.Parameters.Threshold.Value", 50);
			//TBWhiteClip.Value = PG.WhiteClip = (int)Settings.GetObject("GrayScaleConversion.Parameters.WhiteClip", 5);

			//CbDither.SelectedItem = (Core.RasterToGcode.ImageTransform.DitheringMode)Settings.GetObject("GrayScaleConversion.DitheringOptions.DitheringMode", Core.RasterToGcode.ImageTransform.DitheringMode.FloydSteinberg);

			//if (RbLineToLineTracing.Checked && !supportPWM)
			//	RbDithering.Checked = true;
		}

		void OnRGBCBDoubleClick(object sender, EventArgs e)
		{((UserControls.ColorSlider)sender).Value = 100;}

		void OnThresholdDoubleClick(object sender, EventArgs e)
		{((UserControls.ColorSlider)sender).Value = 50;}

	

		private void RefreshVE()
		{
			//GbVectorizeOptions.Visible = RbVectorize.Checked;
			//GbLineToLineOptions.Visible = RbLineToLineTracing.Checked || RbDithering.Checked;
			//GbLineToLineOptions.Text = RbLineToLineTracing.Checked ? "Line To Line Options" : "Dithering Options";

			//CbThreshold.Visible = !RbDithering.Checked;
			//TbThreshold.Visible = !RbDithering.Checked && CbThreshold.Checked;

			//LblDitherMode.Visible = CbDither.Visible = RbDithering.Checked;
		}


	

	

	

		private void RasterToLaserForm_Load(object sender, EventArgs e)
		{
			PG.Start();
		}
		
		void RasterToLaserFormFormClosing(object sender, FormClosingEventArgs e)
		{
			if (preventClose)
			{
				e.Cancel = true;
			}
			else
			{
				//Core.RasterToGcode.PreviewGenerator.PreviewReady -= OnPreviewReady;
				//Core.RasterToGcode.PreviewGenerator.PreviewBegin -= OnPreviewBegin;
				//Core.RasterToGcode.PreviewGenerator.GenerationComplete -= OnGenerationComplete;
				if (PG != null) PG.Dispose();
			}
		}



		//void CbInterpolationSelectedIndexChanged(object sender, EventArgs e)
		//{
		//	Conf.ColorToGrayscale.InterpolationMode = (InterpolationMode)CbInterpolation.SelectedItem;
		//	PG.Refresh();
		//}

		void BtRotateCWClick(object sender, EventArgs e)
		{PG.RotateCW();}

		void BtRotateCCWClick(object sender, EventArgs e)
		{PG.RotateCCW();}

		void BtFlipHClick(object sender, EventArgs e)
		{PG.FlipH();}

		void BtFlipVClick(object sender, EventArgs e)
		{PG.FlipV();}
		
		void BtnRevertClick(object sender, EventArgs e)
		{PG.Revert();}

	
		
		
		bool isDrag = false;
		Rectangle imageRectangle;
  		Rectangle theRectangle = new Rectangle(new Point(0, 0), new Size(0, 0));
		Point sP;
		Point eP;
	  	
		void PbConvertedMouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button==MouseButtons.Left && Cropping)
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
				
				theRectangle = new Rectangle(PbConverted.PointToScreen(sP), new Size(eP.X-sP.X, eP.Y-sP.Y));
		
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
			                                         Math.Abs(eP.X-sP.X),
			                                         Math.Abs(eP.Y-sP.Y));
				
				//Rectangle CropRect = new Rectangle(p.X-left, p.Y-top, orientedRect.Width, orientedRect.Height);
				
				PG.CropImage(CropRect, PbConverted.Image.Size);
				
				//PbOriginal.Image = PG.Original;
				
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
			Close();
		}



	

		private void PbConverted_Resize(object sender, EventArgs e)
		{
			PG.Resize();
		}

		bool requireRefresh;
		private void OnSomeValueChanged(object sender, EventArgs e)
		{
			PG.ShowWClipDemo = GS.ClipPreview;
			PG.ShowLinePreview = CbLinePreview.Checked;
			requireRefresh = true;
		}

		void Application_Idle(object sender, EventArgs e)
		{
			if (requireRefresh)
				PG.Refresh();
			requireRefresh = false;
		}





	

	


	}
}
