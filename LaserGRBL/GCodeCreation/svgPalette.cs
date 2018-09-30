///*  GRBL-Plotter. Another GCode sender for GRBL.
//    This file is part of the GRBL-Plotter application.
   
//    Copyright (C) 2015-2017 Sven Hasemann contact: svenhb@web.de

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

//using System;
//using System.Drawing;
//using System.IO;

//namespace GRBL_Plotter
//{
//    public struct palette
//    {
//        public int toolnr;
//        public System.Drawing.Color clr;
//        public bool use;
//        public int codeSize;
//        public int pixelCount;
//        public double diff;
//        public String name;
//    }

//    public static class svgPalette
//    {
//        private static int svgToolMax = 100;            // max amount of tools
//        private static palette[] svgToolTable = new palette[svgToolMax];   // load color palette into this array
//        private static int svgToolIndex = 0;            // last index
//        private static bool svgToolColor = true;        // if true take tool nr. from nearest pallet entry
//        private static bool svgToolSort = true;         // if true sort objects by tool-nr. (to avoid back and forth pen change)
//        private static string svgPaletteFile = "";          // Path to GIMP plaette to use
//        private static bool useException = false;
//        private static int tmpIndex = 0;

//        public static string getToolName(int index)
//        {   Array.Sort<palette>(svgToolTable, (x, y) => x.toolnr.CompareTo(y.toolnr));
//            if (index < 0) index = 0;
//            if (index >= svgToolIndex - 2) index = svgToolIndex - 2;
//            return svgToolTable[index+1].name;
//        }
//        public static void setToolCodeSize(int index, int size)
//        {
//            Array.Sort<palette>(svgToolTable, (x, y) => x.toolnr.CompareTo(y.toolnr));
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
//        {   Array.Sort<palette>(svgToolTable, (x, y) => x.toolnr.CompareTo(y.toolnr));    // sort by tool nr
//        }
//        public static void sortByCodeSize()
//        {   Array.Sort<palette>(svgToolTable, (x, y) => y.codeSize.CompareTo(x.codeSize));    // sort by size
//        }
//        public static void sortByPixelCount()
//        {   Array.Sort<palette>(svgToolTable, (x, y) => y.pixelCount.CompareTo(x.pixelCount));    // sort by size
//        }

//        // set tool / color table
//        public static int init()    // return number of entries
//        {
//            svgToolColor = Properties.Settings.Default.importSVGToolColor;
//            svgPaletteFile = Properties.Settings.Default.importPalette;
//            svgToolSort = Properties.Settings.Default.importSVGToolSort;
//            //            gcodeToolChange = Properties.Settings.Default.importGCTool;
//            useException=false;
//            Array.Resize(ref svgToolTable, svgToolMax);
//            svgToolIndex = 2;
//            svgToolTable[0].toolnr = -1; 
//            svgToolTable[0].clr = Color.White; 
//            svgToolTable[0].use = false; 
//            svgToolTable[0].diff = int.MaxValue; 
//            svgToolTable[0].name = "except";
//            svgToolTable[0].pixelCount=0;

//            svgToolTable[1].toolnr = 0; svgToolTable[1].pixelCount = 0; svgToolTable[svgToolIndex].use = true; svgToolTable[1].clr = Color.Black; svgToolTable[1].diff = int.MaxValue; svgToolTable[1].name = "black";

//            if (svgToolColor)
//            {
//                if (File.Exists(svgPaletteFile))
//                {
//                    string line, sr, sg, sb, cmt;
//                    int ir, ig, ib;
//                    System.IO.StreamReader file = new System.IO.StreamReader(svgPaletteFile);
//                    while ((line = file.ReadLine()) != null)
//                    {
//                        if (line.Length > 11)
//                        {
//                            sr = line.Substring(0, 3);
//                            sg = line.Substring(4, 3);
//                            sb = line.Substring(8, 3);
//                            cmt = line.Substring(12);
//                            if (Int32.TryParse(sr, out ir) && Int32.TryParse(sg, out ig) && Int32.TryParse(sb, out ib))
//                            {
//                                svgToolTable[svgToolIndex].toolnr = svgToolIndex-1;
//                                svgToolTable[svgToolIndex].clr = System.Drawing.Color.FromArgb(255, ir, ig, ib);
//                                svgToolTable[svgToolIndex].use = false;
//                                svgToolTable[svgToolIndex].diff = int.MaxValue;
//                                svgToolTable[svgToolIndex].name = cmt;
//                                svgToolTable[svgToolIndex].pixelCount = 0;
//                                if (svgToolIndex < svgToolMax-1) svgToolIndex++;
//                            }
//                        }
//                    }
//                    file.Close();
//                    Array.Resize(ref svgToolTable, svgToolIndex);
//                }
//                else
//                {   
//                    //                   gcodeString[gcodeStringIndex].Append("(!!! SVG-Palette file not found - use black,r,g,b !!!)\r\n");
//                    svgToolTable[1].toolnr = 0; svgToolTable[1].pixelCount = 0; svgToolTable[svgToolIndex].use = false; svgToolTable[1].clr = Color.Black;svgToolTable[1].diff = int.MaxValue; svgToolTable[1].name = "black";
//                    svgToolTable[2].toolnr = 1; svgToolTable[2].pixelCount = 0; svgToolTable[svgToolIndex].use = false; svgToolTable[2].clr = Color.Red;  svgToolTable[2].diff = int.MaxValue; svgToolTable[2].name = "red";
//                    svgToolTable[3].toolnr = 2; svgToolTable[3].pixelCount = 0; svgToolTable[svgToolIndex].use = false; svgToolTable[3].clr = Color.Green;svgToolTable[3].diff = int.MaxValue; svgToolTable[3].name = "green";
//                    svgToolTable[4].toolnr = 3; svgToolTable[4].pixelCount = 0; svgToolTable[svgToolIndex].use = false; svgToolTable[4].clr = Color.Blue; svgToolTable[4].diff = int.MaxValue; svgToolTable[4].name = "blue";
//                    svgToolIndex = 5;
//                    Array.Resize(ref svgToolTable, svgToolIndex);
//                }
//            }
//            else
//            {
//                svgToolTable[1].toolnr = 0; svgToolTable[1].pixelCount = 0; svgToolTable[1].use = false; svgToolTable[1].clr = Color.Black; svgToolTable[1].diff = int.MaxValue; svgToolTable[1].name = "black";
//                svgToolIndex = 2;
//                Array.Resize(ref svgToolTable, svgToolIndex);
//            }
//            return svgToolIndex;
//        }

//        // set exception color
//        public static string setExceptionColor(Color mycolor)
//        {   useException=true;
//            Array.Sort<palette>(svgToolTable, (x, y) => x.toolnr.CompareTo(y.toolnr));    // sort by tool nr
//            svgToolTable[0].toolnr = -1; 
//            svgToolTable[0].clr = mycolor; 
//            svgToolTable[0].use = false; 
//            svgToolTable[0].diff = int.MaxValue; 
//            svgToolTable[0].name = "not used";
//            return svgToolTable[0].clr.ToString();
//        }
//        // Clear exception color
//        public static void clrExceptionColor()
//        {   useException=false;}

//        // return tool nr of nearest color
//        public static int getToolNr(String mycolor, int mode)
//        {
//            if (mycolor == "")
//                return 0;
//            int cr, cg, cb;
//            int num = int.Parse(mycolor, System.Globalization.NumberStyles.AllowHexSpecifier);
//            cb = num & 255; cg = num >> 8 & 255; cr = num >> 16 & 255;
//            return getToolNr(Color.FromArgb(255,cr,cg,cb),mode);
//        }
//        public static int getToolNr(Color mycolor,int mode)
//        {
//            int i,start=1;
//            Array.Sort<palette>(svgToolTable, (x, y) => x.toolnr.CompareTo(y.toolnr));    // sort by tool nr
//            if (useException) start=0;  // first element is exception
//            for (i = start; i < svgToolIndex; i++)
//            {
//                if (mycolor == svgToolTable[i].clr)         // direct hit
//                {
//                    tmpIndex = i;
//                    return svgToolTable[i].toolnr;
//                }
//                else if (mode == 0)
//                    svgToolTable[i].diff = ColorDiff(mycolor, svgToolTable[i].clr);
//                else if (mode == 1)
//                    svgToolTable[i].diff = getHueDistance(mycolor.GetHue(), svgToolTable[i].clr.GetHue());
//                else
//                    svgToolTable[i].diff = Math.Abs(ColorNum(svgToolTable[i].clr) - ColorNum(mycolor)) +
//                                              getHueDistance(svgToolTable[i].clr.GetHue(), mycolor.GetHue());
//            }
//            Array.Sort<palette>(svgToolTable, (x, y) => x.diff.CompareTo(y.diff));    // sort by color difference
//            tmpIndex = 0;
//            return svgToolTable[0].toolnr; ;   // return tool nr of nearest color
//        }

//        public static void countPixel()
//        { svgToolTable[tmpIndex].pixelCount++; }

//        public static int pixelCount()
//        { return svgToolTable[tmpIndex].pixelCount; }

//        public static Color getColor()
//        { return svgToolTable[tmpIndex].clr; }

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
