using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaserGRBL.UserControls
{
	public partial class UsageClassPreview : UserControl
	{
		private LaserLifeHandler.LaserLifeCounter mLLC;
		private RectangleF[] Rectangles = new RectangleF[10];

		private static Color[] mCol =
		{
			Color.FromArgb(0x22,0x3D,0xFA),
			Color.FromArgb(0x6B,0xB0,0xFC),
			Color.FromArgb(0x90,0xFF,0xE4),
			Color.FromArgb(0x90,0xFF,0x43),
			Color.FromArgb(0xC3,0xFF,0x3C),
			Color.FromArgb(0xF8,0xFF,0x3C),
			Color.FromArgb(0xED,0xB6,0x2B),
			Color.FromArgb(0xE1,0x70,0x1A),
			Color.FromArgb(0XDD,0X3D,0X07),
			Color.FromArgb(0xDB,0x00,0x00)
		};

		public UsageClassPreview()
		{
			InitializeComponent();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			using (Pen p = new Pen(Color.DarkGray))
			{
				p.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
				for (int i = 1; i < 10; i++)
				{
					float y = (Height - 1) / 10.0f * i;
					e.Graphics.DrawLine(p, 0, y, Width, y);
				}
			}

			if (mLLC != null)
			{
				double max =  mLLC.Classes.Max().TotalHours * 1.05;
				if (max > 0)
				{
					for (int i = 0; i< mLLC.Classes.Length; i++)
					{
						using (Brush b = new SolidBrush(Color.FromArgb(200, mCol[i])))
							e.Graphics.FillRectangle(b, Rectangles[i]);

						using (Pen p = new Pen(ColorScheme.ChangeColorBrightness(mCol[i], -0.3f)))
							e.Graphics.DrawRectangle(p, Rectangles[i].X, Rectangles[i].Y, Rectangles[i].Width, Rectangles[i].Height);
					}
				}
			}

			e.Graphics.DrawRectangle(Pens.DarkGray , 0, 0, Width -1, Height	-1);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			string tt = null;
			for (int i = 0; i < Rectangles.Length; i++)
			{
				if (Rectangles[i].Contains((float)e.X, (float)e.Y))
					tt = $"{i * 10}-{(i + 1) * 10}%  {mLLC.Classes[i].TotalHours:0.0} h"; 
			}

			if (TTLab.GetToolTip(this) != tt)
				TTLab.SetToolTip(this, tt);
		}

		public LaserLifeHandler.LaserLifeCounter LifeCounter
		{
			get { return mLLC; }
			set
			{
				if (mLLC != value)
				{
					mLLC = value;
					ComputeRect();
					Invalidate();
				}
			}
		}

		private void ComputeRect()
		{
			if (mLLC != null)
			{
				double w = (double)(Width - 1) / mLLC.Classes.Length;
				double max = mLLC.Classes.Max().TotalHours * 1.05;
				if (max > 0)
				{
					for (int i = 0; i < mLLC.Classes.Length; i++)
					{
						float h = Math.Max(2, (float)(mLLC.Classes[i].TotalHours / max * Height));
						Rectangles[i] = new RectangleF((float)(i * w), Height - h - 1, (float)w, h);
					}
				}
			}
			else
			{
				Rectangles = new RectangleF[10];
			}
		}
	}
}
