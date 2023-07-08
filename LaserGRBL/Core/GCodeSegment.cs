//Copyright (c) 2023 Alexandre Besnier - https://github.com/Varamil/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Text.RegularExpressions;

namespace LaserGRBL
{
    class GCodeSegment
    {
        /// <summary>
        /// Start position of the segment
        /// </summary>
        public Point Start;
        /// <summary>
        /// Stop position of the segments
        /// </summary>
        public Point Stop;
        /// <summary>
        /// GCode list used to reach <see cref="Stop"/> from <see cref="Start"/>
        /// </summary>
        public List<string> gcode;
        /// <summary>
        /// Power used during the segment
        /// </summary>
        public int Power;

        #region Constructors
        public GCodeSegment(Point s, int p)
        {
            Start = new Point(s.X, s.Y);
            Power = p;
            gcode = new List<string>();
        }

        public GCodeSegment(Point st, Point sp, string gc)
        {
            Start = st;
            Stop = sp;
            gcode = new List<string>();
            gcode.Add(gc);
        }
        #endregion

        #region 2D Calculation
        /// <summary>
        /// Gets the distance a given point to 0,0. 
        /// </summary>
        /// <param name="p"></param>
        /// <remarks>Not the math distance, the square root is not apply to save calculation</remarks>
        /// <returns></returns>
        static public double Distance(Point p)
        {
            return p.X * p.X + p.Y * p.Y;
        }

        /// <summary>
        /// Gets the distance between 2 points
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <remarks>Not the math distance, the square root is not apply to save calculation</remarks>
        /// <returns></returns>
        static public double Distance(Point p1, Point p2)
        {
            return (p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y);
        }

        /// <summary>
        /// Gets the distance of the segment to 0,0
        /// </summary>
        /// <remarks>Not the math distance, the square root is not apply to save calculation</remarks>
        /// <returns></returns>
        public double Distance()
        {
            return Math.Min(Distance(Start), Distance(Stop));
        }

        /// <summary>
        /// Gets the distance of the segment to a given point
        /// </summary>
        /// <param name="p"></param>
        /// <remarks>Not the math distance, the square root is not apply to save calculation</remarks>
        /// <returns></returns>
        public double DistanceTo(Point p)
        {
            return Math.Min(Distance(Start, p), Distance(Stop, p));
        }

        /// <summary>
        /// Gets the (general) slope of the current segment.  
        /// </summary>
        /// <returns></returns>
        public double Slope()
        {
            return (Stop.X - Start.X) == 0 ? double.MaxValue : (Stop.Y - Start.Y) / (Stop.X - Start.X);
        }
        #endregion

        #region Optimization
        /// <summary>
        /// Clean and optimize a list of gcode commands
        /// </summary>
        /// <param name="gcode">gcode string to optimize</param>
        /// <returns></returns>
        static public string[] CleanGCode(string gcode)
        {
            return CleanGCode(new List<string>(gcode.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)));
        }

        /// <summary>
        /// Clean and optimize a list of gcode commands
        /// </summary>
        /// <param name="commands">List of <see cref="GrblCommand"/> to optimize</param>
        /// <returns></returns>
        static public string[] CleanGCode(List<GrblCommand> commands)
        {
            return CleanGCode(commands.Select(c => c.Command).ToList());
        }

        /// <summary>
        /// Clean and optimize a list of gcode commands
        /// </summary>
        /// <param name="commands">The list of commands to optimize</param>
        /// <returns></returns>
        static public string[] CleanGCode(List<string> commands)
        {
            List<string> inlines = commands;
            List<string> outlines = new List<string>();

            try
            {
                //First clean pen start/stop
                char nextcmd, cmd = char.ToUpper(inlines[0].Take(1).Single());
                for (int i = 0; i < inlines.Count - 1; i++)
                {
                    nextcmd = char.ToUpper(inlines[i + 1].Take(1).Single());
                    if (cmd != 'S' || cmd != nextcmd)
                    {
                        outlines.Add(inlines[i]);
                    } //else skip this S command because will be overwritten by the next one

                    cmd = nextcmd;
                }
                outlines.Add(inlines[inlines.Count - 1]); //do not forget the last one

                inlines = outlines;
                outlines = new List<string>();

                //secondly clean S with value equal to current value
                int power = -1, newpower;
                for (int i = 0; i < inlines.Count; i++)
                {
                    cmd = char.ToUpper(inlines[i].Take(1).Single());
                    if (cmd == 'S' && int.TryParse(Regex.Match(inlines[i].Substring(1), "^[0-9]+").Value, out newpower))
                    {
                        if (newpower != power)
                        {
                            outlines.Add(inlines[i]);
                            power = newpower;
                        }
                    }
                    else
                        outlines.Add(inlines[i]);
                }

                //identify segments

                inlines = outlines;
                outlines = new List<string>();
                Point currentPosition = new Point(0, 0);
                GCodeSegment currentSegment;
                List<GCodeSegment> segments = new List<GCodeSegment>();
                List<int> powers = new List<int>();
                for (int i = 0; i < inlines.Count; i++)
                {
                    cmd = gcodeCommand(inlines[i]);
                    if (cmd == 'M') //group using M commands 
                    {
                        // end group, 
                        if (segments.Count > 0)
                        {
                            outlines.AddRange(GCodeSegment.optimizeSegments(segments, powers));
                        }
                        outlines.Add(inlines[i]);

                        //start a new one
                        segments = new List<GCodeSegment>();
                    }
                    else
                    {
                        //look for a S
                        if (cmd == 'S')
                        {
                            //save the power
                            power = 0;
                            int.TryParse(Regex.Match(inlines[i].Substring(1), "^[0-9]+").Value, out power);

                            if (power == 0) // just movement, so save the last one
                            {
                                for (int j = i + 1; j < inlines.Count; j++)
                                {
                                    if (gcodeCommand(inlines[j]) == 'G')
                                        currentPosition = GCodeSegment.GCode2position(inlines[j], currentPosition);
                                    else
                                    {
                                        i = j - 1;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                // New segment
                                currentSegment = new GCodeSegment(currentPosition, power);
                                if (!powers.Contains(power)) powers.Add(power);

                                for (int j = i + 1; j < inlines.Count; j++)
                                {
                                    if (gcodeCommand(inlines[j]) == 'G')
                                    {
                                        currentPosition = GCodeSegment.GCode2position(inlines[j], currentPosition);
                                        currentSegment.gcode.Add(inlines[j]);
                                    }
                                    else
                                    {
                                        //stop is current position
                                        currentSegment.Stop = new Point(currentPosition.X, currentPosition.Y);
                                        //save the segment
                                        segments.Add(currentSegment);

                                        i = j - 1;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                if (segments.Count > 0)
                {
                    // missing M at the end, correct that and finalize
                    if (segments.Count > 0)
                    {
                        outlines.AddRange(GCodeSegment.optimizeSegments(segments, powers));
                    }
                    outlines.Add("M5 S0");
                }

                //Sort 
            }
            catch (Exception ex)
            {
                if (ex != null)
                {

                }
            }

            return outlines.ToArray();
        }
        /// <summary>
        /// Optimizes a list of commands (sort and merge)
        /// </summary>
        /// <param name="segments">The list of commands to optimize</param>
        /// <param name="powers">List of power used by commands. Commands are optimized within a same power</param>
        /// <returns></returns>
        static public List<string> optimizeSegments(List<GCodeSegment> segments, List<int> powers)
        {
            List<string> outcode = new List<string>();

            //optimize for each power
            foreach (int power in powers)
            {
                List<GCodeSegment> seg = segments.FindAll(sg => sg.Power == power);
                List<GCodeSegment> sortedseg = new List<GCodeSegment>();

                //find the segment closest to 0,0
                int iRef = 0;
                double distmin = seg[iRef].Distance();
                for (int i = 1; i < seg.Count; i++)
                {
                    if (seg[i].Distance() < distmin)
                    {
                        iRef = i;
                        distmin = seg[i].Distance();
                    }
                }

                //need to revert 1st segment ?
                if (GCodeSegment.Distance(seg[iRef].Start) > GCodeSegment.Distance(seg[iRef].Stop))
                {
                    seg[iRef].Revert();
                }

                //sort segments
                GCodeSegment currentseg = seg[iRef];
                sortedseg.Add(seg[iRef]);
                seg.RemoveAt(iRef);
                double d, dmin, s;
                while (seg.Count > 0)
                {
                    dmin = double.MaxValue;
                    iRef = -1;
                    s = currentseg.Slope();
                    //find the closest segment
                    for (int i = 0; i < seg.Count; i++)
                    {
                        d = seg[i].DistanceTo(currentseg.Stop);
                        if (d < dmin)
                        {
                            dmin = d;
                            iRef = i;
                        }
                        else if (d == dmin)
                        {
                            if (s < double.MaxValue && Math.Abs(seg[i].Slope() - s) < Math.Abs(seg[iRef].Slope() - s) || s == double.MaxValue && seg[i].Slope() > seg[iRef].Slope())
                            {
                                dmin = d;
                                iRef = i;
                            }
                        }
                    }

                    if (iRef > -1)
                    {
                        if (GCodeSegment.Distance(currentseg.Stop, seg[iRef].Start) > GCodeSegment.Distance(currentseg.Stop, seg[iRef].Stop))
                        {
                            seg[iRef].Revert();
                        }

                        currentseg = seg[iRef];
                        sortedseg.Add(seg[iRef]);
                        seg.RemoveAt(iRef);
                    } //else, not found ???
                }

                //merge segments
                seg = new List<GCodeSegment>();
                double epsilon = 0.01; // todo : TBD
                currentseg = sortedseg[0];
                seg.Add(currentseg);
                for (int i = 1; i < sortedseg.Count; i++)
                {
                    if (GCodeSegment.Distance(currentseg.Stop, sortedseg[i].Start) < epsilon)
                    {
                        //can be merged
                        currentseg.gcode.AddRange(sortedseg[i].gcode);
                        currentseg.Stop = sortedseg[i].Stop;
                    }
                    else
                    {
                        seg.Add(sortedseg[i]);
                        currentseg = sortedseg[i];
                    }
                }

                //reconstruct gcode list

                for (int i = 0; i < seg.Count; i++)
                {
                    outcode.Add("S0"); //before move
                    outcode.Add("G0" + "X" + seg[i].Start.X + "Y" + seg[i].Start.Y); //move to start
                    outcode.Add("S" + power.ToString()); //fire
                    outcode.AddRange(seg[i].gcode);
                }
            }

            return outcode;
        }

        /// <summary>
        /// Reverts the path 
        /// </summary>
        public void Revert()
        {
            Point currentPosition = Start;
            Start = Stop;

            //construct list of forward positions
            List<GCodeSegment> pos = new List<GCodeSegment>();
            for (int i = 0; i < gcode.Count; i++)
            {
                pos.Add(new GCodeSegment(currentPosition, GCode2position(gcode[i], currentPosition), gcode[i]));
                currentPosition = pos.Last().Stop;
            }

            List<string> gc = new List<string>();
            string cmd, xy, offx, offy;
            Point center;
            //generate backward commands
            for (int i = pos.Count - 1; i >= 0; i--)
            {
                cmd = pos[i].gcode[0];
                switch (cmd.Substring(0, 2).ToUpper())
                {
                    case "G1":
                        xy = ((pos[i].Start.X == pos[i].Stop.X) ? "" : "X" + pos[i].Start.X) + ((pos[i].Start.Y == pos[i].Stop.Y) ? "" : "Y" + pos[i].Start.Y);
                        cmd = Regex.Replace(cmd, "((X|Y)[0-9.-]+)+", xy);
                        currentPosition = GCode2position(cmd, currentPosition);
                        gc.Add(cmd);
                        break;

                    case "G2":
                        center = GCode2center(cmd, pos[i].Start);
                        cmd = "G3" + cmd.Substring(2);

                        xy =  "X" + pos[i].Start.X +  "Y" + pos[i].Start.Y;
                        cmd = Regex.Replace(cmd, "((X|Y)[0-9.-]+)+", xy);

                        xy = string.Format("I{0}J{1}", Math.Round(center.X - pos[i].Stop.X, 3), Math.Round(center.Y - pos[i].Stop.Y, 3));
                        cmd = Regex.Replace(cmd, "((I|J)[0-9.-]+)+", xy);

                        currentPosition = GCode2position(cmd, currentPosition);
                        gc.Add(cmd);
                        break;

                    case "G3":
                        center = GCode2center(cmd, pos[i].Start);
                        cmd = "G2" + cmd.Substring(2);

                        xy = "X" + pos[i].Start.X + "Y" + pos[i].Start.Y;
                        cmd = Regex.Replace(cmd, "((X|Y)[0-9.-]+)+", xy);

                        xy = string.Format("I{0}J{1}", Math.Round(center.X - pos[i].Stop.X, 3), Math.Round(center.Y - pos[i].Stop.Y, 3));
                        cmd = Regex.Replace(cmd, "((I|J)[0-9.-]+)+", xy);


                        currentPosition = GCode2position(cmd, currentPosition);
                        gc.Add(cmd);
                        break;

                    default: //shouldn't happen
                        gc.Add(cmd);
                        break;
                }
            }

            //finalize
            gcode = gc;
            Stop = currentPosition;
        }
        #endregion

        #region Tools
        /// <summary>
        /// Gets the end absolute position after the execution of the given command
        /// </summary>
        /// <param name="cmd">The command to analyze</param>
        /// <param name="currentPosition">Poisition (absolute) before command execution</param>
        /// <returns></returns>
        static public Point GCode2position(string cmd, Point currentPosition)
        {
            double X = currentPosition.X;
            double Y = currentPosition.Y;

            if (cmd.Contains("X")) double.TryParse(Regex.Match(cmd, "(?<=X)[0-9.-]+").Value, out X);
            if (cmd.Contains("Y")) double.TryParse(Regex.Match(cmd, "(?<=Y)[0-9.-]+").Value, out Y);

            return new Point(X, Y);
        }

        static public Point GCode2center(string cmd, Point currentPosition)
        {
            double X = 0;
            double Y = 0;

            if (cmd.Contains("I")) double.TryParse(Regex.Match(cmd, "(?<=I)[0-9.-]+").Value, out X);
            if (cmd.Contains("J")) double.TryParse(Regex.Match(cmd, "(?<=J)[0-9.-]+").Value, out Y);

            return new Point(currentPosition.X + X, currentPosition.Y + Y);
        }


        /// <summary>
        /// Gets the first letter of a command
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private static char gcodeCommand(string cmd)
        {
            return char.ToUpper(cmd.Take(1).Single());
        }

        public new string ToString()
        {
            return Start.ToString() + " / " + Stop.ToString() + " (" + gcode.Count + ")";
        }
        #endregion


    }
}
