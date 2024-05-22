//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using LaserGRBL.Icons;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LaserGRBL.UserControls
{

    [System.ComponentModel.DefaultEvent("Click")]
    public partial class ImageButton : System.Windows.Forms.UserControl
    {
        private const float CAPTION_FONTSIZE = 7f;
        private const int CAPTION_HEIGHT = 12;
        private const int CLICK_SCALE_IN_PIXEL = 1;

        #region " Codice generato da Progettazione Windows Form "

        public ImageButton()
            : base()
        {
            EnabledChanged += ImageButtonEnabledChanged;
            MouseDown += ImageButtonMouseDown;
            MouseUp += ImageButtonMouseUp;
            MouseLeave += ImageButtonMouseLeave;
            MouseEnter += ImageButtonMouseEnter;

            //Chiamata richiesta da Progettazione Windows Form.
            InitializeComponent();

            //Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent()
            Init();

            this.Cursor = Cursors.Hand;
        }

        #endregion

        private void Init()
        {
            SetStyle(System.Windows.Forms.ControlStyles.UserPaint, true);
            SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(System.Windows.Forms.ControlStyles.DoubleBuffer, true);
            SetStyle(System.Windows.Forms.ControlStyles.ResizeRedraw, true);
            SetStyle(System.Windows.Forms.ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(System.Windows.Forms.ControlStyles.ContainerControl, true);
            this.BackColor = Color.FromArgb(0, 0, 0, 0);
        }

        private Image _image;
        public virtual Image Image
        {
            get { return _image; }
            set
            {
                _image = value;
                SizingMode = SizingMode;
                Invalidate();
            }
        }

        public string Caption { get; set; }

        private bool HasCaption
        {
            get { return !string.IsNullOrEmpty(Caption); }
        }

        private Image _altimage;
        public virtual Image AltImage
        {
            get { return _altimage; }
            set
            {
                _altimage = value;
                SizingMode = SizingMode;
                Invalidate();
            }
        }

        private bool _UseAltImage;
        public bool UseAltImage
        {
            get { return _UseAltImage; }
            set
            {
                _UseAltImage = value;
                Invalidate();
            }
        }

        public enum SizingModes
        {
            StretchImage,
            FixedSize
        }

        private SizingModes _sizingmode = SizingModes.FixedSize;
        public SizingModes SizingMode
        {
            get { return _sizingmode; }
            set
            {
                _sizingmode = value;
                if (SizingMode == SizingModes.FixedSize && (Image != null))
                {
                    this.Size = new Size(Image.Width + 1, Image.Height + 1);
                }
            }
        }

        public bool RoundedBorders { get; set; } = false;

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);

            Image touse = UseAltImage ? AltImage : Image;


            if ((touse != null) & this.Visible)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

                Image Tmp = (Image)touse.Clone();
                Tmp.Tag = touse.Tag;

                Point Point = new Point(0, 0);
                Size Size = new Size(Image.Width, Image.Height);

                if (SizingMode == SizingModes.StretchImage)
                {
                    Size = new Size(this.Width - 1, this.Height - 1);
                }

                if (IconsMgr.LegacyIcons)
                {

                    if (HasCaption)
                    {
                        Size.Height -= CAPTION_HEIGHT;
                        Size.Width -= CAPTION_HEIGHT;
                        Point.X += (Image.Width - Size.Width) / 2;
                    }


                    if (DrawDisabled())
                    {
                        //Disabilitato
                        Tmp = Base.Drawing.ImageTransform.GrayScale(Tmp, Base.Drawing.ImageTransform.Formula.CCIRRec709);
                        Tmp = Base.Drawing.ImageTransform.Brightness(Tmp, 0.11F);
                    }
                    else
                    {

                        if (IsMouseInside())
                        {
                            if (MouseButtons == System.Windows.Forms.MouseButtons.Left)
                            {
                                //Contenuto con mouse premuto
                                Tmp = Base.Drawing.ImageTransform.Brightness(Tmp, 0.15F);
                                Point = new Point(Point.X + CLICK_SCALE_IN_PIXEL, CLICK_SCALE_IN_PIXEL);
                                Size.Width -= CLICK_SCALE_IN_PIXEL * 2;
                                Size.Height -= CLICK_SCALE_IN_PIXEL * 2;
                            }
                            else
                            {
                                //Contenuto con mouse non premuto
                                Tmp = Base.Drawing.ImageTransform.Brightness(Tmp, 0.1F);
                            }
                        }
                        else
                        {
                            //Non contenuto
                            Tmp = Base.Drawing.ImageTransform.Brightness(Tmp, 0);
                        }
                    }

                    if ((Tmp != null))
                    {
                        e.Graphics.DrawImage(Tmp, new Rectangle(Point, Size));
                    }

                    if (this.HasCaption)
                    {
                        StringFormat sf = new StringFormat()
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Center,
                            FormatFlags = StringFormatFlags.LineLimit
                        };

                        using (Font captionFont = new Font("Microsoft Sans Serif", CAPTION_FONTSIZE))
                        {
                            float textY = Height - CAPTION_HEIGHT - 4;

                            using (Brush b = new SolidBrush(ForeColor))
                                e.Graphics.DrawString(Caption, captionFont, b, new RectangleF(0f, textY, Width, Height - textY), sf);
                        }
                    }
                }
                else
                {
                    float direction = ColorScheme.DarkScheme ? 1 : -1;
                    Color borderColor = ColorScheme.ControlsBorder;
                    if (Tmp != null)
                    {
                        LoadedImageTag tag = Tmp.Tag as LoadedImageTag;
                        borderColor = tag?.Color ?? ColorScheme.ControlsBorder;
                        if (DrawDisabled())
                        {
                            Tmp = Base.Drawing.ImageTransform.SetColor(Tmp, ColorScheme.DisabledButtons);
                            borderColor = ColorScheme.DisabledButtons;
                        }
                        else
                        {
                            if (IsMouseInside())
                            {
                                if (MouseButtons == MouseButtons.Left)
                                {
                                    borderColor = ColorScheme.ChangeColorBrightness(borderColor, 0.3F * direction);
                                    Tmp = Base.Drawing.ImageTransform.Brightness(Tmp, 0.3F * direction);
                                    Point = new Point(Point.X + CLICK_SCALE_IN_PIXEL, CLICK_SCALE_IN_PIXEL);
                                    Size.Width -= CLICK_SCALE_IN_PIXEL * 2;
                                    Size.Height -= CLICK_SCALE_IN_PIXEL * 2;
                                }
                                else
                                {
                                    borderColor = ColorScheme.ChangeColorBrightness(borderColor, 0.2F * direction);
                                    Tmp = Base.Drawing.ImageTransform.Brightness(Tmp, 0.2F * direction);
                                }
                            }
                            else
                            {
                                //Tmp = Base.Drawing.ImageTransform.Brightness(Tmp, 0);
                            }
                        }
                    }

                    e.Graphics.Clear(ColorScheme.FormBackColor);
                    if (RoundedBorders)
                    {
                        // draw rounded border
                        //borderColor = DrawDisabled() ? ColorScheme.DisabledButtons : ColorScheme.FormForeColor;
                        using (Brush brush = new SolidBrush(borderColor))
                        using (GraphicsPath path = Tools.Graph.RoundedRect(new Rectangle(Point, Size), 7))
                        {
                            e.Graphics.FillPath(brush, path);
                        }
                        // draw rounded background
                        Size.Width -= 6;
                        Size.Height -= 6;
                        Point.X += 3;
                        Point.Y += 3;
                        Color backColor = BackColor.A == 0 ? ColorScheme.FormBackColor : BackColor;
                        using (Brush brush = new SolidBrush(backColor))
                        using (GraphicsPath path = Tools.Graph.RoundedRect(new Rectangle(Point, Size), 5))
                        {
                            e.Graphics.FillPath(brush, path);
                        }
                        Size.Width -= 2;
                        Size.Height -= 2;
                        Point.X += 1;
                        Point.Y += 1;
                    }
                    else
                    {
                        // draw rounded background
                        using (Brush brush = new SolidBrush(BackColor))
                        using (GraphicsPath path = Tools.Graph.RoundedRect(new Rectangle(Point, Size), 5))
                        {
                            e.Graphics.FillPath(brush, path);
                        }
                    }
                    if ((Tmp != null))
                    {
                        e.Graphics.DrawImage(Tmp, new Rectangle(Point, Size));
                    }
                }

            }

        }

        public virtual bool IsMouseInside()
        {
            return RectangleToScreen(ClientRectangle).Contains(System.Windows.Forms.Cursor.Position);
        }

        protected virtual bool DrawDisabled()
        {
            return !Enabled;
        }

        private void ImageButtonMouseEnter(object sender, System.EventArgs e)
        {
            Invalidate();
        }

        private void ImageButtonMouseLeave(object sender, System.EventArgs e)
        {
            Invalidate();
        }

        private void ImageButtonMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Invalidate();
        }

        private void ImageButtonMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Invalidate();
        }

        private void ImageButtonEnabledChanged(object sender, System.EventArgs e)
        {
            Invalidate();
        }

        protected override void OnControlAdded(System.Windows.Forms.ControlEventArgs e)
        {
            e.Control.MouseEnter += ImageButtonMouseEnter;
            e.Control.MouseLeave += ImageButtonMouseLeave;
            e.Control.MouseUp += ImageButtonMouseUp;
            e.Control.MouseDown += ImageButtonMouseDown;
        }

        protected override void OnControlRemoved(System.Windows.Forms.ControlEventArgs e)
        {
            e.Control.MouseEnter -= ImageButtonMouseEnter;
            e.Control.MouseLeave -= ImageButtonMouseLeave;
            e.Control.MouseUp -= ImageButtonMouseUp;
            e.Control.MouseDown -= ImageButtonMouseDown;
        }

    }



}
