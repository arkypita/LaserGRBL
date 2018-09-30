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
//using System.Text;
//using System.Windows.Forms;
//using System.Globalization;
//using System.Threading;
//using System.Drawing;

//namespace GRBL_Plotter
//{
//    public partial class GCodeFromShape : Form
//    {
//        private string shapegcode = "";
//        public string shapeGCode
//        { get { return shapegcode; } }
//        private static StringBuilder gcodeString = new StringBuilder();

//        public float offsetX = 0, offsetY = 0;
//        public GCodeFromShape()
//        {
//            CultureInfo ci = new CultureInfo(Properties.Settings.Default.language);
//            Thread.CurrentThread.CurrentCulture = ci;
//            Thread.CurrentThread.CurrentUICulture = ci;
//            InitializeComponent();
//        }

//        private void btnCancel_Click(object sender, EventArgs e)
//        {
//            this.Close();
//        }

//        private void btnApply_Click(object sender, EventArgs e)
//        {
//            saveSettings();
//            gcode.setup();                  // load defaults from setup-tab
//            gcode.gcodeXYFeed = (float)nUDToolFeedXY.Value;    // override devault values
//            gcode.gcodeZFeed  = (float)nUDToolFeedZ.Value;    // override devault values
//            gcode.gcodeSpindleSpeed = (float)nUDToolSpindleSpeed.Value;    // override devault values
//            gcode.gcodeZDown = (float)nUDImportGCZDown.Value;

//            gcodeString.Clear();

//            gcode.Tool(gcodeString, tprop.toolnr, tprop.name);
//            if (!Properties.Settings.Default.importGCSpindleToggle) gcode.SpindleOn(gcodeString, "Start");

//            float x, y, rShape,d,dTool,overlap,rTool,zStep;
//            float zStart = 0;
//            x = (float)nUDShapeX.Value;
//            y = (float)nUDShapeY.Value;
//            rShape = (float)nUDShapeR.Value;
//            d = 2 * rShape;
//            dTool = (float)nUDToolDiameter.Value;          // tool diameter;
//            overlap = dTool * (float)nUDToolOverlap.Value/100;  // tool overlap
//            if (rBToolpath1.Checked) { dTool = 0; }        // engrave
//            if (rBToolpath3.Checked) { dTool = -dTool; }      // outside
//            rTool = dTool / 2;                                // tool radius

//            int counter=0,safety = 100;
//            float dx = 0, dy = 0, rDelta=0;
//            if (rBShape1.Checked)               // rectangle
//            {   getOffset(x,y);
//                offsetX -= rTool; offsetY -= rTool;
//                x += dTool;y += dTool;
//                zStep = zStart;
//                while (zStep > (float)nUDImportGCZDown.Value)
//                {
//                    zStep -= (float)nUDToolZStep.Value;
//                    if (zStep < (float)nUDImportGCZDown.Value)
//                        zStep = (float)nUDImportGCZDown.Value;
//                    gcode.PenUp(gcodeString);
//                    if (cBToolpathPocket.Checked)
//                        gcode.MoveToRapid(gcodeString, offsetX + overlap, offsetY + overlap, ""); 
//                    else
//                        gcode.MoveToRapid(gcodeString, offsetX, offsetY, "");
//                    gcode.gcodeZDown = zStep;               // adapt Z-deepth
//                    gcode.PenDown(gcodeString);
//                    if (cBToolpathPocket.Checked)
//                    {
//                        dx = overlap; dy = overlap;
//                        while (((dx < x/2) && (dy < y/2)) && (counter++ < safety))
//                        {
//                            makeRect(offsetX+dx, offsetY+dy, offsetX + x - dx, offsetY + y - dy, 0, false);  // rectangle clockwise
//                            dx += overlap; dy += overlap;
//                            if ((dx < x / 2) && (dy < y / 2))
//                                gcode.MoveTo(gcodeString, offsetX + dx, offsetY + dy, "");
//                        }
//                        gcode.PenUp(gcodeString);
//                        gcode.MoveToRapid(gcodeString, offsetX, offsetY, "");
//                        gcode.PenDown(gcodeString);
//                    }

//                    makeRect(offsetX, offsetY, offsetX + x, offsetY + y, 0, true);  // rectangle clockwise
//                }
//                gcode.PenUp(gcodeString);
//            }
//            if (rBShape2.Checked)           // rectangle with round edge
//            {
//                getOffset(x, y);
//                offsetX -= rTool; offsetY -= rTool;
//                x += dTool; y += dTool;
// //                   gcode.Move(gcodeString, 0, offsetX, offsetY + r, false, "");
//                zStep = zStart;
//                while (zStep > (float)nUDImportGCZDown.Value)
//                {
//                    zStep -= (float)nUDToolZStep.Value;
//                    if (zStep < (float)nUDImportGCZDown.Value)
//                        zStep = (float)nUDImportGCZDown.Value;
//                    gcode.PenUp(gcodeString);
//                    if (cBToolpathPocket.Checked)
//                        gcode.MoveToRapid(gcodeString, offsetX + overlap, offsetY + rShape , "");
//                    else
//                        gcode.MoveToRapid(gcodeString, offsetX, offsetY + rShape, "");
//                    gcode.gcodeZDown = zStep;               // adapt Z-deepth
//                    gcode.PenDown(gcodeString);
//                    if (cBToolpathPocket.Checked)
//                    {
//                        dx = overlap; dy = overlap; rDelta = rShape - overlap;
//                        while (((dx < x / 2) && (dy < y / 2)) && (counter++ < safety))
//                        {
//                            makeRect(offsetX + dx, offsetY + dy, offsetX + x - dx, offsetY + y - dy, rDelta, false);  // rectangle clockwise
//                            dx += overlap; dy += overlap; rDelta -= overlap;
//                            if (dx > x / 2) { dx = x / 2; }
//                            if (dy > x / 2) { dy = y / 2; }
//                            if (rDelta < 0) { rDelta = 0; }
//                            if ((dx < x / 2) && (dy < y / 2))
//                                gcode.MoveTo(gcodeString, offsetX + dx, offsetY + dy + rDelta, "");
//                        }
//                        gcode.PenUp(gcodeString);
//                        gcode.MoveToRapid(gcodeString, offsetX, offsetY + rShape, "");
//                        gcode.PenDown(gcodeString);
//                    }
//                    makeRect(offsetX, offsetY, offsetX + x, offsetY + y, rShape, true);  // rectangle clockwise
//                }
//                gcode.PenUp(gcodeString);
//            }
//            if (rBShape3.Checked)           // circle
//            {
//                getOffset(d, d);
//                offsetX -= rTool; offsetY -= rTool;
//                rShape += rTool;                    // take care of tool diameter if set

//                zStep = zStart;
//                while (zStep > (float)nUDImportGCZDown.Value)
//                {
//                    gcode.PenUp(gcodeString);
//                    if (cBToolpathPocket.Checked)
//                        gcode.MoveToRapid(gcodeString, offsetX + rShape-overlap, offsetY + rShape, "");
//                    else
//                        gcode.MoveToRapid(gcodeString, offsetX, offsetY + rShape, "");
//                    zStep -= (float)nUDToolZStep.Value;
//                    if (zStep < (float)nUDImportGCZDown.Value)
//                        zStep = (float)nUDImportGCZDown.Value;
//                    gcode.gcodeZDown = zStep;               // adapt Z-deepth
//                    gcode.PenDown(gcodeString);
//                    rDelta = overlap;
//                    counter = 0;
//                    if (cBToolpathPocket.Checked)
//                    {   while ((rDelta < rShape) && (counter++ < safety))
//                        {
//                            gcode.Arc(gcodeString, 2, offsetX + rShape - rDelta, offsetY + rShape, rDelta, 0, "");
//                            rDelta += overlap;
//                            if (rDelta < rShape)
//                                gcode.MoveTo(gcodeString, offsetX + rShape - rDelta, offsetY + rShape, "");
//                        }
//                        gcode.MoveTo(gcodeString, offsetX , offsetY + rShape, "");
//                    }
//                    gcode.Arc(gcodeString, 2, offsetX, offsetY + rShape, rShape, 0, "");
//                }
//                gcode.PenUp(gcodeString);
//            }

//            string header = gcode.GetHeader("Simple Shape");
//            string footer = gcode.GetFooter();

//            gcodeString.Replace(',', '.');
//            shapegcode = header + gcodeString.ToString() + footer;
//        }

//        private void makeRect(float x1, float y1, float x2, float y2, float r, bool cw=true )
//        {   // start bottom left
//            if (cw)
//            {
//                gcode.MoveTo(gcodeString, x1, y2 - r, "");          //BL to TL
//                if (r > 0) { gcode.Arc(gcodeString, 2, x1 + r, y2, r, 0,  "");}
//                gcode.MoveTo(gcodeString, x2 - r, y2, "");          // TL to TR
//                if (r > 0) { gcode.Arc(gcodeString, 2, x2, y2 - r, 0, -r,  ""); }
//                gcode.MoveTo(gcodeString, x2, y1 + r, "");          // TR to BR
//                if (r > 0) { gcode.Arc(gcodeString, 2, x2 - r, y1, -r, 0,  ""); }
//                gcode.MoveTo(gcodeString, x1 + r, y1, "");          // BR to BL
//                if (r > 0) { gcode.Arc(gcodeString, 2, x1, y1 + r, 0, r,  ""); }
//            }
//            else
//            {
//                if (r > 0) { gcode.Arc(gcodeString, 3, x1 + r, y1, r, 0,  ""); }
//                gcode.MoveTo(gcodeString, x2 - r, y1, "");          // to BR
//                if (r > 0) { gcode.Arc(gcodeString, 3, x2, y1 + r, 0, r,  ""); }
//                gcode.MoveTo(gcodeString, x2, y2 - r, "");           // to TR
//                if (r > 0) { gcode.Arc(gcodeString, 3, x2 - r, y2, -r, 0,  ""); }
//                gcode.MoveTo(gcodeString, x1 + r, y2, "");           // to TL
//                if (r > 0) { gcode.Arc(gcodeString, 3, x1, y2 - r, 0, -r,  ""); }
//                gcode.MoveTo(gcodeString, x1, y1 + r, "");           // to BL 
//            }

//        }
//        private void getOffset(float x, float y)
//        {   if (rBOrigin1.Checked) { offsetX = 0; offsetY = -y; }
//            if (rBOrigin2.Checked) { offsetX = -x/2; offsetY = -y; }
//            if (rBOrigin3.Checked) { offsetX = -x; offsetY = -y; }
//            if (rBOrigin4.Checked) { offsetX = 0; offsetY = -y/2; }
//            if (rBOrigin5.Checked) { offsetX = -x / 2; offsetY = -y/2; }
//            if (rBOrigin6.Checked) { offsetX = -x; offsetY = -y/2; }
//            if (rBOrigin7.Checked) { offsetX = 0; offsetY = 0; }
//            if (rBOrigin8.Checked) { offsetX = -x / 2; offsetY = 0; }
//            if (rBOrigin9.Checked) { offsetX = -x; offsetY = 0; }
//        }

//        private void ShapeToGCode_Load(object sender, EventArgs e)
//        {
//            Location = Properties.Settings.Default.locationShapeForm;
//            Size desktopSize = System.Windows.Forms.SystemInformation.PrimaryMonitorSize;
//            if ((Location.X < -20) || (Location.X > (desktopSize.Width - 100)) || (Location.Y < -20) || (Location.Y > (desktopSize.Height - 100))) { Location = new Point(0, 0); }

//            nUDToolDiameter.Value = Properties.Settings.Default.toolDiameter;
//            nUDToolZStep.Value = Properties.Settings.Default.toolZStep;
//            nUDToolFeedXY.Value = Properties.Settings.Default.toolFeedXY;
//            nUDToolFeedZ.Value = Properties.Settings.Default.toolFeedZ;
//            nUDToolOverlap.Value = Properties.Settings.Default.toolOverlap;
//            nUDToolSpindleSpeed.Value = Properties.Settings.Default.toolSpindleSpeed;
//            nUDShapeX.Value = Properties.Settings.Default.shapeX;
//            nUDShapeY.Value = Properties.Settings.Default.shapeY;
//            nUDShapeR.Value = Properties.Settings.Default.shapeR;
//            switch (Properties.Settings.Default.shapeType)
//            {
//                case 2:
//                    rBShape2.Checked = true;
//                    break;
//                case 3:
//                    rBShape3.Checked = true;
//                    break;
//                default:
//                    rBShape1.Checked = true;
//                    break;
//            }
//            nUDImportGCZDown.Value = Properties.Settings.Default.importGCZDown;
//            switch (Properties.Settings.Default.toolPath)
//            {
//                case 2:
//                    rBToolpath1.Checked = true;
//                    break;
//                case 3:
//                    rBToolpath3.Checked = true;
//                    break;
//                default:
//                    rBToolpath1.Checked = true;
//                    break;
//            }
//            switch (Properties.Settings.Default.shapeOrigin)
//            {
//                case 1:
//                    rBOrigin1.Checked = true;
//                    break;
//                case 2:
//                    rBOrigin2.Checked = true;
//                    break;
//                case 3:
//                    rBOrigin3.Checked = true;
//                    break;
//                case 4:
//                    rBOrigin4.Checked = true;
//                    break;
//                case 6:
//                    rBOrigin6.Checked = true;
//                    break;
//                case 7:
//                    rBOrigin7.Checked = true;
//                    break;
//                case 8:
//                    rBOrigin8.Checked = true;
//                    break;
//                case 9:
//                    rBOrigin9.Checked = true;
//                    break;
//                default:
//                    rBOrigin5.Checked = true;
//                    break;
//            }
//            int toolCount = toolTable.init();
//            toolProp tmpTool;
//            bool defaultToolFound = false;
//            for (int i = 0; i < toolCount; i++)
//            {
//                tmpTool = toolTable.getToolProperties(i);
//                if (i == tmpTool.toolnr)
//                {
//                    cBTool.Items.Add(i.ToString() + ") " + tmpTool.name);
//                    if (i == Properties.Settings.Default.importGCToolDefNr)
//                    {   cBTool.SelectedIndex = cBTool.Items.Count - 1;
//                        defaultToolFound=true;
//                    }
//                }
//            }
//            if (!defaultToolFound)
//                cBTool.SelectedIndex = 0;
//            tprop = toolTable.getToolProperties(1);
//            enableTool(!cBToolSet.Checked);
//        }

//        private void ShapeToGCode_FormClosing(object sender, FormClosingEventArgs e)
//        {
//            Properties.Settings.Default.locationShapeForm = Location;
//            saveSettings();
//        }
//        private void saveSettings()
//        {
//            Properties.Settings.Default.toolDiameter = nUDToolDiameter.Value;
//            Properties.Settings.Default.toolZStep = nUDToolZStep.Value;
//            Properties.Settings.Default.toolFeedXY = nUDToolFeedXY.Value;
//            Properties.Settings.Default.toolFeedZ = nUDToolFeedZ.Value;
//            Properties.Settings.Default.toolOverlap = nUDToolOverlap.Value;
//            Properties.Settings.Default.toolSpindleSpeed = nUDToolSpindleSpeed.Value;
//            Properties.Settings.Default.shapeX = nUDShapeX.Value;
//            Properties.Settings.Default.shapeY = nUDShapeY.Value;
//            Properties.Settings.Default.shapeR = nUDShapeR.Value;
//            if (rBShape1.Checked) Properties.Settings.Default.shapeType = 1;
//            if (rBShape2.Checked) Properties.Settings.Default.shapeType = 2;
//            if (rBShape3.Checked) Properties.Settings.Default.shapeType = 3;
//            Properties.Settings.Default.importGCZDown = nUDImportGCZDown.Value;
//            if (rBToolpath1.Checked) Properties.Settings.Default.toolPath = 1;
//            if (rBToolpath2.Checked) Properties.Settings.Default.toolPath = 2;
//            if (rBToolpath3.Checked) Properties.Settings.Default.toolPath = 3;
//            if (rBOrigin1.Checked) Properties.Settings.Default.shapeOrigin = 1;
//            if (rBOrigin2.Checked) Properties.Settings.Default.shapeOrigin = 2;
//            if (rBOrigin3.Checked) Properties.Settings.Default.shapeOrigin = 3;
//            if (rBOrigin4.Checked) Properties.Settings.Default.shapeOrigin = 4;
//            if (rBOrigin5.Checked) Properties.Settings.Default.shapeOrigin = 5;
//            if (rBOrigin6.Checked) Properties.Settings.Default.shapeOrigin = 6;
//            if (rBOrigin7.Checked) Properties.Settings.Default.shapeOrigin = 7;
//            if (rBOrigin8.Checked) Properties.Settings.Default.shapeOrigin = 8;
//            if (rBOrigin9.Checked) Properties.Settings.Default.shapeOrigin = 9;
//            Properties.Settings.Default.Save();
//        }

//        private void nUDShapeR_ValueChanged(object sender, EventArgs e)
//        {   if (rBShape2.Checked)
//            {
//                decimal min = Math.Min(nUDShapeX.Value, nUDShapeY.Value);
//                if (nUDShapeR.Value > min / 2)
//                    nUDShapeR.Value = min / 2;
//            }
//        }

//        private void cBToolSet_CheckedChanged(object sender, EventArgs e)
//        {
//            enableTool(!cBToolSet.Checked);
//            cBTool_SelectedIndexChanged(sender, e);
//        }

//        private void enableTool(bool state)
//        {   nUDToolDiameter.Enabled = state;
//            nUDToolFeedXY.Enabled = state;
//            nUDToolZStep.Enabled = state;
//            nUDToolFeedZ.Enabled = state;
//            nUDToolOverlap.Enabled = state;
//            nUDToolSpindleSpeed.Enabled = state;
//        }

//        toolProp tprop;
//        private void cBTool_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            string tmp = cBTool.SelectedItem.ToString();
//            if (tmp.IndexOf(")") > 0)
//            {
//                int tnr = int.Parse(tmp.Substring(0, tmp.IndexOf(")")));
//                Properties.Settings.Default.importGCToolDefNr = tnr;
//                if (cBToolSet.Checked)
//                {
//                    tprop = toolTable.getToolProperties(tnr);
//                    nUDToolDiameter.Value = (decimal)tprop.diameter;
//                    nUDToolFeedXY.Value = (decimal)tprop.feedXY;
//                    nUDToolZStep.Value = (decimal)Math.Abs(tprop.stepZ);
//                    nUDToolFeedZ.Value = (decimal)tprop.feedZ;
//                    nUDToolOverlap.Value = (decimal)tprop.overlap;
//                    nUDToolSpindleSpeed.Value = (decimal)tprop.spindleSpeed;
//                }
//            }
//        }

//        private void updateControls()
//        { }
//    }
//}
