using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using CsPotrace;

namespace LaserGRBL.RasterConverter
{
	public partial class RasterToLaserForm : Form
	{
		GrblFile mFile;
		Image mOriginal;
		Image mScaled;
		Image mConverted;

		string mFileName;
		bool mNeedRefresh;

		double scaleX = 1.0; //square ratio

		enum Tool 
		{Line2Line, Vectorize}

		private RasterToLaserForm(GrblFile file, string filename)
		{
			InitializeComponent();
			LoadSettings();
			
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
			RefreshSizes();
			mNeedRefresh = true;
			Application.Idle += Application_Idle;
		}

		void Application_Idle(object sender, EventArgs e)
		{
			if (mNeedRefresh)
			{
				if (BW.IsBusy)
					BW.CancelAsync();
				else
				{
					GeneratorOptions go = new GeneratorOptions();
					go.SelectedTool = RbLineToLineTracing.Checked ? Tool.Line2Line : Tool.Vectorize;
					go.Formula = (ImageTransform.Formula)CbMode.SelectedItem;
					go.R = TBRed.Value;
					go.G = TBGreen.Value;
					go.B = TBBlue.Value;
					go.Brightness = TbBright.Value;
					go.Contrast = TbContrast.Value;
					go.Threshold = TbThreshold.Value;
					go.UseThreshold = CbThreshold.Checked;
					go.Quality = (int)UDQuality.Value;
					go.LinePreview = CbLinePreview.Checked;
					
					go.SpotRemoval = (int)UDSpotRemoval.Value;
					go.UseSpotRemoval = CbSpotRemoval.Checked;
					go.Optimize = UDOptimize.Value;
					go.UseOptimize = CbOptimize.Checked;
					go.Smoothing = UDSmoothing.Value;
					go.UseSmootihing = CbSmoothing.Checked;
					
					go.ShowDots = CbShowDots.Checked;
					go.ShowImage = CbShowImage.Checked;
					
					go.TargetSize = PbConverted.Size;
					go.SrcImage = mScaled;
					go.GeneratePreview = true;
					
					//PB.Visible = true;
					BW.RunWorkerAsync(go);
				}
			}
			mNeedRefresh = false;
		}
		
		private class GeneratorOptions
		{
			public Tool SelectedTool;
			public ImageTransform.Formula Formula;
			public int R;
			public int G;
			public int B;
			public int Contrast;
			public int Brightness;
			public int Threshold;
			public bool UseThreshold;
			public int Quality;
			public bool LinePreview;
			public int SpotRemoval;
			public bool UseSpotRemoval;
			public decimal Optimize;
			public bool UseOptimize;
			public decimal Smoothing;
			public bool UseSmootihing;
			
			public bool ShowDots;
			public bool ShowImage;
			
			public Size TargetSize;
			
			public bool GeneratePreview;
			public Image SrcImage;
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

		private Bitmap ProduceBitmap(GeneratorOptions go)
		{
 			Image src = go.SrcImage; //preview ? mScaled : mOriginal...
 			int H = go.TargetSize.Height; // preview ? mScaled.Height : IISizeH.CurrentValue * (int)UDQuality.Value;
 			int W = go.TargetSize.Width; // preview ? mScaled.Width : IISizeW.CurrentValue * (int)UDQuality.Value;

			using (Bitmap resized = ImageTransform.ResizeImage(src, W, H))
			{
				if (BW.CancellationPending)
					return null;
				using (Bitmap flat = ImageTransform.KillAlfa(resized))
				{
					
					if (BW.CancellationPending)
						return null;
					using (Bitmap grayscale = ImageTransform.GrayScale(flat, go.R / 100.0F, go.G / 100.0F, go.B / 100.0F, go.Formula))
					{
						if (BW.CancellationPending)
							return null;
						using (Bitmap brightContrast = ImageTransform.BrightnessContrast(grayscale, -((100 - go.Brightness) / 100.0F), (go.Contrast / 100.0F)))
						{
							return ImageTransform.Threshold(brightContrast, go.Threshold / 100.0F, go.UseThreshold);
						}
					}
				}
			}
		}


		private void PreviewVector(Bitmap bmp, GeneratorOptions go)
		{
			ArrayList ListOfCurveArray = new ArrayList();

			Potrace.turdsize = go.UseSpotRemoval ? go.SpotRemoval : 2;
			Potrace.alphamax = go.UseSmootihing ? (double)go.Smoothing : 1.0;
			Potrace.opttolerance = go.UseOptimize ? (double)go.Optimize : 0.2;
			Potrace.curveoptimizing = CbOptimize.Checked; //optimize the path p, replacing sequences of Bezier segments by a single segment when possible.
            
            bool[,] Matrix = Potrace.BitMapToBinary(bmp, 125);
           	Potrace.potrace_trace(Matrix, ListOfCurveArray);


			using (Graphics g = Graphics.FromImage(bmp))
			{
				if (!go.ShowImage)
					g.Clear(Color.White);
				else
				{
					using (Brush b = new SolidBrush(Color.FromArgb(240, Color.White)))
						g.FillRectangle(b, 0, 0, bmp.Width, bmp.Height);
				}
			}

           	drawVector(ListOfCurveArray, bmp, go);
          	
		}
		
		private void drawVector(ArrayList ListOfCurveArray, Bitmap bmp, GeneratorOptions go)
        {
            if (ListOfCurveArray == null) return;
            Graphics g = Graphics.FromImage(bmp);

			g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
			g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;



            GraphicsPath gp = new GraphicsPath();
            for (int i = 0; i < ListOfCurveArray.Count; i++)
            {   
                ArrayList CurveArray = (ArrayList)ListOfCurveArray[i];
                GraphicsPath Contour=null;
                GraphicsPath Hole = null;
                GraphicsPath Current=null;

                for (int j = 0; j < CurveArray.Count; j++)
                {
                	if(BW.CancellationPending)
                		return;
                	
                    if (j == 0)
                    {
                        Contour = new GraphicsPath();
                        Current = Contour;
                    }
                    else
                    {
                        
                        Hole = new GraphicsPath();
                        Current = Hole;
      
                    }
                    Potrace.Curve[] Curves = (Potrace.Curve[])CurveArray[j];
                    float factor = 1;
                    if (true)
                        factor = 1;
                    for (int k = 0; k < Curves.Length; k++)
                    {
                    	if(BW.CancellationPending)
                			return;	
                    	                	
                        if (Curves[k].Kind == Potrace.CurveKind.Bezier)
                            Current.AddBezier((float)Curves[k].A.X * factor, (float)Curves[k].A.Y * factor, (float)Curves[k].ControlPointA.X * factor, (float)Curves[k].ControlPointA.Y * factor,
                                        (float)Curves[k].ControlPointB.X * factor, (float)Curves[k].ControlPointB.Y * factor, (float)Curves[k].B.X * factor, (float)Curves[k].B.Y * factor);
                        else
                            Current.AddLine((float)Curves[k].A.X * factor, (float)Curves[k].A.Y * factor, (float)Curves[k].B.X * factor, (float)Curves[k].B.Y * factor);

                    }
                    if (j > 0) Contour.AddPath(Hole, false);
                }
                gp.AddPath(Contour, false);
            }

            g.DrawPath(Pens.Black,gp); //draw path

			if (go.ShowDots)
       	 		drawPoints(ListOfCurveArray, bmp); //draw points
        }

        private void drawPoints(ArrayList ListOfCurveArray, Bitmap bmp)
        {
            if (ListOfCurveArray == null) return;
            Graphics g = Graphics.FromImage(bmp);
            for (int i = 0; i < ListOfCurveArray.Count; i++)
            {
            	if(BW.CancellationPending)
                	return;
            	                	
                ArrayList CurveArray = (ArrayList)ListOfCurveArray[i];
                for (int j = 0; j < CurveArray.Count; j++)
                {
            		if(BW.CancellationPending)
                		return;
                    Potrace.Curve[] Curves = (Potrace.Curve[])CurveArray[j];
                   
                    float factor = 1;
                    if (true)
                        factor = 1;
                    for (int k = 0; k < Curves.Length; k++)
                    {
            			if(BW.CancellationPending)
                			return;                    	
                        g.FillRectangle(Brushes.Red, (float)((Curves[k].A.X) * factor - 0.5), (float)((Curves[k].A.Y) * factor - 0.5), 1, 1);
                    }
                }
            }
        }
		
		
		
		

		private void PreviewLineByLine(Bitmap bmp, GeneratorOptions go)
		{
			if (go.LinePreview)
			{
				using (Graphics g = Graphics.FromImage(bmp))
				{
					g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

					int mod = 2; //7 - (int)UDQuality.Value / 3;
					for (int Y = 0; Y < go.TargetSize.Height; Y++)
					{
						using (Pen mark = new Pen(Color.FromArgb(0, 255, 255, 255), 1F))
						{
							using (Pen nomark = new Pen(Color.FromArgb(255, 255, 255, 255), 1F))
							{
								if (Y % mod == 0)
									g.DrawLine(mark, 0, Y, go.TargetSize.Width, Y);
								else
									g.DrawLine(nomark, 0, Y, go.TargetSize.Width, Y);
							}
						}
					}
					
					if (BW.CancellationPending)
						return;
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
			TbThreshold.Enabled = CbThreshold.Checked;
			UDSpotRemoval.Enabled = CbSpotRemoval.Checked;
			UDSmoothing.Enabled = CbSmoothing.Checked;
			UDOptimize.Enabled = CbOptimize.Checked;
			GbLineToLineOptions.Visible = RbLineToLineTracing.Checked;
			GbVectorizeOptions.Visible = RbVectorize.Checked;
			mNeedRefresh = true;
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

			StoreSettings();
			
			if (RbLineToLineTracing.Checked)
			{
				//using (Bitmap bmp = ProduceBitmap(false))
				//	mFile.LoadImage(bmp, mFileName, (int)UDQuality.Value, IIOffsetX.CurrentValue, IIOffsetY.CurrentValue, IIMarkSpeed.CurrentValue, IITravelSpeed.CurrentValue, IIMinPower.CurrentValue, IIMaxPower.CurrentValue, TxtLaserOn.Text, TxtLaserOff.Text);
			}
			else if (RbVectorize.Checked)
			{
				System.Windows.Forms.MessageBox.Show("Not implemented yet!");
				return;
			}

			Close();
		}

		private void StoreSettings()
		{
			Settings.SetObject("GrayScaleConversion.ConversionTool", RbLineToLineTracing.Checked ? Tool.Line2Line : Tool.Vectorize);
			
			Settings.SetObject("GrayScaleConversion.Line2LineOptions.Quality", UDQuality.Value);
			Settings.SetObject("GrayScaleConversion.Line2LineOptions.Preview", CbLinePreview.Checked);
			
			Settings.SetObject("GrayScaleConversion.VectorizeOptions.SpotRemoval.Enabled", CbSpotRemoval.Checked);
			Settings.SetObject("GrayScaleConversion.VectorizeOptions.SpotRemoval.Value", UDSpotRemoval.Value);
			Settings.SetObject("GrayScaleConversion.VectorizeOptions.Smooting.Enabled", CbSmoothing.Checked);
			Settings.SetObject("GrayScaleConversion.VectorizeOptions.Smooting.Value", UDSmoothing.Value);
			Settings.SetObject("GrayScaleConversion.VectorizeOptions.Optimize.Enabled", CbOptimize.Checked);
			Settings.SetObject("GrayScaleConversion.VectorizeOptions.Optimize.Value", UDOptimize.Value);
			
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
			if ((Tool)Settings.GetObject("GrayScaleConversion.ConversionTool", Tool.Line2Line) == Tool.Line2Line)
				RbLineToLineTracing.Checked = true;
			else
				RbVectorize.Checked = true;
			
			UDQuality.Value = (decimal)Settings.GetObject("GrayScaleConversion.Line2LineOptions.Quality", 3m);
			CbLinePreview.Checked = (bool)Settings.GetObject("GrayScaleConversion.Line2LineOptions.Preview", false);
			
			CbSpotRemoval.Checked = (bool)Settings.GetObject("GrayScaleConversion.VectorizeOptions.SpotRemoval.Enabled", false);
			UDSpotRemoval.Value = (decimal)Settings.GetObject("GrayScaleConversion.VectorizeOptions.SpotRemoval.Value", 2.0m);
			CbSmoothing.Checked = (bool)Settings.GetObject("GrayScaleConversion.VectorizeOptions.Smooting.Enabled", false);
			UDSmoothing.Value = (decimal)Settings.GetObject("GrayScaleConversion.VectorizeOptions.Smooting.Value", 1.0m);
			CbOptimize.Checked = (bool)Settings.GetObject("GrayScaleConversion.VectorizeOptions.Optimize.Enabled", false);
			UDOptimize.Value = (decimal)Settings.GetObject("GrayScaleConversion.VectorizeOptions.Optimize.Value", 0.2m);
			
			CbMode.SelectedItem = (ImageTransform.Formula)Settings.GetObject("GrayScaleConversion.Parameters.Mode", ImageTransform.Formula.SimpleAverage);
			TBRed.Value = (int)Settings.GetObject("GrayScaleConversion.Parameters.R", 100);
			TBGreen.Value = (int)Settings.GetObject("GrayScaleConversion.Parameters.G", 100);
			TBBlue.Value = (int)Settings.GetObject("GrayScaleConversion.Parameters.B", 100);
			TbBright.Value = (int)Settings.GetObject("GrayScaleConversion.Parameters.Brightness", 100);
			TbContrast.Value = (int)Settings.GetObject("GrayScaleConversion.Parameters.Contrast", 100);
			CbThreshold.Checked = (bool)Settings.GetObject("GrayScaleConversion.Parameters.Threshold.Enabled", false);
			TbThreshold.Value = (int)Settings.GetObject("GrayScaleConversion.Parameters.Threshold.Value", 50);
			
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
				IISizeH.CurrentValue = (int)(NewValue / scaleX);
		}

		private void IISizeH_CurrentValueChanged(object sender, int NewValue, bool ByUser)
		{
			if (ByUser)
				IISizeW.CurrentValue = (int)(NewValue * scaleX);
		}

		private static bool IsOdd(int value)
		{ return value % 2 != 0; }
		
		void OnRGBCBDoubleClick(object sender, EventArgs e)
		{
			((UserControls.ColorSlider)sender).Value = 100;
		}
		void OnThresholdDoubleClick(object sender, EventArgs e)
		{
			((UserControls.ColorSlider)sender).Value = 50;
		}
		void BWDoWork(object sender, DoWorkEventArgs e)
		{
			GeneratorOptions go = e.Argument as GeneratorOptions;
			Bitmap bmp = ProduceBitmap(go); //non usare using perché poi viene assegnato al visualizzatore 
			
			if (!BW.CancellationPending)
			{
				if (RbLineToLineTracing.Checked)
					PreviewLineByLine(bmp, go);
				else if (RbVectorize.Checked)
					PreviewVector(bmp, go);
	
				e.Result = bmp;
			}
			else
			{
				if (bmp != null)
					bmp.Dispose();
				
				e.Result = null;
				e.Cancel = true;
			}
			
		}
		void BWProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			//PB.Value = e.ProgressPercentage;
		}
		void BWRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			//PB.Visible = false;
			
			if (!e.Cancelled)
			{
				PbConverted.SuspendLayout();
				if (mConverted != null)
					mConverted.Dispose();
				mConverted = (Bitmap)e.Result;
				PbConverted.Image = mConverted;
				PbConverted.ResumeLayout();
			}
			else
			{
				mNeedRefresh = true;
				Application_Idle(null, null);
			}
		}


	}
}
