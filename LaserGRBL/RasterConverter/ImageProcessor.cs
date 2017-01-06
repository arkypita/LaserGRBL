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
	public class ImageProcessor
	{
		public delegate void ImageReadyDlg(Image img);
		public event ImageReadyDlg ImageReady;

		private System.ComponentModel.BackgroundWorker BW;

		private bool mGrayScale;
		
		private bool mSuspended;
		private Control mSincro;
		private Image mOriginal;
		private Size mTargetSize;
		
		private Bitmap mResized; //syncronized

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

		private InterpolationMode mInterpolation = InterpolationMode.HighQualityBicubic;
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
		
		private Tool mTool;
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

		private ImageTransform.Formula mFormula;
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

		private int mRed;
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

		private int mGreen;
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

		private int mBlue;
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

		private int mContrast;
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

		private int mBrightness;
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

		private int mThreshold;
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

		private bool mUseThreshold;
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

		private decimal mQuality;
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

		private bool mLinePreview;
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

		private decimal mSpotRemoval;
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


		private bool mUseSpotRemoval;
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

		private decimal mOptimize;
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

		private bool mUseOptimize;
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

		private decimal mSmoothing;
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

		private bool mUseSmootihing;
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

		private bool mShowDots;
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

		private bool mShowImage;
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
		
		private Direction mDirection;
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

			StopBW();
			
			//create a new bw
			BW = new BackgroundWorker();
			BW.WorkerSupportsCancellation = true;
			BW.DoWork += BW_DoWork;
			BW.RunWorkerCompleted += BW_RunWorkerCompleted;
			
			Image src;
			lock (this)
			{src = (Image)mResized.Clone();}
			
			BW.RunWorkerAsync(new object[] {BW, src});
		}
		
		public void Dispose()
		{
			Suspend();
			StopBW();
		}

		private void StopBW()
		{
			if (BW != null) //stop prev background worker
			{
				BW.RunWorkerCompleted -= BW_RunWorkerCompleted;
				BW.DoWork -= BW_DoWork;
				if (BW.IsBusy && !BW.CancellationPending)
					BW.CancelAsync();	
				BW.Dispose();
				BW = null;
			}
		}
		
		private void RiseReady(Image img)
		{
			if (ImageReady != null)
				mSincro.Invoke(ImageReady, img);
		}

		void BW_DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker cw = ((object[])e.Argument)[0] as BackgroundWorker;
			Image src = ((object[])e.Argument)[1] as Bitmap;
			
			e.Cancel = true;
			try
			{
				Bitmap bmp = ProduceBitmap(src, mTargetSize, cw); //non usare using perché poi viene assegnato al visualizzatore 

				if (!cw.CancellationPending)
				{
					if (SelectedTool == Tool.Line2Line)
						PreviewLineByLine(bmp, cw);
					else if (SelectedTool == Tool.Vectorize)
						PreviewVector(bmp, cw);

					if (!cw.CancellationPending)
					{
						e.Result = bmp;
						e.Cancel = false;
					}
				}
			}
			catch {e.Cancel = true;}
			finally {src.Dispose();}
		}
		
		public Bitmap CreateTarget(Size size)
		{
			return ProduceBitmap(mOriginal, size, null); //non usare using perché poi viene assegnato al postprocessing 
		}

		void BW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (!e.Cancelled)
				RiseReady(e.Result as Image);
			else
				Refresh();
		}

		private Bitmap ProduceBitmap(Image img, Size size, BackgroundWorker cw)
		{
			if (cw != null && cw.CancellationPending)
				return null;
							
			using (Bitmap resized = ImageTransform.ResizeImage(img, size, false, Interpolation))
			{
				if (cw != null && cw.CancellationPending)
					return null;

				using (Bitmap grayscale = ImageTransform.GrayScale(resized, Red / 100.0F, Green / 100.0F, Blue / 100.0F, -((100 - Brightness) / 100.0F), (Contrast / 100.0F), IsGrayScale ? ImageTransform.Formula.SimpleAverage : Formula))
					return ImageTransform.Threshold(grayscale, Threshold / 100.0F, UseThreshold);
			}
		}

		private void PreviewLineByLine(Bitmap bmp, BackgroundWorker cw)
		{
			if (LinePreview)
			{
				using (Graphics g = Graphics.FromImage(bmp))
				{
					g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
					
					if (LineDirection == Direction.Horizontal)
					{
						for (int Y = 0; Y < bmp.Height && !cw.CancellationPending; Y++)
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
						for (int X = 0; X < bmp.Width && !cw.CancellationPending; X++)
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
						for (int I = 0; I < bmp.Width + bmp.Height -1 && !cw.CancellationPending; I++)
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


		private void PreviewVector(Bitmap bmp, BackgroundWorker cw)
		{
			ArrayList ListOfCurveArray = new ArrayList();

			Potrace.turdsize = (int)(UseSpotRemoval ? SpotRemoval : 2);
			Potrace.alphamax = UseSmoothing ? (double)Smoothing : 1.0;
			Potrace.opttolerance = UseOptimize ? (double)Optimize : 0.2;
			Potrace.curveoptimizing = UseOptimize; //optimize the path p, replacing sequences of Bezier segments by a single segment when possible.

			bool[,] Matrix = Potrace.BitMapToBinary(bmp, 125);
			Potrace.potrace_trace(Matrix, ListOfCurveArray);

			if (!cw.CancellationPending)
			{
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
			}
			if (!cw.CancellationPending)
				DrawVector(ListOfCurveArray, bmp, cw);
		}

		private void DrawVector(ArrayList ListOfCurveArray, Bitmap bmp, BackgroundWorker cw)
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
				GraphicsPath Contour = null;
				GraphicsPath Hole = null;
				GraphicsPath Current = null;

				for (int j = 0; j < CurveArray.Count && !cw.CancellationPending; j++)
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
					for (int k = 0; k < Curves.Length && !cw.CancellationPending; k++)
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

			if (!cw.CancellationPending)
				g.DrawPath(Pens.Black, gp); //draw path

			if (ShowDots && !cw.CancellationPending)
				DrawPoints(ListOfCurveArray, bmp, cw); //draw points
		}

		private void DrawPoints(ArrayList ListOfCurveArray, Bitmap bmp, BackgroundWorker cw)
		{
			if (ListOfCurveArray == null) return;
			Graphics g = Graphics.FromImage(bmp);
			for (int i = 0; i < ListOfCurveArray.Count && !cw.CancellationPending; i++)
			{
				ArrayList CurveArray = (ArrayList)ListOfCurveArray[i];
				for (int j = 0; j < CurveArray.Count && !cw.CancellationPending; j++)
				{
					Potrace.Curve[] Curves = (Potrace.Curve[])CurveArray[j];
					for (int k = 0; k < Curves.Length && !cw.CancellationPending; k++)
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
