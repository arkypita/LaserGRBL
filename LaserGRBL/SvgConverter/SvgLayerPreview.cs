//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using WPoint = System.Windows.Point;

namespace LaserGRBL.SvgConverter
{
	/// <summary>
	/// Preview of the svg color layers in the import dialog: each layer is drawn as it will be engraved (line, cut, fill),
	/// with zoom, pan and selection of a layer by clicking on its shapes
	/// </summary>
	public class SvgLayerPreview : Control
	{
		// geometry of a layer, in mm (same coordinates of the gcode, Y up)
		private class LayerView
		{
			public SvgColorLayer Layer;
			public List<List<WPoint>> Shapes;   // as captured, input of the filling
			public List<PointF[]> Outlines;
			public List<RectangleF> OutlineBounds;
			public List<PointF[]> Regions;      // shapes joined and closed, for clicks inside an area
			public List<RectangleF> RegionBounds;
			public List<PointF[]> FillLines;    // hatch lines, null until computed
			public List<RectangleF> FillBounds;
			public string FillKey;              // fill parameters of FillLines
			public volatile string PendingKey;  // fill parameters requested to the worker (read by the worker thread)

			// what is drawn: the lines clipped to the cached area and scaled to pixels (see EnsureScreenPaths)
			public GraphicsPath ScreenOutline, ScreenFill;

			public void ResetScreen()
			{
				if (ScreenOutline != null) ScreenOutline.Dispose();
				if (ScreenFill != null) ScreenFill.Dispose();
				ScreenOutline = ScreenFill = null;
			}
		}

		private enum Emphasis { Normal, Dimmed, Hover, Selected }

		public event Action<SvgColorLayer> LayerClicked; // null when clicking on empty space

		// conversion and filling use static state: all the work is done in order by a single thread
		private BlockingCollection<Action> mQueue = new BlockingCollection<Action>();
		private Thread mWorker;

		private List<LayerView> mViews;     // null while loading
		private RectangleF mBounds;         // bounds of the drawing in mm
		private bool mHasGeometry;
		private string mError;

		private double mScale = 1;          // pixel per mm
		private double mOffX, mOffY;        // screen position of the origin
		private bool mUserView;             // zoomed or moved by the user: keep it when resizing

		private SvgColorLayer mSelected;
		private PointF[] mSelectedShape;    // the shape clicked in the preview, highlighted over its layer
		private SvgColorLayer mHover;

		private Point mMouseDown;
		private MouseButtons mMouseButton = MouseButtons.None;
		private bool mDragging;
		private double mDragOffX, mDragOffY;
		private System.Windows.Forms.Timer mHoverTimer;
		private Point mHoverPos;

		public SvgLayerPreview()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.Selectable, true);
			BackColor = ColorScheme.PreviewBackColor;
			ForeColor = ColorScheme.PreviewText;

			mHoverTimer = new System.Windows.Forms.Timer();
			mHoverTimer.Interval = 150;
			mHoverTimer.Tick += HoverTimer_Tick;

			mWorker = new Thread(WorkerLoop);
			mWorker.IsBackground = true;
			mWorker.Name = "SvgLayerPreview";
			mWorker.Start();
		}

		#region Worker

		private void WorkerLoop()
		{
			foreach (Action action in mQueue.GetConsumingEnumerable())
			{
				try { action(); }
				catch (Exception ex) { Logger.LogException("SvgLayerPreview", ex); }
			}
		}

		private void PostToUI(Action action)
		{
			try
			{
				if (IsHandleCreated && !IsDisposed)
					BeginInvoke(action);
			}
			catch (InvalidOperationException) { } // closed in the meanwhile
		}

		/// <summary>
		/// Stop the background work, waiting for the running job: the gcode conversion must not run at the same time
		/// </summary>
		public void StopWorker()
		{
			if (!mQueue.IsAddingCompleted)
				mQueue.CompleteAdding();
			if (mWorker != null && mWorker.IsAlive)
				mWorker.Join();
		}

		#endregion

		#region Loading

		/// <summary>
		/// Capture the geometry of every layer in background, then show it
		/// </summary>
		public void Load(VectorImportSource source, List<SvgColorLayer> layers, GrblCore core)
		{
			mViews = null;
			mError = null;
			Invalidate();

			if (source == null) // the file could not be read
			{
				mError = "";
				mViews = new List<LayerView>();
				return;
			}

			mQueue.Add(delegate
			{
				List<LayerView> views = new List<LayerView>();
				try
				{
					Dictionary<string, List<List<WPoint>>> shapes = source.CaptureLayers(core, layers);
					foreach (SvgColorLayer layer in layers)
						views.Add(CreateView(layer, shapes[layer.Color]));
				}
				catch (Exception ex)
				{
					Logger.LogException("SvgLayerPreview", ex);
					PostToUI(delegate { mError = ex.Message; mViews = new List<LayerView>(); Invalidate(); });
					return;
				}

				PostToUI(delegate { ShowViews(views); });
			});
		}

		private static LayerView CreateView(SvgColorLayer layer, List<List<WPoint>> shapes)
		{
			LayerView v = new LayerView();
			v.Layer = layer;
			v.Shapes = shapes.Where(s => s.Count >= 2).ToList();
			v.Outlines = v.Shapes.Select(s => ToPointF(s)).ToList();
			v.OutlineBounds = v.Outlines.Select(p => BoundsOf(p)).ToList();
			v.Regions = SvgFilling.JoinOpenPaths(v.Shapes).Where(s => s.Count >= 3).Select(s => ToPointF(s)).ToList();
			v.RegionBounds = v.Regions.Select(p => BoundsOf(p)).ToList();
			return v;
		}

		private void ShowViews(List<LayerView> views)
		{
			mViews = views;
			List<RectangleF> all = views.SelectMany(v => v.OutlineBounds).ToList();
			mHasGeometry = all.Count > 0;
			mBounds = mHasGeometry ? all.Aggregate(RectangleF.Union) : RectangleF.Empty;

			foreach (LayerView v in views)
				if (v.Layer.HasFill)
					RequestFill(v);

			FitToView();
		}

		private static PointF[] ToPointF(List<WPoint> shape)
		{
			PointF[] rv = new PointF[shape.Count];
			for (int i = 0; i < shape.Count; i++)
				rv[i] = new PointF((float)shape[i].X, (float)shape[i].Y);
			return rv;
		}

		private static RectangleF BoundsOf(PointF[] points)
		{
			float minX = float.MaxValue, minY = float.MaxValue, maxX = float.MinValue, maxY = float.MinValue;
			foreach (PointF p in points)
			{
				minX = Math.Min(minX, p.X); maxX = Math.Max(maxX, p.X);
				minY = Math.Min(minY, p.Y); maxY = Math.Max(maxY, p.Y);
			}
			return RectangleF.FromLTRB(minX, minY, maxX, maxY);
		}

		#endregion

		#region Layers state

		public SvgColorLayer SelectedLayer
		{
			get { return mSelected; }
			set
			{
				if (mSelected != value)
				{
					mSelected = value;
					mSelectedShape = null;
					Invalidate();
				}
			}
		}

		/// <summary>
		/// The settings of a layer changed: redraw it, computing the filling again if needed
		/// </summary>
		public void LayerChanged(SvgColorLayer layer)
		{
			if (mViews != null)
			{
				LayerView v = mViews.FirstOrDefault(x => x.Layer == layer);
				if (v != null && layer.HasFill)
					RequestFill(v);
			}
			Invalidate();
		}

		private static string FillKey(SvgColorLayer layer)
		{ return layer.FillDirection + "|" + layer.LinesPerMM.ToString(System.Globalization.CultureInfo.InvariantCulture); }

		private void RequestFill(LayerView v)
		{
			string key = FillKey(v.Layer);
			if (key == v.FillKey || key == v.PendingKey || mQueue.IsAddingCompleted)
				return;

			v.PendingKey = key;
			RasterConverter.ImageProcessor.Direction dir = v.Layer.FillDirection;
			double linesPerMM = v.Layer.LinesPerMM;
			List<List<WPoint>> shapes = v.Shapes;

			mQueue.Add(delegate
			{
				if (v.PendingKey != key)
					return; // changed again before starting

				List<PointF[]> lines = SvgFilling.Build(shapes, dir, linesPerMM).Select(s => ToPointF(s)).ToList();
				List<RectangleF> bounds = lines.Select(l => BoundsOf(l)).ToList();
				PostToUI(delegate
				{
					if (v.PendingKey != key)
						return; // superseded by a newer request
					v.FillLines = lines;
					v.FillBounds = bounds;
					v.ResetScreen();
					v.FillKey = key;
					v.PendingKey = null;
					Invalidate();
				});
			});
		}

		#endregion

		#region View

		public void FitToView()
		{
			mUserView = false;
			if (mViews == null || !mHasGeometry)
			{
				Invalidate();
				return;
			}

			const int margin = 30;
			double w = Math.Max(mBounds.Width, 0.1), h = Math.Max(mBounds.Height, 0.1);
			mScale = Math.Max(1e-3, Math.Min((ClientSize.Width - 2 * margin) / w, (ClientSize.Height - 2 * margin) / h));
			mOffX = ClientSize.Width / 2.0 - (mBounds.Left + mBounds.Width / 2.0) * mScale;
			mOffY = ClientSize.Height / 2.0 + (mBounds.Top + mBounds.Height / 2.0) * mScale;
			Invalidate();
		}

		// drawing lines much longer than the screen is very slow (dashed and wide pens most of all) when zoomed in:
		// the lines are clipped to the visible area plus one screen around it, so panning just translates them
		private double mCacheScale;
		private RectangleF mCacheArea;

		private RectangleF VisibleArea()
		{
			PointF tl = ToWorld(new Point(0, 0)), br = ToWorld(new Point(ClientSize.Width, ClientSize.Height));
			return RectangleF.FromLTRB(tl.X, br.Y, br.X, tl.Y);
		}

		private void EnsureScreenPaths()
		{
			RectangleF visible = VisibleArea();
			if (mCacheScale != mScale || !mCacheArea.Contains(visible))
			{
				mCacheScale = mScale;
				mCacheArea = RectangleF.Inflate(visible, visible.Width, visible.Height);
				foreach (LayerView v in mViews)
					v.ResetScreen();
			}

			foreach (LayerView v in mViews)
			{
				if (v.ScreenOutline == null)
					v.ScreenOutline = ClippedPath(v.Outlines, v.OutlineBounds);
				if (v.ScreenFill == null && v.FillLines != null)
					v.ScreenFill = ClippedPath(v.FillLines, v.FillBounds);
			}
		}

		// polylines clipped to the cache area, in pixels (without the view offset, applied as a translation)
		private GraphicsPath ClippedPath(List<PointF[]> lines, List<RectangleF> bounds)
		{
			RectangleF area = mCacheArea;
			float s = (float)mScale;
			GraphicsPath path = new GraphicsPath();
			List<PointF> figure = new List<PointF>();

			for (int i = 0; i < lines.Count; i++)
			{
				RectangleF b = bounds[i];
				if (b.Right < area.Left || b.Left > area.Right || b.Bottom < area.Top || b.Top > area.Bottom)
					continue;

				PointF[] pts = lines[i];
				bool inside = b.Left >= area.Left && b.Right <= area.Right && b.Top >= area.Top && b.Bottom <= area.Bottom;
				for (int k = 1; k < pts.Length; k++)
				{
					PointF a = pts[k - 1], c = pts[k];
					if (!inside && !ClipSegment(area, ref a, ref c))
					{
						AddFigure(path, figure); // this segment is outside: the line continues later
						continue;
					}
					PointF sa = new PointF(a.X * s, -a.Y * s), sc = new PointF(c.X * s, -c.Y * s);
					if (figure.Count > 0 && figure[figure.Count - 1] != sa)
						AddFigure(path, figure); // entered again after an exit
					if (figure.Count == 0)
						figure.Add(sa);
					figure.Add(sc);
				}
				AddFigure(path, figure);
			}
			return path;
		}

		private static void AddFigure(GraphicsPath path, List<PointF> figure)
		{
			if (figure.Count >= 2)
			{
				path.StartFigure();
				path.AddLines(figure.ToArray());
			}
			figure.Clear();
		}

		// Liang-Barsky: false when the segment is completely outside the rectangle
		private static bool ClipSegment(RectangleF r, ref PointF a, ref PointF b)
		{
			float dx = b.X - a.X, dy = b.Y - a.Y, t0 = 0, t1 = 1;
			float[] p = { -dx, dx, -dy, dy };
			float[] q = { a.X - r.Left, r.Right - a.X, a.Y - r.Top, r.Bottom - a.Y };
			for (int i = 0; i < 4; i++)
			{
				if (p[i] == 0)
				{
					if (q[i] < 0) return false;
				}
				else
				{
					float t = q[i] / p[i];
					if (p[i] < 0) { if (t > t1) return false; if (t > t0) t0 = t; }
					else { if (t < t0) return false; if (t < t1) t1 = t; }
				}
			}
			PointF start = a;
			if (t1 < 1) b = new PointF(start.X + t1 * dx, start.Y + t1 * dy);
			if (t0 > 0) a = new PointF(start.X + t0 * dx, start.Y + t0 * dy);
			return true;
		}

		private PointF ToWorld(Point p)
		{ return new PointF((float)((p.X - mOffX) / mScale), (float)((mOffY - p.Y) / mScale)); }

		private PointF ToScreen(double x, double y)
		{ return new PointF((float)(x * mScale + mOffX), (float)(mOffY - y * mScale)); }

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (!mUserView)
				FitToView();
		}

		private void ZoomAt(Point p, double factor)
		{
			double fit = FitScale();
			double scale = Math.Max(fit / 20, Math.Min(fit * 500, mScale * factor));
			PointF w = ToWorld(p);
			mScale = scale;
			mOffX = p.X - w.X * mScale;
			mOffY = p.Y + w.Y * mScale;
			mUserView = true;
			Invalidate();
		}

		private double FitScale()
		{
			double w = Math.Max(mBounds.Width, 0.1), h = Math.Max(mBounds.Height, 0.1);
			return Math.Max(1e-3, Math.Min(Math.Max(1, ClientSize.Width - 60) / w, Math.Max(1, ClientSize.Height - 60) / h));
		}

		#endregion

		#region Mouse

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			Focus(); // for the mouse wheel
			mMouseDown = e.Location;
			mMouseButton = e.Button;
			mDragging = false;
			mDragOffX = mOffX;
			mDragOffY = mOffY;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (mMouseButton != MouseButtons.None)
			{
				if (!mDragging && (Math.Abs(e.X - mMouseDown.X) > 4 || Math.Abs(e.Y - mMouseDown.Y) > 4))
				{
					mDragging = true;
					Cursor = Cursors.SizeAll;
				}
				if (mDragging)
				{
					mOffX = mDragOffX + e.X - mMouseDown.X;
					mOffY = mDragOffY + e.Y - mMouseDown.Y;
					mUserView = true;
					Invalidate();
				}
			}
			else
			{
				mHoverPos = e.Location;
				mHoverTimer.Stop();
				mHoverTimer.Start();
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (!mDragging && e.Button == MouseButtons.Left && mViews != null && LayerClicked != null)
			{
				PointF[] shape;
				SvgColorLayer layer = HitTest(e.Location, out shape);
				LayerClicked(layer);
				if (layer != null && mSelected == layer)
				{
					mSelectedShape = shape;
					Invalidate();
				}
			}

			mMouseButton = MouseButtons.None;
			mDragging = false;
			Cursor = mHover != null ? Cursors.Hand : Cursors.Default;
		}

		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			base.OnMouseDoubleClick(e);
			FitToView();
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			if (mViews != null)
				ZoomAt(e.Location, e.Delta > 0 ? 1.25 : 1 / 1.25);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			mHoverTimer.Stop();
			SetHover(null);
		}

		private void HoverTimer_Tick(object sender, EventArgs e)
		{
			mHoverTimer.Stop();
			if (mMouseButton == MouseButtons.None && mViews != null && ClientRectangle.Contains(PointToClient(MousePosition)))
			{
				PointF[] shape;
				SetHover(HitTest(mHoverPos, out shape));
			}
		}

		private void SetHover(SvgColorLayer layer)
		{
			if (mHover != layer)
			{
				mHover = layer;
				Cursor = layer != null ? Cursors.Hand : Cursors.Default;
				Invalidate();
			}
		}

		/// <summary>
		/// The layer under the given point: a line right under it, otherwise a fill that covers it (what is painted there),
		/// otherwise the smallest closed shape that contains it, otherwise a line close to it.
		/// The areas win over a near line: when zoomed out almost every point is near a line
		/// </summary>
		private SvgColorLayer HitTest(Point screen, out PointF[] shape)
		{
			shape = null;
			if (mViews == null)
				return null;

			PointF p = ToWorld(screen);
			SvgColorLayer layer = HitOutline(p, (float)(3 / mScale), out shape);
			if (layer != null)
				return layer;

			layer = HitFill(p, out shape);
			if (layer != null)
				return layer;

			// inside a closed shape: the smallest one
			float bestArea = float.MaxValue;
			foreach (LayerView v in mViews)
			{
				for (int i = 0; i < v.Regions.Count; i++)
				{
					RectangleF b = v.RegionBounds[i];
					float area = b.Width * b.Height;
					if (area < bestArea && b.Contains(p) && Inside(v.Regions[i], p))
					{
						layer = v.Layer;
						shape = v.Regions[i];
						bestArea = area;
					}
				}
			}
			if (layer != null)
			{
				shape = Closed(shape);
				return layer;
			}

			return HitOutline(p, (float)(8 / mScale), out shape);
		}

		// a layer in fill mode that paints the point: inside an odd number of its shapes (holes are not painted),
		// the ones drawn over the others first. The shape is the smallest one around the point (the outer border of a ring)
		private SvgColorLayer HitFill(PointF p, out PointF[] shape)
		{
			foreach (LayerView v in mViews.Where(x => x.Layer.HasFill).OrderBy(x => DrawOrder(x)).Reverse())
			{
				int count = 0;
				PointF[] smallest = null;
				float smallestArea = float.MaxValue;
				for (int i = 0; i < v.Regions.Count; i++)
				{
					RectangleF b = v.RegionBounds[i];
					if (b.Contains(p) && Inside(v.Regions[i], p))
					{
						count++;
						if (b.Width * b.Height < smallestArea)
						{
							smallest = v.Regions[i];
							smallestArea = b.Width * b.Height;
						}
					}
				}
				if (count % 2 == 1)
				{
					shape = Closed(smallest);
					return v.Layer;
				}
			}
			shape = null;
			return null;
		}

		// the selected layer first, then the ones drawn over the others
		private SvgColorLayer HitOutline(PointF p, float tol, out PointF[] shape)
		{
			foreach (LayerView v in mViews.OrderBy(x => DrawOrder(x)).Reverse())
			{
				for (int i = 0; i < v.Outlines.Count; i++)
				{
					RectangleF b = v.OutlineBounds[i];
					if (p.X < b.Left - tol || p.X > b.Right + tol || p.Y < b.Top - tol || p.Y > b.Bottom + tol)
						continue;
					if (NearPolyline(v.Outlines[i], p, tol))
					{
						// files from CAD often store each side as a separate line: show the whole joined shape
						shape = v.Outlines[i];
						for (int r = 0; r < v.Regions.Count; r++)
						{
							RectangleF rb = v.RegionBounds[r];
							if (p.X >= rb.Left - tol && p.X <= rb.Right + tol && p.Y >= rb.Top - tol && p.Y <= rb.Bottom + tol && NearPolyline(Closed(v.Regions[r]), p, tol))
							{
								shape = Closed(v.Regions[r]);
								break;
							}
						}
						return v.Layer;
					}
				}
			}
			shape = null;
			return null;
		}

		// close the small gaps left by the joining, but never draw a long line between the ends of an open shape
		private static PointF[] Closed(PointF[] shape)
		{
			PointF a = shape[0], b = shape[shape.Length - 1];
			float gap = (float)Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
			return gap == 0 || gap > 1 ? shape : shape.Concat(new PointF[] { a }).ToArray();
		}

		private static bool NearPolyline(PointF[] pts, PointF p, float tol)
		{
			float tol2 = tol * tol;
			for (int i = 1; i < pts.Length; i++)
				if (SegmentDistance2(pts[i - 1], pts[i], p) <= tol2)
					return true;
			return false;
		}

		private static float SegmentDistance2(PointF a, PointF b, PointF p)
		{
			float dx = b.X - a.X, dy = b.Y - a.Y;
			float len2 = dx * dx + dy * dy;
			float t = len2 > 0 ? Math.Max(0, Math.Min(1, ((p.X - a.X) * dx + (p.Y - a.Y) * dy) / len2)) : 0;
			float x = a.X + t * dx - p.X, y = a.Y + t * dy - p.Y;
			return x * x + y * y;
		}

		// even-odd rule, the shape is considered closed
		private static bool Inside(PointF[] poly, PointF p)
		{
			bool inside = false;
			for (int i = 0, j = poly.Length - 1; i < poly.Length; j = i++)
			{
				if ((poly[i].Y > p.Y) != (poly[j].Y > p.Y) &&
					p.X < (poly[j].X - poly[i].X) * (p.Y - poly[i].Y) / (poly[j].Y - poly[i].Y) + poly[i].X)
					inside = !inside;
			}
			return inside;
		}

		#endregion

		#region Paint

		// ignored layers below, the selected one over all the others
		private int DrawOrder(LayerView v)
		{
			if (v.Layer == mSelected) return 3;
			if (v.Layer == mHover) return 2;
			return v.Layer.Mode == SvgLayerMode.Ignore ? 0 : 1;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.Clear(BackColor);

			if (mViews == null)
			{
				DrawCenteredText(g, Strings.SvgPreviewLoading);
				return;
			}
			if (mError != null)
			{
				DrawCenteredText(g, Strings.SvgPreviewError + "\r\n" + mError);
				return;
			}

			DrawGrid(g);

			g.SmoothingMode = SmoothingMode.AntiAlias;
			GraphicsState state = g.Save();
			g.TranslateTransform((float)mOffX, (float)mOffY);
			EnsureScreenPaths();
			foreach (LayerView v in mViews.OrderBy(x => DrawOrder(x)))
				DrawLayer(g, v);
			g.Restore(state);

			if (mSelectedShape != null)
				DrawSelectedShape(g);

			DrawOverlay(g);
		}

		private void DrawLayer(Graphics g, LayerView v)
		{
			Emphasis emphasis = v.Layer == mSelected ? Emphasis.Selected : v.Layer == mHover ? Emphasis.Hover : mSelected != null ? Emphasis.Dimmed : Emphasis.Normal;
			int alpha = emphasis == Emphasis.Dimmed ? 80 : 255;
			Color color = VisibleColor(v.Layer.DrawingColor);
			const float px = 1;

			// highlight around the shapes of the selected (or hovered) layer
			if (emphasis == Emphasis.Selected || emphasis == Emphasis.Hover)
			{
				using (Pen halo = new Pen(Color.FromArgb(emphasis == Emphasis.Selected ? 110 : 60, SystemColors.Highlight), 7 * px))
				{
					halo.LineJoin = LineJoin.Round;
					g.DrawPath(halo, v.ScreenOutline);
				}
			}

			switch (v.Layer.Mode)
			{
				case SvgLayerMode.Ignore:
					using (Pen pen = new Pen(Color.FromArgb(alpha * 3 / 4, Blend(color, BackColor, 0.65)), px))
					{
						pen.DashPattern = new float[] { 4, 3 };
						g.DrawPath(pen, v.ScreenOutline);
					}
					break;
				case SvgLayerMode.Line:
					using (Pen pen = new Pen(Color.FromArgb(alpha, color), px))
						g.DrawPath(pen, v.ScreenOutline);
					break;
				case SvgLayerMode.Cut:
					using (Pen pen = new Pen(Color.FromArgb(alpha, color), 2.5f * px))
						g.DrawPath(pen, v.ScreenOutline);
					break;
				case SvgLayerMode.Fill:
				case SvgLayerMode.FillAndLine:
					bool ready = v.ScreenFill != null && v.FillKey == FillKey(v.Layer);
					if (ready)
						using (Pen pen = new Pen(Color.FromArgb(alpha, color), px))
							g.DrawPath(pen, v.ScreenFill);

					// outline: engraved in FillAndLine mode, just a reference (or a placeholder while computing) in Fill mode
					bool outline = v.Layer.Mode == SvgLayerMode.FillAndLine;
					using (Pen pen = new Pen(Color.FromArgb(outline ? alpha : alpha / 3, color), px))
					{
						if (!ready && !outline)
							pen.DashPattern = new float[] { 2, 2 };
						g.DrawPath(pen, v.ScreenOutline);
					}
					break;
			}
		}

		// the clicked shape, over its (already highlighted) layer: clipped to the screen like everything else
		private void DrawSelectedShape(Graphics g)
		{
			RectangleF screen = RectangleF.Inflate(ClientRectangle, 10, 10);
			using (Pen pen = new Pen(SystemColors.Highlight, 3))
			{
				pen.StartCap = pen.EndCap = LineCap.Round;
				for (int i = 1; i < mSelectedShape.Length; i++)
				{
					PointF a = ToScreen(mSelectedShape[i - 1].X, mSelectedShape[i - 1].Y), b = ToScreen(mSelectedShape[i].X, mSelectedShape[i].Y);
					if (ClipSegment(screen, ref a, ref b))
						g.DrawLine(pen, a, b);
				}
			}
		}

		private void DrawGrid(Graphics g)
		{
			// grid step: the first one that is at least 25 pixel wide
			double step = 1;
			foreach (double s in new double[] { 1, 5, 10, 50, 100, 500, 1000, 5000 })
			{
				step = s;
				if (s * mScale >= 25) break;
			}

			PointF tl = ToWorld(new Point(0, 0)), br = ToWorld(new Point(ClientSize.Width, ClientSize.Height));
			using (Pen minor = new Pen(ColorScheme.PreviewGridMinor))
			using (Pen axis = new Pen(ColorScheme.PreviewRuler))
			{
				for (double x = Math.Floor(tl.X / step) * step; x <= br.X; x += step)
				{
					float sx = ToScreen(x, 0).X;
					g.DrawLine(Math.Abs(x) < step / 2 ? axis : minor, sx, 0, sx, ClientSize.Height);
				}
				for (double y = Math.Floor(br.Y / step) * step; y <= tl.Y; y += step)
				{
					float sy = ToScreen(0, y).Y;
					g.DrawLine(Math.Abs(y) < step / 2 ? axis : minor, 0, sy, ClientSize.Width, sy);
				}
			}

			// job size
			using (Pen range = new Pen(ColorScheme.PreviewJobRange))
			{
				range.DashPattern = new float[] { 4, 4 };
				// each side clipped to the screen: a dashed line thousands of pixel long is very slow to draw
				PointF[] c = { ToScreen(mBounds.Left, mBounds.Bottom), ToScreen(mBounds.Right, mBounds.Bottom), ToScreen(mBounds.Right, mBounds.Top), ToScreen(mBounds.Left, mBounds.Top) };
				RectangleF screen = RectangleF.Inflate(ClientRectangle, 10, 10);
				for (int i = 0; i < 4; i++)
				{
					PointF p1 = c[i], p2 = c[(i + 1) % 4];
					if (ClipSegment(screen, ref p1, ref p2))
						g.DrawLine(range, p1, p2);
				}
			}
		}

		private void DrawOverlay(Graphics g)
		{
			g.SmoothingMode = SmoothingMode.None;
			TextFormatFlags flags = TextFormatFlags.NoPadding | TextFormatFlags.SingleLine;
			Color text = ColorScheme.PreviewText;
			int pad = 8;

			string size = string.Format("{0:0.0} × {1:0.0} mm", mBounds.Width, mBounds.Height);
			Size sz = TextRenderer.MeasureText(size, Font, Size.Empty, flags);
			TextRenderer.DrawText(g, size, Font, new Point(ClientSize.Width - sz.Width - pad, ClientSize.Height - sz.Height - pad), text, flags);

			string help = Strings.SvgPreviewNavigation;
			Size hs = TextRenderer.MeasureText(help, Font, Size.Empty, flags);
			TextRenderer.DrawText(g, help, Font, new Point(pad, ClientSize.Height - hs.Height - pad), Color.FromArgb(160, text), flags);

			if (mSelected == null && mViews.Count > 0)
				TextRenderer.DrawText(g, Strings.SvgPreviewHint, Font, new Point(pad, pad), text, flags);

			if (mViews.Any(v => v.PendingKey != null && v.Layer.HasFill))
			{
				string busy = Strings.SvgPreviewFilling;
				Size bs = TextRenderer.MeasureText(busy, Font, Size.Empty, flags);
				TextRenderer.DrawText(g, busy, Font, new Point(ClientSize.Width - bs.Width - pad, pad), text, flags);
			}
		}

		private void DrawCenteredText(Graphics g, string text)
		{
			TextRenderer.DrawText(g, text, Font, ClientRectangle, ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
		}

		// colors too similar to the background (i.e. white on a white preview) are darkened or lightened
		private Color VisibleColor(Color c)
		{
			double bg = Luminance(BackColor);
			if (Math.Abs(Luminance(c) - bg) >= 0.25)
				return c;
			return Blend(c, bg > 0.5 ? Color.Black : Color.White, 0.5);
		}

		private static double Luminance(Color c)
		{ return (0.299 * c.R + 0.587 * c.G + 0.114 * c.B) / 255; }

		private static Color Blend(Color a, Color b, double amount)
		{
			return Color.FromArgb(
				(int)(a.R + (b.R - a.R) * amount),
				(int)(a.G + (b.G - a.G) * amount),
				(int)(a.B + (b.B - a.B) * amount));
		}

		#endregion

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				StopWorker();
				mHoverTimer.Dispose();
				if (mViews != null)
				{
					foreach (LayerView v in mViews)
					{
						v.ResetScreen();
					}
				}
			}
			base.Dispose(disposing);
		}
	}
}
