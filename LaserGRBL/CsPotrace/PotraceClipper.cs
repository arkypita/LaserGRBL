using ClipperLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace CsPotrace
{
	class PotraceClipper
	{
		private const double resolution = 1000.0;

		internal static List<List<Curve>> BuildFilling(List<List<Curve>> plist, double spacing, double w, double h, LaserGRBL.RasterConverter.ImageProcessor.Direction dir)
		{
			if (dir == LaserGRBL.RasterConverter.ImageProcessor.Direction.NewInsetFilling)
				return BuildInsetFilling(plist, spacing);
			else
				return BuildGridFilling(plist, spacing, w, h, dir);
		}

		private static List<List<Curve>> BuildGridFilling(List<List<Curve>> plist, double spacing, double w, double h, LaserGRBL.RasterConverter.ImageProcessor.Direction dir)
		{
			Clipper c = new Clipper();
			AddGridSubject(c, spacing, w, h, dir);
			AddGridClip(plist, c);

			PolyTree solution = new PolyTree();
			bool succeeded = c.Execute(ClipType.ctIntersection, solution, PolyFillType.pftEvenOdd, PolyFillType.pftEvenOdd);

			if (!succeeded)
				return null;

			List<List<IntPoint>> ll = Clipper.OpenPathsFromPolyTree(solution);
			return ToPotraceList(ll, false);
		}

		private static List<List<Curve>> BuildInsetFilling(List<List<Curve>> plist, double spacing)
		{
			List<List<Curve>> flist = new List<List<Curve>>();
			ClipperOffset c = new ClipperOffset();
			AddOffsetSubject(plist, c);

			for (int i = 1;  ; i++)
			{
				List<List<IntPoint>> solution = new List<List<IntPoint>>();
				c.Execute(ref solution, -spacing * i * resolution);

				if (solution.Count > 0)
					flist.AddRange(ToPotraceList(solution, true));
				else
					break;
			}

			return flist;
		}

		private static List<List<Curve>> ToPotraceList(List<List<IntPoint>> ll, bool close)
		{
			List<List<Curve>> flist = new List<List<Curve>>();

			foreach (List<IntPoint> pg in ll)
			{
				if (pg.Count == 2)
				{
					dPoint a = new dPoint(pg[0].X / resolution, pg[0].Y / resolution);
					dPoint b = new dPoint(pg[1].X / resolution, pg[1].Y / resolution);
					flist.Add(new List<Curve>() { new Curve(CurveKind.Line, a, a, b, b) });
				}
				else
				{ 
					List<Curve> potcurve = new List<Curve>();
					for (int i = 0; i < pg.Count - 1; i++)
					{
						dPoint a = new dPoint(pg[i].X / resolution, pg[i].Y / resolution);
						dPoint b = new dPoint(pg[i + 1].X / resolution, pg[i + 1].Y / resolution);
						potcurve.Add(new Curve(CurveKind.Line, a, a, b, b));
					}
					if (close)
					{
						dPoint a = new dPoint(pg[pg.Count-1].X / resolution, pg[pg.Count - 1].Y / resolution);
						dPoint b = new dPoint(pg[0].X / resolution, pg[0].Y / resolution);
						potcurve.Add(new Curve(CurveKind.Line, a, a, b, b));
					}

					flist.Add(potcurve);
				}
			}

			//flist.Reverse();
			return flist;
		}

		private static void AddGridClip(List<List<Curve>> plist, Clipper c)
		{
			foreach (List<Curve> LC in plist)
			{
				GraphicsPath Current = new GraphicsPath();
				for (int j = 0; j < LC.Count; j++)
				{
					Curve C = LC[j];
					if (C.Kind == CurveKind.Line)
					{
						Current.AddLine(new PointF((float)C.A.X, (float)C.A.Y), new PointF((float)C.B.X, (float)C.B.Y));
					}
					else
					{
						PointF A = new PointF((float)C.A.X, (float)C.A.Y);
						Current.AddBezier(new PointF((float)C.A.X, (float)C.A.Y), new PointF((float)C.ControlPointA.X, (float)C.ControlPointA.Y), new PointF((float)C.ControlPointB.X, (float)C.ControlPointB.Y), new PointF((float)C.B.X, (float)C.B.Y));
					}
				}

				AddClip(c, Current);
			}
		}

		private static void AddOffsetSubject(List<List<Curve>> plist, ClipperOffset c)
		{
			foreach (List<Curve> LC in plist)
			{
				GraphicsPath Current = new GraphicsPath();
				for (int j = 0; j < LC.Count; j++)
				{
					Curve C = LC[j];
					if (C.Kind == CurveKind.Line)
					{
						Current.AddLine(new PointF((float)C.A.X, (float)C.A.Y), new PointF((float)C.B.X, (float)C.B.Y));
					}
					else
					{
						PointF A = new PointF((float)C.A.X, (float)C.A.Y);
						Current.AddBezier(new PointF((float)C.A.X, (float)C.A.Y), new PointF((float)C.ControlPointA.X, (float)C.ControlPointA.Y), new PointF((float)C.ControlPointB.X, (float)C.ControlPointB.Y), new PointF((float)C.B.X, (float)C.B.Y));
					}
				}

				AddClip(c, Current);
			}
		}

		static void AddClip(Clipper c, GraphicsPath psubject)
		{
			psubject.Flatten();

			List<IntPoint> subject = new List<IntPoint>(psubject.PathPoints.Count());
			foreach (PointF p in psubject.PathPoints)
				subject.Add(new IntPoint(p.X * resolution, p.Y * resolution));

			c.AddPath(subject, PolyType.ptClip, true);
		}

		static void AddClip(ClipperOffset c, GraphicsPath psubject)
		{
			psubject.Flatten();

			List<IntPoint> subject = new List<IntPoint>(psubject.PathPoints.Count());
			foreach (PointF p in psubject.PathPoints)
				subject.Add(new IntPoint(p.X * resolution, p.Y * resolution));

			c.AddPath(subject, JoinType.jtRound, EndType.etClosedPolygon);
		}

		static void AddGridSubject(Clipper c, double step, double w, double h, LaserGRBL.RasterConverter.ImageProcessor.Direction dir)
		{
			if (dir == LaserGRBL.RasterConverter.ImageProcessor.Direction.NewGrid || dir == LaserGRBL.RasterConverter.ImageProcessor.Direction.NewDiagonalGrid)
				step = step * 2;	//essendo che fa due passate dobbiamo dimezzare la risoluzione per avere la stessa "densità"

			double dstep = step * Math.Sqrt(2); //step for diagonal (1.414)
			double rdstep = step * (1 / Math.Sqrt(2)); //step for diagonal (1.414)
			List<List<IntPoint>> paths = new List<List<IntPoint>>();

			if (dir == LaserGRBL.RasterConverter.ImageProcessor.Direction.NewHorizontal || dir == LaserGRBL.RasterConverter.ImageProcessor.Direction.NewGrid)
			{
				for (int y = 1; y < (h / step); y ++)
					AddSegment(paths, 0, y * step, w, y * step, y % 2 == 1);
			}
			if (dir == LaserGRBL.RasterConverter.ImageProcessor.Direction.NewVertical || dir == LaserGRBL.RasterConverter.ImageProcessor.Direction.NewGrid)
			{
				for (int x = 1; x < (w / step); x++)
					AddSegment(paths, x * step, 0, x * step, h, x % 2 == 0);
			}
			if (dir == LaserGRBL.RasterConverter.ImageProcessor.Direction.NewDiagonal || dir == LaserGRBL.RasterConverter.ImageProcessor.Direction.NewDiagonalGrid)
			{
				for (int i = 0 ; i < (w + h) / dstep; i ++)
					AddSegment(paths, 0, i * dstep, i * dstep, 0, i % 2 == 0);
			}
			if (dir == LaserGRBL.RasterConverter.ImageProcessor.Direction.NewReverseDiagonal || dir == LaserGRBL.RasterConverter.ImageProcessor.Direction.NewDiagonalGrid)
			{
				for (int i = 0; i < (w + h) / dstep; i++)
					AddSegment(paths, 0, h - (i * dstep), i * dstep, h, i % 2 == 1);
			}

			if (dir == LaserGRBL.RasterConverter.ImageProcessor.Direction.NewCross)
			{
				double cl = step / 3; //cross len
				for (int y = 1; y < (h / step); y++)
				{
					for (int x = 1; x < (w / step); x++)
					{
						AddSegment(paths, (x * step) - cl, y * step, (x * step) + cl, y * step, y % 2 == 1);    //stanghette orizzontali
						AddSegment(paths, x * step, (y * step) - cl, x * step, (y * step) + cl, x % 2 == 0);    //stanghette verticali
					}
				}
			}

			if (dir == LaserGRBL.RasterConverter.ImageProcessor.Direction.NewDiagonalCross)
			{
				double cl = rdstep / 3;
				for (int y = 0; y < (h / step) + 1; y++)
				{
					for (int x = 0; x < (w / step) + 1; x++)
					{
						AddSegment(paths, (x * step) - cl, (y * step) - cl, (x * step) + cl, (y * step) + cl, x % 2 == 1);	//stanghetta verso l'alto
						AddSegment(paths, (x * step) + cl, (y * step) - cl, (x * step) - cl, (y * step) + cl, x % 2 == 1);	//stanghetta verso il basso
					}
				}
			}
			if (dir == LaserGRBL.RasterConverter.ImageProcessor.Direction.NewSquares)
			{
				double cl = step / 3;
				for (int y = 0; y < (h / step) + 1; y++)
				{
					for (int x = 0; x < (w / step) + 1; x++)
					{
						List<IntPoint> list = new List<IntPoint>();
						AddPathPoint(list, (x * step) - cl, y * step);
						AddPathPoint(list, x * step, (y * step) + cl);
						AddPathPoint(list, (x * step) + cl, y * step);
						AddPathPoint(list, x * step, (y * step) - cl);
						AddPathPoint(list, (x * step) - cl, y * step);
						AddPathPoints(paths, list);
					}
				}
			}
			if (dir == LaserGRBL.RasterConverter.ImageProcessor.Direction.NewZigZag)
			{
				double hs = step / 2;	//halfstep

				for (int i = 1; i < (w + h) / step; i++)
				{
					List<IntPoint> list = new List<IntPoint>();

					if (i % 2 == 1)
					{
						double my = 0;
						double mx = (i * step) - hs;

						AddPathPoint(list, mx, my);
						while (mx > 0)
						{
							AddPathPoint(list, mx, my += hs);
							AddPathPoint(list, mx -= hs, my);
						}
					}
					else
					{
						double mx = 0;
						double my = (i * step) - hs;

						AddPathPoint(list, mx, my);
						while (my > 0)
						{
							AddPathPoint(list, mx += hs, my);
							AddPathPoint(list, mx, my -= hs);
						}
					}

					AddPathPoints(paths, list);
				}
			}
			c.AddPaths(paths, PolyType.ptSubject, false);
		}

		private static void AddSegment(List<List<IntPoint>> target, double x1, double y1, double x2, double y2, bool invert)
		{
			if (!invert)
				target.Add(new List<IntPoint>() { new IntPoint(x1 * resolution, y1 * resolution), new IntPoint(x2 * resolution, y2 * resolution) });
			else
				target.Add(new List<IntPoint>() { new IntPoint(x2 * resolution, y2 * resolution), new IntPoint(x1 * resolution, y1 * resolution) });
		}

		private static void AddPathPoint(List<IntPoint> target, double x, double y)
		{
			target.Add(new IntPoint(x * resolution, y * resolution));
		}

		private static void AddPathPoints(List<List<IntPoint>> target, List<IntPoint> points)
		{
			target.Add(points);
		}

	}
}
