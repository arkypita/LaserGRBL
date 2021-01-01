//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

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


		private Color _coloration = Color.Empty;
		public Color Coloration
		{
			get { return _coloration; }
			set { _coloration = value; }
		}

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			base.OnPaint(e);

			System.Drawing.Image touse = UseAltImage ? AltImage : Image;


			if ((touse != null) & this.Visible)
			{
				e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
				e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

				Image Tmp = (Image)touse.Clone();

				Point Point = new Point(0, 0);
				Size Size = new Size(Image.Width, Image.Height);

				if (SizingMode == SizingModes.StretchImage)
				{
					Size = new Size(this.Width - 1, this.Height - 1);
				}

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
					//Tmp = Base.Drawing.ImageTransform.ChangeAlpha(Tmp, 150);
				}
				else
                {
                    if (!Coloration.Equals(Color.Empty))
                    {
                        Tmp = Base.Drawing.ImageTransform.GrayScale(Tmp, Base.Drawing.ImageTransform.Formula.CCIRRec709);
                        Tmp = Base.Drawing.ImageTransform.Brightness(Tmp, -0.5F);
                        Tmp = Base.Drawing.ImageTransform.Translate(Tmp, Coloration, 0);
                    }

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

