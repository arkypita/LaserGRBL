using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace LaserGRBL.RasterConverter
{
	public partial class RasterToLaserForm : Form
	{
		GrblFile mFile;
		string mFileName;
		ImageProcessor IP;

		private RasterToLaserForm(GrblFile file, string filename)
		{
			InitializeComponent();

			mFile = file;
			mFileName = filename;

			IP = new ImageProcessor(this,  Image.FromFile(filename), PbConverted.Size);
			PbOriginal.Image = IP.Original;
			ImageProcessor.ImageReady += OnImageReady;
			ImageProcessor.ImageBegin += OnImageBegin;
			
			LblGrayscale.Visible = CbMode.Visible = !IP.IsGrayScale;
			
			CbResize.SuspendLayout();
			CbResize.Items.Add(InterpolationMode.HighQualityBicubic);
			CbResize.Items.Add(InterpolationMode.NearestNeighbor);
			CbResize.ResumeLayout();
			CbMode.SuspendLayout();
			foreach (ImageTransform.Formula formula in Enum.GetValues(typeof(ImageTransform.Formula)))
				CbMode.Items.Add(formula);
			CbMode.ResumeLayout();
			CbDirections.SuspendLayout();
			foreach (ImageProcessor.Direction direction in Enum.GetValues(typeof(ImageProcessor.Direction)))
				CbDirections.Items.Add(direction);
			CbDirections.ResumeLayout();
			
			IISizeW.CurrentValue = 50;
			IISizeH.CurrentValue = IP.WidthToHeight(50);
			
			LoadSettings();
		}
		
		void OnImageBegin()
		{
			WT.Enabled = true;
		}
		void OnImageReady(Image img)
		{
			Image old = PbConverted.Image;
			PbConverted.Image = img.Clone() as Image;
			if (old != null)
				old.Dispose();
			WT.Enabled = false;
			WB.Visible = false;
			WB.Running = false;
		}
		
		void WTTick(object sender, EventArgs e)
		{
			WT.Enabled = false;
			WB.Visible = true;
			WB.Running = true;
		}
		
		internal static void CreateAndShowDialog(GrblFile file, string filename)
		{
			RasterToLaserForm f = new RasterToLaserForm(file, filename);
			f.ShowDialog();
			f.Dispose();
		}

		void GoodInput(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
				e.Handled = true;
		}

		void BtnCreateClick(object sender, EventArgs e)
		{
			Cursor = Cursors.WaitCursor;
			SuspendLayout();
			TCOriginalPreview.SelectedIndex = 0;
			TCOptions.Enabled = false;
			FlipControl.Enabled = false;
			BtnCreate.Enabled = false;
			WB.Visible = true;
			WB.Running = true;
			ResumeLayout();
	
			StoreSettings();

			BackgroundWorker BW = new BackgroundWorker();
			BW.DoWork += GenerateCode;
			BW.RunWorkerCompleted += GenerateComplete;
			
			BW.RunWorkerAsync(new object[] {IISizeW.CurrentValue,
						                  	IISizeH.CurrentValue,
						                  	IIOffsetX.CurrentValue,
						                  	IIOffsetY.CurrentValue,
						                  	IIMarkSpeed.CurrentValue,
						                  	IITravelSpeed.CurrentValue,
						                  	IIMinPower.CurrentValue,
						                  	IIMaxPower.CurrentValue,
						                  	TxtLaserOn.Text,
						                  	TxtLaserOff.Text,
						                  	IP.Clone()});
		}

		void GenerateCode(object sender, DoWorkEventArgs e)
		{
			int W = (int)((object[])e.Argument)[0];
			int H = (int)((object[])e.Argument)[1];
			int oX = (int)((object[])e.Argument)[2];
			int oY = (int)((object[])e.Argument)[3];
			int mS = (int)((object[])e.Argument)[4];
			int tS = (int)((object[])e.Argument)[5];
			int minP = (int)((object[])e.Argument)[6];
			int maxP = (int)((object[])e.Argument)[7];
			string lOn = (string)((object[])e.Argument)[8];
			string lOff = (string)((object[])e.Argument)[9];
			ImageProcessor opt = (ImageProcessor)((object[])e.Argument)[10];
			
			if (opt.SelectedTool == ImageProcessor.Tool.Line2Line)
			{
				using (Bitmap bmp = opt.CreateTarget(new Size(W * (int)opt.Quality, H * (int)opt.Quality)))
					mFile.LoadImageL2L(bmp, mFileName, (int)opt.Quality, oX, oY, mS, tS, minP, maxP, lOn, lOff, opt.LineDirection);
			}
			else if (RbVectorize.Checked)
			{
				//scale the image to be about 1000px wide
				double potraceRes = 1000.0 / W ;
				Size pixelSize = new Size((int)(W * potraceRes), (int)(H * potraceRes));
				
				using (Bitmap bmp = opt.CreateTarget(pixelSize))
					mFile.LoadImagePotrace(bmp, mFileName, oX, oY, mS, tS, minP, maxP, lOn, lOff, opt.UseSpotRemoval, (int)opt.SpotRemoval, opt.UseSmoothing, opt.Smoothing, opt.UseOptimize, opt.Optimize, potraceRes);
			}

		}

		void GenerateComplete(object sender, RunWorkerCompletedEventArgs e)
		{
			Cursor = Cursors.Default;
			Close();
		}

		private void StoreSettings()
		{
			Settings.SetObject("GrayScaleConversion.RasterConversionTool", RbLineToLineTracing.Checked ? ImageProcessor.Tool.Line2Line : ImageProcessor.Tool.Vectorize);

			
			Settings.SetObject("GrayScaleConversion.Line2LineOptions.Direction", (ImageProcessor.Direction)CbDirections.SelectedItem);
			Settings.SetObject("GrayScaleConversion.Line2LineOptions.Quality", UDQuality.Value);
			Settings.SetObject("GrayScaleConversion.Line2LineOptions.Preview", CbLinePreview.Checked);

			Settings.SetObject("GrayScaleConversion.VectorizeOptions.SpotRemoval.Enabled", CbSpotRemoval.Checked);
			Settings.SetObject("GrayScaleConversion.VectorizeOptions.SpotRemoval.Value", UDSpotRemoval.Value);
			Settings.SetObject("GrayScaleConversion.VectorizeOptions.Smooting.Enabled", CbSmoothing.Checked);
			Settings.SetObject("GrayScaleConversion.VectorizeOptions.Smooting.Value", UDSmoothing.Value);
			Settings.SetObject("GrayScaleConversion.VectorizeOptions.Optimize.Enabled", CbOptimize.Checked);
			Settings.SetObject("GrayScaleConversion.VectorizeOptions.Optimize.Value", UDOptimize.Value);
			Settings.SetObject("GrayScaleConversion.VectorizeOptions.ShowDots.Enabled", CbShowDots.Checked);
			Settings.SetObject("GrayScaleConversion.VectorizeOptions.ShowImage.Enabled", CbShowImage.Checked);

			Settings.SetObject("GrayScaleConversion.Parameters.Interpolation", (InterpolationMode)CbResize.SelectedItem);
			Settings.SetObject("GrayScaleConversion.Parameters.Mode", (ImageTransform.Formula)CbMode.SelectedItem);
			Settings.SetObject("GrayScaleConversion.Parameters.R", TBRed.Value);
			Settings.SetObject("GrayScaleConversion.Parameters.G", TBGreen.Value);
			Settings.SetObject("GrayScaleConversion.Parameters.B", TBBlue.Value);
			Settings.SetObject("GrayScaleConversion.Parameters.Brightness", TbBright.Value);
			Settings.SetObject("GrayScaleConversion.Parameters.Contrast", TbContrast.Value);
			Settings.SetObject("GrayScaleConversion.Parameters.Threshold.Enabled", CbThreshold.Checked);
			Settings.SetObject("GrayScaleConversion.Parameters.Threshold.Value", TbThreshold.Value);

			Settings.SetObject("GrayScaleConversion.Gcode.Speed.Mark", IIMarkSpeed.CurrentValue);
			Settings.SetObject("GrayScaleConversion.Gcode.Speed.Travel", IITravelSpeed.CurrentValue);

			Settings.SetObject("GrayScaleConversion.Gcode.LaserOptions.LaserOn", TxtLaserOn.Text);
			Settings.SetObject("GrayScaleConversion.Gcode.LaserOptions.LaserOff", TxtLaserOff.Text);
			Settings.SetObject("GrayScaleConversion.Gcode.LaserOptions.PowerMin", IIMinPower.CurrentValue);
			Settings.SetObject("GrayScaleConversion.Gcode.LaserOptions.PowerMax", IIMaxPower.CurrentValue);

			Settings.Save(); // Saves settings in application configuration file
		}

		private void LoadSettings()
		{
			if ((IP.SelectedTool = (ImageProcessor.Tool)Settings.GetObject("GrayScaleConversion.RasterConversionTool", ImageProcessor.Tool.Line2Line)) == ImageProcessor.Tool.Line2Line)
				RbLineToLineTracing.Checked = true;
			else
				RbVectorize.Checked = true;

			CbDirections.SelectedItem = (ImageProcessor.Direction)Settings.GetObject("GrayScaleConversion.Line2LineOptions.Direction", ImageProcessor.Direction.Horizontal);
			UDQuality.Value = IP.Quality = (decimal)Settings.GetObject("GrayScaleConversion.Line2LineOptions.Quality", 3m);
			CbLinePreview.Checked = IP.LinePreview = (bool)Settings.GetObject("GrayScaleConversion.Line2LineOptions.Preview", false);

			CbSpotRemoval.Checked = IP.UseSpotRemoval = (bool)Settings.GetObject("GrayScaleConversion.VectorizeOptions.SpotRemoval.Enabled", false);
			UDSpotRemoval.Value = IP.SpotRemoval = (decimal)Settings.GetObject("GrayScaleConversion.VectorizeOptions.SpotRemoval.Value", 2.0m);
			CbSmoothing.Checked = IP.UseSmoothing = (bool)Settings.GetObject("GrayScaleConversion.VectorizeOptions.Smooting.Enabled", false);
			UDSmoothing.Value = IP.Smoothing = (decimal)Settings.GetObject("GrayScaleConversion.VectorizeOptions.Smooting.Value", 1.0m);
			CbOptimize.Checked = IP.UseOptimize = (bool)Settings.GetObject("GrayScaleConversion.VectorizeOptions.Optimize.Enabled", false);
			UDOptimize.Value = IP.Optimize = (decimal)Settings.GetObject("GrayScaleConversion.VectorizeOptions.Optimize.Value", 0.2m);
			CbShowDots.Checked = IP.ShowDots = (bool)Settings.GetObject("GrayScaleConversion.VectorizeOptions.ShowDots.Enabled", false);
			CbShowImage.Checked = IP.ShowImage = (bool)Settings.GetObject("GrayScaleConversion.VectorizeOptions.ShowImage.Enabled", true);

			CbResize.SelectedItem = IP.Interpolation = (InterpolationMode)Settings.GetObject("GrayScaleConversion.Parameters.Interpolation", InterpolationMode.HighQualityBicubic);
			CbMode.SelectedItem = IP.Formula = (ImageTransform.Formula)Settings.GetObject("GrayScaleConversion.Parameters.Mode", ImageTransform.Formula.SimpleAverage);
			TBRed.Value = IP.Red = (int)Settings.GetObject("GrayScaleConversion.Parameters.R", 100);
			TBGreen.Value = IP.Green = (int)Settings.GetObject("GrayScaleConversion.Parameters.G", 100);
			TBBlue.Value = IP.Blue = (int)Settings.GetObject("GrayScaleConversion.Parameters.B", 100);
			TbBright.Value = IP.Brightness = (int)Settings.GetObject("GrayScaleConversion.Parameters.Brightness", 100);
			TbContrast.Value = IP.Contrast = (int)Settings.GetObject("GrayScaleConversion.Parameters.Contrast", 100);
			CbThreshold.Checked = IP.UseThreshold = (bool)Settings.GetObject("GrayScaleConversion.Parameters.Threshold.Enabled", false);
			TbThreshold.Value = IP.Threshold = (int)Settings.GetObject("GrayScaleConversion.Parameters.Threshold.Value", 50);

			IIMarkSpeed.CurrentValue = (int)Settings.GetObject("GrayScaleConversion.Gcode.Speed.Mark", 1000);
			IITravelSpeed.CurrentValue = (int)Settings.GetObject("GrayScaleConversion.Gcode.Speed.Travel", 4000);

			TxtLaserOn.Text = (string)Settings.GetObject("GrayScaleConversion.Gcode.LaserOptions.LaserOn", "M3");
			TxtLaserOff.Text = (string)Settings.GetObject("GrayScaleConversion.Gcode.LaserOptions.LaserOff", "M5");
			IIMinPower.CurrentValue = (int)Settings.GetObject("GrayScaleConversion.Gcode.LaserOptions.PowerMin", 0);
			IIMaxPower.CurrentValue = (int)Settings.GetObject("GrayScaleConversion.Gcode.LaserOptions.PowerMax", 255);
		}

		private void IISizeW_CurrentValueChanged(object sender, int NewValue, bool ByUser)
		{
			if (ByUser)
				IISizeH.CurrentValue = IP.WidthToHeight(NewValue);
		}

		private void IISizeH_CurrentValueChanged(object sender, int NewValue, bool ByUser)
		{
			if (ByUser)
				IISizeW.CurrentValue = IP.HeightToWidht(NewValue);
		}

		void OnRGBCBDoubleClick(object sender, EventArgs e)
		{((UserControls.ColorSlider)sender).Value = 100;}

		void OnThresholdDoubleClick(object sender, EventArgs e)
		{((UserControls.ColorSlider)sender).Value = 50;}

		private void CbMode_SelectedIndexChanged(object sender, EventArgs e)
		{ 
			IP.Formula = (ImageTransform.Formula)CbMode.SelectedItem;

			SuspendLayout();
			TBRed.Visible = TBGreen.Visible = TBBlue.Visible = (IP.Formula == ImageTransform.Formula.Custom && !IP.IsGrayScale);
			LblRed.Visible = LblGreen.Visible = LblBlue.Visible = (IP.Formula == ImageTransform.Formula.Custom && !IP.IsGrayScale);
			ResumeLayout();
		}

		private void TBRed_ValueChanged(object sender, EventArgs e)
		{ IP.Red = TBRed.Value; }

		private void TBGreen_ValueChanged(object sender, EventArgs e)
		{ IP.Green = TBGreen.Value; }

		private void TBBlue_ValueChanged(object sender, EventArgs e)
		{ IP.Blue = TBBlue.Value; }

		private void TbBright_ValueChanged(object sender, EventArgs e)
		{ IP.Brightness = TbBright.Value; }

		private void TbContrast_ValueChanged(object sender, EventArgs e)
		{ IP.Contrast = TbContrast.Value; }

		private void CbThreshold_CheckedChanged(object sender, EventArgs e)
		{ 
			IP.UseThreshold = CbThreshold.Checked;
			TbThreshold.Visible = CbThreshold.Checked;
			LbThreshold.Visible = CbThreshold.Checked;
		}

		private void TbThreshold_ValueChanged(object sender, EventArgs e)
		{ IP.Threshold = TbThreshold.Value; }

		private void RbLineToLineTracing_CheckedChanged(object sender, EventArgs e)
		{
			if (RbLineToLineTracing.Checked)
				IP.SelectedTool = ImageProcessor.Tool.Line2Line;
			GbLineToLineOptions.Visible = RbLineToLineTracing.Checked;
		}

		private void RbVectorize_CheckedChanged(object sender, EventArgs e)
		{
			if (RbVectorize.Checked)
				IP.SelectedTool = ImageProcessor.Tool.Vectorize;
			GbVectorizeOptions.Visible = RbVectorize.Checked;
		}

		private void UDQuality_ValueChanged(object sender, EventArgs e)
		{ IP.Quality = (int)UDQuality.Value; }

		private void CbLinePreview_CheckedChanged(object sender, EventArgs e)
		{ IP.LinePreview = CbLinePreview.Checked; }

		private void UDSpotRemoval_ValueChanged(object sender, EventArgs e)
		{ IP.SpotRemoval = (int)UDSpotRemoval.Value; }

		private void CbSpotRemoval_CheckedChanged(object sender, EventArgs e)
		{ 
			IP.UseSpotRemoval = CbSpotRemoval.Checked;
			UDSpotRemoval.Enabled = CbSpotRemoval.Checked;
		}

		private void UDSmoothing_ValueChanged(object sender, EventArgs e)
		{ IP.Smoothing = UDSmoothing.Value; }

		private void CbSmoothing_CheckedChanged(object sender, EventArgs e)
		{ 
			IP.UseSmoothing = CbSmoothing.Checked;
			UDSmoothing.Enabled = CbSmoothing.Checked;
		}

		private void UDOptimize_ValueChanged(object sender, EventArgs e)
		{ IP.Optimize = UDOptimize.Value; }

		private void CbOptimize_CheckedChanged(object sender, EventArgs e)
		{
			IP.UseOptimize = CbOptimize.Checked;
			UDOptimize.Enabled = CbOptimize.Checked;
		}

		private void CbShowDots_CheckedChanged(object sender, EventArgs e)
		{ IP.ShowDots = CbShowDots.Checked; }

		private void CbShowImage_CheckedChanged(object sender, EventArgs e)
		{ IP.ShowImage = CbShowImage.Checked; }

		private void RasterToLaserForm_Load(object sender, EventArgs e)
		{IP.Resume();}
		
		void RasterToLaserFormFormClosing(object sender, FormClosingEventArgs e)
		{
			IP.Suspend();
		}

		void CbDirectionsSelectedIndexChanged(object sender, EventArgs e)
		{ IP.LineDirection = (ImageProcessor.Direction)CbDirections.SelectedItem; }

		void CbResizeSelectedIndexChanged(object sender, EventArgs e)
		{ 
			IP.Interpolation = (InterpolationMode)CbResize.SelectedItem;
			PbOriginal.Image = IP.Original;
		}
		void BtRotateCWClick(object sender, EventArgs e)
		{
			IP.RotateCW();
			PbOriginal.Image = IP.Original;
			
			int w = IISizeW.CurrentValue;
			int h = IISizeH.CurrentValue;
			
			IISizeW.CurrentValue = h;
			IISizeH.CurrentValue = w;
		}
		void BtRotateCCWClick(object sender, EventArgs e)
		{
			IP.RotateCCW();
			PbOriginal.Image = IP.Original;

			int w = IISizeW.CurrentValue;
			int h = IISizeH.CurrentValue;
			
			IISizeW.CurrentValue = h;
			IISizeH.CurrentValue = w;
			
		}
		void BtFlipHClick(object sender, EventArgs e)
		{
			IP.FlipH();
			PbOriginal.Image = IP.Original;
		}
		void BtFlipVClick(object sender, EventArgs e)
		{
			IP.FlipV();
			PbOriginal.Image = IP.Original;	
		}
	}
}
