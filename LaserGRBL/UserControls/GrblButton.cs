using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Tools;
using static LaserGRBL.UserControls.ColorSlider;

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
            e.Graphics.Clear(ColorScheme.FormBackColor);
            Color backColor = ColorScheme.LogBackColor;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            if (mHover) backColor = ColorScheme.ChangeColorBrightness(backColor, -0.15f);
            if (mPressed) backColor = ColorScheme.ChangeColorBrightness(backColor, -0.15f);
            Rectangle borderRect = e.ClipRectangle;
            borderRect.Width -= 1; borderRect.Height -= 1;
            using (Brush backColorBrush = new SolidBrush(backColor))
            using (Brush foreColorBrush = new SolidBrush(ColorScheme.FormForeColor))
            using (Font font = new Font(Font, Font.Style))
            using (Pen borderPen = new Pen(ColorScheme.ControlsBorder))
            using (GraphicsPath backRect = Graph.RoundedRect(borderRect, 5))
            {
                e.Graphics.FillPath(backColorBrush, backRect);
                e.Graphics.DrawPath(borderPen, backRect);
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
