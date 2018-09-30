///*  GRBL-Plotter. Another GCode sender for GRBL.
//    This file is part of the GRBL-Plotter application.
   
//    Copyright (C) 2015-2018 Sven Hasemann contact: svenhb@web.de

//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.

//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.

//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.
//*/
///*
//    2018-08 only use tool table for color palette
//*/

//using System;
//using System.Drawing;
//using System.IO;
//using System.Windows.Forms;

//namespace GRBL_Plotter
//{
//    public struct toolProp
//    {
//        public int toolnr;
//        public System.Drawing.Color color;
//        public String name;
//        public bool use;
//        public int codeSize;
//        public int pixelCount;
//        public double diff;

//        public float X, Y, Z;
//        public float diameter;
//        public float feedXY;
//        public float feedZ;
//        public float stepZ;
//        public float spindleSpeed;
//        public float overlap;
//    }
//    public struct toolPos
//    {
//        public int toolnr;
//        public String name;
//        public float X, Y;
//    }
//    public static class toolTable
//    {
//        private static int svgToolMax = 100;            // max amount of tools
//        private static toolProp[] svgToolTable = new toolProp[svgToolMax];   // load color palette into this array
//        private static int svgToolIndex = 0;            // last index
//        private static bool useException = false;
//        private static int tmpIndex = 0;
//        private static bool init_done = false;

//        // defaultTool needed in Setup      ToolNr,color,name,X,Y,Z,diameter,XYspeed,Z-step, Zspeed, spindleSpeed, overlap
//        public static string[] defaultTool = { "1", "000000", "Default black", "0.0", "0.0", "0.0", "3.0", "500","1","100","10000","75" };

//        public static toolPos[] getToolCordinates()
//        {   if (!Properties.Settings.Default.importGCTool || !init_done)
//            return null;
//            toolPos[] newpos = new toolPos[svgToolTable.Length];
//            int index = 0;
//            foreach (toolProp tool in svgToolTable)
//            {   if (tool.toolnr >= 0)
//                {   newpos[index].toolnr = tool.toolnr;
//                    newpos[index].name = tool.name;
//                    newpos[index].X = tool.X + (float)Properties.Settings.Default.toolOffX;
//                    newpos[index].Y = tool.Y + (float)Properties.Settings.Default.toolOffY;
//                    index++;
//                }
//            }
//            Array.Resize(ref newpos, index);
//            return newpos;
//        }
//        public static string getToolName(int index)
//        {   foreach (toolProp tool in svgToolTable)
//            {   if (index == tool.toolnr)
//                    return tool.name;
//            }
//            return "not defined";
//        }

//        public static toolProp getToolProperties(int index)
//        {   foreach (toolProp tool in svgToolTable)
//            {   if (index == tool.toolnr)
//                { tmpIndex = index; return tool; }
//            }
//            tmpIndex = 2;
//            return svgToolTable[2]; // return 1st regular tool;
//        }

//        public static void setToolCodeSize(int index, int size)
//        {
//            Array.Sort<toolProp>(svgToolTable, (x, y) => x.toolnr.CompareTo(y.toolnr));
//            if (index < 0) index = 0;
//            if (index >= svgToolIndex - 2) index = svgToolIndex - 2;
//            svgToolTable[index + 1].codeSize = size;
//        }
//        public static void setIndex(int index)
//        {   if ((index >= 0) && (index < svgToolIndex))
//                tmpIndex = index;
//        }
//        public static int indexToolNr()
//        {   return svgToolTable[tmpIndex].toolnr; }
//        public static bool indexUse()
//        {   return svgToolTable[tmpIndex].use;    }
//        public static string indexName()
//        { return svgToolTable[tmpIndex].name; }

//        public static void sortByToolNr()
//        {   Array.Sort<toolProp>(svgToolTable, (x, y) => x.toolnr.CompareTo(y.toolnr));    // sort by tool nr
//        }
//        public static void sortByCodeSize()
//        {   Array.Sort<toolProp>(svgToolTable, (x, y) => y.codeSize.CompareTo(x.codeSize));    // sort by size
//        }
//        public static void sortByPixelCount()
//        {   Array.Sort<toolProp>(svgToolTable, (x, y) => y.pixelCount.CompareTo(x.pixelCount));    // sort by size
//        }

//        public static toolProp setDefault()
//        {   toolProp tmp = new toolProp();
//            tmp.X = 0; tmp.Y = 0; tmp.Z = 0; tmp.diameter=3; tmp.feedXY=100; tmp.feedZ=100;
//            tmp.stepZ=-1; tmp.spindleSpeed=1000;tmp.overlap=100;
//            return tmp;
//        }
//        // set tool / color table
//        public static int init()    // return number of entries
//        {
//            init_done = true;
//            useException=false;
//            Array.Resize(ref svgToolTable, svgToolMax);
//            svgToolIndex = 3;
//            svgToolTable[0] = setDefault();
//            svgToolTable[0].toolnr = -2;            // alpha=0
//            svgToolTable[0].color = Color.White; 
//            svgToolTable[0].use = false; 
//            svgToolTable[0].diff = int.MaxValue; 
//            svgToolTable[0].name = "except";
//            svgToolTable[0].pixelCount=0;

//            svgToolTable[1] = setDefault();
//            svgToolTable[1].toolnr = -1;
//            svgToolTable[1].color = Color.White;
//            svgToolTable[1].use = false;
//            svgToolTable[1].diff = int.MaxValue;
//            svgToolTable[1].name = "except";
//            svgToolTable[1].pixelCount = 0;

//            long clr = 0;
//            clr = Convert.ToInt32(defaultTool[1], 16) | 0xff000000;
//            svgToolTable[2] = setDefault();
//            svgToolTable[2].toolnr =  Convert.ToInt32(defaultTool[0]);
//            svgToolTable[2].pixelCount = 0;
//            svgToolTable[2].use = false;
//            svgToolTable[2].color = System.Drawing.Color.FromArgb((int)clr);
//            svgToolTable[2].diff = int.MaxValue;
//            svgToolTable[2].name = defaultTool[2];

//            string file = System.Windows.Forms.Application.StartupPath + "\\tools.csv";
//            if (File.Exists(file))
//            {
//                string[] readText = File.ReadAllLines(file);
//                string[] col;
//                svgToolIndex = 2;
//                foreach (string s in readText)
//                {   if (s.Length > 25)
//                    {   col = s.Split(';'); //ToolNr,color,name,X,Y,Z,diameter,XYspeed,Z-step, Zspeed, spindleSpeed, overlap
//                        clr = Convert.ToInt32(col[1], 16) | 0xff000000;
//                        svgToolTable[svgToolIndex] = setDefault();
//                        svgToolTable[svgToolIndex].toolnr = Convert.ToInt32(col[0]);
//                        svgToolTable[svgToolIndex].color = System.Drawing.Color.FromArgb((int)clr);
//                        svgToolTable[svgToolIndex].use = false;
//                        svgToolTable[svgToolIndex].diff = int.MaxValue;
//                        svgToolTable[svgToolIndex].name = col[2];
//                        svgToolTable[svgToolIndex].pixelCount = 0;
//                        svgToolTable[svgToolIndex].use = false;
//                        svgToolTable[svgToolIndex].X = float.Parse(col[3], System.Globalization.NumberFormatInfo.InvariantInfo);
//                        svgToolTable[svgToolIndex].Y = float.Parse(col[4], System.Globalization.NumberFormatInfo.InvariantInfo);
//                        svgToolTable[svgToolIndex].Z = float.Parse(col[5], System.Globalization.NumberFormatInfo.InvariantInfo);
//                        svgToolTable[svgToolIndex].diameter = float.Parse(col[6], System.Globalization.NumberFormatInfo.InvariantInfo);
//                        svgToolTable[svgToolIndex].feedXY   = float.Parse(col[7], System.Globalization.NumberFormatInfo.InvariantInfo);
//                        svgToolTable[svgToolIndex].stepZ    = float.Parse(col[8], System.Globalization.NumberFormatInfo.InvariantInfo);
//                        svgToolTable[svgToolIndex].feedZ    = float.Parse(col[9], System.Globalization.NumberFormatInfo.InvariantInfo);
//                        svgToolTable[svgToolIndex].spindleSpeed = float.Parse(col[10], System.Globalization.NumberFormatInfo.InvariantInfo);
//                        svgToolTable[svgToolIndex].overlap  = float.Parse(col[11], System.Globalization.NumberFormatInfo.InvariantInfo);
//                        //TryParse
//                        if (svgToolIndex < svgToolMax - 1) svgToolIndex++;
//                    }
//                }
//            }
//            Array.Resize(ref svgToolTable, svgToolIndex);
//            return svgToolIndex;
//        }

//        private static void showList()
//        {
//            string tmp="";
//            for (int i = 0; i < svgToolTable.Length; i++)
//             { tmp += i.ToString() + "  " + svgToolTable[i].toolnr + "  " + svgToolTable[i].name + "  " + svgToolTable[i].color.ToString() + "  " + String.Format("{0:X}", svgToolTable[i].pixelCount) + "\r\n"; }
//             MessageBox.Show(tmp);
//        }
//        // set exception color
//        public static string setExceptionColor(Color mycolor)
//        {   useException=true;
//            Array.Sort<toolProp>(svgToolTable, (x, y) => x.toolnr.CompareTo(y.toolnr));    // sort by tool nr
//            svgToolTable[0].toolnr = -1; 
//            svgToolTable[0].color = mycolor; 
//            svgToolTable[0].use = false; 
//            svgToolTable[0].diff = int.MaxValue; 
//            svgToolTable[0].name = "not used";
//            return svgToolTable[0].color.ToString();
//        }
//        // Clear exception color
//        public static void clrExceptionColor()
//        {   useException=false;}

//        // return tool nr of nearest color
//        public static int getToolNr(String mycolor, int mode)
//        {
//            if (mycolor == "")
//                return (int)Properties.Settings.Default.importGCToolDefNr;  // return default tool
//            int cr, cg, cb;
//            int num = int.Parse(mycolor, System.Globalization.NumberStyles.AllowHexSpecifier);
//            cb = num & 255; cg = num >> 8 & 255; cr = num >> 16 & 255;
//            return getToolNr(Color.FromArgb(255,cr,cg,cb),mode);
//        }
//        public static int getToolNr(Color mycolor,int mode)
//        {
//            int i,start=1;
//            Array.Sort<toolProp>(svgToolTable, (x, y) => x.toolnr.CompareTo(y.toolnr));    // sort by tool nr
//            if (useException) start=0;  // first element is exception
//            for (i = start; i < svgToolIndex; i++)
//            {
//                if (mycolor == svgToolTable[i].color)         // direct hit
//                {
//                    tmpIndex = i;
//                    return svgToolTable[i].toolnr;
//                }
//                else if (mode == 0)
//                    svgToolTable[i].diff = ColorDiff(mycolor, svgToolTable[i].color);
//                else if (mode == 1)
//                    svgToolTable[i].diff = getHueDistance(mycolor.GetHue(), svgToolTable[i].color.GetHue());
//                else
//                    svgToolTable[i].diff = Math.Abs(ColorNum(svgToolTable[i].color) - ColorNum(mycolor)) +
//                                              getHueDistance(svgToolTable[i].color.GetHue(), mycolor.GetHue());
//            }
//            Array.Sort<toolProp>(svgToolTable, (x, y) => x.diff.CompareTo(y.diff));    // sort by color difference
//            tmpIndex = 0;
//            return svgToolTable[0].toolnr; ;   // return tool nr of nearest color
//        }

//        public static void countPixel()
//        { svgToolTable[tmpIndex].pixelCount++; }

//        public static int pixelCount()
//        { return svgToolTable[tmpIndex].pixelCount; }

//        public static Color getColor()
//        { return svgToolTable[tmpIndex].color; }

//        public static void setUse(bool use)
//        { svgToolTable[tmpIndex].use = use; }

//        public static String getName()
//        { return svgToolTable[tmpIndex].name; }

//        // http://stackoverflow.com/questions/27374550/how-to-compare-color-object-and-get-closest-color-in-an-color
//        // distance between two hues:
//        private static float getHueDistance(float hue1, float hue2)
//        { float d = Math.Abs(hue1 - hue2); return d > 180 ? 360 - d : d; }
//        // color brightness as perceived:
//        private static float getBrightness(Color c)
//        { return (c.R * 0.299f + c.G * 0.587f + c.B * 0.114f) / 256f; }
//        //  weighed only by saturation and brightness 
//        private static float ColorNum(Color c)
//        { return c.GetSaturation() * 5 + getBrightness(c) * 4; }
//        // distance in RGB space
//        private static int ColorDiff(Color c1, Color c2) 
//              { return  (int ) Math.Sqrt((c1.R - c2.R) * (c1.R - c2.R) 
//                                       + (c1.G - c2.G) * (c1.G - c2.G)
//                                       + (c1.B - c2.B) * (c1.B - c2.B)); }
//    }

//}
