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

		internal static List<List<Curve>> BuildFilling(List<List<Curve>> plist, double w, double h, LaserGRBL.GrblFile.L2LConf c)
		{
			if (c.dir == LaserGRBL.RasterConverter.ImageProcessor.Direction.NewInsetFilling)
				return BuildInsetFilling(plist, c);
			else
				return BuildGridFilling(plist, w, h, c);
		}

		private static List<List<Curve>> BuildGridFilling(List<List<Curve>> plist, double w, double h, LaserGRBL.GrblFile.L2LConf cnf)
		{
			Clipper c = new Clipper();
			AddGridSubject(c, w, h, cnf);
			AddGridClip(plist, c);

			PolyTree solution = new PolyTree();
			bool succeeded = c.Execute(ClipType.ctIntersection, solution, PolyFillType.pftEvenOdd, PolyFillType.pftEvenOdd);

			if (!succeeded)
				return null;

			List<List<IntPoint>> ll = Clipper.OpenPathsFromPolyTree(solution);
			return ToPotraceList(ll, false);
		}

		private static List<List<Curve>> BuildInsetFilling(List<List<Curve>> plist, LaserGRBL.GrblFile.L2LConf cnf)
		{
			double spacing = cnf.res / cnf.fres;
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

		static void AddGridSubject(Clipper c, double w, double h, LaserGRBL.GrblFile.L2LConf cnf)
		{
			LaserGRBL.RasterConverter.ImageProcessor.Direction dir = cnf.dir;

			double step = cnf.res / cnf.fres;
			double dstep = step * Math.Sqrt(2); //step for diagonal (1.414)
			double rdstep = step * (1 / Math.Sqrt(2)); //step for diagonal (1.414)

			List<List<IntPoint>> paths = new List<List<IntPoint>>();

			//se si vuole sfasare per via dell'offset è necessario applicare questa correzione a x e y
			//ma i risultati possono essere brutti rispetto a ciò che ci si aspetta
			//double stepmm = 1 / cnf.fres;
			//double cX = (stepmm - cnf.oX % stepmm) * cnf.res;
			//double cY = (stepmm - cnf.oY % stepmm) * cnf.res;

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
				double hs = dstep / 2;	//halfstep

				for (int i = 1; i < (w + h) / dstep; i++)
				{
					List<IntPoint> list = new List<IntPoint>();

					if (i % 2 == 1)
					{
						double my = 0;
						double mx = (i * dstep) - hs;

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
						double my = (i * dstep) - hs;

						AddPathPoint(list, mx-1, my+1); //non togliere questo +/-1, sembra che freghi l'algoritmo clipper obbligandolo a mantenere la mia direzione
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
			if (dir == LaserGRBL.RasterConverter.ImageProcessor.Direction.NewHilbert)
			{
				int n = 6;
				float ts = (float)(Math.Pow(2, n) * step);
				//genera un elemento da n ricorsioni su step
				//la sua dimensione sarà 2^n * step
				List <PointF> texel = Hilbert.Execute(n, (float)(step));

				for (int y = 0; y < (h / ts); y++)
				{
					for (int x = 0; x < (w / ts); x++)
					{
						List<IntPoint> list = new List<IntPoint>();
						for (int i = 0; i < texel.Count; i++)
							AddPathPoint(list, texel[i].X + (x * ts), texel[i].Y + (y * ts));
						AddPathPoints(paths, list);
					}
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


		internal class Hilbert
		{
			private Hilbert() { }

			public static List<PointF> Execute(int depth, float delta)
			{
				List<PointF> rv = new List<PointF>();
				float dy = 0;
				float dx = delta;
				rv.Add(new PointF(0, 0));
				Hilbert h = new Hilbert();
				h.DoHilbert(rv, depth, dx, dy);
				return rv;
			}

			// Draw a Hilbert curve.
			private void DoHilbert(List<PointF> list, int depth, float dx, float dy)
			{
				if (depth > 1) DoHilbert(list, depth - 1, dy, dx);
				list.Add(new PointF(list[list.Count - 1].X + dx, list[list.Count - 1].Y + dy));
				if (depth > 1) DoHilbert(list, depth - 1, dx, dy);
				list.Add(new PointF(list[list.Count - 1].X + dy, list[list.Count - 1].Y + dx));
				if (depth > 1) DoHilbert(list, depth - 1, dx, dy);
				list.Add(new PointF(list[list.Count - 1].X - dx, list[list.Count - 1].Y - dy));
				if (depth > 1) DoHilbert(list, depth - 1, -dy, -dx);
			}

		}

	}
}
