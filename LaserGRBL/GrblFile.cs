﻿using System;
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

		private abstract class ColorSegment
		{
			protected int  mColor;
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
			{get {return false;}}
			
			public bool Fast
			{get {return mConf.pwm ? mColor == 0 : mColor <= 125;}}
			
			public string formatnumber(double number)
			{ return number.ToString("#.###", System.Globalization.CultureInfo.InvariantCulture); }
		}
		
		private class XSegment : ColorSegment
		{
			public XSegment(int col, int len, bool rev, L2LConf c) : base(col, len, rev, c) {}
			
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
					return string.Format("X{0} Y{1} S{2}", formatnumber(mReverse ? - mLen : mLen), formatnumber(mReverse ? mLen : -mLen), mColor);
				else
					return string.Format("X{0} Y{1} {2}", formatnumber(mReverse ? -mLen : mLen), formatnumber(mReverse ? mLen : -mLen), Fast ? mConf.lOff : mConf.lOn);
			}
		}	

		private class VSeparator : ColorSegment
		{
			public VSeparator(L2LConf c) : base(0, 1, false, c) {}
			
			public override string ToString()
			{return string.Format("Y{0}", formatnumber(mLen));}
			
			public override bool IsSeparator
			{get {return true;}}
		}		

		private class HSeparator : ColorSegment
		{
			public HSeparator(L2LConf c) : base(0, 1, false, c) {}
			
			public override string ToString()
			{return string.Format("X{0}", formatnumber(mLen));}
			
			public override bool IsSeparator
			{get {return true;}}
		}



		public void LoadImagePotrace(Bitmap bmp, string filename, bool UseSpotRemoval, int SpotRemoval, bool UseSmoothing, decimal Smoothing, bool UseOptimize, decimal Optimize, L2LConf c)
		{
			bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

			long start = Tools.HiResTimer.TotalMilliseconds;
			mTotalTravelOff = 0;
			mTotalTravelOn = 0;
			mEstimatedTimeOff = TimeSpan.Zero;
			mEstimatedTimeOn = TimeSpan.Zero;
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
            public int startDelay;
            public RasterConverter.ImageProcessor.Direction dir;
			public bool pwm;
			public double fres;
			public bool vectorfilling;
		}

		public void LoadImageL2L(Bitmap bmp, string filename, L2LConf c)
		{

			bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

			long start = Tools.HiResTimer.TotalMilliseconds;
			mTotalTravelOff = 0;
			mTotalTravelOn = 0;
			mEstimatedTimeOff = TimeSpan.Zero;
			mEstimatedTimeOn = TimeSpan.Zero;
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
			bool cumulate = false, isLaserOn = false;

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
                        isLaserOn = false;
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
                        if (c.startDelay >= 1 && !isLaserOn)
                        {
                            rv.Add(new GrblCommand(String.Format("{0} {1}", c.lOn, c.pwm ? String.Format("S{0}", cmd.S.Number) : "")));
                            rv.Add(new GrblCommand(String.Format("G4 P{0}", c.startDelay)));
                        }
                        rv.Add(cmd);
                        isLaserOn = true;
                    }
				}
				catch (Exception ex) { throw ex; }
				finally { cmd.DeleteHelper(); }
			}

			return rv;
		}

		private List<ColorSegment> GetSegments(Bitmap bmp, L2LConf c)
		{
			List<ColorSegment> rv = new List<ColorSegment>();
			if (c.dir == RasterConverter.ImageProcessor.Direction.Horizontal || c.dir == RasterConverter.ImageProcessor.Direction.Vertical)
			{
				bool h = (c.dir == RasterConverter.ImageProcessor.Direction.Horizontal); //horizontal/vertical
				
				for (int i = 0; i < (h ? bmp.Height : bmp.Width); i++)
				{
					bool d = IsEven(i); //direct/reverse
					int prevCol = -1;
					int len = -1;

					for (int j = d ? 0 : (h ? bmp.Width - 1 : bmp.Height -1) ; d ? (j < (h ? bmp.Width : bmp.Height)) : (j >= 0) ; j = (d ? j+1 : j-1) )
						ExtractSegment(bmp, h ? j : i, h ? i : j, !d, ref len, ref prevCol, rv, c); //extract different segments

					if (h)
						rv.Add(new XSegment(prevCol, len + 1, !d, c)); //close last segment
					else
						rv.Add(new YSegment(prevCol, len + 1, !d, c)); //close last segment

					if (i < (h ? bmp.Height-1 : bmp.Width-1))
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
					bool d = IsEven(slice); //direct/reverse

			    	int prevCol = -1;
					int len = -1;
			    	
			        int z1 = slice < h ? 0 : slice - h + 1;
			        int z2 = slice < w ? 0 : slice - w + 1;
			        
					for (int j = (d ? z1 : slice - z2); d ? j <= slice - z2 : j >= z1 ; j = (d ? j+1 : j-1))
						ExtractSegment(bmp, j, slice - j, !d, ref len, ref prevCol, rv, c); //extract different segments
			        rv.Add(new DSegment(prevCol, len + 1, !d, c)); //close last segment

					//System.Diagnostics.Debug.WriteLine(String.Format("sl:{0} z1:{1} z2:{2}", slice, z1, z2));

					if (slice < Math.Min(w, h)-1) //first part of the image
					{
						if (d)
							rv.Add(new HSeparator(c)); //new line
						else
							rv.Add(new VSeparator(c)); //new line
					}
					else if (slice >= Math.Max(w, h)-1) //third part of image
					{
						if (d)
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

		public TimeSpan EstimatedTime { get { return mEstimatedTimeOff + mEstimatedTimeOn; } }

		private void Analyze()
		{ Process(null, Size.Empty); } //analyze only

		public void DrawOnGraphics(Graphics g, Size s)
		{ Process(g, s); }

		private void Process(Graphics g, Size s)
		{
			bool supportPWM = (bool)Settings.GetObject("Support Hardware PWM", true);
            bool laserMode = (bool)Settings.GetObject("Laser Mode", false);
			Boolean analyze = (g == null);
			Boolean drawing = (g != null);

			if (drawing && !mRange.DrawingRange.ValidRange)
				return;

			float zoom = drawing ? DrawJobRange(g, ref s) : 1;
			bool firstline = true;
			bool isLaserCutting = false;
            bool isLaserActive = false;
			decimal curX = 0;
			decimal curY = 0;
			decimal speed = 0;
			int curAlpha = 0;
			bool cw = false; //cw-ccw memo
			bool abs = false; //absolute-relative memo

			if (analyze)
			{
				mRange.ResetRange();
				mRange.UpdateXYRange(0, 0, false);
				mTotalTravelOn = 0;
				mTotalTravelOff = 0;
				mEstimatedTimeOn = TimeSpan.Zero;
				mEstimatedTimeOff = TimeSpan.Zero;
			}

			if (drawing && !mRange.SpindleRange.ValidRange) //assign max alpha if no S range available
				curAlpha = 255;

			foreach (GrblCommand cmd in list)
			{
				try
				{
					cmd.BuildHelper();

					TimeSpan delay = TimeSpan.Zero;

					if (cmd.IsLaserON)
					{
						isLaserActive = true;
						isLaserCutting = true;
					}
					else if (cmd.IsLaserOFF)
					{
						isLaserActive = false;
						isLaserCutting = false;
					}

					if (laserMode == true && isLaserActive == true)
					{
						if (cmd.IsRapidMovement == true)
						{
							isLaserCutting = false;
						}
						else
						{
							isLaserCutting = true;
						}
					}

					if (cmd.IsRelativeCoord)
						abs = false;
					if (cmd.IsAbsoluteCoord)
						abs = true;

					if (cmd.F != null)
						speed = cmd.F.Number;

					if (drawing && cmd.S != null)
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
							mRange.UpdateXYRange(newX, newY, isLaserCutting);

							decimal distance = 0;

							if (cmd.IsLinearMovement)
								distance = Tools.MathHelper.LinearDistance(curX, curY, newX, newY);
							else if (cmd.IsArcMovement) //arc of given radius
								distance = Tools.MathHelper.ArcDistance(curX, curY, newX, newY, cmd.GetArcRadius());

							if (isLaserCutting)
								mTotalTravelOn += distance;
							else
								mTotalTravelOff += distance;

							if (distance != 0 && speed != 0)
								delay = TimeSpan.FromMinutes((double)distance / (double)speed);
						}

						if (drawing)
						{
							Color linecolor = firstline ? ColorScheme.PreviewFirstMovement : isLaserCutting ? ColorScheme.PreviewLaserPower : ColorScheme.PreviewOtherMovement;
							using (Pen pen = GetPen(linecolor))
							{
								pen.ScaleTransform(1 / zoom, 1 / zoom);
								if (isLaserCutting)
									pen.Color = Color.FromArgb(curAlpha, pen.Color);

								if (!isLaserCutting)
								{
									if (supportPWM)
										pen.Color = Color.FromArgb(150, pen.Color);
									else
										pen.Color = Color.FromArgb(50, pen.Color);

									pen.DashStyle = DashStyle.Dash;
									pen.DashPattern = new float[] { 1f, 1f };
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
									{
										try { g.DrawArc(pen, (float)rectX, (float)rectY, (float)rectW, (float)rectH, (float)sA, (float)wA); }
										catch { System.Diagnostics.Debug.WriteLine(String.Format("Ex drwing arc: W{0} H{1}", rectW, rectH)); }
									}
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

					if (isLaserCutting)
						mEstimatedTimeOn += delay;
					else
						mEstimatedTimeOff += delay;

					if (analyze)
						cmd.SetOffset(mTotalTravelOn + mTotalTravelOff, mEstimatedTimeOn + mEstimatedTimeOff);
				}
				catch (Exception ex) { throw ex; }
				finally { cmd.DeleteHelper(); }
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

			using (Pen pen = GetPen(ColorScheme.PreviewJobRange))
			{
				pen.ScaleTransform(1 / zoom, 1 / zoom);
				pen.DashStyle = DashStyle.Dash;
				pen.DashPattern = new float[] { 1f, 2f };

				g.DrawLine(pen, 0, (float)mRange.DrawingRange.Y.Min, wSize.Width, (float)mRange.DrawingRange.Y.Min);
				DrawString(g, zoom, 0, mRange.DrawingRange.Y.Min, mRange.DrawingRange.Y.Min.ToString("0"), false, true, true, false);
				g.DrawLine(pen, 0, (float)mRange.DrawingRange.Y.Max, wSize.Width, (float)mRange.DrawingRange.Y.Max);
				DrawString(g, zoom, 0, mRange.DrawingRange.Y.Max, mRange.DrawingRange.Y.Max.ToString("0"), false, true, true, false);

				g.DrawLine(pen, (float)mRange.DrawingRange.X.Min, 0, (float)mRange.DrawingRange.X.Min, wSize.Height);
				DrawString(g, zoom, mRange.DrawingRange.X.Min, 0, mRange.DrawingRange.X.Min.ToString("0"), true, false, false, false);
				g.DrawLine(pen, (float)mRange.DrawingRange.X.Max, 0, (float)mRange.DrawingRange.X.Max, wSize.Height);
				DrawString(g, zoom, mRange.DrawingRange.X.Max, 0, mRange.DrawingRange.X.Max.ToString("0"), true, false, false, false);
			}
			return zoom;
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
					offsetX += ms.Height;

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