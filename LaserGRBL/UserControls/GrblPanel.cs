using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using LaserGRBL;

namespace LaserGRBL.UserControls
{
	public partial class GrblPanel : UserControl
	{
		GrblCore Core;
		System.Drawing.Bitmap mBitmap;
		System.Threading.Thread TH;
		Matrix mLastMatrix;
		private PointF mLastPosition;
		
		public GrblPanel()
		{
			InitializeComponent();
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			mLastPosition = new PointF(0, 0);
		}
		
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
		
			if (mBitmap != null)
				e.Graphics.DrawImage(mBitmap, 0, 0, Width, Height);


			PointF p = TranslatePoint(mLastPosition);
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			e.Graphics.DrawLine(Pens.Blue, (int)p.X, (int)p.Y - 3, (int)p.X, (int)p.Y - 3 + 7);
			e.Graphics.DrawLine(Pens.Blue, (int)p.X - 3, (int)p.Y, (int)p.X - 3 + 7, (int)p.Y);
		}
		
			

		public void SetComProgram(GrblCore core)
		{
			Core = core;
			Core.OnFileLoaded += OnFileLoaded;
		}

		void OnFileLoaded(long elapsed, string filename)
		{
			RecreateBMP();
		}
		
		public void RecreateBMP()
		{
			if (TH != null)
			{
				TH.Abort();
				TH = null;
			}
			
			TH = new System.Threading.Thread(DoTheWork);
			TH.Start();
		}
		
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			RecreateBMP();
		}
		
		private void DoTheWork()
		{
			Size wSize = Size;
			
			if (wSize.Width < 1 || wSize.Height < 1)
				return;
			
			System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(wSize.Width, wSize.Height);
			using (System.Drawing.Graphics g = Graphics.FromImage(bmp))
			{
				g.SmoothingMode = SmoothingMode.AntiAlias;
				g.InterpolationMode = InterpolationMode.HighQualityBicubic;
				g.PixelOffsetMode = PixelOffsetMode.HighQuality;
				g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
				
				g.ScaleTransform(1.0F, -1.0F);
				g.TranslateTransform(0.0F, -(float)wSize.Height);

				float scaleX = (float)(wSize.Width - 25 - 5) / (float)wSize.Width;
				float scaleY = (float)(wSize.Height - 15 - 5) / (float)wSize.Height;

				g.ScaleTransform(scaleX, scaleY);
				g.TranslateTransform(25, 15);

				g.DrawLines(Pens.Black, new PointF[] { new PointF(0, wSize.Height), new PointF(0, 0), new PointF(wSize.Width, 0) });

				if (Core != null && Core.HasProgram)
					Core.LoadedFile.DrawOnGraphics(g, wSize);

				mLastMatrix = g.Transform;
			}

			AssignBMP(bmp);
		}

		public PointF TranslatePoint(PointF p)
		{
			if (mLastMatrix == null)
				return p;

			PointF[] pa = new PointF[] { p };
			mLastMatrix.TransformPoints(pa);
			p = pa[0];
			return p;
		}

		private void AssignBMP(System.Drawing.Bitmap bmp)
		{
			lock(this)
			{
				if (mBitmap!=null)
					mBitmap.Dispose();
				
				mBitmap = bmp;
			}
			Invalidate();
		}

		public void TimerUpdate()
		{
			if (Core != null && mLastPosition != Core.LaserPosition)
			{
				mLastPosition = Core.LaserPosition;
				Invalidate();
			}
		}

	}
}
