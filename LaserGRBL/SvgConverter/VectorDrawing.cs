//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Collections.Generic;
using System.Linq;
using Point = System.Windows.Point;

namespace LaserGRBL.SvgConverter
{
	/// <summary>
	/// Piece of a vector path, from the end of the previous one: a straight line or a circular arc
	/// </summary>
	public class VectorSegment
	{
		public Point End;
		public bool IsArc;
		public Point Center;    // arcs only
		public bool CCW;        // arcs only: counterclockwise (G3), otherwise clockwise (G2)

		public static VectorSegment Line(Point end)
		{
			VectorSegment s = new VectorSegment();
			s.End = end;
			return s;
		}

		public static VectorSegment Arc(Point end, Point center, bool ccw)
		{
			VectorSegment s = new VectorSegment();
			s.End = end;
			s.IsArc = true;
			s.Center = center;
			s.CCW = ccw;
			return s;
		}

		public double Radius
		{ get { return Distance(End, Center); } }

		// signed angle from start to end around the center (positive = counterclockwise), never a full turn
		public double Sweep(Point start)
		{
			double a1 = Math.Atan2(start.Y - Center.Y, start.X - Center.X);
			double a2 = Math.Atan2(End.Y - Center.Y, End.X - Center.X);
			double sweep = a2 - a1;
			if (CCW && sweep <= 1e-12) sweep += 2 * Math.PI;
			if (!CCW && sweep >= -1e-12) sweep -= 2 * Math.PI;
			return sweep;
		}

		public static double Distance(Point a, Point b)
		{ return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y)); }
	}

	/// <summary>
	/// Connected sequence of lines and arcs of one color, in mm with Y pointing up (like the gcode)
	/// </summary>
	public class VectorPath
	{
		public string Color;    // normalized "#RRGGBB"
		public Point Start;
		public List<VectorSegment> Segments = new List<VectorSegment>();
		public bool Closed;

		public VectorPath(string color, Point start)
		{
			Color = color;
			Start = start;
		}

		public Point End
		{ get { return Segments.Count > 0 ? Segments[Segments.Count - 1].End : Start; } }

		/// <summary>
		/// Approximation with straight lines, arcs within the given distance (mm): for preview and filling
		/// </summary>
		public List<Point> ToPolyline(double tolerance)
		{
			List<Point> rv = new List<Point>();
			rv.Add(Start);
			Point current = Start;
			foreach (VectorSegment s in Segments)
			{
				if (s.IsArc)
				{
					double r = s.Radius;
					double sweep = s.Sweep(current);
					double a1 = Math.Atan2(current.Y - s.Center.Y, current.X - s.Center.X);
					double step = r > tolerance ? 2 * Math.Acos(1 - tolerance / r) : Math.PI / 4;
					int n = Math.Max(1, Math.Min(10000, (int)Math.Ceiling(Math.Abs(sweep) / Math.Min(step, Math.PI / 4))));
					for (int i = 1; i < n; i++)
					{
						double a = a1 + sweep * i / n;
						rv.Add(new Point(s.Center.X + r * Math.Cos(a), s.Center.Y + r * Math.Sin(a)));
					}
				}
				rv.Add(s.End);
				current = s.End;
			}
			return rv;
		}

		// bounds: the end points of every segment and, for the arcs, the quadrant points inside their sweep
		public void ExtendBounds(ref double minX, ref double minY, ref double maxX, ref double maxY)
		{
			Extend(Start, ref minX, ref minY, ref maxX, ref maxY);
			Point current = Start;
			foreach (VectorSegment s in Segments)
			{
				Extend(s.End, ref minX, ref minY, ref maxX, ref maxY);
				if (s.IsArc)
				{
					double r = s.Radius;
					double start = Math.Atan2(current.Y - s.Center.Y, current.X - s.Center.X);
					double end = start + s.Sweep(current);
					double from = Math.Min(start, end), to = Math.Max(start, end);
					for (double q = Math.Ceiling(from / (Math.PI / 2)) * (Math.PI / 2); q <= to; q += Math.PI / 2)
						Extend(new Point(s.Center.X + r * Math.Cos(q), s.Center.Y + r * Math.Sin(q)), ref minX, ref minY, ref maxX, ref maxY);
				}
				current = s.End;
			}
		}

		private static void Extend(Point p, ref double minX, ref double minY, ref double maxX, ref double maxY)
		{
			minX = Math.Min(minX, p.X); maxX = Math.Max(maxX, p.X);
			minY = Math.Min(minY, p.Y); maxY = Math.Max(maxY, p.Y);
		}

		public void Translate(double dx, double dy)
		{
			Start = new Point(Start.X + dx, Start.Y + dy);
			foreach (VectorSegment s in Segments)
			{
				s.End = new Point(s.End.X + dx, s.End.Y + dy);
				s.Center = new Point(s.Center.X + dx, s.Center.Y + dy);
			}
		}
	}

	/// <summary>
	/// A drawing imported as lines and true arcs, without intermediate formats: the arcs can become G2/G3
	/// </summary>
	public class VectorDrawing
	{
		public List<VectorPath> Paths = new List<VectorPath>();

		public bool GetBounds(out double minX, out double minY, out double maxX, out double maxY)
		{
			minX = minY = double.MaxValue;
			maxX = maxY = double.MinValue;
			foreach (VectorPath p in Paths)
				p.ExtendBounds(ref minX, ref minY, ref maxX, ref maxY);
			return minX <= maxX;
		}

		/// <summary>
		/// Move the drawing so that its bounds start at 0,0 (as done for the svg files)
		/// </summary>
		public void MoveToOrigin()
		{
			double minX, minY, maxX, maxY;
			if (GetBounds(out minX, out minY, out maxX, out maxY))
				foreach (VectorPath p in Paths)
					p.Translate(-minX, -minY);
		}

		/// <summary>
		/// The colors in order of first appearance
		/// </summary>
		public List<string> Colors()
		{
			return Paths.Select(p => p.Color).Distinct().ToList();
		}
	}
}
