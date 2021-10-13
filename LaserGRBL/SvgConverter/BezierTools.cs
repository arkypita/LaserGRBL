using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace LaserGRBL.SvgConverter
{
    /// <summary>
    /// Tools for high quality rendering of bezier curves
    /// Written by Darren Starr (https://github.com/darrenstarr)
    /// 
    /// There is some code with roots from the Graphics Gems volume 1 book
    /// https://www.elsevier.com/books/graphics-gems/glassner/978-0-08-050753-8
    /// and the code which is ported from the original book's code
    /// is available under a very strange license as specified in LICENSE.md at
    /// https://github.com/erich666/GraphicsGems
    /// </summary>
    internal static class BezierTools
    {
        /// <summary>
        /// Split a curve at the given value of t
        /// </summary>
        /// 
        /// This is a implementation of De Casteljau's algorithm for raising the order of the
        /// curve through matrices as is similarly showing in https://pomax.github.io/bezierinfo/#matrixsplit
        /// 
        /// This code is a modified version of what was found in the nearest point algorithm 
        /// in Graphics Gems as mentioned in the heading.
        /// 
        /// <param name="controlPoints">The control points representing the curve</param>
        /// <param name="t">The time/t to split at</param>
        /// <returns>A tuple containing two Bezier curves</returns>
        private static Tuple<Point [], Point[]> SplitCurveAtT(Point[] controlPoints, double t)
        {
            var degree = controlPoints.Length - 1;
            Point[,] Vtemp = new Point[degree + 1, degree + 1];

            // Copy control points	
            for (var j = 0; j <= degree; j++)
                Vtemp[0, j] = controlPoints[j];

            // Triangle computation	
            for (var i = 1; i <= degree; i++)
            {
                for (var j = 0; j <= degree - i; j++)
                {
                    Vtemp[i, j] = new Point
                    {
                        X = (1.0 - t) * Vtemp[i - 1, j].X + t * Vtemp[i - 1, j + 1].X,
                        Y = (1.0 - t) * Vtemp[i - 1, j].Y + t * Vtemp[i - 1, j + 1].Y
                    };
                }
            }

            var result = new Tuple<Point[], Point[]>(
                new Point[degree + 1],
                new Point[degree + 1]
            );

            for (var i = 0; i <= degree; i++)
            {
                result.Item1[i] = Vtemp[i, 0];
                result.Item2[i] = Vtemp[degree - i, i];
            }

            return result;
        }

        /// <summary>
        /// Creates an approximation of a cubic Bezier curve as a series of line segments
        /// </summary>
        /// 
        /// The algorithm is quite simple. If the curve isn't flat or small enough, then split the curve in half and recursively
        /// test each half for flatness and assemble the segments in a list.
        /// 
        /// <param name="points">The 4 control points of a segment</param>
        /// <param name="error">The linear error to enforce.</param>
        /// <returns></returns>
        private static IEnumerable<Point> FlattenSegmentTo(IEnumerable<Point> points, double error, int subdivisions, int maxSubdivisions)
        {
            // Convert the points to an array
            var segment = points.ToArray();

            // Base case: test how flat or small each approximating "triangle" has become. If it's below our error
            // then we are done since it would either form a long, thin line or two sides of a very small, triangular dot.
            if (subdivisions >= maxSubdivisions
                || Math.Sqrt(CalculateTriangleArea(segment[0], segment[1], segment[2])) < error
                && Math.Sqrt(CalculateTriangleArea(segment[1], segment[2], segment[3])) < error)
            {
                return segment;
            }

            // Split the curve in half.
            var curveParts = SplitCurveAtT(segment, 0.5);

            // Recursive case: further flatten the two segments and combine.
            return FlattenSegmentTo(curveParts.Item1.Take(4), error, subdivisions + 1, maxSubdivisions)
                .Concat(FlattenSegmentTo(curveParts.Item2.Take(4), error, subdivisions + 1, maxSubdivisions).Skip(1));
        }

        private static double CalculateTriangleArea(Point a, Point b, Point c)
            => Math.Abs(a.X * b.Y + b.X * c.Y + c.X * a.Y - a.Y * b.X - b.Y * c.X - c.Y * a.X) / 2;

        /// <summary>
        /// For a series of Bezier curves represented as a series of control points, interpolate a series of points for representing the curve as lines
        /// </summary>
        /// 
        /// Cubic bezier curves are typically represented as a list of 4 control points. Multiple continuous curves are concatenated so that
        /// the last point on one curve is the first point of the next.
        /// 
        /// This function takes a contiguous series of cubic bezier curves and returns an approximated representation of the curve as contiguous lines.
        /// 
        /// The error in this function is a measure of curviness. As such, segments are produced once the segments are declared flat enough. The smaller the
        /// error, the flatter the curve must be before it's converted into a line segment.
        /// 
        /// <param name="points">The list of contiguous bezier segments</param>
        /// <param name="error">The linear error, understood as max distance from ground truth. </param>
        /// <param name="maxSubdivisions">The maximum times to recursively subdivide. 20 means 10 meters would be subdivided into 0.01 millimeters.</param>
        /// <returns>An enumerable list of points representing the line segments</returns>
        public static IEnumerable<Point> FlattenTo(IList<Point> points, double error=0.01, int maxSubdivisions=20)
        {
            var result = new List<Point> { points.First() };
            for (var i = 0; i + 3 <= points.Count; i += 3)
                result.AddRange(FlattenSegmentTo(points.Skip(i).Take(4).ToList(), error, 0, maxSubdivisions).Skip(1));

            return result;
        }
    }
}
