//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using CsPotrace;
using System;
using System.Collections.Generic;
using System.Linq;
using Point = System.Windows.Point;

namespace LaserGRBL.SvgConverter
{
	/// <summary>
	/// Build the hatch lines that fill closed svg shapes, reusing the vector filling of the raster vectorizer
	/// </summary>
	static class SvgFilling
	{
		/// <summary>
		/// Return the polylines that fill the given shapes (even-odd rule: nested shapes are holes, like the inside of a letter "O")
		/// </summary>
		/// <param name="shapes">polylines in mm, open ones are joined when their ends touch</param>
		/// <param name="dir">filling pattern</param>
		/// <param name="linesPerMM">distance between lines is 1 / linesPerMM</param>
		public static List<List<Point>> Build(List<List<Point>> shapes, RasterConverter.ImageProcessor.Direction dir, double linesPerMM)
		{
			List<List<Point>> rv = new List<List<Point>>();
			shapes = JoinOpenPaths(shapes).Where(s => s.Count >= 3).ToList();
			if (shapes.Count == 0 || linesPerMM <= 0)
				return rv;

			// the filling pattern is generated in the rectangle (0,0)-(w,h): move the shapes inside it, with one line of margin
			double step = 1.0 / linesPerMM;
			double minX = shapes.Min(s => s.Min(p => p.X)) - step;
			double minY = shapes.Min(s => s.Min(p => p.Y)) - step;
			double w = shapes.Max(s => s.Max(p => p.X)) - minX + step;
			double h = shapes.Max(s => s.Max(p => p.Y)) - minY + step;

			List<List<Curve>> plist = new List<List<Curve>>();
			foreach (List<Point> shape in shapes)
			{
				List<Curve> curves = new List<Curve>();
				for (int i = 0; i < shape.Count; i++)
				{
					Point a = shape[i];
					Point b = shape[(i + 1) % shape.Count]; // always closed
					dPoint da = new dPoint(a.X - minX, a.Y - minY);
					dPoint db = new dPoint(b.X - minX, b.Y - minY);
					curves.Add(new Curve(CurveKind.Line, da, da, db, db));
				}
				plist.Add(curves);
			}

			GrblFile.L2LConf conf = new GrblFile.L2LConf();
			conf.res = 1; // coordinates are already in mm
			conf.fres = linesPerMM;
			conf.dir = dir;

			List<List<Curve>> flist = PotraceClipper.BuildFilling(plist, w, h, conf);
			if (flist == null)
				return rv;

			flist = GrblFile.ParallelOptimizePaths(flist.Where(l => l.Count > 0).ToList(), 0);

			foreach (List<Curve> curves in flist)
			{
				List<Point> polyline = new List<Point>(curves.Count + 1);
				polyline.Add(new Point(curves[0].A.X + minX, curves[0].A.Y + minY));
				foreach (Curve c in curves)
					polyline.Add(new Point(c.B.X + minX, c.B.Y + minY));
				rv.Add(polyline);
			}

			return rv;
		}

		private const double JoinTolerance = 0.05; // mm

		/// <summary>
		/// Join open paths whose ends touch into longer paths. Files exported from CAD (dxf) often store
		/// each segment of a shape as a separate path: they must be joined to get the closed shape to fill.
		/// </summary>
		internal static List<List<Point>> JoinOpenPaths(List<List<Point>> shapes)
		{
			List<List<Point>> rv = new List<List<Point>>();
			List<List<Point>> open = new List<List<Point>>();
			foreach (List<Point> shape in shapes)
			{
				if (shape.Count < 2)
					continue;
				if (Near(shape[0], shape[shape.Count - 1]))
					rv.Add(shape);
				else
					open.Add(shape);
			}

			// index the endpoints of open paths on a grid, to find touching ends quickly
			Dictionary<long, List<int>> index = new Dictionary<long, List<int>>();
			for (int i = 0; i < open.Count; i++)
			{
				AddToIndex(index, open[i][0], i);
				AddToIndex(index, open[i][open[i].Count - 1], i);
			}

			bool[] used = new bool[open.Count];
			for (int i = 0; i < open.Count; i++)
			{
				if (used[i])
					continue;

				used[i] = true;
				List<Point> chain = new List<Point>(open[i]);

				// grow the chain at the end, then at the start (by reversing it)
				for (int side = 0; side < 2; side++)
				{
					int next;
					while (!Near(chain[0], chain[chain.Count - 1]) && (next = FindTouching(index, open, used, chain[chain.Count - 1])) >= 0)
					{
						used[next] = true;
						List<Point> segment = open[next];
						if (!Near(segment[0], chain[chain.Count - 1]))
							segment = Enumerable.Reverse(segment).ToList();
						chain.AddRange(segment.Skip(1));
					}
					chain.Reverse();
				}

				rv.Add(chain);
			}

			return rv;
		}

		private static bool Near(Point a, Point b)
		{ return Math.Abs(a.X - b.X) <= JoinTolerance && Math.Abs(a.Y - b.Y) <= JoinTolerance; }

		private static long CellKey(long cx, long cy)
		{ return (cx << 32) ^ (cy & 0xFFFFFFFF); }

		private static void AddToIndex(Dictionary<long, List<int>> index, Point p, int i)
		{
			long key = CellKey((long)Math.Floor(p.X / JoinTolerance), (long)Math.Floor(p.Y / JoinTolerance));
			List<int> list;
			if (!index.TryGetValue(key, out list))
				index.Add(key, list = new List<int>());
			list.Add(i);
		}

		private static int FindTouching(Dictionary<long, List<int>> index, List<List<Point>> open, bool[] used, Point p)
		{
			long cx = (long)Math.Floor(p.X / JoinTolerance);
			long cy = (long)Math.Floor(p.Y / JoinTolerance);
			for (long dx = -1; dx <= 1; dx++)
			{
				for (long dy = -1; dy <= 1; dy++)
				{
					List<int> list;
					if (index.TryGetValue(CellKey(cx + dx, cy + dy), out list))
						foreach (int i in list)
							if (!used[i] && (Near(open[i][0], p) || Near(open[i][open[i].Count - 1], p)))
								return i;
				}
			}
			return -1;
		}
	}
}
