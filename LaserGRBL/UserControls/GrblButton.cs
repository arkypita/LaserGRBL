using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Management.Instrumentation;
using System.Windows.Forms;

namespace LaserGRBL.UserControls
{
    public class GrblButton: Button
    {

        private bool mHover = false;
        private bool mPressed = false;

        public GrblButton() {
            SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Color backColor = ColorScheme.LogBackColor;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            if (mHover) backColor = ColorScheme.ChangeColorBrightness(backColor, -0.15f);
            if (mPressed) backColor = ColorScheme.ChangeColorBrightness(backColor, -0.15f);
            using (Brush backColorBrush = new SolidBrush(backColor))
            using (Brush foreColorBrush = new SolidBrush(ColorScheme.FormForeColor))
            using (Font font = new Font(Font, Font.Style))
            {
                e.Graphics.FillRectangle(backColorBrush, e.ClipRectangle);
                ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, ColorScheme.ControlsBorder, ButtonBorderStyle.Solid);
                int startX = 0;
                if (Image != null)
                {
                    startX = Height - 8;
                    Rectangle rect = new Rectangle(4, 4, Height - 8, Height - 8);
                    e.Graphics.DrawImage(Image, rect);
                }
                SizeF fontSize = e.Graphics.MeasureString(Text, font);
                e.Graphics.DrawString(Text, font, foreColorBrush, startX + (Width - fontSize.Width - startX) / 2, (Height - fontSize.Height) / 2);
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            mHover = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            mHover = false;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            mPressed = true;
            Invalidate();
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            mPressed = false;
            Invalidate();
        }


    }
}
