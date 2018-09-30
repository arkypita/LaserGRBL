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

///*  Thanks to:
// *  3dpBurner Image2Gcode. A Image to GCODE converter for GRBL based devices.
//    This file is part of 3dpBurner Image2Gcode application.
   
//    Copyright (C) 2015  Adrian V. J. (villamany) contact: villamany@gmail.com
//*/

//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.Drawing.Imaging;
//using System.IO;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Windows.Forms;
//using System.Globalization;
//using System.Threading;
//using System.Net;

//namespace GRBL_Plotter
//{
//    public partial class GCodeFromImage : Form
//    {
//        Bitmap loadedImage;
//        Bitmap originalImage;
//        Bitmap adjustedImage;
//        Bitmap resultImage;
//        private static int svgToolMax = 256;            // max amount of tools
//        private static StringBuilder[] gcodeString = new StringBuilder[svgToolMax];
//        private static int gcodeStringIndex = 0;
//        private static StringBuilder finalString = new StringBuilder();
//        private static StringBuilder tmpString = new StringBuilder();
//        private static int svgToolIndex = 0;            // last index
//        private static bool gcodeToolChange = false;          // Apply tool exchange command
//        private static bool gcodeSpindleToggle = false; // Switch on/off spindle for Pen down/up (M3/M5)
//        private static bool loadFromFile = false;

//        private string imagegcode = "";
//        public string imageGCode
//        { get { return imagegcode; } }

//        public GCodeFromImage(bool loadFile=false)
//        {
//            CultureInfo ci = new CultureInfo(Properties.Settings.Default.language);
//            Thread.CurrentThread.CurrentCulture = ci;
//            Thread.CurrentThread.CurrentUICulture = ci;
//            InitializeComponent();
//            loadFromFile = loadFile;
//        }

//        private void getSettings()
//        {
//            svgToolIndex = toolTable.init();       // svgPalette.cs
//        }

//        // load picture when form opens
//        private void ImageToGCode_Load(object sender, EventArgs e)
//        {
//            if (loadFromFile) loadExtern(lastFile);
//            originalImage = new Bitmap(Properties.Resources.modell);

//            Location = Properties.Settings.Default.locationImageForm;
//            Size desktopSize = System.Windows.Forms.SystemInformation.PrimaryMonitorSize;
//            if ((Location.X < -20) || (Location.X > (desktopSize.Width - 100)) || (Location.Y < -20) || (Location.Y > (desktopSize.Height - 100))) { Location = new Point(0, 0); }
//            processLoading();
//        }

//        private static string lastFile = "";
//        //OpenFile, save picture grayscaled to originalImage and save the original aspect ratio to ratio
//        private void btnLoad_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                OpenFileDialog sfd = new OpenFileDialog();
//                sfd.Filter = "Images|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tif;";
//                if (sfd.ShowDialog() == DialogResult.OK)
//                {
//                    if (!File.Exists(sfd.FileName)) return;
//                    lastFile = sfd.FileName;
//                    loadedImage = new Bitmap(Image.FromFile(sfd.FileName));
//                    originalImage = new Bitmap(Image.FromFile(sfd.FileName));
//                    processLoading();
//                }
//            }
//            catch (Exception err)
//            {
//                MessageBox.Show("Error opening file: " + err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        public void loadExtern(string file)
//        {
//            if (!File.Exists(file)) return;
//            lastFile = file;
//            loadedImage = new Bitmap(Image.FromFile(file));
//            originalImage = new Bitmap(Image.FromFile(file));
//            processLoading();
//        }

//        private void pasteFromClipboardToolStripMenuItem_Click(object sender, EventArgs e)
//        {   loadClipboard();  }
//        public void loadClipboard()
//        {
//            IDataObject iData = Clipboard.GetDataObject();
//            if (iData.GetDataPresent(DataFormats.Bitmap))
//            {
//                lastFile = "";
//                loadedImage = new Bitmap(Clipboard.GetImage());
//                originalImage = new Bitmap(Clipboard.GetImage());
//                processLoading();
//            }
//        }

//        public void loadURL(string url)
//        {
//            pictureBox1.Load(url);
//            originalImage = new Bitmap(pictureBox1.Image);
//        }

//        private void processLoading()
//        {
//            lblStatus.Text = "Opening file...";
//            Refresh();
//            tBarBrightness.Value = 0;
//            tBarContrast.Value = 0;
//            tBarGamma.Value = 100;
//            lblBrightness.Text = Convert.ToString(tBarBrightness.Value);
//            lblContrast.Text = Convert.ToString(tBarContrast.Value);
//            lblGamma.Text = Convert.ToString(tBarGamma.Value / 100.0f);
//            if (cbGrayscale.Checked) originalImage = imgGrayscale(originalImage);
//            adjustedImage = new Bitmap(originalImage);
//            resultImage = new Bitmap(originalImage);
//            ratio = (originalImage.Width + 0.0f) / originalImage.Height;//Save ratio for future use if needled
//            if (cbLockRatio.Checked) nUDHeight.Value = nUDWidth.Value / (decimal)ratio; //Initialize y size
//            userAdjust();
//            lblStatus.Text = "Done";
//            getSettings();
//        }

//        float ratio; //Used to lock the aspect ratio when the option is selected

//        //Interpolate a 8 bit grayscale value (0-255) between min,max
//        private Int32 interpolate(Int32 grayValue, Int32 min, Int32 max)
//        {
//            Int32 dif = max - min;
//            return (min + ((grayValue * dif) / 255));
//        }


//        //Apply dirthering to an image (Convert to 1 bit)
//        private Bitmap imgDirther(Bitmap input)
//        {
//            lblStatus.Text = "Dirthering...";
//            Refresh();
//            var masks = new byte[] { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };
//            var output = new Bitmap(input.Width, input.Height, PixelFormat.Format1bppIndexed);
//            var data = new sbyte[input.Width, input.Height];
//            var inputData = input.LockBits(new Rectangle(0, 0, input.Width, input.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
//            try
//            {
//                var scanLine = inputData.Scan0;
//                var line = new byte[inputData.Stride];
//                for (var y = 0; y < inputData.Height; y++, scanLine += inputData.Stride)
//                {
//                    Marshal.Copy(scanLine, line, 0, line.Length);
//                    for (var x = 0; x < input.Width; x++)
//                    {
//                        data[x, y] = (sbyte)(64 * (GetGreyLevel(line[x * 3 + 2], line[x * 3 + 1], line[x * 3 + 0]) - 0.5));
//                    }
//                }
//            }
//            finally
//            {
//                input.UnlockBits(inputData);
//            }
//            var outputData = output.LockBits(new Rectangle(0, 0, output.Width, output.Height), ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed);
//            try
//            {
//                var scanLine = outputData.Scan0;
//                for (var y = 0; y < outputData.Height; y++, scanLine += outputData.Stride)
//                {
//                    var line = new byte[outputData.Stride];
//                    for (var x = 0; x < input.Width; x++)
//                    {
//                        var j = data[x, y] > 0;
//                        if (j) line[x / 8] |= masks[x % 8];
//                        var error = (sbyte)(data[x, y] - (j ? 32 : -32));
//                        if (x < input.Width - 1) data[x + 1, y] += (sbyte)(7 * error / 16);
//                        if (y < input.Height - 1)
//                        {
//                            if (x > 0) data[x - 1, y + 1] += (sbyte)(3 * error / 16);
//                            data[x, y + 1] += (sbyte)(5 * error / 16);
//                            if (x < input.Width - 1) data[x + 1, y + 1] += (sbyte)(1 * error / 16);
//                        }
//                    }
//                    Marshal.Copy(line, 0, scanLine, outputData.Stride);
//                }
//            }
//            finally
//            {
//                output.UnlockBits(outputData);
//            }
//            lblStatus.Text = "Done";
//            Refresh();
//            return (output);
//        }
//        private static double GetGreyLevel(byte r, byte g, byte b)//aux for dirthering
//        {
//            return (r * 0.299 + g * 0.587 + b * 0.114) / 255;
//        }
//        //Adjust brightness contrast and gamma of an image      
//        private Bitmap imgBalance(Bitmap img, int brigh, int cont, int gam)
//        {
//            lblStatus.Text = "Balancing...";
//            Refresh();
//            ImageAttributes imageAttributes;
//            float brightness = (brigh / 100.0f) + 1.0f;
//            float contrast = (cont / 100.0f) + 1.0f;
//            float gamma = 1 / (gam / 100.0f);
//            float adjustedBrightness = brightness - 1.0f;
//            Bitmap output;
//            // create matrix that will brighten and contrast the image
//            float[][] ptsArray ={
//            new float[] {contrast, 0, 0, 0, 0}, // scale red
//            new float[] {0, contrast, 0, 0, 0}, // scale green
//            new float[] {0, 0, contrast, 0, 0}, // scale blue
//            new float[] {0, 0, 0, 1.0f, 0}, // don't scale alpha
//            new float[] {adjustedBrightness, adjustedBrightness, adjustedBrightness, 0, 1}};

//            output = new Bitmap(img);
//            imageAttributes = new ImageAttributes();
//            imageAttributes.ClearColorMatrix();
//            imageAttributes.SetColorMatrix(new ColorMatrix(ptsArray), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
//            imageAttributes.SetGamma(gamma, ColorAdjustType.Bitmap);
//            Graphics g = Graphics.FromImage(output);
//            g.DrawImage(output, new Rectangle(0, 0, output.Width, output.Height)
//            , 0, 0, output.Width, output.Height,
//            GraphicsUnit.Pixel, imageAttributes);
//            lblStatus.Text = "Done";
//            Refresh();
//            return (output);
//        }
//        //Return a grayscale version of an image
//        private Bitmap imgGrayscale(Bitmap original)
//        {
//            lblStatus.Text = "Grayscaling...";
//            Refresh();
//            Bitmap newBitmap = new Bitmap(original.Width, original.Height);//create a blank bitmap the same size as original
//            Graphics g = Graphics.FromImage(newBitmap);//get a graphics object from the new image
//            //create the grayscale ColorMatrix
//            ColorMatrix colorMatrix = new ColorMatrix(
//               new float[][]
//                {
//                    new float[] {.299f, .299f, .299f, 0, 0},
//                    new float[] {.587f, .587f, .587f, 0, 0},
//                    new float[] {.114f, .114f, .114f, 0, 0},
//                    new float[] {0, 0, 0, 1, 0},
//                    new float[] {0, 0, 0, 0, 1}
//                });
//            ImageAttributes attributes = new ImageAttributes();//create some image attributes
//            attributes.SetColorMatrix(colorMatrix);//set the color matrix attribute

//            //draw the original image on the new image using the grayscale color matrix
//            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
//               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
//            g.Dispose();//dispose the Graphics object
//            lblStatus.Text = "Done";
//            Refresh();
//            return (newBitmap);
//        }
//        //Return a inverted colors version of a image
//        private Bitmap imgInvert(Bitmap original)
//        {
//            lblStatus.Text = "Inverting...";
//            Refresh();
//            Bitmap newBitmap = new Bitmap(original.Width, original.Height);//create a blank bitmap the same size as original
//            Graphics g = Graphics.FromImage(newBitmap);//get a graphics object from the new image
//            //create the grayscale ColorMatrix
//            ColorMatrix colorMatrix = new ColorMatrix(
//               new float[][]
//                {
//                    new float[] {-1, 0, 0, 0, 0},
//                    new float[] {0, -1, 0, 0, 0},
//                    new float[] {0, 0, -1, 0, 0},
//                    new float[] {0, 0, 0, 1, 0},
//                    new float[] {1, 1, 1, 0, 1}
//                });
//            ImageAttributes attributes = new ImageAttributes();//create some image attributes
//            attributes.SetColorMatrix(colorMatrix);//set the color matrix attribute

//            //draw the original image on the new image using the grayscale color matrix
//            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
//               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
//            g.Dispose();//dispose the Graphics object
//            lblStatus.Text = "Done";
//            Refresh();
//            return (newBitmap);
//        }

//        // Resize image to desired width/height for gcode generation
//        //  http://stackoverflow.com/questions/1922040/resize-an-image-c-sharp
//        public static Bitmap imgResize(Image image, int width, int height)
//        {
//            var destRect = new Rectangle(0, 0, width, height);
//            var destImage = new Bitmap(width, height);

//            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

//            using (var graphics = Graphics.FromImage(destImage))
//            {
//                graphics.CompositingMode = CompositingMode.SourceCopy;
//                graphics.CompositingQuality = CompositingQuality.AssumeLinear;//.HighQuality;
//                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;// HighQualityBicubic;
//                graphics.SmoothingMode = SmoothingMode.None;//.HighQuality;
//                graphics.PixelOffsetMode = PixelOffsetMode.None;//.HighQuality;

//                using (var wrapMode = new ImageAttributes())
//                {
//                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
//                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
//                }
//            }
//            return destImage;
//        }        
        
//        //Invoked when the user input any value for image adjust
//        private void userAdjust()
//        {
//            try
//            {
//                if (adjustedImage == null) return;//if no image, do nothing
//                //Apply resize to original image
//                Int32 xSize;//Total X pixels of resulting image for GCode generation
//                Int32 ySize;//Total Y pixels of resulting image for GCode generation
//                xSize = (int)(nUDWidth.Value / nUDReso.Value);
//                ySize = (int)(nUDHeight.Value / nUDReso.Value); //Convert.ToInt32(float.Parse(tbHeight.Text, CultureInfo.InvariantCulture.NumberFormat) / float.Parse(tbRes.Text, CultureInfo.InvariantCulture.NumberFormat));
//                adjustedImage = imgResize(originalImage, xSize, ySize);
//                //Apply balance to adjusted (resized) image
//                adjustedImage = imgBalance(adjustedImage, tBarBrightness.Value, tBarContrast.Value, tBarGamma.Value);
//                //Reset dirthering to adjusted (resized and balanced) image
//                //              cbDirthering.Text = "GrayScale 8 bit";
//                //Display image
//                if (rbModeDither.Checked)// cbDirthering.Text == "Dirthering FS 1 bit")
//                {
//                    lblStatus.Text = "Dirtering...";
//                    adjustedImage = imgDirther(adjustedImage);
//                    pictureBox1.Image = adjustedImage;
//                    lblStatus.Text = "Done";
//                }
//                else
//                    pictureBox1.Image = adjustedImage;
//                lblColors.Text = "Colors:";
//                updateLabelColor = true;
//                resultImage = new Bitmap(adjustedImage);

//            }
//            catch (Exception e)
//            {
//                MessageBox.Show("Error resizing/balancing image: " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        private bool updateLabelColor = false;
//        private void timer1_Tick(object sender, EventArgs e)
//        {
//            if (updateLabelColor)
//                countImageColors();
//        }
//        private void countImageColors()
//        {   Dictionary<Color, int> colors = new Dictionary<Color, int>();
//            Color newcol;
//            for (int y = 0; y < adjustedImage.Height - 1; y++)
//            {
//                for (int x = 0; x < adjustedImage.Width - 1; x++)
//                {
//                    newcol = adjustedImage.GetPixel(x, y);
//                    if (colors.ContainsKey(newcol))
//                        colors[newcol] = colors[newcol] + 1;
//                    else
//                        colors.Add(newcol, 1);
//                }
//            }
//            lblColors.Text = "Colors: " + colors.Count.ToString();
//            updateLabelColor = false;
//        }

//        //Contrast adjusted by user
//        private void tBarContrast_Scroll(object sender, EventArgs e)
//        {
//            lblContrast.Text = Convert.ToString(tBarContrast.Value);
//            Refresh();
//            userAdjust();
//        }
//        //Brightness adjusted by user
//        private void tBarBrightness_Scroll(object sender, EventArgs e)
//        {
//            lblBrightness.Text = Convert.ToString(tBarBrightness.Value);
//            Refresh();
//            userAdjust();
//        }
//        //Gamma adjusted by user
//        private void tBarGamma_Scroll(object sender, EventArgs e)
//        {
//            lblGamma.Text = Convert.ToString(tBarGamma.Value / 100.0f);
//            Refresh();
//            userAdjust();
//        }
//        //Quick preview of the original image. Todo: use a new image container for fas return to processed image
//        private void btnCheckOrig_MouseDown(object sender, MouseEventArgs e)
//        {
//            if (adjustedImage == null) return;//if no image, do nothing
//            getSettings();
//            usedColors = new string[svgToolIndex + 1];
//            countColors = new int[svgToolIndex + 1];
//            Array.Resize<string>(ref usedColors, svgToolIndex + 1);     //usedColors = string[svgToolIndex + 1];
//            Array.Resize<int>(ref countColors, svgToolIndex + 1);     //countColors = new int[svgToolIndex + 1];
//            for (int i = 0; i <= svgToolIndex; i++)
//            {  countColors[i] = 0; usedColors[i] = ""; }    // usedColors[i] = "";
//            generateResultImage();
//            pictureBox1.Image = resultImage;
//        }
//        //Reload the processed image after temporal preiew of the original image
//        private void btnCheckOrig_MouseUp(object sender, MouseEventArgs e)
//        {
//            if (adjustedImage == null) return;//if no image, do nothing
//            pictureBox1.Image = adjustedImage;
//        }

//        //CheckBox lockAspectRatio checked. Set as mandatory the user setted width and calculate the height by using the original aspect ratio
//        private void cbLockRatio_CheckedChanged(object sender, EventArgs e)
//        {
//            if (cbLockRatio.Checked)
//            {
//                nUDHeight.Value = nUDWidth.Value / (decimal)ratio; //Initialize y size
//                if (adjustedImage == null) return;//if no image, do nothing
//                userAdjust();
//            }
//        }
//        //On form load
//        private void Form1_Load(object sender, EventArgs e)
//        {
//            lblStatus.Text = "Done";
//            getSettings();
//            autoZoomToolStripMenuItem_Click(this, null);//Set preview zoom mode
//        }

//        //Generate a "minimalist" gcode line based on the actual and last coordinates and laser power
//        float coordX;//X
//        float coordY;//Y
//        float lastX;//Last x/y  coords for compare
//        float lastY;

//        // colorMap will store x-start; x-stop values for each color and line(y)
//        private static Dictionary<int, List<int>[]> colorMap = new Dictionary<int, List<int>[]>();
//        private static int colorStart = -2, colorEnd = -2, lastTool = -2,lastLine=-2;
//        int myToolNumber;
//        private void setColorMap(int col, int line, int direction)
//        {
//            Color myColor = adjustedImage.GetPixel(col, (adjustedImage.Height - 1) - line);  //Get pixel color
//            myToolNumber = toolTable.getToolNr(myColor, (int)nUDMode.Value);     // find nearest color in palette
//            toolTable.countPixel();
//            if (((cbExceptAlpha.Checked) && (myColor.A == 0)) || (myToolNumber < 0))
//                myToolNumber = -1;

//            if ((myToolNumber >= 0) && (!colorMap.ContainsKey(myToolNumber)))
//            {   colorMap.Add(myToolNumber, new List<int>[adjustedImage.Height]);
//                for (int i=0; i< adjustedImage.Height;i++)
//                {   colorMap[myToolNumber][i] = new List<int>(); }
//            }

//            if (lastTool != myToolNumber)
//            {   if (lastTool >= 0)
//                    colorMap[lastTool][line].Add(colorEnd);// -direction);     // finish old color
//                colorStart = col;
//                colorEnd = col;
//                if (myToolNumber >= 0)
//                    colorMap[myToolNumber][line].Add(colorStart);   // start new color
//            }
//            else
//            {
//                if ((lastLine != line) && (lastLine >= 0) && (myToolNumber >= 0))
//                { colorMap[myToolNumber][lastLine].Add(colorEnd);// -direction); // finish old line
//                    colorStart = col;
//                    colorMap[myToolNumber][line].Add(colorStart);   // start new line
//                }
//                colorEnd = col;
//            }
//            lastTool = myToolNumber;
//            lastLine = line;
//        }

//        private void convertColorMap(float resol)
//        {
//            int tool, skipTooNr=0;
//            int key;
//            gcode.MoveToRapid(finalString, 0, 0);          // move to start pos
//            for (int index = 0; index < svgToolIndex; index++)  // go through sorted by pixel-amount list
//            {
//                toolTable.setIndex(index);                     // set index in class
//                key = toolTable.indexToolNr();                 // if tool-nr == known key go on
//                if (colorMap.ContainsKey(key))
//                {
//                    tool = toolTable.indexToolNr();            // use tool in palette order
//                    if (cbSkipToolOrder.Checked)
//                        tool = skipTooNr++;                     // or start tool-nr at 0

//                    finalString.AppendLine("\r\n( +++++ Tool change +++++ )");
//                    gcode.Tool(finalString, tool, toolTable.indexName());  // + svgPalette.pixelCount());
//                    for (int y = 0; y < adjustedImage.Height; y++)
//                    {
//                        while (colorMap[key][y].Count > 1)          // stat at line 0 and check line by line
//                            drawColorMap(resol, key, y, 0, true);   
//                    }
//                }
//            }
//        }

//        // check recursive line by line for same color near by given x-value
//        private void drawColorMap(float resol, int color, int line, int startIndex, bool first)
//        {   int start, stop, newIndex;
//            float coordy = resol * (float)line;
//            if (colorMap[color][line].Count > 1)
//            {   if ((startIndex % 2 == 0) && ((startIndex +1)< colorMap[color][line].Count)) //////
//                {
//                    start = colorMap[color][line][startIndex];
//                    stop = colorMap[color][line][startIndex+1];
//                    colorMap[color][line].RemoveRange(startIndex, 2);
//                }
//                else
//                {
//                    start = colorMap[color][line][startIndex];
//                    stop = colorMap[color][line][startIndex-1];
//                    colorMap[color][line].RemoveRange(startIndex-1, 2);
//                }
//                float coordX = resol * (float)start;
//                if (first)
//                {
//                    gcode.MoveToRapid(finalString, coordX, coordy);          // move to start pos
//                    gcode.PenDown(finalString);
//                }
//                else
//                    gcode.MoveTo(finalString, coordX, coordy);          // move to start pos
//                coordX = resol * (float)stop;
//                gcode.MoveTo(finalString, coordX, coordy);              // move to end pos

//                if (line < (adjustedImage.Height - 1))
//                {
//                    var nextLine = colorMap[color][line + 1];   // check for pixel nearby
//                    bool end = true;
//                    for (int k = -1; k <= 1; k++)
//                    {   // check recursive line by line for same color near by given x-value
//                        newIndex = nextLine.IndexOf(stop + k);
//                        if (newIndex >= 0)
//                        {
//                            drawColorMap(resol, color, line + 1, newIndex, false);
//                            end = false;
//                            break;
//                        }
//                    }
//                    if (end)
//                        gcode.PenUp(finalString);
//                }
//                else
//                    gcode.PenUp(finalString);
//            }
//        }

//        private static string debugColorMap()
//        {
//            string temp = "";
//            int i = 0;
//            foreach (var pair in colorMap)
//            {
//                i = 0;
//                temp += "\r\n\r\n NEW COLOR\r\n";
//                foreach (var y in pair.Value)
//                {
//                    temp += pair.Key+" "+ i.ToString() + "  ";
//                    foreach (int pixel in y)
//                    {
//                        temp += pixel.ToString() + "|";
//                    }
//                    temp += "\r\n"; i++;
//                }
//            }
//            return temp;
//        }


//        private void drawHeight(int col, int lin, float coordX, float coordY)
//        {
//            Color myColor = adjustedImage.GetPixel(col, (adjustedImage.Height - 1) - lin);          // Get pixel color
//            double height = 255 - Math.Round((double)(myColor.R + myColor.G + myColor.B) / 3);  // calc height
//            float coordZ = (float)((double)nUDZTop.Value - height * (double)(nUDZTop.Value- nUDZBot.Value) / 255);    // calc Z value
//            string feed = string.Format("F{0}", gcode.gcodeXYFeed); ;
//            gcode.MoveTo(finalString, coordX, coordY, coordZ, "");
//        }


//        private int lastToolNumber=-1;
//        private float lastDrawX, lastDrawY;
//        bool lastIfBackground = false;
//        private void drawPixel(int col, int lin, float coordX, float coordY, int edge, int dir)
//        {
//            Color myColor = adjustedImage.GetPixel(col, (adjustedImage.Height - 1) - lin);  //Get pixel color
//            int myToolNumber = toolTable.getToolNr(myColor,(int)nUDMode.Value);     // find nearest color in palette
//            toolTable.countPixel();
//            bool ifBackground = false;
//            float myX = coordX;
//            float myY = coordY;

//            if (((cbExceptAlpha.Checked) && (myColor.A == 0)) || (myToolNumber < 0))
//            {   ifBackground = true;
//                myToolNumber = -1;
//                toolTable.setUse(false);
//            }
//            else
//                toolTable.setUse(true);

//            if (edge == 0)
//            {
//                if (dir == -1) myX += (float)nUDReso.Value;
//                if (dir == -2) myX += (float)nUDReso.Value;
//                if (dir == 2) myY += (float)nUDReso.Value;
//            }
//            if ((lastToolNumber != myToolNumber) ||  (edge>0))
//            {   
//                if ((edge != 1) && !lastIfBackground)
//                { lineEnd(myX,myY);   }
//                if (myToolNumber >=0) gcodeStringIndex = myToolNumber;
//                if ((edge != 2) && !ifBackground)
//                { lineStart(myX, myY); }
//                lastDrawX = coordX;
//                lastDrawY = coordY;
//            }

//            lastToolNumber = myToolNumber;
//            lastX = coordX; lastY = coordY;
//            lastIfBackground = ifBackground;
//        }
//        private void lineEnd(float x, float y, string txt="")   // finish line with old pen
//        {
//            gcode.MoveTo(tmpString, x, y, txt);          // move to end pos
//            gcode.PenUp(tmpString);                             // pen up
//            gcodeString[gcodeStringIndex].Append(tmpString);    // take over code if...
//            tmpString.Clear();
//        }
//        private void lineStart(float x, float y, string txt="")
//        {
//            gcode.MoveToRapid(tmpString, x, y, txt);         // rapid move to start pos
//            gcode.PenDown(tmpString);                           // pen down
//        }

//        //Generate button click
//        public void btnGenerate_Click(object sender, EventArgs e)
//        {
//            Cursor.Current = Cursors.WaitCursor;
//            getSettings();
//            colorMap.Clear();
//            if (cbExceptColor.Checked)
//                toolTable.setExceptionColor(cbExceptColor.BackColor);
//            else
//                toolTable.clrExceptionColor();

//            gcodeStringIndex = 0;
//            for (int i = 0; i < svgToolMax; i++)
//            {
//                gcodeString[i] = new StringBuilder();
//                gcodeString[i].Clear();
//            }
//            finalString.Clear();
//            gcode.setup();

//            if (rBProcessZ.Checked)
//            {   generateHeightData();
//                return;
//            }//            gcodeString.Clear();
//            if (adjustedImage == null) return;  //if no image, do nothing
//            float resol = (float)nUDReso.Value;
//            float w = (float)nUDWidth.Value;   
//            float h = (float)nUDHeight.Value;  

//            if ((resol <= 0) | (adjustedImage.Width < 1) | (adjustedImage.Height < 1) | (w < 1) | (h < 1))
//            {
//                MessageBox.Show("Check widht, height and resolution values.", "Invalid value", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return;
//            }
//            Int32 lin;//top/botom pixel
//            Int32 col;//Left/right pixel

//            lblStatus.Text = "Generating file...";
//            Refresh();
//            //Generate picture Gcode
//            Int32 pixTot = adjustedImage.Width * adjustedImage.Height;
//            Int32 pixBurned = 0;
//            int edge = 0;
//            int direction = 0;
//            if (!gcodeSpindleToggle) gcode.SpindleOn(finalString, "Start spindle");
//            gcode.PenUp(finalString);                             // pen up
//            //            colorMap= new Dictionary<int, new List<int>[adjustedImage.Width]>();
//            //////////////////////////////////////////////
//            // Generate Gcode lines by Horozontal scanning
//            //////////////////////////////////////////////
//            #region horizontal and setColorMap
//            if (rbEngravingPattern1.Checked)
//            {
//                //Start image
//                lin = adjustedImage.Height - 1;//top tile
//                col = 0;//Left pixel
//                tmpString.Clear();
//                lastX = 0;//reset last positions
//                lastY = resol * (float)lin;
//                while (lin >= 0)
//                {
//                    //Y coordinate
//                    coordY = resol * (float)lin;
//                    edge = 1;                           // line starts
//                    direction = 1;                      // left to right
//                    while (col < adjustedImage.Width)   //From left to right
//                    {
//                        //X coordinate
//                        coordX = resol * (float)col;
//                        setColorMap(col, lin, direction);
//                 //       drawPixel(col,lin,coordX, coordY,edge,dir);
//                        edge = 0;
//                        pixBurned++;
//                        col++;
//                        if (col >= adjustedImage.Width-1) edge = 2; // line ends
//                    }
//                    col--;
//                    lin--;
//                    coordY = resol * (float)lin;
//                    edge = 1;                           // line starts
//                    direction = -1;                     // right to left
//                    while ((col >= 0) & (lin >= 0))     //From right to left
//                    {
//                        //X coordinate
//                        coordX = resol * (float)col;
//                        setColorMap(col, lin, direction);
//                  //      drawPixel(col, lin, coordX, coordY,edge,dir);
//                        edge = 0;
//                        pixBurned++;
//                        col--;
//                        if (col <= 0) edge = 2;         // line ends
//                    }
//                    col++;
//                    lin--;
//                    lblStatus.Text = "Generating GCode... " + Convert.ToString((pixBurned * 100) / pixTot) + "%";
//                    Refresh();
//                }
//                if (myToolNumber >=0)
//                    colorMap[myToolNumber][0].Add(colorEnd);
//            }
//            #endregion
//            //////////////////////////////////////////////
//            // Generate Gcode lines by Diagonal scanning
//            //////////////////////////////////////////////
//            #region diagonal and  drawPixel
//            else
//            {
//                //Start image
//                col = 0;
//                lin = 0;
//                lastX = 0;//reset last positions
//                lastY = 0;
//                while ((col < adjustedImage.Width) | (lin < adjustedImage.Height))
//                {
//                    edge = 1;
//                    direction = 2;    // up-left to low-right

//                    while ((col < adjustedImage.Width) & (lin >= 0))
//                    {
//                        //Y coordinate
//                        coordY = resol * (float)lin;
//                        //X coordinate
//                        coordX = resol * (float)col;

//                        drawPixel(col, lin, coordX, coordY,edge,direction);
//                        edge = 0;
//                        pixBurned++;
//                        col++;
//                        lin--;
//                        if ((col >= adjustedImage.Width-1) || (lin <=0)) edge = 2;  //&& (lin == 0)
//                    }
//                    col--;
//                    lin++;

//                    if (col >= adjustedImage.Width - 1) lin++;
//                    else col++;
//                    edge = 1;
//                    direction = -2;    // low-right to up-left 
//                    while ((col >= 0) & (lin < adjustedImage.Height))
//                    {
//                        //Y coordinate
//                        coordY = resol * (float)lin;
//                        //X coordinate
//                        coordX = resol * (float)col;

//                        drawPixel(col, lin, coordX, coordY,edge,direction);
//                        edge = 0;
//                        pixBurned++;
//                        col--;
//                        lin++;
//                        if ((col <= 0) || (lin >= adjustedImage.Height-1)) edge = 2;  //&& (lin >= adjustedImage.Height-1)
//                    }
//                    col++;
//                    lin--;
//                    if (lin >= adjustedImage.Height - 1) col++;
//                    else lin++;
//                    lblStatus.Text = "Generating GCode... " + Convert.ToString((pixBurned * 100) / pixTot) + "%";
//                    Refresh();
//                }


//            }
//            #endregion

//            Refresh();
//            lblStatus.Text = "Done (" + Convert.ToString(pixBurned) + "/" + Convert.ToString(pixTot) + ")";
//            imagegcode = "( Generated by GRBL-Plotter )\r\n";

//            int arrayIndex;
//            int skipTooNr = 0;
//            int tool = 0;

//            if (cBImportSVGTool.Checked)
//                toolTable.sortByToolNr();       
//            else
//                toolTable.sortByPixelCount();    // sort by color area (max. first)

//            if (!rbEngravingPattern1.Checked)   // diagonal
//            {
//                for (int i = 0; i < svgToolIndex; i++)
//                {
//                    toolTable.setIndex(i);
//                    arrayIndex = toolTable.indexToolNr();
//                    if ((arrayIndex >= 0) && (gcodeString[arrayIndex].Length > 1))
//                    {
//                        if ((gcodeToolChange) && toolTable.indexUse())
//                        {
//                            tool = toolTable.indexToolNr();
//                            if (cbSkipToolOrder.Checked)
//                                tool = skipTooNr++;
//                            finalString.AppendLine("\r\n( +++++ Tool change +++++ )");
//                            gcode.Tool(finalString, tool, toolTable.indexName());  // + svgPalette.pixelCount());
//                        }
//                        finalString.Append(gcodeString[toolTable.indexToolNr()]);
//                    }
//                }

//                if (!gcodeSpindleToggle) gcode.SpindleOff(finalString, "Stop spindle");
//                imagegcode += gcode.GetHeader("Image import") + finalString.Replace(',', '.').ToString() + gcode.GetFooter();
//            }
//            else
//            {               // horizontal
//                gcode.reduceGCode = true;
//                convertColorMap(resol);
//                if (!gcodeSpindleToggle) gcode.SpindleOff(finalString, "Stop spindle");
//                imagegcode += gcode.GetHeader("Image import") + finalString.Replace(',', '.').ToString() + gcode.GetFooter();
//           //     imagegcode += debugColorMap();
//            }
//        }

//        private void generateHeightData()
//        {
//            getSettings();
//            finalString.Clear();
//            gcode.setup();
//            gcode.reduceGCode = cBCompress.Checked;

//            if (adjustedImage == null) return;  //if no image, do nothing
//            float resol = (float)nUDReso.Value;
//            float w = (float)nUDWidth.Value;
//            float h = (float)nUDHeight.Value;

//            if ((resol <= 0) | (adjustedImage.Width < 1) | (adjustedImage.Height < 1) | (w < 1) | (h < 1))
//            {
//                MessageBox.Show("Check widht, height and resolution values.", "Invalid value", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return;
//            }
//            Int32 lin;//top/botom pixel
//            Int32 col;//Left/right pixel

//            lblStatus.Text = "Generating file...";
//            Refresh();
//            //Generate picture Gcode
//            Int32 pixTot = adjustedImage.Width * adjustedImage.Height;
//            Int32 pixBurned = 0;
//            //int direction = 0;
//            if (!gcodeSpindleToggle) gcode.SpindleOn(finalString, "Start spindle");

//            if (rbEngravingPattern1.Checked)
//            {
//                //Start image
//                lin = adjustedImage.Height - 1;//top tile
//                col = 0;//Left pixel
//                lastX = 0;//reset last positions
//                lastY = resol * (float)lin;
//                coordX = resol * (float)col;
//                coordY = resol * (float)lin;
//                gcode.PenUp(finalString);                             // pen up
//                gcode.MoveToRapid(finalString, coordX, coordY, "");         // rapid move to start pos
//                while (lin >= 0)
//                {
//                    //Y coordinate
//                    coordY = resol * (float)lin;
//                    //direction = 1;                      // left to right
//                    while (col < adjustedImage.Width)   //From left to right
//                    {
//                        //X coordinate
//                        coordX = resol * (float)col;
//                        drawHeight(col, lin, coordX, coordY);
//                        pixBurned++;
//                        col++;
//                    }
//                    col--;
//                    lin--;
//                    coordY = resol * (float)lin;
//                    //direction = -1;                     // right to left
//                    while ((col >= 0) & (lin >= 0))     //From right to left
//                    {
//                        //X coordinate
//                        coordX = resol * (float)col;
//                        drawHeight(col, lin, coordX, coordY);
//                        pixBurned++;
//                        col--;
//                    }
//                    col++;
//                    lin--;
//                    lblStatus.Text = "Generating GCode... " + Convert.ToString((pixBurned * 100) / pixTot) + "%";
//                    Refresh();
//                }
//            }
//            else
//            {
//                //Start image
//                col = 0;
//                lin = 0;
//                lastX = 0;//reset last positions
//                lastY = 0;
//                while ((col < adjustedImage.Width) | (lin < adjustedImage.Height))
//                {
//                    //direction = 2;    // up-left to low-right

//                    while ((col < adjustedImage.Width) & (lin >= 0))
//                    {
//                        //Y coordinate
//                        coordY = resol * (float)lin;
//                        //X coordinate
//                        coordX = resol * (float)col;

//                        drawHeight(col, lin, coordX, coordY);
//                        pixBurned++;
//                        col++;
//                        lin--;
//                    }
//                    col--;
//                    lin++;

//                    if (col >= adjustedImage.Width - 1) lin++;
//                    else col++;
//                    //direction = -2;    // low-right to up-left 
//                    while ((col >= 0) & (lin < adjustedImage.Height))
//                    {
//                        //Y coordinate
//                        coordY = resol * (float)lin;
//                        //X coordinate
//                        coordX = resol * (float)col;

//                        drawHeight(col, lin, coordX, coordY);
//                        pixBurned++;
//                        col--;
//                        lin++;
//                    }
//                    col++;
//                    lin--;
//                    if (lin >= adjustedImage.Height - 1) col++;
//                    else lin++;
//                    lblStatus.Text = "Generating GCode... " + Convert.ToString((pixBurned * 100) / pixTot) + "%";
//                    Refresh();
//                }
//            }
//            gcode.PenUp(finalString);                             // pen up
//            if (!gcodeSpindleToggle) gcode.SpindleOff(finalString, "Stop spindle");

//            imagegcode = "( Generated by GRBL-Plotter )\r\n";
//            imagegcode += gcode.GetHeader("Image import") + finalString.Replace(',', '.').ToString() + gcode.GetFooter();
//        }

//        private static string debug_string;
//        private static string[] usedColors = new string[svgToolIndex+1];
//        private static int[] countColors = new int[svgToolIndex + 1];
//        private void btnList_Click(object sender, EventArgs e)
//        {
//            getSettings();
//            Array.Resize<string>(ref usedColors, svgToolIndex + 1);     //usedColors = string[svgToolIndex + 1];
//            Array.Resize<int>(ref countColors, svgToolIndex + 1);     //countColors = new int[svgToolIndex + 1];

//            for (int i = 0; i <= svgToolIndex; i++)
//            { usedColors[i] = ""; countColors[i] = 0; }
//            generateResultImage();

//            string tool_string = "";
//            string not_used = "\r\nAll current palette colors:\r\n";
//            Dictionary<int, string> values =  new Dictionary<int, string>();

//            debug_string = "Palette has " + (svgToolIndex - 1).ToString() + " colors\r\n\r\n";
//            if (cbSkipToolOrder.Checked)
//                debug_string += "Colors / Tools used in image (sort by pixel count):\r\nTool nr not from palette\r\n ";
//            else
//                debug_string += "Colors / Tools used in image:\r\n ";
//            for (int i = 0; i <= svgToolIndex; i++)
//            {
//                if (usedColors[i].Length > 1)
//                {
//                    if (i < 2)
//                        debug_string += (i - 2).ToString() + ") Exception color " + usedColors[i] + "\r\n";
//                    else
//                    {
//                        tool_string += (i - 2).ToString() + ") " + usedColors[i] + "\r\n";
//                        while (values.ContainsKey(countColors[i]))
//                            countColors[i]++;
//                        values.Add(countColors[i], usedColors[i]);
//                    }
//                }
//            }
//            for (int i = 0; i < svgToolIndex-1; i++)
//            { not_used += "Tool Nr.: "+(i).ToString() + ") " + toolTable.getToolName(i) + "\r\n"; }

//            if (cbSkipToolOrder.Checked)
//            {   if (values.Count() > 0)
//                {
//                    tool_string = "";
//                    var list = values.Keys.ToList();
//                    list.Sort();            // sort by pixelamount
//                    list.Reverse();         // but descending order
//                    int i = 0;
//                    foreach (var key in list)
//                    {
//                        tool_string += (i++).ToString() + ") " + " "+key+"  "+values[key] + "\r\n";
//                    }
//                }
//            }
//            MessageBox.Show(debug_string + tool_string + not_used, "List of pens");
//        }

//        private void generateResultImage()
//        {   int x, y;
//            Color myColor,newColor;
//            if (cbExceptColor.Checked)
//                toolTable.setExceptionColor(cbExceptColor.BackColor);
//            else
//                toolTable.clrExceptionColor();
//            int myToolNr, myIndex;
//            int mode = (int)nUDMode.Value;
//            for (y = 0; y < adjustedImage.Height; y++)
//            {
//                for (x = 0; x < adjustedImage.Width; x++)
//                {
//                    myColor = adjustedImage.GetPixel(x, y);                 // Get pixel color}
//                    if (((cbExceptAlpha.Checked) && (myColor.A == 0)))      // skip exception
//                    {  newColor = Color.White; myToolNr = -2; usedColors[0] = "Alpha = 0      " + myColor.ToString();
//                    }
//                    else
//                    {   myToolNr = toolTable.getToolNr(myColor, mode);     // find nearest color in palette
//                        if (myToolNr < 0)
//                            newColor = Color.White;
//                        else
//                            newColor = toolTable.getColor();   // Color.FromArgb(255, r, g, b);
//                    }
//                    myIndex = myToolNr + 2;
//                    if (myIndex < 0) MessageBox.Show("index <0");
//                    else
//                    {
//                        countColors[myIndex]++;
//                        if (usedColors[myIndex].Length < 1)
//                            usedColors[myIndex] = toolTable.getName() + "      " + toolTable.getColor().ToString();
//                    }
//                    resultImage.SetPixel(x, y, newColor);
//                }
//            }
//        }

//        //Horizontal mirroing
//        private void btnHorizMirror_Click(object sender, EventArgs e)
//        {
//            if (adjustedImage == null) return;//if no image, do nothing
//            lblStatus.Text = "Mirroing...";
//            Refresh();
//            adjustedImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
//            originalImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
//            pictureBox1.Image = adjustedImage;
//            lblStatus.Text = "Done";
//        }
//        //Vertical mirroing
//        private void btnVertMirror_Click(object sender, EventArgs e)
//        {
//            if (adjustedImage == null) return;//if no image, do nothing
//            lblStatus.Text = "Mirroing...";
//            Refresh();
//            adjustedImage.RotateFlip(RotateFlipType.RotateNoneFlipY);
//            originalImage.RotateFlip(RotateFlipType.RotateNoneFlipY);
//            pictureBox1.Image = adjustedImage;
//            lblStatus.Text = "Done";
//        }
//        //Rotate right
//        private void btnRotateRight_Click(object sender, EventArgs e)
//        {
//            if (adjustedImage == null) return;//if no image, do nothing
//            lblStatus.Text = "Rotating...";
//            Refresh();
//            adjustedImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
//            originalImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
//            ratio = 1 / ratio;
//            decimal s = nUDHeight.Value;
//            nUDHeight.Value = nUDWidth.Value;
//            nUDWidth.Value = s;
//            pictureBox1.Image = adjustedImage;
//            autoZoomToolStripMenuItem_Click(this, null);
//            lblStatus.Text = "Done";
//        }
//        //Rotate left
//        private void btnRotateLeft_Click(object sender, EventArgs e)
//        {
//            if (adjustedImage == null) return;//if no image, do nothing
//            lblStatus.Text = "Rotating...";
//            Refresh();
//            adjustedImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
//            originalImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
//            ratio = 1 / ratio;
//            decimal s = nUDHeight.Value;
//            nUDHeight.Value = nUDWidth.Value;
//            nUDWidth.Value = s;
//            pictureBox1.Image = adjustedImage;
//            autoZoomToolStripMenuItem_Click(this, null);
//            lblStatus.Text = "Done";
//        }
//        //Invert image color
//        private void btnInvert_Click(object sender, EventArgs e)
//        {
//            if (adjustedImage == null) return;//if no image, do nothing
//            adjustedImage = imgInvert(adjustedImage);
//            originalImage = imgInvert(originalImage);
//            pictureBox1.Image = adjustedImage;
//        }

//       // private void cbDirthering_SelectedIndexChanged(object sender, EventArgs e)
//        private void rbModeGray_CheckedChanged(object sender, EventArgs e)
//        {
//            if (adjustedImage == null) return;//if no image, do nothing
//            if (rbModeDither.Checked)// cbDirthering.Text == "Dirthering FS 1 bit")
//            {
//                lblStatus.Text = "Dirtering...";
//                adjustedImage = imgDirther(adjustedImage);
//                pictureBox1.Image = adjustedImage;
//                lblStatus.Text = "Done";
//            }
//            else
//                userAdjust();
//            updateLabelColor = true;

//        }
//        private void autoZoomToolStripMenuItem_Click(object sender, EventArgs e)
//        {
//            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
//            pictureBox1.Width = panel1.Width;
//            pictureBox1.Height = panel1.Height;
//            pictureBox1.Top = 0;
//            pictureBox1.Left = 0;
//        }

//        private void nUDWidth_ValueChanged(object sender, EventArgs e)
//        {
//            if (adjustedImage == null) return;//if no image, do nothing
//            if (cbLockRatio.Checked)
//            {
//                nUDHeight.Value = (decimal)((float)nUDWidth.Value / ratio);
//            }
//            userAdjust();
//        }

//        private void nUDHeight_ValueChanged(object sender, EventArgs e)
//        {
//            if (adjustedImage == null) return;//if no image, do nothing
//            if (cbLockRatio.Checked)
//            {
//                nUDWidth.Value = (decimal)((float)nUDHeight.Value * ratio);
//            }
//            userAdjust();
//        }

//        private void nUDReso_ValueChanged(object sender, EventArgs e)
//        {
//            if (adjustedImage == null) return;//if no image, do nothing
//            userAdjust();
//        }

//        private void cbGrayscale_CheckedChanged(object sender, EventArgs e)
//        {
//            if (cbGrayscale.Checked)
//                originalImage = imgGrayscale(originalImage);
//            else
//            {   //if (fileLoaded)
//               //     originalImage = new Bitmap(Image.FromFile(lastFile));
//               // else
//               //     originalImage = new Bitmap(Properties.Resources.modell);
//                originalImage = new Bitmap(loadedImage);
//            }
//            adjustedImage = new Bitmap(originalImage);
//            userAdjust();
//        }

//        private void GCodeFromImage_FormClosing(object sender, FormClosingEventArgs e)
//        {
//            Properties.Settings.Default.locationImageForm = Location;
//        }

//        private void GCodeFromImage_KeyDown(object sender, KeyEventArgs e)
//        {
//            if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
//            {
//                loadClipboard();
//                e.SuppressKeyPress = true;
//            }
//        }

//        private Point oldPoint = new Point(0,0);
//        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
//        {
//            if ((e.Location != oldPoint) || (e.Button == MouseButtons.Left))
//            {
//                Color clr = GetColorAt(e.Location);
//                if (e.Button == MouseButtons.Left)
//                {
//                    int i = toolTable.getToolNr(clr, (int)nUDMode.Value);
//                    lblStatus.Text = clr.ToString() + " = " + toolTable.getToolName(i);
//                    cbExceptColor.BackColor = clr;
//                }
//                float zoom = (float)nUDWidth.Value / pictureBox1.Width;
//                //        toolTip1.SetToolTip(pictureBox1, (e.X * zoom).ToString() + "  " + (e.Y * zoom).ToString());
//                toolTip1.SetToolTip(pictureBox1, (e.X * zoom).ToString() + "  " + (e.Y * zoom).ToString() + "   " +clr.ToString());
//                oldPoint = e.Location;
//            }
//        }

//        private void cbExceptColor_CheckedChanged(object sender, EventArgs e)
//        {
//            if (cbExceptColor.Checked)
//                toolTable.setExceptionColor(cbExceptColor.BackColor);
//            else
//                toolTable.clrExceptionColor();
//        }

//        private Color GetColorAt(Point point)
//        {
//            float zoom = ((float)nUDWidth.Value / (float)nUDReso.Value) / pictureBox1.Width  ;
//            int x = (int)(point.X * zoom);
//            if (x < 0) x = 0;
//            if (x >= adjustedImage.Width-1) x = adjustedImage.Width-1;
//            int y = (int)(point.Y * zoom);  //(adjustedImage.Height - 1) - (int)(point.Y * zoom);
//            if (y < 0) y = 0;
//            if (y >= adjustedImage.Height-1) y = adjustedImage.Height - 1;
//            return adjustedImage.GetPixel(x, y);
//        }
//    }
//}
