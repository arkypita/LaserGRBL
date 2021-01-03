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
		private const int resolution = 10;

		internal static List<List<Curve>> BuildFilling(List<List<Curve>> plist, double spacing, double w, double h, LaserGRBL.RasterConverter.ImageProcessor.Direction dir)
		{
			if (dir == LaserGRBL.RasterConverter.ImageProcessor.Direction.NewInsetFilling)
				return BuildInsetFilling(plist, spacing);

			List<List<Curve>> flist = new List<List<Curve>>();

			Clipper c = new Clipper();

			AddGridSubject(c, spacing, w, h, dir);

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

			long t1 = Tools.HiResTimer.TotalMilliseconds;
			PolyTree solution = new PolyTree();
			bool succeeded = c.Execute(ClipType.ctIntersection, solution, PolyFillType.pftEvenOdd, PolyFillType.pftEvenOdd);

			long t2 = Tools.HiResTimer.TotalMilliseconds;
			System.Diagnostics.Debug.WriteLine($"ClipperExecute: {t2 - t1}ms");

			//GraphicsPath result = new GraphicsPath();

			List<List<IntPoint>> ll = Clipper.OpenPathsFromPolyTree(solution);
			foreach (List<IntPoint> pg in ll)
			{
				PointF[] pts = PolygonToPointFArray(pg, resolution);
				if (pts.Count() > 2)
					;// result.AddPolygon(pts);
				else
				{
					dPoint a = new dPoint(pts[0].X, pts[0].Y);
					dPoint b = new dPoint(pts[1].X, pts[1].Y);
					flist.Add(new List<Curve>() { new Curve(CurveKind.Line, a, a, b, b) });
				}
				//result.AddLine(pts[0], pts[1]);
			}


			return flist;
		}

		private static List<List<Curve>> BuildInsetFilling(List<List<Curve>> plist, double spacing)
		{
			int loop = 1;
			bool empty = false;
			List<List<Curve>> flist = new List<List<Curve>>();

			ClipperOffset c = new ClipperOffset();
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

			while (!empty)
			{
				long t1 = Tools.HiResTimer.TotalMilliseconds;
				List<List<IntPoint>> solution = new List<List<IntPoint>>();
				c.Execute(ref solution, -spacing * loop * resolution);

				loop++;

				long t2 = Tools.HiResTimer.TotalMilliseconds;
				System.Diagnostics.Debug.WriteLine($"ClipperExecute: {t2 - t1}ms");

				//GraphicsPath result = new GraphicsPath();
				if (solution.Count == 0)
					empty = true;

				foreach (List<IntPoint> pg in solution)
				{
					PointF[] pts = PolygonToPointFArray(pg, resolution);
					if (pts.Count() > 2)
					{
						List<Curve> curve = new List<Curve>();
						for (int i = 0; i < pts.Length - 1; i++)
						{
							dPoint a = new dPoint(pts[i].X, pts[i].Y);
							dPoint b = new dPoint(pts[i + 1].X, pts[i + 1].Y);
							curve.Add(new Curve(CurveKind.Line, a, a, b, b));
						}

						//chiudi la figura
						dPoint a1 = new dPoint(pts[pts.Length - 1].X, pts[pts.Length - 1].Y);
						dPoint b1 = new dPoint(pts[0].X, pts[0].Y);
						curve.Add(new Curve(CurveKind.Line, a1, a1, b1, b1));
						flist.Add(curve);
					}
					else
					{
						dPoint a = new dPoint(pts[0].X, pts[0].Y);
						dPoint b = new dPoint(pts[1].X, pts[1].Y);
						flist.Add(new List<Curve>() { new Curve(CurveKind.Line, a, a, b, b) });
					}
					//result.AddLine(pts[0], pts[1]);
				}
			}

			return flist;
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

			long t1 = Tools.HiResTimer.TotalMilliseconds;

			double dstep = step * Math.Sqrt(2); //step for diagonal (1.414)
			List<List<IntPoint>> paths = new List<List<IntPoint>>();

			if (dir == LaserGRBL.RasterConverter.ImageProcessor.Direction.NewVertical || dir == LaserGRBL.RasterConverter.ImageProcessor.Direction.NewGrid)
			{
				double x = 0;
				double hPres = h * resolution;
				while (x <= w + step)
				{
					double xPres = x * resolution;
					paths.Add(new List<IntPoint>()
					{
						new IntPoint(xPres, 0),
						new IntPoint(xPres, hPres)
					});
					x += step;
				}
			}
			if (dir == LaserGRBL.RasterConverter.ImageProcessor.Direction.NewHorizontal || dir == LaserGRBL.RasterConverter.ImageProcessor.Direction.NewGrid)
			{
				double y = 0;
				double wPres = w * resolution;
				while (y <= h + step)
				{
					double yPres = y * resolution;
					paths.Add(new List<IntPoint>()
					{
						new IntPoint(0, yPres),
						new IntPoint(wPres, yPres)
					});

					y += step;
				}
			}

			if (dir == LaserGRBL.RasterConverter.ImageProcessor.Direction.NewDiagonal || dir == LaserGRBL.RasterConverter.ImageProcessor.Direction.NewDiagonalGrid)
			{
				double i = 0;
				while (i <= (2 * w) + step || i <= (2 * h) + step)
				{
					double iPres = i * resolution;
					paths.Add(new List<IntPoint>()
					{
						new IntPoint(0, iPres),
						new IntPoint(iPres, 0)
					});

					i += dstep;
				}
			}
			if (dir == LaserGRBL.RasterConverter.ImageProcessor.Direction.NewReverseDiagonal || dir == LaserGRBL.RasterConverter.ImageProcessor.Direction.NewDiagonalGrid)
			{
				double i = 0;
				while (i <= (2 * w) + step || i <= (2 * h) + step)
				{
					paths.Add(new List<IntPoint>()
					{
						new IntPoint(0, (h - i) * resolution),
						new IntPoint(i * resolution, h * resolution)
					});

					i += dstep;
				}
			}

			if (dir == LaserGRBL.RasterConverter.ImageProcessor.Direction.NewCross)
			{
				double crosslen = step / 3 * resolution;
				double x = 0;
				while (x <= w + step)
				{
					double y = 0;
					while (y <= h + step)
					{
						paths.Add(new List<IntPoint>()
						{
							new IntPoint(x * resolution, y * resolution - crosslen),
							new IntPoint(x * resolution, y * resolution + crosslen)
						});

						paths.Add(new List<IntPoint>()
						{
							new IntPoint(x * resolution - crosslen, y * resolution),
							new IntPoint(x * resolution + crosslen, y * resolution)
						});

						y += step;
					}
					x += step;
				}
			}

			if (dir == LaserGRBL.RasterConverter.ImageProcessor.Direction.NewDiagonalCross)
			{


				double crosslen = step / 3 * resolution;
				double x = 0;
				while (x <= w + step)
				{
					double xPres = x * resolution;

					double y = 0;
					while (y <= h + step)
					{
						double yPres = y * resolution;

						paths.Add(new List<IntPoint>()
						{
							new IntPoint(xPres - crosslen, yPres - crosslen),
							new IntPoint(xPres + crosslen, yPres + crosslen)
						});

						paths.Add(new List<IntPoint>()
						{
							new IntPoint(xPres - crosslen, yPres + crosslen),
							new IntPoint(xPres + crosslen, yPres - crosslen)
						});

						y += step;
					}
					x += step;
				}
			}

			if (dir == LaserGRBL.RasterConverter.ImageProcessor.Direction.NewSquares)
			{


				double crosslen = step / 3 * resolution;
				double x = 0;
				while (x <= w + step)
				{
					double xPres = x * resolution;

					double y = 0;
					while (y <= h + step)
					{
						double yPres = y * resolution;

						paths.Add(new List<IntPoint>()
						{
							new IntPoint(xPres - crosslen, yPres),
							new IntPoint(xPres, yPres + crosslen),
						});

						paths.Add(new List<IntPoint>()
						{
							new IntPoint(xPres, yPres + crosslen),
							new IntPoint(xPres + crosslen, yPres),
						});

						paths.Add(new List<IntPoint>()
						{
							new IntPoint(xPres + crosslen, yPres),
							new IntPoint(xPres, yPres - crosslen),
						});

						paths.Add(new List<IntPoint>()
						{
							new IntPoint(xPres, yPres - crosslen),
							new IntPoint(xPres - crosslen, yPres)
						});

						y += step;
					}
					x += step;
				}
			}

			long t2 = Tools.HiResTimer.TotalMilliseconds;

			System.Diagnostics.Debug.WriteLine($"GeneratePath: {t2 - t1}ms");

			c.AddPaths(paths, PolyType.ptSubject, false);

			long t3 = Tools.HiResTimer.TotalMilliseconds;
			System.Diagnostics.Debug.WriteLine($"AddPath: {t3 - t2}ms");
		}

		static PointF[] PolygonToPointFArray(List<IntPoint> pg, float scale)
		{
			PointF[] result = new PointF[pg.Count];
			for (int i = 0; i < pg.Count; ++i)
			{
				result[i].X = (float)pg[i].X / scale;
				result[i].Y = (float)pg[i].Y / scale;
			}
			return result;
		}


	}
}
