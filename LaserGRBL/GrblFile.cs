using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace LaserGRBL
{
	public class GrblFile : IEnumerable<GrblCommand>
	{
		public delegate void OnFileLoadedDlg(long elapsed, string filename);
		public event OnFileLoadedDlg OnFileLoaded;

		private List<GrblCommand> list = new List<GrblCommand>();
		private ProgramRange mRange = new ProgramRange();
		private decimal mTotalTravelOn;
		private decimal mTotalTravelOff;
		private TimeSpan mEstimatedTimeOn;
		private TimeSpan mEstimatedTimeOff;

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
			mTotalTravelOff = 0;
			mTotalTravelOn = 0;
			mEstimatedTimeOff = TimeSpan.Zero;
			mEstimatedTimeOn = TimeSpan.Zero;
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


		private class ColorSegment
		{
			private int  mColor;
			private double mLen;
			public ColorSegment(int col, int len, int res)
			{
				mColor = col;
				mLen = (double)len / (double)res;
			}

			public int SegmentColor
			{ get { return mColor; } }

			public double SegmentLen
			{ get { return mLen; } }
		}

		private class Separator : ColorSegment
		{
			public Separator(int res) : base(0, 1, res)
			{ }


		}


		public void LoadImage(Bitmap image, string filename, int res, int oX, int oY, int markSpeed, int travelSpeed, int minPower, int maxPower, string lOn, string lOff, RasterConverter.ImageProcessor.Direction dir)
		{

			image.RotateFlip(RotateFlipType.RotateNoneFlipY);

			long start = Tools.HiResTimer.TotalMilliseconds;
			mTotalTravelOff = 0;
			mTotalTravelOn = 0;
			mEstimatedTimeOff = TimeSpan.Zero;
			mEstimatedTimeOn = TimeSpan.Zero;
			list.Clear();
			mRange.ResetRange();

			List<ColorSegment> segments = GetSegments(image, dir, minPower, maxPower, res);
			list.Add(new GrblCommand("G90"));
			list.Add(new GrblCommand(String.Format("F{0}", travelSpeed)));
			list.Add(new GrblCommand(String.Format("G0 X{0} Y{1}", formatnumber(oX), formatnumber(oY)))); //move fast to offset
			list.Add(new GrblCommand(String.Format("M5 S{0}", minPower))); //laser off and power to minPower
			list.Add(new GrblCommand(String.Format("G1 F{0}", markSpeed)));
			list.Add(new GrblCommand("G91"));
			list.Add(new GrblCommand("M3"));

			bool fast = false;

			if (dir == RasterConverter.ImageProcessor.Direction.Horizontal)
			{
				foreach (ColorSegment seg in segments)
				{
					if (seg is Separator)
					{
						bool changespeed = (fast != true); //se veloce != dafareveloce
						fast = true;

						list.Add(new GrblCommand("M5"));
						if (changespeed)
							list.Add(new GrblCommand(String.Format("{0} F{1} Y{2}", fast ? "G0" : "G1", fast ? travelSpeed : markSpeed, formatnumber(seg.SegmentLen))));
						else
							list.Add(new GrblCommand(String.Format("Y{0}", formatnumber(seg.SegmentLen), travelSpeed)));
						list.Add(new GrblCommand("M3"));
					}
					else
					{
						bool changespeed = (fast != (seg.SegmentColor == 0)); //se veloce != dafareveloce
						fast = (seg.SegmentColor == 0);

						if (changespeed)
							list.Add(new GrblCommand(String.Format("{0} F{1} X{2} S{3}", fast ? "G0" : "G1", fast ? travelSpeed : markSpeed, formatnumber(seg.SegmentLen), seg.SegmentColor)));
						else
							list.Add(new GrblCommand(String.Format("X{0} S{1}", formatnumber(seg.SegmentLen), seg.SegmentColor)));
					}
				}
			}
			else if (dir == RasterConverter.ImageProcessor.Direction.Vertical)
			{
				foreach (ColorSegment seg in segments)
				{
					if (seg is Separator)
					{
						bool changespeed = (fast != true); //se veloce != dafareveloce
						fast = true;

						list.Add(new GrblCommand("M5"));
						if (changespeed)
							list.Add(new GrblCommand(String.Format("{0} F{1} X{2}", fast ? "G0" : "G1", fast ? travelSpeed : markSpeed, formatnumber(seg.SegmentLen))));
						else
							list.Add(new GrblCommand(String.Format("X{0}", formatnumber(seg.SegmentLen), travelSpeed)));
						list.Add(new GrblCommand("M3"));
					}
					else
					{
						bool changespeed = (fast != (seg.SegmentColor == 0)); //se veloce != dafareveloce
						fast = (seg.SegmentColor == 0);

						if (changespeed)
							list.Add(new GrblCommand(String.Format("{0} F{1} Y{2} S{3}", fast ? "G0" : "G1", fast ? travelSpeed : markSpeed, formatnumber(seg.SegmentLen), seg.SegmentColor)));
						else
							list.Add(new GrblCommand(String.Format("Y{0} S{1}", formatnumber(seg.SegmentLen), seg.SegmentColor)));
					}
				}
			}
			else if (dir == RasterConverter.ImageProcessor.Direction.Diagonal)
			{
				bool reverse = false;
				fast = true;
				list.Add(new GrblCommand(String.Format("G0 Y{0} F{1}", formatnumber(1.0 / (double)res), travelSpeed)));
				
				foreach (ColorSegment seg in segments)
				{
					if (seg is Separator)
					{
						bool changespeed = (fast != true); //se veloce != dafareveloce
						fast = true;

						list.Add(new GrblCommand("M5"));
						if (changespeed)
							list.Add(new GrblCommand(String.Format("{0} F{1} {2}{3} ", fast ? "G0" : "G1", fast ? travelSpeed : markSpeed, reverse ? "Y" : "X", formatnumber(seg.SegmentLen))));
						else
							list.Add(new GrblCommand(String.Format("{0}{1}", reverse ? "Y" : "X", formatnumber(seg.SegmentLen), travelSpeed)));
						list.Add(new GrblCommand("M3"));
						
						reverse = !reverse;
					}
					else
					{
						bool changespeed = (fast != (seg.SegmentColor == 0)); //se veloce != dafareveloce
						fast = (seg.SegmentColor == 0);

						double X = -seg.SegmentLen;
						double Y = seg.SegmentLen;
						
						
						if (changespeed)
							list.Add(new GrblCommand(String.Format("{0} F{1} X{2} Y{3} S{4}", fast ? "G0" : "G1", fast ? travelSpeed : markSpeed, formatnumber(X), formatnumber(Y), seg.SegmentColor)));
						else
							list.Add(new GrblCommand(String.Format("X{0} Y{1} S{2}", formatnumber(X), formatnumber(Y), seg.SegmentColor)));
					}
				}
			}

			list.Add(new GrblCommand("M5"));
			list.Add(new GrblCommand("G90"));

			Analyze();
			long elapsed = Tools.HiResTimer.TotalMilliseconds - start;

			if (OnFileLoaded != null)
				OnFileLoaded(elapsed, filename);
		}

		private List<ColorSegment> GetSegments(Bitmap image, RasterConverter.ImageProcessor.Direction dir, int minPower, int maxPower, int res)
		{
			List<ColorSegment> rv = new List<ColorSegment>();
			if (dir == RasterConverter.ImageProcessor.Direction.Horizontal)
			{
				for (int y = 0; y < image.Height; y++)
				{
					int prevCol = -1;
					int len = -1;

					if (IsEven(y))
					{
						for (int x = 0; x < image.Width; x++)
							ExtractSegment(image, x, y, true, ref len, ref prevCol, rv, minPower, maxPower, res); //extract different segments
						rv.Add(new ColorSegment(prevCol, len + 1, res)); //close last segment
					}
					else
					{
						for (int x = image.Width - 1; x >= 0; x--)
							ExtractSegment(image, x, y, false, ref len, ref prevCol, rv, minPower, maxPower, res); //extract different segments
						rv.Add(new ColorSegment(prevCol, -(len + 1), res)); //close last segment
					}

					if (y < image.Height)
						rv.Add(new Separator(res)); //new line
				}
			}
			else if (dir == RasterConverter.ImageProcessor.Direction.Vertical)
			{
				for (int x = 0; x < image.Width; x++)
				{
					int prevCol = -1;
					int len = -1;

					if (IsEven(x))
					{
						for (int y = 0; y < image.Height; y++)
							ExtractSegment(image, x, y, true, ref len, ref prevCol, rv, minPower, maxPower, res); //extract different segments
						rv.Add(new ColorSegment(prevCol, len + 1, res)); //close last segment
					}
					else
					{
						for (int y = image.Height - 1; y >= 0; y--)
							ExtractSegment(image, x, y, false, ref len, ref prevCol, rv, minPower, maxPower, res); //extract different segments
						rv.Add(new ColorSegment(prevCol, -(len + 1), res)); //close last segment
					}

					if (x < image.Width)
						rv.Add(new Separator(res)); //new line
				}
			}
			else if (dir == RasterConverter.ImageProcessor.Direction.Diagonal)
			{
				int m = image.Width;
				int n = image.Height;
				bool reverse = true;
			    for (int slice = 0; slice < m + n - 1; ++slice) 
			    {
			    	int prevCol = -1;
					int len = -1;
			    	
			        int z1 = slice < n ? 0 : slice - n + 1;
			        int z2 = slice < m ? 0 : slice - m + 1;
			        
			        if (!reverse)
			        {
				        for (int j = slice - z2; j >= z1; --j)
				        {
				        	//System.Diagnostics.Debug.WriteLine(String.Format("X:{0} Y:{1}", j, slice-j));
							ExtractSegment(image, j, slice - j, true, ref len, ref prevCol, rv, minPower, maxPower, res); //extract different segments
				        }
				        rv.Add(new ColorSegment(prevCol, len + 1, res)); //close last segment
			        }
			        else
			        {
				        for (int j = z1 ; j <= slice - z2; ++j)
				        {
				        	//System.Diagnostics.Debug.WriteLine(String.Format("X:{0} Y:{1}", j, slice-j));
							ExtractSegment(image, j, slice - j, false, ref len, ref prevCol, rv, minPower, maxPower, res); //extract different segments
				        }
				        rv.Add(new ColorSegment(prevCol, -(len + 1), res)); //close last segment
			        }
			        
			        if (slice < m + n - 1)
						rv.Add(new Separator(res)); //new line
			        
			        //System.Diagnostics.Debug.WriteLine(String.Format("----------------"));
			        reverse = !reverse;
			    }
			}

			return rv;
		}
		
		private int diagonalLen(int W, int H)
		{return (int)Math.Sqrt(W*W + H*H);}
		
		private int lineLen(Bitmap image, int lineIndex)
		{
			return 0;
		}

		private void ExtractSegment(Bitmap image, int x, int y, bool even, ref int len, ref int prevCol, List<ColorSegment> rv, int minPower, int maxPower, int res)
		{
			len++;
			int col = GetColor(image, x, y, minPower, maxPower);
			if (prevCol == -1)
				prevCol = col;

			if (prevCol != col)
			{
				rv.Add(new ColorSegment(prevCol, even ? len : -len, res));
				len = 0;
			}

			prevCol = col;
		}


		private int GetColor(Bitmap I, int X, int Y, int min, int max)
		{
			Color C = I.GetPixel(X, Y);
			int rv = (255 - C.R) * C.A / 255;
			return rv * (max - min) / 255 + min; //scale to range
		}

		private string formatnumber(double number)
		{ return number.ToString("#.###", System.Globalization.CultureInfo.InvariantCulture); }

		private static bool IsEven(int value)
		{ return value % 2 == 0; }

		public int Count
		{ get { return list.Count; } }

		public TimeSpan EstimatedTime { get { return mEstimatedTimeOff + mEstimatedTimeOn; } }

		private void Analyze()
		{ Process(null, Size.Empty); } //analyze only

		public void DrawOnGraphics(Graphics g, Size s)
		{ Process(g, s); }

		private void Process(Graphics g, Size s)
		{
			Boolean analyze = (g == null);
			Boolean drawing = (g != null);

			if (drawing && !mRange.DrawingRange.ValidRange)
				return;

			float zoom = drawing ? DrawJobRange(g, ref s) : 1;
			bool firstline = true;
			bool laser = false;
			decimal curX = 0;
			decimal curY = 0;
			decimal speed = 0;
			int curAlpha = 0;
			bool cw = false; //cw-ccw memo
			bool abs = false; //absolute-relative memo

			if (analyze)
			{
				mRange.ResetRange();
				//mRange.UpdateSRange(0);
				mRange.UpdateXYRange(0, 0, false);
				mTotalTravelOn = 0;
				mTotalTravelOff = 0;
				mEstimatedTimeOn = TimeSpan.Zero;
				mEstimatedTimeOff = TimeSpan.Zero;
			}


			foreach (GrblCommand cmd in list)
			{
				TimeSpan delay = TimeSpan.Zero;

				if (cmd.IsLaserON)
					laser = true;
				else if (cmd.IsLaserOFF)
					laser = false;

				if (cmd.IsRelativeCoord)
					abs = false;
				if (cmd.IsAbsoluteCoord)
					abs = true;

				if (cmd.F != null)
					speed = cmd.F.Number;

				if (cmd.S != null)
				{
					if (mRange.SpindleRange.ValidRange)
						curAlpha = (int)((cmd.S.Number - mRange.SpindleRange.S.Min) * 255 / (mRange.SpindleRange.S.Max - mRange.SpindleRange.S.Min));
					else
						curAlpha = 255;
				}

				if (analyze && cmd.S != null)
					mRange.UpdateSRange(cmd.S.Number);

				if (cmd.IsMovement && cmd.TrueMovement(curX, curY, abs))
				{
					decimal newX = cmd.X != null ? (abs ? cmd.X.Number : curX + cmd.X.Number) : curX;
					decimal newY = cmd.Y != null ? (abs ? cmd.Y.Number : curY + cmd.Y.Number) : curY;

					if (analyze)
					{
						mRange.UpdateXYRange(newX, newY, laser);

						decimal distance = 0;

						if (cmd.IsLinearMovement)
							distance = Tools.MathHelper.LinearDistance(curX, curY, newX, newY);
						else if (cmd.IsArcMovement) //arc of given radius
							distance = Tools.MathHelper.ArcDistance(curX, curY, newX, newY, cmd.GetArcRadius());

						if (laser)
							mTotalTravelOn += distance;
						else
							mTotalTravelOff += distance;

						if (distance != 0 && speed != 0)
							delay = TimeSpan.FromMinutes((double)distance / (double)speed);
					}

					if (drawing)
					{
						Pen colorpen = firstline ? Pens.Blue : laser ? Pens.Red : Pens.LightGray;
						using (Pen pen = colorpen.Clone() as Pen)
						{
							pen.ScaleTransform(1 / zoom, 1 / zoom);
							if (laser)
								pen.Color = Color.FromArgb(curAlpha, pen.Color);

							if (!laser)
							{
								pen.DashStyle = DashStyle.Dot;
								//pen.EndCap = LineCap.ArrowAnchor;
								//pen.DashPattern = new float[] { 4f, 4f };
							}

							if (cmd.IsLinearMovement)
							{
								g.DrawLine(pen, new PointF((float)curX, (float)curY), new PointF((float)newX, (float)newY));
							}
							else if (cmd.IsArcMovement)
							{
								cw = cmd.IsCW(cw);

								PointF center = cmd.GetCenter((float)curX, (float)curY);
								double cX = center.X;
								double cY = center.Y;
								double aX = (double)curX;
								double aY = (double)curY;
								double bX = (double)newX;
								double bY = (double)newY;

								double ray = cmd.GetArcRadius();
								double rectX = cX - ray;
								double rectY = cY - ray;
								double rectW = 2 * ray;
								double rectH = 2 * ray;

								double aA = Tools.MathHelper.CalculateAngle(cX, cY, aX, aY);	//180/Math.PI*Math.Atan2(y1-y0, x1-x0);
								double bA = Tools.MathHelper.CalculateAngle(cX, cY, bX, bY);	//180/Math.PI*Math.Atan2(y2-y0, x2-x0);

								double sA = aA;	//start angle
								double wA = Tools.MathHelper.AngularDistance(aA, bA, cw);

								if (rectW > 0 && rectH > 0)
									g.DrawArc(pen, (float)rectX, (float)rectY, (float)rectW, (float)rectH, (float)sA, (float)wA);
							}

						}

						firstline = false;
					}

					curX = newX;
					curY = newY;
				}
				else if (cmd.IsPause)
				{
					if (analyze)
					{
						//TimeSpan delay = cmd.P != null ? TimeSpan.FromMilliseconds((double)cmd.P.Number) : cmd.S != null ? TimeSpan.FromSeconds((double)cmd.S.Number) : TimeSpan.Zero;
						//grbl seem to use both P and S as number of seconds
						delay = cmd.P != null ? TimeSpan.FromSeconds((double)cmd.P.Number) : cmd.S != null ? TimeSpan.FromSeconds((double)cmd.S.Number) : TimeSpan.Zero;
					}
				}

				if (laser)
					mEstimatedTimeOn += delay;
				else
					mEstimatedTimeOff += delay;

				if (analyze)
					cmd.SetOffset(mTotalTravelOn + mTotalTravelOff, mEstimatedTimeOn + mEstimatedTimeOff);
			}
		}

		private float DrawJobRange(Graphics g, ref Size s)
		{
			Size wSize = s;
			float zoom = 1;
			float ctrW = wSize.Width - 10;
			float ctrH = wSize.Height - 10;
			float proW = (float)mRange.DrawingRange.X.Max;
			float proH = (float)mRange.DrawingRange.Y.Max;
			zoom = Math.Min(ctrW / proW, ctrH / proH);
			g.ScaleTransform(zoom, zoom);

			using (Pen pen = Pens.LightGray.Clone() as Pen)
			{
				pen.ScaleTransform(1 / zoom, 1 / zoom);
				pen.DashStyle = DashStyle.Dash;
				pen.DashPattern = new float[] { 1f, 2f };

				g.DrawLine(pen, 0, (float)mRange.DrawingRange.Y.Min, wSize.Width, (float)mRange.DrawingRange.Y.Min);
				DrawString(g, zoom, 0, mRange.DrawingRange.Y.Min, mRange.DrawingRange.Y.Min.ToString("0"), false, true, true);
				g.DrawLine(pen, 0, (float)mRange.DrawingRange.Y.Max, wSize.Width, (float)mRange.DrawingRange.Y.Max);
				DrawString(g, zoom, 0, mRange.DrawingRange.Y.Max, mRange.DrawingRange.Y.Max.ToString("0"), false, true, true);

				g.DrawLine(pen, (float)mRange.DrawingRange.X.Min, 0, (float)mRange.DrawingRange.X.Min, wSize.Height);
				DrawString(g, zoom, mRange.DrawingRange.X.Min, 0, mRange.DrawingRange.X.Min.ToString("0"), true);
				g.DrawLine(pen, (float)mRange.DrawingRange.X.Max, 0, (float)mRange.DrawingRange.X.Max, wSize.Height);
				DrawString(g, zoom, mRange.DrawingRange.X.Max, 0, mRange.DrawingRange.X.Max.ToString("0"), true);
			}
			return zoom;
		}

		private static void DrawString(Graphics g, float zoom, decimal curX, decimal curY, string text, bool centerX = false, bool centerY = false, bool subtractX = false, bool subtractY = false)
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
					offsetX += ms.Height;

				g.DrawString(text, f, Brushes.Black, (float)curX - offsetX, (float)-curY - offsetY);

			}
			g.Restore(state);
		}



		System.Collections.Generic.IEnumerator<GrblCommand> IEnumerable<GrblCommand>.GetEnumerator()
		{ return list.GetEnumerator(); }


		public System.Collections.IEnumerator GetEnumerator()
		{ return list.GetEnumerator(); }
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

			public void UpdateRange(decimal x, decimal y)
			{
				X.UpdateRange(x);
				Y.UpdateRange(y);
			}

			public void ResetRange()
			{
				X.ResetRange();
				Y.ResetRange();
			}

			public bool ValidRange
			{ get { return X.ValidRange && Y.ValidRange; } }
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

		public void UpdateXYRange(decimal x, decimal y, bool drawing)
		{
			if (drawing)
				DrawingRange.UpdateRange(x, y);
			MovingRange.UpdateRange(x, y);
		}

		public void UpdateSRange(decimal s)
		{ SpindleRange.UpdateRange(s); }

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