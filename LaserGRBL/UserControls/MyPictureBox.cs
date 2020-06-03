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
			//BackgroundImage = Image.FromFile(@"C:\\Users\\UserName\\Desktop\\Picture\\2.jpg");
			BackgroundImageLayout = ImageLayout.None;
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


	}
}