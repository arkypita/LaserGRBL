// draw rectangle move with mouse 
using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace LaserGRBL.UserControls
{
	public partial class MyPictureBox
	{
		public MyPictureBox()
		{
			ResizeRedraw = true;
			DoubleBuffered = true;
		}

		enum MouseStage { Handle = 3, Fence = 2, Outside = 1, NoAction = 0 }
		private MouseStage mStage = MouseStage.NoAction;
		private PointF MouseDownPt, MouseMovePt, MouseDownOffsetPt;
		private RectangleF FenceRect;

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			mStage = GetMouseOverType(e.X, e.Y);
			if (mStage == MouseStage.Outside)
			{
				MouseDownPt = e.Location;
				MouseMovePt = e.Location;
			}
			else if (mStage == MouseStage.Fence || mStage == MouseStage.Handle) // mouse down on rectangle or handle begine moving
			{
				MouseDownPt = FenceRect.Location;
				MouseDownOffsetPt.X = e.X - FenceRect.X;
				MouseDownOffsetPt.Y = e.Y - FenceRect.Y;
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			MouseMovePt = e.Location;
			if (mStage != MouseStage.NoAction)
			{
				float dx, dy;

				if (mStage == MouseStage.Outside) // drawing rectangle 
				{
					FenceRect = GetFenceRect(MouseDownPt, new SizeF(MouseMovePt.X - MouseDownPt.X, MouseMovePt.Y - MouseDownPt.Y));
				}
				else if (mStage == MouseStage.Fence) // moving fence
				{
					dx = e.X - MouseDownPt.X;
					dy = e.Y - MouseDownPt.Y;
					FenceRect.X = MouseDownPt.X + dx - MouseDownOffsetPt.X;
					FenceRect.Y = MouseDownPt.Y + dy - MouseDownOffsetPt.Y;
				}
				else if (mStage == MouseStage.Handle) // moving handle
				{
					FenceRect.Width = Math.Abs(FenceRect.X - e.X);
					FenceRect.Height = Math.Abs(FenceRect.Y - e.Y);
				}


				Invalidate();
			}
			else
			{
				MouseStage stage = GetMouseOverType(e.X, e.Y);
				if (stage == MouseStage.Fence)
					base.Cursor = Cursors.Hand;
				else if (stage == MouseStage.Handle)
					base.Cursor = Cursors.SizeNESW;
				else
					base.Cursor = Cursors.Default;
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			if (mStage == MouseStage.Outside)
				FenceRect = GetFenceRect(MouseDownPt, new SizeF(MouseMovePt.X - MouseDownPt.X, MouseMovePt.Y - MouseDownPt.Y));

			mStage = MouseStage.NoAction;
			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
			Graphics g = pe.Graphics;

			using (var p = new Pen(Color.Red, 1))
			{
				// draw the saved rectangle
				if (FenceRect.Width > 0)
				{
					// rectangle outline dashed white
					p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
					g.DrawRectangle(p, Rectangle.Round(FenceRect));

					// draw the handle solid green
					int h = 5;
					var rect2 = new Rectangle((int)(FenceRect.X + FenceRect.Width - h), (int)(FenceRect.Y + FenceRect.Height - h), 2 * h, 2 * h);
					p.Width = 2;
					p.Color = Color.LimeGreen;
					p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
					g.DrawRectangle(p, rect2);
				}
			}
		}

		private MouseStage GetMouseOverType(int x, int y)
		{
			// determine if the mouse pointer is over the handle or fence
			int h = 5;
			var handleRect = new Rectangle((int)(FenceRect.X + FenceRect.Width - h), (int)(FenceRect.Y + FenceRect.Height - h), 2 * h, 2 * h);
			if (handleRect.Contains(x, y))
				return MouseStage.Handle;
			else if (FenceRect.Contains(x, y))
				return MouseStage.Fence;
			else
				return MouseStage.Outside;
		}

		private RectangleF GetFenceRect(PointF locPt, SizeF sz)
		{
			RectangleF GetFenceRectRet;
			// creates proper FenceRect from any direction for point and size 
			float X = locPt.X;
			float X1 = sz.Width;
			if (X1 < 0)
			{
				X1 = Math.Abs(X1);
				X -= X1;
			}

			float Y = locPt.Y;
			float Y1 = sz.Height;
			if (Y1 < 0)
			{
				Y1 = Math.Abs(Y1);
				Y -= Y1;
			}

			GetFenceRectRet = new RectangleF(X, Y, X1, Y1);
			return GetFenceRectRet;
		}

		//private Image mBackgroundImage;
		//public new Image BackgroundImage
		//{
		//	get { return mBackgroundImage; }
		//	set
		//	{
		//		if (mBackgroundImage != value)
		//		{
		//			mBackgroundImage = value;
		//			Invalidate();
		//		}
		//	}
		//}

		private Image mImage;
		public Image Image
		{
			get { return mImage; }
			set
			{
				if (mImage != value)
				{
					mImage = value;
					Invalidate();
				}
			}
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			Graphics g = pevent.Graphics;

			DrawChess(g);
			DrawPaper(g);
			DrawImage(g);
		}

		private void DrawPaper(Graphics g)
		{
			Rectangle r = PaperRectangle;

			//ombra
			r.Offset(5, 5);
			g.FillRectangle(Brushes.Gray, r);

			//foglio
			r.Offset(-5, -5);
			g.FillRectangle(Brushes.White, r);
			g.DrawRectangle(Pens.LimeGreen, r);
		}

		private void DrawImage(Graphics g)
		{
			Rectangle r = ImageRectangle;
			//immagine
			if (mImage != null)
			{
				g.DrawImage(mImage, r);
			}
		}

		private Rectangle ImageRectangle
		{
			get
			{
				Rectangle r = PaperRectangle;
				return new Rectangle(r.Left + 1, r.Top + 1, r.Width - 1, r.Height - 1);
			}
		}

		private Rectangle PaperRectangle
		{
			get
			{
				Rectangle r = ControlRectangle;

				if (mImage != null)
				{
					Size s = CalculateResizeToFit(mImage.Size, r.Size);
					r = new Rectangle(new Point((r.Width - s.Width) / 2, (r.Height - s.Height) / 2), s);
				}

				r.Inflate(-10, -10);
				return r;
			}
		}

		private static Size CalculateResizeToFit(Size imageSize, Size boxSize)
		{
			// TODO: Check for arguments (for null and <=0)
			double widthScale = boxSize.Width / (double)imageSize.Width;
			double heightScale = boxSize.Height / (double)imageSize.Height;
			double scale = Math.Min(widthScale, heightScale);
			return new Size((int)Math.Round(imageSize.Width * scale), (int)Math.Round(imageSize.Height * scale));
		}

		private void DrawChess(Graphics g)
		{
			Rectangle cr = ControlRectangle; //control rectangle
			g.Clear(BackColor);
			if (BackgroundImage != null)
			{
				using (TextureBrush b = new TextureBrush(BackgroundImage))
					g.FillRectangle(b, cr);
			}
		}

		private Rectangle ControlRectangle { get => new Rectangle(0, 0, Width, Height); }
	}
}