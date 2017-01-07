using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using CsPotrace;
using System.Threading;

namespace LaserGRBL.RasterConverter
{
	public class ImageProcessor : ICloneable
	{
		public delegate void ImageBeginDlg();
		public static event ImageBeginDlg ImageBegin;
		
		public delegate void ImageReadyDlg(Image img);
		public static event ImageReadyDlg ImageReady;

		private Image mOriginal;
		private Bitmap mResized; //syncronized
		
		private ImageProcessor Current;
		private bool mGrayScale;
		private bool mSuspended;
		private Control mSincro;
		private Size mTargetSize;

		
		private InterpolationMode mInterpolation = InterpolationMode.HighQualityBicubic;
		private Tool mTool;
		private ImageTransform.Formula mFormula;
		private int mRed;
		private int mGreen;
		private int mBlue;
		private int mContrast;
		private int mBrightness;
		private int mThreshold;
		private bool mUseThreshold;
		private decimal mQuality;
		private bool mLinePreview;
		private decimal mSpotRemoval;
		private bool mUseSpotRemoval;
		private decimal mOptimize;
		private bool mUseOptimize;
		private decimal mSmoothing;
		private bool mUseSmootihing;
		private bool mShowDots;		
		private bool mShowImage;		
		private Direction mDirection;

		Thread TH;
		protected ManualResetEvent MustExit;
		
		public object Clone()
		{
			ImageProcessor rv = this.MemberwiseClone() as ImageProcessor;
			rv.TH = null;
			rv.MustExit = null;
			rv.mOriginal = null;
			rv.mResized = mResized.Clone() as Bitmap;
			return rv;
		}
		
		public enum Tool
		{ Line2Line, Vectorize }
		
		public enum Direction
		{ Horizontal, Vertical, Diagonal }

		public ImageProcessor(Control sincro, Image source, Size boxSize)
		{
			mSuspended = true;
			mSincro = sincro;
			mOriginal = source;
			mTargetSize = CalculateResizeToFit(source.Size, boxSize);
			
			lock (this)
			{mResized = ImageTransform.ResizeImage(mOriginal, mTargetSize, false, Interpolation);}
			
			mGrayScale = TestGrayScale(mResized);
		}
		
		public bool IsGrayScale
		{get {return mGrayScale;}}
		
		bool TestGrayScale(Bitmap bmp)
		{
			for(int x = 0; x < bmp.Width; x+=10)
			{
				for(int y = 0; y < bmp.Height; y+=10)
				{
					Color c = bmp.GetPixel(x,y);
					if (c.R != c.G || c.G != c.B)
						return false;
				}
			}
			return true;
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
			get{return mInterpolation;}
			set 
			{
				if (value != mInterpolation)
				{
					mInterpolation = value;

					lock (this)
					{
						mResized.Dispose();
						mResized = ImageTransform.ResizeImage(mOriginal, mTargetSize, false, Interpolation);
					}
					
					Refresh();
				}
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

		public decimal Quality
		{
			get { return mQuality; }
			set
			{
				if (value != mQuality)
				{
					mQuality = value;
					Refresh();
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

		public bool ShowDots
		{
			get { return mShowDots; }
			set
			{
				if (value != mShowDots)
				{
					mShowDots = value;
					Refresh();
				}
			}
		}

		public bool ShowImage
		{
			get { return mShowImage; }
			set
			{
				if (value != mShowImage)
				{
					mShowImage = value;
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
			TH = new Thread(DoWork);
			TH.Name = "Image Processor";
			TH.Start();
			RiseBegin();
		}
		
		private void AbortThread()
		{
			if ((TH != null) && TH.ThreadState != System.Threading.ThreadState.Stopped) 
			{
				MustExit.Set();

				if (!object.ReferenceEquals(System.Threading.Thread.CurrentThread, TH)) 
				{
					TH.Join(100);
					if (TH != null && TH.ThreadState != System.Threading.ThreadState.Stopped) {
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
		
		protected bool MustExitTH
		{get{return MustExit != null && MustExit.WaitOne(0, false);}}
		
		private void RiseBegin()
		{
			if (ImageBegin != null)
				mSincro.Invoke(ImageBegin);
		}
		
		private void RiseReady(Image img)
		{
			if (ImageReady != null)
				mSincro.Invoke(ImageReady, img);
		}

		void DoWork()
		{
			try
			{
				using (Bitmap bmp = ProduceBitmap(mResized, mTargetSize))
				{
					if (!MustExitTH)
					{
						if (SelectedTool == Tool.Line2Line)
							PreviewLineByLine(bmp);
						else if (SelectedTool == Tool.Vectorize)
							PreviewVector(bmp);
					}
					
					if (!MustExitTH)
						RiseReady(bmp);
				}
			}
			catch(Exception ex) 
			{
				System.Diagnostics.Debug.WriteLine(ex.ToString());
			}
			finally
			{
				mResized.Dispose();
			}
		}
		
		public Bitmap CreateTarget(Size size)
		{
			return ProduceBitmap(mOriginal, size); //non usare using perché poi viene assegnato al postprocessing 
		}

		private Bitmap ProduceBitmap(Image img, Size size)
		{
			using (Bitmap resized = ImageTransform.ResizeImage(img, size, false, Interpolation))
				using (Bitmap grayscale = ImageTransform.GrayScale(resized, Red / 100.0F, Green / 100.0F, Blue / 100.0F, -((100 - Brightness) / 100.0F), (Contrast / 100.0F), IsGrayScale ? ImageTransform.Formula.SimpleAverage : Formula))
					return ImageTransform.Threshold(grayscale, Threshold / 100.0F, UseThreshold);
		}

		private void PreviewLineByLine(Bitmap bmp)
		{
			if (LinePreview && !MustExitTH)
			{
				using (Graphics g = Graphics.FromImage(bmp))
				{
					g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
					if (LineDirection == Direction.Horizontal)
					{
						for (int Y = 0; Y < bmp.Height && !MustExitTH; Y++)
						{
							using (Pen p = new Pen(Color.FromArgb(200, 255, 255, 255), 1F))
							{
								if (Y % 2 == 0)
									g.DrawLine(p, 0, Y, bmp.Width, Y);
							}
						}
					}
					else if (LineDirection == Direction.Vertical)
					{
						for (int X = 0; X < bmp.Width && !MustExitTH; X++)
						{
							using (Pen p = new Pen(Color.FromArgb(200, 255, 255, 255), 1F))
							{
								if (X % 2 == 0)
									g.DrawLine(p, X, 0, X, bmp.Height);
							}
						}
					}
					else if (LineDirection == Direction.Diagonal)
					{
						for (int I = 0; I < bmp.Width + bmp.Height -1 && !MustExitTH ; I++)
						{
							using (Pen p = new Pen(Color.FromArgb(255, 255, 255, 255), 1F))
							{
									if (I % 3 == 0)
										g.DrawLine(p, 0, bmp.Height-I, I, bmp.Height);
							}
						}						
					}
					
				}
			}
		}


		private void PreviewVector(Bitmap bmp)
		{
			ArrayList ListOfCurveArray = new ArrayList();

			Potrace.turdsize = (int)(UseSpotRemoval ? SpotRemoval : 2);
			Potrace.alphamax = UseSmoothing ? (double)Smoothing : 1.0;
			Potrace.opttolerance = UseOptimize ? (double)Optimize : 0.2;
			Potrace.curveoptimizing = UseOptimize; //optimize the path p, replacing sequences of Bezier segments by a single segment when possible.

			bool[,] Matrix = Potrace.BitMapToBinary(bmp, 125);
			
			if (MustExitTH)
				return;
			
			Potrace.potrace_trace(Matrix, ListOfCurveArray);

			if (MustExitTH)
				return;
			
			using (Graphics g = Graphics.FromImage(bmp))
			{
				if (!ShowImage)
					g.Clear(Color.White);
				else
				{
					using (Brush b = new SolidBrush(Color.FromArgb(220, Color.White)))
						g.FillRectangle(b, 0, 0, bmp.Width, bmp.Height);
				}
			}

			if (!MustExitTH)
				DrawVector(ListOfCurveArray, bmp);
		}

		private void DrawVector(ArrayList ListOfCurveArray, Bitmap bmp)
		{
			if (ListOfCurveArray == null) return;
			Graphics g = Graphics.FromImage(bmp);

			g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
			g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;



			GraphicsPath gp = new GraphicsPath();
			for (int i = 0; i < ListOfCurveArray.Count && !MustExitTH; i++)
			{
				ArrayList CurveArray = (ArrayList)ListOfCurveArray[i];
				GraphicsPath Contour = null;
				GraphicsPath Hole = null;
				GraphicsPath Current = null;

				for (int j = 0; j < CurveArray.Count && !MustExitTH; j++)
				{
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
					for (int k = 0; k < Curves.Length && !MustExitTH; k++)
					{
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

			if(!MustExitTH)
				g.DrawPath(Pens.Black, gp); //draw path

			if (ShowDots && !MustExitTH)
				DrawPoints(ListOfCurveArray, bmp); //draw points
		}

		private void DrawPoints(ArrayList ListOfCurveArray, Bitmap bmp)
		{
			if (ListOfCurveArray == null) return;
			Graphics g = Graphics.FromImage(bmp);
			for (int i = 0; i < ListOfCurveArray.Count && !MustExitTH; i++)
			{
				ArrayList CurveArray = (ArrayList)ListOfCurveArray[i];
				for (int j = 0; j < CurveArray.Count && !MustExitTH; j++)
				{
					Potrace.Curve[] Curves = (Potrace.Curve[])CurveArray[j];
					for (int k = 0; k < Curves.Length && !MustExitTH; k++)
					{
						g.FillRectangle(Brushes.Red, (float)((Curves[k].A.X) - 0.5), (float)((Curves[k].A.Y) - 0.5), 1, 1);
					}
				}
			}
		}

		public int WidthToHeight(int Width)
		{ return Width * mOriginal.Height / mOriginal.Width; }

		public int HeightToWidht(int Height)
		{ return Height * mOriginal.Width / mOriginal.Height; }

		private static Size CalculateResizeToFit(Size imageSize, Size boxSize)
		{
			// TODO: Check for arguments (for null and <=0)
			var widthScale = boxSize.Width / (double)imageSize.Width;
			var heightScale = boxSize.Height / (double)imageSize.Height;
			var scale = Math.Min(widthScale, heightScale);
			return new Size((int)Math.Round((imageSize.Width * scale)),(int)Math.Round((imageSize.Height * scale)));
		}


		public Bitmap Original { get {return mResized;}}
	}
}
