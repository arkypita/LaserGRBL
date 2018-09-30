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
//using System;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Windows.Forms;
//using System.Text.RegularExpressions;
//using System.Globalization;
//using System.Threading;
//using System.Drawing;

//// http://imajeenyus.com/computer/20150110_single_line_fonts/
//// Hershey code from: http://www.evilmadscientist.com/2011/hershey-text-an-inkscape-extension-for-engraving-fonts/
//namespace GRBL_Plotter
//{
//    public partial class GCodeFromText : Form
//    {

//        private static StringBuilder gcodeString = new StringBuilder();

//        public GCodeFromText()
//        {
//            CultureInfo ci = new CultureInfo(Properties.Settings.Default.language);
//            Thread.CurrentThread.CurrentCulture = ci;
//            Thread.CurrentThread.CurrentUICulture = ci;
//            InitializeComponent();
//        }

//        private string textgcode = "";
//        public string textGCode
//        { get { return textgcode; } }

//        //        private static float gcodeXYFeed = 2000;
//        private static bool gcodeUseSpindle = false;
//        //        private static bool gcodePenIsUp = false;
//        private void TextForm_Load(object sender, EventArgs e)
//        {
//            cBFont.Items.AddRange(GCodeFromFont.fontNames);
//            cBFont.Items.AddRange(GCodeFromFont.fontFileName());

//            cBFont.SelectedIndex = Properties.Settings.Default.textFontIndex;
//            tBText.Text = Properties.Settings.Default.textFontText;
//            nUDFontSize.Value = Properties.Settings.Default.textFontSize;

//            Location = Properties.Settings.Default.locationTextForm;
//            Size desktopSize = System.Windows.Forms.SystemInformation.PrimaryMonitorSize;
//            if ((Location.X < -20) || (Location.X > (desktopSize.Width - 100)) || (Location.Y < -20) || (Location.Y > (desktopSize.Height - 100))) { Location = new Point(0, 0); }
//            getSettings();

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
//                    {
//                        cBTool.SelectedIndex = cBTool.Items.Count - 1;
//                        defaultToolFound = true;
//                    }
//                }
//            }
//            if (!defaultToolFound)
//                cBTool.SelectedIndex = 0;
//        }
//        private void getSettings()
//        {
//            //        gcodeXYFeed = (float)Properties.Settings.Default.importGCXYFeed;
//            gcodeUseSpindle = Properties.Settings.Default.importGCZEnable;
//        }
//        private void TextForm_FormClosing(object sender, FormClosingEventArgs e)
//        {
//            Properties.Settings.Default.textFontIndex = cBFont.SelectedIndex;
//            Properties.Settings.Default.textFontSize = nUDFontSize.Value;
//            Properties.Settings.Default.textFontText = tBText.Text;
//            Properties.Settings.Default.locationTextForm = Location;
//            Properties.Settings.Default.Save();
//        }

//        // get text, break it into chars, get path, etc... This event needs to be assigned in MainForm to poll text
//        private void btnApply_Click(object sender, EventArgs e)     // in MainForm:  _text_form.btnApply.Click += getGCodeFromText;
//        {
//            getSettings();
//            gcode.setup();
//            GCodeFromFont.reset();

//            GCodeFromFont.gcText = tBText.Text;
//            GCodeFromFont.gcFont = GCodeFromFont.getFontIndex(cBFont.SelectedIndex);
//            GCodeFromFont.gcFontName = cBFont.Items[cBFont.SelectedIndex].ToString();
//            GCodeFromFont.gcHeight = (double)nUDFontSize.Value;
//            GCodeFromFont.gcFontDistance = (double)nUDFontDistance.Value;
//            GCodeFromFont.gcLineDistance = (double)(nUDFontLine.Value / nUDFontSize.Value);
//            GCodeFromFont.gcSpacing = (double)(nUDFontLine.Value / nUDFontSize.Value) / 1.5;
//            GCodeFromFont.gcPauseChar = cBPauseChar.Checked;
//            GCodeFromFont.gcPauseWord = cBPauseWord.Checked;
//            GCodeFromFont.gcPauseLine = cBPauseLine.Checked;

//            //           MessageBox.Show(cBFont.Items[cBFont.SelectedIndex].ToString());
//            gcodeString.Clear();
//            GCodeFromFont.getCode(gcodeString);

//            if (gcodeUseSpindle) gcode.SpindleOff(gcodeString, "End");
//            gcodeString.Replace(',', '.');
//            string header = gcode.GetHeader("Text import");
//            string footer = gcode.GetFooter();

//            textgcode = header + gcodeString.ToString() + footer;
//        }


//        // adapt line distance depending on font size
//        private void nUDFontSize_ValueChanged(object sender, EventArgs e)
//        {
//            nUDFontLine.Value = nUDFontSize.Value * (decimal)1.5;
//        }

//        private void btnCancel_Click(object sender, EventArgs e)
//        {
//            this.Close();
//        }

//        private void cBTool_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            string tmp = cBTool.SelectedItem.ToString();
//            if (tmp.IndexOf(")") > 0)
//            {
//                int tnr = int.Parse(tmp.Substring(0, tmp.IndexOf(")")));
//                Properties.Settings.Default.importGCToolDefNr = tnr;
//            }
//        }
//    }
//}
