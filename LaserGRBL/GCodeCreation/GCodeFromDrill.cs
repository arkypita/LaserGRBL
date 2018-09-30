///*  GRBL-Plotter. Another GCode sender for GRBL.
//    This file is part of the GRBL-Plotter application.
   
//    Copyright (C) 2018 Sven Hasemann contact: svenhb@web.de

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
//using System.IO;
//using System.Text;
//using System.Windows.Forms;

//namespace GRBL_Plotter
//{
//    class GCodeFromDrill
//    {
//        private static StringBuilder finalString = new StringBuilder();
//        private static StringBuilder gcodeString = new StringBuilder();
//        private static bool gcodeUseSpindle = false;            // Switch on/off spindle for Pen down/up (M3/M5)
//        private static bool gcodeToolChange = false;            // Apply tool exchange command
//        private static bool importComments = true;              // if true insert additional comments into GCode
//        private static bool importUnitmm = true;                // convert units if needed

//        private static string   infoDate = "unknown";
//        private static bool     infoModeIsAbsolute = true;
//        private static string   infoUnits = "Inch";
//        private static double   infoFraction = 0.00001;         // default 1/100000
//        private static string[] infoDrill = new string[20];

//        /// <summary>
//        /// Entrypoint for conversion: apply file-path 
//        /// </summary>
//        /// <param name="file">String keeping file-name</param>
//        /// <returns>String with GCode of imported data</returns>
//        public static string ConvertFile(string file)
//        {
//            if (file == "")
//            {   MessageBox.Show("Empty file name");
//                return "";
//            }

//            gcode.setup();      // initialize GCode creation (get stored settings for export)
//            gcodeToolChange = Properties.Settings.Default.importGCTool;
//            importComments = Properties.Settings.Default.importSVGAddComments;
//            importUnitmm  = Properties.Settings.Default.importUnitmm;

//            for (int i = 0; i < 20; i++)
//                infoDrill[i] = "";

//            if (file.Substring(0, 4) == "http")
//            {
//                MessageBox.Show("Load via http is not supported up to now");
//            }
//            else
//            {
//                string file_dri="", file_drd="";
//                if (file.Substring(file.Length - 3, 3).ToLower() == "dri")      // build up filenames
//                {
//                    file_dri = file;
//                    file_drd = file.Substring(0, file.Length - 3) + "drd";
//                }
//                else if (file.Substring(file.Length - 3, 3).ToLower() == "drd")      // build up filenames
//                {
//                    file_drd = file;
//                    file_dri = file.Substring(0, file.Length - 3) + "dri";
//                }
//                else
//                {   file_drd = file;        // KiCad drl
//                    file_dri = "";
//                }
//                if (File.Exists(file_dri))              
//                {   try
//                    {   string[] drillInformation = File.ReadAllLines(file_dri);     // get drill information
//                        getDrillInfos(drillInformation);
//                    }
//                    catch (Exception e)
//                    {   MessageBox.Show("Error '" + e.ToString() + "' in file " + file_dri); return ""; }
//                }
//                else {  MessageBox.Show("Drill information not found : " + file_dri + "\r\nTry to convert *.drd with default settings"); return ""; }

//                if (File.Exists(file_drd))              
//                {   try
//                    {   string[] drillCoordinates = File.ReadAllLines(file_drd);     // get drill coordinates
//                        convertDrill(drillCoordinates, file_drd);
//                    }
//                    catch (Exception e)
//                    {   MessageBox.Show("Error '" + e.ToString() + "' in file " + file_drd); return ""; }
//                }
//                else {  MessageBox.Show("Drill file does not exist: " + file_drd); return ""; }
//            }

//            string header = gcode.GetHeader("Drill import", file);
//            string footer = gcode.GetFooter();
//            gcodeUseSpindle = Properties.Settings.Default.importGCZEnable;

//            finalString.Clear();

//            if (gcodeUseSpindle) gcode.SpindleOn(finalString, "Start spindle - Option Z-Axis");

//            finalString.Append(gcodeString);     
//            if (gcodeUseSpindle) gcode.SpindleOff(finalString, "Stop spindle - Option Z-Axis");

//            return header + finalString.ToString().Replace(',', '.') + footer;
//        }


//        private static void getDrillInfos(string[] drillInfo)
//        {
//            foreach (string line in drillInfo)
//            {
//                string[] part = line.Split(':');
//                if (part[0].IndexOf("Date") >= 0)       { infoDate = part[1].Trim(); }
//                if (part[0].IndexOf("Data Mode") >= 0)
//                {   infoModeIsAbsolute = (part[1].IndexOf("Absolute") >= 0) ? true : false;
//                }
//                if (part[0].IndexOf("Units") >= 0)
//                {   }
//                if (part[0].IndexOf("T") >= 0)
//                {   }
//            }
//        }

//        private static void convertDrill(string[] drillCode, string info)
//        {
//            gcodeString.Clear();
//            if (importComments)
//            {
//                gcodeString.AppendFormat("( Import Unit    : {0} )\r\n", infoUnits);
//                gcodeString.AppendFormat("( Import Fraction: {0} )\r\n", infoFraction);
//                gcodeString.Append("( Numbers exported to mm )\r\n");
//            }
//            gcode.PenUp(gcodeString, "Drill Start ");
//            bool isHeader = false;
//            foreach (string line in drillCode)
//            {
//                if (line.IndexOf("%") >= 0)
//                {   isHeader = (isHeader)? false:true; }

//                if (isHeader)
//                {   if (importComments)
//                        gcodeString.AppendLine("( " + line + " )");
//                    if ((line.IndexOf("T") >= 0) && (line.IndexOf("C") >= 0))
//                    {
//                        string[] part = line.Split('C');
//                        int tnr = 0;
//                        double dmm = 0;
//                        Int32.TryParse(part[0].Substring(1), out tnr);
//                        Double.TryParse(part[1].Substring(0), out dmm);
//                        dmm = dmm * 25.4;
//                        infoDrill[tnr] = part[1] + " Inch = " + dmm.ToString() + " mm";
//                    }
//                }
//                else
//                {
//                    if (line.IndexOf("T") >= 0)
//                    {
//                        gcodeString.AppendLine(" ");        // add empty line for better view
//                        if (gcodeToolChange)
//                        {
//                            int tnr = 0;
//                            Int32.TryParse(line.Substring(1), out tnr);
//                            gcode.Tool(gcodeString, tnr, infoDrill[tnr]);
//                        }
//                        else
//                        {   gcodeString.AppendLine("( " + line + " tool change not enabled)"); }
//                    }

//                    if ((line.IndexOf("X") >= 0) && (line.IndexOf("Y") >= 0))
//                    {
//                        string[] part = line.Split('Y');
//                        double x = 0;
//                        double y = 0;
//                        double.TryParse(part[0].Substring(1), out x);
//                        double.TryParse(part[1].Substring(0), out y);

//                        x = x * infoFraction;
//                        y = y * infoFraction;
//                        if (importUnitmm)
//                        {   x = x * 25.4;
//                            y = y * 25.4;
//                        }
//                        string cmt = "";
//                        if (importComments)
//                            cmt = line;
//                        gcode.MoveToRapid(gcodeString, (float)x, (float)y, cmt);
//                        gcode.PenDown(gcodeString);
//                        gcode.PenUp(gcodeString);
//                    }
//                }
//            }
//        }
//    }
//}
