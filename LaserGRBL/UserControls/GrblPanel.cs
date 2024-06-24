//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using LaserGRBL;
using SharpGL;

namespace LaserGRBL.UserControls
{
	public partial class GrblPanel : UserControl, IGrblPanel
	{
		GrblCore Core;
		System.Drawing.Bitmap mBitmap;
		System.Threading.Thread TH;
		Matrix mLastMatrix;
		private GPoint mLastWPos;
		private GPoint mLastMPos;
		private float mCurF;
		private float mCurS;
		private bool mFSTrig;

		public GrblPanel()
		{
			InitializeComponent();
			Settings.CurrentGraphicMode = Settings.GraphicMode.GDI;
			GrblPanel3D.CurrentVendor = "LaserGRBL";
			GrblPanel3D.CurrentRenderer = "Legacy Preview";
			Logger.LogMessage("OpenGL", "{0}->{1} {2}, {3}", Settings.RequestedGraphicMode, Settings.CurrentGraphicMode, GrblPanel3D.CurrentVendor, GrblPanel3D.CurrentRenderer);

			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			mLastWPos = GPoint.Zero;
			mLastMPos = GPoint.Zero;

			forcez = Settings.GetObject("Enale Z Jog Control", false);
			SettingsForm.SettingsChanged += SettingsForm_SettingsChanged;
		}

		private void SettingsForm_SettingsChanged(object sender, EventArgs e)
		{
			bool newforce = Settings.GetObject("Enale Z Jog Control", false);
			if (newforce != forcez)
			{
				forcez = newforce;
				Invalidate();
			}
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			e.Graphics.Clear(ColorScheme.PreviewBackColor);
		}

		bool forcez = false;
		protected override void OnPaint(PaintEventArgs e)
		{
			try
			{
				if (mBitmap != null)
					e.Graphics.DrawImage(mBitmap, 0, 0, Width, Height);

				if (Core != null)
				{
					PointF p = MachineToDraw(mLastWPos.ToPointF());
					e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
					e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

					using (Pen px = GetPen(ColorScheme.PreviewCross, 2f))
					{
						e.Graphics.DrawLine(px, (int)p.X, (int)p.Y - 5, (int)p.X, (int)p.Y - 5 + 10);
						e.Graphics.DrawLine(px, (int)p.X - 5, (int)p.Y, (int)p.X - 5 + 10, (int)p.Y);
					}

					using (Brush b = GetBrush(ColorScheme.PreviewText))
					{
						Rectangle r = ClientRectangle;
						r.Inflate(-5, -5);
						StringFormat sf = new StringFormat();

						//  II | I
						// ---------
						// III | IV
						GrblFile.CartesianQuadrant q = Core != null && Core.LoadedFile != null ? Core.LoadedFile.Quadrant : GrblFile.CartesianQuadrant.Unknown;
						sf.Alignment = q == GrblFile.CartesianQuadrant.II || q == GrblFile.CartesianQuadrant.III ? StringAlignment.Near : StringAlignment.Far;
						sf.LineAlignment = q == GrblFile.CartesianQuadrant.III || q == GrblFile.CartesianQuadrant.IV ? StringAlignment.Far : StringAlignment.Near;

						String position = string.Format("X: {0:0.000} Y: {1:0.000}", Core != null ? mLastMPos.X : 0, Core != null ? mLastMPos.Y : 0);

                        if (Core != null && (mLastWPos.Z != 0 || mLastMPos.Z != 0 || forcez))
                            position = position + string.Format(" Z: {0:0.000}", mLastMPos.Z);

                        if (Core != null && Core.WorkingOffset != GPoint.Zero)
							position = position + "\n" + string.Format("X: {0:0.000} Y: {1:0.000}", Core != null ? mLastWPos.X : 0, Core != null ? mLastWPos.Y : 0);

                        if (Core != null && Core.WorkingOffset != GPoint.Zero  && (mLastWPos.Z != 0 || mLastMPos.Z != 0 || forcez))
                            position = position + string.Format(" Z: {0:0.000}", mLastWPos.Z);

                        if (mCurF != 0 || mCurS != 0 || mFSTrig)
						{
							mFSTrig = true;
							String fs = string.Format("F: {0:00000.##} S: {1:000.##}", Core != null ? mCurF : 0, Core != null ? mCurS : 0);
							position = position + "\n" + fs;
						}

						e.Graphics.DrawString(position, Font, b, r, sf);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.LogException("GrblPanel Paint", ex);
			}
		}


		private Pen GetPen(Color color, float width)
		{ return new Pen(color, width); }

		private Pen GetPen(Color color)
		{ return new Pen(color); }

		private Brush GetBrush(Color color)
		{ return new SolidBrush(color); }

		public void SetCore(GrblCore core)
		{
			Core = core;
			Core.OnFileLoading += OnFileLoading;
			Core.OnFileLoaded += OnFileLoaded;
		}

		void OnFileLoading(long elapsed, string filename)
		{
			AbortCreation();
		}

		void OnFileLoaded(long elapsed, string filename)
		{
			RecreateBMP();
		}

		public void RecreateBMP()
		{
			AbortCreation();

			TH = new System.Threading.Thread(DoTheWork);
			TH.Name = "GrblPanel Drawing Thread";
			TH.Start();
		}

		private void AbortCreation()
		{
			if (TH != null)
			{
				TH.Abort();
				TH = null;
			}
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			RecreateBMP();
		}

		private void DoTheWork()
		{
			try
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

					if (Core != null /*&& Core.HasProgram*/)
						Core.LoadedFile.DrawOnGraphics(g, wSize);

					mLastMatrix = g.Transform;
				}

				AssignBMP(bmp);
			}
			catch (System.Threading.ThreadAbortException)
			{
 				//standard condition for abort and recreation
			}
			catch (Exception ex)
			{
				Logger.LogException("Drawing Preview", ex);
			}
		}

		public PointF MachineToDraw(PointF p)
		{
			if (mLastMatrix == null)
				return p;

			PointF[] pa = new PointF[] { p };
			mLastMatrix.TransformPoints(pa);
			return pa[0];
		}

		public PointF DrawToMachine(PointF p)
		{
			if (mLastMatrix == null || !mLastMatrix.IsInvertible)
				return p;

			Matrix mInvert = mLastMatrix.Clone();
			mInvert.Invert();

			PointF[] pa = new PointF[] { p };
			mInvert.TransformPoints(pa);

			return pa[0];
		}

		private void AssignBMP(System.Drawing.Bitmap bmp)
		{
			lock (this)
			{
				if (mBitmap != null)
					mBitmap.Dispose();

				mBitmap = bmp;
			}
			Invalidate();
		}

		public void TimerUpdate()
		{
			if (Core != null && (mLastWPos != Core.WorkPosition || mLastMPos != Core.MachinePosition || mCurF != Core.CurrentF || mCurS != Core.CurrentS))
			{
				mLastWPos = Core.WorkPosition;
				mLastMPos = Core.MachinePosition;
				mCurF = Core.CurrentF;
				mCurS = Core.CurrentS;
				Invalidate();
			}
		}


		public void OnColorChange()
		{
			RecreateBMP();
		}

		private void GrblPanel_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (Settings.GetObject("Click N Jog", true))
			{
				PointF coord = DrawToMachine(new PointF(e.X, e.Y));
				Core.JogToPosition(coord, e.Button == MouseButtons.Right);
			}
		}
	}
}
