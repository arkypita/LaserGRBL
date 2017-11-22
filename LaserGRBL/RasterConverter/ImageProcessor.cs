using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using CsPotrace;
using System.Threading;
using Cyotek.DitheringTest.Helpers;
using System.Collections.Generic;

namespace LaserGRBL.RasterConverter
{
	public class ImageProcessor : ICloneable
	{
		public delegate void PreviewBeginDlg();
		public static event PreviewBeginDlg PreviewBegin;

		public delegate void PreviewReadyDlg(Image img);
		public static event PreviewReadyDlg PreviewReady;

		public delegate void GenerationCompleteDlg(Exception ex);
		public static event GenerationCompleteDlg GenerationComplete;

		private Bitmap mTrueOriginal;	//real original image
		private Bitmap mOriginal;		//original image (cropped or rotated)
		private Bitmap mResized;		//resized for preview

		private bool mGrayScale;		//image has no color
		private bool mSuspended;		//image generator suspended for multiple property change
		private Size mBoxSize;			//size of the picturebox frame

		//options for image processing
		private InterpolationMode mInterpolation = InterpolationMode.HighQualityBicubic;
		private Tool mTool;
		private ImageTransform.Formula mFormula;
		private int mRed;
		private int mGreen;
		private int mBlue;
		private int mContrast;
		private int mWhitePoint;
		private int mBrightness;
		private int mThreshold;
		private bool mUseThreshold;
		private double mQuality;
		private bool mLinePreview;
		private decimal mSpotRemoval;
		private bool mUseSpotRemoval;
		private decimal mOptimize;
		private bool mUseOptimize;
		private decimal mSmoothing;
		private bool mUseSmootihing;
		private decimal mDownSampling;
		private bool mUseDownSampling;
		private Direction mDirection;
		private Direction mFillingDirection;
		private ImageTransform.DitheringMode mDithering;
		private double mFillingQuality;
		public bool mDemo;

		//option for gcode generator
		public Size TargetSize;
		public Point TargetOffset;
		public string LaserOn;
		public string LaserOff;
		public int TravelSpeed;
		public int BorderSpeed;
		public int MarkSpeed;
		public int MinPower;
		public int MaxPower;

		private string mFileName;
		GrblCore mCore;

		private ImageProcessor Current; 		//current instance of processor thread/class - used to call abort
		Thread TH;								//processing thread
		protected ManualResetEvent MustExit;	//exit condition


		public enum Tool
		{ Line2Line, Dithering, Vectorize }

		public enum Direction
		{ Horizontal, Vertical, Diagonal, None }

		public ImageProcessor(GrblCore core, string fileName, Size boxSize)
		{
			mCore = core;
			mFileName = fileName;
			mSuspended = true;
			//mOriginal = new Bitmap(fileName);

			//this double pass is needed to normalize loaded image pixelformat
			//http://stackoverflow.com/questions/2016406/converting-bitmap-pixelformats-in-c-sharp
			using (Bitmap loadedBmp = new Bitmap(fileName))
			using (Bitmap tmpBmp = new Bitmap(loadedBmp))
				mOriginal = tmpBmp.Clone(new Rectangle(0, 0, tmpBmp.Width, tmpBmp.Height), System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			mTrueOriginal = mOriginal.Clone() as Bitmap;

			mBoxSize = boxSize;
			ResizeRecalc();
			mGrayScale = TestGrayScale(mOriginal);
		}

		internal void FormResize(Size size)
		{
			mBoxSize = size;
			ResizeRecalc();
			Refresh();
		}

		public object Clone()
		{
			ImageProcessor rv = this.MemberwiseClone() as ImageProcessor;
			rv.TH = null;
			rv.MustExit = null;
			rv.mTrueOriginal = mTrueOriginal;
			rv.mOriginal = mOriginal;
			rv.mResized = mResized.Clone() as Bitmap;
			return rv;
		}

		public bool IsGrayScale
		{ get { return mGrayScale; } }

		bool TestGrayScale(Bitmap bmp)
		{
			int maxdiff = 0;

			for (int x = 0; x < bmp.Width; x += 10)
			{
				for (int y = 0; y < bmp.Height; y += 10)
				{
					Color c = bmp.GetPixel(x, y);
					maxdiff = Math.Max(maxdiff, Math.Abs(c.R - c.G));
					maxdiff = Math.Max(maxdiff, Math.Abs(c.G - c.B));
					maxdiff = Math.Max(maxdiff, Math.Abs(c.R - c.B));
				}
			}

			return (maxdiff < 20);
		}

		public void Dispose()
		{
			Suspend();
			if (Current != null)
				Current.AbortThread();

			mTrueOriginal.Dispose();
			mOriginal.Dispose();
			mResized.Dispose();
		}

		public void Suspend()
		{
			mSuspended = true;
		}


		public void Resume()
		{
			if (mSuspended)
			{
				mSuspended = false;
				Refresh();
			}
		}

		public InterpolationMode Interpolation
		{
			get { return mInterpolation; }
			set
			{
				if (value != mInterpolation)
				{
					mInterpolation = value;
					ResizeRecalc();
					Refresh();
				}
			}
		}

		public void CropImage(Rectangle rect, Size rsize)
		{
			if (rect.Width <= 0 || rect.Height <= 0)
				return;

			Rectangle scaled = new Rectangle(rect.X * mOriginal.Width / rsize.Width,
											 rect.Y * mOriginal.Height / rsize.Height,
											 rect.Width * mOriginal.Width / rsize.Width,
											 rect.Height * mOriginal.Height / rsize.Height);

			if (scaled.Width <= 0 || scaled.Height <= 0)
				return;

			Bitmap newBmp = mOriginal.Clone(scaled, mOriginal.PixelFormat);
			Bitmap oldBmp = mOriginal;

			mOriginal = newBmp;
			oldBmp.Dispose();

			ResizeRecalc();
			Refresh();
		}

		public void RotateCW()
		{
			mOriginal.RotateFlip(RotateFlipType.Rotate90FlipNone);
			ResizeRecalc();
			Refresh();
		}

		public void RotateCCW()
		{
			mOriginal.RotateFlip(RotateFlipType.Rotate270FlipNone);
			ResizeRecalc();
			Refresh();
		}

		public void FlipH()
		{
			mOriginal.RotateFlip(RotateFlipType.RotateNoneFlipY);
			ResizeRecalc();
			Refresh();
		}

		public void Revert()
		{
			Bitmap tmp = mOriginal;
			mOriginal = mTrueOriginal.Clone() as Bitmap;
			tmp.Dispose();

			ResizeRecalc();
			Refresh();
		}

		public void FlipV()
		{
			mOriginal.RotateFlip(RotateFlipType.RotateNoneFlipX);
			ResizeRecalc();
			Refresh();
		}

		private void ResizeRecalc()
		{
			lock (this)
			{
				if (mResized != null)
					mResized.Dispose();

				mResized = ImageTransform.ResizeImage(mOriginal, CalculateResizeToFit(mOriginal.Size, mBoxSize), false, Interpolation);
			}
		}

		public Tool SelectedTool
		{
			get { return mTool; }
			set
			{
				if (value != mTool)
				{
					mTool = value;
					Refresh();
				}
			}
		}

		public ImageTransform.Formula Formula
		{
			get { return mFormula; }
			set
			{
				if (value != mFormula)
				{
					mFormula = value;
					Refresh();
				}
			}
		}


		public ImageTransform.DitheringMode DitheringMode
		{
			get { return mDithering; }
			set
			{
				if (value != mDithering)
				{
					mDithering = value;
					Refresh();
				}
			}
		}

		public int Red
		{
			get { return mRed; }
			set
			{
				if (value != mRed)
				{
					mRed = value;
					Refresh();
				}
			}
		}

		public int Green
		{
			get { return mGreen; }
			set
			{
				if (value != mGreen)
				{
					mGreen = value;
					Refresh();
				}
			}
		}

		public int Blue
		{
			get { return mBlue; }
			set
			{
				if (value != mBlue)
				{
					mBlue = value;
					Refresh();
				}
			}
		}

		public int Contrast
		{
			get { return mContrast; }
			set
			{
				if (value != mContrast)
				{
					mContrast = value;
					Refresh();
				}
			}
		}

		public int Brightness
		{
			get { return mBrightness; }
			set
			{
				if (value != mBrightness)
				{
					mBrightness = value;
					Refresh();
				}
			}
		}

		public int WhiteClip
		{
			get { return mWhitePoint; }
			set
			{
				if (value != mWhitePoint)
				{
					mWhitePoint = value;
					Refresh();
				}
			}
		}

		public int Threshold
		{
			get { return mThreshold; }
			set
			{
				if (value != mThreshold)
				{
					mThreshold = value;
					Refresh();
				}
			}
		}

		public bool UseThreshold
		{
			get { return mUseThreshold; }
			set
			{
				if (value != mUseThreshold)
				{
					mUseThreshold = value;
					Refresh();
				}
			}
		}

		public double Quality
		{
			get { return mQuality; }
			set
			{
				if (value != mQuality)
				{
					mQuality = value;
					//Refresh();
				}
			}
		}

		public bool LinePreview
		{
			get { return mLinePreview; }
			set
			{
				if (value != mLinePreview)
				{
					mLinePreview = value;
					Refresh();
				}
			}
		}


		public decimal SpotRemoval
		{
			get { return mSpotRemoval; }
			set
			{
				if (value != mSpotRemoval)
				{
					mSpotRemoval = value;
					Refresh();
				}
			}
		}

		public bool UseSpotRemoval
		{
			get { return mUseSpotRemoval; }
			set
			{
				if (value != mUseSpotRemoval)
				{
					mUseSpotRemoval = value;
					Refresh();
				}
			}
		}

		public decimal Optimize
		{
			get { return mOptimize; }
			set
			{
				if (value != mOptimize)
				{
					mOptimize = value;
					Refresh();
				}
			}
		}

		public bool UseOptimize
		{
			get { return mUseOptimize; }
			set
			{
				if (value != mUseOptimize)
				{
					mUseOptimize = value;
					Refresh();
				}
			}
		}

		public decimal Smoothing
		{
			get { return mSmoothing; }
			set
			{
				if (value != mSmoothing)
				{
					mSmoothing = value;
					Refresh();
				}
			}
		}


		public bool UseDownSampling
		{
			get { return mUseDownSampling; }
			set
			{
				if (value != mUseDownSampling)
				{
					mUseDownSampling = value;
					Refresh();
				}
			}
		}

		public decimal DownSampling
		{
			get { return mDownSampling; }
			set
			{
				if (value != mDownSampling)
				{
					mDownSampling = value;
					Refresh();
				}
			}
		}

		public bool UseSmoothing
		{
			get { return mUseSmootihing; }
			set
			{
				if (value != mUseSmootihing)
				{
					mUseSmootihing = value;
					Refresh();
				}
			}
		}

		public Direction LineDirection
		{
			get { return mDirection; }
			set
			{
				if (value != mDirection)
				{
					mDirection = value;
					Refresh();
				}
			}
		}

		public Direction FillingDirection
		{
			get { return mFillingDirection; }
			set
			{
				if (value != mFillingDirection)
				{
					mFillingDirection = value;
					Refresh();
				}
			}
		}

		public double FillingQuality
		{
			get { return mFillingQuality; }
			set
			{
				if (value != mFillingQuality)
				{
					mFillingQuality = value;
					Refresh();
				}
			}
		}

		public bool Demo
		{
			get { return mDemo; }
			set
			{
				if (value != mDemo)
				{
					mDemo = value;
					Refresh();
				}
			}
		}

		private void Refresh()
		{
			if (mSuspended)
				return;

			if (Current != null)
				Current.AbortThread();

			Current = (ImageProcessor)this.Clone();
			Current.RunThread();
		}

		private void RunThread()
		{
			MustExit = new ManualResetEvent(false);
			TH = new Thread(CreatePreview);
			TH.Name = "Image Processor";

			if (PreviewBegin != null)
				PreviewBegin();

			TH.Start();
		}

		private void AbortThread()
		{
			if ((TH != null) && TH.ThreadState != System.Threading.ThreadState.Stopped)
			{
				MustExit.Set();

				if (!object.ReferenceEquals(System.Threading.Thread.CurrentThread, TH))
				{
					TH.Join(100);
					if (TH != null && TH.ThreadState != System.Threading.ThreadState.Stopped)
					{
						System.Diagnostics.Debug.WriteLine(string.Format("Devo forzare la terminazione del Thread '{0}'", TH.Name));
						TH.Abort();
					}
				}
				else
				{
					System.Diagnostics.Debug.WriteLine(string.Format("ATTENZIONE! Chiamata rientrante a thread stop '{0}'", TH.Name));
				}
			}

			TH = null;
			MustExit = null;
			mResized.Dispose();
		}

		private bool MustExitTH
		{ get { return MustExit != null && MustExit.WaitOne(0, false); } }

		void CreatePreview()
		{
			try
			{
				if (mDemo)
				{
					using (Bitmap bmp = ProduceWhitepointDemo(mResized, mResized.Size))
					{
						if (!MustExitTH && PreviewReady != null)
							PreviewReady(bmp);
					}
				}
				else
				{
					using (Bitmap bmp = ProduceBitmap(mResized, mResized.Size))
					{
						if (!MustExitTH)
						{
							if (SelectedTool == Tool.Line2Line)
								PreviewLineByLine(bmp);
							else if (SelectedTool == Tool.Dithering)
								PreviewDithering(bmp);
							else if (SelectedTool == Tool.Vectorize)
								PreviewVector(bmp);
						}

						if (!MustExitTH && PreviewReady != null)
							PreviewReady(bmp);
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.ToString());
			}
			finally
			{
				mResized.Dispose();
			}
		}

		private void PreviewDithering(Bitmap bmp)
		{
			PreviewLineByLine(bmp);
		}




		public void GenerateGCode()
		{
			TH = new Thread(DoTrueWork);
			TH.Name = "GCode Generator";
			TH.Start();
		}

		private static bool Is32Bit
		{ get { return IntPtr.Size == 4; } }

		void DoTrueWork()
		{
			try
			{
				int maxSize = Is32Bit ? 6000 * 7000 : 22000 * 22000; //on 32bit OS we have memory limit - allow Higher value on 64bit
				double maxRes = Math.Sqrt((maxSize / (TargetSize.Width * TargetSize.Height))); //limit res if resultimg bmp size is to big
				double res = Math.Min(maxRes, SelectedTool == ImageProcessor.Tool.Line2Line || SelectedTool == ImageProcessor.Tool.Dithering ? (double)Quality : 10.0); //use a fixed resolution of 10ppmm
				double fres = Math.Min(maxRes, FillingQuality);

				Size pixelSize = new Size((int)(TargetSize.Width * res), (int)(TargetSize.Height * res));

				if (res > 0)
				{
					using (Bitmap bmp = CreateTarget(pixelSize))
					{
						GrblFile.L2LConf conf = new GrblFile.L2LConf();
						conf.res = res;
						conf.fres = fres;
						conf.markSpeed = MarkSpeed;
						conf.travelSpeed = TravelSpeed;
						conf.minPower = MinPower;
						conf.maxPower = MaxPower;
						conf.lOn = LaserOn;
						conf.lOff = LaserOff;
						conf.dir = SelectedTool == ImageProcessor.Tool.Vectorize ? FillingDirection : LineDirection;
						conf.oX = TargetOffset.X;
						conf.oY = TargetOffset.Y;
						conf.borderSpeed = BorderSpeed;
						conf.pwm = (bool)Settings.GetObject("Support Hardware PWM", true);

						if (SelectedTool == ImageProcessor.Tool.Line2Line || SelectedTool == ImageProcessor.Tool.Dithering)
							mCore.LoadedFile.LoadImageL2L(bmp, mFileName, conf);
						else if (SelectedTool == ImageProcessor.Tool.Vectorize)
							mCore.LoadedFile.LoadImagePotrace(bmp, mFileName, UseSpotRemoval, (int)SpotRemoval, UseSmoothing, Smoothing, UseOptimize, Optimize, conf);
					}

					if (GenerationComplete != null)
						GenerationComplete(null);
				}
				else
				{
					if (GenerationComplete != null)
						GenerationComplete(new System.InvalidOperationException("Target size too big for processing!"));
				}
			}
			catch (Exception ex)
			{
				if (GenerationComplete != null)
					GenerationComplete(ex);
			}
		}

		private Bitmap CreateTarget(Size size)
		{
			return ProduceBitmap(mOriginal, size); //non usare using perché poi viene assegnato al postprocessing 
		}

		private Bitmap ProduceBitmap(Image img, Size size)
		{
			if (SelectedTool == Tool.Vectorize && UseDownSampling && DownSampling > 1) //if downsampling
			{
				using (Image downsampled = ImageTransform.ResizeImage(img, new Size((int)(size.Width * 1 / DownSampling), (int)(size.Height * 1 / DownSampling)), false, InterpolationMode.HighQualityBicubic))
					return ProduceBitmap2(downsampled, ref size);
			}
			else
			{
				return ProduceBitmap2(img, ref size);
			}
		}

		private Bitmap ProduceWhitepointDemo(Image img, Size size)
		{
			using (Bitmap resized = ImageTransform.ResizeImage(mResized, mResized.Size, false, Interpolation))
			using (Bitmap grayscale = ImageTransform.GrayScale(resized, Red / 100.0F, Green / 100.0F, Blue / 100.0F, -((100 - Brightness) / 100.0F), (Contrast / 100.0F), IsGrayScale ? ImageTransform.Formula.SimpleAverage : Formula))
				return ImageTransform.Whitenize(grayscale, mWhitePoint, true);
		}


		private Bitmap ProduceBitmap2(Image img, ref Size size)
		{
			using (Bitmap resized = ImageTransform.ResizeImage(img, size, false, Interpolation))
			{
				using (Bitmap grayscale = ImageTransform.GrayScale(resized, Red / 100.0F, Green / 100.0F, Blue / 100.0F, -((100 - Brightness) / 100.0F), (Contrast / 100.0F), IsGrayScale ? ImageTransform.Formula.SimpleAverage : Formula))
				{
					using (Bitmap whiten = ImageTransform.Whitenize(grayscale, mWhitePoint, false))
					{
						if (SelectedTool == Tool.Dithering)
							return ImageTransform.DitherImage(whiten, mDithering);
						else
							return ImageTransform.Threshold(whiten, Threshold / 100.0F, UseThreshold);
					}
				}
			}
		}

		private void PreviewLineByLine(Bitmap bmp)
		{
			Direction dir = Direction.None;
			if (SelectedTool == ImageProcessor.Tool.Line2Line && LinePreview)
				dir = LineDirection;
			if (SelectedTool == ImageProcessor.Tool.Dithering && LinePreview)
				dir = LineDirection;
			else if (SelectedTool == ImageProcessor.Tool.Vectorize && FillingDirection != Direction.None)
				dir = FillingDirection;

			if (!MustExitTH && dir != Direction.None)
			{
				using (Graphics g = Graphics.FromImage(bmp))
				{
					g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
					if (dir == Direction.Horizontal)
					{
						int alpha = SelectedTool == ImageProcessor.Tool.Dithering ? 100 : 200;
						for (int Y = 0; Y < bmp.Height && !MustExitTH; Y++)
						{
							using (Pen p = new Pen(Color.FromArgb(alpha, 255, 255, 255), 1F))
							{
								if (Y % 2 == 0)
									g.DrawLine(p, 0, Y, bmp.Width, Y);
							}
						}
					}
					else if (dir == Direction.Vertical)
					{
						int alpha = SelectedTool == ImageProcessor.Tool.Dithering ? 100 : 200;
						for (int X = 0; X < bmp.Width && !MustExitTH; X++)
						{
							using (Pen p = new Pen(Color.FromArgb(alpha, 255, 255, 255), 1F))
							{
								if (X % 2 == 0)
									g.DrawLine(p, X, 0, X, bmp.Height);
							}
						}
					}
					else if (dir == Direction.Diagonal)
					{
						int alpha = SelectedTool == ImageProcessor.Tool.Dithering ? 150 : 255;
						for (int I = 0; I < bmp.Width + bmp.Height - 1 && !MustExitTH; I++)
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
			Potrace.turdsize = (int)(UseSpotRemoval ? SpotRemoval : 2);
			Potrace.alphamax = UseSmoothing ? (double)Smoothing : 0.0;
			Potrace.opttolerance = UseOptimize ? (double)Optimize : 0.2;
			Potrace.curveoptimizing = UseOptimize; //optimize the path p, replacing sequences of Bezier segments by a single segment when possible.

			if (MustExitTH)
				return;

			List<List<CsPotrace.Curve>> plist = Potrace.PotraceTrace(bmp);

			if (MustExitTH)
				return;

			using (Graphics g = Graphics.FromImage(bmp))
			{
				g.Clear(Color.White); //remove original image

				using (Brush fill = new SolidBrush(Color.FromArgb(FillingDirection != Direction.None ? 255 : 30, Color.Black)))
					Potrace.Export2GDIPlus(plist, g, fill, null, 1); //trace filling

				if (MustExitTH)
					return;

				PreviewLineByLine(bmp); //process filling with line by line preview

				if (MustExitTH)
					return;

				Potrace.Export2GDIPlus(plist, g, null, Pens.Red, 0); //trace borders

				if (MustExitTH)
					return;
			}
		}

		public int WidthToHeight(int Width)
		{ return Width * mOriginal.Height / mOriginal.Width; }

		public int HeightToWidht(int Height)
		{ return Height * mOriginal.Width / mOriginal.Height; }

		private static Size CalculateResizeToFit(Size imageSize, Size boxSize)
		{
			// TODO: Check for arguments (for null and <=0)
			double widthScale = boxSize.Width / (double)imageSize.Width;
			double heightScale = boxSize.Height / (double)imageSize.Height;
			double scale = Math.Min(widthScale, heightScale);
			return new Size((int)Math.Round((imageSize.Width * scale)), (int)Math.Round((imageSize.Height * scale)));
		}


		public Bitmap Original { get { return mResized; } }
	}
}
