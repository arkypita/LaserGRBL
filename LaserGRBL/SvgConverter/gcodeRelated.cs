/*  GRBL-Plotter. Another GCode sender for GRBL.
    This file is part of the GRBL-Plotter application.
   
    Copyright (C) 2015-2018 Sven Hasemann contact: svenhb@web.de

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
/*
* 2018-07 add line segmentation and subroutine insertion
* 2018-08 add drag tool compensation
*/

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;

namespace LaserGRBL.SvgConverter
{
	public static class gcode
	{
		private static string formatCode = "0";
		private static string formatNumber = "0.###";

		private static bool gcodeDragCompensation = false;
		private static float gcodeDragRadius = 5;
		private static float gcodeDragAngle = 30;

		private static float gcodeXYFeed = 1999;        // XY feed to apply for G1

		//private static bool gcodeSpindleToggle = true; // Switch on/off spindle for Pen down/up (M3/M5)
		private static float gcodeSpindleSpeed = 999; // Spindle speed to apply
		private static string gcodeSpindleCmdOn = "M3"; // Spindle Command M3 / M4
		private static string gcodeSpindleCmdOff = "M4"; // Spindle Command M3 / M4

		private static bool gcodeCompress = true;      // reduce code by avoiding sending again same G-Nr and unchanged coordinates
													   //public static bool gcodeRelative = false;      // calculate relative coordinates for G91
		private static bool gcodeNoArcs = false;        // replace arcs by line segments
		private static float gcodeAngleStep = 1.0f;

		private static int mDecimalPlaces = 3;

		private static Firmware firmwareType = Settings.GetObject("Firmware Type", Firmware.Grbl);
		
		private static int rapidnum = 0;
		private static bool SupportPWM = true;

		public static void setup(GrblCore core)
		{
			SupportPWM = Settings.GetObject("Support Hardware PWM", true); //If Support PWM use S command instead of M3-M4 / M5

			setDecimalPlaces(mDecimalPlaces);

			gcodeXYFeed = Settings.GetObject("GrayScaleConversion.VectorizeOptions.BorderSpeed", 1000);
			
			if (SupportPWM)
				gcodeSpindleSpeed = Settings.GetObject("GrayScaleConversion.Gcode.LaserOptions.PowerMax", 255);
			else
				gcodeSpindleSpeed = (float)GrblCore.Configuration.MaxPWM;


			// Smoothieware firmware need a value between 0.0 and 1.1
			if (firmwareType == Firmware.Smoothie)
				gcodeSpindleSpeed /= 255.0f;
			gcodeSpindleCmdOn = Settings.GetObject("GrayScaleConversion.Gcode.LaserOptions.LaserOn", "M3");
			gcodeSpindleCmdOff = Settings.GetObject("GrayScaleConversion.Gcode.LaserOptions.LaserOff", "M5");
			SupportPWM = Settings.GetObject("Support Hardware PWM", true); //If Support PWM use S command instead of M3-M4 / M5

			lastMovewasG0 = true;
			lastx = -1; lasty = -1; lastz = 0; lasts = -1 ; lastg = -1;
		}

		public static bool reduceGCode
		{
			get { return gcodeCompress; }
			set
			{
				gcodeCompress = value;
				setDecimalPlaces(mDecimalPlaces);
			}
		}

		public static void setRapidNum(int num)
		{ rapidnum = num; }

		public static void setDecimalPlaces(int num)
		{
			formatNumber = "0.";
			if (gcodeCompress)
				formatNumber = formatNumber.PadRight(num + 2, '#'); //'0'
			else
				formatNumber = formatNumber.PadRight(num + 2, '0'); //'0'
		}

		// get GCode one or two digits
		public static int getIntGCode(char code, string tmp)
		{
			string cmdG = getStrGCode(code, tmp);       // find number string
			if (cmdG.Length > 0)
			{ return Convert.ToInt16(cmdG.Substring(1)); }
			return -1;
		}

		public static string getStrGCode(char code, string tmp)
		{
			int cmt = tmp.IndexOf("(");
			if (cmt == 0)
				return "";                      // nothing to do
			if (cmt >= 0)
				tmp = tmp.Substring(0, cmt);     // don't check inside comment
			var cmdG = Regex.Matches(tmp, code + "\\d{1,2}"); // find code and 1 or 2 digits
			if (cmdG.Count > 0)
			{ return cmdG[0].ToString(); }
			return "";
		}

		// get value from X,Y,Z F, S etc.
		public static int getIntValue(char code, string tmp)
		{
			string cmdG = getStringValue(code, tmp);
			if (cmdG.Length > 0)
			{ return Convert.ToInt16(cmdG.Substring(1)); }
			return -1;
		}
		public static string getStringValue(char code, string tmp)
		{
			var cmdG = Regex.Matches(tmp, code + "-?\\d+(.\\d+)?");
			if (cmdG.Count > 0)
			{ return cmdG[cmdG.Count - 1].ToString(); }
			return "";
		}

		public static string frmtCode(int number)      // convert int to string using format pattern
		{ return number.ToString(formatCode); }
		public static string frmtNum(float number)     // convert float to string using format pattern
		{ return number.ToString(formatNumber); }
		public static string frmtNum(double number)     // convert double to string using format pattern
		{ return number.ToString(formatNumber); }

		private static StringBuilder secondMove = new StringBuilder();
		private static bool applyXYFeedRate = true; // apply XY feed after each Pen-move

		public static void SpindleOn(StringBuilder gcodeString, string cmt = "")
		{
			if (cmt.Length > 0) cmt = string.Format(" ({0})", cmt);

			if (SupportPWM)
				gcodeString.AppendFormat("S{0}{1}\r\n", gcodeSpindleSpeed, cmt); //only set SMax
			else
				gcodeString.AppendFormat("{0}{1}\r\n", gcodeSpindleCmdOn, cmt); //only set M3/M4
		}

		public static void SpindleOff(StringBuilder gcodeString, string cmt = "")
		{
			if (cmt.Length > 0) cmt = string.Format(" ({0})", cmt);
			
			if (SupportPWM)
				gcodeString.AppendFormat("S0{0}\r\n", cmt); //only set S0
			else
				gcodeString.AppendFormat("{0}{1}\r\n", gcodeSpindleCmdOff, cmt); //only set M5
		}

		internal static void PutInitialCommand(StringBuilder gcodeString)
		{
			if (SupportPWM)
				gcodeString.AppendFormat("{0} S0\r\n", gcodeSpindleCmdOn); //turn ON with zero power
			else
				gcodeString.AppendFormat("{0} S{1}\r\n", gcodeSpindleCmdOff, gcodeSpindleSpeed); //turn OFF and set MaxPower
		}

		internal static void PutFinalCommand(StringBuilder gcodeString)
		{
			if (SupportPWM)
				gcodeString.AppendFormat("M5 S0\r\n"); //turn OFF and zero power
			else
				gcodeString.AppendFormat("M5 S0\r\n"); //turn OFF and zero power
		}

		public static void PenDown(StringBuilder gcodeString, string cmto = "")
		{
			string cmt = cmto;
			drag1stMove = true;
			origFinalX = lastx;
			origFinalY = lasty;
			//if (gcodeComments) { gcodeString.Append("\r\n"); }
			if (cmt.Length > 0) { cmt = string.Format("({0})", cmt); }

			applyXYFeedRate = true;     // apply XY Feed Rate after each PenDown command (not just after Z-axis)

			//if (gcodeSpindleToggle)
			//{   if (gcodeComments) gcodeString.AppendFormat("({0})\r\n", "Pen down: Spindle-On");
			SpindleOn(gcodeString, cmto);
			//}
			//if (gcodeZApply)
			//{   if (gcodeComments) gcodeString.AppendFormat("({0})\r\n", "Pen down: Z-Axis");
			//    float z_relative = gcodeZDown - lastz;
			//    if (gcodeRelative)
			//        gcodeString.AppendFormat("G{0} Z{1} F{2} {3}\r\n", frmtCode(1), frmtNum(z_relative), gcodeZFeed, cmt);
			//    else
			//        gcodeString.AppendFormat("G{0} Z{1} F{2} {3}\r\n", frmtCode(1), frmtNum(gcodeZDown), gcodeZFeed, cmt);

			//    gcodeTime += Math.Abs((gcodeZUp - gcodeZDown) / gcodeZFeed);
			//    gcodeLines++; lastg = 1; lastf = gcodeZFeed;
			//    lastz = gcodeZDown;
			//}
			//if (gcodePWMEnable)
			//{   if (gcodeComments) gcodeString.AppendFormat("({0})\r\n", "Pen down: Servo control");
			//    gcodeString.AppendFormat("M{0} S{1} {2}\r\n", gcodeSpindleCmd, gcodePWMDown, cmt);
			//    gcodeString.AppendFormat("G{0} P{1}\r\n", frmtCode(4), frmtNum(gcodePWMDlyDown));
			//    gcodeTime += gcodePWMDlyDown;
			//    gcodeLines++;
			//}
			//if (gcodeIndividualTool)
			//{   if (gcodeComments) gcodeString.AppendFormat("({0})\r\n", "Pen down: Individual Cmd");
			//    string[] commands = gcodeIndividualDown.Split(';');
			//    foreach (string cmd in commands)
			//    { gcodeString.AppendFormat("{0}\r\n", cmd.Trim()); }
			//}
			//if (gcodeComments) gcodeString.Append("\r\n");

			//gcodeDownUp++;
		}

		public static void PenUp(StringBuilder gcodeString, string cmto = "")
		{
			string cmt = cmto;
			drag1stMove = true;
			origFinalX = lastx;
			origFinalY = lasty;
			//if (gcodeComments) { gcodeString.Append("\r\n"); }
			if (cmt.Length > 0) { cmt = string.Format("({0})", cmt); }

			//if (gcodeIndividualTool)
			//{   if (gcodeComments) gcodeString.AppendFormat("({0})\r\n", "Pen up: Individual Cmd");
			//    string[] commands = gcodeIndividualUp.Split(';'); 
			//    foreach (string cmd in commands)
			//    {   gcodeString.AppendFormat("{0}\r\n", cmd.Trim());}
			//}

			//if (gcodePWMEnable)
			//{   if (gcodeComments) gcodeString.AppendFormat("({0})\r\n", "Pen up: Servo control");
			//    gcodeString.AppendFormat("M{0} S{1} {2}\r\n", gcodeSpindleCmd, gcodePWMUp, cmt);
			//    gcodeString.AppendFormat("G{0} P{1}\r\n", frmtCode(4), frmtNum(gcodePWMDlyUp));
			//    gcodeTime += gcodePWMDlyUp;
			//    gcodeLines++;
			//}

			//if (gcodeZApply)
			//{   if (gcodeComments) gcodeString.AppendFormat("({0})\r\n", "Pen up: Z-Axis");
			//    float z_relative = gcodeZUp - lastz;
			//    if (gcodeRelative)
			//        gcodeString.AppendFormat("G{0} Z{1} {2}\r\n", frmtCode(0), frmtNum(z_relative), cmt); // use G0 without feedrate
			//    else
			//        gcodeString.AppendFormat("G{0} Z{1} {2}\r\n", frmtCode(0), frmtNum(gcodeZUp), cmt); // use G0 without feedrate

			//    gcodeTime += Math.Abs((gcodeZUp - gcodeZDown) / gcodeZFeed);
			//    gcodeLines++; lastg = 1; lastf = gcodeZFeed;
			//    lastz = gcodeZUp;
			//}

			//if (gcodeSpindleToggle)
			//{   if (gcodeComments) gcodeString.AppendFormat("({0})\r\n", "Pen up: Spindle-Off");
			SpindleOff(gcodeString, cmto);
			//}
			//if (gcodeComments) gcodeString.Append("\r\n");
			dragCompi = 0; dragCompj = 0;
		}

		public static float lastx, lasty, lastz, lastg = -1, lastf, lasts;
		public static bool lastMovewasG0 = true;
		public static void MoveTo(StringBuilder gcodeString, Point coord, string cmt = "")
		{ MoveSplit(gcodeString, 1, (float)coord.X, (float)coord.Y, applyXYFeedRate, cmt); }
		public static void MoveTo(StringBuilder gcodeString, float x, float y, string cmt = "")
		{ MoveSplit(gcodeString, 1, x, y, applyXYFeedRate, cmt); }
		public static void MoveTo(StringBuilder gcodeString, float x, float y, float z, string cmt = "")
		{ MoveSplit(gcodeString, 1, x, y, z, applyXYFeedRate, cmt); }
		public static void MoveToRapid(StringBuilder gcodeString, Point coord, string cmt = "")
		{
			Move(gcodeString, rapidnum, (float)coord.X, (float)coord.Y, false, cmt); lastMovewasG0 = true; 
		}
		public static void MoveToRapid(StringBuilder gcodeString, float x, float y, string cmt = "")
		{ 
			Move(gcodeString, rapidnum, x, y, false, cmt); lastMovewasG0 = true;
		}

		// MoveSplit breaks down a line to line segments with given max. length
		private static void MoveSplit(StringBuilder gcodeString, int gnr, float x, float y, bool applyFeed, string cmt)
		{ MoveSplit(gcodeString, gnr, x, y, null, applyFeed, cmt); }

		private static float remainingC = 10;
		private static float segFinalX = 0, segFinalY = 0, segLastFinalX = 0, segLastFinalY = 0;
		private static void MoveSplit(StringBuilder gcodeString, int gnr, float finalx, float finaly, float? z, bool applyFeed, string cmt)
		{
			segLastFinalX = segFinalX; segLastFinalY = segFinalY;
			segFinalX = finalx; segFinalY = finaly;

			if (gcodeDragCompensation)  // start move with an arc and end with extended move
			{
				Point tmp = dragToolCompensation(gcodeString, finalx, finaly);
				finalx = (float)tmp.X; finaly = (float)tmp.Y;   // get extended final position
			}

			//if (Properties.Settings.Default.importGCLineSegmentation)       // apply segmentation
			//{
			//	float segFinalX = finalx, segFinalY = finaly;
			//	if (gcodeDragCompensation)
			//	{
			//		lastx = segLastFinalX;// origLastX;
			//		lasty = segLastFinalY;
			//	}
			//	float dx = finalx - lastx;       // remaining distance until full move
			//	float dy = finaly - lasty;       // lastXY is global
			//	float moveLength = (float)Math.Sqrt(dx * dx + dy * dy);
			//	float segmentLength = (float)Properties.Settings.Default.importGCLineSegmentLength;
			//	bool equidistance = Properties.Settings.Default.importGCLineSegmentEquidistant;

			//	//auch nach G0 move
			//	if (Properties.Settings.Default.importGCSubFirst && (lastMovewasG0 || (moveLength >= segmentLength)))       // also subroutine at first point
			//	{
			//		//if (gcodeInsertSubroutine)
			//		//    applyFeed = insertSubroutine(gcodeString, lastx, lasty, lastz, applyFeed);
			//		remainingC = segmentLength;
			//	}

			//	if ((moveLength <= remainingC))//  && !equidistance)           // nothing to split 
			//	{
			//		//if (gcodeComments)
			//		//    cmt += string.Format("{0:0.0} until subroutine",remainingC);
			//		Move(gcodeString, 1, finalx, finaly, z, applyXYFeedRate, cmt);      // remainingC.ToString()
			//		remainingC -= moveLength;
			//		if (gcodeDragCompensation)
			//			remainingC += gcodeDragRadius;
			//	}
			//	else
			//	{
			//		float tmpX, tmpY, origX, origY, deltaX, deltaY;
			//		int count = (int)Math.Ceiling(moveLength / segmentLength);
			//		gcodeString.AppendFormat("(count {0})\r\n", count.ToString());
			//		origX = lastx; origY = lasty;
			//		if (equidistance)               // all segments in same length (but shorter than set)
			//		{
			//			for (int i = 1; i < count; i++)
			//			{
			//				deltaX = i * dx / count;
			//				deltaY = i * dy / count;
			//				tmpX = origX + deltaX;
			//				tmpY = origY + deltaY;
			//				Move(gcodeString, 1, tmpX, tmpY, z, applyFeed, cmt);
			//				if (i >= 1) { applyFeed = false; cmt = ""; }
			//				//if (gcodeInsertSubroutine)
			//				//    applyFeed = insertSubroutine(gcodeString, lastx, lasty, lastz, applyFeed);
			//			}
			//		}
			//		else
			//		{
			//			remainingX = dx * remainingC / moveLength;
			//			remainingY = dy * remainingC / moveLength;
			//			for (int i = 0; i < count; i++)
			//			{
			//				deltaX = remainingX + i * segmentLength * dx / moveLength;        // n-1 segments in exact length, last segment is shorter
			//				deltaY = remainingY + i * segmentLength * dy / moveLength;
			//				tmpX = origX + deltaX;
			//				tmpY = origY + deltaY;
			//				remainingC = segmentLength;
			//				if ((float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY) >= moveLength)
			//					break;
			//				Move(gcodeString, 1, tmpX, tmpY, z, applyFeed, cmt);
			//				if (i >= 1) { applyFeed = false; cmt = ""; }
			//				//if (gcodeInsertSubroutine)
			//				//    applyFeed = insertSubroutine(gcodeString, lastx, lasty, lastz, applyFeed);
			//			}
			//		}
			//		finalx = segFinalX; finaly = segFinalY;
			//		remainingC = segmentLength - fdistance(finalx, finaly, lastx, lasty);
			//		Move(gcodeString, 1, finalx, finaly, z, applyFeed, cmt);
			//		if ((equidistance) || (remainingC == 0))
			//		{
			//			//if (gcodeInsertSubroutine)
			//			//    insertSubroutine(gcodeString, lastx, lasty, lastz, applyFeed);
			//			remainingC = segmentLength;
			//		}
			//	}
			//}
			//else
			Move(gcodeString, 1, finalx, finaly, z, applyXYFeedRate, cmt);
			lastMovewasG0 = false;
		}

		private static bool dragArc = false, drag1stMove = true;
		private static float origLastX, origLastY, origFinalX, origFinalY;
		private static float dragCompi = 0, dragCompj = 0;
		private static Point dragToolCompensation(StringBuilder gcodeString, float finalx, float finaly)
		{
			float dx = finalx - origFinalX;// lastx;       // remaining distance until full move
			float dy = finaly - origFinalY;// lasty;       // lastXY is global
			float moveLength = (float)Math.Sqrt(dx * dx + dy * dy);
			if (moveLength == 0)
				return new Point(finalx, finaly);

			// calc arc angle between last move and current move, dragCompx y = lastrealpos, dragCompi j = centerofcircle
			float rx = origFinalX + gcodeDragRadius * dx / moveLength;     // calc end-pos of arc
			float ry = origFinalY + gcodeDragRadius * dy / moveLength;
			float[] angle = getAngle(lastx, lasty, rx, ry, dragCompi, dragCompj); // get start-,end- and diff-angle
			if (angle[2] > 180) { angle[2] = angle[2] - 360; }
			if (angle[2] < -180) { angle[2] = 360 + angle[2]; }
			int gcnr = 2;
			if (angle[2] > 0)
				gcnr = 3;
			dragArc = (Math.Abs(angle[2]) > gcodeDragAngle);

			// draw arc before move
			if (dragArc)        // add arc to connect next move
			{
				origLastX = lastx; origLastY = lasty;
				if ((angle[2] != 0) && !drag1stMove)
				{
					MoveArc(gcodeString, gcnr, rx, ry, dragCompi, dragCompj, applyXYFeedRate, "Drag t. comp.");// + angle[2].ToString());
				}
				lastx = origLastX; lasty = origLastY;
			}
			else
			{               // connect first extend move with end of next extend move
			}
			// calc end-pos with added offset and center of new arc
			origLastX = lastx;
			origLastY = lasty;
			origFinalX = finalx;
			origFinalY = finaly;
			finalx = finalx + gcodeDragRadius * dx / moveLength;    // set new finalx for move command
			finaly = finaly + gcodeDragRadius * dy / moveLength;
			dragCompi = origFinalX - finalx;                              // calc center of arc to connect to next move
			dragCompj = origFinalY - finaly;

			drag1stMove = false;
			return new Point(finalx, finaly);
		}

		//// process subroutine, afterwards move back to last regular position before subroutine        
		//private static bool insertSubroutine(StringBuilder gcodeString, float lX, float lY, float lZ, bool applyFeed)
		//{
		//    gcodeString.AppendFormat("M98 P99 (call subroutine)\r\nG90 G0 X{0} Y{1}\r\nG1 Z{2} F{3}\r\n", frmtNum(lX), frmtNum(lY), frmtNum(lZ), 1000);
		//    applyXYFeedRate = true;
		//    gcodeSubroutineCount++;
		//    if (gcodeSubroutineEnable == 0)     // read file once
		//    {
		//        string file = Properties.Settings.Default.importGCSubroutine;
		//        gcodeSubroutine = "\r\n(subroutine)\r\nO99\r\n";
		//        if (File.Exists(file))
		//            gcodeSubroutine += File.ReadAllText(file);
		//        else
		//            gcodeSubroutine += "(file " + file + " not found)\r\n";
		//        gcodeSubroutine += "M99\r\n";
		//        gcodeSubroutineEnable++;
		//    }
		//    return true;    // applyFeed is needed
		//}


		private static void Move(StringBuilder gcodeString, int gnr, float x, float y, bool applyFeed, string cmt)
		{ Move(gcodeString, gnr, x, y, null, applyFeed, cmt); }
		private static void Move(StringBuilder gcodeString, int gnr, float x, float y, float? z, bool applyFeed, string cmt)
		{
			if (gnr == 0)
			{
				segLastFinalX = segFinalX; segLastFinalY = segFinalY;
				segFinalX = x; segFinalY = y;
			}
			string feed = "";
			StringBuilder gcodeTmp = new StringBuilder();
			bool isneeded = false;
			float x_relative = x - lastx;
			float y_relative = y - lasty;
			float z_relative = lastz;
			float tz = 0;

			float delta = fdistance(lastx, lasty, x, y);

			if (z != null)
			{
				z_relative = (float)z - lastz;
				tz = (float)z;
			}

			if (applyFeed && (gnr > 0))
			{
				feed = string.Format("F{0}", gcodeXYFeed);
				applyXYFeedRate = false;                        // don't set feed next time
			}
			
			if (cmt.Length > 0) cmt = string.Format("({0})", cmt);

			if (gcodeCompress)
			{
				if (((gnr > 0) || (lastx != x) || (lasty != y) || (lastz != tz)))  // else nothing to do
				{
					// For Marlin, we must change this line to :
					// if (lastg != gnr || firmwareType == Firmware.Marlin) { gcodeTmp.AppendFormat("G{0}", frmtCode(gnr)); isneeded = true; }
					if (lastg != gnr) { gcodeTmp.AppendFormat("G{0}", frmtCode(gnr)); isneeded = true; }

					if (lastx != x) { gcodeTmp.AppendFormat("X{0}", frmtNum(x)); isneeded = true; }
					if (lasty != y) { gcodeTmp.AppendFormat("Y{0}", frmtNum(y)); isneeded = true; }
					if (z != null)
					{
						if (lastz != z) { gcodeTmp.AppendFormat("Z{0}", frmtNum((float)z)); isneeded = true; }
					}


					if ((gnr == 1) && (lastf != gcodeXYFeed) || applyFeed)
					{
						gcodeTmp.AppendFormat("F{0}", gcodeXYFeed);
						lastf = gcodeXYFeed;
						isneeded = true;
					}
					// Smothieware firmware need to know laserpower when G1 command is run
					if ((gnr == 1) && (lasts != gcodeSpindleSpeed) && firmwareType == Firmware.Smoothie)
					{
						gcodeTmp.AppendFormat("S{0}", frmtNum(gcodeSpindleSpeed));
						lasts = gcodeSpindleSpeed;
						isneeded = true;
					}
					gcodeTmp.AppendFormat("{0}\r\n", cmt);
					if (isneeded)
						gcodeString.Append(gcodeTmp);
				}
			}
			else
			{
				if (z != null)
					gcodeString.AppendFormat("G{0} X{1} Y{2} Z{3} {4} {5}\r\n", frmtCode(gnr), frmtNum(x), frmtNum(y), frmtNum((float)z), feed, cmt);
				else
					gcodeString.AppendFormat("G{0} X{1} Y{2} {3} {4}\r\n", frmtCode(gnr), frmtNum(x), frmtNum(y), feed, cmt);

			}
			//gcodeTime += delta / gcodeXYFeed;
			lastx = x; lasty = y; lastg = gnr; // lastz = tz;
											   //gcodeLines++;
		}

		public static void splitLine(StringBuilder gcodeString, int gnr, float x1, float y1, float x2, float y2, float maxStep, bool applyFeed, string cmt = "")
		{
			float dx = x2 - x1;
			float dy = y2 - y1;
			float c = (float)Math.Sqrt(dx * dx + dy * dy);
			float tmpX, tmpY;
			int divid = (int)Math.Ceiling(c / maxStep);
			lastg = -1;
			for (int i = 1; i <= divid; i++)
			{
				tmpX = x1 + i * dx / divid;
				tmpY = y1 + i * dy / divid;
				if (i > 1) { applyFeed = false; cmt = ""; }
				if (gnr == 0)
				{ Move(gcodeString, gnr, tmpX, tmpY, false, cmt); }
				else
				{ Move(gcodeString, gnr, tmpX, tmpY, applyFeed, cmt); }
			}
		}


		public static void Arc(StringBuilder gcodeString, int gnr, Point coordxy, Point coordij, string cmt = "", bool avoidG23 = false)
		{ MoveArc(gcodeString, gnr, (float)coordxy.X, (float)coordxy.Y, (float)coordij.X, (float)coordij.Y, applyXYFeedRate, cmt, avoidG23); }
		public static void Arc(StringBuilder gcodeString, int gnr, float x, float y, float i, float j, string cmt = "", bool avoidG23 = false)
		{ MoveArc(gcodeString, gnr, x, y, i, j, applyXYFeedRate, cmt, avoidG23); }
		private static void MoveArc(StringBuilder gcodeString, int gnr, float x, float y, float i, float j, bool applyFeed, string cmt = "", bool avoidG23 = false)
		{
			string feed = "";
			string splindleSpeed = "";
			float x_relative = x - lastx;
			float y_relative = y - lasty;

			if (applyFeed)
			{
				feed = string.Format("F{0}", gcodeXYFeed);
				applyXYFeedRate = false;                        // don't set feed next time
			}
			if (cmt.Length > 0) cmt = string.Format("({0})", cmt);
			// Smothieware firmware need to know laserpower when G2-G3 command is run
			if (firmwareType == Firmware.Smoothie)
			{
				splindleSpeed = string.Format("S{0}", frmtNum(gcodeSpindleSpeed));
			}
			if (gcodeNoArcs || avoidG23)
			{
				splitArc(gcodeString, gnr, lastx, lasty, x, y, i, j, applyFeed, cmt);
			}
			else
			{
				gcodeString.AppendFormat("G{0}X{1}Y{2}I{3}J{4}{5}{6}{7}\r\n", frmtCode(gnr), frmtNum(x), frmtNum(y), frmtNum(i), frmtNum(j), feed, splindleSpeed, cmt);
				lastg = gnr;
			}
			//gcodeTime += fdistance(lastx, lasty, x, y) / gcodeXYFeed;
			lastx = x; lasty = y; lastf = gcodeXYFeed;
			//gcodeLines++;
		}

		public static void splitArc(StringBuilder gcodeString, int gnr, float x1, float y1, float x2, float y2, float i, float j, bool applyFeed, string cmt = "")
		{
			float segmentLength = 10;
			bool equidistance = false;

			float radius = (float)Math.Sqrt(i * i + j * j);                 // get radius of circle
			float cx = x1 + i, cy = y1 + j;                                 // get center point of circle

			float[] ret = getAngle(x1, y1, x2, y2, i, j);
			float a1 = ret[0], a2 = ret[1], da = ret[2];

			da = -(360 + a1 - a2);
			if (gnr == 3) { da = Math.Abs(360 + a2 - a1); }
			if (da > 360) { da -= 360; }
			if (da < -360) { da += 360; }

			if ((x1 == x2) && (y1 == y2))
			{
				if (gnr == 2) { da = -360; }
				else { da = 360; }
			}
			float step = (float)(Math.Asin((double)gcodeAngleStep / (double)radius) * 180 / Math.PI);

			applyXYFeedRate = true;
			float moveLength = remainingC;
			int count;
			if (equidistance)
			{
				float circum = radius * da * (float)Math.PI / 180;
				count = (int)Math.Ceiling(circum / segmentLength);
				segmentLength = circum / count;
				//Comment(gcodeString, circum.ToString() + " " + count.ToString() + " " + segmentLength.ToString() );
				moveLength = 0;
			}
			count = 1;
			if (da > 0)                                             // if delta >0 go counter clock wise
			{
				for (float angle = (a1 + step); angle < (a1 + da); angle += step)
				{
					float x = cx + radius * (float)Math.Cos(Math.PI * angle / 180);
					float y = cy + radius * (float)Math.Sin(Math.PI * angle / 180);
					moveLength += fdistance(x, y, lastx, lasty);

					Move(gcodeString, 1, x, y, applyXYFeedRate, cmt);

					if (moveLength >= (count * segmentLength))
					{
						applyXYFeedRate = true;// insertSubroutine(gcodeString, lastx, lasty, lastz, applyXYFeedRate);
						count++;
					}
					if (cmt.Length > 1) cmt = "";
				}
			}
			else                                                       // else go clock wise
			{
				for (float angle = (a1 - step); angle > (a1 + da); angle -= step)
				{
					float x = cx + radius * (float)Math.Cos(Math.PI * angle / 180);
					float y = cy + radius * (float)Math.Sin(Math.PI * angle / 180);
					moveLength += fdistance(x, y, lastx, lasty);

					Move(gcodeString, 1, x, y, applyXYFeedRate, cmt);

					if (moveLength >= (count * segmentLength))
					{
						applyXYFeedRate = true;//insertSubroutine(gcodeString, lastx, lasty, lastz, applyXYFeedRate);
						count++;
					}
					if (cmt.Length > 1) cmt = "";
				}
			}
			Move(gcodeString, 1, x2, y2, applyXYFeedRate, "End Arc conversion");
			if ((moveLength >= (count * segmentLength)) || equidistance)
			{
				applyXYFeedRate = true; // insertSubroutine(gcodeString, lastx, lasty, lastz, applyXYFeedRate);
				moveLength = 0;
			}
		}

		private static float[] getAngle(float x1, float y1, float x2, float y2, float i, float j)
		{
			float[] ret = new float[3];
			float radius = (float)Math.Sqrt(i * i + j * j);             // get radius of circle

			float cos1 = i / radius;                                    // get start angle
			if (cos1 > 1) cos1 = 1;
			if (cos1 < -1) cos1 = -1;
			float a1 = 180 - 180 * (float)(Math.Acos(cos1) / Math.PI);
			if (j > 0) { a1 = -a1; }

			float cos2 = (float)(x1 + i - x2) / radius;                 // get stop angle
			if (cos2 > 1) cos2 = 1;
			if (cos2 < -1) cos2 = -1;
			float a2 = 180 - 180 * (float)(Math.Acos(cos2) / Math.PI);
			if ((y1 + j - y2) > 0) { a2 = -a2; }                        // get delta angle

			float da = -(360 + a1 - a2);
			if (da > 360) { da -= 360; }
			if (da < -360) { da += 360; }

			ret[0] = a1; ret[1] = a2; ret[2] = da;
			return ret;
		}

		//public static void Tool(StringBuilder gcodeString, int toolnr, string cmt="")
		//{
		//    string toolCmd = "";
		//    if (gcodeToolChange)                // otherweise no command needed
		//    {
		//        //if (gcodeZApply) gcode.SpindleOff(gcodeString, "Stop spindle - Option Z-Axis");
		//        if (cmt.Length > 0) cmt = string.Format("({0})", cmt);
		//        toolCmd = string.Format("T{0:D2} M{1} {2}", toolnr, frmtCode(6), cmt);
		//        if (gcodeToolChangeM0)
		//        { gcodeString.AppendFormat("M0 ({0})\r\n", toolCmd); }
		//        else
		//        { gcodeString.AppendFormat("{0}\r\n", toolCmd); }
		//        gcodeToolCounter++;
		//        gcodeLines++;
		//        gcodeToolText += string.Format("( {0}) ToolNr: {1:D2}, Name: {2})\r\n", gcodeToolCounter,toolnr, cmt);

		//        remainingC = (float)Properties.Settings.Default.importGCLineSegmentLength;

		//        toolProp toolInfo = toolTable.getToolProperties(toolnr);
		//        if (Properties.Settings.Default.importGCTTSSpeed)
		//            gcodeSpindleSpeed = toolInfo.spindleSpeed;
		//        if (Properties.Settings.Default.importGCTTXYFeed)
		//            gcodeXYFeed = toolInfo.feedXY;
		//        //if (Properties.Settings.Default.importGCTTZFeed)
		//        //    gcodeZFeed = toolInfo.feedZ;
		//        //if (Properties.Settings.Default.importGCTTZDeepth)
		//        //    gcodeZDown = toolInfo.stepZ;

		//        //if (Properties.Settings.Default.importGCDragKnifePercentEnable)
		//        //{   gcodeDragRadius = Math.Abs(gcodeZDown*(float)Properties.Settings.Default.importGCDragKnifePercent/100); }

		//        //if (gcodeZApply) gcode.SpindleOn(gcodeString, "Start spindle - Option Z-Axis");
		//    }
		//}

		// helper functions
		private static float fsqrt(float x) { return (float)Math.Sqrt(x); }
		private static float fvmag(float x, float y) { return fsqrt(x * x + y * y); }
		private static float fdistance(float x1, float y1, float x2, float y2) { return fvmag(x2 - x1, y2 - y1); }
	}
}
