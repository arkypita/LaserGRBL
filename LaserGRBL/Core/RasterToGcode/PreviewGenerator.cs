using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using CsPotrace;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LaserGRBL.Core.RasterToGcode
{
	public class PreviewGenerator
	{
		private string FileName;
		public Bitmap OriginalImage;	//real original image
		private Bitmap BaseImage;		//base image (cropped or rotated)
		private Bitmap ResizedImage;	//resized for preview (smaller = faster preview)
		public bool IsGrayScale;		//image has no color
		
		public bool ShowWClipDemo;
		public bool ShowLinePreviw;

		private ColorToGrayscale BWC;
		private ConversionTool CT;
		private GrblCore C;
		private PictureBox PB;

		public PreviewGenerator(GrblCore core, System.Windows.Forms.PictureBox target, string filename)
		{
			C = core;
			FileName = filename;
			PB = target;
			
			//this double pass is needed to normalize loaded image pixelformat
			//http://stackoverflow.com/questions/2016406/converting-bitmap-pixelformats-in-c-sharp
			using (Bitmap loadedBmp = new Bitmap(filename))
				using (Bitmap tmpBmp = new Bitmap(loadedBmp))
					BaseImage = tmpBmp.Clone(new Rectangle(0, 0, tmpBmp.Width, tmpBmp.Height), PixelFormat.Format32bppArgb); 
			
			OriginalImage = BaseImage.Clone() as Bitmap;
			IsGrayScale = ImageTransform.IsGrayScaleImage(BaseImage);
		}

		public void Refresh()
		{

		}

		public void Resize()
		{
			ResizeRecalc();
			Refresh(); 
		}

		public void Dispose()
		{
			OriginalImage.Dispose();
			BaseImage.Dispose();
			ResizedImage.Dispose();
		}
		
		public void CropImage(Rectangle rect, Size rsize)
		{
			if (rect.Width <= 0 || rect.Height <= 0)
				return;
			
			Rectangle scaled = new Rectangle(rect.X * BaseImage.Width / rsize.Width,
			                                 rect.Y * BaseImage.Height / rsize.Height,
											 rect.Width * BaseImage.Width / rsize.Width,
											 rect.Height * BaseImage.Height / rsize.Height);
			
			if (scaled.Width <= 0 || scaled.Height <= 0)
				return;
			
			Bitmap newBmp = BaseImage.Clone(scaled, BaseImage.PixelFormat);
			Bitmap oldBmp = BaseImage;
		
			BaseImage = newBmp;
			oldBmp.Dispose();

			ResizeRecalc();
			Refresh();
		}
		
		public void RotateCW()
		{
			BaseImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
			ResizeRecalc();
			Refresh();
		}

		public void RotateCCW()
		{
			BaseImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
			ResizeRecalc();
			Refresh();			
		}
		
		public void FlipH()
		{
			BaseImage.RotateFlip(RotateFlipType.RotateNoneFlipY);
			ResizeRecalc();
			Refresh();
		}
		
		public void Revert()
		{
			Bitmap tmp = BaseImage;
			BaseImage = OriginalImage.Clone() as Bitmap;
			tmp.Dispose();
			
			ResizeRecalc();
			Refresh();		
		}
		
		public void FlipV()
		{
			BaseImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
			ResizeRecalc();
			Refresh();			
		}
		
		private void ResizeRecalc()
		{
			lock (this)
			{
				if (ResizedImage != null)
					ResizedImage.Dispose();
				
				ResizedImage = ImageTransform.ResizeImage(BaseImage, CalculateResizeToFit(BaseImage.Size, PB.Size), false, BWC.InterpolationMode);
			}
		}
		
		private class GeneratorThread
		{
			public delegate void OnGeneratorComplete(Bitmap result);

			private Thread TH;
			private ManualResetEvent EV;

			Bitmap SRC;
			ColorToGrayscale CTG; 
			ConversionTool CT;
			OnGeneratorComplete DLG;
			bool CLIP;
			bool LINE;
			bool GRAY;

			public GeneratorThread(Bitmap source, ColorToGrayscale ctg, ConversionTool ct, OnGeneratorComplete dlg, bool clip, bool line, bool grayscale)
			{
				SRC = (Bitmap)source.Clone();
				CTG = (ColorToGrayscale)ctg.Clone();
				CT = (ConversionTool)ct.Clone();
				DLG = dlg;
				CLIP = clip;
				LINE = line;
				GRAY = grayscale;
			}

			private bool MustExit
			{ get { return EV.WaitOne(0, false); } }

			void CreatePreview()
			{
				try
				{
					if (CLIP)
					{
						using (Bitmap bmp = ProduceWhitepointDemo(SRC, SRC.Size))
						{
							if (!MustExit) DLG(bmp);
						}
					}
					else
					{
						using (Bitmap bmp = ProduceBitmap(SRC, SRC.Size))
						{
							if (!MustExit)
							{
								if (CT is LineToLine) PreviewLineByLine(bmp);
								else if (CT is Dithering) PreviewDithering(bmp);
								else if (CT is Vectorization) PreviewVector(bmp);
							}

							if (!MustExit) DLG(bmp);
						}
					}
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex.ToString());
				}
				finally
				{
					SRC.Dispose();
				}
			}

			private void PreviewDithering(Bitmap bmp)
			{
				PreviewLineByLine(bmp);
			}

			private Bitmap ProduceBitmap(Image img, Size size)
			{
				if (CT is Vectorization && ((Vectorization)CT).DownSampling.Enabled && ((Vectorization)CT).DownSampling.Value > 1) //if downsampling
				{
					using (Image downsampled = ImageTransform.ResizeImage(img, new Size((int)(size.Width * 1 / ((Vectorization)CT).DownSampling.Value), (int)(size.Height * 1 / ((Vectorization)CT).DownSampling.Value)), false, InterpolationMode.HighQualityBicubic))
						return ProduceBitmap2(downsampled, size);
				}
				else
				{
					return ProduceBitmap2(img, size);
				}
			}

			private Bitmap ProduceWhitepointDemo(Image img, Size size)
			{
				using (Bitmap resized = ImageTransform.ResizeImage(img, size, false, CTG.InterpolationMode))
				using (Bitmap grayscale = ImageTransform.GrayScale(resized, CTG.Red / 100.0F, CTG.Green / 100.0F, CTG.Blue / 100.0F, -((100 - CTG.Brightness) / 100.0F), (CTG.Contrast / 100.0F), GRAY ? ImageTransform.Formula.SimpleAverage : CTG.Formula))
					return ImageTransform.Whitenize(grayscale, CTG.WhitePoint, true);
			}


			private Bitmap ProduceBitmap2(Image img, Size size)
			{
				using (Bitmap resized = ImageTransform.ResizeImage(img, size, false, CTG.InterpolationMode))
				{
					using (Bitmap grayscale = ImageTransform.GrayScale(resized, CTG.Red / 100.0F, CTG.Green / 100.0F, CTG.Blue / 100.0F, -((100 - CTG.Brightness) / 100.0F), (CTG.Contrast / 100.0F), GRAY ? ImageTransform.Formula.SimpleAverage : CTG.Formula))
					{
						using (Bitmap whiten = ImageTransform.Whitenize(grayscale, CTG.WhitePoint, false))
						{
							if (CT is Dithering)
								return ImageTransform.DitherImage(whiten, ((Dithering)CT).Mode);
							else
								return ImageTransform.Threshold(whiten, CTG.Threshold.Value / 100.0F, CTG.Threshold.Enabled);
						}
					}
				}
			}

			private void PreviewLineByLine(Bitmap bmp)
			{
				if (!MustExit && LINE && CT.Direction != ConversionTool.EngravingDirection.None)
				{
					using (Graphics g = Graphics.FromImage(bmp))
					{
						g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
						if (CT.Direction == ConversionTool.EngravingDirection.Horizontal)
						{
							int alpha = CT is Dithering ? 100 : 200;
							for (int Y = 0; Y < bmp.Height && !MustExit; Y++)
							{
								using (Pen p = new Pen(Color.FromArgb(alpha, 255, 255, 255), 1F))
								{
									if (Y % 2 == 0)
										g.DrawLine(p, 0, Y, bmp.Width, Y);
								}
							}
						}
						else if (CT.Direction == ConversionTool.EngravingDirection.Vertical)
						{
							int alpha = CT is Dithering ? 100 : 200;
							for (int X = 0; X < bmp.Width && !MustExit; X++)
							{
								using (Pen p = new Pen(Color.FromArgb(alpha, 255, 255, 255), 1F))
								{
									if (X % 2 == 0)
										g.DrawLine(p, X, 0, X, bmp.Height);
								}
							}
						}
						else if (CT.Direction == ConversionTool.EngravingDirection.Diagonal)
						{
							int alpha = CT is Dithering ? 150 : 255;
							for (int I = 0; I < bmp.Width + bmp.Height - 1 && !MustExit; I++)
							{
								using (Pen p = new Pen(Color.FromArgb(alpha, 255, 255, 255), 1F))
								{
									if (I % 3 == 0)
										g.DrawLine(p, 0, bmp.Height - I, I, bmp.Height);
								}
							}
						}

					}
				}
			}


			private void PreviewVector(Bitmap bmp)
			{
				Vectorization V = CT as Vectorization;

				Potrace.turdsize = (int)(V.SpotRemoval.Enabled ? V.SpotRemoval.Value : 2.0m);
				Potrace.alphamax = V.Smoothing.Enabled ? (double)V.Smoothing.Value : 0.0;
				Potrace.opttolerance = V.Optimize.Enabled ? (double)V.Optimize.Value : 0.2;
				Potrace.curveoptimizing = V.Optimize.Enabled; //optimize the path p, replacing sequences of Bezier segments by a single segment when possible.

				if (MustExit)
					return;

				List<List<CsPotrace.Curve>> plist = Potrace.PotraceTrace(bmp);

				if (MustExit)
					return;

				using (Graphics g = Graphics.FromImage(bmp))
				{
					g.Clear(Color.White); //remove original image

					using (Brush fill = new SolidBrush(Color.FromArgb(CT.Direction != ConversionTool.EngravingDirection.None ? 255 : 30, Color.Black)))
						Potrace.Export2GDIPlus(plist, g, fill, null, 1); //trace filling

					if (MustExit)
						return;

					PreviewLineByLine(bmp); //process filling with line by line preview

					if (MustExit)
						return;

					Potrace.Export2GDIPlus(plist, g, null, Pens.Red, 0); //trace borders

					if (MustExit)
						return;
				}
			}

		}

	
	


		//void DoTrueWork()
		//{
		//	try
		//	{
		//		int maxSize = 6000*7000; //testato con immagini da 600*700 con res 10ppm
		//		double maxRes = Math.Sqrt((maxSize / (TargetSize.Width * TargetSize.Height))); //limit res if resultimg bmp size is to big
		//		double res = Math.Min(maxRes, SelectedTool == PreviewGenerator.Tool.Line2Line || SelectedTool == PreviewGenerator.Tool.Dithering ? (double)Quality : 10.0); //use a fixed resolution of 10ppmm
		//		double fres = Math.Min(maxRes, FillingQuality);

		//		Size pixelSize = new Size((int)(TargetSize.Width * res), (int)(TargetSize.Height * res));
				
		//		if (res > 0)
		//		{
		//			using (Bitmap bmp = CreateTarget(pixelSize))
		//			{
		//				GrblFile.L2LConf conf = new GrblFile.L2LConf();
		//				conf.res = res;
		//				conf.fres = fres;
		//				conf.markSpeed = MarkSpeed;
		//				conf.travelSpeed = TravelSpeed;
		//				conf.minPower = MinPower;
		//				conf.maxPower = MaxPower;
		//				conf.lOn = LaserOn;
		//				conf.lOff = LaserOff;
		//				conf.dir = SelectedTool == PreviewGenerator.Tool.Vectorize ? FillingDirection : LineDirection;
		//				conf.oX = TargetOffset.X;
		//				conf.oY = TargetOffset.Y;
		//				conf.borderSpeed = BorderSpeed;
		//				conf.pwm = (bool)Settings.GetObject("Support Hardware PWM", true);

		//				if (SelectedTool == PreviewGenerator.Tool.Line2Line || SelectedTool == PreviewGenerator.Tool.Dithering)
		//					C.LoadedFile.LoadImageL2L(bmp, FileName, conf);
		//				else if (SelectedTool == PreviewGenerator.Tool.Vectorize)
		//					C.LoadedFile.LoadImagePotrace(bmp, FileName, UseSpotRemoval, (int)SpotRemoval, UseSmoothing, Smoothing, UseOptimize, Optimize, conf);
		//			}
					
		//			if (GenerationComplete != null)
		//				GenerationComplete(null);
		//		}
		//		else
		//		{
		//			if (GenerationComplete != null)
		//				GenerationComplete(new System.InvalidOperationException("Target size too big for processing!"));
		//		}
		//	}
		//	catch(Exception ex)
		//	{
		//		if (GenerationComplete != null)
		//			GenerationComplete(ex);
		//	}
		//}
		
		//private Bitmap CreateTarget(Size size)
		//{
		//	return ProduceBitmap(BaseImage, size); //non usare using perché poi viene assegnato al postprocessing 
		//}

	

		public int WidthToHeight(int Width)
		{ return Width * BaseImage.Height / BaseImage.Width; }

		public int HeightToWidht(int Height)
		{ return Height * BaseImage.Width / BaseImage.Height; }

		private static Size CalculateResizeToFit(Size imageSize, Size boxSize)
		{
			// TODO: Check for arguments (for null and <=0)
			double widthScale = boxSize.Width / (double)imageSize.Width;
			double heightScale = boxSize.Height / (double)imageSize.Height;
			double scale = Math.Min(widthScale, heightScale);
			return new Size((int)Math.Round((imageSize.Width * scale)),(int)Math.Round((imageSize.Height * scale)));
		}
	}
}
