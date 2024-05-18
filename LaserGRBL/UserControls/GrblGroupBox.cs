using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaserGRBL.UserControls
{
    public class GrblGroupBox: GroupBox
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(ColorScheme.FormBackColor);
            Rectangle bounds2 = e.ClipRectangle;
            Rectangle bounds = e.ClipRectangle;
            bounds2.Width -= 8;
            Font font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
            Size size = TextRenderer.MeasureText(e.Graphics, Text, font, new Size(bounds2.Width, bounds2.Height), TextFormatFlags.Default);
            bounds2.Width = size.Width;
            bounds2.Height = size.Height;
            bounds2.X += 8;
            TextRenderer.DrawText(e.Graphics, Text, font, bounds2, ColorScheme.FormForeColor, TextFormatFlags.Default);
            if (bounds2.Width > 0)
            {
                bounds2.Inflate(2, 0);
            }
            Pen pen = new Pen(ColorScheme.ControlsBorder);
            int num = bounds.Top + font.Height / 2;
            e.Graphics.DrawLine(pen, bounds.Left, num, bounds.Left, bounds.Height - 1);
            e.Graphics.DrawLine(pen, bounds.Left, bounds.Height - 1, bounds.Width - 1, bounds.Height - 1);
            e.Graphics.DrawLine(pen, bounds.Left, num, bounds2.X, num);
            e.Graphics.DrawLine(pen, bounds2.X + bounds2.Width + 1, num, bounds.Width - 1, num);
            e.Graphics.DrawLine(pen, bounds.Width - 1, num, bounds.Width - 1, bounds.Height - 1);
            pen.Dispose();
            
        }
    }
}
