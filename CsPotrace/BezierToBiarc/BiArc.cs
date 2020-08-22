using System;

namespace CsPotrace.BezierToBiarc
{
    public struct BiArc
    {
        public readonly Arc A1;
        public readonly Arc A2;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="P1">Start point</param>
        /// <param name="T1">Tangent vector at P1</param>
        /// <param name="P2">End point</param>
        /// <param name="T2">Tangent vector at P2</param>
        /// <param name="T">Transition point</param>
        public BiArc(Vector2 P1, Vector2 T1, Vector2 P2, Vector2 T2, Vector2 T)
        {
            // Calculate the orientation
            // https://en.wikipedia.org/wiki/Curve_orientation

            double sum = 0d;
            sum += (T.X - P1.X) * (T.Y + P1.Y);
            sum += (P2.X - T.X) * (P2.Y + T.Y);
            sum += (P1.X - P2.X) * (P1.Y + P2.Y);
            bool cw = sum < 0;

            // Calculate perpendicular lines to the tangent at P1 and P2
            Line tl1 = Line.CreatePerpendicularAt(P1, P1 + T1);
            Line tl2 = Line.CreatePerpendicularAt(P2, P2 + T2);

            // Calculate the perpendicular bisector of P1T and P2T
            Vector2 P1T2 = (P1 + T) / 2;
            Line pbP1T = Line.CreatePerpendicularAt(P1T2, T);

            Vector2 P2T2 = (P2 + T) / 2;
            Line pbP2T = Line.CreatePerpendicularAt(P2T2, T);

            // The origo of the circles are at the intersection points
            Vector2 C1 = tl1.Intersection(pbP1T);
            Vector2 C2 = tl2.Intersection(pbP2T);

            // Calculate the radii
            float r1 = (C1 - P1).Length();
            float r2 = (C2 - P2).Length();

            // Calculate start and sweep angles
            Vector2 startVector1 = P1 - C1;
            Vector2 endVector1 = T - C1;
            double startAngle1 = Math.Atan2(startVector1.Y, startVector1.X);
            double sweepAngle1 = Math.Atan2(endVector1.Y, endVector1.X) - startAngle1;

            Vector2 startVector2 = T - C2;
            Vector2 endVector2 = P2 - C2;
            double startAngle2 = Math.Atan2(startVector2.Y, startVector2.X);
            double sweepAngle2 = Math.Atan2(endVector2.Y, endVector2.X) - startAngle2;

            // Adjust angles according to the orientation of the curve
            if (cw && sweepAngle1 < 0) sweepAngle1 = 2 * Math.PI + sweepAngle1;
            if (!cw && sweepAngle1 > 0) sweepAngle1 = sweepAngle1 - 2 * Math.PI;
            if (cw && sweepAngle2 < 0) sweepAngle2 = 2 * Math.PI + sweepAngle2;
            if (!cw && sweepAngle2 > 0) sweepAngle2 = sweepAngle2 - 2 * Math.PI;

            A1 = new Arc(C1, r1, (float)startAngle1, (float)sweepAngle1, P1, T);
            A2 = new Arc(C2, r2, (float)startAngle2, (float)sweepAngle2, T, P2);
        }

        /// <summary>
        /// Implement the parametric equation.
        /// </summary>
        /// <param name="t">Parameter of the curve. Must be in [0,1]</param>
        /// <returns></returns>
        public Vector2 PointAt(float t)
        {
            float s = A1.Length / (A1.Length + A2.Length);

            if (t <= s)
            {
                return A1.PointAt(t / s);
            }
            else
            {
                return A2.PointAt((t - s) / (1 - s));
            }
        }

        public float Length
        {
            get { return A1.Length + A2.Length; }
        }
    }
}
