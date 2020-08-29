using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;


//Copyright (C) 2001-2016 Peter Selinger
//Copyright (C) 2009-2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.
// You should have received a copy of the GNU General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. 

namespace CsPotrace
{
	public partial class Potrace
	{
		#region Potrace classes and contants
		public static TurnPolicy turnpolicy = TurnPolicy.minority;

		//----------------------Potrace Constants and aux functions
		const int POTRACE_CORNER = 1;
		const int POTRACE_CURVETO = 2;
		//static double COS179 = Math.Cos(179 * Math.PI / 180);

		/// <summary>
		/// area of largest path to be ignored
		/// </summary>
		public static int turdsize = 2;
		/// <summary>
		///  corner threshold
		/// </summary>
		public static double alphamax = 1.0;
		/// <summary>
		///  use curve optimization
		///  optimize the path p, replacing sequences of Bezier segments by a
		///  single segment when possible.
		/// </summary>
		public static bool curveoptimizing = true;
		/// <summary>
		/// curve optimization tolerance
		/// </summary>
		public static double opttolerance = 0.2;
		public static double Treshold = 0.5;

		Bitmap_p bm = null;
		List<Path> pathlist = new List<Path>();

		#endregion

		#region Static function of Potrace
		/// <summary>
		/// 
		/// Produces a binary Matrix with Dimensions
		/// For the threshold, we take the sum of  weighted R,g,b value. The sum of weights must be 1.
		/// The result fills the field bm;
		/// </summary>
		/// <param name="bitmap"> A Bitmap, which will be transformed to a binary Matrix</param>
		/// <returns>Returns a binaray boolean Matrix </returns>
		void ConvertBitmap(Bitmap bitmap)
		{

			byte[] Result = new byte[bitmap.Width * bitmap.Height];
			BitmapData SourceData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

			unsafe
			{
				byte* SourcePtr = (byte*)(void*)SourceData.Scan0;

				int l = Result.Length;
				for (int i = 0; i < l; i++)
				{
					//  if ((0.2126 * (double)SourcePtr[4 * i + 2] + 0.7153 * (double)SourcePtr[4 * i + 1] + 0.0721 * (double)SourcePtr[4 * i]) < Treshold*255)
					if (((double)SourcePtr[4 * i + 2] + (double)SourcePtr[4 * i + 1] + (double)SourcePtr[4 * i]) < Treshold * 255 * 3)
						Result[i] = 1;
					else
						Result[i] = 0;
				}
			}

			bitmap.UnlockBits(SourceData);
			bm = new Bitmap_p(bitmap.Width, bitmap.Height);
			bm.data = Result;
		}
		/// <summary>
		/// Searches a x and a y such that source[x,y] = 1 and source[x+1,y] 0.
		/// If this not exists, false will be returned else the result is true. 
		/// 
		/// </summary>
		/// <param name="source">Is a Binary Matrix, which is produced by <see cref="BitMapToBinary"/>
		/// <param name="x">x index in the source Matrix</param>
		/// <param name="y">y index in the source Matrix</param>
		static bool findNext(Bitmap_p bm1, Point point, ref Point result)
		{
			int i = bm1.w * point.Y + point.X;
			while ((i < bm1.size) && (bm1.data[i] != 1))
			{
				i++;
			}
			if (i >= bm1.size) return false;
			result = bm1.index(i);
			return true;
		}
		/// <summary>
		/// Compute a path in the binary matrix.
		/// Start path at the point (x0,x1), which must be an upper left corner
		/// of the path. Also compute the area enclosed by the path. Return a
		/// new path_t object, or NULL on error (note that a legitimate path
		/// cannot have length 0). 
		/// </summary>
		/// <param name="Matrix">Binary Matrix</param>
		/// <returns></returns>
		/// <param name="x">x index in the source Matrix</param>
		/// <param name="y">y index in the source Matrix</param>
		Path findPath(Bitmap_p bm1, Point point)
		{
			Path path = new Path();
			int x = point.X;
			int y = point.Y;
			int dirx = 0;
			int diry = 1;
			int tmp = -1;

			path.sign = bm.at(point.X, point.Y) ? "+" : "-";

			while (true)
			{
				path.pt.Add(new Point(x, y));
				if (x > path.maxX)
					path.maxX = x;
				if (x < path.minX)
					path.minX = x;
				if (y > path.maxY)
					path.maxY = y;
				if (y < path.minY)
					path.minY = y;
				path.len++;

				x += dirx;
				y += diry;
				path.area -= x * diry;

				if (x == point.X && y == point.Y)
					break;

				bool l = bm1.at(x + (dirx + diry - 1) / 2, y + (diry - dirx - 1) / 2);
				bool r = bm1.at(x + (dirx - diry - 1) / 2, y + (diry + dirx - 1) / 2);

				if (r && !l)
				{
					if ((turnpolicy == TurnPolicy.right) ||
					(((turnpolicy == TurnPolicy.black) && (path.sign == "+"))) ||
					(((turnpolicy == TurnPolicy.white) && (path.sign == "-"))) ||
					(((turnpolicy == TurnPolicy.majority) && (majority(bm1, x, y)))) ||
					((turnpolicy == TurnPolicy.minority && !majority(bm1, x, y))))
					{
						tmp = dirx;
						dirx = -diry;
						diry = tmp;
					}
					else
					{
						tmp = dirx;
						dirx = diry;
						diry = -tmp;
					}
				}
				else if (r)
				{
					tmp = dirx;
					dirx = -diry;
					diry = tmp;
				}
				else if (!l)
				{
					tmp = dirx;
					dirx = diry;
					diry = -tmp;
				}
			}
			return path;
		}
		/// <summary>
		/// Decompose the given bitmap into paths. Returns a linked list of
		/// Path objects with the fields len, pt, area filled
		/// </summary>
		/// <param name="bm">A binary bitmap which holds the imageinformations.</param>
		/// <param name="plistp">List of Path objects</param>
		Path bmToPathlist()
		{

			Bitmap_p bm1 = bm.copy();
			Point currentPoint = new Point(0, 0);
			Path path = new Path();

			bool weiter = findNext(bm1, currentPoint, ref currentPoint);
			while (weiter)
			{

				path = findPath(bm1, currentPoint);

				xorPath(bm1, path);

				if (path.area > turdsize)
				{
					pathlist.Add(path);
				}
				weiter = findNext(bm1, currentPoint, ref currentPoint);
			}




			return path;
		}
		static void xorPath(Bitmap_p bm1, Path path)
		{
			int y1 = path.pt[0].Y,
			  len = path.len,
			  x, y, maxX, minY, i, j;
			for (i = 1; i < len; i++)
			{
				x = path.pt[i].X;
				y = path.pt[i].Y;

				if (y != y1)
				{
					minY = y1 < y ? y1 : y;
					maxX = path.maxX;
					for (j = x; j < maxX; j++)
					{
						bm1.flip(j, minY);
					}
					y1 = y;
				}
			}
		}
		/* ---------------------------------------------------------------------- */
		/*  */
		/// <summary>
		///Preparation: fill in the sum* fields of a path (used for later
		///rapid summing). 
		/// </summary>
		/// <param name="pp">Path for which the preparation will be done</param>
		/// <returns></returns>
		static void calcSums(Path path)
		{
			double x, y;
			// origin 
			path.x0 = path.pt[0].X;
			path.y0 = path.pt[0].Y;


			List<Sum> s = path.sums;
			s.Add(new Sum(0, 0, 0, 0, 0));
			for (int i = 0; i < path.len; i++)
			{
				x = path.pt[i].X - path.x0;
				y = path.pt[i].Y - path.y0;
				s.Add(new Sum(s[i].x + x, s[i].y + y, s[i].xy + x * y,
					s[i].x2 + x * x, s[i].y2 + y * y));
			}
		}
		/* ---------------------------------------------------------------------- */
		/* Stage 2: calculate the optimal polygon (Sec. 2.2.2-2.2.4). */

		/* Auxiliary function: calculate the penalty of an edge from i to j in
		   the given path. This needs the "lon" and "sum*" data. */
		static double penalty3(Path path, int i, int j)
		{

			int n = path.len;
			List<Point> pt = path.pt;
			List<Sum> sums = path.sums;

			double x, y, xy, x2, y2;
			double a, b, c, s,
			  px, py, ex, ey;
			int r = 0;
			int k = 0;

			if (j >= n)
			{
				j -= n;
				r = 1;
			}

			if (r == 0)
			{
				x = sums[j + 1].x - sums[i].x;
				y = sums[j + 1].y - sums[i].y;
				x2 = sums[j + 1].x2 - sums[i].x2;
				xy = sums[j + 1].xy - sums[i].xy;
				y2 = sums[j + 1].y2 - sums[i].y2;
				k = j + 1 - i;
			}
			else
			{
				x = sums[j + 1].x - sums[i].x + sums[n].x;
				y = sums[j + 1].y - sums[i].y + sums[n].y;
				x2 = sums[j + 1].x2 - sums[i].x2 + sums[n].x2;
				xy = sums[j + 1].xy - sums[i].xy + sums[n].xy;
				y2 = sums[j + 1].y2 - sums[i].y2 + sums[n].y2;
				k = j + 1 - i + n;
			}

			px = (pt[i].X + pt[j].X) / 2.0 - pt[0].X;
			py = (pt[i].Y + pt[j].Y) / 2.0 - pt[0].Y;
			ey = (pt[j].X - pt[i].X);
			ex = -(pt[j].Y - pt[i].Y);

			a = ((x2 - 2 * x * px) / k + px * px);
			b = ((xy - x * py - y * px) / k + px * py);
			c = ((y2 - 2 * y * py) / k + py * py);

			s = ex * ex * a + 2 * ex * ey * b + ey * ey * c;

			return Math.Sqrt(s);
		}

		static void calcLon(Path path)
		{

			int n = path.len;
			List<Point> pt = path.pt;

			int dir;
			int[] pivk = new int[n];
			int[] nc = new int[n];

			int[] ct = new int[4];

			path.lon = new int[n];

			Point[] constraint = new Point[2];
			Point cur = new Point();
			Point off = new Point();
			Point dk = new Point();
			int foundk;

			int j, k1;
			int a, b, c, d;
			/* initialize the nc data structure. Point from each point to the
			   furthest future point to which it is connected by a vertical or
			   horizontal segment. We take advantage of the fact that there is
			   always a direction change at 0 (due to the path decomposition
			   algorithm). But even if this were not so, there is no harm, as
			   in practice, correctness does not depend on the word "furthest"
			   above.  */
			int k = 0;
			/* determine pivot points: for each i, let pivk[i] be the furthest k
			   such that all j with i<j<k lie on a line connecting i,k. */

			for (int i = n - 1; i >= 0; i--)
			{
				if (pt[i].X != pt[k].X && pt[i].Y != pt[k].Y)
				{
					k = i + 1;
				}
				nc[i] = k;
			}

			for (int i = n - 1; i >= 0; i--)
			{
				ct[0] = ct[1] = ct[2] = ct[3] = 0;
				dir = (3 + 3 * (pt[mod(i + 1, n)].X - pt[i].X) +
					(pt[mod(i + 1, n)].Y - pt[i].Y)) / 2;
				ct[dir]++;

				constraint[0].X = 0;
				constraint[0].Y = 0;
				constraint[1].X = 0;
				constraint[1].Y = 0;

				k = nc[i];
				k1 = i;
				while (true)
				{
					foundk = 0;
					dir = (3 + 3 * sign(pt[k].X - pt[k1].X) +
						sign(pt[k].Y - pt[k1].Y)) / 2;
					ct[dir]++;

					if ((ct[0] == 1) && (ct[1] == 1) && (ct[2] == 1) && (ct[3] == 1))
					{
						pivk[i] = k1;
						foundk = 1;
						break;
					}

					cur.X = pt[k].X - pt[i].X;
					cur.Y = pt[k].Y - pt[i].Y;

					if (xprodi(constraint[0], cur) < 0 || xprodi(constraint[1], cur) > 0)
					{
						break;
					}

					if (Math.Abs(cur.X) <= 1 && Math.Abs(cur.Y) <= 1)
					{

					}
					else
					{
						off.X = cur.X + ((cur.Y >= 0 && (cur.Y > 0 || cur.X < 0)) ? 1 : -1);
						off.Y = cur.Y + ((cur.X <= 0 && (cur.X < 0 || cur.Y < 0)) ? 1 : -1);
						if (xprodi(constraint[0], off) >= 0)
						{
							constraint[0].X = off.X;
							constraint[0].Y = off.Y;
						}
						off.X = cur.X + ((cur.Y <= 0 && (cur.Y < 0 || cur.X < 0)) ? 1 : -1);
						off.Y = cur.Y + ((cur.X >= 0 && (cur.X > 0 || cur.Y < 0)) ? 1 : -1);
						if (xprodi(constraint[1], off) <= 0)
						{
							constraint[1].X = off.X;
							constraint[1].Y = off.Y;
						}
					}
					k1 = k;
					k = nc[k1];
					if (!cyclic(k, i, k1))
					{
						break;
					}
				}
				if (foundk == 0)
				{
					dk.X = sign(pt[k].X - pt[k1].X);
					dk.Y = sign(pt[k].Y - pt[k1].Y);
					cur.X = pt[k1].X - pt[i].X;
					cur.Y = pt[k1].Y - pt[i].Y;

					a = xprodi(constraint[0], cur);
					b = xprodi(constraint[0], dk);
					c = xprodi(constraint[1], cur);
					d = xprodi(constraint[1], dk);

					j = 10000000;
					if (b < 0)
					{
						j = a / -b;
					}
					if (d > 0)
					{
						j = Math.Min(j, -c / d);
					}
					pivk[i] = mod(k1 + j, n);
				}
			}

			j = pivk[n - 1];
			path.lon[n - 1] = j;
			for (int i = n - 2; i >= 0; i--)
			{
				if (cyclic(i + 1, pivk[i], j))
				{
					j = pivk[i];
				}
				path.lon[i] = j;
			}

			for (int i = n - 1; cyclic(mod(i + 1, n), j, path.lon[i]); i--)
			{
				path.lon[i] = j;
			}
		}

		/* find the optimal polygon. Fill in the m and po components. Return 1
		  on failure with errno set, else 0. Non-cyclic version: assumes i=0
		  is in the polygon. Fixme: ### implement cyclic version. */
		static void bestPolygon(Path path)
		{

			double thispen, best;
			int i, j, m, k;
			int n = path.len;
			int c;
			int[] clip0 = new int[n];
			double[] pen = new double[n + 1];
			int[] prev = new int[n + 1];
			int[] clip1 = new int[n + 1];
			int[] seg0 = new int[n + 1];
			int[] seg1 = new int[n + 1];



			for (i = 0; i < n; i++)
			{
				c = mod(path.lon[mod(i - 1, n)] - 1, n);
				if (c == i)
				{
					c = mod(i + 1, n);
				}
				if (c < i)
				{
					clip0[i] = n;
				}
				else
				{
					clip0[i] = c;
				}
			}

			j = 1;
			for (i = 0; i < n; i++)
			{
				while (j <= clip0[i])
				{
					clip1[j] = i;
					j++;
				}
			}

			i = 0;
			for (j = 0; i < n; j++)
			{
				seg0[j] = i;
				i = clip0[i];
			}
			seg0[j] = n;
			m = j;

			i = n;
			for (j = m; j > 0; j--)
			{
				seg1[j] = i;
				i = clip1[i];
			}
			seg1[0] = 0;

			pen[0] = 0;
			/* now find the shortest path with m segments, based on penalty3 */
			/* note: the outer 2 loops jointly have at most n interations, thus
			the worst-case behavior here is quadratic. In practice, it is
			close to linear since the inner loop tends to be short. */
			for (j = 1; j <= m; j++)
			{
				for (i = seg1[j]; i <= seg0[j]; i++)
				{
					best = -1;
					for (k = seg0[j - 1]; k >= clip1[i]; k--)
					{
						thispen = penalty3(path, k, i) + pen[k];
						if (best < 0 || thispen < best)
						{
							prev[i] = k;
							best = thispen;
						}
					}
					pen[i] = best;
				}
			}
			path.m = m;
			path.po = new int[m];
			/* read off shortest path */
			for (i = n, j = m - 1; i > 0; j--)
			{
				i = prev[i];
				path.po[j] = i;
			}
		}

		/* Stage 3: vertex adjustment (Sec. 2.3.1). */

		/* Adjust vertices of optimal polygon: calculate the intersection of
		   the two "optimal" line segments, then move it into the unit square
		   if it lies outside. Return 1 with errno set on error; 0 on
		   success. */

		/* calculate "optimal" point-slope representation for each line segment */

		static void adjustVertices(Path path)
		{


			int m = path.m;
			int[] po = path.po;
			int n = path.len;
			List<Point> pt = path.pt;
			double x0 = path.x0;
			double y0 = path.y0;
			dPoint[] ctr = new dPoint[m];
			dPoint[] dir = new dPoint[m];
			Quad[] q = new Quad[m];
			int i, j, k, l;
			double[] v = new double[3];

			dPoint s = new dPoint();
			double d;
			path.curve = new privcurve(m);
			/* calculate "optimal" point-slope representation for each line
					segment */
			for (i = 0; i < m; i++)
			{
				j = po[mod(i + 1, m)];
				j = mod(j - po[i], n) + po[i];
				ctr[i] = new dPoint();
				dir[i] = new dPoint();
				pointslope(path, po[i], j, ctr[i], dir[i]);
			}
			/* represent each line segment as a singular quadratic form; the
						 distance of a point (x,y) from the line segment will be
						 (x,y,1)Q(x,y,1)^t, where Q=q[i]. */
			for (i = 0; i < m; i++)
			{
				q[i] = new Quad();
				d = dir[i].X * dir[i].X + dir[i].Y * dir[i].Y;
				if (d == 0.0)
				{
					for (j = 0; j < 3; j++)
					{
						for (k = 0; k < 3; k++)
						{
							q[i].data[j * 3 + k] = 0;
						}
					}
				}
				else
				{
					v[0] = dir[i].Y;
					v[1] = -dir[i].X;
					v[2] = -v[1] * ctr[i].Y - v[0] * ctr[i].X;
					for (l = 0; l < 3; l++)
					{
						for (k = 0; k < 3; k++)
						{
							q[i].data[l * 3 + k] = v[l] * v[k] / d;
						}
					}
				}
			}

			double dx, dy, det;
			int z;
			double xmin, ymin; /* coordinates of minimum */
			double min, cand; /* minimum and candidate for minimum of quad. form */
			/* now calculate the "intersections" of consecutive segments.
			   Instead of using the actual intersection, we find the point
			   within a given unit square which minimizes the square distance to
			   the two lines. */
			for (i = 0; i < m; i++)
			{
				Quad Q = new Quad();
				dPoint w = new dPoint();
				/* let s be the vertex, in coordinates relative to x0/y0 */
				s.X = pt[po[i]].X - x0;
				s.Y = pt[po[i]].Y - y0;
				/* intersect segments i-1 and i */
				j = mod(i - 1, m);
				/* add quadratic forms */
				for (l = 0; l < 3; l++)
				{
					for (k = 0; k < 3; k++)
					{
						Q.data[l * 3 + k] = q[j].at(l, k) + q[i].at(l, k);
					}
				}

				while (true)
				{
					/* minimize the quadratic form Q on the unit square */
					/* find intersection */
					det = Q.at(0, 0) * Q.at(1, 1) - Q.at(0, 1) * Q.at(1, 0);
					if (det != 0.0)
					{
						w.X = (-Q.at(0, 2) * Q.at(1, 1) + Q.at(1, 2) * Q.at(0, 1)) / det;
						w.Y = (Q.at(0, 2) * Q.at(1, 0) - Q.at(1, 2) * Q.at(0, 0)) / det;
						break;
					}
					/* matrix is singular - lines are parallel. Add another,
								  orthogonal axis, through the center of the unit square */
					if (Q.at(0, 0) > Q.at(1, 1))
					{
						v[0] = -Q.at(0, 1);
						v[1] = Q.at(0, 0);
					}
					else if (Q.at(1, 1) != 0.0)
					{
						v[0] = -Q.at(1, 1);
						v[1] = Q.at(1, 0);
					}
					else
					{
						v[0] = 1;
						v[1] = 0;
					}
					d = v[0] * v[0] + v[1] * v[1];
					v[2] = -v[1] * s.Y - v[0] * s.X;
					for (l = 0; l < 3; l++)
					{
						for (k = 0; k < 3; k++)
						{
							Q.data[l * 3 + k] += v[l] * v[k] / d;
						}
					}
				}
				dx = Math.Abs(w.X - s.X);
				dy = Math.Abs(w.Y - s.Y);
				if (dx <= 0.5 && dy <= 0.5)
				{

					path.curve.vertex[i] = new dPoint(w.X + x0, w.Y + y0);
					continue;
				}
				/* the minimum was not in the unit square; now minimize quadratic
				   on boundary of square */
				min = quadform(Q, s);
				xmin = s.X;
				ymin = s.Y;

				if (Q.at(0, 0) != 0.0)
				{
					for (z = 0; z < 2; z++)
					{
						/* value of the y-coordinate */
						w.Y = s.Y - 0.5 + z;
						w.X = -(Q.at(0, 1) * w.Y + Q.at(0, 2)) / Q.at(0, 0);
						dx = Math.Abs(w.X - s.X);
						cand = quadform(Q, w);
						if (dx <= 0.5 && cand < min)
						{
							min = cand;
							xmin = w.X;
							ymin = w.Y;
						}
					}
				}

				if (Q.at(1, 1) != 0.0)
				{
					for (z = 0; z < 2; z++)
					{
						/* value of the x-coordinate */
						w.X = s.X - 0.5 + z;
						w.Y = -(Q.at(1, 0) * w.X + Q.at(1, 2)) / Q.at(1, 1);
						dy = Math.Abs(w.Y - s.Y);
						cand = quadform(Q, w);
						if (dy <= 0.5 && cand < min)
						{
							min = cand;
							xmin = w.X;
							ymin = w.Y;
						}
					}
				}
				/* check four corners */
				for (l = 0; l < 2; l++)
				{
					for (k = 0; k < 2; k++)
					{
						w.X = s.X - 0.5 + l;
						w.Y = s.Y - 0.5 + k;
						cand = quadform(Q, w);
						if (cand < min)
						{
							min = cand;
							xmin = w.X;
							ymin = w.Y;
						}
					}
				}

				path.curve.vertex[i] = new dPoint(xmin + x0, ymin + y0);
			}
		}

		/* ---------------------------------------------------------------------- */
		/* Stage 4: smoothing and corner analysis (Sec. 2.3.3) */
		static void reverse(Path path)
		{
			privcurve curve = path.curve;
			int m = curve.n;
			dPoint[] v = curve.vertex;
			int i, j;
			dPoint tmp;

			for (i = 0, j = m - 1; i < j; i++, j--)
			{
				tmp = v[i];
				v[i] = v[j];
				v[j] = tmp;
			}

		}
		/* Always succeeds and returns 0 */
		static void smooth(Path path)
		{
			int m = path.curve.n;
			privcurve curve = path.curve;
			if (path.sign == "-")
				reverse(path);
			int i, j, k;
			double dd, denom, alpha;
			dPoint p2, p3, p4;
			/* examine each vertex and find its best fit */
			for (i = 0; i < m; i++)
			{
				j = mod(i + 1, m);
				k = mod(i + 2, m);
				p4 = interval(1 / 2.0, curve.vertex[k], curve.vertex[j]);

				denom = ddenom(curve.vertex[i], curve.vertex[k]);
				if (denom != 0.0)
				{
					dd = dpara(curve.vertex[i], curve.vertex[j], curve.vertex[k]) / denom;
					dd = Math.Abs(dd);
					alpha = dd > 1 ? (1 - 1.0 / dd) : 0;
					alpha = alpha / 0.75;
				}
				else
				{
					alpha = 4 / 3.0;
				}
				curve.alpha0[j] = alpha;   /* remember "original" value of alpha */

				if (alpha >= alphamax)
				{
					curve.tag[j] = POTRACE_CORNER;
					curve.c[3 * j + 1] = curve.vertex[j];
					curve.c[3 * j + 2] = p4;
				}
				else
				{
					if (alpha < 0.55)
					{
						alpha = 0.55;
					}
					else if (alpha > 1)
					{
						alpha = 1;
					}
					p2 = interval(0.5 + 0.5 * alpha, curve.vertex[i], curve.vertex[j]);
					p3 = interval(0.5 + 0.5 * alpha, curve.vertex[k], curve.vertex[j]);
					curve.tag[j] = POTRACE_CURVETO;
					curve.c[3 * j + 0] = p2;
					curve.c[3 * j + 1] = p3;
					curve.c[3 * j + 2] = p4;
				}
				curve.alpha[j] = alpha;  /* store the "cropped" value of alpha */
				curve.beta[j] = 0.5;
			}
			curve.alphacurve = 1;
		}
		/* ---------------------------------------------------------------------- */
		/* Stage 5: Curve optimization (Sec. 2.4) */

		/* calculate best fit from i+.5 to j+.5.  Assume i<j (cyclically).
				Return 0 and set badness and parameters (alpha, beta), if
				possible. Return 1 if impossible. */
		static int opti_penalty(Path path, int i, int j, Opti res, double opttolerance,
			 int[] convc, double[] areac)
		{
			int m = path.curve.n;
			privcurve curve = path.curve;
			dPoint[] vertex = curve.vertex;
			int k, k1, k2, conv, i1;
			double area, alpha, d, d1, d2;
			dPoint p0, p1, p2, p3, pt;
			double A, R, A1, A2, A3, A4,
			  s, t;
			/* check convexity, corner-freeness, and maximum bend < 179 degrees */
			if (i == j)
			{ /* sanity - a full loop can never be an opticurve */
				return 1;
			}

			k = i;
			i1 = mod(i + 1, m);
			k1 = mod(k + 1, m);
			conv = convc[k1];
			if (conv == 0)
			{
				return 1;
			}
			d = ddist(vertex[i], vertex[i1]);
			for (k = k1; k != j; k = k1)
			{
				k1 = mod(k + 1, m);
				k2 = mod(k + 2, m);
				if (convc[k1] != conv)
				{
					return 1;
				}
				if (sign(cprod(vertex[i], vertex[i1], vertex[k1], vertex[k2])) !=
					conv)
				{
					return 1;
				}
				if (iprod1(vertex[i], vertex[i1], vertex[k1], vertex[k2]) <
					d * ddist(vertex[k1], vertex[k2]) * -0.999847695156)
				{
					return 1;
				}
			}
			/* the curve we're working in: */
			p0 = curve.c[mod(i, m) * 3 + 2].copy();
			p1 = vertex[mod(i + 1, m)].copy();
			p2 = vertex[mod(j, m)].copy();
			p3 = curve.c[mod(j, m) * 3 + 2].copy();
			/* determine its area */
			area = areac[j] - areac[i];
			area -= dpara(vertex[0], curve.c[i * 3 + 2], curve.c[j * 3 + 2]) / 2;
			if (i >= j)
			{
				area += areac[m];
			}
			/* find intersection o of p0p1 and p2p3. Let t,s such that o =
					 interval(t,p0,p1) = interval(s,p3,p2). Let A be the area of the
					 triangle (p0,o,p3). */
			A1 = dpara(p0, p1, p2);
			A2 = dpara(p0, p1, p3);
			A3 = dpara(p0, p2, p3);

			A4 = A1 + A3 - A2;

			if (A2 == A1)
			{/* this should never happen */
				return 1;
			}

			t = A3 / (A3 - A4);
			s = A2 / (A2 - A1);
			A = A2 * t / 2.0;

			if (A == 0.0)
			{
				/* this should never happen */
				return 1;
			}

			R = area / A; /* relative area */
			alpha = 2 - Math.Sqrt(4 - R / 0.3); /* overall alpha for p0-o-p3 curve */

			res.c[0] = interval(t * alpha, p0, p1);
			res.c[1] = interval(s * alpha, p3, p2);
			res.alpha = alpha;
			res.t = t;
			res.s = s;

			p1 = res.c[0].copy();
			p2 = res.c[1].copy(); /* the proposed curve is now (p0,p1,p2,p3) */

			res.pen = 0;
			/* calculate penalty */
			/* check tangency with edges */
			for (k = mod(i + 1, m); k != j; k = k1)
			{
				k1 = mod(k + 1, m);
				t = tangent(p0, p1, p2, p3, vertex[k], vertex[k1]);
				if (t < -0.5)
				{
					return 1;
				}
				pt = bezier(t, p0, p1, p2, p3);
				d = ddist(vertex[k], vertex[k1]);
				if (d == 0.0)
				{
					/* this should never happen */
					return 1;
				}
				d1 = dpara(vertex[k], vertex[k1], pt) / d;
				if (Math.Abs(d1) > opttolerance)
				{
					return 1;
				}
				if (iprod(vertex[k], vertex[k1], pt) < 0 ||
					iprod(vertex[k1], vertex[k], pt) < 0)
				{
					return 1;
				}
				res.pen += d1 * d1;
			}
			/* check corners */
			for (k = i; k != j; k = k1)
			{
				k1 = mod(k + 1, m);
				t = tangent(p0, p1, p2, p3, curve.c[k * 3 + 2], curve.c[k1 * 3 + 2]);
				if (t < -0.5)
				{
					return 1;
				}
				pt = bezier(t, p0, p1, p2, p3);
				d = ddist(curve.c[k * 3 + 2], curve.c[k1 * 3 + 2]);
				if (d == 0.0)
				{
					/* this should never happen */
					return 1;
				}
				d1 = dpara(curve.c[k * 3 + 2], curve.c[k1 * 3 + 2], pt) / d;
				d2 = dpara(curve.c[k * 3 + 2], curve.c[k1 * 3 + 2], vertex[k1]) / d;
				d2 *= 0.75 * curve.alpha[k1];
				if (d2 < 0)
				{
					d1 = -d1;
					d2 = -d2;
				}
				if (d1 < d2 - opttolerance)
				{
					return 1;
				}
				if (d1 < d2)
				{
					res.pen += (d1 - d2) * (d1 - d2);
				}
			}

			return 0;
		}

		/* optimize the path p, replacing sequences of Bezier segments by a
	   single segment when possible. Return 0 on success, 1 with errno set
	   on failure. */
		static void optiCurve(Path path)
		{
			privcurve curve = path.curve;
			int m = curve.n;
			dPoint[] vert = curve.vertex;
			int[] pt = new int[m + 1];
			double[] pen = new double[m + 1];
			int[] len = new int[m + 1];
			Opti[] opt = new Opti[m + 1];
			Opti o = new Opti();
			int om, i, j, r;
			dPoint p0;
			int i1;
			double area, alpha;
			privcurve ocurve;
			int[] convc = new int[m];  /* conv[m]: pre-computed convexities */
			double[] areac = new double[m + 1];
			/* pre-calculate convexity: +1 = right turn, -1 = left turn, 0 = corner */
			for (i = 0; i < m; i++)
			{
				if (curve.tag[i] == POTRACE_CURVETO)
				{
					convc[i] = sign(dpara(vert[mod(i - 1, m)], vert[i], vert[mod(i + 1, m)]));
				}
				else
				{
					convc[i] = 0;
				}
			}
			/* pre-calculate areas */
			area = 0.0;
			areac[0] = 0.0;
			p0 = curve.vertex[0];
			for (i = 0; i < m; i++)
			{
				i1 = mod(i + 1, m);
				if (curve.tag[i1] == POTRACE_CURVETO)
				{
					alpha = curve.alpha[i1];
					area += 0.3 * alpha * (4 - alpha) *
						dpara(curve.c[i * 3 + 2], vert[i1], curve.c[i1 * 3 + 2]) / 2;
					area += dpara(p0, curve.c[i * 3 + 2], curve.c[i1 * 3 + 2]) / 2;
				}
				areac[i + 1] = area;
			}

			pt[0] = -1;
			pen[0] = 0;
			len[0] = 0;

			/* Fixme: we always start from a fixed point -- should find the best
					  curve cyclically ### */
			for (j = 1; j <= m; j++)
			{
				/* calculate best path from 0 to j */
				pt[j] = j - 1;
				pen[j] = pen[j - 1];
				len[j] = len[j - 1] + 1;

				for (i = j - 2; i >= 0; i--)
				{
					r = opti_penalty(path, i, mod(j, m), o, opttolerance, convc,
						areac);
					if (r == 1)
					{
						break;
					}
					if (len[j] > len[i] + 1 ||
						(len[j] == len[i] + 1 && pen[j] > pen[i] + o.pen))
					{
						pt[j] = i;
						pen[j] = pen[i] + o.pen;
						len[j] = len[i] + 1;
						opt[j] = o;
						o = new Opti();
					}
				}
			}
			om = len[m];
			ocurve = new privcurve(om);
			double[] s = new double[om];
			double[] t = new double[om];

			j = m;
			for (i = om - 1; i >= 0; i--)
			{
				if (pt[j] == j - 1)
				{
					ocurve.tag[i] = curve.tag[mod(j, m)];
					ocurve.c[i * 3 + 0] = curve.c[mod(j, m) * 3 + 0];
					ocurve.c[i * 3 + 1] = curve.c[mod(j, m) * 3 + 1];
					ocurve.c[i * 3 + 2] = curve.c[mod(j, m) * 3 + 2];
					ocurve.vertex[i] = curve.vertex[mod(j, m)];
					ocurve.alpha[i] = curve.alpha[mod(j, m)];
					ocurve.alpha0[i] = curve.alpha0[mod(j, m)];
					ocurve.beta[i] = curve.beta[mod(j, m)];
					s[i] = t[i] = 1.0;
				}
				else
				{
					ocurve.tag[i] = POTRACE_CURVETO;
					ocurve.c[i * 3 + 0] = opt[j].c[0];
					ocurve.c[i * 3 + 1] = opt[j].c[1];
					ocurve.c[i * 3 + 2] = curve.c[mod(j, m) * 3 + 2];
					ocurve.vertex[i] = interval(opt[j].s, curve.c[mod(j, m) * 3 + 2],
												 vert[mod(j, m)]);
					ocurve.alpha[i] = opt[j].alpha;
					ocurve.alpha0[i] = opt[j].alpha;
					s[i] = opt[j].s;
					t[i] = opt[j].t;
				}
				j = pt[j];
			}
			/* calculate beta parameters */
			for (i = 0; i < om; i++)
			{
				i1 = mod(i + 1, om);
				ocurve.beta[i] = s[i] / (s[i] + t[i1]);
			}
			ocurve.alphacurve = 1;
			path.curve = ocurve;
		}
		
		static bool majority(Bitmap_p bm1, int x, int y)
		{
			int i;
			int a;
			int ct;
			for (i = 2; i < 5; i++)
			{
				ct = 0;
				for (a = -i + 1; a <= i - 1; a++)
				{
					ct += bm1.at(x + a, y + i - 1) ? 1 : -1;
					ct += bm1.at(x + i - 1, y + a - 1) ? 1 : -1;
					ct += bm1.at(x + a - 1, y - i) ? 1 : -1;
					ct += bm1.at(x - i, y + a) ? 1 : -1;
				}
				if (ct > 0)
				{
					return true;
				}
				else if (ct < 0)
				{
					return false;
				}
			}
			return false;
		}


		void tracetoList(List<List<Curve>> ListOfPathes)
		{
			if (ListOfPathes == null) return;
			for (int i = 0; i < pathlist.Count; i++)
			{
				Path P = pathlist[i];
				List<Curve> CurveList = new List<Curve>();
				ListOfPathes.Add(CurveList);
				dPoint L = P.curve.c[(P.curve.n - 1) * 3 + 2];
				for (int j = 0; j < P.curve.n; j++)
				{
					dPoint A = P.curve.c[j * 3 + 1];
					dPoint B = P.curve.c[j * 3 + 2];

					if (P.curve.tag[j] == POTRACE_CORNER)
					{
						CurveList.Add(new Curve(CurveKind.Line, L, L, A, A));
						CurveList.Add(new Curve(CurveKind.Line, A, A, B, B));
					}
					else
					{
						dPoint CP = P.curve.c[j * 3];
						CurveList.Add(new Curve(CurveKind.Bezier, L, CP, A, B));
					}
					L = B;
				}
			}
		}

		void Clear()
		{
			bm = null;
			pathlist.Clear();
		}

		public List<List<Curve>> PotraceTrace(Bitmap Bitmap)
		{
			Clear();

			ConvertBitmap(Bitmap);
			bmToPathlist();
			for (int i = 0; i < pathlist.Count; i++)
			{
				calcSums(pathlist[i]);
				calcLon(pathlist[i]);
				bestPolygon(pathlist[i]);
				adjustVertices(pathlist[i]);

				smooth(pathlist[i]);
				if (curveoptimizing)
					optiCurve(pathlist[i]);

			}

			List<List<Curve>> rv = new List<List<Curve>>();
			tracetoList(rv);
			return rv;
		}
		#endregion

		/*
		#region create svg
		static string toString(double value)
		{
			return string.Format(System.Globalization.CultureInfo.GetCultureInfo("en-US"), "{0:0.000}", value);
		}
		static string segment(privcurve curve, int i, double size)
		{
			string s = "\nL " + toString(curve.c[i * 3 + 1].X * size) + ' ' +
			   toString(curve.c[i * 3 + 1].Y * size) + ' ';
			s += toString(curve.c[i * 3 + 2].X * size) + ' ' +
			   toString(curve.c[i * 3 + 2].Y * size) + ' ';
			return s;
		}
		static string bezier(privcurve curve, int i, double size)
		{
			string b = "\nC " + toString(curve.c[i * 3 + 0].X * size) + ' ' +
			  toString(curve.c[i * 3 + 0].Y * size) + ' ';
			b += toString(curve.c[i * 3 + 1].X * size) + ' ' +
			   toString(curve.c[i * 3 + 1].Y * size) + ' ';
			b += toString(curve.c[i * 3 + 2].X * size) + ' ' +
			   toString(curve.c[i * 3 + 2].Y * size) + ' ';
			return b;
		}
		static string path(privcurve curve, double size)
		{


			int n = curve.n, i;
			string p = "\nM " + toString(curve.c[(n - 1) * 3 + 2].X * size) +
				' ' + toString(curve.c[(n - 1) * 3 + 2].Y * size) + ' ';
			for (i = 0; i < n; i++)
			{
				if (curve.tag[i] == POTRACE_CURVETO)
				{
					p += bezier(curve, i, size);
				}
				else if (curve.tag[i] == POTRACE_CORNER)
				{
					p += segment(curve, i, size);
				}
			}
			//p += 
			return p;
		}
		public static string getSVG()
		{
			double size = 1;
			int w = (int)(bm.w * size);
			int h = (int)(bm.h * size);
			int len = pathlist.Count;
			privcurve c;
			int i;
			string strokec, fillc, fillrule;

			string svg = "<svg id=\"svg\" version=\"1.1\" width=\"" + w.ToString() + "\" height=\"" + h.ToString() + "\""
		+ " xmlns=\"http://www.w3.org/2000/svg\">";
			svg += "<path d=\"";
			for (i = 0; i < len; i++)
			{

				c = pathlist[i].curve;
				svg += path(c, size);

			}
			//if (opt_type == "curve") {
			//  strokec = "black";
			//  fillc = "none";
			//  fillrule = "";
			//} else {
			strokec = "";
			fillc = "black";

			fillrule = " fill-rule=\"evenodd\"";

			//}
			svg += "\"\n  stroke=\"" + strokec + "\" fill=\"" + fillc + "\" " + fillrule + "/></svg>";
			//    svg += "\n\" stroke="' + strokec + '" fill="' + fillc + '"' + fillrule + '/></svg>
			//  svg +="Z \"/></g></svg>";
			return svg;
		}
#endregion
		*/
	}
}
