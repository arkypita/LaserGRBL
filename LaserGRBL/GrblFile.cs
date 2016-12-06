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

		public int Count
		{ get { return list.Count; } }

		public TimeSpan EstimatedTime { get { return mEstimatedTimeOff + mEstimatedTimeOn; } }
		
		private void Analyze()
		{Process(null, Size.Empty);} //analyze only

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
				mRange.UpdateSRange(0);
				mRange.UpdateXYRange(0, 0, false);
				mTotalTravelOn = 0;
				mTotalTravelOff = 0;
				mEstimatedTimeOn = TimeSpan.Zero;
				mEstimatedTimeOff = TimeSpan.Zero;
			}


			foreach (GrblCommand cmd in list)
			{
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
					curAlpha = mRange.SpindleRange.ValidRange ? (int)(cmd.S.Number * 255 / mRange.SpindleRange.S.Max) : 255;

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
						{
							if (laser)
								mEstimatedTimeOn += TimeSpan.FromMinutes((double)distance / (double)speed);
							else
								mEstimatedTimeOff += TimeSpan.FromMinutes((double)distance / (double)speed);
						}
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
								pen.DashStyle = DashStyle.Dash;
								pen.DashPattern = new float[] { 4f, 4f };
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
					else if (analyze && cmd.IsPause)
					{
						//TimeSpan delay = cmd.P != null ? TimeSpan.FromMilliseconds((double)cmd.P.Number) : cmd.S != null ? TimeSpan.FromSeconds((double)cmd.S.Number) : TimeSpan.Zero;

						//grbl seem to use both P and S as number of seconds
						TimeSpan delay = cmd.P != null ? TimeSpan.FromSeconds((double)cmd.P.Number) : cmd.S != null ? TimeSpan.FromSeconds((double)cmd.S.Number) : TimeSpan.Zero;

						if (laser)
							mEstimatedTimeOn += delay;
						else
							mEstimatedTimeOff += delay;
					}

					curX = newX;
					curY = newY;
				}

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
		{return list.GetEnumerator();}
		
		
		public System.Collections.IEnumerator GetEnumerator()
		{return list.GetEnumerator();}
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
				{ get { return Min != decimal.MaxValue && Max != decimal.MinValue && Max > 0; } }
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
		{SpindleRange.UpdateRange(s);}

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