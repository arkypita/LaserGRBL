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
///*  GCodeFromDXF.cs a static class to convert SVG data into G-Code 
// *  Many thanks to https://github.com/mkernel/DXFLib
// *  
// *  Spline conversion is faulty if more than 4 point are given
// *  Not implemented by me up to now: 
// *      Text, Image
// *      Transform: rotation, scaling
//*/

//using System;
//using System.Text;
//using System.Collections;
//using System.Windows.Forms;
//using System.Windows.Media;
//using System.IO;
//using DXFLib;
//using System.Globalization;

//namespace GRBL_Plotter //DXFImporter
//{
//    class GCodeFromDXF
//    {
//        private static int svgToolMax = 100;            // max amount of tools
//        private static StringBuilder[] gcodeString = new StringBuilder[svgToolMax];
//        private static int gcodeStringIndex = 0;
//        private static StringBuilder finalString = new StringBuilder();
//        private static bool gcodeUseSpindle = false; // Switch on/off spindle for Pen down/up (M3/M5)
//        private static bool gcodeReduce = false;        // if true remove G1 commands if distance is < limit
//        private static double gcodeReduceVal = .1;        // limit when to remove G1 commands

//        private static bool gcodeZIncEnable = false;
//        private static double gcodeZIncrement = 2;

//        private static bool dxfPauseElement = true;     // if true insert GCode pause M0 before each element
//        private static bool dxfPausePenDown = true;     // if true insert pause M0 before pen down
//        private static bool dxfComments = true;         // if true insert additional comments into GCode
//        private static bool importUnitmm = true;        // convert units if needed

//        private static bool gcodeToolChange = false;          // Apply tool exchange command

//        private static ArrayList drawingList;
//        private static ArrayList objectIdentifier;

//        private static int dxfBezierAccuracy = 6;       // applied line segments at bezier curves
//        private static int dxfColor = -1;

//        /// <summary>
//        /// Entrypoint for conversion: apply file-path 
//        /// </summary>
//        /// <param name="file">String keeping file-name or URL</param>
//        /// <returns>String with GCode of imported data</returns>
//        public static string convertFromText(string text)
//        {
//            byte[] byteArray = Encoding.UTF8.GetBytes(text);
//            MemoryStream stream = new MemoryStream(byteArray);
//            loadDXF(stream);
//            return convertDXF("from Clipboard");                  
//        }
//        public static string ConvertFromFile(string file)
//        {   if (file == "")
//            { MessageBox.Show("Empty file name"); return ""; }

//            if (file.Substring(0, 4) == "http")
//            {
//                string content = "";
//                using (var wc = new System.Net.WebClient())
//                { try { content = wc.DownloadString(file); }
//                    catch { MessageBox.Show("Could not load content from " + file); return ""; }
//                }
//                int pos = content.IndexOf("dxfrw");
//                if ((content != "") && (pos >= 0) && (pos < 8))
//                { try
//                    {
//                        byte[] byteArray = Encoding.UTF8.GetBytes(content);
//                        MemoryStream stream = new MemoryStream(byteArray);
//                        loadDXF(stream);
//                    }
//                    catch (Exception e)
//                    { MessageBox.Show("Error '" + e.ToString() + "' in DXF file " + file); }
//                }
//                else
//                    MessageBox.Show("This is probably not a DXF document.\r\nFirst line: " + content.Substring(0, 50));
//            }
//            else
//            {
//                if (File.Exists(file))
//                {
//                    try
//                    {   loadDXF(file);
//                    }
//                    catch (Exception e)
//                    { MessageBox.Show("Error '" + e.ToString() + "' in DXF file " + file); return ""; }
//                }
//                else { MessageBox.Show("File does not exist: " + file); return ""; }
//            }
//            return convertDXF(file);
//        }

//        private static string convertDXF(string txt)
//        {
//            drawingList = new ArrayList();
//            objectIdentifier = new ArrayList();
//            gcodeStringIndex = 0;
//            gcodeString[gcodeStringIndex] = new StringBuilder();
//            gcodeString[gcodeStringIndex].Clear();
//            importUnitmm = Properties.Settings.Default.importUnitmm;

//            dxfBezierAccuracy = (int)Properties.Settings.Default.importSVGBezier;
//            gcodeReduce = Properties.Settings.Default.importSVGReduce;
//            gcodeReduceVal = (double)Properties.Settings.Default.importSVGReduceLimit;

//            gcodeZIncEnable = Properties.Settings.Default.importGCZIncEnable;
//            gcodeZIncrement = (double)Properties.Settings.Default.importGCZIncrement;

//            dxfPauseElement = Properties.Settings.Default.importSVGPauseElement;
//            dxfPausePenDown = Properties.Settings.Default.importSVGPausePenDown;
//            dxfComments = Properties.Settings.Default.importSVGAddComments;

//            gcodeToolChange = Properties.Settings.Default.importGCTool;
//            dxfColor = -1;
//            int dxfToolIndex = toolTable.init();

//            gcode.setup();  // initialize GCode creation (get stored settings for export)
//            if (Properties.Settings.Default.importGCTool && !Properties.Settings.Default.importDXFToolIndex)
//            {
//                toolProp tmpTool = toolTable.getToolProperties((int)Properties.Settings.Default.importGCToolDefNr);
//                gcode.Tool(gcodeString[gcodeStringIndex], tmpTool.toolnr, tmpTool.name);
//            }

//            GetVectorDXF();

//            string header = gcode.GetHeader("DXF import",txt);
//            string footer = gcode.GetFooter();
//            gcodeUseSpindle = Properties.Settings.Default.importGCZEnable;

//            finalString.Clear();

//            if (gcodeUseSpindle) gcode.SpindleOn(finalString, "Start spindle - Option Z-Axis");
//            finalString.Append(gcodeString[0]);     //.Replace(',', '.')
//            if (gcodeUseSpindle) gcode.SpindleOff(finalString, "Stop spindle - Option Z-Axis");

//            string output = "";
//            if (Properties.Settings.Default.importSVGRepeatEnable)
//            {
//                for (int i = 0; i < Properties.Settings.Default.importSVGRepeat; i++)
//                    output += finalString.ToString().Replace(',', '.');

//                return header + output + footer;
//            }
//            else
//                return header + finalString.ToString().Replace(',', '.') + footer;
//        }


//        /// <summary>
//        /// Load and parse DXF code
//        /// </summary>
//        /// <param name="filename">String keeping file-name</param>
//        /// <returns></returns>
//        private static DXFDocument doc;
//        private static void loadDXF(string filename)
//        {   doc = new DXFDocument();
//            doc.Load(filename);
//        //    GetVectorDXF();
//        }
//        private static void loadDXF(Stream content)
//        {   doc = new DXFDocument();
//            doc.Load(content);
//        //    GetVectorDXF();
//        }
//        private static void GetVectorDXF()
//        {
//            gcodePenUp("DXF Start");
//            lastGCX = -1; lastGCY = -1; lastSetGCX = -1; lastSetGCY = -1;
////            MessageBox.Show("unit "+ doc.Header.MeasurementUnits.Value.ToString());

//            foreach (DXFEntity dxfEntity in doc.Entities)
//            {

//                if (dxfEntity.GetType() == typeof(DXFInsert))
//                {
//                    DXFInsert ins = (DXFInsert)dxfEntity;
//                    double ins_x = (double)ins.InsertionPoint.X;
//                    double ins_y = (double)ins.InsertionPoint.Y;

//                    foreach (DXFBlock block in doc.Blocks)
//                    {
//                        if (block.BlockName.ToString() == ins.BlockName)
//                        {   if (dxfComments)
//                            {
//                                gcode.Comment(gcodeString[gcodeStringIndex], "Color: " + block.ColorNumber.ToString());
//                                gcode.Comment(gcodeString[gcodeStringIndex], "Block: " + block.BlockName.ToString() + " at " + ins_x.ToString() + " " + ins_y.ToString());
//                            }
//                            foreach (DXFEntity blockEntity in block.Children)
//                            {
//                                processEntities(blockEntity, ins_x, ins_y);
//                            }
//                            if (dxfComments)
//                                gcode.Comment(gcodeString[gcodeStringIndex], "Block: " + block.BlockName.ToString() + " end");
//                        }
//                    }
//                }
//                else
//                    processEntities(dxfEntity);
//            }
//            if (askPenUp)   // retrieve missing pen up
//            { gcode.PenUp(gcodeString[gcodeStringIndex]); askPenUp = false; }
//        }

//        /// <summary>
//        /// Parse DXF entities
//        /// </summary>
//        /// <param name="entity">Entity to convert</param>
//        /// <param name="offsetX">Offset to apply if called by block insertion</param>
//        /// <returns></returns>                       
//        private static System.Windows.Point[] points;
//        private static bool isReduceOk=false;

//        /// <summary>
//        /// Process entities
//        /// </summary>
//        private static void processEntities(DXFEntity entity, double offsetX=0, double offsetY=0)
//        {
//            int index = 0;
//            double x, y, x2 = 0, y2 = 0, bulge;
//            if (dxfComments)
//            {   gcode.Comment(gcodeString[gcodeStringIndex], "Entity: " + entity.GetType().ToString());
//                gcode.Comment(gcodeString[gcodeStringIndex], "Color:  " + entity.ColorNumber.ToString());
//            }

//            if (dxfColor != entity.ColorNumber)
//            {
//                if (Properties.Settings.Default.importGCTool && Properties.Settings.Default.importDXFToolIndex)
//                {
//                    gcode.Tool(gcodeString[gcodeStringIndex], entity.ColorNumber, entity.ColorNumber.ToString());
//                }
//            }
//            dxfColor = entity.ColorNumber;

//            if (entity.GetType() == typeof(DXFPointEntity))
//            {
//                DXFPointEntity point = (DXFPointEntity)entity;
//                x = (float)point.Location.X + (float)offsetX;
//                y = (float)point.Location.Y + (float)offsetY;
//                gcodeStartPath(x, y, "Start Point");
//                gcodeStopPath();
//            }

//            #region DXFLWPolyline
//            else if (entity.GetType() == typeof(DXFLWPolyLine))
//            {
//                DXFLWPolyLine lp = (DXFLWPolyLine)entity;
//                index = 0; bulge = 0;
//                DXFLWPolyLine.Element coordinate;
//                bool roundcorner = false;
//                for (int i = 0; i < lp.VertexCount; i++)
//                {
//                    coordinate = lp.Elements[i];
//                    bulge = coordinate.Bulge;
//                    x = (double)coordinate.Vertex.X + (double)offsetX;
//                    y = (double)coordinate.Vertex.Y + (double)offsetY;
//                    if (i == 0)
//                    {
//                        gcodeStartPath(x, y, "Start LWPolyLine");
//                        isReduceOk = true;
//                    }
//                    else
//                    {
//                        if (!roundcorner)
//                            gcodeMoveTo(x, y, "");
//                        if (bulge != 0)
//                        {
//                            if (i < (lp.VertexCount - 1))
//                                AddRoundCorner(lp.Elements[i], lp.Elements[i + 1]);
//                            else
//                                AddRoundCorner(lp.Elements[i], lp.Elements[0]);
//                            roundcorner = true;
//                        }
//                        else
//                            roundcorner = false;
//                    }
//                    x2 = x; y2 = y;
//                }
//                if (lp.Flags > 0)
//                    gcodeMoveTo((float)lp.Elements[0].Vertex.X, (float)lp.Elements[0].Vertex.Y, "End LWPolyLine");
//                gcodeStopPath();
//            }
//            #endregion
//            #region DXFPolyline
//            else if (entity.GetType() == typeof(DXFPolyLine))
//            {
//                DXFPolyLine lp = (DXFPolyLine)entity;
//                index = 0;
//                foreach (DXFVertex coordinate in lp.Children)
//                {
//                    if (coordinate.GetType() == typeof(DXFVertex))
//                        if (coordinate.Location.X != null && coordinate.Location.Y != null)
//                        {
//                            x = (float)coordinate.Location.X + (float)offsetX;
//                            y = (float)coordinate.Location.Y + (float)offsetY;
//                            if (index == 0)
//                            {
//                                gcodeStartPath(x, y, "Start PolyLine");
//                            }
//                            else
//                                gcodeMoveTo(x, y, "");
//                            index++;
//                        }
//                }
//                gcodeStopPath();
//            }
//            #endregion
//            #region DXFLine
//            else if (entity.GetType() == typeof(DXFLine))
//            {
//                DXFLine line = (DXFLine)entity;
//                x = (float)line.Start.X + (float)offsetX;
//                y = (float)line.Start.Y + (float)offsetY;
//                x2 = (float)line.End.X + (float)offsetX;
//                y2 = (float)line.End.Y + (float)offsetY;
//                isReduceOk = false;
//                gcodeStartPath(x, y, "Start Line");
//                gcodeMoveTo(x2, y2, "");
//                gcodeStopPath();
//            }
//            #endregion
//            #region DXFSpline
//            else if (entity.GetType() == typeof(DXFSpline))
//            {
//                DXFSpline spline = (DXFSpline)entity;
//                index = 0;
//                double cx0, cy0, cx1, cy1, cx2, cy2, cx3, cy3, cxMirror, cyMirror, lastX, lastY;
//                lastX = (double)spline.ControlPoints[0].X + offsetX;
//                lastY = (double)spline.ControlPoints[0].Y + offsetY;
//                string cmt = "Start Spline " + spline.KnotValues.Count.ToString() + " " + spline.ControlPoints.Count.ToString() + " " + spline.FitPoints.Count.ToString();
//                gcodeStartPath(lastX, lastY, cmt);
//                isReduceOk = true;

//                for (int rep = 0; rep < spline.ControlPointCount; rep += 4)
//                {
//                    cx0 = (double)spline.ControlPoints[rep].X + offsetX; cy0 = (double)spline.ControlPoints[rep].Y + offsetY;
//                    cx1 = (double)spline.ControlPoints[rep + 1].X + offsetX; cy1 = (double)spline.ControlPoints[rep + 1].Y + offsetY;
//                    cx2 = (double)spline.ControlPoints[rep + 2].X + offsetX; cy2 = (double)spline.ControlPoints[rep + 2].Y + offsetY;
//                    cx3 = (double)spline.ControlPoints[rep + 3].X + offsetX; cy3 = (double)spline.ControlPoints[rep + 3].Y + offsetY;
//                    points = new System.Windows.Point[4];
//                    points[0] = new System.Windows.Point(cx0, cy0); //(qpx1, qpy1);
//                    points[1] = new System.Windows.Point(cx1, cy1); //(qpx1, qpy1);
//                    points[2] = new System.Windows.Point(cx2, cy2); //(qpx2, qpy2);
//                    points[3] = new System.Windows.Point(cx3, cy3);
//                    cxMirror = cx3 - (cx2 - cx3); cyMirror = cy3 - (cy2 - cy3);
//                    lastX = cx3; lastY = cy3;
//                    var b = GetBezierApproximation(points, dxfBezierAccuracy);
//                    for (int i = 1; i < b.Points.Count; i++)
//                        gcodeMoveTo((float)b.Points[i].X, (float)b.Points[i].Y, "");
//                }
//                gcodeStopPath();
//            }
//            #endregion
//            #region DXFCircle
//            else if (entity.GetType() == typeof(DXFCircle))
//            {
//                DXFCircle circle = (DXFCircle)entity;
//                x = (float)circle.Center.X + (float)offsetX;
//                y = (float)circle.Center.Y + (float)offsetY;
//                gcodeStartPath(x + circle.Radius, y, "Start Circle");
//                gcode.Arc(gcodeString[gcodeStringIndex], 2, (float)x + (float)circle.Radius, (float)y, -(float)circle.Radius, 0, "");
//                gcodeStopPath();
//            }
//            #endregion

//            else if (entity.GetType() == typeof(DXFEllipse))
//            {
//                DXFEllipse circle = (DXFEllipse)entity;
//                gcode.Comment(gcodeString[gcodeStringIndex], "Ellipse: " + circle.ColorNumber.ToString());
//            }
//            #region DXFArc
//            else if (entity.GetType() == typeof(DXFArc))
//            {
//                DXFArc arc = (DXFArc)entity;
                
//                double X = (double)arc.Center.X + offsetX;
//                double Y = (double)arc.Center.Y + offsetY;
//                double R = arc.Radius;
//                double startAngle = arc.StartAngle;
//                double endAngle = arc.EndAngle;
//                if (startAngle > endAngle) endAngle += 360;
//                double stepwidth = (double)Properties.Settings.Default.importGCSegment;
//                float StepAngle = (float)(Math.Asin(stepwidth / R) * 180 / Math.PI);// Settings.Default.page11arcMaxLengLine);
//                double currAngle = startAngle;
//                index = 0;
//                while (currAngle < endAngle)
//                {
//                    double angle = currAngle * Math.PI / 180;
//                    double rx = (double)(X + R * Math.Cos(angle));
//                    double ry = (double)(Y + R * Math.Sin(angle));
//                    if (index == 0)
//                    {
//                        gcodeStartPath(rx, ry, "Start Arc");
//                        isReduceOk = true;
//                    }
//                    else
//                        gcodeMoveTo(rx, ry, "");
//                    currAngle += StepAngle;
//                    if (currAngle > endAngle)
//                    {
//                        double angle2 = endAngle * Math.PI / 180;
//                        double rx2 = (double)(X + R * Math.Cos(angle2));
//                        double ry2 = (double)(Y + R * Math.Sin(angle2));

//                        if (index == 0)
//                        {
//                            gcodeStartPath(rx2, ry2, "Start Arc");
//                        }
//                        else
//                            gcodeMoveTo(rx2, ry2, "");
//                    }
//                    index++;
//                }
//                gcodeStopPath();
//            }
//            #endregion
//            #region DXFMText
//            else if (entity.GetType() == typeof(DXFMText))
//            {   // https://www.autodesk.com/techpubs/autocad/acad2000/dxf/mtext_dxf_06.htm
//                DXFMText txt = (DXFMText)entity;
//                xyPoint origin = new xyPoint(0,0);
//                GCodeFromFont.reset();

//                foreach (var entry in txt.Entries)
//                {   if (entry.GroupCode == 1) { GCodeFromFont.gcText = entry.Value.ToString(); }
//                    else if (entry.GroupCode == 40) { GCodeFromFont.gcHeight = double.Parse(entry.Value, CultureInfo.InvariantCulture.NumberFormat); } //Convert.ToDouble(entry.Value); }// gcode.Comment(gcodeString[gcodeStringIndex], "Height "+entry.Value); }
//                    else if (entry.GroupCode == 41) { GCodeFromFont.gcWidth = double.Parse(entry.Value, CultureInfo.InvariantCulture.NumberFormat); } //Convert.ToDouble(entry.Value); }// gcode.Comment(gcodeString[gcodeStringIndex], "Width "+entry.Value); }
//                    else if (entry.GroupCode == 71) { GCodeFromFont.gcAttachPoint = Convert.ToInt16(entry.Value); }// gcode.Comment(gcodeString[gcodeStringIndex], "Origin " + entry.Value); }
//                    else if (entry.GroupCode == 10) { GCodeFromFont.gcOffX = double.Parse(entry.Value, CultureInfo.InvariantCulture.NumberFormat); } //Convert.ToDouble(entry.Value); }
//                    else if (entry.GroupCode == 20) { GCodeFromFont.gcOffY = double.Parse(entry.Value, CultureInfo.InvariantCulture.NumberFormat); } //Convert.ToDouble(entry.Value); }
//                    else if (entry.GroupCode == 50) { GCodeFromFont.gcAngle = double.Parse(entry.Value, CultureInfo.InvariantCulture.NumberFormat); } //Convert.ToDouble(entry.Value); }// gcode.Comment(gcodeString[gcodeStringIndex], "Angle " + entry.Value); }
//                    else if (entry.GroupCode == 44) { GCodeFromFont.gcSpacing = double.Parse(entry.Value, CultureInfo.InvariantCulture.NumberFormat); } //Convert.ToDouble(entry.Value); }
//                    else if (entry.GroupCode == 7) { GCodeFromFont.gcFontName = entry.Value.ToString(); }
//                }
//                GCodeFromFont.getCode(gcodeString[gcodeStringIndex]);
//            }
//            #endregion
//            else
//                gcode.Comment(gcodeString[gcodeStringIndex], "Unknown: " + entity.GetType().ToString());
//        }

//        private static PolyLineSegment GetBezierApproximation(System.Windows.Point[] controlPoints, int outputSegmentCount)
//        {
//            System.Windows.Point[] points = new System.Windows.Point[outputSegmentCount + 1];
//            for (int i = 0; i <= outputSegmentCount; i++)
//            {
//                double t = (double)i / outputSegmentCount;
//                points[i] = GetBezierPoint(t, controlPoints, 0, controlPoints.Length);
//            }
//            return new PolyLineSegment(points, true);
//        }
//        private static System.Windows.Point GetBezierPoint(double t, System.Windows.Point[] controlPoints, int index, int count)
//        {
//            if (count == 1)
//                return controlPoints[index];
//            var P0 = GetBezierPoint(t, controlPoints, index, count - 1);
//            var P1 = GetBezierPoint(t, controlPoints, index + 1, count - 1);
//            double x = (1 - t) * P0.X + t * P1.X;
//            return new System.Windows.Point(x, (1 - t) * P0.Y + t * P1.Y);
//        }

//        /// <summary>
//        /// Calculate round corner of DXFLWPolyLine if Bulge is given
//        /// </summary>
//        /// <param name="var1">First vertex coord</param>
//        /// <param name="var2">Second vertex</param>
//        /// <returns></returns>
//        private static void AddRoundCorner(DXFLWPolyLine.Element var1, DXFLWPolyLine.Element var2)
//        {
//            double bulge = var1.Bulge;
//            double p1x = (double)var1.Vertex.X;
//            double p1y = (double)var1.Vertex.Y;
//            double p2x = (double)var2.Vertex.X;
//            double p2y = (double)var2.Vertex.Y;

//            //Definition of bulge, from Autodesk DXF fileformat specs
//            double angle = Math.Abs(Math.Atan(bulge) * 4);
//            bool girou = false;

//            //For my method, this angle should always be less than 180. 
//            if (angle > Math.PI)
//            {   angle = Math.PI * 2 - angle;
//                girou = true;
//            }

//            //Distance between the two vertexes, the angle between Center-P1 and P1-P2 and the arc radius
//            double distance = Math.Sqrt(Math.Pow(p1x - p2x, 2) + Math.Pow(p1y - p2y, 2));
//            double alpha = (Math.PI - angle) / 2;
//            double ratio = distance * Math.Sin(alpha) / Math.Sin(angle);
//            if (angle == Math.PI)
//                ratio = distance / 2;

//            double xc, yc, direction;

//            //Used to invert the signal of the calculations below
//            if (bulge < 0)
//                direction = 1;
//            else
//                direction = -1;

//            //calculate the arc center
//            double part = Math.Sqrt(Math.Pow(2 * ratio / distance, 2) - 1);
//            if (!girou)
//            {   xc = ((p1x + p2x) / 2) - direction * ((p1y - p2y) / 2) * part;
//                yc = ((p1y + p2y) / 2) + direction * ((p1x - p2x) / 2) * part;
//            }
//            else
//            {   xc = ((p1x + p2x) / 2) + direction * ((p1y - p2y) / 2) * part;
//                yc = ((p1y + p2y) / 2) - direction * ((p1x - p2x) / 2) * part;
//            }

//            string cmt = "";
//            if (dxfComments) { cmt = "Bulge " + bulge.ToString(); }
//            if (bulge > 0)
//                gcode.Arc(gcodeString[gcodeStringIndex], 3, (float)p2x, (float)p2y, (float)(xc-p1x), (float)(yc-p1y), cmt);
//            else
//                gcode.Arc(gcodeString[gcodeStringIndex], 2, (float)p2x, (float)p2y, (float)(xc-p1x), (float)(yc-p1y), cmt);
//        }

//        /// <summary>
//        /// Transform XY coordinate using matrix and scale  
//        /// </summary>
//        /// <param name="pointStart">coordinate to transform</param>
//        /// <returns>transformed coordinate</returns>
//        private static System.Windows.Point translateXY(float x, float y)
//        {
//            System.Windows.Point coord = new System.Windows.Point(x, y);
//            return translateXY(coord);
//        }
//        private static System.Windows.Point translateXY(System.Windows.Point pointStart)
//        {
//            return pointStart;// pointResult;
//        }
//        /// <summary>
//        /// Transform I,J coordinate using matrix and scale  
//        /// </summary>
//        /// <param name="pointStart">coordinate to transform</param>
//        /// <returns>transformed coordinate</returns>
//        private static System.Windows.Point translateIJ(float i, float j)
//        {
//            System.Windows.Point coord = new System.Windows.Point(i, j);
//            return translateIJ(coord);
//        }
//        private static System.Windows.Point translateIJ(System.Windows.Point pointStart)
//        {
//            System.Windows.Point pointResult = pointStart;
//            double tmp_i = pointStart.X, tmp_j = pointStart.Y;
//            return pointResult;
//        }

//        /// <summary>
//        /// Insert G0, Pen down gcode command
//        /// </summary>
//        private static void gcodeStartPath(double x, double y, string cmt = "")
//        {   gcodeStartPath((float) x, (float) y,cmt); }
//        private static void gcodeStartPath(float x, float y, string cmt="")
//        {
//            if (!dxfComments)
//                cmt = "";
//            System.Windows.Point coord = translateXY(x, y);
//            if (((float)lastGCX == coord.X) && ((float)lastGCY == coord.Y))    // no change in position, no need for pen-up -down
//            {   askPenUp = false;
//                gcode.Comment(gcodeString[gcodeStringIndex], cmt);
//            }
//            else
//            {   if (askPenUp)   // retrieve missing pen up
//                {   gcode.PenUp(gcodeString[gcodeStringIndex], cmt);
//                    isPenDown = false;
//                    askPenUp = false;
//                }
//                lastGCX = coord.X; lastGCY = coord.Y;
//                lastSetGCX = coord.X; lastSetGCY = coord.Y;
//                gcode.MoveToRapid(gcodeString[gcodeStringIndex], coord, cmt);
//            }
//            if (!isPenDown)
//            {
//                if (dxfPausePenDown) { gcode.Pause(gcodeString[gcodeStringIndex], cmt); }
//                gcode.PenDown(gcodeString[gcodeStringIndex]);
//                isPenDown = true;
//            }
//            isReduceOk = false;
//        }
//        private static bool askPenUp = false;
//        private static bool isPenDown = false;
//        private static void gcodeStopPath(string cmt="")
//        {
//            if (!dxfComments)
//                cmt = "";
//            if (gcodeReduce)
//            { if ((lastSetGCX != lastGCX) || (lastSetGCY != lastGCY))
//                {
//                    gcode.MoveTo(gcodeString[gcodeStringIndex], new System.Windows.Point(lastGCX, lastGCY), cmt);
//                    lastSetGCX = lastGCX; lastSetGCY = lastGCY;
//                }
//            }
//            if (isPenDown)
//            {   askPenUp = true; }//
//        }

//        /// <summary>
//        /// Insert G1 gcode command
//        /// </summary>
//        private static void gcodeMoveTo(double x, double y, string cmt)
//        {
//            System.Windows.Point coord = new System.Windows.Point(x, y);
//            gcodeMoveTo(coord, cmt);
//        }
//        /// <summary>
//        /// Insert G1 gcode command
//        /// </summary>
//        private static void gcodeMoveTo(float x, float y, string cmt)
//        {
//            System.Windows.Point coord = new System.Windows.Point(x, y);
//            gcodeMoveTo(coord, cmt);
//        }

//        private static bool rejectPoint = false;
//        private static double lastGCX =-1, lastGCY=-1, lastSetGCX = -1, lastSetGCY = -1, distance;
//        /// <summary>
//        /// Insert G1 gcode command
//        /// </summary>
//        private static void gcodeMoveTo(System.Windows.Point orig, string cmt)
//        {
//            if (!dxfComments)
//                cmt = "";
//            System.Windows.Point coord = translateXY(orig);
//            rejectPoint = false;
//            if (gcodeReduce && isReduceOk)
//            {   distance = Math.Sqrt(((coord.X - lastSetGCX)* (coord.X - lastSetGCX)) + ((coord.Y - lastSetGCY) * (coord.Y - lastSetGCY)));
//                if (distance < gcodeReduceVal)      // discard actual G1 movement
//                { rejectPoint = true; }
//                else
//                { lastSetGCX = coord.X; lastSetGCY = coord.Y; }
//            }
//            if (!gcodeReduce || !rejectPoint)       // write GCode
//            {
//                gcode.MoveTo(gcodeString[gcodeStringIndex], coord, cmt);
//                lastSetGCX = coord.X; lastSetGCY = coord.Y;
//            }
//            lastGCX = coord.X; lastGCY = coord.Y;
//        }


//        /// <summary>
//        /// Insert G2/G3 gcode command
//        /// </summary>
//        private static void gcodeArcToCCW(float x, float y, float i, float j, string cmt)
//        {
//            System.Windows.Point coordxy = translateXY(x, y);
//            System.Windows.Point coordij = translateIJ(i, j);
//            if (gcodeReduce && isReduceOk)          // restore last skipped point for accurat G2/G3 use
//            {
//                if ((lastSetGCX != lastGCX) || (lastSetGCY != lastGCY))
//                    gcode.MoveTo(gcodeString[gcodeStringIndex], new System.Windows.Point(lastGCX, lastGCY), cmt);
//            }
//            gcode.Arc(gcodeString[gcodeStringIndex], 3, coordxy, coordij, cmt);
//        }

//        /// <summary>
//        /// Insert Pen-up gcode command
//        /// </summary>
//        private static void gcodePenUp(string cmt="")
//        {
//            gcode.PenUp(gcodeString[gcodeStringIndex], cmt);
//            isPenDown = false;
//        }
//    }
//}
