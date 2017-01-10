using System;

namespace CsPotrace.BezierToBiarc
{
    /// <summary>
    /// Defines a line in point-slope form: y - y1 = m * (x - x1)
    /// Vertical line: m = NaN
    /// Horizontal line: m = 0
    /// </summary>
    public struct Line
    {
        /// <summary>
        /// Slope
        /// </summary>
        public readonly float m;
        /// <summary>
        /// Point
        /// </summary>
        public readonly Vector2 P;

        /// <summary>
        /// Define a line by two points
        /// </summary>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        public Line(Vector2 P1, Vector2 P2) : this(P1, Slope(P1, P2))
        {
        }

        /// <summary>
        /// Define a line by a point and slope
        /// </summary>
        /// <param name="P"></param>
        /// <param name="m"></param>
        public Line(Vector2 P, float m)
        {
            this.P = P;
            this.m = m;
        }

        /// <summary>
        /// Calculate the intersection point of this line and another one
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public Vector2 Intersection(Line l)
        {
            if(float.IsNaN(this.m))
            {
                return VerticalIntersection(this, l);
            }
            else if(float.IsNaN(l.m))
            {
                return VerticalIntersection(l, this);
            }
            else
            {
                float x = (this.m * this.P.X - l.m * l.P.X - this.P.Y + l.P.Y) / (this.m - l.m);
                float y = m * x - m * P.X + P.Y;
                return new Vector2(x, y);
            }
        }

        /// <summary>
        /// Special case, the first one is vertical (we suppose that the other one is not, 
        /// otherwise they do not cross)
        /// </summary>
        /// <param name="hl"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        private static Vector2 VerticalIntersection(Line vl, Line l)
        {
            float x = vl.P.X;
            float y = l.m * (x - l.P.X) + l.P.Y;
            return new Vector2(x, y);
        }

        /// <summary>
        /// Creates a a line which is perpendicular to the line defined by P and P1 and goes through P
        /// </summary>
        /// <param name="P"></param>
        /// <param name="P1"></param>
        /// <returns></returns>
        public static Line CreatePerpendicularAt(Vector2 P, Vector2 P1)
        {
            float m = Slope(P, P1);

            if (m == 0)
            {
                return new Line(P, float.NaN);
            }
            else if(float.IsNaN(m))
            {
                return new Line(P, 0);
            }
            else
            {
                return new Line(P, -1f / m);
            }
        }

        private static float Slope(Vector2 P1, Vector2 P2)
        {
            if(P2.X == P1.X)
            {
                return float.NaN;
            }
            else
            {
                return (P2.Y - P1.Y) / (P2.X - P1.X);
            }
        }

    }
}
