using CsPotrace.BezierToBiarc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

namespace CsPotrace
{
    public partial class Potrace
    {
		#region auxiliary functions
		/* range over the straight line segment [a,b] when lambda ranges over [0,1] */
		static dPoint interval(double lambda, dPoint a, dPoint b)
		{
			dPoint res = new dPoint();

			res.X = a.X + lambda * (b.X - a.X);
			res.Y = a.Y + lambda * (b.Y - a.Y);
			return res;
		}
		/* return a direction that is 90 degrees counterclockwise from p2-p0,
				  but then restricted to one of the major wind directions (n, nw, w, etc) */
		static dPoint dorth_infty(dPoint p0, dPoint p2)
		{
			dPoint r = new dPoint();

			r.Y = sign(p2.X - p0.X);
			r.X = -sign(p2.Y - p0.Y);

			return r;
		}
		/* ddenom/dpara have the property that the square of radius 1 centered
			  at p1 intersects the line p0p2 iff |dpara(p0,p1,p2)| <= ddenom(p0,p2) */
		static double ddenom(dPoint p0, dPoint p2)
		{
			dPoint r = dorth_infty(p0, p2);

			return r.Y * (p2.X - p0.X) - r.X * (p2.Y - p0.Y);
		}
		/* return (p1-p0)x(p2-p0), the area of the parallelogram */
		static double dpara(dPoint p0, dPoint p1, dPoint p2)
		{
			double x1, y1, x2, y2;

			x1 = p1.X - p0.X;
			y1 = p1.Y - p0.Y;
			x2 = p2.X - p0.X;
			y2 = p2.Y - p0.Y;

			return x1 * y2 - x2 * y1;
		}
		/* calculate (p1-p0)x(p3-p2) */
		static double cprod(dPoint p0, dPoint p1, dPoint p2, dPoint p3)
		{
			double x1, y1, x2, y2;

			x1 = p1.X - p0.X;
			y1 = p1.Y - p0.Y;
			x2 = p3.X - p2.X;
			y2 = p3.Y - p2.Y;

			return x1 * y2 - x2 * y1;
		}
		/* calculate (p1-p0)*(p2-p0) */
		static double iprod(dPoint p0, dPoint p1, dPoint p2)
		{
			double x1, y1, x2, y2;

			x1 = p1.X - p0.X;
			y1 = p1.Y - p0.Y;
			x2 = p2.X - p0.X;
			y2 = p2.Y - p0.Y;

			return x1 * x2 + y1 * y2;
		}
		/* calculate (p1-p0)*(p3-p2) */
		static double iprod1(dPoint p0, dPoint p1, dPoint p2, dPoint p3)
		{
			double x1, y1, x2, y2;

			x1 = p1.X - p0.X;
			y1 = p1.Y - p0.Y;
			x2 = p3.X - p2.X;
			y2 = p3.Y - p2.Y;

			return x1 * x2 + y1 * y2;
		}
		/* calculate distance between two points */
		static double ddist(dPoint p, dPoint q)
		{
			return Math.Sqrt((p.X - q.X) * (p.X - q.X) + (p.Y - q.Y) * (p.Y - q.Y));
		}

		/* calculate p1 x p2 */
		static double xprod(dPoint p1, dPoint p2)
		{
			return p1.X * p2.Y - p1.Y * p2.X;
		}
		/* calculate p1 x p2 */
		static int xprodi(Point p1, Point p2)
		{
			return p1.X * p2.Y - p1.Y * p2.X;
		}
		/* return 1 if a <= b < c < a, in a cyclic sense (mod n) */
		static bool cyclic(double a, double b, double c)
		{
			if (a <= c)
			{
				return (a <= b && b < c);
			}
			else
			{
				return (a <= b || b < c);
			}
		}

		static int sign(double i)
		{
			return i > 0 ? 1 : i < 0 ? -1 : 0;
		}
		/* Apply quadratic form Q to vector w = (w.x,w.y) */
		static double quadform(Quad Q, dPoint w)
		{
			double sum = 0;
			double[] v = new double[3];
			v[0] = w.X;
			v[1] = w.Y;
			v[2] = 1;
			sum = 0.0;

			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					sum += v[i] * Q.at(i, j) * v[j];
				}
			}
			return sum;
		}

		/* calculate point of a bezier curve */
		static dPoint bezier(double t, dPoint p0, dPoint p1, dPoint p2, dPoint p3)
		{
			double s = 1 - t; dPoint res = new dPoint();
			/* Note: a good optimizing compiler (such as gcc-3) reduces the
					   following to 16 multiplications, using common subexpression
					   elimination. */
			res.X = s * s * s * p0.X + 3 * (s * s * t) * p1.X + 3 * (t * t * s) * p2.X + t * t * t * p3.X;
			res.Y = s * s * s * p0.Y + 3 * (s * s * t) * p1.Y + 3 * (t * t * s) * p2.Y + t * t * t * p3.Y;

			return res;
		}
		/* calculate the point t in [0..1] on the (convex) bezier curve
				 (p0,p1,p2,p3) which is tangent to q1-q0. Return -1.0 if there is no
				 solution in [0..1]. */
		static double tangent(dPoint p0, dPoint p1, dPoint p2, dPoint p3, dPoint q0, dPoint q1)
		{
			double A, B, C;               /* (1-t)^2 A + 2(1-t)t B + t^2 C = 0 */

			double a, b, c;   /* a t^2 + b t + c = 0 */
			double d, s, r1, r2;
			A = cprod(p0, p1, q0, q1);
			B = cprod(p1, p2, q0, q1);
			C = cprod(p2, p3, q0, q1);

			a = A - 2 * B + C;
			b = -2 * A + 2 * B;
			c = A;

			d = b * b - 4 * a * c;

			if (a == 0 || d < 0)
			{
				return -1.0;
			}

			s = Math.Sqrt(d);

			r1 = (-b + s) / (2 * a);
			r2 = (-b - s) / (2 * a);

			if (r1 >= 0 && r1 <= 1)
			{
				return r1;
			}
			else if (r2 >= 0 && r2 <= 1)
			{
				return r2;
			}
			else
			{
				return -1.0;
			}
		}

		/* determine the center and slope of the line i..j. Assume i<j. Needs
			   "sum" components of p to be set. */
		static void pointslope(Path path, int i, int j, dPoint ctr, dPoint dir)
		{
			int n = path.len;
			List<Sum> sums = path.sums;
			double x, y, x2, xy, y2;
			double a, b, c, lambda2, l;
			int k = 0;
			/* assume i<j */
			int r = 0; /* rotations from i to j */

			while (j >= n)
			{
				j -= n;
				r += 1;
			}
			while (i >= n)
			{
				i -= n;
				r -= 1;
			}
			while (j < 0)
			{
				j += n;
				r -= 1;
			}
			while (i < 0)
			{
				i += n;
				r += 1;
			}

			x = sums[j + 1].x - sums[i].x + r * sums[n].x;
			y = sums[j + 1].y - sums[i].y + r * sums[n].y;
			x2 = sums[j + 1].x2 - sums[i].x2 + r * sums[n].x2;
			xy = sums[j + 1].xy - sums[i].xy + r * sums[n].xy;
			y2 = sums[j + 1].y2 - sums[i].y2 + r * sums[n].y2;
			k = j + 1 - i + r * n;

			ctr.X = x / k;
			ctr.Y = y / k;

			a = (x2 - x * x / k) / k;
			b = (xy - x * y / k) / k;
			c = (y2 - y * y / k) / k;

			lambda2 = (a + c + Math.Sqrt((a - c) * (a - c) + 4 * b * b)) / 2;  /* larger e.value */

			a -= lambda2;
			c -= lambda2;

			if (Math.Abs(a) >= Math.Abs(c))
			{
				l = Math.Sqrt(a * a + b * b);
				if (l != 0)
				{
					dir.X = -b / l;
					dir.Y = a / l;
				}
			}
			else
			{
				l = Math.Sqrt(c * c + b * b);
				if (l != 0)
				{
					dir.X = -c / l;
					dir.Y = b / l;
				}
			}
			if (l == 0)
			{
				dir.X = dir.Y = 0;  /* sometimes this can happen when k=4:
			      the two eigenvalues coincide */
			}
		}

		/* integer arithmetic */

		static int abs(int a) { return ((a) > 0 ? (a) : -(a)); }
		static int min(int a, int b) { return ((a) < (b) ? (a) : (b)); }
		static int max(int a, int b) { return ((a) > (b) ? (a) : (b)); }
		static int sq(int a) { return ((a) * (a)); }
		static int cu(int a) { return ((a) * (a) * (a)); }

		static int mod(int a, int n)
		{
			return a >= n ? a % n : a >= 0 ? a : n - 1 - (-1 - a) % n;
		}
		static int floordiv(int a, int n)
		{
			return a >= 0 ? a / n : -1 - (-1 - a) / n;
		}
		#endregion

		private static string formatnumber(double number, double scale)
		{
			double num = number / scale;
			if (!double.IsNaN(num))
				return num.ToString("0.###", CultureInfo.InvariantCulture);
			else
				return "0";
		}

		public static PointF AsPointF(Vector2 v)
		{
			return new PointF(v.X, v.Y);
		}

	}
}
