using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaserGRBL
{
    public partial class AxiiRescale : Form
    {
        private GrblCore Core;
        private double sourceWidth;
        private double sourceHeight;
        private double tableWidth;
        private double tableHeight;
        private double maxRatio;

        public AxiiRescale(GrblCore core)
        {
            InitializeComponent();
            Core = core;
            sourceWidth = (double)Core.LoadedFile.Range.MovingRange.Width;
            sourceHeight = (double)Core.LoadedFile.Range.MovingRange.Height;
            tableWidth = (double)Core.Configuration.TableWidth;
            tableHeight = (double)Core.Configuration.TableHeight;
            XTextBox.Text = Double2String(sourceWidth);
            YTextBox.Text = Double2String(sourceHeight);
            maxRatio = Math.Min(tableWidth / sourceWidth, tableHeight / sourceHeight);
            SizeValidation();
        }

        internal static void CreateAndShowDialog(GrblCore core)
        {
            using (AxiiRescale sf = new AxiiRescale(core))
            {
                sf.ShowDialog();
            }
        }

        private string Double2String(double v)
        {
            string s = v.ToString("F3", CultureInfo.InvariantCulture);
            return s.Contains(".") ? s.TrimEnd('0').TrimEnd('.') : s;
        }

        private double String2Double(string v)
        {
            double d = 0D;
            double.TryParse(v.Replace(",", "."), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out d);
            return d;
        }

        private void SizeValidation()
        {
            if (String2Double(XTextBox.Text) > tableWidth)
            {
                if (RatioLockCheckBox.Checked)
                {
                    XTextBox.Text = Double2String(sourceWidth * maxRatio);
                    YTextBox.Text = Double2String(sourceHeight * maxRatio);
                }
                else
                    XTextBox.Text = Double2String(tableWidth);
            }
            if (String2Double(YTextBox.Text) > tableHeight) {
                if (RatioLockCheckBox.Checked)
                {
                    XTextBox.Text = Double2String(sourceWidth * maxRatio);
                    YTextBox.Text = Double2String(sourceHeight * maxRatio);
                }
                else
                    YTextBox.Text = Double2String(tableHeight);
            }
        }

        private void HalfSizeButton_Click(object sender, EventArgs e)
        {
            XTextBox.Text = Double2String(sourceWidth / 2);
            YTextBox.Text = Double2String(sourceHeight / 2);
            SizeValidation();
        }

        private void OriginalSizeButton_Click(object sender, EventArgs e)
        {
            XTextBox.Text = Double2String(sourceWidth);
            YTextBox.Text = Double2String(sourceHeight);
            SizeValidation();
        }

        private void DoubleSizeButton_Click(object sender, EventArgs e)
        {
            XTextBox.Text = Double2String(sourceWidth * 2);
            YTextBox.Text = Double2String(sourceHeight * 2);
            SizeValidation();
        }

        private void RatioLockCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (RatioLockCheckBox.Checked)
            {
                YTextBox.Enabled = false;
            }
            else
            {
                XTextBox.Enabled = true;
                YTextBox.Enabled = true;
            }
        }

        private void TextBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            switch (((TextBox)sender).Name)
            {
                case "XTextBox":
                    if (!YTextBox.Enabled)
                    {
                        XTextBox.Enabled = false;
                        YTextBox.Enabled = true;
                    }
                    break;
                case "YTextBox":
                    if (!XTextBox.Enabled)
                    {
                        YTextBox.Enabled = false;
                        XTextBox.Enabled = true;
                    }
                    break;
            }
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (RatioLockCheckBox.Checked)
            {
                double ratio;
                switch (((TextBox)sender).Name)
                {
                    case "XTextBox":
                        ratio = String2Double(XTextBox.Text) / sourceWidth;
                        YTextBox.Text = Double2String(sourceHeight * ratio);
                        break;
                    case "YTextBox":
                        ratio = String2Double(YTextBox.Text) / sourceHeight;
                        XTextBox.Text = Double2String(sourceWidth * ratio);
                        break;
                }
                SizeValidation();
            }
        }

        private void NumbersValidation(object sender, CancelEventArgs e)
        {
            e.Cancel = !double.TryParse(((TextBox)sender).Text.Replace(",", "."), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out _);
            SizeValidation();
        }

        private void RescaleButton_Click(object sender, EventArgs e)
        {
            double XRatio = String2Double(XTextBox.Text) / sourceWidth;
            double YRatio = String2Double(YTextBox.Text) / sourceHeight;
            Core.LoadedFile.AxiiRescale(XRatio, YRatio);
            Dispose();
        }
    }
}
