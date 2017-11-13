using System;
using System.Collections.Generic;
using System.Collections;
using CsPotrace;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace LaserGRBL
{
	public class GrblFile : IEnumerable<GrblCommand>
	{
		public enum CartesianQuadrant { I, II, III, IV, Mix, Unknown }

		public delegate void OnFileLoadedDlg(long elapsed, string filename);
		public event OnFileLoadedDlg OnFileLoaded;

		private List<GrblCommand> list = new List<GrblCommand>();
		private ProgramRange mRange = new ProgramRange();
		private TimeSpan mEstimatedTotalTime;

		public GrblFile()
		{

		}

		public GrblFile(decimal x, decimal y, decimal x1, decimal y1)
		{
			mRange.UpdateXYRange(new GrblCommand.Element('X', x), new GrblCommand.Element('Y', y), false);
			mRange.UpdateXYRange(new GrblCommand.Element('X', x1), new GrblCommand.Element('Y', y1), false);
		}

		public void SaveProgram(string filename)
		{
			try
			{
				using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filename))
				{
					foreach (GrblCommand cmd in list)
						sw.WriteLine(cmd.Command);
					sw.Close();
				}
			}
			catch { }
		}

		public void LoadFile(string filename)
		{
			long start = Tools.HiResTimer.TotalMilliseconds;
			list.Clear();
			mRange.ResetRange();
			if (System.IO.File.Exists(filename))
			{
				using (System.IO.StreamReader sr = new System.IO.StreamReader(filename))
				{
					string line = null;
					while ((line = sr.ReadLine()) != null)
						if ((line = line.Trim()).Length > 0)
						{
							GrblCommand cmd = new GrblCommand(line);
							if (!cmd.IsEmpty)
								list.Add(cmd);
						}
				}
			}
			Analyze();
			long elapsed = Tools.HiResTimer.TotalMilliseconds - start;

			if (OnFileLoaded != null)
				OnFileLoaded(elapsed, filename);
		}

		private abstract class ColorSegment
		{
			protected int mColor;
			protected double mLen;
			protected bool mReverse;
			protected L2LConf mConf;

			public ColorSegment(int col, int len, bool rev, L2LConf c)
			{
				mColor = col;
				mLen = len / (c.vectorfilling ? c.fres : c.res);
				mReverse = rev;
				mConf = c;
			}

			public virtual bool IsSeparator
			{ get { return false; } }

			public bool Fast
			{ get { return mConf.pwm ? mColor == 0 : mColor <= 125; } }

			public string formatnumber(double number)
			{ return number.ToString("#.###", System.Globalization.CultureInfo.InvariantCulture); }
		}

		private class XSegment : ColorSegment
		{
			public XSegment(int col, int len, bool rev, L2LConf c) : base(col, len, rev, c) { }

			public override string ToString()
			{
				if (mConf.pwm)
					return string.Format("X{0} S{1}", formatnumber(mReverse ? -mLen : mLen), mColor);
				else
					return string.Format("X{0} {1}", formatnumber(mReverse ? -mLen : mLen), Fast ? mConf.lOff : mConf.lOn);
			}
		}

		private class YSegment : ColorSegment
		{
			public YSegment(int col, int len, bool rev, L2LConf c) : base(col, len, rev, c) { }

			public override string ToString()
			{
				if (mConf.pwm)
					return string.Format("Y{0} S{1}", formatnumber(mReverse ? -mLen : mLen), mColor);
				else
					return string.Format("Y{0} {1}", formatnumber(mReverse ? -mLen : mLen), Fast ? mConf.lOff : mConf.lOn);
			}
		}

		private class DSegment : ColorSegment
		{
			public DSegment(int col, int len, bool rev, L2LConf c) : base(col, len, rev, c) { }

			public override string ToString()
			{
				if (mConf.pwm)
					return string.Format("X{0} Y{1} S{2}", formatnumber(mReverse ? -mLen : mLen), formatnumber(mReverse ? mLen : -mLen), mColor);
				else
					return string.Format("X{0} Y{1} {2}", formatnumber(mReverse ? -mLen : mLen), formatnumber(mReverse ? mLen : -mLen), Fast ? mConf.lOff : mConf.lOn);
			}
		}

		private class VSeparator : ColorSegment
		{
			public VSeparator(L2LConf c) : base(0, 1, false, c) { }

			public override string ToString()
			{ return string.Format("Y{0}", formatnumber(mLen)); }

			public override bool IsSeparator
			{ get { return true; } }
		}

		private class HSeparator : ColorSegment
		{
			public HSeparator(L2LConf c) : base(0, 1, false, c) { }

			public override string ToString()
			{ return string.Format("X{0}", formatnumber(mLen)); }

			public override bool IsSeparator
			{ get { return true; } }
		}



		public void LoadImagePotrace(Bitmap bmp, string filename, bool UseSpotRemoval, int SpotRemoval, bool UseSmoothing, decimal Smoothing, bool UseOptimize, decimal Optimize, L2LConf c)
		{
			bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
			long start = Tools.HiResTimer.TotalMilliseconds;
			list.Clear();
			mRange.ResetRange();

			Potrace.turdsize = (int)(UseSpotRemoval ? SpotRemoval : 2);
			Potrace.alphamax = UseSmoothing ? (double)Smoothing : 0.0;
			Potrace.opttolerance = UseOptimize ? (double)Optimize : 0.2;
			Potrace.curveoptimizing = UseOptimize; //optimize the path p, replacing sequences of Bezier segments by a single segment when possible.

			List<List<CsPotrace.Curve>> plist = Potrace.PotraceTrace(bmp);

			if (c.dir != RasterConverter.ImageProcessor.Direction.None)
			{
				using (Bitmap ptb = new Bitmap(bmp.Width, bmp.Height))
				{
					using (Graphics g = Graphics.FromImage(ptb))
					{
						//Potrace.Export2GDIPlus(plist, g, Brushes.Black, null, (Math.Max(c.res/c.fres, 1) + 1) / 2.0f);
						Potrace.Export2GDIPlus(plist, g, Brushes.Black, null, Math.Max(1, c.res / c.fres));
						using (Bitmap resampled = RasterConverter.ImageTransform.ResizeImage(ptb, new Size((int)(bmp.Width * c.fres / c.res), (int)(bmp.Height * c.fres / c.res)), true, InterpolationMode.HighQualityBicubic))
						{
							//absolute
							list.Add(new GrblCommand("G90"));
							//use travel speed
							list.Add(new GrblCommand(String.Format("F{0}", c.travelSpeed)));
							//move fast to offset
							list.Add(new GrblCommand(String.Format("G0 X{0} Y{1}", formatnumber(c.oX), formatnumber(c.oY))));
							if (c.pwm)
								list.Add(new GrblCommand(String.Format("{0} S0", c.lOn))); //laser on and power to zero
							else
								list.Add(new GrblCommand(String.Format("{0} S255", c.lOff))); //laser off and power to max power

							//set speed to markspeed						
							list.Add(new GrblCommand(String.Format("G1 F{0}", c.markSpeed)));
							//relative
							list.Add(new GrblCommand("G91"));


							c.vectorfilling = true;
							ImageLine2Line(resampled, c);

							//laser off
							list.Add(new GrblCommand(c.lOff));
						}
					}
				}
			}

			//absolute
			list.Add(new GrblCommand("G90"));
			//use travel speed
			list.Add(new GrblCommand(String.Format("F{0}", c.travelSpeed)));
			//move fast to offset
			list.Add(new GrblCommand(String.Format("G0 X{0} Y{1}", formatnumber(c.oX), formatnumber(c.oY))));
			//laser off and power to maxPower
			list.Add(new GrblCommand(String.Format("{0} S{1}", c.lOff, c.maxPower)));
			//set speed to borderspeed
			list.Add(new GrblCommand(String.Format("G1 F{0}", c.borderSpeed)));

			//trace borders
			List<string> gc = Potrace.Export2GCode(plist, c.oX, c.oY, c.res, c.lOn, c.lOff, bmp.Size);

			foreach (string code in gc)
				list.Add(new GrblCommand(code));


			//laser off
			list.Add(new GrblCommand(String.Format("{0}", c.lOff)));

			//move fast to origin
			list.Add(new GrblCommand("G0 X0 Y0"));

			Analyze();
			long elapsed = Tools.HiResTimer.TotalMilliseconds - start;

			if (OnFileLoaded != null)
				OnFileLoaded(elapsed, filename);
		}

		public class L2LConf
		{
			public double res;
			public int oX;
			public int oY;
			public int markSpeed;
			public int travelSpeed;
			public int borderSpeed;
			public int minPower;
			public int maxPower;
			public string lOn;
			public string lOff;
			public RasterConverter.ImageProcessor.Direction dir;
			public bool pwm;
			public double fres;
			public bool vectorfilling;
		}

		public void LoadImageL2L(Bitmap bmp, string filename, L2LConf c)
		{

			bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

			long start = Tools.HiResTimer.TotalMilliseconds;
			list.Clear();
			mRange.ResetRange();

			//absolute
			list.Add(new GrblCommand("G90"));
			//use travel speed
			list.Add(new GrblCommand(String.Format("F{0}", c.travelSpeed)));
			//move fast to offset
			list.Add(new GrblCommand(String.Format("G0 X{0} Y{1}", formatnumber(c.oX), formatnumber(c.oY))));
			if (c.pwm)
				list.Add(new GrblCommand(String.Format("{0} S0", c.lOn))); //laser on and power to zero
			else
				list.Add(new GrblCommand(String.Format("{0} S255", c.lOff))); //laser off and power to maxpower

			//set speed to markspeed						
			list.Add(new GrblCommand(String.Format("G1 F{0}", c.markSpeed)));
			//relative
			list.Add(new GrblCommand("G91"));

			ImageLine2Line(bmp, c);

			//laser off
			list.Add(new GrblCommand(c.lOff));
			//absolute
			list.Add(new GrblCommand("G90"));
			//move fast to origin
			list.Add(new GrblCommand("G0 X0 Y0"));

			Analyze();
			long elapsed = Tools.HiResTimer.TotalMilliseconds - start;

			if (OnFileLoaded != null)
				OnFileLoaded(elapsed, filename);
		}

		private void ImageLine2Line(Bitmap bmp, L2LConf c)
		{
			bool fast = false;
			List<ColorSegment> segments = GetSegments(bmp, c);
			List<GrblCommand> temp = new List<GrblCommand>();
			foreach (ColorSegment seg in segments)
			{
				bool changespeed = (fast != seg.Fast); //se veloce != dafareveloce

				if (seg.IsSeparator && !fast) //fast = previous segment contains S0 color
				{
					if (c.pwm)
						temp.Add(new GrblCommand("S0"));
					else
						temp.Add(new GrblCommand(c.lOff)); //laser off
				}

				fast = seg.Fast;

				if (changespeed)
					temp.Add(new GrblCommand(String.Format("{0} F{1} {2}", fast ? "G0" : "G1", fast ? c.travelSpeed : c.markSpeed, seg.ToString())));
				else
					temp.Add(new GrblCommand(seg.ToString()));

				//if (seg.IsSeparator)
				//	list.Add(new GrblCommand(lOn));
			}

			temp = OptimizeLine2Line(temp, c);
			list.AddRange(temp);
		}

		private List<GrblCommand> OptimizeLine2Line(List<GrblCommand> temp, L2LConf c)
		{
			List<GrblCommand> rv = new List<GrblCommand>();

			decimal cumX = 0;
			decimal cumY = 0;
			bool cumulate = false;

			foreach (GrblCommand cmd in temp)
			{
				try
				{
					cmd.BuildHelper();

					bool oldcumulate = cumulate;

					if (c.pwm)
					{
						if (cmd.S != null) //is S command
						{
							if (cmd.S.Number == 0) //is S command with zero power
								cumulate = true;   //begin cumulate
							else
								cumulate = false;  //end cumulate
						}
					}
					else
					{
						if (cmd.IsLaserOFF)
							cumulate = true;   //begin cumulate
						else if (cmd.IsLaserON)
							cumulate = false;  //end cumulate
					}


					if (oldcumulate && !cumulate) //cumulate down front -> flush
					{
						if (c.pwm)
							rv.Add(new GrblCommand(string.Format("G0 X{0} Y{1} F{2} S0", formatnumber((double)cumX), formatnumber((double)cumY), c.travelSpeed)));
						else
							rv.Add(new GrblCommand(string.Format("G0 X{0} Y{1} F{2} {3}", formatnumber((double)cumX), formatnumber((double)cumY), c.travelSpeed, c.lOff)));

						cumX = cumY = 0;
					}

					if (cumulate) //cumulate
					{
						if (cmd.IsMovement)
						{
							if (cmd.X != null)
								cumX += cmd.X.Number;
							if (cmd.Y != null)
								cumY += cmd.Y.Number;
						}
						else
						{
							rv.Add(cmd);
						}
					}
					else //emit line normally
					{
						rv.Add(cmd);
					}
				}
				catch (Exception ex) { throw ex; }
				finally { cmd.DeleteHelper(); }
			}

			return rv;
		}

		private List<ColorSegment> GetSegments(Bitmap bmp, L2LConf c)
		{
			bool uni = (bool)Settings.GetObject("Unidirectional Engraving", false);

			List<ColorSegment> rv = new List<ColorSegment>();
			if (c.dir == RasterConverter.ImageProcessor.Direction.Horizontal || c.dir == RasterConverter.ImageProcessor.Direction.Vertical)
			{
				bool h = (c.dir == RasterConverter.ImageProcessor.Direction.Horizontal); //horizontal/vertical

				for (int i = 0; i < (h ? bmp.Height : bmp.Width); i++)
				{
					bool d = uni || IsEven(i); //direct/reverse
					int prevCol = -1;
					int len = -1;

					for (int j = d ? 0 : (h ? bmp.Width - 1 : bmp.Height - 1); d ? (j < (h ? bmp.Width : bmp.Height)) : (j >= 0); j = (d ? j + 1 : j - 1))
						ExtractSegment(bmp, h ? j : i, h ? i : j, !d, ref len, ref prevCol, rv, c); //extract different segments

					if (h)
						rv.Add(new XSegment(prevCol, len + 1, !d, c)); //close last segment
					else
						rv.Add(new YSegment(prevCol, len + 1, !d, c)); //close last segment

					if (uni) // add "go back"
					{
						if (h) rv.Add(new XSegment(0, bmp.Width, true, c));
						else rv.Add(new YSegment(0, bmp.Height, true, c));
					}

					if (i < (h ? bmp.Height - 1 : bmp.Width - 1))
					{
						if (h)
							rv.Add(new VSeparator(c)); //new line
						else
							rv.Add(new HSeparator(c)); //new line
					}
				}
			}
			else if (c.dir == RasterConverter.ImageProcessor.Direction.Diagonal)
			{
				//based on: http://stackoverflow.com/questions/1779199/traverse-matrix-in-diagonal-strips
				//based on: http://stackoverflow.com/questions/2112832/traverse-rectangular-matrix-in-diagonal-strips

				/*

				+------------+
				|  -         |
				|  -  -      |
				+-------+    |
				|  -  - |  - |
				+-------+----+

				*/


				//the algorithm runs along the matrix for diagonal lines (slice index)
				//z1 and z2 contains the number of missing elements in the lower right and upper left
				//the length of the segment can be determined as "slice - z1 - z2"
				//my modified version of algorithm reverses travel direction each slice

				rv.Add(new VSeparator(c)); //new line

				int w = bmp.Width;
				int h = bmp.Height;
				for (int slice = 0; slice < w + h - 1; ++slice)
				{
					bool d = uni || IsEven(slice); //direct/reverse

					int prevCol = -1;
					int len = -1;

					int z1 = slice < h ? 0 : slice - h + 1;
					int z2 = slice < w ? 0 : slice - w + 1;

					for (int j = (d ? z1 : slice - z2); d ? j <= slice - z2 : j >= z1; j = (d ? j + 1 : j - 1))
						ExtractSegment(bmp, j, slice - j, !d, ref len, ref prevCol, rv, c); //extract different segments
					rv.Add(new DSegment(prevCol, len + 1, !d, c)); //close last segment

					//System.Diagnostics.Debug.WriteLine(String.Format("sl:{0} z1:{1} z2:{2}", slice, z1, z2));

					if (uni) // add "go back"
					{
						int slen = (slice - z1 - z2) + 1;
						rv.Add(new DSegment(0, slen, true, c));
						//System.Diagnostics.Debug.WriteLine(slen);
					}

					if (slice < Math.Min(w, h) - 1) //first part of the image
					{
						if (d && !uni)
							rv.Add(new HSeparator(c)); //new line
						else
							rv.Add(new VSeparator(c)); //new line
					}
					else if (slice >= Math.Max(w, h) - 1) //third part of image
					{
						if (d && !uni)
							rv.Add(new VSeparator(c)); //new line
						else
							rv.Add(new HSeparator(c)); //new line
					}
					else //central part of the image
					{
						if (w > h)
							rv.Add(new HSeparator(c)); //new line
						else
							rv.Add(new VSeparator(c)); //new line
					}
				}
			}

			return rv;
		}

		private void ExtractSegment(Bitmap image, int x, int y, bool reverse, ref int len, ref int prevCol, List<ColorSegment> rv, L2LConf c)
		{
			len++;
			int col = GetColor(image, x, y, c.minPower, c.maxPower, c.pwm);
			if (prevCol == -1)
				prevCol = col;

			if (prevCol != col)
			{
				if (c.dir == RasterConverter.ImageProcessor.Direction.Horizontal)
					rv.Add(new XSegment(prevCol, len, reverse, c));
				else if (c.dir == RasterConverter.ImageProcessor.Direction.Vertical)
					rv.Add(new YSegment(prevCol, len, reverse, c));
				else if (c.dir == RasterConverter.ImageProcessor.Direction.Diagonal)
					rv.Add(new DSegment(prevCol, len, reverse, c));

				len = 0;
			}

			prevCol = col;
		}


		private int GetColor(Bitmap I, int X, int Y, int min, int max, bool pwm)
		{
			Color C = I.GetPixel(X, Y);
			int rv = (255 - C.R) * C.A / 255;

			if (rv == 0)
				return 0; //zero is always zero
			else if (pwm)
				return rv * (max - min) / 255 + min; //scale to range
			else
				return rv;
		}

		public string formatnumber(double number)
		{ return number.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture); }

		private static bool IsEven(int value)
		{ return value % 2 == 0; }

		public int Count
		{ get { return list.Count; } }

		public TimeSpan EstimatedTime { get { return mEstimatedTotalTime; } }


		//  II | I
		// ---------
		// III | IV
		public CartesianQuadrant Quadrant
		{
			get
			{
				if (!mRange.DrawingRange.ValidRange)
					return CartesianQuadrant.Unknown;
				else if (mRange.DrawingRange.X.Min >= 0 && mRange.DrawingRange.Y.Min >= 0)
					return CartesianQuadrant.I;
				else if (mRange.DrawingRange.X.Max <= 0 && mRange.DrawingRange.Y.Min >= 0)
					return CartesianQuadrant.II;
				else if (mRange.DrawingRange.X.Max <= 0 && mRange.DrawingRange.Y.Max <= 0)
					return CartesianQuadrant.III;
				else if (mRange.DrawingRange.X.Min >= 0 && mRange.DrawingRange.Y.Max <= 0)
					return CartesianQuadrant.IV;
				else
					return CartesianQuadrant.Mix;
			}
		}

		internal void DrawOnGraphics(Graphics g, Size s)
		{
			if (!mRange.MovingRange.ValidRange) return;

			GrblCommand.StatePositionBuilder spb = new GrblCommand.StatePositionBuilder();
			ProgramRange.XYRange scaleRange = mRange.MovingRange;

			//Get scale factors for both directions. To preserve the aspect ratio, use the smaller scale factor.
			float zoom = scaleRange.Width > 0 && scaleRange.Height > 0 ? Math.Min((float)s.Width / (float)scaleRange.Width, (float)s.Height / (float)scaleRange.Height) * 0.95f : 1;
			bool firstline = true; //used to draw the first line in a different color

			ScaleAndPosition(g, s, scaleRange, zoom);

			foreach (GrblCommand cmd in list)
			{
				try
				{
					cmd.BuildHelper();
					spb.AnalyzeCommand(cmd, false);


					if (spb.TrueMovement())
					{
						Color linecolor = Color.FromArgb(spb.GetCurrentAlpha(mRange.SpindleRange), firstline ? ColorScheme.PreviewFirstMovement : spb.LaserBurning ? ColorScheme.PreviewLaserPower : ColorScheme.PreviewOtherMovement);
						using (Pen pen = GetPen(linecolor))
						{
							pen.ScaleTransform(1 / zoom, 1 / zoom);

							if (!spb.LaserBurning)
							{
								pen.DashStyle = DashStyle.Dash;
								pen.DashPattern = new float[] { 1f, 1f };
							}

							if (spb.G0G1 && cmd.IsLinearMovement && pen.Color.A > 0)
							{
								g.DrawLine(pen, new PointF((float)spb.X.Previous, (float)spb.Y.Previous), new PointF((float)spb.X.Number, (float)spb.Y.Number));
							}
							else if (spb.G2G3 && cmd.IsArcMovement && pen.Color.A > 0)
							{
								PointF center = cmd.GetCenter((float)spb.X.Previous, (float)spb.Y.Previous);
								double cX = center.X;
								double cY = center.Y;
								double aX = (double)spb.X.Previous;
								double aY = (double)spb.Y.Previous;
								double bX = (double)spb.X.Number;
								double bY = (double)spb.Y.Number;

								double ray = cmd.GetArcRadius();
								double rectX = cX - ray;
								double rectY = cY - ray;
								double rectW = 2 * ray;
								double rectH = 2 * ray;

								double aA = Tools.MathHelper.CalculateAngle(cX, cY, aX, aY);	//180/Math.PI*Math.Atan2(y1-y0, x1-x0);
								double bA = Tools.MathHelper.CalculateAngle(cX, cY, bX, bY);	//180/Math.PI*Math.Atan2(y2-y0, x2-x0);

								double sA = aA;	//start angle
								double wA = Tools.MathHelper.AngularDistance(aA, bA, spb.G2);

								if (rectW > 0 && rectH > 0)
								{
									try { g.DrawArc(pen, (float)rectX, (float)rectY, (float)rectW, (float)rectH, (float)sA, (float)wA); }
									catch { System.Diagnostics.Debug.WriteLine(String.Format("Ex drwing arc: W{0} H{1}", rectW, rectH)); }
								}
							}

						}

						firstline = false;
					}
				}
				catch (Exception ex) { throw ex; }
				finally { cmd.DeleteHelper(); }
			}

			DrawJobRange(g, s, zoom);

		}



		private void Analyze() //analyze the file and build global range and timing for each command
		{
			GrblCommand.StatePositionBuilder spb = new GrblCommand.StatePositionBuilder();

			mRange.ResetRange();
			mRange.UpdateXYRange("X0", "Y0", false);
			mEstimatedTotalTime = TimeSpan.Zero;

			foreach (GrblCommand cmd in list)
			{
				try
				{
					GrblConf conf = (GrblConf)Settings.GetObject("Grbl Configuration", new GrblConf());
					TimeSpan delay = spb.AnalyzeCommand(cmd, true, conf);

					mRange.UpdateSRange(spb.S);
					mRange.UpdateXYRange(spb.X, spb.Y, spb.LaserBurning);
					mEstimatedTotalTime += delay;
					cmd.SetOffset(mEstimatedTotalTime);
				}
				catch (Exception ex) { throw ex; }
				finally { cmd.DeleteHelper(); }
			}
		}

		private void ScaleAndPosition(Graphics g, Size s, ProgramRange.XYRange scaleRange, float zoom)
		{
			g.ResetTransform();
			float margin = 10;
			CartesianQuadrant q = Quadrant;
			if (q == CartesianQuadrant.Unknown || q == CartesianQuadrant.I)
			{
				//Scale and invert Y
				g.ScaleTransform(zoom, -zoom, MatrixOrder.Append);
				//Translate to position bottom-left
				g.TranslateTransform(margin, s.Height - margin, MatrixOrder.Append);
			}
			else if (q == CartesianQuadrant.II)
			{
				//Scale and invert Y
				g.ScaleTransform(zoom, -zoom, MatrixOrder.Append);
				//Translate to position bottom-left
				g.TranslateTransform(s.Width - margin, s.Height - margin, MatrixOrder.Append);
			}
			else if (q == CartesianQuadrant.III)
			{
				//Scale and invert Y
				g.ScaleTransform(zoom, -zoom, MatrixOrder.Append);
				//Translate to position bottom-left
				g.TranslateTransform(s.Width - margin, margin, MatrixOrder.Append);
			}
			else if (q == CartesianQuadrant.IV)
			{
				//Scale and invert Y
				g.ScaleTransform(zoom, -zoom, MatrixOrder.Append);
				//Translate to position bottom-left
				g.TranslateTransform(margin, margin, MatrixOrder.Append);
			}
			else
			{
				//Translate to center of gravity of the image
				g.TranslateTransform(-scaleRange.Center.X, -scaleRange.Center.Y, MatrixOrder.Append);
				//Scale and invert Y
				g.ScaleTransform(zoom, -zoom, MatrixOrder.Append);
				//Translate to center over the drawing area.
				g.TranslateTransform(s.Width / 2, s.Height / 2, MatrixOrder.Append);
			}

		}

		private void DrawJobRange(Graphics g, Size s, float zoom)
		{
			//RectangleF frame = new RectangleF(-s.Width / zoom, -s.Height / zoom, s.Width / zoom, s.Height / zoom);

			SizeF wSize = new SizeF(s.Width / zoom, s.Height / zoom);

			//draw cartesian plane
			using (Pen pen = GetPen(ColorScheme.PreviewText))
			{
				pen.ScaleTransform(1 / zoom, 1 / zoom);
				g.DrawLine(pen, -wSize.Width, 0.0f, wSize.Width, 0.0f);
				g.DrawLine(pen, 0, -wSize.Height, 0, wSize.Height);
			}

			//draw job range
			if (mRange.DrawingRange.ValidRange)
			{
				using (Pen pen = GetPen(ColorScheme.PreviewJobRange))
				{
					pen.DashStyle = DashStyle.Dash;
					pen.DashPattern = new float[] { 1.0f / zoom, 2.0f / zoom }; //pen.DashPattern = new float[] { 1f / zoom, 2f / zoom};
					pen.ScaleTransform(1.0f / zoom, 1.0f / zoom);

					g.DrawLine(pen, -wSize.Width, (float)mRange.DrawingRange.Y.Min, wSize.Width, (float)mRange.DrawingRange.Y.Min);
					g.DrawLine(pen, -wSize.Width, (float)mRange.DrawingRange.Y.Max, wSize.Width, (float)mRange.DrawingRange.Y.Max);
					g.DrawLine(pen, (float)mRange.DrawingRange.X.Min, -wSize.Height, (float)mRange.DrawingRange.X.Min, wSize.Height);
					g.DrawLine(pen, (float)mRange.DrawingRange.X.Max, -wSize.Height, (float)mRange.DrawingRange.X.Max, wSize.Height);

					CartesianQuadrant q = Quadrant;
					bool right = q == CartesianQuadrant.I || q == CartesianQuadrant.IV;
					bool top = q == CartesianQuadrant.I || q == CartesianQuadrant.II;

					DrawString(g, zoom, 0, mRange.DrawingRange.Y.Min, mRange.DrawingRange.Y.Min.ToString("0"), false, true, !right, false);
					DrawString(g, zoom, 0, mRange.DrawingRange.Y.Max, mRange.DrawingRange.Y.Max.ToString("0"), false, true, !right, false);
					DrawString(g, zoom, mRange.DrawingRange.X.Min, 0, mRange.DrawingRange.X.Min.ToString("0"), true, false, false, top);
					DrawString(g, zoom, mRange.DrawingRange.X.Max, 0, mRange.DrawingRange.X.Max.ToString("0"), true, false, false, top);
				}
			}
		}

		private Pen GetPen(Color color)
		{ return new Pen(color); }

		private static Brush GetBrush(Color color)
		{ return new SolidBrush(color); }

		private static void DrawString(Graphics g, float zoom, decimal curX, decimal curY, string text, bool centerX, bool centerY, bool subtractX, bool subtractY)
		{
			GraphicsState state = g.Save();
			g.ScaleTransform(1.0f, -1.0f);

			using (Font f = new Font(FontFamily.GenericMonospace, 8 * 1 / zoom))
			{
				float offsetX = 0;
				float offsetY = 0;

				SizeF ms = g.MeasureString(text, f);

				if (centerX)
					offsetX = ms.Width / 2;

				if (centerY)
					offsetY = ms.Height / 2;

				if (subtractX)
					offsetX += ms.Width;

				if (subtractY)
					offsetY += ms.Height;

				using (Brush b = GetBrush(ColorScheme.PreviewText))
				{ g.DrawString(text, f, b, (float)curX - offsetX, (float)-curY - offsetY); }

			}
			g.Restore(state);
		}



		System.Collections.Generic.IEnumerator<GrblCommand> IEnumerable<GrblCommand>.GetEnumerator()
		{ return list.GetEnumerator(); }


		public System.Collections.IEnumerator GetEnumerator()
		{ return list.GetEnumerator(); }

		public ProgramRange Range { get { return mRange; } }

		public GrblCommand this[int index]
		{ get { return list[index]; } }

	}






	public class ProgramRange
	{
		public class XYRange
		{
			public class Range
			{
				public decimal Min;
				public decimal Max;

				public Range()
				{ ResetRange(); }

				public void UpdateRange(decimal val)
				{
					Min = Math.Min(Min, val);
					Max = Math.Max(Max, val);
				}

				public void ResetRange()
				{
					Min = decimal.MaxValue;
					Max = decimal.MinValue;
				}

				public bool ValidRange
				{ get { return Min != decimal.MaxValue && Max != decimal.MinValue; } }
			}

			public Range X = new Range();
			public Range Y = new Range();

			public void UpdateRange(GrblCommand.Element x, GrblCommand.Element y)
			{
				if (x != null) X.UpdateRange(x.Number);
				if (y != null) Y.UpdateRange(y.Number);
			}

			public void ResetRange()
			{
				X.ResetRange();
				Y.ResetRange();
			}

			public bool ValidRange
			{ get { return X.ValidRange && Y.ValidRange; } }

			public decimal Width
			{ get { return X.Max - X.Min; } }

			public decimal Height
			{ get { return Y.Max - Y.Min; } }

			public PointF Center
			{
				get
				{
					if (ValidRange)
						return new PointF((float)X.Min + (float)Width / 2.0f, (float)Y.Min + (float)Height / 2.0f);
					else
						return new PointF(0, 0);
				}
			}
		}

		public class SRange
		{
			public class Range
			{
				public decimal Min;
				public decimal Max;

				public Range()
				{ ResetRange(); }

				public void UpdateRange(decimal val)
				{
					Min = Math.Min(Min, val);
					Max = Math.Max(Max, val);
				}

				public void ResetRange()
				{
					Min = decimal.MaxValue;
					Max = decimal.MinValue;
				}

				public bool ValidRange
				{ get { return Min != Max && Min != decimal.MaxValue && Max != decimal.MinValue && Max > 0; } }
			}

			public Range S = new Range();

			public void UpdateRange(decimal s)
			{
				S.UpdateRange(s);
			}

			public void ResetRange()
			{
				S.ResetRange();
			}

			public bool ValidRange
			{ get { return S.ValidRange; } }
		}

		public XYRange DrawingRange = new XYRange();
		public XYRange MovingRange = new XYRange();
		public SRange SpindleRange = new SRange();

		public void UpdateXYRange(GrblCommand.Element X, GrblCommand.Element Y, bool drawing)
		{
			if (drawing) DrawingRange.UpdateRange(X, Y);
			MovingRange.UpdateRange(X, Y);
		}

		public void UpdateSRange(GrblCommand.Element S)
		{ if (S != null) SpindleRange.UpdateRange(S.Number); }

		public void ResetRange()
		{
			DrawingRange.ResetRange();
			MovingRange.ResetRange();
			SpindleRange.ResetRange();
		}

	}

}

/*
Gnnn	Standard GCode command, such as move to a point
Mnnn	RepRap-defined command, such as turn on a cooling fan
Tnnn	Select tool nnn. In RepRap, a tool is typically associated with a nozzle, which may be fed by one or more extruders.
Snnn	Command parameter, such as time in seconds; temperatures; voltage to send to a motor
Pnnn	Command parameter, such as time in milliseconds; proportional (Kp) in PID Tuning
Xnnn	A X coordinate, usually to move to. This can be an Integer or Fractional number.
Ynnn	A Y coordinate, usually to move to. This can be an Integer or Fractional number.
Znnn	A Z coordinate, usually to move to. This can be an Integer or Fractional number.
U,V,W	Additional axis coordinates (RepRapFirmware)
Innn	Parameter - X-offset in arc move; integral (Ki) in PID Tuning
Jnnn	Parameter - Y-offset in arc move
Dnnn	Parameter - used for diameter; derivative (Kd) in PID Tuning
Hnnn	Parameter - used for heater number in PID Tuning
Fnnn	Feedrate in mm per minute. (Speed of print head movement)
Rnnn	Parameter - used for temperatures
Qnnn	Parameter - not currently used
Ennn	Length of extrudate. This is exactly like X, Y and Z, but for the length of filament to consume.
Nnnn	Line number. Used to request repeat transmission in the case of communications errors.
;		Gcode comments begin at a semicolon
*/

/*
Supported G-Codes in v0.9i
G38.3, G38.4, G38.5: Probing
G40: Cutter Radius Compensation Modes
G61: Path Control Modes
G91.1: Arc IJK Distance Modes
Supported G-Codes in v0.9h
G38.2: Probing
G43.1, G49: Dynamic Tool Length Offsets
Supported G-Codes in v0.8 (and v0.9)
G0, G1: Linear Motions (G0 Fast, G1 Controlled)
G2, G3: Arc and Helical Motions
G4: Dwell
G10 L2, G10 L20: Set Work Coordinate Offsets
G17, G18, G19: Plane Selection
G20, G21: Units
G28, G30: Go to Pre-Defined Position
G28.1, G30.1: Set Pre-Defined Position
G53: Move in Absolute Coordinates
G54, G55, G56, G57, G58, G59: Work Coordinate Systems
G80: Motion Mode Cancel
G90, G91: Distance Modes
G92: Coordinate Offset
G92.1: Clear Coordinate System Offsets
G93, G94: Feedrate Modes
M0, M2, M30: Program Pause and End
M3, M4, M5: Spindle Control
M8, M9: Coolant Control
*/