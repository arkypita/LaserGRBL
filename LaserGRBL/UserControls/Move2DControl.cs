using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace LaserGRBL.UserControls
{
	public class Move2DControl : UserControl
	{
		#region SubClass
		public class MouvementZone
		{
			public MouvementZone(GraphicsPath path, Point mouvement, Color color)
			{
				Path = path;
				Mouvement = mouvement;
				Color = color;
			}

			public GraphicsPath Path { get; private set; }
			public Point Mouvement { get; private set; }
			public Color Color { get; private set; }

			public GrblCore.JogDirection Direction
			{
				get
				{
					if(IsUp)
					{
						if (IsLeft)
							return GrblCore.JogDirection.NW;
						if (IsRight)
							return GrblCore.JogDirection.NE;
						return GrblCore.JogDirection.N;
					}
					if (IsDown)
					{
						if (IsLeft)
							return GrblCore.JogDirection.SW;
						if (IsRight)
							return GrblCore.JogDirection.SE;
						return GrblCore.JogDirection.S;
					}
					if (IsLeft)
						return GrblCore.JogDirection.W;
					return GrblCore.JogDirection.E;
				}
			}
			public int Speed
			{
				get { return IsHome ? 0 : (int)Math.Max(Math.Abs(Mouvement.X), Math.Abs(Mouvement.Y)); }
			}
			public bool HitTest(Point pt)
			{
				return Path.IsVisible(pt);
			}


			public bool IsHome { get { return Mouvement.IsEmpty; } }
			public bool IsLeft { get { return !IsHome && Mouvement.X < 0; } }
			public bool IsRight { get { return !IsHome && Mouvement.X > 0; } }

			public bool IsUp { get { return !IsHome && Mouvement.Y > 0; } }
			public bool IsDown { get { return !IsHome && Mouvement.Y < 0; } }


			public bool Draw(Graphics g, Point mouseLocation, Brush hlBrush, Pen hlPen)
			{
				bool hit = HitTest(mouseLocation);
				using (var b = new SolidBrush(Color))
					g.FillPath(b, Path);
				if (hit)
				{
					if (hlBrush != null)
						g.FillPath(hlBrush, Path);
					if (hlPen != null)
						g.DrawPath(hlPen, Path);
				}
				return hit;
			}

		}
		#endregion

		#region ctor
		public Move2DControl()
		{
			InitializeComponent();
			ResizeRedraw = true;
		}
		#endregion

		#region Member and properties
		List<MouvementZone> Zones;
		#endregion

		#region Events
		private void Move2DControl_Paint(object sender, PaintEventArgs e)
		{
			//	Invalidate();

			int size = Math.Min(Width, Height);
			Rectangle rect = new Rectangle(0, 0, size, size);
			rect.X = Padding.Left + (Width - size) / 2;
			rect.Y = Padding.Top + (Height - size) / 2;
			rect.Width -= Padding.Horizontal;
			rect.Height -= Padding.Vertical;

			int shadowOffset = size / 50;

			rect.Width -= shadowOffset;
			rect.Height -= shadowOffset;




			Graphics g = e.Graphics;
			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;


			float outerRadius = rect.Width / 2f;
			float innerRadius = outerRadius / 3f;
			PointF center = new PointF(rect.X + rect.Width / 2f, rect.Y + rect.Width / 2f);

			Zones = CreateMouvementList(center, innerRadius, outerRadius);


			#region Background
			GraphicsPath bg = new GraphicsPath();
			bg.AddEllipse(rect);
			using (var b = new SolidBrush(Color.White))
				PaintWithShadow(g, bg, b, shadowOffset);
			#endregion

			string info = string.Empty;
			Point local = this.PointToClient(Cursor.Position);

			MouvementZone hit = null;
			int nalpha = this.Enabled ? 128 : 64;
			int halpha = 255;

			using (var hb = new SolidBrush(Color.FromArgb(128, SystemColors.Highlight)))
			{
				foreach (var m in Zones)
				{
					if (m.Draw(g, local, Enabled ? hb : null, null))
					{
						if (Enabled)
						{
							hit = m;
							if (m.IsHome)
								info = "Home";
							else
								info = String.Format("X{0,5 }\r\nY{1,5 }", m.Mouvement.X, m.Mouvement.Y);
						}
					}
				}
			}

			if (!string.IsNullOrEmpty(info))
			{
				var s = g.MeasureString(info, this.Font);
				g.DrawString(info, Font, Brushes.Black, new PointF(Width - Padding.Right - s.Width, Height - Padding.Bottom - s.Height));
			}

			#region icons
			var upArrow = GetUpArrow(new PointF(rect.Left + rect.Width / 2, rect.Top + rect.Height / 6), rect.Height / 4);
			var bottomArrow = GetBottomArrow(new PointF(rect.Left + rect.Width / 2, rect.Bottom - rect.Height / 6), rect.Height / 4);
			var leftArrow = GetLeftArrow(new PointF(rect.Left + rect.Width / 6, rect.Top + rect.Height / 2), rect.Height / 4);
			var rightArrow = GetRigthArrow(new PointF(rect.Right - rect.Width / 6, rect.Top + rect.Height / 2), rect.Height / 4);
			var homeIcon = GetHomeIcon(center, innerRadius / 1.5f, shadowOffset / 2);

			using (Brush nb = new SolidBrush(Color.FromArgb(nalpha, Enabled ? SystemColors.Highlight : Color.LightGray)))
			using (var hb = new SolidBrush(Color.FromArgb(halpha, SystemColors.Highlight)))
			using (var np = new Pen(Color.FromArgb(nalpha, 0, 0, 0), shadowOffset / 2))
			using (var hp = new Pen(Color.FromArgb(halpha, 0, 0, 0), shadowOffset / 2))
			{
				hp.LineJoin = LineJoin.Round;
				np.LineJoin = LineJoin.Round;

				bool h = hit == null ? false : hit.IsHome;
				bool l = hit == null ? false : hit.IsLeft;
				bool r = hit == null ? false : hit.IsRight;
				bool u = hit == null ? false : hit.IsUp;
				bool d = hit == null ? false : hit.IsDown;


				PaintIcon(g, upArrow, u ? hb : nb, u ? hp : np, shadowOffset);
				PaintIcon(g, bottomArrow, d ? hb : nb, d ? hp : np, shadowOffset);
				PaintIcon(g, leftArrow, l ? hb : nb, l ? hp : np, shadowOffset);
				PaintIcon(g, rightArrow, r ? hb : nb, r ? hp : np, shadowOffset);
				PaintIcon(g, homeIcon, h ? hb : nb, h ? hp : np, shadowOffset);
			}
			#endregion
		}
		private void Move2DControl_MouseMove(object sender, MouseEventArgs e)
		{
			Invalidate();
		}
		private void MoveHearControl_Click(object sender, EventArgs e)
		{
			if (Enabled && Zones != null)
			{
				Point local = this.PointToClient(Cursor.Position);
				foreach (var m in Zones)
				{
					if (m.HitTest(local))
					{
						if (m.Mouvement.IsEmpty)
						{
							if (this.HomeClick != null)
								this.HomeClick(this, e);
						}
						else
						{
							if (this.MoveClick != null)
								this.MoveClick(this, new MoveEventArgs(m));
						}
					}
				}
			}
		}
		#endregion

		#region MouvementZone generation
		private List<MouvementZone> CreateMouvementList(PointF center, float innerRadius, float outerRadius)
		{
			GraphicsPath home = new GraphicsPath();
			home.AddEllipse(new RectangleF(center.X - innerRadius, center.Y - innerRadius, innerRadius * 2f, innerRadius * 2f));
			List<MouvementZone> ret = new List<MouvementZone>();
			ret.Add(new MouvementZone(home, Point.Empty, ScaleColor(5)));
			ret.AddRange(CreateMouvementRange(center, innerRadius, outerRadius, 0, new Point(0, 1)));
			ret.AddRange(CreateMouvementRange(center, innerRadius, outerRadius, 45, new Point(1, 1)));
			ret.AddRange(CreateMouvementRange(center, innerRadius, outerRadius, 90, new Point(1, 0)));
			ret.AddRange(CreateMouvementRange(center, innerRadius, outerRadius, 135, new Point(1, -1)));
			ret.AddRange(CreateMouvementRange(center, innerRadius, outerRadius, 180, new Point(0, -1)));
			ret.AddRange(CreateMouvementRange(center, innerRadius, outerRadius, 225, new Point(-1, -1)));
			ret.AddRange(CreateMouvementRange(center, innerRadius, outerRadius, 270, new Point(-1, 0)));
			ret.AddRange(CreateMouvementRange(center, innerRadius, outerRadius, 315, new Point(-1, 1)));

			return ret;
		}
		private List<MouvementZone> CreateMouvementRange(PointF center, float innerRadius, float outerRadius, int angle, Point direction)
		{
			int numsteps = 6;
			float deltaRadius = (outerRadius - innerRadius) / numsteps;

			float gap = 0 * deltaRadius / 15;

			float radius1 = outerRadius;
			float radius2 = radius1 - deltaRadius;
			float radius3 = radius2 - deltaRadius;
			float radius4 = radius3 - deltaRadius;
			float radius5 = radius4 - deltaRadius;
			float radius6 = radius5 - deltaRadius;
			float radius7 = radius6 - deltaRadius;

			List<MouvementZone> ret = new List<MouvementZone>();
			ret.Add(CreateMouvement(center, radius1 - gap, radius2 + gap, angle, direction, 200, ScaleColor(200)));
			ret.Add(CreateMouvement(center, radius2 - gap, radius3 + gap, angle, direction, 100, ScaleColor(180)));
			ret.Add(CreateMouvement(center, radius3 - gap, radius4 + gap, angle, direction, 50, ScaleColor(160)));
			ret.Add(CreateMouvement(center, radius4 - gap, radius5 + gap, angle, direction, 10, ScaleColor(140)));
			ret.Add(CreateMouvement(center, radius5 - gap, radius6 + gap, angle, direction, 5, ScaleColor(120)));
			ret.Add(CreateMouvement(center, radius6 - gap, radius7 + gap, angle, direction, 1, ScaleColor(100)));

			return ret;
		}
		public MouvementZone CreateMouvement(PointF center, float innerRadius, float outerRadius, int angle, Point direction, int mouvementFactor, Color color)
		{
			var mouvement = new Point(direction.X * mouvementFactor, direction.Y * mouvementFactor);
			float sweepAngle = 44;
			float startAngle = angle - sweepAngle / 2;

			RectangleF innerRectangle = new RectangleF(center.X - innerRadius, center.Y - innerRadius, innerRadius * 2, innerRadius * 2);
			RectangleF outerRectangle = new RectangleF(center.X - outerRadius, center.Y - outerRadius, outerRadius * 2, outerRadius * 2);

			GraphicsPath p = new GraphicsPath();
			PointF iStart = GetPeriferical(startAngle, center, innerRadius);
			PointF oStart = GetPeriferical(startAngle, center, outerRadius);
			p.AddLine(iStart, oStart);
			p.AddArc(outerRectangle, startAngle - 90, sweepAngle);

			PointF iEnd = GetPeriferical(startAngle + sweepAngle, center, innerRadius);
			PointF oEnd = GetPeriferical(startAngle + sweepAngle, center, outerRadius);
			p.AddLine(oEnd, iEnd);
			p.AddArc(innerRectangle, startAngle - 90 + sweepAngle, -sweepAngle);

			p.CloseFigure();

			return new MouvementZone(p, mouvement, color);
		}
		#endregion

		#region Graphical Paths
		GraphicsPath GetUpArrow(PointF center, float size)
		{

			GraphicsPath p = new GraphicsPath();
			float u = size / 10f;
			var pts = new List<PointF>();
			float x = center.X - size / 2;
			float y = center.Y - size / 2;
			pts.Add(new PointF(x + u * 5, y + u * 0));
			pts.Add(new PointF(x + u * 10, y + u * 5));
			pts.Add(new PointF(x + u * 8, y + u * 5));
			pts.Add(new PointF(x + u * 8, y + u * 10));
			pts.Add(new PointF(x + u * 2, y + u * 10));
			pts.Add(new PointF(x + u * 2, y + u * 5));
			pts.Add(new PointF(x + u * 0, y + u * 5));
			pts.Add(new PointF(x + u * 5, y + u * 0));
			p.AddPolygon(pts.ToArray());
			//			p.AddRectangle(new RectangleF(x, y, size, size));
			return p;
		}
		GraphicsPath GetBottomArrow(PointF center, float size)
		{

			GraphicsPath p = new GraphicsPath();
			float u = size / 10f;
			var pts = new List<PointF>();
			float x = center.X - size / 2;
			float y = center.Y - size / 2;
			pts.Add(new PointF(x + u * 5, y + u * 10));
			pts.Add(new PointF(x + u * 0, y + u * 5));
			pts.Add(new PointF(x + u * 2, y + u * 5));
			pts.Add(new PointF(x + u * 2, y + u * 0));
			pts.Add(new PointF(x + u * 8, y + u * 0));
			pts.Add(new PointF(x + u * 8, y + u * 5));
			pts.Add(new PointF(x + u * 10, y + u * 5));
			pts.Add(new PointF(x + u * 5, y + u * 10));
			p.AddPolygon(pts.ToArray());
			//			p.AddRectangle(new RectangleF(x, y, size, size));
			return p;
		}
		GraphicsPath GetLeftArrow(PointF center, float size)
		{

			GraphicsPath p = new GraphicsPath();
			float u = size / 10f;
			var pts = new List<PointF>();
			float x = center.X - size / 2;
			float y = center.Y - size / 2;
			pts.Add(new PointF(x + u * 0, y + u * 5));
			pts.Add(new PointF(x + u * 5, y + u * 0));
			pts.Add(new PointF(x + u * 5, y + u * 2));
			pts.Add(new PointF(x + u * 10, y + u * 2));
			pts.Add(new PointF(x + u * 10, y + u * 8));
			pts.Add(new PointF(x + u * 5, y + u * 8));
			pts.Add(new PointF(x + u * 5, y + u * 10));
			pts.Add(pts[0]);
			p.AddPolygon(pts.ToArray());
			//			p.AddRectangle(new RectangleF(x, y, size, size));
			return p;
		}
		GraphicsPath GetRigthArrow(PointF center, float size)
		{

			GraphicsPath p = new GraphicsPath();
			float u = size / 10f;
			var pts = new List<PointF>();
			float x = center.X - size / 2;
			float y = center.Y - size / 2;
			pts.Add(new PointF(x + u * 10, y + u * 5));
			pts.Add(new PointF(x + u * 5, y + u * 0));
			pts.Add(new PointF(x + u * 5, y + u * 2));
			pts.Add(new PointF(x + u * 0, y + u * 2));
			pts.Add(new PointF(x + u * 0, y + u * 8));
			pts.Add(new PointF(x + u * 5, y + u * 8));
			pts.Add(new PointF(x + u * 5, y + u * 10));
			pts.Add(pts[0]);
			p.AddPolygon(pts.ToArray());
			//			p.AddRectangle(new RectangleF(x, y, size, size));
			return p;
		}


		GraphicsPath GetHomeIcon(PointF center, float size, int shadowOffset)
		{
			GraphicsPath home = new GraphicsPath();
			float x = center.X - size - shadowOffset / 2f;
			float y = center.Y - size - shadowOffset / 2f;

			float u = size / 50f;
			var pts = new List<PointF>();

			pts.Add(new PointF(x + u * 50, y + u * 00));
			pts.Add(new PointF(x + u * 100, y + u * 50));
			pts.Add(new PointF(x + u * 95, y + u * 55));
			pts.Add(new PointF(x + u * 90, y + u * 55));
			pts.Add(new PointF(x + u * 90, y + u * 55));
			pts.Add(new PointF(x + u * 90, y + u * 100));
			pts.Add(new PointF(x + u * 60, y + u * 100));
			pts.Add(new PointF(x + u * 60, y + u * 60));
			pts.Add(new PointF(x + u * 40, y + u * 60));
			pts.Add(new PointF(x + u * 40, y + u * 100));
			pts.Add(new PointF(x + u * 10, y + u * 100));
			pts.Add(new PointF(x + u * 10, y + u * 55));
			pts.Add(new PointF(x + u * 05, y + u * 55));
			pts.Add(new PointF(x + u * 00, y + u * 50));
			pts.Add(new PointF(x + u * 50, y + u * 00));
			home.AddPolygon(pts.ToArray());
			return home;
		}

		#endregion

		#region Custom Event
		public delegate void MoveEventHandler(object sender, MoveEventArgs e);
		public class MoveEventArgs : EventArgs
		{
			public MoveEventArgs(MouvementZone move)
				: base()
			{
				Move = move;
			}

			public readonly MouvementZone Move;
		}
		public event MoveEventHandler MoveClick;
		public event EventHandler HomeClick;
		#endregion

		#region utils
		Color ScaleColor(int step)
		{
			int a = step * 2 / 3;
			Color c = this.ForeColor;
			if (!Enabled)
			{
				int val = (c.R + c.G + c.B) / 3;
				return Color.FromArgb(a / 2, val, val, val);

			}
			else
				return Color.FromArgb(a, c);
		}
		public PointF GetPeriferical(double angle, PointF center, float radius)
		{
			double rad = angle * System.Math.PI / 180;
			return new PointF(center.X + (float)Math.Sin(rad) * radius, center.Y - (float)Math.Cos(rad) * radius);

		}

		public void PaintWithShadow(Graphics g, GraphicsPath path, Brush brush, int shadowOffset)
		{
			int microstep = 2;
			if (shadowOffset > microstep)
			{
				/*
				using (PathGradientBrush _Brush = new PathGradientBrush(path))
				{
					ColorBlend _ColorBlend = new ColorBlend(3);
					_ColorBlend.Colors = new Color[]{Color.Transparent,
					Color.FromArgb(180, Color.Black),
					Color.FromArgb(180, Color.DimGray)};
					_ColorBlend.Positions = new float[] { 0f, .5f, 1f };

					_Brush.WrapMode = WrapMode.Clamp;
					_Brush.InterpolationColors = _ColorBlend;
					g.TranslateTransform(shadowOffset, shadowOffset);
					g.FillPath(_Brush, path);
					g.TranslateTransform(-shadowOffset, -shadowOffset);
				}*/
				using (var _Brush = new SolidBrush(Color.FromArgb(microstep * 128 / shadowOffset, 0, 0, 0)))
				{
					var oldClip = g.Clip;
					var oldransform = g.Transform;
					g.Clip = new Region();
					g.SetClip(path, CombineMode.Exclude);
					for (int i = 0; i < shadowOffset / microstep; i++)
					{
						g.TranslateTransform(microstep, microstep);
						g.FillPath(_Brush, path);
					}
					g.Transform = oldransform;
					g.Clip = oldClip;
				}

			}
			g.FillPath(brush, path);
		}

		public void PaintIcon(Graphics g, GraphicsPath path, Brush brush, Pen pen, int shadowOffset)
		{

			PaintWithShadow(g, path, brush, shadowOffset);
			if (pen != null)
				g.DrawPath(pen, path);
		}

		#endregion

		#region Editor Designer stuff
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// MoveHearControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.DoubleBuffered = true;
			this.Name = "Move2DControl";
			this.Padding = new System.Windows.Forms.Padding(2);
			this.Size = new System.Drawing.Size(228, 217);
			this.Click += new System.EventHandler(this.MoveHearControl_Click);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.Move2DControl_Paint);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Move2DControl_MouseMove);
			this.ResumeLayout(false);

		}

		#endregion
		#endregion
	}
}
