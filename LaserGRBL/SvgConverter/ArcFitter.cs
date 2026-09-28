//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Collections.Generic;
using Point = System.Windows.Point;

namespace LaserGRBL.SvgConverter
{
	/// <summary>
	/// Replace a finely sampled curve (i.e. a spline) with lines and circular arcs that stay within a tolerance from
	/// every sample: much shorter gcode (G2/G3) and a smoother motion than thousands of short G1 moves
	/// </summary>
	public static class ArcFitter
	{
		public const string ToleranceSetting = "Vector.SplineArcTolerance";
		public const double DefaultTolerance = 0.01; // mm
		private const double MaxRadius = 10000;      // mm, flatter arcs are lines for the controller

		/// <summary>
		/// The segments that go from points[0] to the last point, each one ending on one of the points
		/// </summary>
		public static List<VectorSegment> Fit(List<Point> points, double tolerance)
		{
			List<VectorSegment> rv = new List<VectorSegment>();
			int n = points.Count;
			int i = 0;
			while (i < n - 1)
			{
				// the longest range starting from i that fits: grow it doubling, then binary search the limit
				int good = i + 1;
				VectorSegment goodSegment = VectorSegment.Line(points[good]);
				int bad = -1;
				int probe = Math.Min(n - 1, i + 2);
				while (probe > good)
				{
					VectorSegment s = FitRange(points, i, probe, tolerance);
					if (s == null)
					{
						bad = probe;
						break;
					}
					good = probe;
					goodSegment = s;
					probe = Math.Min(n - 1, i + (probe - i) * 2);
				}
				while (bad > good + 1)
				{
					int mid = (good + bad) / 2;
					VectorSegment s = FitRange(points, i, mid, tolerance);
					if (s == null) bad = mid;
					else { good = mid; goodSegment = s; }
				}

				rv.Add(goodSegment);
				i = good;
			}
			return rv;
		}

		// a line or an arc from points[i] to points[j] passing within tolerance from all the points between them
		private static VectorSegment FitRange(List<Point> p, int i, int j, double tol)
		{
			if (LineFits(p, i, j, tol))
				return VectorSegment.Line(p[j]);

			// circle through the first, the middle and the last point
			Point a = p[i], b = p[(i + j) / 2], c = p[j];
			double d = 2 * (a.X * (b.Y - c.Y) + b.X * (c.Y - a.Y) + c.X * (a.Y - b.Y));
			if (Math.Abs(d) < 1e-12)
				return null;
			double a2 = a.X * a.X + a.Y * a.Y, b2 = b.X * b.X + b.Y * b.Y, c2 = c.X * c.X + c.Y * c.Y;
			Point center = new Point((a2 * (b.Y - c.Y) + b2 * (c.Y - a.Y) + c2 * (a.Y - b.Y)) / d, (a2 * (c.X - b.X) + b2 * (a.X - c.X) + c2 * (b.X - a.X)) / d);
			double r = VectorSegment.Distance(a, center);
			if (r > MaxRadius)
				return null;

			// direction from the turn of the three points, then every point must be on the circle and in order along the arc
			bool ccw = (b.X - a.X) * (c.Y - b.Y) - (b.Y - a.Y) * (c.X - b.X) > 0;
			double start = Math.Atan2(a.Y - center.Y, a.X - center.X);
			double sweep = Progress(start, Math.Atan2(c.Y - center.Y, c.X - center.X), ccw);
			if (sweep < 1e-9 || sweep > 2 * Math.PI - 1e-6)
				return null;

			double last = 0;
			for (int k = i + 1; k < j; k++)
			{
				if (Math.Abs(VectorSegment.Distance(p[k], center) - r) > tol)
					return null;
				double progress = Progress(start, Math.Atan2(p[k].Y - center.Y, p[k].X - center.X), ccw);
				if (progress < last - 1e-9 || progress > sweep + 1e-9)
					return null;
				last = progress;
			}
			return VectorSegment.Arc(c, center, ccw);
		}

		// angle travelled from start to angle in the given direction, in [0, 2PI)
		private static double Progress(double start, double angle, bool ccw)
		{
			double rv = ccw ? angle - start : start - angle;
			while (rv < 0) rv += 2 * Math.PI;
			while (rv >= 2 * Math.PI) rv -= 2 * Math.PI;
			return rv;
		}

		private static bool LineFits(List<Point> p, int i, int j, double tol)
		{
			Point a = p[i], b = p[j];
			double dx = b.X - a.X, dy = b.Y - a.Y, len2 = dx * dx + dy * dy;
			for (int k = i + 1; k < j; k++)
			{
				double t = len2 > 0 ? ((p[k].X - a.X) * dx + (p[k].Y - a.Y) * dy) / len2 : 0;
				if (t < 0 || t > 1)
					return false;
				double x = a.X + t * dx - p[k].X, y = a.Y + t * dy - p[k].Y;
				if (x * x + y * y > tol * tol)
					return false;
			}
			return true;
		}
	}
}
