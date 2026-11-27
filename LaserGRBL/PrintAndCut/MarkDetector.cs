// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA.

using System;
using System.Collections.Generic;

namespace LaserGRBL.PrintAndCut
{
	/// <summary>Axis-aligned bounding box, in mm.</summary>
	public struct Bounds
	{
		/// <summary>Left edge.</summary>
		public readonly double MinX;
		/// <summary>Bottom edge.</summary>
		public readonly double MinY;
		/// <summary>Right edge.</summary>
		public readonly double MaxX;
		/// <summary>Top edge.</summary>
		public readonly double MaxY;

		/// <summary>Creates a box from its edges; the edges are sorted if given in the wrong order.</summary>
		public Bounds(double minX, double minY, double maxX, double maxY)
		{
			MinX = Math.Min(minX, maxX);
			MinY = Math.Min(minY, maxY);
			MaxX = Math.Max(minX, maxX);
			MaxY = Math.Max(minY, maxY);
		}

		/// <summary>Width of the box.</summary>
		public double Width { get { return MaxX - MinX; } }
		/// <summary>Height of the box.</summary>
		public double Height { get { return MaxY - MinY; } }
		/// <summary>Center of the box.</summary>
		public Vec2 Center { get { return new Vec2((MinX + MaxX) / 2, (MinY + MaxY) / 2); } }

		/// <summary>Smallest box containing both boxes.</summary>
		public Bounds Union(Bounds other)
		{ return new Bounds(Math.Min(MinX, other.MinX), Math.Min(MinY, other.MinY), Math.Max(MaxX, other.MaxX), Math.Max(MaxY, other.MaxY)); }

		/// <summary>True if the two boxes are closer than <paramref name="gap"/> (touching counts).</summary>
		public bool IsNear(Bounds other, double gap)
		{
			return MinX - gap <= other.MaxX && other.MinX - gap <= MaxX
				&& MinY - gap <= other.MaxY && other.MinY - gap <= MaxY;
		}

		/// <summary>Bounding box of a set of points.</summary>
		/// <exception cref="ArgumentException">If there are no points.</exception>
		public static Bounds FromPoints(IEnumerable<Vec2> points)
		{
			if (points == null) throw new ArgumentNullException("points");

			bool any = false;
			double minX = 0, minY = 0, maxX = 0, maxY = 0;
			foreach (Vec2 p in points)
			{
				if (!any) { minX = maxX = p.X; minY = maxY = p.Y; any = true; }
				else
				{
					minX = Math.Min(minX, p.X); maxX = Math.Max(maxX, p.X);
					minY = Math.Min(minY, p.Y); maxY = Math.Max(maxY, p.Y);
				}
			}
			if (!any) throw new ArgumentException("At least one point is required.", "points");
			return new Bounds(minX, minY, maxX, maxY);
		}
	}

	/// <summary>A registration mark: one or more strokes of the reserved color close to each other.</summary>
	public sealed class RegistrationMark
	{
		internal RegistrationMark(int number, Bounds bounds, int strokeCount, bool isOversized)
		{
			Number = number;
			Bounds = bounds;
			StrokeCount = strokeCount;
			IsOversized = isOversized;
		}

		/// <summary>1-based number shown to the user (reading order: top to bottom, left to right).</summary>
		public int Number { get; private set; }
		/// <summary>Bounding box of all the strokes of the mark.</summary>
		public Bounds Bounds { get; private set; }
		/// <summary>Center of the bounding box: the point associated to Set First / Second Target Position.</summary>
		public Vec2 Center { get { return Bounds.Center; } }
		/// <summary>Number of strokes merged in this mark.</summary>
		public int StrokeCount { get; private set; }
		/// <summary>True if the mark is bigger than expected: probably several marks merged together.</summary>
		public bool IsOversized { get; private set; }
	}

	/// <summary>
	/// Groups the strokes drawn with the reserved color into registration marks.
	/// Strokes whose bounding boxes are closer than a tolerance belong to the same mark
	/// (a cross made of two lines is one mark).
	/// </summary>
	public static class MarkDetector
	{
		/// <summary>Default gap (mm) under which two strokes belong to the same mark.</summary>
		public const double DefaultTolerance = 1.0;

		/// <summary>Default size (mm) above which a mark is reported as oversized.</summary>
		public const double DefaultMaxMarkSize = 20.0;

		/// <summary>Groups the strokes with the default tolerance and maximum size.</summary>
		public static List<RegistrationMark> Detect(IList<Bounds> strokes)
		{
			return Detect(strokes, DefaultTolerance, DefaultMaxMarkSize);
		}

		/// <summary>
		/// Groups the strokes into marks, numbered in reading order (top to bottom, then left to right;
		/// Y grows upwards as in machine coordinates).
		/// </summary>
		/// <param name="strokes">Bounding box of each stroke of the reserved color, in mm.</param>
		/// <param name="tolerance">Maximum gap (mm) between two strokes of the same mark.</param>
		/// <param name="maxMarkSize">Width or height (mm) above which a mark is flagged as oversized.</param>
		public static List<RegistrationMark> Detect(IList<Bounds> strokes, double tolerance, double maxMarkSize)
		{
			if (strokes == null) throw new ArgumentNullException("strokes");
			if (tolerance < 0) throw new ArgumentOutOfRangeException("tolerance");
			if (maxMarkSize <= 0) throw new ArgumentOutOfRangeException("maxMarkSize");

			List<Bounds> groups = new List<Bounds>();
			List<int> counts = new List<int>();
			foreach (Bounds b in strokes)
			{
				groups.Add(b);
				counts.Add(1);
			}

			// merge until stable: a group that grows may come close to another one
			bool merged = true;
			while (merged)
			{
				merged = false;
				for (int i = 0; i < groups.Count && !merged; i++)
				{
					for (int j = i + 1; j < groups.Count && !merged; j++)
					{
						if (groups[i].IsNear(groups[j], tolerance))
						{
							groups[i] = groups[i].Union(groups[j]);
							counts[i] += counts[j];
							groups.RemoveAt(j);
							counts.RemoveAt(j);
							merged = true;
						}
					}
				}
			}

			List<int> order = new List<int>();
			for (int i = 0; i < groups.Count; i++) order.Add(i);
			order.Sort(delegate (int a, int b)
			{
				int byY = groups[b].Center.Y.CompareTo(groups[a].Center.Y); // top first
				return byY != 0 ? byY : groups[a].Center.X.CompareTo(groups[b].Center.X);
			});

			List<RegistrationMark> marks = new List<RegistrationMark>();
			foreach (int i in order)
			{
				bool oversized = groups[i].Width > maxMarkSize || groups[i].Height > maxMarkSize;
				marks.Add(new RegistrationMark(marks.Count + 1, groups[i], counts[i], oversized));
			}
			return marks;
		}
	}
}
