//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using CsPotrace;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;

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
        private int mFileDPI;
        private Size mFileResolution;

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
		private decimal mQuality;
		private bool mLinePreview;
		private decimal mSpotRemoval;
		private bool mUseSpotRemoval;
		private decimal mOptimize;
		private bool mUseOptimize;
		private bool mUseAdaptiveQuality;
		private decimal mSmoothing;
		private bool mUseSmootihing;
		private decimal mDownSampling;
		private bool mUseDownSampling;
		private bool mOptimizeFast;
		private Direction mDirection;
		private Direction mFillingDirection;
		private ImageTransform.DitheringMode mDithering;
		private decimal mFillingQuality;
		private bool mUseLineThreshold;
		private int mLineThreshold;
		private bool mUseCornerThreshold;
		private int mCornerThreshold;
		public bool mDemo;

		//option for gcode generator
		public SizeF TargetSize;
		public PointF TargetOffset;
		public string LaserOn;
		public string LaserOff;
		public int BorderSpeed;
		public int MarkSpeed;
		public int MinPower;
		public int MaxPower;

		private string mFileName;
		private bool mAppend;
		GrblCore mCore;

		private ImageProcessor Current; 		//current instance of processor thread/class - used to call abort
		Thread TH;								//processing thread
		protected ManualResetEvent MustExit;	//exit condition


		public enum Tool
		{ 
			Line2Line,
			Dithering,
			Vectorize,
            Centerline,
			NoProcessing
        }

		public enum Direction
		{
			None,
			Horizontal, Vertical, Diagonal,
			NewHorizontal, NewVertical, NewDiagonal,
			NewReverseDiagonal, NewGrid, NewDiagonalGrid,
			NewCross, NewDiagonalCross,
			NewSquares,
			NewZigZag,
			NewHilbert,
			NewInsetFilling,
		}

		public ImageProcessor(GrblCore core, string fileName, Size boxSize, bool append)
		{
			mCore = core;
			mFileName = fileName;
			mAppend = append;
			mSuspended = true;
            //mOriginal = new Bitmap(fileName);

            //this double pass is needed to normalize loaded image pixelformat
            //http://stackoverflow.com/questions/2016406/converting-bitmap-pixelformats-in-c-sharp
            using (Bitmap loadedBmp = new Bitmap(fileName))
            {
                mFileDPI = (int)loadedBmp.HorizontalResolution;
                mFileResolution = loadedBmp.Size;

                using (Bitmap tmpBmp = new Bitmap(loadedBmp))
                    mOriginal = tmpBmp.Clone(new Rectangle(0, 0, tmpBmp.Width, tmpBmp.Height), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            }

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

		public void AutoTrim()
		{
			//if (rect.Width <= 0 || rect.Height <= 0)
			//	return;

			//Rectangle scaled = new Rectangle(rect.X * mOriginal.Width / rsize.Width,
			//								 rect.Y * mOriginal.Height / rsize.Height,
			//								 rect.Width * mOriginal.Width / rsize.Width,
			//								 rect.Height * mOriginal.Height / rsize.Height);

			Color bgcolor = GuessTrimColor();

			if (!bgcolor.IsEmpty)
			{
				int[] trim = new int[4];
				for (int i = 0; i < trim.Length; i++)
					trim[i] = FindLimit(bgcolor, i);
				//mode: 0 = top, 1 = bottom, 2 = left, 3 = right

				Rectangle scaled = new Rectangle(trim[2], trim[0], mOriginal.Width - trim[2] - trim[3], mOriginal.Height - trim[0] - trim[1]);

				if (scaled.Width <= 0 || scaled.Height <= 0)
					return;

				Bitmap newBmp = mOriginal.Clone(scaled, mOriginal.PixelFormat);
				Bitmap oldBmp = mOriginal;

				mOriginal = newBmp;
				oldBmp.Dispose();

				ResizeRecalc();
				Refresh();
			}
		}

		//mode: 0 = top, 1 = bottom, 2 = left, 3 = right
		private int FindLimit(Color bgcolor, int mode)
		{
			int limit = (mode == 0 || mode == 1) ? mOriginal.Height : mOriginal.Width;

			int i = 0;
			while (i < limit && !GetLineColor(mode, 1, i, bgcolor).IsEmpty)
				i++;

			return i;
		}

		private Color GuessTrimColor()
		{
			Color[] colors = new Color[4];

			for (int i = 0; i < colors.Length; i++)
				colors[i] = GetLineColor(i, 1, 0, Color.Empty);

			Color rv = Color.Empty;
			for (int i = 0; i < colors.Length; i++)
			{
				if (!colors[i].IsEmpty) //skippa i bordi non omogenei
				{
					if (rv.IsEmpty)
						rv = colors[i];
					else if (IsSimilarColor(rv, colors[i]))
						rv = ColorAVG(rv, colors[i]);
					else
						return Color.Empty;
				}
			}
			return rv;
		}

		//mode: 0 = top, 1 = bottom, 2 = left, 3 = right
		//step: numero di pixel da skippare nel test, per fare più veloci
		//check: colore da verificare, se empty verifica il primo pixel della riga/colonna
		private Color GetLineColor(int mode, int step, int line, Color check)
		{
			Color primopixel = Color.Empty;
			Color rv = Color.Empty;

			int limit = (mode == 0 || mode == 1) ? mOriginal.Width : mOriginal.Height;
			int limit2 = (mode == 0 || mode == 1) ? mOriginal.Height : mOriginal.Width;
			for (int i = 0; i < limit; i+=step)
			{
				Color pixel;

				if (mode == 0) pixel = mOriginal.GetPixel(i, line);
				else if (mode == 1) pixel = mOriginal.GetPixel(i, limit2 - 1 - line);
				else if (mode == 2) pixel = mOriginal.GetPixel(line, i);
				else pixel = mOriginal.GetPixel(limit2 - 1 - line, i); //(mode == 3)

				if (primopixel.IsEmpty)
					primopixel = pixel;

				if (rv.IsEmpty)									//il primo lo mettiamo via come valore di base per la media
					rv = pixel;
				else if (IsSimilarColor(pixel, primopixel)) //confrontiamo i successivi con il primo
					rv = ColorAVG(rv, pixel);					//li mediamo nel valore di ritorno
				else
					return Color.Empty;
			}
			return rv;
		}

		private Color ColorAVG(Color c1, Color c2)
		{
			return Color.FromArgb((c1.A + c2.A) / 2, (c1.R + c2.R) / 2, (c1.G + c2.G) / 2, (c1.B + c2.B) / 2);
		}

		private bool IsSimilarColor(Color c1, Color c2, int tolerance = 20)
		{
			return Math.Abs(c1.A - c2.A) < tolerance &&
				Math.Abs(c1.R - c2.R) < tolerance &&
				Math.Abs(c1.G - c2.G) < tolerance &&
				Math.Abs(c1.B - c2.B) < tolerance;	
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

		public void Invert()
		{
			mOriginal = ImageTransform.InvertingImage(mOriginal);
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

		public decimal Quality
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

		public bool UseAdaptiveQuality
		{
			get => mUseAdaptiveQuality;
			set => mUseAdaptiveQuality = value;
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

		public bool OptimizeFast
		{
			get { return mOptimizeFast; }
			set
			{
				if (value != mOptimizeFast)
				{
					mOptimizeFast = value;
					//Refresh();
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

		public decimal FillingQuality
		{
			get { return mFillingQuality; }
			set
			{
				if (value != mFillingQuality)
				{
					mFillingQuality = value;
					//Refresh();
				}
			}
		}

		public int LineThreshold
		{
			get { return mLineThreshold; }
			set
			{
				if (value != mLineThreshold)
				{
					mLineThreshold = value;
					Refresh();
				}
			}
		}

		public bool UseLineThreshold
		{
			get { return mUseLineThreshold; }
			set
			{
				if (value != mUseLineThreshold)
				{
					mUseLineThreshold = value;
					Refresh();
				}
			}
		}

		public int CornerThreshold
		{
			get { return mCornerThreshold; }
			set
			{
				if (value != mCornerThreshold)
				{
					mCornerThreshold = value;
					Refresh();
				}
			}
		}

		public bool UseCornerThreshold
		{
			get { return mUseCornerThreshold; }
			set
			{
				if (value != mUseCornerThreshold)
				{
					mUseCornerThreshold = value;
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
                            else if (SelectedTool == Tool.Centerline)
                                PreviewCenterline(bmp);
							else if (SelectedTool == Tool.NoProcessing)
								PreviewLineByLine(bmp);
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

		/*
		corner-always-threshold <angle-in-degrees>: if the angle at a pixel is  less than this, it is considered a corner, even if it is within  `corner-surround' pixels of another corner; default is 60.
		corner-surround <unsigned>: number of pixels on either side of a  point to consider when determining if that point is a corner;  default is 4.
		corner-threshold <angle-in-degrees>: if a pixel, its predecessor(s),  and its successor(s) meet at an angle smaller than this, it's a  corner; default is 100.
		despeckle-level <unsigned>: 0..20; default is no despeckling.
		despeckle-tightness <real>: 0.0..8.0; default is 2.0.
		imageerror-threshold <real>: subdivide fitted curves that are off by  more pixels than this; default is 2.0.
		filter-iterations <unsigned>: smooth the curve this many times  before fitting; default is 4.
		line-reversion-threshold <real>: if a spline is closer to a straight  line than this, weighted by the square of the curve length, keep it a  straight line even if it is a list with curves; default is .01.
		line-threshold <real>: if the spline is not more than this far away  from the straight line defined by its endpoints,  then output a straight line; default is 1.
		preserve-width: whether to preserve line width prior to thinning.
		remove-adjacent-corners: remove corners that are adjacent.
		tangent-surround <unsigned>: number of points on either side of a  point to consider when computing the tangent at that point; default is 3.
		*/

		//System.Text.RegularExpressions.Regex colorRegex = new System.Text.RegularExpressions.Regex("stroke:#([0-9a-fA-F]+);", System.Text.RegularExpressions.RegexOptions.Compiled);
        private void PreviewCenterline(Bitmap bmp)
        {
			try
			{
				if (MustExitTH) return;

				Svg.SvgDocument svg = Autotrace.BitmapToSvgDocument(bmp, UseCornerThreshold, CornerThreshold, UseLineThreshold, LineThreshold);

				if (MustExitTH) return;

				using (Graphics g = Graphics.FromImage(bmp))
				{
					g.FillRectangle(new SolidBrush(Color.FromArgb(180, Color.White)), g.ClipBounds);

					if (MustExitTH) return;

					GraphicsPath path = new GraphicsPath();
					svg.Draw(path);
					g.SmoothingMode = SmoothingMode.HighQuality;
					g.DrawPath(Pens.Red, path);
				}
			}
			catch (Exception ex)
			{
				using (Graphics g = Graphics.FromImage(bmp))
				{
					if (MustExitTH) return;

					g.FillRectangle(new SolidBrush(Color.FromArgb(180, Color.White)), g.ClipBounds);

					if (MustExitTH) return;

					StringFormat format = new StringFormat();
					format.LineAlignment = StringAlignment.Center;
					format.Alignment = StringAlignment.Center;

					g.DrawString(ex.Message, SystemFonts.DefaultFont, Brushes.Red, new RectangleF( 0,0, bmp.Width, bmp.Height), format);

					if (MustExitTH) return;
				}
			}
		}

        private void PreviewDithering(Bitmap bmp)
		{
			PreviewLineByLine(bmp);
		}

		public void GenerateGCode()
		{
			if (mSuspended)
				return;

			if (Current != null)
				Current.AbortThread();

			Current = (ImageProcessor)this.Clone();
			Current.GenerateGCode2();
		}

		private void GenerateGCode2()
		{
			MustExit = new ManualResetEvent(false);
			TH = new Thread(DoTrueWork);
			TH.Name = "GCode Generator";
			TH.Start();
		}

		void DoTrueWork()
		{
			try
			{
				int maxSize = Tools.OSHelper.Is64BitProcess ? 22000 * 22000 : 6000 * 7000; //on 32bit OS we have memory limit - allow Higher value on 64bit

				double filesize = TargetSize.Width * TargetSize.Height;
				double maxRes = Math.Sqrt(maxSize / filesize); //limit res if resultimg bmp size is to big
				double fres = Math.Min(maxRes, (double)FillingQuality);

				double res = 10.0;

				if (SelectedTool == Tool.Line2Line || SelectedTool == Tool.Dithering)
					res = Math.Min(maxRes, (double)Quality);
				else if (SelectedTool == Tool.Centerline)
					res = 10.0;
				else
					res = Math.Min(maxRes, GetVectorQuality(filesize, UseAdaptiveQuality));

				//System.Diagnostics.Debug.WriteLine(res);

				Size pixelSize = new Size((int)(TargetSize.Width * res), (int)(TargetSize.Height * res));


				if (SelectedTool == Tool.NoProcessing)
				{
					pixelSize = mOriginal.Size;
					fres = res = FileDPI / 25.4;
				}

				if (res > 0)
				{
					using (Bitmap bmp = CreateTarget(pixelSize))
					{
						GrblFile.L2LConf conf = new GrblFile.L2LConf();
						conf.res = res;
						conf.fres = fres;
						conf.markSpeed = MarkSpeed;
						conf.minPower = MinPower;
						conf.maxPower = MaxPower;
						conf.lOn = LaserOn;
						conf.lOff = LaserOff;


						if (SelectedTool == Tool.NoProcessing)
							conf.dir = Direction.Horizontal;
						else if (SelectedTool == Tool.Vectorize)
							conf.dir = FillingDirection;
						else
							conf.dir = LineDirection;

						conf.oX = TargetOffset.X;
						conf.oY = TargetOffset.Y;
						conf.borderSpeed = BorderSpeed;
						conf.pwm = Settings.GetObject("Support Hardware PWM", true);
						conf.firmwareType = Settings.GetObject("Firmware Type", Firmware.Grbl);

						if (SelectedTool == Tool.Line2Line || SelectedTool == Tool.Dithering || SelectedTool == Tool.NoProcessing)
							mCore.LoadedFile.LoadImageL2L(bmp, mFileName, conf, mAppend);
						else if (SelectedTool == Tool.Vectorize)
							mCore.LoadedFile.LoadImagePotrace(bmp, mFileName, UseSpotRemoval, (int)SpotRemoval, UseSmoothing, Smoothing, UseOptimize, Optimize, OptimizeFast, conf, mAppend, mCore);
						else if (SelectedTool == Tool.Centerline)
							mCore.LoadedFile.LoadImageCenterline(bmp, mFileName, UseCornerThreshold, CornerThreshold, UseLineThreshold, LineThreshold, conf, mAppend);
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

		private static double GetVectorQuality(double size, bool adaptive)
		{
			if (!adaptive) return 10.0; //compatibilità versione precedente

			//inserisce un fattore di qualità inversamente proporzionale alle dimensioni del file
			//su dimensioni output molto piccole aumenta la qualità, su dimensioni molto grandi la diminuisce (per rendere più veloce il calcolo)

			double lato = Math.Sqrt(size);
			double fqual = 255 * Math.Pow(lato, -0.5);

			fqual = Math.Min(fqual, 255);	//valore limite verso l'alto
			fqual = Math.Max(fqual, 4);		//valore limite verso il basso

			return fqual;
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
			if (SelectedTool == Tool.NoProcessing)
			{
				return ImageTransform.GrayScale(img, 0, 0, 0, 0, 1, ImageTransform.Formula.SimpleAverage);
			}
			else
			{
				using (Bitmap resized = ImageTransform.ResizeImage(img, size, false, Interpolation))
				{
					using (Bitmap grayscale = ImageTransform.GrayScale(resized, Red / 100.0F, Green / 100.0F, Blue / 100.0F, -((100 - Brightness) / 100.0F), (Contrast / 100.0F), IsGrayScale ? ImageTransform.Formula.SimpleAverage : Formula))
					{
						using (Bitmap whiten = ImageTransform.Whitenize(grayscale, mWhitePoint, false))
						{
							if (SelectedTool == Tool.Dithering)
								return ImageTransform.DitherImage(whiten, mDithering);
							else if (SelectedTool == Tool.Centerline)
							{
								//apply variable threshold (if needed) + 50% threshold (always)
								return ImageTransform.Threshold(ImageTransform.Threshold(whiten, Threshold / 100.0F, UseThreshold), 50.0F / 100.0F, true);
							}
							else
								return ImageTransform.Threshold(whiten, Threshold / 100.0F, UseThreshold);
						}
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
			if (SelectedTool == ImageProcessor.Tool.NoProcessing)
				dir = Direction.Horizontal;

			if (!MustExitTH && dir != Direction.None)
			{
				using (Graphics g = Graphics.FromImage(bmp))
				{
					g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
					if (dir == Direction.Horizontal || dir == Direction.NewHorizontal || dir == Direction.NewGrid || dir == Direction.NewCross)
					{
						int mod = dir == Direction.Horizontal ? 2 : 3;
						int alpha = SelectedTool == ImageProcessor.Tool.Dithering ? 100 : 200;
						for (int Y = 0; Y < bmp.Height && !MustExitTH; Y++)
						{
							using (Pen p = new Pen(Color.FromArgb(alpha, 255, 255, 255), 1F))
							{
								if (Y % mod == 0)
									g.DrawLine(p, 0, Y, bmp.Width, Y);
							}
						}
					}
					if (dir == Direction.Vertical || dir == Direction.NewVertical || dir == Direction.NewGrid || dir == Direction.NewCross)
					{
						int mod = dir == Direction.Vertical ? 2 : 3;
						int alpha = SelectedTool == ImageProcessor.Tool.Dithering ? 100 : 200;
						for (int X = 0; X < bmp.Width && !MustExitTH; X++)
						{
							using (Pen p = new Pen(Color.FromArgb(alpha, 255, 255, 255), 1F))
							{
								if (X % mod == 0)
									g.DrawLine(p, X, 0, X, bmp.Height);
							}
						}
					}
					if (dir == Direction.Diagonal || dir == Direction.NewDiagonal || dir == Direction.NewDiagonalGrid || dir == Direction.NewDiagonalCross || dir == Direction.NewSquares || dir == Direction.NewZigZag)
					{
						int mod = dir == Direction.Diagonal ? 3 : 5;
						int alpha = SelectedTool == ImageProcessor.Tool.Dithering ? 150 : 255;
						for (int I = 0; I < bmp.Width + bmp.Height - 1 && !MustExitTH; I++)
						{
							using (Pen p = new Pen(Color.FromArgb(alpha, 255, 255, 255), 1F))
							{
								if (I % mod == 0)
									g.DrawLine(p, 0, bmp.Height - I, I, bmp.Height);
							}
						}
					}
					if (dir == Direction.NewReverseDiagonal || dir == Direction.NewDiagonalGrid || dir == Direction.NewDiagonalCross || dir == Direction.NewSquares)
					{
						int alpha = SelectedTool == ImageProcessor.Tool.Dithering ? 150 : 255;
						for (int I = 0; I < bmp.Width + bmp.Height - 1 && !MustExitTH; I++)
						{
							using (Pen p = new Pen(Color.FromArgb(alpha, 255, 255, 255), 1F))
							{
								if (I % 5 == 0)
									g.DrawLine(p, 0, I, I, 0);
							}
						}
					}

				}
			}
		}

		private void PreviewNoProcessing(Bitmap bmp)
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
					if (dir == Direction.Horizontal || dir == Direction.NewHorizontal || dir == Direction.NewGrid || dir == Direction.NewCross)
					{
						int mod = dir == Direction.Horizontal ? 2 : 3;
						int alpha = SelectedTool == ImageProcessor.Tool.Dithering ? 100 : 200;
						for (int Y = 0; Y < bmp.Height && !MustExitTH; Y++)
						{
							using (Pen p = new Pen(Color.FromArgb(alpha, 255, 255, 255), 1F))
							{
								if (Y % mod == 0)
									g.DrawLine(p, 0, Y, bmp.Width, Y);
							}
						}
					}
					if (dir == Direction.Vertical || dir == Direction.NewVertical || dir == Direction.NewGrid || dir == Direction.NewCross)
					{
						int mod = dir == Direction.Vertical ? 2 : 3;
						int alpha = SelectedTool == ImageProcessor.Tool.Dithering ? 100 : 200;
						for (int X = 0; X < bmp.Width && !MustExitTH; X++)
						{
							using (Pen p = new Pen(Color.FromArgb(alpha, 255, 255, 255), 1F))
							{
								if (X % mod == 0)
									g.DrawLine(p, X, 0, X, bmp.Height);
							}
						}
					}
					if (dir == Direction.Diagonal || dir == Direction.NewDiagonal || dir == Direction.NewDiagonalGrid || dir == Direction.NewDiagonalCross || dir == Direction.NewSquares || dir == Direction.NewZigZag)
					{
						int mod = dir == Direction.Diagonal ? 3 : 5;
						int alpha = SelectedTool == ImageProcessor.Tool.Dithering ? 150 : 255;
						for (int I = 0; I < bmp.Width + bmp.Height - 1 && !MustExitTH; I++)
						{
							using (Pen p = new Pen(Color.FromArgb(alpha, 255, 255, 255), 1F))
							{
								if (I % mod == 0)
									g.DrawLine(p, 0, bmp.Height - I, I, bmp.Height);
							}
						}
					}
					if (dir == Direction.NewReverseDiagonal || dir == Direction.NewDiagonalGrid || dir == Direction.NewDiagonalCross || dir == Direction.NewSquares)
					{
						int alpha = SelectedTool == ImageProcessor.Tool.Dithering ? 150 : 255;
						for (int I = 0; I < bmp.Width + bmp.Height - 1 && !MustExitTH; I++)
						{
							using (Pen p = new Pen(Color.FromArgb(alpha, 255, 255, 255), 1F))
							{
								if (I % 5 == 0)
									g.DrawLine(p, 0, I, I, 0);
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

		public float WidthToHeight(float Width)
		{ return Width * mOriginal.Height / mOriginal.Width; }

		public float HeightToWidht(float Height)
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
        public Bitmap TrueOriginal { get { return mOriginal; } } //originale eventualmente croppata e ruotata
        public int FileDPI { get { return mFileDPI; } }

		
		//public Size FileResolution { get { return mFileResolution; } }
	}
}
