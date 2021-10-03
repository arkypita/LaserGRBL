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
        /// ControlPolygonFlatEnough :
        ///   Check if the control polygon of a Bezier curve is flat enough
        ///    for recursive subdivision to bottom out.
        /// 
        /// Corrections by James Walker, jw@jwwalker.com, as follows:
        /// 
        /// There seem to be errors in the ControlPolygonFlatEnough function in the
        /// Graphics Gems book and the repository (NearestPoint.c). This function
        /// is briefly described on p. 413 of the text, and appears on pages 793-794.
        /// I see two main problems with it.
        /// 
        /// The idea is to find an upper bound for the error of approximating the x
        /// intercept of the Bezier curve by the x intercept of the line through the
        /// first and last control points. It is claimed on p. 413 that this error is
        /// bounded by half of the difference between the intercepts of the bounding
        /// box. I don't see why that should be true. The line joining the first and
        /// last control points can be on one side of the bounding box, and the actual
        /// curve can be near the opposite side, so the bound should be the difference
        /// of the bounding box intercepts, not half of it.
        /// 
        /// Second, we come to the implementation. The values distance[i] computed in
        /// the first loop are not actual distances, but squares of distances. I
        /// realize that minimizing or maximizing the squares is equivalent to
        /// minimizing or maximizing the distances.  But when the code claims that
        /// one of the sides of the bounding box has equation
        /// a * x + b * y + c + max_distance_above, where max_distance_above is one of
        /// those squared distances, that makes no sense to me.
        /// 
        /// I have appended my version of the function. If you apply my code to the
        /// cubic Bezier curve used to test NearestPoint.c,
        /// 
        /// static Point2 bezCurve[4] = {    /  A cubic Bezier curve    /
        ///   { 0.0, 0.0 },
        ///   { 1.0, 2.0 },
        ///   { 3.0, 3.0 },
        ///   { 4.0, 2.0 },
        /// };
        /// 
        /// my code computes left_intercept = -3.0 and right_intercept = 0.0, which you
        /// can verify by sketching a graph. The original code computes
        /// left_intercept = 0.0 and right_intercept = 0.9
        internal static double CalculateFlatnessError(Point[] controlPoints, int degree)
        {
            // Derive the implicit equation for line connecting first 
            //  and last control points 
            var a = controlPoints[0].Y - controlPoints[degree].Y;
            var b = controlPoints[degree].X - controlPoints[0].X;
            var c = controlPoints[0].X * controlPoints[degree].Y - controlPoints[degree].X * controlPoints[0].Y;

            var max_distance_above = 0.0;
            var max_distance_below = 0.0;

            for (var i = 1; i < degree; i++)
            {
                var value = a * controlPoints[i].X + b * controlPoints[i].Y + c;

                if (value > max_distance_above)
                    max_distance_above = value;
                else if (value < max_distance_below)
                    max_distance_below = value;
            }

            //  Implicit equation for zero line 
            const double a1 = 0.0;
            const double b1 = 1.0;
            const double c1 = 0.0;

            //  Implicit equation for "above" line 
            var a2 = a;
            var b2 = b;
            var c2 = c - max_distance_above;

            var det = a1 * b2 - a2 * b1;
            if (det == 0)
            {
                return 0; // Otherwise the intercepts would blow up to +/- Inf.
            }

            var dInv = 1.0 / det;

            var intercept_1 = (b1 * c2 - b2 * c1) * dInv;

            //  Implicit equation for "below" line 
            a2 = a;
            b2 = b;
            c2 = c - max_distance_below;

            det = a1 * b2 - a2 * b1;
            dInv = 1.0 / det;

            var intercept_2 = (b1 * c2 - b2 * c1) * dInv;

            // Compute intercepts of bounding box   
            var left_intercept = Math.Min(intercept_1, intercept_2);
            var right_intercept = Math.Max(intercept_1, intercept_2);

            //Precision of root
            var error = right_intercept - left_intercept;

            return error;
        }

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
        /// The algorithm is quite simple. If the curve isn't flat enough, then split the curve in half and recursively
        /// test each half for flatness and assemble the segments in a list.
        /// 
        /// <param name="points">The 4 control points of a segment</param>
        /// <param name="error">The flatness error to enforce.</param>
        /// <returns></returns>
        private static IEnumerable<Point> FlattenSegmentTo(IEnumerable<Point> points, double error)
        {
            // Convert the points to an array
            var segment = points.ToArray();

            // If the segment is flat enough, then return
            if (CalculateFlatnessError(segment, 3) < error)
                return points;

            // Otherwise, split the curve in half
            var curveParts = SplitCurveAtT(segment, 0.5);

            // Flatten the two segments and combine them.
            return FlattenSegmentTo(curveParts.Item1.Take(4), error).Concat(FlattenSegmentTo(curveParts.Item2.Take(4), error).Skip(1));
        }

        /// <summary>
        /// For a series of Bezier curves represented as a series of control points, interpolate a series of points for representing the curve as lines
        /// </summary>
        /// 
        /// Cubic bezier curves are typically represented as a list of 4 control points. Multiple continuous curves are concenated so that
        /// the last point on one curve is the first point of the next.
        /// 
        /// This function takes a contiguous series of cubic bezier curves and returns an approximated representation of the curve as contiguous lines.
        /// 
        /// The error in this function is a measure of curviness. As such, segments are produced once the segments are declared flat enough. The smaller the
        /// error, the flatter the curve must be before it's converted into a line segment.
        /// 
        /// <param name="points">The list of contiguous bezier segments</param>
        /// <param name="error">The flatness error. 0.01 being the default, but 0.001 producing beautiful results.</param>
        /// <returns>An enumerable list of points representing the line segments</returns>
        public static IEnumerable<Point> FlattenTo(IList<Point> points, double error=0.01)
        {
            var result = new List<Point> { points.First() };
            for (var i = 0; i + 3 <= points.Count; i += 3)
                result.AddRange(FlattenSegmentTo(points.Skip(i).Take(4).ToList(), error).Skip(1));

            return result;
        }
    }
}
