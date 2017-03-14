using System;

namespace CsPotrace.BezierToBiarc
{
    public struct CubicBezier
    {
        /// <summary>
        /// Start point
        /// </summary>
        public readonly Vector2 P1;
        /// <summary>
        /// End point
        /// </summary>
        public readonly Vector2 P2;
        /// <summary>
        /// First control point
        /// </summary>
        public readonly Vector2 C1;
        /// <summary>
        /// Second control point
        /// </summary>
        public readonly Vector2 C2;

        public CubicBezier(Vector2 P1, Vector2 C1, Vector2 C2, Vector2 P2)
        {
            this.P1 = P1;
            this.C1 = C1;
            this.P2 = P2;
            this.C2 = C2;
        }

        /// <summary>
        /// Implement the parametric equation.
        /// </summary>
        /// <param name="t">Parameter of the curve. Must be in [0,1]</param>
        /// <returns></returns>
        public Vector2 PointAt(float t)
        {
            return (float)Math.Pow(1 - t, 3) * P1 +
                       (float)(3 * Math.Pow(1 - t, 2) * t) * C1 +
                       (float)(3 * (1 - t) * Math.Pow(t, 2)) * C2 +
                       (float)Math.Pow(t, 3) * P2;
        }

        /// <summary>
        /// Split a bezier curve at a given parameter value. It returns both of the new ones
        /// </summary>
        /// <param name="t">Parameter of the curve. Must be in [0,1]</param>
        /// <returns></returns>
        public Tuple<CubicBezier, CubicBezier> Split(float t)
        {
            Vector2 p0 = P1 + t * (C1 - P1);
            Vector2 p1 = C1 + t * (C2 - C1);
            Vector2 p2 = C2 + t * (P2 - C2);

            Vector2 p01 = p0 + t * (p1 - p0);
            Vector2 p12 = p1 + t * (p2 - p1);

            Vector2 dp = p01 + t * (p12 - p01);

            return new Tuple<CubicBezier, CubicBezier> (new CubicBezier(P1, p0, p01, dp), new CubicBezier(dp, p12, p2, P2));
        }

        /// <summary>
        /// The orientation of the Bezier curve
        /// </summary>
        public bool IsClockwise
        {
            get
            {
                double sum = 0d;
                sum += (C1.X - P1.X) * (C1.Y + P1.Y);
                sum += (C2.X - C1.X) * (C2.Y + C1.Y);
                sum += (P2.X - C2.X) * (P2.Y + C2.Y);
                sum += (P1.X - P2.X) * (P1.Y + P2.Y);
                return sum < 0;
            }
        }

        /// <summary>
        /// Inflexion points of the Bezier curve. They only valid if they are real and in the range of [0,1]
        /// </summary>
        /// <param name="bezier"></param>
        /// <returns></returns>
        public Tuple<Complex, Complex> InflexionPoints
        {
            get
            {
                // http://www.caffeineowl.com/graphics/2d/vectorial/cubic-inflexion.html

                Vector2 A = C1 - P1;
                Vector2 B = C2 - C1 - A;
                Vector2 C = P2 - C2 - A - 2 * B;

                Complex a = new Complex(B.X * C.Y - B.Y * C.X, 0);
                Complex b = new Complex(A.X * C.Y - A.Y * C.X, 0);
                Complex c = new Complex(A.X * B.Y - A.Y * B.X, 0);

                Complex t1 = (-b + Complex.Sqrt(b * b - 4 * a * c)) / (2 * a);
                Complex t2 = (-b - Complex.Sqrt(b * b - 4 * a * c)) / (2 * a);

                return new Tuple<Complex, Complex>(t1, t2);
            }
        }
    }
}
