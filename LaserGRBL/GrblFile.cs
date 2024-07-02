//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Collections.Generic;
using System.Collections;
using CsPotrace;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using LaserGRBL.SvgConverter;
using LaserGRBL.Obj3D;
using SharpGL.SceneGraph;

namespace LaserGRBL
{
	public class GrblFile : IEnumerable<GrblCommand>
	{
		public enum CartesianQuadrant { I, II, III, IV, Mix, Unknown }

		public delegate void OnFileLoadedDlg(long elapsed, string filename);
		public event OnFileLoadedDlg OnFileLoading;
		public event OnFileLoadedDlg OnFileLoaded;

		private List<GrblCommand> list = new List<GrblCommand>();
		private ProgramRange mRange = new ProgramRange();
		private TimeSpan mEstimatedTotalTime;
		public List<GrblCommand> Commands => list;
		private Thread mLoadingThread = null;

		public GrblFile()
		{

		}

		public GrblFile(decimal x, decimal y, decimal x1, decimal y1)
		{
			mRange.UpdateXYRange(new GrblCommand.Element('X', x), new GrblCommand.Element('Y', y), false);
			mRange.UpdateXYRange(new GrblCommand.Element('X', x1), new GrblCommand.Element('Y', y1), false);
        }

        private void ClearList()
        {
            foreach (GrblCommand command in list)
            {
                command.Dispose();
            }
            list.Clear();
        }

        public void SaveGCODE(string filename, bool header, bool footer, bool between, int cycles, bool useLFLineEndings, GrblCore core)
		{
			try
			{
				using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filename))
				{
					if (useLFLineEndings)
						sw.NewLine = "\n";

					if (header)
						EvaluateAddLines(core, sw, Settings.GetObject("GCode.CustomHeader", GrblCore.GCODE_STD_HEADER));

					for (int i = 0; i < cycles; i++)
					{
						foreach (GrblCommand cmd in list)
							sw.WriteLine(cmd.Command);


						if (between && i < cycles - 1)
							EvaluateAddLines(core, sw, Settings.GetObject("GCode.CustomPasses", GrblCore.GCODE_STD_PASSES));
					}

					if (footer)
						EvaluateAddLines(core, sw, Settings.GetObject("GCode.CustomFooter", GrblCore.GCODE_STD_FOOTER));

					sw.Close();
				}
			}
			catch { }
		}

		private static void EvaluateAddLines(GrblCore core, System.IO.StreamWriter sw, string lines)
		{
			string[] arr = lines.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
			foreach (string line in arr)
			{
				if (line.Trim().Length > 0)
				{
					string command = core.EvaluateExpression(line);
					if (!string.IsNullOrEmpty(command))
						sw.WriteLine(command);
				}
			}
		}

		private void SafeLoadFile(ThreadStart loadFileAction)
        {
            if (CheckInUse()) return;
            mLoadingThread = new Thread(new ThreadStart(() =>
            {
				try
				{
					loadFileAction.Invoke();
                }
                catch (Exception ex)
                {
                    Logger.LogException("SafeLoadFile", ex);
                }
                finally
                {
                    mLoadingThread = null;
                }
            }));
            mLoadingThread.Start();
        }

		public void LoadFile(string filename, bool append)
        {
            SafeLoadFile(() =>
            {
				RiseOnFileLoading(filename);

				long start = Tools.HiResTimer.TotalMilliseconds;

				if (!append)
                    ClearList();

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

				RiseOnFileLoaded(filename, elapsed);
            });
		}

		public void LoadImportedSVG(string filename, bool append, GrblCore core, ColorFilter filter)
        {
            SafeLoadFile(() =>
            {
				RiseOnFileLoading(filename);

				long start = Tools.HiResTimer.TotalMilliseconds;

				if (!append)
                    ClearList();

				mRange.ResetRange();

				SvgConverter.GCodeFromSVG converter = new SvgConverter.GCodeFromSVG();
				converter.GCodeXYFeed = Settings.GetObject("GrayScaleConversion.VectorizeOptions.BorderSpeed", 1000);
				converter.UseLegacyBezier = !Settings.GetObject($"Vector.UseSmartBezier", true);

				string gcode = converter.convertFromFile(filename, core, filter);
				string[] lines = gcode.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
				foreach (string l in lines)
				{
					string line = l;
					if ((line = line.Trim()).Length > 0)
					{
						GrblCommand cmd = new GrblCommand(line);
						if (!cmd.IsEmpty)
							list.Add(cmd);
					}
				}

				Analyze();
				long elapsed = Tools.HiResTimer.TotalMilliseconds - start;

				RiseOnFileLoaded(filename, elapsed);
            });
        }


		private abstract class ColorSegment
		{
			public int mColor { get; set; }
			protected int mPixLen;

			public ColorSegment(int col, int len, bool rev)
			{
				mColor = col;
				mPixLen = rev ? -len : len;
			}

			public virtual bool IsSeparator
			{ get { return false; } }

			public bool Fast(L2LConf c)
			{ return c.pwm ? mColor == 0 : mColor <= 125; }

			public string formatnumber(int number, float offset, L2LConf c)
			{
				double dval = Math.Round(number / (c.vectorfilling ? c.fres : c.res) + offset, 3);
				return dval.ToString(System.Globalization.CultureInfo.InvariantCulture);
			}

			// Format laser power value
			// grbl                    with pwm : color can be between 0 and configured SMax - S128
			// smoothiware             with pwm : Value between 0.00 and 1.00    - S0.50
			// Marlin : Laser power can not be defined as switch (Add in comment hard coded changes)
			public string FormatLaserPower(int color, L2LConf c)
			{
				if (c.firmwareType == Firmware.Smoothie)
					return string.Format(System.Globalization.CultureInfo.InvariantCulture, "S{0:0.00}", color / 255.0); //maybe scaling to UI maxpower VS config maxpower instead of fixed / 255.0 ?
																														 //else if (c.firmwareType == Firmware.Marlin)
																														 //	return "";
				else
					return string.Format(System.Globalization.CultureInfo.InvariantCulture, "S{0}", color);
			}

			public abstract string ToGCodeNumber(ref int cumX, ref int cumY, L2LConf c);
		}

		private class XSegment : ColorSegment
		{
			public XSegment(int col, int len, bool rev) : base(col, len, rev) { }

			public override string ToGCodeNumber(ref int cumX, ref int cumY, L2LConf c)
			{
				cumX += mPixLen;

				if (c.pwm)
					return string.Format("X{0} {1}", formatnumber(cumX, c.oX, c), FormatLaserPower(mColor, c));
				else
					return string.Format("X{0} {1}", formatnumber(cumX, c.oX, c), Fast(c) ? c.lOff : c.lOn);
			}
		}

		private class YSegment : ColorSegment
		{
			public YSegment(int col, int len, bool rev) : base(col, len, rev) { }

			public override string ToGCodeNumber(ref int cumX, ref int cumY, L2LConf c)
			{
				cumY += mPixLen;

				if (c.pwm)
					return string.Format("Y{0} {1}", formatnumber(cumY, c.oY, c), FormatLaserPower(mColor, c));
				else
					return string.Format("Y{0} {1}", formatnumber(cumY, c.oY, c), Fast(c) ? c.lOff : c.lOn);
			}
		}

		private class DSegment : ColorSegment
		{
			public DSegment(int col, int len, bool rev) : base(col, len, rev) { }

			public override string ToGCodeNumber(ref int cumX, ref int cumY, GrblFile.L2LConf c)
			{
				cumX += mPixLen;
				cumY -= mPixLen;

				if (c.pwm)
					return string.Format("X{0} Y{1} {2}", formatnumber(cumX, c.oX, c), formatnumber(cumY, c.oY, c), FormatLaserPower(mColor, c));
				else
					return string.Format("X{0} Y{1} {2}", formatnumber(cumX, c.oX, c), formatnumber(cumY, c.oY, c), Fast(c) ? c.lOff : c.lOn);
			}
		}

		private class VSeparator : ColorSegment
		{
			public VSeparator() : base(0, 1, false) { }

			public override string ToGCodeNumber(ref int cumX, ref int cumY, L2LConf c)
			{
				if (mPixLen < 0)
					throw new Exception();

				cumY += mPixLen;
				return string.Format("Y{0}", formatnumber(cumY, c.oY, c));
			}

			public override bool IsSeparator
			{ get { return true; } }
		}

		private class HSeparator : ColorSegment
		{
			public HSeparator() : base(0, 1, false) { }

			public override string ToGCodeNumber(ref int cumX, ref int cumY, L2LConf c)
			{
				if (mPixLen < 0)
					throw new Exception();

				cumX += mPixLen;
				return string.Format("X{0}", formatnumber(cumX, c.oX, c));
			}

			public override bool IsSeparator
			{ get { return true; } }
		}

		public static bool RasterFilling(RasterConverter.ImageProcessor.Direction dir)
		{
			return dir == RasterConverter.ImageProcessor.Direction.Diagonal || dir == RasterConverter.ImageProcessor.Direction.Horizontal || dir == RasterConverter.ImageProcessor.Direction.Vertical;
		}
		public static bool VectorFilling(RasterConverter.ImageProcessor.Direction dir)
		{
			return dir == RasterConverter.ImageProcessor.Direction.NewDiagonal ||
			dir == RasterConverter.ImageProcessor.Direction.NewHorizontal ||
			dir == RasterConverter.ImageProcessor.Direction.NewVertical ||
			dir == RasterConverter.ImageProcessor.Direction.NewReverseDiagonal ||
			dir == RasterConverter.ImageProcessor.Direction.NewGrid ||
			dir == RasterConverter.ImageProcessor.Direction.NewDiagonalGrid ||
			dir == RasterConverter.ImageProcessor.Direction.NewCross ||
			dir == RasterConverter.ImageProcessor.Direction.NewDiagonalCross ||
			dir == RasterConverter.ImageProcessor.Direction.NewSquares ||
			dir == RasterConverter.ImageProcessor.Direction.NewZigZag ||
			dir == RasterConverter.ImageProcessor.Direction.NewHilbert ||
			dir == RasterConverter.ImageProcessor.Direction.NewInsetFilling;
		}

		public static bool TimeConsumingFilling(RasterConverter.ImageProcessor.Direction dir)
		{
			return
			dir == RasterConverter.ImageProcessor.Direction.NewCross ||
			dir == RasterConverter.ImageProcessor.Direction.NewDiagonalCross ||
			dir == RasterConverter.ImageProcessor.Direction.NewSquares;
		}

		public bool CheckInUse(bool showMessageBox = true)
		{
            if (mLoadingThread != null || InUse)
            {
                if(showMessageBox)
					MessageBox.Show(Strings.AlreadyLoading);

				return true;
            }
			return false;
        }


		public void LoadImagePotrace(Bitmap bmp, string filename, bool UseSpotRemoval, int SpotRemoval, bool UseSmoothing, decimal Smoothing, bool UseOptimize, decimal Optimize, bool useOptimizeFast, L2LConf c, bool append, GrblCore core)
		{
            if (CheckInUse()) return;

            skipcmd = Settings.GetObject("Disable G0 fast skip", false) ? "G1" : "G0";

			RiseOnFileLoading(filename);

			bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
			long start = Tools.HiResTimer.TotalMilliseconds;

			if (!append)
                ClearList();

			//list.Add(new GrblCommand("G90")); //absolute (Moved to custom Header)

			mRange.ResetRange();

			Potrace.turdsize = (int)(UseSpotRemoval ? SpotRemoval : 2);
			Potrace.alphamax = UseSmoothing ? (double)Smoothing : 0.0;
			Potrace.opttolerance = UseOptimize ? (double)Optimize : 0.2;
			Potrace.curveoptimizing = UseOptimize; //optimize the path p, replacing sequences of Bezier segments by a single segment when possible.

			List<List<Curve>> plist = Potrace.PotraceTrace(bmp);
			List<List<Curve>> flist = null;


			if (VectorFilling(c.dir))
			{
				flist = PotraceClipper.BuildFilling(plist, bmp.Width, bmp.Height, c);
				flist = ParallelOptimizePaths(flist, 0 /*ComputeDirectionChangeCost(c, core, false)*/);
			}
			if (RasterFilling(c.dir))
			{
				using (Bitmap ptb = new Bitmap(bmp.Width, bmp.Height))
				{
					using (Graphics g = Graphics.FromImage(ptb))
					{
						double inset = Math.Max(1, c.res / c.fres); //bordino da togliere per finire un po' prima del bordo

						Potrace.Export2GDIPlus(plist, g, Brushes.Black, null, inset);

						using (Bitmap resampled = RasterConverter.ImageTransform.ResizeImage(ptb, new Size((int)(bmp.Width * c.fres / c.res) + 1, (int)(bmp.Height * c.fres / c.res) + 1), true, InterpolationMode.HighQualityBicubic))
						{
							if (c.pwm)
								list.Add(new GrblCommand(String.Format("{0} S0", c.lOn))); //laser on and power to zero
							else
								list.Add(new GrblCommand(String.Format($"{c.lOff} S{GrblCore.Configuration.MaxPWM}"))); //laser off and power to max power

							//set speed to markspeed
							// For marlin, need to specify G1 each time :
							// list.Add(new GrblCommand(String.Format("G1 F{0}", c.markSpeed)));
							list.Add(new GrblCommand(String.Format("F{0}", c.markSpeed)));

							c.vectorfilling = true;
							ImageLine2Line(resampled, c);

							//laser off
							list.Add(new GrblCommand(c.lOff));
						}
					}
				}
			}

			bool supportPWM = Settings.GetObject("Support Hardware PWM", true);


			if (supportPWM)
				list.Add(new GrblCommand($"{c.lOn} S0"));   //laser on and power to 0
			else
				list.Add(new GrblCommand($"{c.lOff} S{GrblCore.Configuration.MaxPWM}"));   //laser off and power to maxPower

			//trace raster filling
			if (flist != null)
			{
				List<string> gc = new List<string>();
				if (supportPWM)
					gc.AddRange(Potrace.Export2GCode(flist, c.oX, c.oY, c.res, $"S{c.maxPower}", "S0", bmp.Size, skipcmd));
				else
					gc.AddRange(Potrace.Export2GCode(flist, c.oX, c.oY, c.res, c.lOn, c.lOff, bmp.Size, skipcmd));

				list.Add(new GrblCommand(String.Format("F{0}", c.markSpeed)));
				foreach (string code in gc)
					list.Add(new GrblCommand(code));
			}


			//trace borders
			if (plist != null) //always true
			{
				//Optimize fast movement
				if (useOptimizeFast)
					plist = OptimizePaths(plist, 0 /*ComputeDirectionChangeCost(c, core, true)*/);
				else
					plist.Reverse(); //la lista viene fornita da potrace con prima esterni e poi interni, ma per il taglio è meglio il contrario

				List<string> gc = new List<string>();
				if (supportPWM)
					gc.AddRange(Potrace.Export2GCode(plist, c.oX, c.oY, c.res, $"S{c.maxPower}", "S0", bmp.Size, skipcmd));
				else
					gc.AddRange(Potrace.Export2GCode(plist, c.oX, c.oY, c.res, c.lOn, c.lOff, bmp.Size, skipcmd));

				// For marlin, need to specify G1 each time :
				//list.Add(new GrblCommand(String.Format("G1 F{0}", c.borderSpeed)));
				list.Add(new GrblCommand(String.Format("F{0}", c.borderSpeed)));
				foreach (string code in gc)
					list.Add(new GrblCommand(code));
			}

			//if (supportPWM)
			//	gc = Potrace.Export2GCode(flist, c.oX, c.oY, c.res, $"S{c.maxPower}", "S0", bmp.Size, skipcmd);
			//else
			//	gc = Potrace.Export2GCode(flist, c.oX, c.oY, c.res, c.lOn, c.lOff, bmp.Size, skipcmd);

			//foreach (string code in gc)
			//	list.Add(new GrblCommand(code));


			//laser off (superflua??)
			if (supportPWM)
				list.Add(new GrblCommand(c.lOff));  //necessaria perché finisce con solo S0

			Analyze();
			long elapsed = Tools.HiResTimer.TotalMilliseconds - start;

			RiseOnFileLoaded(filename, elapsed);
		}

		private void RiseOnFileLoaded(string filename, long elapsed)
		{
			if (OnFileLoaded != null)
				OnFileLoaded(elapsed, filename);
		}

		private void RiseOnFileLoading(string filename)
		{
			if (OnFileLoading != null)
				OnFileLoading(0, filename);
		}

		public class L2LConf
		{
			public double res;
			public float oX;
			public float oY;
			public int markSpeed;
			public int borderSpeed;
			public int minPower;
			public int maxPower;
			public string lOn;
			public string lOff;
			public RasterConverter.ImageProcessor.Direction dir;
			public bool pwm;
			public double fres;
			public bool vectorfilling;
			public Firmware firmwareType;
		}

		private string skipcmd = "G0";
		public void LoadImageL2L(Bitmap bmp, string filename, L2LConf c, bool append, GrblCore core)
        {
            if (CheckInUse()) return;

            skipcmd = Settings.GetObject("Disable G0 fast skip", false) ? "G1" : "G0";

			RiseOnFileLoading(filename);

			bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

			long start = Tools.HiResTimer.TotalMilliseconds;

			if (!append)
                ClearList();

			mRange.ResetRange();

			//absolute
			//list.Add(new GrblCommand("G90")); //(Moved to custom Header)

			//move fast to offset (or slow if disable G0) and set mark speed
			list.Add(new GrblCommand(String.Format("{0} X{1} Y{2} F{3}", skipcmd, formatnumber(c.oX), formatnumber(c.oY), c.markSpeed)));
			if (c.pwm)
				list.Add(new GrblCommand(String.Format("{0} S0", c.lOn))); //laser on and power to zero
			else
				list.Add(new GrblCommand($"{c.lOff} S{GrblCore.Configuration.MaxPWM}")); //laser off and power to maxpower

			//set speed to markspeed						
			// For marlin, need to specify G1 each time :
			//list.Add(new GrblCommand(String.Format("G1 F{0}", c.markSpeed)));
			//list.Add(new GrblCommand(String.Format("F{0}", c.markSpeed))); //replaced by the first move to offset and set speed

			ImageLine2Line(bmp, c);

			//laser off
			list.Add(new GrblCommand(c.lOff));

			//move fast to origin
			//list.Add(new GrblCommand("G0 X0 Y0")); //moved to custom footer

			Analyze();
			long elapsed = Tools.HiResTimer.TotalMilliseconds - start;

			RiseOnFileLoaded(filename, elapsed);
		}

		internal void GenerateCuttingTest(int f_col, int f_start, int f_end, int p_start, int p_end, int s_fixed, int f_text, int s_text, string title, string ton)
        {
            if (CheckInUse()) return;

            int p_row = p_end - p_start + 1;
			double ox = 3;
			double oy = 3;
			string filename = "Cutting Test";

			int x_size = f_col * 14 - 4;
			int y_size = p_row * 14 - 4;

			RiseOnFileLoading(filename);

			long start = Tools.HiResTimer.TotalMilliseconds;

            ClearList();
			mRange.ResetRange();

			double f_delta = f_col > 1 ? (f_end - f_start) / (double)(f_col - 1) : 0;

			//back to origin
			list.Add(new GrblCommand(String.Format("G0 X{0} Y{1} S{2}", formatnumber(ox), formatnumber(oy), formatnumber(s_fixed))));
			list.Add(new GrblCommand($"G1 {ton} F{formatnumber(f_start)}"));

			double cx = ox;
			double cy = oy;

			for (int p = 0; p < p_row; p++) // rows
			{
				cy = oy + 14 * p;
				for (int f = 0; f < f_col; f += 1) //cols
				{
					cx = ox + 14 * f;
					for (int pass = 0; pass < p_start + p; pass++)
					{
						//move to position
						list.Add(new GrblCommand(String.Format("G0 X{0} Y{1}", formatnumber(cx), formatnumber(cy))));
						//now draw rectangle
						list.Add(new GrblCommand(String.Format("G1 X{0} F{1} {2}", formatnumber(cx+10), formatnumber(f_start + f_delta * f), ton))); //Laser ON
						list.Add(new GrblCommand(String.Format("G1 Y{0}", formatnumber(cy+10))));
						list.Add(new GrblCommand(String.Format("G1 X{0}", formatnumber(cx))));
						list.Add(new GrblCommand(String.Format("G1 Y{0}", formatnumber(cy))));
						list.Add(new GrblCommand("M5")); // Laser OFF
					}
				}
			}
			
			//back to origin
			list.Add(new GrblCommand(String.Format("G0 X{0} Y{1} S0", formatnumber(ox), formatnumber(oy))));
			list.Add(new GrblCommand($"G1 {ton} F{formatnumber(f_text)}"));


			for (int x = 0; x < f_col; x++)
				list.AddRange(Hershey.Hershey.CreateString($"F{(int)(f_start + (x * f_delta))}", (x * 14) + (10 / 2) + ox, oy / 2, f_text, s_text, true, "M4"));
			for (int y = 0; y < p_row; y++)
				list.AddRange(Hershey.Hershey.CreateString($"{(int)(p_start + y)}pass", ox / 2, (y * 14) + (10 / 2) + oy, f_text, s_text, false, "M4"));

			string srange = $"S{s_fixed}";
			string frange = (f_start != f_end) ? $"F{f_start} - F{f_end}" : $"F{f_end}";
			string prange = (p_start != p_end) ? $"{p_start} - F{p_end} pass" : $"{p_end} pass";


			if (string.IsNullOrEmpty(title))
				title = "LaserGRBL cutting test";
			else
				title = $"LaserGRBL cutting test [{title}]";
			string parmessage = $"{srange}, {frange}, {prange}, {ton}";
			list.AddRange(Hershey.Hershey.CreateString(title, x_size / 2 + ox, y_size + oy + oy + oy / 2, f_text, s_text, true, "M4"));
			list.AddRange(Hershey.Hershey.CreateString(parmessage, x_size / 2 + ox, y_size + oy + oy / 2, f_text, s_text, true, "M4"));

			//laser off
			list.Add(new GrblCommand("M5"));


			Analyze();
			long elapsed = Tools.HiResTimer.TotalMilliseconds - start;

			RiseOnFileLoaded(filename, elapsed);
		}

		public void GenerateGreyscaleTest(int f_row, int s_col, int f_start, int f_end, int s_start, int s_end, int x_size, int y_size, double resolution, int f_grid, int s_grid, string title, int f_text, int s_text, string ton)
        {
            if (CheckInUse()) return;

            //string text = Hershey.Hershey.Test();

            double ox = 3;
			double oy = 3;
			string filename = "PowerSpeed Test";

			RiseOnFileLoading(filename);

			long start = Tools.HiResTimer.TotalMilliseconds;

            ClearList();
			mRange.ResetRange();

			bool forward = true;
			double f_delta = f_row > 1 ? (f_end - f_start) / (double)(f_row-1) : 0;
			double s_delta = s_col > 1 ? (s_end - s_start) / (double)(s_col-1) : 0;

			double x_step = x_size / (double)s_col;
			double y_step = y_size / (double)f_row;
			double filling_step = 1 / resolution;

			//back to origin
			list.Add(new GrblCommand(String.Format("G0 X{0} Y{1} S0", formatnumber(ox), formatnumber(oy))));
			list.Add(new GrblCommand($"G1 {ton} F{formatnumber(f_start)}"));

			//draw filling
			double prevF = double.NaN, curF = double.NaN;
			forward = true;
			for (double y = 0; y <= y_size; y += filling_step)
			{
				curF = f_start + ((int)(y / y_step)) * f_delta;

				if (curF != prevF)
				{
					list.Add(new GrblCommand($"F{formatnumber(curF)}"));
					prevF = curF;
				}

				list.Add(new GrblCommand($"Y{formatnumber(oy + y)} S0"));

				for (int x = 0; x < s_col; x++)
				{
					double cx = forward ? ((x + 1) * x_step) : x_size - ((x + 1) * x_step);
					double cs = forward ? s_start + (x * s_delta) : s_end - (x * s_delta);

					list.Add(new GrblCommand(String.Format("X{0} S{1}", formatnumber(ox + cx), formatnumber(cs))));
				}

				forward = !forward;
			}

			//back to origin
			list.Add(new GrblCommand(String.Format("G0 X{0} Y{1} S0", formatnumber(ox), formatnumber(oy))));
			list.Add(new GrblCommand($"G1 {ton} F{formatnumber(f_grid)}"));

			// draw grid X
			forward = true;
			for (int y = 0; y < f_row + 1; y++)
			{
				double cy = y * y_step;
				list.Add(new GrblCommand(String.Format("Y{0} S0", formatnumber(oy + cy))));

				for (int x = 0; x < s_col + 1; x++)
				{
					double cx = forward ? x * x_step : x_size - x * x_step;
					list.Add(new GrblCommand(String.Format("X{0} S{1}", formatnumber(ox + cx), formatnumber(s_grid))));
				}

				forward = !forward;
			}

			//back to origin
			list.Add(new GrblCommand(String.Format("G0 X{0} Y{1} S0", formatnumber(ox), formatnumber(oy))));
			list.Add(new GrblCommand($"G1 {ton} F{formatnumber(f_grid)}"));

			// draw grid Y
			forward = true;
			for (int x = 0; x < s_col + 1; x++)
			{
				double cx = x * x_step;
				list.Add(new GrblCommand(String.Format("X{0} S0", formatnumber(ox + cx))));

				for (int y = 0; y < f_row + 1; y++)
				{
					double cy = forward ? y * y_step : y_size - y * y_step;
					list.Add(new GrblCommand(String.Format("Y{0} S{1}", formatnumber(oy + cy), formatnumber(s_grid))));
				}

				forward = !forward;
			}

			for (int x = 0; x < s_col; x++)
				list.AddRange( Hershey.Hershey.CreateString($"S{(int)(s_start + (x * s_delta))}", (x*x_step) + (x_step/2) + ox, oy / 2, f_text, s_text, true, "M4"));
			for (int y = 0; y < f_row; y++)
				list.AddRange(Hershey.Hershey.CreateString($"F{(int)(f_start + (y * f_delta))}", ox / 2, (y * y_step) + (y_step / 2) + oy, f_text, s_text, false, "M4"));

			string srange = (s_start != s_end) ? $"S{s_start} - S{s_end}" : $"S{s_end}";
			string frange = (f_start != f_end) ? $"F{f_start} - F{f_end}" : $"F{f_end}";

			
			if (string.IsNullOrEmpty(title))
				title = "LaserGRBL power/speed test";
			else
				title = $"LaserGRBL power/speed test [{title}]";
			string parmessage = $"{srange}, {frange}, {ton},  {formatnumber(resolution)} line/mm";
			list.AddRange(Hershey.Hershey.CreateString(title, x_size / 2 + ox, y_size + oy + oy + oy/2, f_text, s_text, true, "M4"));
			list.AddRange(Hershey.Hershey.CreateString(parmessage, x_size / 2 + ox, y_size + oy + oy / 2, f_text, s_text, true, "M4"));

			//laser off
			list.Add(new GrblCommand("M5"));


			Analyze();
			long elapsed = Tools.HiResTimer.TotalMilliseconds - start;

			RiseOnFileLoaded(filename, elapsed);
		}

		internal void GenerateShakeTest(string axis, int flimit, int axislen, int cpower, int cspeed)
        {
            if (CheckInUse()) return;

            string filename = $"Shake Test {axis}";

			RiseOnFileLoading(filename);

			long start = Tools.HiResTimer.TotalMilliseconds;

            ClearList();
			mRange.ResetRange();

			list.Add(new GrblCommand("M5"));                    //laser OFF
			list.Add(new GrblCommand("G1 F1000 X0 Y0 S0"));     //move to origin (slowly)
			list.Add(new GrblCommand($"G1 F{cspeed} X7 Y10"));        //positioning
			list.Add(new GrblCommand("M4"));                    //laser ON
			list.Add(new GrblCommand($"G1 F{cspeed} S{cpower} X13 Y10"));        //drow cross
			list.Add(new GrblCommand("M5"));                    //laser OFF
			list.Add(new GrblCommand($"G1 F{cspeed} X10 Y7"));        //positioning
			list.Add(new GrblCommand("M4"));                    //laser ON
			list.Add(new GrblCommand($"G1 F{cspeed} S{cpower} X10 Y13"));        //drow cross
			list.Add(new GrblCommand("M5"));                    //laser OFF
			list.Add(new GrblCommand("G1 F1000 X10 Y10 S0"));     //move to cross center (slowly)

			GenerateShakeTest2(axis, flimit, axislen, 10, 50, 0.5);
			GenerateShakeTest2(axis, flimit, axislen, 10, 100, 2);
			GenerateShakeTest2(axis, flimit, axislen, 10, 200, 4);
			GenerateShakeTest2(axis, flimit, axislen, 10, 400, 8);

			list.Add(new GrblCommand($"G1 F{cspeed} X10 Y10 S0"));     //move to cross center (fast)

			list.Add(new GrblCommand($"G1 F{cspeed} X7 Y10"));        //positioning
			list.Add(new GrblCommand("M4"));                    //laser ON
			list.Add(new GrblCommand($"G1 F{cspeed} S{cpower} X13 Y10"));        //drow cross
			list.Add(new GrblCommand("M5"));                    //laser OFF
			list.Add(new GrblCommand($"G1 F{cspeed} X10 Y7"));        //positioning
			list.Add(new GrblCommand("M4"));                    //laser ON
			list.Add(new GrblCommand($"G1 F{cspeed} S{cpower} X10 Y13"));        //drow cross
			list.Add(new GrblCommand("M5"));                    //laser OFF
			list.Add(new GrblCommand("G1 F1000 X0 Y0 S0"));     //move to origin (slowly)

			Analyze();
			long elapsed = Tools.HiResTimer.TotalMilliseconds - start;

			RiseOnFileLoaded(filename, elapsed);
		}

		long map(long x, long in_min, long in_max, long out_min, long out_max)
		{
			return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
		}

		private void GenerateShakeTest2(string axis, int flimit, int axislen, int o, int trip, double step)
        {
            for (int c = trip / 2; c < axislen - trip / 2; c += trip) //centro dei punti di oscillazione
			{
				for (double i = 0; i < trip / 3; i += step)
				{
					list.Add(new GrblCommand($"G1 F{flimit} {axis}{formatnumber(o + c + i)}"));
					list.Add(new GrblCommand($"G1 F{flimit} {axis}{formatnumber(o + c - i)}"));
				}
			}
		}

        //internal void GenerateShakeTest(string axis, int flimit, int axislen, int cpower, int cspeed)
        //{
        //	string filename = $"Shake Test {axis}";

        //	RiseOnFileLoading(filename);

        //	long start = Tools.HiResTimer.TotalMilliseconds;

        //	ClearList();
        //	mRange.ResetRange();

        //	list.Add(new GrblCommand("M5"));                    //laser OFF
        //	list.Add(new GrblCommand("G1 F1000 X0 Y0 S0"));     //move to origin (slowly)
        //	list.Add(new GrblCommand($"G1 F{cspeed} X7 Y10"));        //positioning
        //	list.Add(new GrblCommand("M4"));                    //laser ON
        //	list.Add(new GrblCommand($"G1 F{cspeed} S{cpower} X13 Y10"));        //drow cross
        //	list.Add(new GrblCommand("M5"));                    //laser OFF
        //	list.Add(new GrblCommand($"G1 F{cspeed} X10 Y7"));        //positioning
        //	list.Add(new GrblCommand("M4"));                    //laser ON
        //	list.Add(new GrblCommand($"G1 F{cspeed} S{cpower} X10 Y13"));        //drow cross
        //	list.Add(new GrblCommand("M5"));                    //laser OFF
        //	list.Add(new GrblCommand("G1 F1000 X10 Y10 S0"));     //move to cross center (slowly)

        //	int ca = 10;
        //	int da = 1;
        //	while ((ca + da) < axislen)
        //	{
        //		ca += da;
        //		list.Add(new GrblCommand($"G1 F{flimit} {axis}{ca}"));
        //		ca -= da;
        //		list.Add(new GrblCommand($"G1 F{flimit} {axis}{ca}"));
        //		da += 5;
        //	}

        //	list.Add(new GrblCommand($"G1 F{cspeed} X7 Y10"));        //positioning
        //	list.Add(new GrblCommand("M4"));                    //laser ON
        //	list.Add(new GrblCommand($"G1 F{cspeed} S{cpower} X13 Y10"));        //drow cross
        //	list.Add(new GrblCommand("M5"));                    //laser OFF
        //	list.Add(new GrblCommand($"G1 F{cspeed} X10 Y7"));        //positioning
        //	list.Add(new GrblCommand("M4"));                    //laser ON
        //	list.Add(new GrblCommand($"G1 F{cspeed} S{cpower} X10 Y13"));        //drow cross
        //	list.Add(new GrblCommand("M5"));                    //laser OFF

        //	Analyze();
        //	long elapsed = Tools.HiResTimer.TotalMilliseconds - start;

        //	RiseOnFileLoaded(filename, elapsed);
        //}

        // For Marlin, as we sen M106 command, we need to know last color send
        //private int lastColorSend = 0;
        private void ImageLine2Line(Bitmap bmp, L2LConf c)
		{
			bool fast = true;
			List<ColorSegment> segments = GetSegments(bmp, c);
			List<GrblCommand> temp = new List<GrblCommand>();

			int cumX = 0;
			int cumY = 0;

			foreach (ColorSegment seg in segments)
			{
				bool changeGMode = (fast != seg.Fast(c)); //se veloce != dafareveloce

				if (seg.IsSeparator && !fast) //fast = previous segment contains S0 color
				{
					if (c.pwm)
						temp.Add(new GrblCommand("S0"));
					else
						temp.Add(new GrblCommand(c.lOff)); //laser off
				}

				fast = seg.Fast(c);

				// For marlin firmware, we must defined laser power before moving (unsing M106 or M107)
				// So we have to speficy gcode (G0 or G1) each time....
				//if (c.firmwareType == Firmware.Marlin)
				//{
				//	// Add M106 only if color has changed
				//	if (lastColorSend != seg.mColor)
				//		temp.Add(new GrblCommand(String.Format("M106 P1 S{0}", fast ? 0 : seg.mColor)));
				//	lastColorSend = seg.mColor;
				//	temp.Add(new GrblCommand(String.Format("{0} {1}", fast ? "G0" : "G1", seg.ToGCodeNumber(ref cumX, ref cumY, c))));
				//}
				//else
				//{

				if (changeGMode)
					temp.Add(new GrblCommand(String.Format("{0} {1}", fast ? skipcmd : "G1", seg.ToGCodeNumber(ref cumX, ref cumY, c))));
				else
					temp.Add(new GrblCommand(seg.ToGCodeNumber(ref cumX, ref cumY, c)));

				//}
			}

			temp = OptimizeLine2Line(temp, c);
			list.AddRange(temp);
		}


		private List<GrblCommand> OptimizeLine2Line(List<GrblCommand> temp, L2LConf c)
		{
			List<GrblCommand> rv = new List<GrblCommand>();

			decimal curX = (decimal)c.oX;
			decimal curY = (decimal)c.oY;
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
							rv.Add(new GrblCommand(string.Format("{0} X{1} Y{2} S0", skipcmd, formatnumber((double)curX), formatnumber((double)curY))));
						else
							rv.Add(new GrblCommand(string.Format("{0} X{1} Y{2} {3}", skipcmd, formatnumber((double)curX), formatnumber((double)curY), c.lOff)));

						//curX = curY = 0;
					}

					if (cmd.IsMovement)
					{
						if (cmd.X != null) curX = cmd.X.Number;
						if (cmd.Y != null) curY = cmd.Y.Number;
					}

					if (!cmd.IsMovement || !cumulate)
						rv.Add(cmd);
				}
				catch (Exception ex) { throw ex; }
				finally { cmd.DeleteHelper(); }
			}

			return rv;
		}

		private List<ColorSegment> GetSegments(Bitmap bmp, L2LConf c)
		{
			bool uni = Settings.GetObject("Unidirectional Engraving", false);

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
						rv.Add(new XSegment(prevCol, len + 1, !d)); //close last segment
					else
						rv.Add(new YSegment(prevCol, len + 1, !d)); //close last segment

					if (uni) // add "go back"
					{
						if (h) rv.Add(new XSegment(0, bmp.Width, true));
						else rv.Add(new YSegment(0, bmp.Height, true));
					}

					if (i < (h ? bmp.Height - 1 : bmp.Width - 1))
					{
						if (h)
							rv.Add(new VSeparator()); //new line
						else
							rv.Add(new HSeparator()); //new line
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

				rv.Add(new VSeparator()); //new line

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
					rv.Add(new DSegment(prevCol, len + 1, !d)); //close last segment

					//System.Diagnostics.Debug.WriteLine(String.Format("sl:{0} z1:{1} z2:{2}", slice, z1, z2));

					if (uni) // add "go back"
					{
						int slen = (slice - z1 - z2) + 1;
						rv.Add(new DSegment(0, slen, true));
						//System.Diagnostics.Debug.WriteLine(slen);
					}

					if (slice < Math.Min(w, h) - 1) //first part of the image
					{
						if (d && !uni)
							rv.Add(new HSeparator()); //new line
						else
							rv.Add(new VSeparator()); //new line
					}
					else if (slice >= Math.Max(w, h) - 1) //third part of image
					{
						if (d && !uni)
							rv.Add(new VSeparator()); //new line
						else
							rv.Add(new HSeparator()); //new line
					}
					else //central part of the image
					{
						if (w > h)
							rv.Add(new HSeparator()); //new line
						else
							rv.Add(new VSeparator()); //new line
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
					rv.Add(new XSegment(prevCol, len, reverse));
				else if (c.dir == RasterConverter.ImageProcessor.Direction.Vertical)
					rv.Add(new YSegment(prevCol, len, reverse));
				else if (c.dir == RasterConverter.ImageProcessor.Direction.Diagonal)
					rv.Add(new DSegment(prevCol, len, reverse));

				len = 0;
			}

			prevCol = col;
		}

		private List<List<Curve>> ParallelOptimizePaths(List<List<Curve>> list, double changecost)
		{
			if (list == null || list.Count <= 1)
				return list;

			int maxblocksize = 2048;    //max number of List<Curve> to process in a single OptimizePaths operation

			int blocknum = (int)Math.Ceiling(list.Count / (double)maxblocksize);
			if (blocknum <= 1)
				return OptimizePaths(list, changecost);

			System.Diagnostics.Debug.WriteLine("Count: " + list.Count);

			Task<List<List<Curve>>>[] taskArray = new Task<List<List<Curve>>>[blocknum];
			for (int i = 0; i < taskArray.Length; i++)
				taskArray[i] = Task.Factory.StartNew((data) => OptimizePaths((List<List<Curve>>)data, changecost), GetTaskJob(i, taskArray.Length, list));
			Task.WaitAll(taskArray);

			List<List<Curve>> rv = new List<List<Curve>>();
			for (int i = 0; i < taskArray.Length; i++)
			{
				List<List<Curve>> lc = taskArray[i].Result;
				rv.AddRange(lc);
			}

			return rv;
		}

		private List<List<Curve>> GetTaskJob(int threadIndex, int threadCount, List<List<Curve>> list)
		{
			int from = (threadIndex * list.Count) / threadCount;
			int to = ((threadIndex + 1) * list.Count) / threadCount;

			List<List<Curve>> rv = list.GetRange(from, to - from);
			System.Diagnostics.Debug.WriteLine($"Thread {threadIndex}/{threadCount}: {rv.Count} [from {from} to {to}]");
			return rv;
		}

		private List<List<Curve>> OptimizePaths(List<List<Curve>> list, double changecost)
		{
			if (list.Count <= 1)
				return list;


			dPoint Origin = new dPoint(0, 0);
			int nearestToZero = 0;
			double bestDistanceToZero = Double.MaxValue;

			double[,] costs = new double[list.Count, list.Count];   //array bidimensionale dei costi di viaggio dal punto finale della curva 1 al punto iniziale della curva 2
			for (int c1 = 0; c1 < list.Count; c1++)                 //ciclo due volte sulla lista di curve
			{
				dPoint c1fa = list[c1].First().A;	//punto iniziale del primo segmento del percorso (per calcolo distanza dallo zero)
				//dPoint c1la = list[c1].Last().A;	//punto iniziale dell'ulimo segmento del percorso (per calcolo direzione di uscita)
				dPoint c1lb = list[c1].Last().B;	//punto finale dell'ultimo segmento del percorso (per calcolo distanza tra percorsi e direzione di uscita e ingresso)
				

				for (int c2 = 0; c2 < list.Count; c2++)             //con due indici diversi c1, c2
				{
					dPoint c2fa = list[c2].First().A;     //punto iniziale del primo segmento del percorso (per calcolo distanza tra percorsi e direzione di ingresso)
					//dPoint c2fb = list[c2].First().B;     //punto finale del primo segmento del percorso (per calcolo direzione di continuazione)

					if (c1 == c2)
						costs[c1, c2] = double.MaxValue;  //distanza del punto con se stesso (caso degenere)
					else
						costs[c1, c2] = SquareDistance(c1lb, c2fa); //TravelCost(c1la, c1lb, c2fa, c2fb, changecost);
				}

				//trova quello che parte più vicino allo zero
				double distZero = SquareDistanceZero(c1fa);
				if (distZero < bestDistanceToZero)
				{
					nearestToZero = c1;
					bestDistanceToZero = distZero;
				}
			}

			//Create a list of unvisited places
			List<int> unvisited = Enumerable.Range(0, list.Count).ToList();

			//Pick nearest points
			List<List<CsPotrace.Curve>> bestPath = new List<List<Curve>>();

			//parti da quello individuato come "il più vicino allo zero"
			bestPath.Add(list[nearestToZero]);
			unvisited.Remove(nearestToZero);
			int lastIndex = nearestToZero;
			
			while (unvisited.Count > 0)
			{
				int bestIndex = 0;
				double bestDistance = double.MaxValue;

				foreach (int nextIndex in unvisited)                    //cicla tutti gli "unvisited" rimanenti
				{
					double dist = costs[lastIndex, nextIndex];
					if (dist < bestDistance)
					{
						bestIndex = nextIndex;                    //salva il bestIndex
						bestDistance = dist;                      //salva come risultato migliore                        
					}
				}

				bestPath.Add(list[bestIndex]);
				unvisited.Remove(bestIndex);

				//Save nearest point
				lastIndex = bestIndex;                   //l'ultimo miglior indice trovato diventa il prossimo punto da analizzare			
			}

			return bestPath;
		}

		////questa funzione calcola il "costo" di un cambio di direzione
		////in termini di distanza che sarebbe possibile percorrere
		////nel tempo di una decelerazione da velocità di marcatura, a zero 
		//private double ComputeDirectionChangeCost(L2LConf c, GrblCore core, bool border)
		//{
		//	double speed = (border ? c.borderSpeed : c.markSpeed) / 60.0; //velocità di marcatura (mm/sec)
		//	double accel = core.Configuration != null ? (double)core.Configuration.AccelerationXY : 2000; //acceleration (mm/sec^2)
		//	double cost = (speed * speed) / (2 * accel); //(mm)
		//	cost = cost * c.res; //mm tradotti nella risoluzione immagine

		//	return cost;
		//}

		//private double TravelCost(dPoint s1a, dPoint s1b, dPoint s2a, dPoint s2b, double changecost)
		//{
		//	double d = Math.Sqrt(SquareDistance(s1b, s2a));
		//	double a1 = DirectionChange(s1a, s1b, s2a);
		//	double a2 = DirectionChange(s1b, s2a, s2b);
		//	double cd = d + changecost * a1 + changecost * a2;

		//	//System.Diagnostics.Debug.WriteLine($"{d}\t{a1}\t{a2}\t{cd}");
		//	return cd;
		//}

		private static double SquareDistance(dPoint a, dPoint b)
		{
			double dX = b.X - a.X;
			double dY = b.Y - a.Y;
			return ((dX * dX) + (dY * dY));
		}
		private static double SquareDistanceZero(dPoint a)
		{
			return ((a.X * a.X) + (a.Y * a.Y));
		}

		//questo metodo ritorna un fattore 0 se c'è continuità di direzione, 0.5 su angolo 90°, 1 se c'è inversione totale (180°)
		private double DirectionChange(dPoint p1, dPoint p2, dPoint p3)
		{
			double angleA = Math.Atan2(p2.Y - p1.Y, p2.X - p1.X); //angolo del segmento corrente
			double angleB = Math.Atan2(p3.Y - p2.Y, p3.X - p2.X); //angolo della retta congiungente

			double angleAB = Math.Abs(Math.Abs(angleB) - Math.Abs(angleA)) ; //0 se stessa direzione, pigreco se inverte direzione
			double factor = angleAB / Math.PI;
			return factor;
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

		internal void DrawOnGraphics(Graphics g, Size size)
		{
			if (!mRange.MovingRange.ValidRange) return;

			GrblCommand.StatePositionBuilder spb = new GrblCommand.StatePositionBuilder();
			ProgramRange.XYRange scaleRange = mRange.MovingRange;

			//Get scale factors for both directions. To preserve the aspect ratio, use the smaller scale factor.
			float zoom = scaleRange.Width > 0 && scaleRange.Height > 0 ? Math.Min((float)size.Width / (float)scaleRange.Width, (float)size.Height / (float)scaleRange.Height) * 0.95f : 1;


			ScaleAndPosition(g, size, scaleRange, zoom);
			DrawJobPreview(g, spb, zoom);
			DrawJobRange(g, size, zoom);

		}

		private void DrawJobPreview(Graphics g, GrblCommand.StatePositionBuilder spb, float zoom)
		{
			bool firstline = true; //used to draw the first line in a different color
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
								GrblCommand.G2G3Helper ah = spb.GetArcHelper(cmd);

								if (ah.RectW > 0 && ah.RectH > 0)
								{
									try { g.DrawArc(pen, (float)ah.RectX, (float)ah.RectY, (float)ah.RectW, (float)ah.RectH, (float)(ah.StartAngle * 180 / Math.PI), (float)(ah.AngularWidth * 180 / Math.PI)); }
									catch { System.Diagnostics.Debug.WriteLine(String.Format("Ex drwing arc: W{0} H{1}", ah.RectW, ah.RectH)); }
								}
							}

						}

						firstline = false;
					}
				}
				catch (Exception ex) { throw ex; }
				finally { cmd.DeleteHelper(); }
			}
		}

		internal void LoadImageCenterline(Bitmap bmp, string filename, bool useCornerThreshold, int cornerThreshold, bool useLineThreshold, int lineThreshold, L2LConf conf, bool append, GrblCore core)
		{
			if (CheckInUse()) return;

			RiseOnFileLoading(filename);

			long start = Tools.HiResTimer.TotalMilliseconds;

			if (!append)
                ClearList();

			mRange.ResetRange();

			string content = "";

			try
			{
				content = Autotrace.BitmapToSvgString(bmp, useCornerThreshold, cornerThreshold, useLineThreshold, lineThreshold);
			}
			catch (Exception ex) { Logger.LogException("Centerline", ex); }

			SvgConverter.GCodeFromSVG converter = new SvgConverter.GCodeFromSVG();
			converter.GCodeXYFeed = Settings.GetObject("GrayScaleConversion.VectorizeOptions.BorderSpeed", 1000);
			converter.SvgScaleApply = true;
			converter.SvgMaxSize = (float)Math.Max(bmp.Width / 10.0, bmp.Height / 10.0);
			converter.UserOffset.X = Settings.GetObject("GrayScaleConversion.Gcode.Offset.X", 0F);
			converter.UserOffset.Y = Settings.GetObject("GrayScaleConversion.Gcode.Offset.Y", 0F);
			converter.UseLegacyBezier = !Settings.GetObject($"Vector.UseSmartBezier", true);

			string gcode = converter.convertFromText(content, core);
			string[] lines = gcode.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			foreach (string l in lines)
			{
				string line = l;
				if ((line = line.Trim()).Length > 0)
				{
					GrblCommand cmd = new GrblCommand(line);
					if (!cmd.IsEmpty)
						list.Add(cmd);
				}
			}

			Analyze();
			long elapsed = Tools.HiResTimer.TotalMilliseconds - start;

			RiseOnFileLoaded(filename, elapsed);

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
					GrblConfST conf =  GrblCore.Configuration;
					TimeSpan delay = spb.AnalyzeCommand(cmd, true, conf);

					mRange.UpdateSRange(spb.S);

					if (spb.LastArcHelperResult != null)
						mRange.UpdateXYRange(spb.LastArcHelperResult.BBox.X, spb.LastArcHelperResult.BBox.Y, spb.LastArcHelperResult.BBox.Width, spb.LastArcHelperResult.BBox.Height, spb.LaserBurning);
					else
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

					string format = "0";
					if (mRange.DrawingRange.Width < 50 && mRange.DrawingRange.Height < 50)
						format = "0.0";

					DrawString(g, zoom, 0, mRange.DrawingRange.Y.Min, mRange.DrawingRange.Y.Min.ToString(format), false, true, !right, false, ColorScheme.PreviewText);
					DrawString(g, zoom, 0, mRange.DrawingRange.Y.Max, mRange.DrawingRange.Y.Max.ToString(format), false, true, !right, false, ColorScheme.PreviewText);
					DrawString(g, zoom, mRange.DrawingRange.X.Min, 0, mRange.DrawingRange.X.Min.ToString(format), true, false, false, top, ColorScheme.PreviewText);
					DrawString(g, zoom, mRange.DrawingRange.X.Max, 0, mRange.DrawingRange.X.Max.ToString(format), true, false, false, top, ColorScheme.PreviewText);
				}
			}

			//draw ruler
			using (Pen pen = GetPen(ColorScheme.PreviewRuler))
			{
				//pen.DashStyle = DashStyle.Dash;
				//pen.DashPattern = new float[] { 1.0f / zoom, 2.0f / zoom }; //pen.DashPattern = new float[] { 1f / zoom, 2f / zoom};
				pen.ScaleTransform(1.0f / zoom, 1.0f / zoom);
				CartesianQuadrant q = Quadrant;
				bool right = q == CartesianQuadrant.Unknown || q == CartesianQuadrant.I || q == CartesianQuadrant.IV; //l'oggetto si trova a destra
				bool top = q == CartesianQuadrant.Unknown || q == CartesianQuadrant.I || q == CartesianQuadrant.II; //l'oggetto si trova in alto

				string format = "0";

				if (mRange.DrawingRange.ValidRange && mRange.DrawingRange.Width < 50 && mRange.DrawingRange.Height < 50)
					format = "0.0";

				//scala orizzontale
				Tools.RulerStepCalculator hscale = new Tools.RulerStepCalculator(-wSize.Width, wSize.Width, (int)(2 * s.Width / 100));

				double h1 = (top ? -4.0 : 4.0) / zoom;
				double h2 = 1.8 * h1;
				double h3 = (top ? 1.0 : -1.0) / zoom;

				for (float d = (float)hscale.FirstSmall; d < wSize.Width; d += (float)hscale.SmallStep)
					g.DrawLine(pen, d, 0, d, (float)h1);

				for (float d = (float)hscale.FirstBig; d < wSize.Width; d += (float)hscale.BigStep)
					g.DrawLine(pen, d, 0, d, (float)h2);

				for (float d = (float)hscale.FirstBig; d < wSize.Width; d += (float)hscale.BigStep)
					DrawString(g, zoom, (decimal)d, (decimal)h3, d.ToString(format), false, false, !right, !top, ColorScheme.PreviewText);

				//scala verticale

				Tools.RulerStepCalculator vscale = new Tools.RulerStepCalculator(-wSize.Height, wSize.Height, (int)(2 * s.Height / 100));
				double v1 = (right ? -4.0 : 4.0) / zoom;
				double v2 = 1.8 * v1;
				double v3 = (right ? 2.5 : 0) / zoom;

				for (float d = (float)vscale.FirstSmall; d < wSize.Height; d += (float)vscale.SmallStep)
					g.DrawLine(pen, 0, d, (float)v1, d);

				for (float d = (float)vscale.FirstBig; d < wSize.Height; d += (float)vscale.BigStep)
					g.DrawLine(pen, 0, d, (float)v2, d);

				for (float d = (float)vscale.FirstBig; d < wSize.Height; d += (float)vscale.BigStep)
					DrawString(g, zoom, (decimal)v3, (decimal)d, d.ToString(format), false, false, right, !top, ColorScheme.PreviewText, -90);
			}
		}

		private Pen GetPen(Color color)
		{ return new Pen(color); }

		private static Brush GetBrush(Color color)
		{ return new SolidBrush(color); }

		private static void DrawString(Graphics g, float zoom, decimal curX, decimal curY, string text, bool centerX, bool centerY, bool subtractX, bool subtractY, Color color, float rotation = 0)
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
					offsetX += rotation == 0 ? ms.Width : ms.Height;

				if (subtractY)
					offsetY += rotation == 0 ? ms.Height : -ms.Width;

				using (Brush b = GetBrush(color))
				{ DrawRotatedTextAt(g, rotation, text, f, b, (float)curX - offsetX, (float)-curY - offsetY); }

			}
			g.Restore(state);
		}

		private static void DrawRotatedTextAt(Graphics g, float a, string text, Font f, Brush b, float x, float y)
		{
			GraphicsState state = g.Save(); // Save the graphics state.
			g.TranslateTransform(x, y);     //posiziona
			g.RotateTransform(a);           //ruota
			g.DrawString(text, f, b, 0, 0); // scrivi a zero, zero
			g.Restore(state);               // Restore the graphics state.
		}

        System.Collections.Generic.IEnumerator<GrblCommand> IEnumerable<GrblCommand>.GetEnumerator()
		{ return list.GetEnumerator(); }


		public System.Collections.IEnumerator GetEnumerator()
		{ return list.GetEnumerator(); }

		public ProgramRange Range { get { return mRange; } }

        public bool InUse { get; internal set; }

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

			internal void UpdateRange(double rectX, double rectY, double rectW, double rectH)
			{
				X.UpdateRange((decimal)rectX);
				X.UpdateRange((decimal)(rectX + rectW));

				Y.UpdateRange((decimal)rectY);
				Y.UpdateRange((decimal)(rectY + rectH));
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

		internal void UpdateXYRange(double rectX, double rectY, double rectW, double rectH, bool drawing)
		{
			if (drawing) DrawingRange.UpdateRange(rectX, rectY, rectW, rectH);
			MovingRange.UpdateRange(rectX, rectY, rectW, rectH);
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
