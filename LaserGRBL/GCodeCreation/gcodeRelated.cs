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

namespace GRBL_Plotter
{
    public static class gcode
    {   private static string formatCode = "0";
        private static string formatNumber = "0.###";

        private static int gcodeLines = 0;              // counter for GCode lines
        private static float gcodeDistance = 0;         // counter for GCode move distance

        //private static int gcodeSubroutineEnable = 0;    // state subroutine
        private static string gcodeSubroutine = "";    //  subroutine

        private static bool gcodeDragCompensation = false;
        public static float gcodeDragRadius = 0;
        private static float gcodeDragAngle = 30;

        private static int gcodeDownUp = 0;             // counter for GCode Pen Down / Up
        private static float gcodeTime = 0;             // counter for GCode work time
        private static int gcodePauseCounter = 0;       // counter for GCode pause M0 commands
        //private static int gcodeToolCounter = 0;       // counter for GCode Tools
        //private static string gcodeToolText = "";       // counter for GCode Tools

        public static float gcodeXYFeed = 1999;        // XY feed to apply for G1
        private static bool gcodeComments = true;       // if true insert additional comments into GCode

        //private static bool gcodeToolChange = false;          // Apply tool exchange command
        //private static bool gcodeToolChangeM0 = false;

        // Using Spindle pwr. to switch on/off laser
        private static bool gcodeSpindleToggle = true; // Switch on/off spindle for Pen down/up (M3/M5)
        public static float gcodeSpindleSpeed = 999; // Spindle speed to apply
        private static string gcodeSpindleCmd = "3"; // Spindle Command M3 / M4

        // Using Spindle-Speed als PWM output to control RC-Servo
        private static bool gcodePWMEnable = false;     // Change Spindle speed for Pen down/up
        private static float gcodePWMUp = 199;          // Spindle speed for Pen-up
        private static float gcodePWMDlyUp = 0;         // Delay to apply after Pen-up (because servo is slow)
        private static float gcodePWMDown = 799;        // Spindle speed for Pen-down
        private static float gcodePWMDlyDown = 0;       // Delay to apply after Pen-down (because servo is slow)

        private static bool gcodeIndividualTool = false;     // Use individual Pen down/up
        private static string gcodeIndividualUp = "";
        private static string gcodeIndividualDown = "";

        private static bool gcodeCompress = false;      // reduce code by avoiding sending again same G-Nr and unchanged coordinates
        public static bool gcodeRelative = false;      // calculate relative coordinates for G91
        private static bool gcodeNoArcs = false;        // replace arcs by line segments
        private static float gcodeAngleStep = 0.1f;
        //private static bool gcodeInsertSubroutine = false;
        //private static int gcodeSubroutineCount = 0;

        private static Stopwatch stopwatch = new Stopwatch();

        public static void setup()
        {
            setDecimalPlaces((int)Properties.Settings.Default.importGCDecPlaces);
            gcodeXYFeed = (float)Properties.Settings.Default.importGCXYFeed;

            gcodeComments = Properties.Settings.Default.importGCAddComments;
            gcodeSpindleToggle = Properties.Settings.Default.importGCSpindleToggle;
            gcodeSpindleSpeed = (float)Properties.Settings.Default.importGCSSpeed;
            if (Properties.Settings.Default.importGCSpindleCmd)
                gcodeSpindleCmd = "3";
            else
                gcodeSpindleCmd = "4";

            //gcodeZApply = Properties.Settings.Default.importGCZEnable;
            //gcodeZUp = (float)Properties.Settings.Default.importGCZUp;
            //gcodeZDown = (float)Properties.Settings.Default.importGCZDown;
            //gcodeZFeed = (float)Properties.Settings.Default.importGCZFeed;

            gcodePWMEnable = Properties.Settings.Default.importGCPWMEnable;
            gcodePWMUp = (float)Properties.Settings.Default.importGCPWMUp;
            gcodePWMDlyUp = (float)Properties.Settings.Default.importGCPWMDlyUp;
            gcodePWMDown = (float)Properties.Settings.Default.importGCPWMDown;
            gcodePWMDlyDown = (float)Properties.Settings.Default.importGCPWMDlyDown;

            gcodeIndividualTool = Properties.Settings.Default.importGCIndEnable;
            gcodeIndividualUp = Properties.Settings.Default.importGCIndPenUp;
            gcodeIndividualDown = Properties.Settings.Default.importGCIndPenDown;

            gcodeReduce = Properties.Settings.Default.importSVGReduce;
            gcodeReduceVal = (float)Properties.Settings.Default.importSVGReduceLimit;

            //gcodeToolChange = Properties.Settings.Default.importGCTool;
            //gcodeToolChangeM0 = Properties.Settings.Default.importGCToolM0;

            gcodeCompress = Properties.Settings.Default.importGCCompress;        // reduce code by 
            gcodeRelative = Properties.Settings.Default.importGCRelative;        // reduce code by 
            gcodeNoArcs = Properties.Settings.Default.importGCNoArcs;        // reduce code by 
            gcodeAngleStep = (float)Properties.Settings.Default.importGCSegment;

            gcodeDragCompensation = Properties.Settings.Default.importGCDragKnifeEnable;
            gcodeDragRadius = (float)Properties.Settings.Default.importGCDragKnifeLength;
            //if (Properties.Settings.Default.importGCDragKnifePercentEnable)
            //{   gcodeDragRadius = Math.Abs(gcodeZDown * (float)Properties.Settings.Default.importGCDragKnifePercent / 100); }

            gcodeDragAngle = (float)Properties.Settings.Default.importGCDragKnifeAngle;

            //gcodeInsertSubroutine = Properties.Settings.Default.importGCSubEnable;
            //gcodeSubroutineCount = 0;
            lastMovewasG0 = true;

            gcodeLines = 1;             // counter for GCode lines
            gcodeDistance = 0;          // counter for GCode move distance
            remainingC = (float)Properties.Settings.Default.importGCLineSegmentLength;

            //gcodeSubroutineEnable = 0;
            gcodeSubroutine = "";
            gcodeDownUp = 0;            // counter for GCode Down/Up
            gcodeTime = 0;              // counter for GCode work time
            gcodePauseCounter = 0;      // counter for GCode pause M0 commands
            //gcodeToolCounter = 0;
            //gcodeToolText = "";
            lastx = -1; lasty = -1; lastz = 0;

            if (gcodeRelative)
            { lastx = 0; lasty = 0; }

            stopwatch = new Stopwatch();
            stopwatch.Start();

        }

        public static bool reduceGCode
        {
            get { return gcodeCompress; }
            set {   gcodeCompress = value;
                    setDecimalPlaces((int)Properties.Settings.Default.importGCDecPlaces);
                }
        }

        public static void setDecimalPlaces(int num)
        {   formatNumber = "0.";
            if (gcodeCompress)
                formatNumber = formatNumber.PadRight(num + 2, '#'); //'0'
            else
                formatNumber = formatNumber.PadRight(num + 2, '0'); //'0'
        }

        // get GCode one or two digits
        public static int getIntGCode(char code, string tmp)
        {   string cmdG = getStrGCode(code, tmp);       // find number string
            if (cmdG.Length > 0)
            {  return Convert.ToInt16(cmdG.Substring(1));  }
            return -1;
        }
        public static string getStrGCode(char code,string tmp)
        {
            int cmt = tmp.IndexOf("(");
            if (cmt == 0)
                return "";                      // nothing to do
            if (cmt >= 0)
                tmp = tmp.Substring(0,cmt);     // don't check inside comment
            var cmdG = Regex.Matches(tmp, code+"\\d{1,2}"); // find code and 1 or 2 digits
            if (cmdG.Count > 0)
            {  return cmdG[0].ToString();  }
            return "";
        }

        // get value from X,Y,Z F, S etc.
        public static int getIntValue(char code, string tmp)
        {
            string cmdG = getStringValue(code, tmp);
            if (cmdG.Length > 0)
            {  return Convert.ToInt16(cmdG.Substring(1));  }
            return -1;
        }
        public static string getStringValue(char code, string tmp)
        {
            var cmdG = Regex.Matches(tmp, code+ "-?\\d+(.\\d+)?");
            if (cmdG.Count > 0)
            {  return cmdG[cmdG.Count-1].ToString(); }
            return "";
        }

        public static string frmtCode(int number)      // convert int to string using format pattern
        {   return number.ToString(formatCode); }
        public static string frmtNum(float number)     // convert float to string using format pattern
        {   return number.ToString(formatNumber); }
        public static string frmtNum(double number)     // convert double to string using format pattern
        {   return number.ToString(formatNumber); }

        private static bool gcodeReduce = false;        // if true remove G1 commands if distance is < limit
        private static float gcodeReduceVal = 0.1f;     // limit when to remove G1 commands
        private static StringBuilder secondMove = new StringBuilder();
        private static bool applyXYFeedRate = true; // apply XY feed after each Pen-move

        public static void Pause(StringBuilder gcodeString, string cmt="")
        {
            if (cmt.Length > 0) cmt = string.Format("({0})", cmt);
            gcodeString.AppendFormat("M{0:00} {1}\r\n",0,cmt);
            gcodeLines++;
            gcodePauseCounter++;
        }

        public static void SpindleOn(StringBuilder gcodeString, string cmt="")
        {
            if (Properties.Settings.Default.importGCTTSSpeed) { cmt += " spindle speed from tool table"; }
            if (cmt.Length > 0) cmt = string.Format("({0})", cmt);
            gcodeString.AppendFormat("M{0} S{1} {2}\r\n", gcodeSpindleCmd, gcodeSpindleSpeed, cmt);
            gcodeLines++;
        }

        public static void SpindleOff(StringBuilder gcodeString, string cmt="")
        {
            if (cmt.Length > 0) cmt = string.Format("({0})", cmt);
            gcodeString.AppendFormat("M{0} {1}\r\n", frmtCode(5), cmt);
            gcodeLines++;
        }

        public static void PenDown(StringBuilder gcodeString, string cmto = "")
        {
            string cmt = cmto;
            if (Properties.Settings.Default.importGCTTZFeed) { cmt += " Z feed from tool table"; }
            if (Properties.Settings.Default.importGCTTZDeepth) { cmt += " Z down from tool table"; }
            drag1stMove = true;
            origFinalX = lastx;
            origFinalY = lasty;
            if (gcodeComments) { gcodeString.Append("\r\n"); }
            if (gcodeRelative) { cmt += string.Format("rel {0}", lastz); }
            if (cmt.Length >0) { cmt = string.Format("({0})", cmt); }

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

            gcodeDownUp++;
        }

        public static void PenUp(StringBuilder gcodeString, string cmto = "")
        {
            string cmt = cmto;
            drag1stMove = true;
            origFinalX = lastx;
            origFinalY = lasty;
            if (gcodeComments) { gcodeString.Append("\r\n"); }
            if (gcodeRelative) { cmt += string.Format("rel {0}", lastz); }
            if (cmt.Length >0) { cmt = string.Format("({0})", cmt); }

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

        public static float lastx, lasty, lastz, lastg, lastf;
        public static bool lastMovewasG0 = true;
        public static void MoveTo(StringBuilder gcodeString, Point coord, string cmt = "")
        {   MoveSplit(gcodeString, 1, (float)coord.X, (float)coord.Y, applyXYFeedRate, cmt); }
        public static void MoveTo(StringBuilder gcodeString, float x, float y, string cmt = "")
        {   MoveSplit(gcodeString, 1, x, y, applyXYFeedRate, cmt); }
        public static void MoveTo(StringBuilder gcodeString, float x, float y, float z, string cmt = "")
        {   MoveSplit(gcodeString, 1, x, y, z, applyXYFeedRate, cmt); }
        public static void MoveToRapid(StringBuilder gcodeString, Point coord, string cmt = "")
        {   Move(gcodeString, 0, (float)coord.X, (float)coord.Y, false, cmt); lastMovewasG0 = true; }
        public static void MoveToRapid(StringBuilder gcodeString, float x, float y, string cmt = "")
        {   Move(gcodeString, 0, x, y, false, cmt); lastMovewasG0 = true; }

        // MoveSplit breaks down a line to line segments with given max. length
        private static void MoveSplit(StringBuilder gcodeString, int gnr, float x, float y, bool applyFeed, string cmt)
        { MoveSplit(gcodeString, gnr, x, y, null, applyFeed, cmt); }

        private static float remainingX = 10, remainingY = 10, remainingC = 10;
        private static float segFinalX = 0, segFinalY = 0, segLastFinalX = 0, segLastFinalY = 0;
        private static void MoveSplit(StringBuilder gcodeString, int gnr, float finalx, float finaly, float? z, bool applyFeed, string cmt)
        {
            segLastFinalX = segFinalX; segLastFinalY = segFinalY;
            segFinalX = finalx; segFinalY = finaly;

            if (gcodeDragCompensation)  // start move with an arc and end with extended move
            {   Point tmp = dragToolCompensation(gcodeString, finalx, finaly);
                finalx = (float)tmp.X; finaly = (float)tmp.Y;   // get extended final position
            }

            if (Properties.Settings.Default.importGCLineSegmentation)       // apply segmentation
            {   float segFinalX = finalx, segFinalY = finaly;
                if (gcodeDragCompensation)
                {   lastx = segLastFinalX;// origLastX;
                    lasty = segLastFinalY;
                }
                float dx = finalx -  lastx;       // remaining distance until full move
                float dy = finaly -  lasty;       // lastXY is global
                float moveLength = (float)Math.Sqrt(dx * dx + dy * dy);
                float segmentLength = (float)Properties.Settings.Default.importGCLineSegmentLength;
                bool equidistance = Properties.Settings.Default.importGCLineSegmentEquidistant;

                //auch nach G0 move
                if (Properties.Settings.Default.importGCSubFirst && (lastMovewasG0 || (moveLength >= segmentLength)))       // also subroutine at first point
                {
                    //if (gcodeInsertSubroutine)
                    //    applyFeed = insertSubroutine(gcodeString, lastx, lasty, lastz, applyFeed);
                    remainingC = segmentLength;
                }

                if ((moveLength <= remainingC))//  && !equidistance)           // nothing to split 
                {
                    if (gcodeComments)
                        cmt += string.Format("{0:0.0} until subroutine",remainingC);
                    Move(gcodeString, 1, finalx, finaly, z, applyXYFeedRate, cmt);      // remainingC.ToString()
                    remainingC -= moveLength;
                    if (gcodeDragCompensation)
                        remainingC += gcodeDragRadius;
                }
                else
                {
                    float tmpX, tmpY, origX, origY, deltaX, deltaY;
                    int count = (int)Math.Ceiling(moveLength / segmentLength);
                    gcodeString.AppendFormat("(count {0})\r\n", count.ToString());
                    origX = lastx; origY = lasty;
                    if (equidistance)               // all segments in same length (but shorter than set)
                    {   for (int i = 1; i < count; i++)
                        {   deltaX = i * dx / count;
                            deltaY = i * dy / count;
                            tmpX = origX + deltaX;
                            tmpY = origY + deltaY;
                            Move(gcodeString, 1, tmpX, tmpY, z, applyFeed, cmt);
                            if (i >= 1) { applyFeed = false; cmt = ""; }
                            //if (gcodeInsertSubroutine)
                            //    applyFeed = insertSubroutine(gcodeString, lastx, lasty, lastz, applyFeed);
                        }
                    }
                    else
                    {
                        remainingX = dx * remainingC / moveLength;
                        remainingY = dy * remainingC / moveLength;
                        for (int i = 0; i < count; i++)
                        {   deltaX = remainingX + i * segmentLength * dx / moveLength;        // n-1 segments in exact length, last segment is shorter
                            deltaY = remainingY + i * segmentLength * dy / moveLength;
                            tmpX = origX + deltaX;
                            tmpY = origY + deltaY;
                            remainingC = segmentLength;
                            if ((float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY) >= moveLength)
                                break;
                            Move(gcodeString, 1, tmpX, tmpY, z, applyFeed, cmt);
                            if (i >= 1) { applyFeed = false; cmt = ""; }
                            //if (gcodeInsertSubroutine)
                            //    applyFeed = insertSubroutine(gcodeString, lastx, lasty, lastz, applyFeed);
                        }
                    }
                    finalx = segFinalX; finaly = segFinalY;
                    remainingC = segmentLength - fdistance(finalx, finaly, lastx, lasty);
                    Move(gcodeString, 1, finalx, finaly, z, applyFeed, cmt);
                    if ((equidistance) || (remainingC == 0))
                    {
                        //if (gcodeInsertSubroutine)
                        //    insertSubroutine(gcodeString, lastx, lasty, lastz, applyFeed);
                        remainingC = segmentLength;
                    }
                }
            }
            else
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
                {   MoveArc(gcodeString, gcnr, rx, ry, dragCompi, dragCompj, applyXYFeedRate, "Drag t. comp.");// + angle[2].ToString());
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
        {   if (gnr == 0)
            {   segLastFinalX = segFinalX; segLastFinalY = segFinalY;
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
            {   z_relative = (float)z - lastz;
                tz = (float)z;
            }

            if (applyFeed && (gnr > 0))
            {
                feed = string.Format("F{0}", gcodeXYFeed);
                applyXYFeedRate = false;                        // don't set feed next time
            }
            if (Properties.Settings.Default.importGCTTXYFeed) { cmt += " XY feed from tool table"; }
            if (cmt.Length > 0) cmt = string.Format("({0})", cmt);

            if (gcodeCompress)
            {
                if (((gnr > 0) || (lastx != x) || (lasty != y) || (lastz != tz)))  // else nothing to do
                {
                    if (lastg != gnr) { gcodeTmp.AppendFormat("G{0}", frmtCode(gnr)); isneeded = true; }
                    if (gcodeRelative)
                    {
                        if (lastx != x) { gcodeTmp.AppendFormat("X{0}", frmtNum(x_relative)); isneeded = true; }
                        if (lasty != y) { gcodeTmp.AppendFormat("Y{0}", frmtNum(y_relative)); isneeded = true; }
                        if (z!=null)
                        {
                            if (lastz != z) { gcodeTmp.AppendFormat("Z{0}", frmtNum(z_relative)); isneeded = true; }
                        }
                    }
                    else
                    {
                        if (lastx != x) { gcodeTmp.AppendFormat("X{0}", frmtNum(x)); isneeded = true; }
                        if (lasty != y) { gcodeTmp.AppendFormat("Y{0}", frmtNum(y)); isneeded = true; }
                        if (z != null)
                        {
                            if (lastz != z) { gcodeTmp.AppendFormat("Z{0}", frmtNum((float)z)); isneeded = true; }
                        }
                    }

                    if ((gnr == 1) && (lastf != gcodeXYFeed) || applyFeed)
                    {
                        gcodeTmp.AppendFormat("F{0} ", gcodeXYFeed);
                        lastf = gcodeXYFeed;
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
                {
                    if (gcodeRelative)
                        gcodeString.AppendFormat("G{0} X{1} Y{2} Z{3} {4} {5}\r\n", frmtCode(gnr), frmtNum(x_relative), frmtNum(y_relative), frmtNum(z_relative), feed, cmt);
                    else
                        gcodeString.AppendFormat("G{0} X{1} Y{2} Z{3} {4} {5}\r\n", frmtCode(gnr), frmtNum(x), frmtNum(y), frmtNum((float)z), feed, cmt);
                }
                else
                {
                    if (gcodeRelative)
                        gcodeString.AppendFormat("G{0} X{1} Y{2} {3} {4}\r\n", frmtCode(gnr), frmtNum(x_relative), frmtNum(y_relative), feed, cmt);
                    else
                        gcodeString.AppendFormat("G{0} X{1} Y{2} {3} {4}\r\n", frmtCode(gnr), frmtNum(x), frmtNum(y), feed, cmt);
                }
            }
            gcodeTime += delta / gcodeXYFeed;
            lastx = x; lasty = y; lastg = gnr; // lastz = tz;
            gcodeLines++;
        }

        public static void splitLine(StringBuilder gcodeString, int gnr, float x1, float y1, float x2, float y2, float maxStep, bool applyFeed, string cmt = "")
        {
            float dx = x2 - x1;
            float dy = y2 - y1;
            float c = (float)Math.Sqrt(dx * dx + dy * dy);
            float tmpX,tmpY;
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
        private static void MoveArc(StringBuilder gcodeString, int gnr, float x, float y, float i, float j, bool applyFeed, string cmt="", bool avoidG23 = false)
        {
            string feed = "";
            float x_relative = x - lastx;
            float y_relative = y - lasty;

            if (applyFeed)
            {   feed = string.Format("F{0}", gcodeXYFeed);
                applyXYFeedRate = false;                        // don't set feed next time
            }
            if (cmt.Length > 0) cmt = string.Format("({0})", cmt);
            if (gcodeNoArcs || avoidG23)
            {
                    splitArc(gcodeString, gnr, lastx, lasty, x, y, i, j, applyFeed, cmt);
            }
            else
            {
                if (gcodeRelative)
                    gcodeString.AppendFormat("G{0} X{1} Y{2}  I{3} J{4} {5} {6}\r\n", frmtCode(gnr), frmtNum(x_relative), frmtNum(y_relative), frmtNum(i), frmtNum(j), feed, cmt);
                else
                    gcodeString.AppendFormat("G{0} X{1} Y{2}  I{3} J{4} {5} {6}\r\n", frmtCode(gnr), frmtNum(x), frmtNum(y), frmtNum(i), frmtNum(j), feed, cmt);
                lastg = gnr;
            }
            gcodeTime += fdistance(lastx, lasty, x, y) / gcodeXYFeed;
            lastx = x; lasty = y; lastf = gcodeXYFeed;
            gcodeLines++;
        }

        public static void splitArc(StringBuilder gcodeString, int gnr, float x1, float y1, float x2, float y2, float i, float j, bool applyFeed, string cmt = "")
        {
            float segmentLength = (float)Properties.Settings.Default.importGCLineSegmentLength;
            bool equidistance = Properties.Settings.Default.importGCLineSegmentEquidistant;

            float radius = (float)Math.Sqrt(i * i + j * j);					// get radius of circle
            float cx = x1 + i, cy = y1 + j;                                 // get center point of circle

            float[] ret = getAngle(x1,y1,x2,y2,i,j);
            float a1 = ret[0], a2 = ret[1], da = ret[2];

            da = -(360 + a1 - a2);
            if (gnr == 3) { da = Math.Abs(360 + a2 - a1); }
            if (da > 360) { da -= 360; }
            if (da < -360) { da += 360; }

            if ((x1 == x2) && (y1 == y2))
            {   if (gnr == 2) { da = -360; }
                else { da = 360; }
            }
            float step = (float)(Math.Asin((double)gcodeAngleStep / (double)radius) * 180 / Math.PI);

            applyXYFeedRate = true;
            float moveLength= remainingC;
            int count;
            if (equidistance)
            {   float circum = radius * da* (float)Math.PI / 180;
                count = (int)Math.Ceiling(circum / segmentLength);
                segmentLength = circum / count;
                //Comment(gcodeString, circum.ToString() + " " + count.ToString() + " " + segmentLength.ToString() );
                moveLength = 0;
            }
            count = 1;
            if (da > 0)                                             // if delta >0 go counter clock wise
            {
                for (float angle = (a1+step); angle < (a1+da); angle+= step)
                {
                    float x = cx + radius * (float)Math.Cos(Math.PI * angle / 180);
                    float y = cy + radius * (float)Math.Sin(Math.PI * angle / 180);
                    moveLength += fdistance(x, y, lastx, lasty);

                    Move(gcodeString, 1, x, y, applyXYFeedRate, cmt);

                    if (moveLength >= (count*segmentLength))
                    {
                        applyXYFeedRate = true;// insertSubroutine(gcodeString, lastx, lasty, lastz, applyXYFeedRate);
                        count++;
                    }
                    if (cmt.Length > 1) cmt = "";
                }
            }
            else                                                       // else go clock wise
            {
                for (float angle = (a1-step); angle > (a1+da); angle-= step)
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
            float radius = (float)Math.Sqrt(i * i + j * j);				// get radius of circle

            float cos1 = i / radius;									// get start angle
            if (cos1 > 1) cos1 = 1;
            if (cos1 < -1) cos1 = -1;
            float a1 = 180 - 180 * (float)(Math.Acos(cos1) / Math.PI);
            if (j > 0) { a1 = -a1; }										

            float cos2 = (float)(x1 + i - x2) / radius;                 // get stop angle
            if (cos2 > 1) cos2 = 1;
            if (cos2 < -1) cos2 = -1;
            float a2 = 180 - 180 * (float)(Math.Acos(cos2) / Math.PI);
            if ((y1 + j - y2) > 0) { a2 = -a2; }		    			// get delta angle

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

        public static string GetHeader(string cmt,string source="")
        {
            gcodeTime += gcodeDistance / gcodeXYFeed;
            string header = ""; //= "( "+cmt+" by GRBL-Plotter )\r\n";
            //if (source.Length>1)
            //    header += string.Format("( Source: {0} )\r\n", source);
            //if (Properties.Settings.Default.importSVGRepeatEnable)
            //    header += string.Format("( G-Code repetitions: {0:0} times)\r\n", Properties.Settings.Default.importSVGRepeat);
            //header += string.Format("( G-Code lines: {0} )\r\n", gcodeLines);
            //header += string.Format("( Pen Down/Up : {0} times )\r\n", gcodeDownUp);
            //header += string.Format("( Path length : {0:0.0} units )\r\n", gcodeDistance);
            //header += string.Format("( Duration ca.: {0:0.0} min. )\r\n", gcodeTime);
            //if (gcodeSubroutineCount > 0)
            //    header += string.Format("( Call to subs.: {0} )\r\n", gcodeSubroutineCount);

            stopwatch.Stop();
            //header += string.Format("( Conv. time  : {0} )\r\n", stopwatch.Elapsed);

            //if (gcodeToolChange)
            //{
            //    header += string.Format("( Tool changes: {0})\r\n", gcodeToolCounter);
            //    header += gcodeToolText;
            //}
            //if (gcodePauseCounter>0)
            //    header += string.Format("( M0 count    : {0})\r\n", gcodePauseCounter);
            string[] commands = Properties.Settings.Default.importGCHeader.Split(';');
            //foreach (string cmd in commands)
            //    if (cmd.Length > 1)
            //    { header += string.Format("{0} (Setup - GCode-Header)\r\n", cmd.Trim()); gcodeLines++; }
            if (gcodeRelative)
            { header += string.Format("G91 (Setup relative movement)\r\n"); gcodeLines++; }

            if (Properties.Settings.Default.importUnitGCode)
            {
                if (Properties.Settings.Default.importUnitmm)
                { header += "G21 (use mm as unit - check setup)"; }
                else
                { header += "G20 (use inch as unit - check setup)"; }
            }
            return header;
        }

        public static string GetFooter()
        {
            string footer = "";
            string[] commands = Properties.Settings.Default.importGCFooter.Split(';');
            foreach (string cmd in commands)
                if (cmd.Length > 1)
                { footer += string.Format("{0} (Setup - GCode-Footer)\r\n", cmd.Trim()); gcodeLines++; }

            //if (gcodeToolChange && Properties.Settings.Default.ctrlToolChangeEmpty)
            //{ footer += string.Format("T{0} M{1} (Remove tool)\r\n", frmtCode((int)Properties.Settings.Default.ctrlToolChangeEmptyNr), frmtCode(6)); }

            footer += "M30 (Program end)\r\n";
            return footer + gcodeSubroutine;
        }

        //public static void Comment(StringBuilder gcodeString, string cmt)
        //{   if (cmt.Length>1)
        //        gcodeString.AppendFormat("({0})\r\n", cmt);
        //}

        // helper functions
        private static float fsqrt(float x) { return (float)Math.Sqrt(x); }
        private static float fvmag(float x, float y) { return fsqrt(x * x + y * y); }
        private static float fdistance(float x1, float y1, float x2, float y2) { return fvmag(x2 - x1, y2 - y1); }
    }
}
