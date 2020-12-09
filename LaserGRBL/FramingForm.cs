using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaserGRBL
{
    public partial class FramingForm : Form
    {
        private GrblCore Core;

        public FramingForm(GrblCore core)
        {
            InitializeComponent();
            Core = core;
        }

        internal static void CreateAndShowDialog(GrblCore core)
        {
            using (FramingForm sf = new FramingForm(core))
            {
                sf.SPowerNum.Minimum = core.Configuration.MinPWM;
                sf.SPowerNum.Maximum = core.Configuration.MaxPWM;
                sf.FeedrateNum.Maximum = core.Configuration.MaxRateX < core.Configuration.MaxRateY ? core.Configuration.MaxRateX : core.Configuration.MaxRateY;
                sf.ShowDialog();
            }
        }

        private void FramingButton_Click(object sender, EventArgs e)
        {
            if (RectangularFraming.Checked)
            {
                Core.EnqueueCommand(new GrblCommand("G0 X0 Y0"));
                Core.EnqueueCommand(new GrblCommand("M3 S" + SPowerNum.Value + " F" + FeedrateNum.Value));
                byte count = 0;
                do
                {
                    Core.EnqueueCommand(new GrblCommand("G1 Y" + YTextBox.Text));
                    Core.EnqueueCommand(new GrblCommand("G1 X" + XTextBox.Text));
                    Core.EnqueueCommand(new GrblCommand("G1 Y0"));
                    Core.EnqueueCommand(new GrblCommand("G1 X0"));
                    count++;
                } while (count < NPassesNum.Value);
                Core.EnqueueCommand(new GrblCommand("M5"));
            }
            else if (CircularFraming.Checked)
            {
                string r = Double2String(double.Parse(YTextBox.Text) / 2);
                Core.EnqueueCommand(new GrblCommand("G0 X0 Y" + r));
                Core.EnqueueCommand(new GrblCommand("M3 S" + SPowerNum.Value + " F" + FeedrateNum.Value));
                byte count = 0;
                do
                {
                    Core.EnqueueCommand(new GrblCommand("G2 Y" + r + " I" + r));
                    count++;
                } while (count < NPassesNum.Value);
                Core.EnqueueCommand(new GrblCommand("M5"));
                Core.EnqueueCommand(new GrblCommand("G0 X0 Y0"));
            }
            else if (EllipticFraming.Checked)
            {
                double step = 2 * Math.PI / 72;
                double rX = double.Parse(XTextBox.Text) / 2;
                double rY = double.Parse(YTextBox.Text) / 2;
                double centerX = rX;
                double centerY = rY;

                Core.EnqueueCommand(new GrblCommand("G0 X0 Y" + Double2String(double.Parse(YTextBox.Text) / 2)));
                Core.EnqueueCommand(new GrblCommand("M3 S" + SPowerNum.Value + " F" + FeedrateNum.Value));
                byte count = 0;
                do
                {
                    for (double theta = Math.PI; theta < 2 * Math.PI; theta += step)
                    {
                        double x = centerX + rX * Math.Cos(theta);
                        double y = centerY - rY * Math.Sin(theta);
                        Core.EnqueueCommand(new GrblCommand("G1 X" + Double2String(x) + " Y" + Double2String(y)));
                    }
                    for (double theta = 0; theta < Math.PI; theta += step)
                    {
                        double x = centerX + rX * Math.Cos(theta);
                        double y = centerY - rY * Math.Sin(theta);
                        Core.EnqueueCommand(new GrblCommand("G1 X" + Double2String(x) + " Y" + Double2String(y)));
                    }
                    count++;
                } while (count < NPassesNum.Value);
                Core.EnqueueCommand(new GrblCommand("G1 X0 Y" + Double2String(double.Parse(YTextBox.Text) / 2)));
                Core.EnqueueCommand(new GrblCommand("M5"));
                Core.EnqueueCommand(new GrblCommand("G0 X0 Y0"));
            }
        }

        private string Double2String(double v)
        {
            return v.ToString("F3", System.Globalization.CultureInfo.InvariantCulture).TrimEnd('0').TrimEnd('.');
        }

        private void NumbersValidation(object sender, CancelEventArgs e)
        {
            e.Cancel = !double.TryParse(((TextBox)sender).Text, out _);

            if (int.Parse(XTextBox.Text) > Core.Configuration.TableWidth)
                XTextBox.Text = Double2String((double)Core.Configuration.TableWidth);
            if (int.Parse(YTextBox.Text) > Core.Configuration.TableHeight)
                YTextBox.Text = Double2String((double)Core.Configuration.TableHeight);
        }

        private void CircleValidation(object sender, EventArgs e)
        {
            if (CircularFraming.Checked)
            {
                switch (((TextBox)sender).Name)
                {
                    case "XTextBox":
                        YTextBox.Text = XTextBox.Text;
                        break;
                    case "YTextBox":
                        XTextBox.Text = YTextBox.Text;
                        break;
                }
            }
        }

        private void CircularFramingChecked(object sender, EventArgs e)
        {
            YTextBox.Text = XTextBox.Text;
        }
    }
}
