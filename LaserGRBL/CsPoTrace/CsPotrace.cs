
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.Diagnostics;

//Copyright (C) 2001-2007 Peter Selinger
//Copyright (C) 2009 Wolfgang Nagl
//Copyright (C) 2012 Dileep.M
//Copyright (C) 2015 Olivier Spinelli

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.
// You should have received a copy of the GNU General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. 

namespace CsPotrace
{

    public partial class Potrace
    {

        #region additional Class definitions

        //----------------------Potrace Constants and aux functions
        const int POTRACE_CORNER = 1;
        const int POTRACE_CURVETO = 2;
        static double COS179 = Math.Cos( 179 * Math.PI / 180 );

        /// <summary>
        /// Area of largest path to be ignored.
        /// </summary>
        public static int turdsize = 2;

        /// <summary>
        /// Corner threshold.
        /// </summary>
        public static double alphamax = 1.0;

        /// <summary>
        /// Use curve optimization.
        /// optimize the path p, replacing sequences of Bezier segments by a single segment when possible.
        /// </summary>
        public static bool curveoptimizing = true;

        /// <summary>
        /// Curve optimization tolerance
        /// </summary>
        public static double opttolerance = 0.2;

        /// <summary>
        /// Simple North/East/South/West enumeration.
        /// </summary>
        enum Direction
        {
            North,
            East,
            South,
            West,
        }

        /// <summary>
        /// Kind of Curve : Line or Bezier
        /// </summary>
        public enum CurveKind
        {
            Line,
            Bezier
        }

        /// <summary>
        /// Holds the information about a produced curve.
        /// </summary>
        public struct Curve
        {
            /// <summary>
            /// Bezier or Line.
            /// </summary>
            public readonly CurveKind Kind;

            /// <summary>
            /// Startpoint of the line or bezier.
            /// </summary>
            public readonly DoublePoint A;

            /// <summary>
            /// ControlPoint
            /// </summary>
            public readonly DoublePoint ControlPointA;

            /// <summary>
            /// ControlPoint
            /// </summary>
            public readonly DoublePoint ControlPointB;

            /// <summary>
            /// Endpoint
            /// </summary>
            public readonly DoublePoint B;

            /// <summary>
            /// Initializes a new curve.
            /// </summary>
            /// <param name="kind"></param>
            /// <param name="a">Startpoint</param>
            /// <param name="controlPointA">Controlpoint</param>
            /// <param name="ControlPointB">Controlpoint</param>
            /// <param name="b">Endpoint</param>
            public Curve( CurveKind kind, DoublePoint a, DoublePoint controlPointA, DoublePoint ControlPointB, DoublePoint b )
            {
                this.Kind = kind;
                this.A = a;
                this.B = b;
                this.ControlPointA = controlPointA;
                this.ControlPointB = ControlPointB;
                this.B = b;
            }
        }

        class Path
        {
            public int area;
            public ArrayList MonotonIntervals;
            public IntPoint[] pt;
            public int[] Lon;
            public SumStruct[] Sums;
            public int[] po;
            public PrivCurve Curves;
            public PrivCurve OptimizedCurves;
            public PrivCurve FCurves;
        }

        struct IntPoint
        {
            public readonly int X;
            public readonly int Y;

            public IntPoint( int x, int y )
            {
                X = x;
                Y = y;
            }

        }

        /// <summary>
        /// Holds the coordinates of a Point.
        /// </summary>
        public struct DoublePoint
        {
            /// <summary>
            /// x-coordinate
            /// </summary>
            public readonly double X;

            /// <summary>
            /// y-coordinate
            /// </summary>
            public readonly double Y;

            /// <summary>
            /// Creates a point
            /// </summary>
            /// <param name="x">x-coordinate</param>
            /// <param name="y">y-coordinate</param>
            public DoublePoint( double x, double y )
            {
                X = x;
                Y = y;
            }
        }

        struct SumStruct
        {
            public int X;
            public int Y;
            public int XY;
            public int X2;
            public int Y2;
        }

        class PrivCurve
        {
            /// <summary>
            /// Number of segments.
            /// </summary>
            public readonly int Count;

            /// <summary>
            /// Tag[n]: POTRACE_CORNER or POTRACE_CURVETO.
            /// </summary>
            public readonly int[] Tag;

            /// <summary>
            /// c[n][i]: control points. 
            /// c[n][0] is unused for tag[n]=POTRACE_CORNER
            /// </summary>
            public readonly DoublePoint[,] ControlPoints;

            /// <summary>
            /// for POTRACE_CORNER, this equals c[1](*c)[3]; 
            /// c[n][i]: control points.
            /// c[n][0] is unused for tag[n]=POTRACE_CORNER
            /// </summary>
            public readonly DoublePoint[] Vertex;

            /// <summary>
            /// only for POTRACE_CURVETO.
            /// </summary>
            public readonly double[] Alpha;

            /// <summary>
            /// "uncropped" alpha parameter - for debug output only.
            /// </summary>
            public readonly double[] Alpha0;

            public readonly double[] Beta;

            public PrivCurve( int count )
            {
                Count = count;
                Tag = new int[Count];
                ControlPoints = new DoublePoint[Count, 3];
                Vertex = new DoublePoint[Count];
                Alpha = new double[Count];
                Alpha0 = new double[Count];
                Beta = new double[Count];
            }

        };

        #region auxiliary functions


        /// <summary>
        /// Calculates a point of a bezier curve.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns></returns>
        static DoublePoint Bezier( double t, DoublePoint p0, DoublePoint p1, DoublePoint p2, DoublePoint p3 )
        {
            double s = 1 - t;
            double x = s * s * s * p0.X + 3 * (s * s * t) * p1.X + 3 * (t * t * s) * p2.X + t * t * t * p3.X;
            double y = s * s * s * p0.Y + 3 * (s * s * t) * p1.Y + 3 * (t * t * s) * p2.Y + t * t * t * p3.Y;
            return new DoublePoint( x, y );
        }

        /// <summary>
        /// Calculates the point t in [0..1] on the (convex) bezier curve
        /// (p0,p1,p2,p3) which is tangent to q1-q0. Return -1.0 if there is no
        /// solution in [0..1]. 
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="q0"></param>
        /// <param name="q1"></param>
        /// <returns></returns>
        static double Tangent( DoublePoint p0, DoublePoint p1, DoublePoint p2, DoublePoint p3, DoublePoint q0, DoublePoint q1 )
        {
            double A, B, C;   /* (1-t)^2 A + 2(1-t)t B + t^2 C = 0 */
            double a, b, c;   /* a t^2 + b t + c = 0 */
            double d, s, r1, r2;

            A = cprod( p0, p1, q0, q1 );
            B = cprod( p1, p2, q0, q1 );
            C = cprod( p2, p3, q0, q1 );

            a = A - 2 * B + C;
            b = -2 * A + 2 * B;
            c = A;

            d = b * b - 4 * a * c;

            if( a == 0 || d < 0 )
            {
                return -1.0;
            }

            s = Math.Sqrt( d );

            r1 = (-b + s) / (2 * a);
            r2 = (-b - s) / (2 * a);

            if( r1 >= 0 && r1 <= 1 )
            {
                return r1;
            }
            else if( r2 >= 0 && r2 <= 1 )
            {
                return r2;
            }
            else
            {
                return -1.0;
            }
        }

        /// <summary>
        /// Calculate distance between two points.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        static double ddist( DoublePoint p, DoublePoint q )
        {
            return Math.Sqrt( (p.X - q.X) * (p.X - q.X) + (p.Y - q.Y) * (p.Y - q.Y) );
        }

        /* calculate p1 x p2 */
        static int xprod( IntPoint p1, IntPoint p2 )
        {
            return p1.X * p2.Y - p1.Y * p2.X;
        }
        /* calculate p1 x p2 */
        static double xprod( DoublePoint p1, DoublePoint p2 )
        {
            return p1.X * p2.Y - p1.Y * p2.X;
        }
        /* calculate (p1-p0)x(p3-p2) */
        static double cprod( DoublePoint p0, DoublePoint p1, DoublePoint p2, DoublePoint p3 )
        {
            double x1, y1, x2, y2;

            x1 = p1.X - p0.X;
            y1 = p1.Y - p0.Y;
            x2 = p3.X - p2.X;
            y2 = p3.Y - p2.Y;

            return x1 * y2 - x2 * y1;
        }
        /* calculate (p1-p0)*(p2-p0) */
        static double iprod( DoublePoint p0, DoublePoint p1, DoublePoint p2 )
        {
            double x1, y1, x2, y2;

            x1 = p1.X - p0.X;
            y1 = p1.Y - p0.Y;
            x2 = p2.X - p0.X;
            y2 = p2.Y - p0.Y;

            return x1 * x2 + y1 * y2;
        }

        /* calculate (p1-p0)*(p3-p2) */
        static double iprod1( DoublePoint p0, DoublePoint p1, DoublePoint p2, DoublePoint p3 )
        {
            double x1, y1, x2, y2;

            x1 = p1.X - p0.X;
            y1 = p1.Y - p0.Y;
            x2 = p3.X - p2.X;
            y2 = p3.Y - p2.Y;

            return x1 * x2 + y1 * y2;
        }

        /// <summary>
        /// return a direction that is 90 degrees counterclockwise from p2-p0,
        /// but then restricted to one of the major wind directions (n, nw, w, etc).
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        static IntPoint dorth_infty( DoublePoint p0, DoublePoint p2 )
        {
            return new IntPoint( -sign( p2.Y - p0.Y ), sign( p2.X - p0.X ) );
        }

        /// <summary>
        /// Range over the straight line segment [a,b] when lambda ranges over [0,1].
        /// </summary>
        /// <param name="lambda">Scale.</param>
        /// <param name="a">Start point.</param>
        /// <param name="b">Stop point.</param>
        /// <returns>Point on the segment.</returns>
        static DoublePoint interval( double lambda, DoublePoint a, DoublePoint b )
        {
            return new DoublePoint( a.X + lambda * (b.X - a.X), a.Y + lambda * (b.Y - a.Y) );
        }

        /* return (p1-p0)x(p2-p0), the area of the parallelogram */
        static double dpara( DoublePoint p0, DoublePoint p1, DoublePoint p2 )
        {
            double x1, y1, x2, y2;

            x1 = p1.X - p0.X;
            y1 = p1.Y - p0.Y;
            x2 = p2.X - p0.X;
            y2 = p2.Y - p0.Y;

            return x1 * y2 - x2 * y1;
        }

        /* ddenom/dpara have the property that the square of radius 1 centered
           at p1 intersects the line p0p2 iff |dpara(p0,p1,p2)| <= ddenom(p0,p2) */
        static double ddenom( DoublePoint p0, DoublePoint p2 )
        {
            IntPoint r = dorth_infty( p0, p2 );

            return r.Y * (p2.X - p0.X) - r.X * (p2.Y - p0.Y);
        }

        /* return 1 if a <= b < c < a, in a cyclic sense (mod n) */
        static bool cyclic( int a, int b, int c )
        {
            if( a <= c )
            {
                return (a <= b && b < c);
            }
            else
            {
                return (a <= b || b < c);
            }
        }

        /// <summary>
        /// determine the center and slope of the line i..j. Assume i &lt; j. Needs
        /// "sum" components of p to be set.
        /// </summary>
        /// <param name="pp"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="ctr"></param>
        /// <param name="dir"></param>
        static void pointslope( Path pp, int i, int j, ref DoublePoint ctr, ref DoublePoint dir )
        {
            Debug.Assert( i < j );

            int n = pp.pt.Length;
            SumStruct[] sums = pp.Sums;

            double x, y, x2, xy, y2;
            double k;
            double a, b, c, lambda2, l;
            int r = 0; /* rotations from i to j */

            while( j >= n )
            {
                j -= n;
                r += 1;
            }
            while( i >= n )
            {
                i -= n;
                r -= 1;
            }
            while( j < 0 )
            {
                j += n;
                r -= 1;
            }
            while( i < 0 )
            {
                i += n;
                r += 1;
            }

            x = sums[j + 1].X - sums[i].X + r * sums[n].X;
            y = sums[j + 1].Y - sums[i].Y + r * sums[n].Y;
            x2 = sums[j + 1].X2 - sums[i].X2 + r * sums[n].X2;
            xy = sums[j + 1].XY - sums[i].XY + r * sums[n].XY;
            y2 = sums[j + 1].Y2 - sums[i].Y2 + r * sums[n].Y2;
            k = j + 1 - i + r * n;

            ctr = new DoublePoint( x / k, y / k );

            a = (x2 - (double)x * x / k) / k;
            b = (xy - (double)x * y / k) / k;
            c = (y2 - (double)y * y / k) / k;

            lambda2 = (a + c + Math.Sqrt( (a - c) * (a - c) + 4 * b * b )) / 2; // larger e.value

            /* now find e.vector for lambda2 */
            a -= lambda2;
            c -= lambda2;

            if( Math.Abs( a ) >= Math.Abs( c ) )
            {
                l = Math.Sqrt( a * a + b * b );
                if( l != 0 )
                {
                    dir = new DoublePoint( -b / l, a / l );
                }
            }
            else
            {
                l = Math.Sqrt( c * c + b * b );
                if( l != 0 )
                {
                    dir = new DoublePoint( -c / l, b / l );
                }
            }
            if( l == 0 )
            {
                // Sometimes this can happen when k=4: the two eigenvalues coincide.
                dir = new DoublePoint();
            }
        }

        /* integer arithmetic */
        static int sign( int x ) { return ((x) > 0 ? 1 : (x) < 0 ? -1 : 0); }
        static int sign( double x ) { return ((x) > 0 ? 1 : (x) < 0 ? -1 : 0); }
        static int abs( int a ) { return ((a) > 0 ? (a) : -(a)); }
        static int min( int a, int b ) { return ((a) < (b) ? (a) : (b)); }
        static int max( int a, int b ) { return ((a) > (b) ? (a) : (b)); }
        static int sq( int a ) { return ((a) * (a)); }
        static int cu( int a ) { return ((a) * (a) * (a)); }

        static int mod( int a, int n )
        {
            return a >= n ? a % n : a >= 0 ? a : n - 1 - (-1 - a) % n;
        }
        static int floordiv( int a, int n )
        {
            return a >= 0 ? a / n : -1 - (-1 - a) / n;
        }


        #endregion

        /// <summary>
        /// The MonotonInterval defines a structure related to an iPoint array.
        /// For each index i,j with 
        /// from &lt;= i &lt; j &lt;= to 
        /// in a cyclic sense is iPointArray[i].y &lt;= iPointArray[j].y if Increasing is true,
        /// else is iPointArray[i].y &gt;= iPointArray[j].y.
        /// </summary>
        class MonotonInterval
        {
            public bool Increasing;
            public int from;
            public int to;
            public void ResetCurrentID( int modulo )
            {
                if( !Increasing )
                    CurrentID = mod( Min() + 1, modulo );
                else
                    CurrentID = Min();
            }

            public int CurrentID; // only used by Invert

            public MonotonInterval( bool Increasing, int from, int to )
            {
                this.Increasing = Increasing;
                this.from = from;
                this.to = to;
            }
            public int Min()
            {
                if( Increasing ) return from;
                return to;
            }
            public int MinY( IntPoint[] Pts )
            {
                return Pts[Min()].Y;
            }
            public int MaxY( IntPoint[] Pts )
            {
                return Pts[Max()].Y;
            }
            public int Max()
            {
                if( !Increasing ) return from;
                return to;
            }
        }

        #endregion

        #region Static function of Potrace


        /// <summary>
        /// 
        /// Produces a binary Matrix with Dimensions b.Width+2 and b.Height +2, where
        /// the Border ( of width) in the Matrix is filled with 'true' -values.
        /// On this way we avoid a lot of boundsinequalities.
        /// For the threshold, we take the Maximum of (R,G,B ) of a Pixel at x,y. If this is less then the threshold the resultMatrix at x+1, y+1 is filled with
        /// false else with true.
        /// 
        /// </summary>
        /// <param name="b"> A Bitmap, which will be transformed to a binary Matrix</param>
        /// <param name="_treshold">Gives a threshold ( between 1 and 254 ) for Converting</param>
        /// <returns>Returns a binaray boolean Matrix </returns>
        public static bool[,] BitMapToBinary( Bitmap b, int _treshold )
        {

            BitmapData SourceData = b.LockBits( new Rectangle( 0, 0, b.Width, b.Height ), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb );
            // inflated Result

            bool[,] Result = new bool[b.Width + 2, b.Height + 2];
            int SourceStride = SourceData.Stride;
            int H = b.Height;
            int W = b.Width;
            unsafe
            {
                byte* SourcePtr = (byte*)(void*)SourceData.Scan0;
                int Ydisp = 0;
                for( int y = 0; y < H; y++ )
                {
                    for( int x = 0; x < W; x++ )
                        if( Math.Max(
                            Math.Max( SourcePtr[x * 3 + 2 + Ydisp], SourcePtr[x * 3 + 1 + Ydisp] ),
                            SourcePtr[x * 3 + 0 + Ydisp] ) < _treshold )
                            Result[x + 1, y + 1] = false;
                        else
                            Result[x + 1, y + 1] = true;
                    Ydisp = Ydisp + SourceStride;
                }
            }
            b.UnlockBits( SourceData );
            // White Border
            for( int x = 0; x < Result.GetLength( 0 ); x++ )
            {
                Result[x, 0] = true;
                Result[x, Result.GetLength( 1 ) - 1] = true;
            }
            for( int y = 1; y < Result.GetLength( 1 ) - 1; y++ )
            {
                Result[0, y] = true;
                Result[Result.GetLength( 0 ) - 1, y] = true;
            }

            return Result;

        }
        /// <summary>
        /// Makes a Black and White Bitmap from the Data of a Binarymatrix.
        /// Value with 'true' returns a white Pixel such with 'false' a Black pixel.
        /// </summary>
        ///<param name="IgnoreBorder">If this value is set then a Border with 1 Pixel is ignored</param>
        /// <param name="Matrix">A Binary Matrix, which have boolean values</param>
        /// <returns>Returns a Black and white Image </returns>
        public static Bitmap BinaryToBitmap( bool[,] Matrix, bool IgnoreBorder )
        {
            int W = Matrix.GetLength( 0 );
            int H = Matrix.GetLength( 1 );
            if( IgnoreBorder )
            {
                W = W - 2;
                H = H - 2;
            }

            Bitmap OutPutImage = new Bitmap( W, H );
            BitmapData CopyData = OutPutImage.LockBits( new Rectangle( 0, 0, OutPutImage.Width, OutPutImage.Height ), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb );
            int stride = CopyData.Stride;
            int d = 0;
            if( IgnoreBorder ) d = 1;

            unsafe
            {

                byte* DestPtr = (byte*)(void*)CopyData.Scan0;
                for( int i = 0; i < W; i++ )
                    for( int j = 0; j < H; j++ )
                    {
                        if( Matrix[i + d, j + d] )
                        {
                            DestPtr[i * 3 + j * stride] = 255;
                            DestPtr[i * 3 + j * stride + 1] = 255;
                            DestPtr[i * 3 + j * stride + 2] = 255;
                        }
                        else
                        {
                            DestPtr[i * 3 + j * stride] = 0;
                            DestPtr[i * 3 + j * stride + 1] = 0;
                            DestPtr[i * 3 + j * stride + 2] = 0;

                        }
                    }
            }
            OutPutImage.UnlockBits( CopyData );
            return OutPutImage;
        }
        /// <summary>
        /// Searches a x and a y such that source[x,y] = true and source[x+1,y] false.
        /// If this not exists, false will be returned else the result is true. 
        /// 
        /// </summary>
        /// <param name="source">Is a Binary Matrix, which is produced by <see cref="BitMapToBinary"/>
        /// <param name="x">x index in the source Matrix</param>
        /// <param name="y">y index in the source Matrix</param>
        private static bool FindNext( bool[,] Matrix, ref int x, ref int y )
        {
            for( y = 1; y < Matrix.GetLength( 1 ) - 1; y++ )
                for( x = 0; x < Matrix.GetLength( 0 ) - 1; x++ )
                    if( !Matrix[x + 1, y] ) // black found
                        return true;
            x = -1;
            return false;
        }
        // <summary>
        /// Searches a x and a y inside the Path P such that source[x,y] = true and source[x+1,y] false.
        /// If this not exists, false will be returned else the result is true. 
        /// 
        /// </summary>
        /// <param name="source">Is a Binary Matrix, which is produced by <see cref="BitMapToBinary"/>
        /// <param name="x">x index in the source Matrix</param>
        /// <param name="y">y index in the source Matrix</param>
        static bool FindNext( bool[,] Matrix, ref int x, ref int y, Path P )
        {
            int i = 0;
            int n = P.pt.Length;
            ArrayList MonotonIntervals = P.MonotonIntervals;
            if( MonotonIntervals.Count == 0 ) return false;
            MonotonInterval MI = (MonotonInterval)MonotonIntervals[0];
            MI.ResetCurrentID( n );
            y = P.pt[MI.CurrentID].Y;
            ArrayList CurrentIntervals = new ArrayList();
            CurrentIntervals.Add( MI );
            MI.CurrentID = MI.Min();

            while( (i + 1 < MonotonIntervals.Count) && (((MonotonInterval)MonotonIntervals[i + 1]).MinY( P.pt ) == y) )
            {
                MI = (MonotonInterval)MonotonIntervals[i + 1];
                MI.ResetCurrentID( n );
                CurrentIntervals.Add( MI );
                i++;
            }

            while( CurrentIntervals.Count > 0 )
            {
                for( int k = 0; k < CurrentIntervals.Count - 1; k++ )
                {
                    int x1 = P.pt[((MonotonInterval)CurrentIntervals[k]).CurrentID].X + 1;
                    int x2 = P.pt[((MonotonInterval)CurrentIntervals[k + 1]).CurrentID].X;

                    for( x = x1; x <= x2; x++ )
                        if( !Matrix[x, y] )
                        {
                            x--;
                            return true;
                        }
                    k++;
                }

                y++;
                for( int j = CurrentIntervals.Count - 1; j >= 0; j-- )
                {

                    MonotonInterval M = (MonotonInterval)CurrentIntervals[j];

                    if( y > M.MaxY( P.pt ) )
                    {
                        CurrentIntervals.RemoveAt( j );
                        continue;
                    }
                    int CID = M.CurrentID;
                    do
                    {
                        if( M.Increasing )
                            CID = mod( CID + 1, n );
                        else
                            CID = mod( CID - 1, n );
                    }
                    while( P.pt[CID].Y < y );
                    M.CurrentID = CID;
                }
                // Add Items of MonotonIntervals with Miny==y
                while( (i + 1 < MonotonIntervals.Count) && (((MonotonInterval)MonotonIntervals[i + 1]).MinY( P.pt ) == y) )
                {
                    MonotonInterval NewInt = (MonotonInterval)MonotonIntervals[i + 1];
                    int j = 0;
                    // search the correct x-Position
                    int _x = P.pt[NewInt.Min()].X;
                    while( (j < CurrentIntervals.Count) && (_x > P.pt[((MonotonInterval)CurrentIntervals[j]).CurrentID].X) ) j++;
                    CurrentIntervals.Insert( j, NewInt );
                    NewInt.ResetCurrentID( n );
                    i++;
                }
            }
            return false;
        }

        /* Apply quadratic form Q to vector w = (w.x,w.y) */
        static double quadform( double[,] Q, DoublePoint w )
        {
            double[] v = { w.X, w.Y, 1 };
            int i, j;
            double sum = 0;
            for( i = 0; i < 3; i++ )
            {
                for( j = 0; j < 3; j++ )
                {
                    sum += v[i] * Q[i, j] * v[j];
                }
            }
            return sum;
        }



        /*  */
        /// <summary>
        /// Compute a path in the binary matrix.
        /// Start path at the point (x0,x1), which must be an upper left corner
        /// of the path. Also compute the area enclosed by the path. Return a
        /// new path_t object, or NULL on error (note that a legitimate path
        /// cannot have length 0). 
        /// We omit turnpolicies and sign
        /// </summary>
        /// <param name="Matrix">Binary Matrix</param>
        /// <returns></returns>
        static Path findpath( bool[,] Matrix, IntPoint Start )
        {
            ArrayList L = new ArrayList();



            Direction Dir = Direction.North;
            int x;
            int y;
            int area = 0;
            int diry = -1;
            x = Start.X;
            y = Start.Y;

            do
            {
                // area += x * diry;
                L.Add( new IntPoint( x, y ) );
                int _y = y;
                findNextTrace( Matrix, ref x, ref y, ref Dir );
                diry = _y - y;
                area += x * diry;
            }
            while( (x != Start.X) || (y != Start.Y) );

            if( L.Count == 0 ) return null;
            Path result = new Path();
            result.pt = new IntPoint[L.Count];
            result.area = area;

            for( int i = 0; i < L.Count; i++ ) result.pt[i] = (IntPoint)L[i];

            // Shift 1 to be compatible with Potrace

            if( result.pt.Length > 0 )
            {
                IntPoint P = result.pt[result.pt.Length - 1];
                for( int i = result.pt.Length - 1; i >= 0; i-- )
                {
                    if( i > 0 )
                        result.pt[i] = result.pt[i - 1];
                    else
                        result.pt[0] = P;
                }
            }



            result.MonotonIntervals = GetMonotonIntervals( result.pt );



            return result;
        }

        static void findNextTrace( bool[,] Matrix, ref int x, ref int y, ref Direction Dir )
        {
            switch( Dir )
            {
                case Direction.West:
                    {
                        if( !Matrix[x + 1, y + 1] )
                        {
                            y++;
                            Dir = Direction.North;
                        }
                        else

                            if( !Matrix[x + 1, y] )
                            {
                                x++;
                                Dir = Direction.West;
                            }
                            else
                            {
                                y--;
                                Dir = Direction.South;
                            }
                        break;
                    }

                case Direction.South:
                    {
                        if( !Matrix[x + 1, y] )
                        {
                            x++;
                            Dir = Direction.West;
                        }
                        else
                            if( !Matrix[x, y] )
                            {
                                y--;
                                Dir = Direction.South;
                            }
                            else
                            {
                                x--;
                                Dir = Direction.East;
                            }
                        break;
                    }
                case Direction.East:
                    {
                        if( !Matrix[x, y] )
                        {
                            y--;
                            Dir = Direction.South;
                        }
                        else

                            if( !Matrix[x, y + 1] )
                            {
                                x--;
                                Dir = Direction.East;
                            }
                            else
                            {
                                y++;
                                Dir = Direction.North;
                            }
                        break;
                    }

                case Direction.North:
                    {
                        if( !Matrix[x, y + 1] )
                        {
                            x--;
                            Dir = Direction.East;
                        }
                        else
                            if( !Matrix[x + 1, y + 1] )
                            {
                                y++;
                                Dir = Direction.North;
                            }
                            else
                            {
                                x++;
                                Dir = Direction.West;
                            }
                        break;
                    }
            }
        }

        static ArrayList GetMonotonIntervals( IntPoint[] Pts )
        {

            ArrayList result = new ArrayList();
            int n = Pts.Length;
            if( n == 0 ) return result;
            ArrayList L = new ArrayList();

            //----- Start with Strong Monoton (Pts[i].y < Pts[i+1].y) or (Pts[i].y > Pts[i+1].y)
            int FirstStrongMonoton = 0;
            while( Pts[FirstStrongMonoton].Y == Pts[FirstStrongMonoton + 1].Y ) FirstStrongMonoton++;
            bool Up = (Pts[FirstStrongMonoton].Y < Pts[FirstStrongMonoton + 1].Y);
            MonotonInterval Interval = new MonotonInterval( Up, FirstStrongMonoton, FirstStrongMonoton );
            L.Add( Interval );
            int i = FirstStrongMonoton;
            do
            {
                // Interval.to = i;
                if( (Pts[i].Y == Pts[mod( i + 1, n )].Y) || (Up == (Pts[i].Y < Pts[mod( i + 1, n )].Y)) )
                    Interval.to = i;
                else
                {
                    Up = (Pts[i].Y < Pts[mod( i + 1, n )].Y);
                    Interval = new MonotonInterval( Up, i, i );
                    L.Add( Interval );
                }
                i = mod( i + 1, n );
            }
            while( i != FirstStrongMonoton );

            if( L.Count / 2 * 2 != L.Count )
            {// Connect the Last with first 
                MonotonInterval M0 = (MonotonInterval)L[0];
                MonotonInterval ML = (MonotonInterval)L[L.Count - 1];
                M0.from = ML.from;
                L.RemoveAt( L.Count - 1 );
            }

            //----- order now by the min y - value of interval to result 
            // and as second Key by the x-value
            //
            while( L.Count > 0 )
            {
                MonotonInterval M = (MonotonInterval)L[0];
                i = 0;
                // order by y-value
                while( (i < result.Count) && (Pts[M.Min()].Y > Pts[((MonotonInterval)result[i]).Min()].Y) ) i++;
                // order by x- value as second Key
                while( (i < result.Count) && (Pts[M.Min()].Y == Pts[((MonotonInterval)result[i]).Min()].Y) &&
                    (Pts[M.Min()].X > (Pts[((MonotonInterval)result[i]).Min()].X)) ) i++;
                result.Insert( i, M );
                L.RemoveAt( 0 );
            }
            return result;
        }

        static void Xor_Path( bool[,] Matrix, Path P )
        {
            int i = 0;
            int n = P.pt.Length;
            ArrayList MonotonIntervals = P.MonotonIntervals;
            if( MonotonIntervals.Count == 0 ) return;
            MonotonInterval MI = (MonotonInterval)MonotonIntervals[0];
            MI.ResetCurrentID( n );
            int y = P.pt[MI.CurrentID].Y;
            ArrayList CurrentIntervals = new ArrayList();
            CurrentIntervals.Add( MI );
            MI.CurrentID = MI.Min();

            while( (i + 1 < MonotonIntervals.Count) && (((MonotonInterval)MonotonIntervals[i + 1]).MinY( P.pt ) == y) )
            {
                MI = (MonotonInterval)MonotonIntervals[i + 1];
                MI.ResetCurrentID( n );
                CurrentIntervals.Add( MI );
                i++;
            }

            while( CurrentIntervals.Count > 0 )
            {   // invertLine
                for( int k = 0; k < CurrentIntervals.Count - 1; k++ )
                {
                    int x1 = P.pt[((MonotonInterval)CurrentIntervals[k]).CurrentID].X + 1;
                    int x2 = P.pt[((MonotonInterval)CurrentIntervals[k + 1]).CurrentID].X;
                    for( int x = x1; x <= x2; x++ ) Matrix[x, y] = !Matrix[x, y];
                    k++;
                }

                y++;
                for( int j = CurrentIntervals.Count - 1; j >= 0; j-- )
                {

                    MonotonInterval M = (MonotonInterval)CurrentIntervals[j];

                    if( y > M.MaxY( P.pt ) )
                    {
                        CurrentIntervals.RemoveAt( j );
                        continue;
                    }
                    int CID = M.CurrentID;
                    do
                    {
                        if( M.Increasing )
                            CID = mod( CID + 1, n );
                        else
                            CID = mod( CID - 1, n );
                    }
                    while( P.pt[CID].Y < y );
                    M.CurrentID = CID;
                }
                // Add Items of MonotonIntervals with Down.y==y
                while( (i + 1 < MonotonIntervals.Count) && (((MonotonInterval)MonotonIntervals[i + 1]).MinY( P.pt ) == y) )
                {
                    MonotonInterval NewInt = (MonotonInterval)MonotonIntervals[i + 1];
                    int j = 0;
                    // search the correct x-Position
                    int _x = P.pt[NewInt.Min()].X;
                    while( (j < CurrentIntervals.Count) && (_x > P.pt[((MonotonInterval)CurrentIntervals[j]).CurrentID].X) ) j++;
                    CurrentIntervals.Insert( j, NewInt );
                    NewInt.ResetCurrentID( n );
                    i++;
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
        static void calc_sums( Path pp )
        {
            int i, x, y;
            int n = pp.pt.Length;
            pp.Sums = new SumStruct[n + 1];


            // origin 
            int x0 = pp.pt[0].X;
            int y0 = pp.pt[0].Y;

            // preparatory computation for later fast summing 
            //pp->sums[0].x2 = pp->sums[0].xy = pp->sums[0].y2 = pp->sums[0].x = pp->sums[0].y = 0;
            pp.Sums[0].X2 = pp.Sums[0].XY = pp.Sums[0].Y2 = pp.Sums[0].X = pp.Sums[0].Y = 0;

            for( i = 0; i < n; i++ )
            {
                x = pp.pt[i].X - x0;
                y = pp.pt[i].Y - y0;
                pp.Sums[i + 1].X = pp.Sums[i].X + x;
                pp.Sums[i + 1].Y = pp.Sums[i].Y + y;
                pp.Sums[i + 1].X2 = pp.Sums[i].X2 + x * x;
                pp.Sums[i + 1].XY = pp.Sums[i].XY + x * y;
                pp.Sums[i + 1].Y2 = pp.Sums[i].Y2 + y * y;
            }
        }
        /* ---------------------------------------------------------------------- */
        /* Stage 2: calculate the optimal polygon (Sec. 2.2.2-2.2.4). */

        /* Auxiliary function: calculate the penalty of an edge from i to j in
           the given path. This needs the "lon" and "sum*" data. */

        static double penalty3( Path pp, int i, int j )
        {
            int n = pp.pt.Length;

            /* assume 0<=i<j<=n  */
            double x, y, x2, xy, y2;
            double k;
            double a, b, c, s;
            double px, py, ex, ey;
            SumStruct[] sums = pp.Sums;
            IntPoint[] pt = pp.pt;

            int r = 0; /* rotations from i to j */
            if( j >= n )
            {
                j -= n;
                r += 1;
            }


            x = sums[j + 1].X - sums[i].X + r * sums[n].X;
            y = sums[j + 1].Y - sums[i].Y + r * sums[n].Y;
            x2 = sums[j + 1].X2 - sums[i].X2 + r * sums[n].X2;
            xy = sums[j + 1].XY - sums[i].XY + r * sums[n].XY;
            y2 = sums[j + 1].Y2 - sums[i].Y2 + r * sums[n].Y2;
            k = j + 1 - i + r * n;

            px = (pt[i].X + pt[j].X) / 2.0 - pt[0].X;
            py = (pt[i].Y + pt[j].Y) / 2.0 - pt[0].Y;
            ey = (pt[j].X - pt[i].X);
            ex = -(pt[j].Y - pt[i].Y);

            a = ((x2 - 2 * x * px) / k + px * px);
            b = ((xy - x * py - y * px) / k + px * py);
            c = ((y2 - 2 * y * py) / k + py * py);

            s = ex * ex * a + 2 * ex * ey * b + ey * ey * c;

            return Math.Sqrt( s );
        }

        static void calc_lon( Path pp )
        {

            int i, j, k, k1;
            int a, b, c, d;
            int[] ct = { 0, 0, 0, 0 };
            int dir;
            IntPoint[] constraint = new IntPoint[2];
            IntPoint cur;
            IntPoint off;
            IntPoint dk;  /* direction of k-k1 */
            IntPoint[] pt = pp.pt;


            int n = pt.Length;
            int[] Pivot = new int[n];
            int[] nc = new int[n];
            /* initialize the nc data structure. Point from each point to the
                 furthest future point to which it is connected by a vertical or
                 horizontal segment. We take advantage of the fact that there is
                 always a direction change at 0 (due to the path decomposition
                 algorithm). But even if this were not so, there is no harm, as
                 in practice, correctness does not depend on the word "furthest"
                 above.  */


            k = 0;
            for( i = n - 1; i >= 0; i-- )
            {
                if( pt[i].X != pt[k].X && pt[i].Y != pt[k].Y )
                {
                    k = i + 1;  /* necessarily i<n-1 in this case */
                }
                nc[i] = k;
            }


            pp.Lon = new int[n];

            // determine pivot points: for each i, let pivk[i] be the furthest k
            // such that all j with i<j<k lie on a line connecting i,k.
            for( i = n - 1; i >= 0; i-- )
            {

                ct[0] = ct[1] = ct[2] = ct[3] = 0;

                /* keep track of "directions" that have occurred */
                dir = (3 + 3 * (pt[mod( i + 1, n )].X - pt[i].X) + (pt[mod( i + 1, n )].Y - pt[i].Y)) / 2;
                ct[dir]++;

                constraint[0] = new IntPoint();
                constraint[1] = new IntPoint();

                /* find the next k such that no straight line from i to k */
                k = nc[i];
                k1 = i;
                while( true )
                {
                    dir = (3 + 3 * sign( pt[k].X - pt[k1].X ) + sign( pt[k].Y - pt[k1].Y )) / 2;
                    ct[dir]++;


                    /* if all four "directions" have occurred, cut this path */
                    if( (ct[0] == 1) && (ct[1] == 1) && (ct[2] == 1) && (ct[3] == 1) )
                    {
                        Pivot[i] = k1;
                        goto foundk;
                    }

                    cur = new IntPoint( pt[k].X - pt[i].X, pt[k].Y - pt[i].Y );

                    /* see if current constraint is violated */
                    if( xprod( constraint[0], cur ) < 0 || xprod( constraint[1], cur ) > 0 )
                    {
                        goto constraint_viol;
                    }

                    /* else, update constraint */
                    if( abs( cur.X ) <= 1 && abs( cur.Y ) <= 1 )
                    {
                        /* no constraint */
                    }
                    else
                    {
                        off = new IntPoint( cur.X + ((cur.Y >= 0 && (cur.Y > 0 || cur.X < 0)) ? 1 : -1), cur.Y + ((cur.X <= 0 && (cur.X < 0 || cur.Y < 0)) ? 1 : -1) );
                        if( xprod( constraint[0], off ) >= 0 )
                        {
                            constraint[0] = off;
                        }
                        off = new IntPoint( cur.X + ((cur.Y <= 0 && (cur.Y < 0 || cur.X < 0)) ? 1 : -1), cur.Y + ((cur.X >= 0 && (cur.X > 0 || cur.Y < 0)) ? 1 : -1) );
                        if( xprod( constraint[1], off ) <= 0 )
                        {
                            constraint[1] = off;
                        }
                    }
                    k1 = k;
                    k = nc[k1];
                    if( !cyclic( k, i, k1 ) )
                    {
                        break;
                    }
                }
                constraint_viol:
                /* k1 was the last "corner" satisfying the current constraint, and
                   k is the first one violating it. We now need to find the last
                   point along k1..k which satisfied the constraint. */

                dk = new IntPoint( sign( pt[k].X - pt[k1].X ), sign( pt[k].Y - pt[k1].Y ) );
                cur = new IntPoint( pt[k1].X - pt[i].X, pt[k1].Y - pt[i].Y );

                /* find largest integer j such that xprod(constraint[0], cur+j*dk)
                   >= 0 and xprod(constraint[1], cur+j*dk) <= 0. Use bilinearity
                   of xprod. */
                a = xprod( constraint[0], cur );
                b = xprod( constraint[0], dk );
                c = xprod( constraint[1], cur );
                d = xprod( constraint[1], dk );
                /* find largest integer j such that a+j*b>=0 and c+j*d<=0. This
                   can be solved with integer arithmetic. */
                j = int.MaxValue;
                if( b < 0 )
                {
                    j = floordiv( a, -b );
                }
                if( d > 0 )
                {
                    j = min( j, floordiv( -c, d ) );
                }
                Pivot[i] = mod( k1 + j, n );
                foundk:
                ;
            } /* for i */

            /* clean up: for each i, let lon[i] be the largest k such that for
               all i' with i<=i'<k, i'<k<=pivk[i']. */

            j = Pivot[n - 1];
            pp.Lon[n - 1] = j;

            for( i = n - 2; i >= 0; i-- )
            {
                if( cyclic( i + 1, Pivot[i], j ) )
                {
                    j = Pivot[i];
                }
                pp.Lon[i] = j;

            }

            for( i = n - 1; cyclic( mod( i + 1, n ), j, pp.Lon[i] ); i-- )
            {
                pp.Lon[i] = j;

            }




        }





        /*  */

        /// <summary>
        /// find the optimal polygon. Fill in the m and po components. Return 1
        /// on failure with errno set, else 0. Non-cyclic version: assumes i=0
        /// is in the polygon. 
        /// Fixme: ### implement cyclic version.
        /// </summary>
        /// <param name="pp"></param>
        static void BestPolygon( Path pp )
        {
            int i, j, m, k;
            int n = pp.pt.Length;
            double[] pen = new double[n + 1]; /* pen[n+1]: penalty vector */
            int[] prev = new int[n + 1];   /* prev[n+1]: best path pointer vector */
            int[] clip0 = new int[n];  /* clip0[n]: longest segment pointer, non-cyclic */
            int[] clip1 = new int[n + 1];  /* clip1[n+1]: backwards segment pointer, non-cyclic */
            int[] seg0 = new int[n + 1];    /* seg0[m+1]: forward segment bounds, m<=n */
            int[] seg1 = new int[n + 1];   /* seg1[m+1]: backward segment bounds, m<=n */

            double thispen;
            double best;
            int c;


            /* calculate clipped paths */
            for( i = 0; i < n; i++ )
            {
                c = mod( pp.Lon[mod( i - 1, n )] - 1, n );

                if( c == i )
                {
                    c = mod( i + 1, n );
                }
                if( c < i )
                {
                    clip0[i] = n;
                }
                else
                {
                    clip0[i] = c;
                }
            }

            /* calculate backwards path clipping, non-cyclic. j <= clip0[i] iff
            clip1[j] <= i, for i,j=0..n. */
            j = 1;
            for( i = 0; i < n; i++ )
            {
                while( j <= clip0[i] )
                {
                    clip1[j] = i;
                    j++;
                }
            }

            /* calculate seg0[j] = longest path from 0 with j segments */
            i = 0;
            for( j = 0; i < n; j++ )
            {
                seg0[j] = i;
                i = clip0[i];
            }
            seg0[j] = n;
            m = j;

            /* calculate seg1[j] = longest path to n with m-j segments */
            i = n;
            for( j = m; j > 0; j-- )
            {
                seg1[j] = i;
                i = clip1[i];
            }
            seg1[0] = 0;

            /* now find the shortest path with m segments, based on penalty3 */
            /* note: the outer 2 loops jointly have at most n interations, thus
            the worst-case behavior here is quadratic. In practice, it is
            close to linear since the inner loop tends to be short. */
            pen[0] = 0;
            for( j = 1; j <= m; j++ )
            {
                for( i = seg1[j]; i <= seg0[j]; i++ )
                {
                    best = -1;
                    for( k = seg0[j - 1]; k >= clip1[i]; k-- )
                    {
                        thispen = penalty3( pp, k, i ) + pen[k];
                        if( best < 0 || thispen < best )
                        {
                            prev[i] = k;
                            best = thispen;
                        }
                    }
                    pen[i] = best;
                }
            }


            /* read off shortest path */

            int[] B = new int[m];

            pp.po = new int[m];
            for( i = n, j = m - 1; i > 0; j-- )
            {
                i = prev[i];
                B[j] = i;
            }

            /*   if ((m > 0) && ( mod(pp.Lon[m - 1]-1,n)<= B[1]))
               {// reduce
                   B[0] = B[m - 1];
                   pp.po = new int[m - 1];
                   for (i = 0; i < m - 1; i++)
                       pp.po[i] = B[i];

               }
               else
             */
            pp.po = B;




        }

        /* Stage 3: vertex adjustment (Sec. 2.3.1). */

        /// <summary>
        /// Calculate "optimal" point-slope representation for each line
        /// segment.
        /// Adjust vertices of optimal polygon: calculate the intersection of
        /// the two "optimal" line segments, then move it into the unit square
        /// if it lies outside. Return 1 with errno set on error; 0 on
        /// success.
        /// </summary>
        /// <param name="pp"></param>
        static void AdjustVertices( Path pp )
        {
            int m = pp.po.Length;
            int[] po = pp.po;
            IntPoint[] pt = pp.pt;
            int n = pt.Length;


            int x0 = pt[0].X;
            int y0 = pt[0].Y;

            DoublePoint[] ctr = new DoublePoint[m];      /* ctr[m] */
            DoublePoint[] dir = new DoublePoint[m];      /* dir[m] */

            double[,,] q = new double[m, 3, 3];
            double[] v = new double[3];
            double d;
            int i, j, k, l;
            DoublePoint s;
            pp.Curves = new PrivCurve( m );

            // Calculate "optimal" point-slope representation for each line segment.
            for( i = 0; i < m; i++ )
            {
                j = po[mod( i + 1, m )];
                j = mod( j - po[i], n ) + po[i];
                pointslope( pp, po[i], j, ref ctr[i], ref dir[i] );
            }
            // Represents each line segment as a singular quadratic form; the
            // distance of a point (x,y) from the line segment will be
            // (x,y,1)Q(x,y,1)^t, where Q=q[i].
            for( i = 0; i < m; i++ )
            {
                d = dir[i].X * dir[i].X + dir[i].Y * dir[i].Y;

                if( d == 0.0 )
                {
                    for( j = 0; j < 3; j++ )
                    {
                        for( k = 0; k < 3; k++ )
                        {
                            q[i, j, k] = 0;
                        }
                    }
                }
                else
                {
                    v[0] = dir[i].Y;
                    v[1] = -dir[i].X;
                    v[2] = -v[1] * ctr[i].Y - v[0] * ctr[i].X;
                    for( l = 0; l < 3; l++ )
                    {
                        for( k = 0; k < 3; k++ )
                        {
                            q[i, l, k] = v[l] * v[k] / d;
                        }
                    }
                }
            }
            /* now calculate the "intersections" of consecutive segments.
               Instead of using the actual intersection, we find the point
               within a given unit square which minimizes the square distance to
               the two lines. */
            for( i = 0; i < m; i++ )
            {
                double[,] Q = new double[3, 3];
                DoublePoint w;
                double dx, dy;
                double det;
                double min, cand; /* minimum and candidate for minimum of quad. form */
                double xmin, ymin;	/* coordinates of minimum */
                int z;

                /* let s be the vertex, in coordinates relative to x0/y0 */
                s = new DoublePoint( pt[po[i]].X - x0, pt[po[i]].Y - y0 );

                /* intersect segments i-1 and i */

                j = mod( i - 1, m );

                /* add quadratic forms */
                for( l = 0; l < 3; l++ )
                {
                    for( k = 0; k < 3; k++ )
                    {
                        Q[l, k] = q[j, l, k] + q[i, l, k];
                    }
                }
                while( true )
                {
                    /* minimize the quadratic form Q on the unit square */
                    /* find intersection */
                    det = Q[0, 0] * Q[1, 1] - Q[0, 1] * Q[1, 0];
                    if( det != 0.0 )
                    {
                        double wx = (-Q[0, 2] * Q[1, 1] + Q[1, 2] * Q[0, 1]) / det;
                        double wy = (Q[0, 2] * Q[1, 0] - Q[1, 2] * Q[0, 0]) / det;
                        w = new DoublePoint( wx, wy );
                        break;
                    }

                    /* matrix is singular - lines are parallel. Add another,
                   orthogonal axis, through the center of the unit square */
                    if( Q[0, 0] > Q[1, 1] )
                    {
                        v[0] = -Q[0, 1];
                        v[1] = Q[0, 0];
                    }
                    else if( Q[1, 1] != 0 ) // nur if (Q[1,1])
                    {
                        v[0] = -Q[1, 1];
                        v[1] = Q[1, 0];
                    }
                    else
                    {
                        v[0] = 1;
                        v[1] = 0;
                    }
                    d = v[0] * v[0] + v[1] * v[1];
                    v[2] = -v[1] * s.Y - v[0] * s.X;
                    for( l = 0; l < 3; l++ )
                    {
                        for( k = 0; k < 3; k++ )
                        {
                            Q[l, k] += v[l] * v[k] / d;
                        }
                    }
                }
                dx = Math.Abs( w.X - s.X );
                dy = Math.Abs( w.Y - s.Y );
                if( dx <= .5 && dy <= .5 )
                {
                    // - 1 because we have a additional border set to the bitmap
                    pp.Curves.Vertex[i] = new DoublePoint( w.X + x0, w.Y + y0 );
                    continue;
                }

                // the minimum was not in the unit square; now minimize quadratic
                // on boundary of square.
                min = quadform( Q, s );
                xmin = s.X;
                ymin = s.Y;

                if( Q[0, 0] == 0.0 )
                {
                    goto fixx;
                }
                for( z = 0; z < 2; z++ )
                {
                    // value of the y-coordinate.
                    double wY = s.Y - 0.5 + z;
                    double wX = -(Q[0, 1] * wY + Q[0, 2]) / Q[0, 0];
                    w = new DoublePoint( wX, wY );
                    dx = Math.Abs( wX - s.X );
                    cand = quadform( Q, w );
                    if( dx <= .5 && cand < min )
                    {
                        min = cand;
                        xmin = w.X;
                        ymin = w.Y;
                    }
                }
                fixx:
                if( Q[1, 1] == 0.0 )
                {
                    goto corners;
                }
                for( z = 0; z < 2; z++ )
                {   /* value of the x-coordinate */
                    double wX = s.X - 0.5 + z;
                    double wY = -(Q[1, 0] * wX + Q[1, 2]) / Q[1, 1];
                    w = new DoublePoint( wX, wY );
                    dy = Math.Abs( wY - s.Y );
                    cand = quadform( Q, w );
                    if( dy <= .5 && cand < min )
                    {
                        min = cand;
                        xmin = w.X;
                        ymin = w.Y;
                    }
                }
                corners:
                /* check four corners */
                for( l = 0; l < 2; l++ )
                {
                    for( k = 0; k < 2; k++ )
                    {
                        w = new DoublePoint( s.X - 0.5 + l, s.Y - 0.5 + k );
                        cand = quadform( Q, w );
                        if( cand < min )
                        {
                            min = cand;
                            xmin = w.X;
                            ymin = w.Y;
                        }
                    }
                }
                // - 1 because we have a additional border set to the bitmap
                pp.Curves.Vertex[i] = new DoublePoint( xmin + x0 - 1, ymin + y0 - 1 );
                continue;
            }


        }

        /* ---------------------------------------------------------------------- */
        /* Stage 4: smoothing and corner analysis (Sec. 2.3.3) */

        /* Always succeeds and returns 0 */
        static void smooth( PrivCurve curve, int sign, double alphamax )
        {
            int m = curve.Count;

            int i, j, k;
            double dd, denom, alpha;
            DoublePoint p2, p3, p4;

            if( sign == '-' )
            {
                /* reverse orientation of negative paths */
                for( i = 0, j = m - 1; i < j; i++, j-- )
                {
                    DoublePoint tmp;
                    tmp = curve.Vertex[i];
                    curve.Vertex[i] = curve.Vertex[j];
                    curve.Vertex[j] = tmp;
                }
            }

            /* examine each vertex and find its best fit */
            for( i = 0; i < m; i++ )
            {
                j = mod( i + 1, m );
                k = mod( i + 2, m );
                p4 = interval( 1 / 2.0, curve.Vertex[k], curve.Vertex[j] );

                denom = ddenom( curve.Vertex[i], curve.Vertex[k] );
                if( denom != 0.0 )
                {
                    dd = dpara( curve.Vertex[i], curve.Vertex[j], curve.Vertex[k] ) / denom;
                    dd = Math.Abs( dd );
                    alpha = dd > 1 ? (1 - 1.0 / dd) : 0;
                    alpha = alpha / 0.75;
                }
                else
                {
                    alpha = 4 / 3.0;
                }
                curve.Alpha0[j] = alpha;	 /* remember "original" value of alpha */

                if( alpha > alphamax )
                {  /* pointed corner */
                    curve.Tag[j] = POTRACE_CORNER;
                    //curve.c[j][1] = curve->vertex[j];
                    curve.ControlPoints[j, 1] = curve.Vertex[j];
                    curve.ControlPoints[j, 2] = p4;
                }
                else
                {
                    if( alpha < 0.55 )
                    {
                        alpha = 0.55;
                    }
                    else if( alpha > 1 )
                    {
                        alpha = 1;
                    }
                    p2 = interval( .5 + .5 * alpha, curve.Vertex[i], curve.Vertex[j] );
                    p3 = interval( .5 + .5 * alpha, curve.Vertex[k], curve.Vertex[j] );
                    curve.Tag[j] = POTRACE_CURVETO;
                    curve.ControlPoints[j, 0] = p2;
                    curve.ControlPoints[j, 1] = p3;
                    curve.ControlPoints[j, 2] = p4;
                }
                curve.Alpha[j] = alpha;	/* store the "cropped" value of alpha */
                curve.Beta[j] = 0.5;
            }


        }

        /* ---------------------------------------------------------------------- */
        /* Stage 5: Curve optimization (Sec. 2.4) */

        /* a private type for the result of opti_penalty */
        struct opti
        {
            public double pen;	   /* penalty */
            public DoublePoint[] c;   /* curve parameters */
            public double t, s;	   /* curve parameters */
            public double alpha;	   /* curve parameter */
        };

        /* calculate best fit from i+.5 to j+.5.  Assume i<j (cyclically).
           Return 0 and set badness and parameters (alpha, beta), if
           possible. Return 1 if impossible. */
        static bool opti_penalty( Path pp, int i, int j, ref opti res, double opttolerance, int[] convc, double[] areac )
        {
            int m = pp.Curves.Count;
            int k, k1, k2, conv, i1;
            double area, alpha, d, d1, d2;
            DoublePoint p0, p1, p2, p3, pt;
            double A, R, A1, A2, A3, A4;
            double s, t;

            /* check convexity, corner-freeness, and maximum bend < 179 degrees */

            if( i == j )
            {  /* sanity - a full loop can never be an opticurve */
                return true;
            }

            k = i;
            i1 = mod( i + 1, m );
            k1 = mod( k + 1, m );
            conv = convc[k1];
            if( conv == 0 )
            {
                return true;
            }
            d = ddist( pp.Curves.Vertex[i], pp.Curves.Vertex[i1] );
            for( k = k1; k != j; k = k1 )
            {
                k1 = mod( k + 1, m );
                k2 = mod( k + 2, m );
                if( convc[k1] != conv )
                {
                    return true;
                }
                if( sign( cprod( pp.Curves.Vertex[i], pp.Curves.Vertex[i1], pp.Curves.Vertex[k1], pp.Curves.Vertex[k2] ) ) != conv )
                {
                    return true;
                }
                if( iprod1( pp.Curves.Vertex[i], pp.Curves.Vertex[i1], pp.Curves.Vertex[k1], pp.Curves.Vertex[k2] ) < d * ddist( pp.Curves.Vertex[k1], pp.Curves.Vertex[k2] ) * COS179 )
                {
                    return true;
                }
            }

            /* the curve we're working in: */
            p0 = pp.Curves.ControlPoints[mod( i, m ), 2];
            p1 = pp.Curves.Vertex[mod( i + 1, m )];
            p2 = pp.Curves.Vertex[mod( j, m )];
            p3 = pp.Curves.ControlPoints[mod( j, m ), 2];

            /* determine its area */
            area = areac[j] - areac[i];
            area -= dpara( pp.Curves.Vertex[0], pp.Curves.ControlPoints[i, 2], pp.Curves.ControlPoints[j, 2] ) / 2;
            if( i >= j )
            {
                area += areac[m];
            }

            /* find intersection o of p0p1 and p2p3. Let t,s such that o =
               interval(t,p0,p1) = interval(s,p3,p2). Let A be the area of the
               triangle (p0,o,p3). */

            A1 = dpara( p0, p1, p2 );
            A2 = dpara( p0, p1, p3 );
            A3 = dpara( p0, p2, p3 );
            /* A4 = dpara(p1, p2, p3); */
            A4 = A1 + A3 - A2;

            if( A2 == A1 )
            {  /* this should never happen */
                return true;
            }

            t = A3 / (A3 - A4);
            s = A2 / (A2 - A1);
            A = A2 * t / 2.0;

            if( A == 0.0 )
            {  /* this should never happen */
                return true;
            }

            R = area / A;	 /* relative area */
            alpha = 2 - Math.Sqrt( 4 - R / 0.3 );  /* overall alpha for p0-o-p3 curve */
            res.c = new DoublePoint[2];
            res.c[0] = interval( t * alpha, p0, p1 );
            res.c[1] = interval( s * alpha, p3, p2 );
            res.alpha = alpha;
            res.t = t;
            res.s = s;

            p1 = res.c[0];
            p2 = res.c[1];  /* the proposed curve is now (p0,p1,p2,p3) */

            res.pen = 0;

            /* calculate penalty */
            /* check tangency with edges */
            for( k = mod( i + 1, m ); k != j; k = k1 )
            {
                k1 = mod( k + 1, m );
                t = Tangent( p0, p1, p2, p3, pp.Curves.Vertex[k], pp.Curves.Vertex[k1] );
                if( t < -.5 )
                {
                    return true;
                }
                pt = Bezier( t, p0, p1, p2, p3 );
                d = ddist( pp.Curves.Vertex[k], pp.Curves.Vertex[k1] );
                if( d == 0.0 )
                {  /* this should never happen */

                    return true;
                }
                d1 = dpara( pp.Curves.Vertex[k], pp.Curves.Vertex[k1], pt ) / d;
                if( Math.Abs( d1 ) > opttolerance )
                {
                    return true;
                }
                if( iprod( pp.Curves.Vertex[k], pp.Curves.Vertex[k1], pt ) < 0 || iprod( pp.Curves.Vertex[k1], pp.Curves.Vertex[k], pt ) < 0 )
                {
                    return true;
                }
                res.pen += d1 * d1;
            }

            /* check corners */
            for( k = i; k != j; k = k1 )
            {
                k1 = mod( k + 1, m );
                t = Tangent( p0, p1, p2, p3, pp.Curves.ControlPoints[k, 2], pp.Curves.ControlPoints[k1, 2] );
                if( t < -.5 )
                {
                    return true;
                }
                pt = Bezier( t, p0, p1, p2, p3 );
                d = ddist( pp.Curves.ControlPoints[k, 2], pp.Curves.ControlPoints[k1, 2] );
                if( d == 0.0 )
                {  /* this should never happen */
                    return true;
                }
                d1 = dpara( pp.Curves.ControlPoints[k, 2], pp.Curves.ControlPoints[k1, 2], pt ) / d;
                d2 = dpara( pp.Curves.ControlPoints[k, 2], pp.Curves.ControlPoints[k1, 2], pp.Curves.Vertex[k1] ) / d;
                d2 *= 0.75 * pp.Curves.Alpha[k1];
                if( d2 < 0 )
                {
                    d1 = -d1;
                    d2 = -d2;
                }
                if( d1 < d2 - opttolerance )
                {
                    return true;
                }
                if( d1 < d2 )
                {
                    res.pen += (d1 - d2) * (d1 - d2);
                }
            }

            return false;
        }
        private void i() { }

        /* optimize the path p, replacing sequences of Bezier segments by a
   single segment when possible. Return 0 on success, 1 with errno set
   on failure. */
        static void opticurve( Path pp, double opttolerance )
        {
            int m = pp.Curves.Count;
            int[] pt = new int[m + 1];     /* pt[m+1] */
            double[] pen = new double[m + 1];  /* pen[m+1] */
            int[] len = new int[m + 1];     /* len[m+1] */
            opti[] opt = new opti[m + 1];    /* opt[m+1] */
            int[] convc = new int[m];       /* conv[m]: pre-computed convexities */
            double[] areac = new double[m + 1];  /* cumarea[m+1]: cache for fast area computation */


            int om;
            int i, j;
            bool r;
            opti o = new opti();
            DoublePoint p0;
            int i1;
            double area;
            double alpha;
            double[] s;
            double[] t;




            /* pre-calculate convexity: +1 = right turn, -1 = left turn, 0 = corner */
            for( i = 0; i < m; i++ )
            {
                if( pp.Curves.Tag[i] == POTRACE_CURVETO )
                {
                    convc[i] = sign( dpara( pp.Curves.Vertex[mod( i - 1, m )], pp.Curves.Vertex[i], pp.Curves.Vertex[mod( i + 1, m )] ) );
                }
                else
                {
                    convc[i] = 0;
                }
            }

            /* pre-calculate areas */
            area = 0.0;
            areac[0] = 0.0;
            p0 = pp.Curves.Vertex[0];
            for( i = 0; i < m; i++ )
            {
                i1 = mod( i + 1, m );
                if( pp.Curves.Tag[i1] == POTRACE_CURVETO )
                {
                    alpha = pp.Curves.Alpha[i1];
                    area += 0.3 * alpha * (4 - alpha) * dpara( pp.Curves.ControlPoints[i, 2], pp.Curves.Vertex[i1], pp.Curves.ControlPoints[i1, 2] ) / 2;
                    area += dpara( p0, pp.Curves.ControlPoints[i, 2], pp.Curves.ControlPoints[i1, 2] ) / 2;
                }
                areac[i + 1] = area;
            }

            pt[0] = -1;
            pen[0] = 0;
            len[0] = 0;

            /* Fixme: we always start from a fixed point -- should find the best
               curve cyclically ### */

            for( j = 1; j <= m; j++ )
            {
                /* calculate best path from 0 to j */
                pt[j] = j - 1;
                pen[j] = pen[j - 1];
                len[j] = len[j - 1] + 1;

                for( i = j - 2; i >= 0; i-- )
                {
                    r = opti_penalty( pp, i, mod( j, m ), ref o, opttolerance, convc, areac );
                    if( r )
                    {
                        break;
                    }
                    if( len[j] > len[i] + 1 || (len[j] == len[i] + 1 && pen[j] > pen[i] + o.pen) )
                    {
                        pt[j] = i;
                        pen[j] = pen[i] + o.pen;
                        len[j] = len[i] + 1;
                        opt[j] = o;
                    }
                }
            }
            om = len[m];
            pp.OptimizedCurves = new PrivCurve( om );

            s = new double[om];
            t = new double[om];


            j = m;
            for( i = om - 1; i >= 0; i-- )
            {
                if( pt[j] == j - 1 )
                {
                    pp.OptimizedCurves.Tag[i] = pp.Curves.Tag[mod( j, m )];
                    pp.OptimizedCurves.ControlPoints[i, 0] = pp.Curves.ControlPoints[mod( j, m ), 0];
                    pp.OptimizedCurves.ControlPoints[i, 1] = pp.Curves.ControlPoints[mod( j, m ), 1];
                    pp.OptimizedCurves.ControlPoints[i, 2] = pp.Curves.ControlPoints[mod( j, m ), 2];
                    pp.OptimizedCurves.Vertex[i] = pp.Curves.Vertex[mod( j, m )];
                    pp.OptimizedCurves.Alpha[i] = pp.Curves.Alpha[mod( j, m )];
                    pp.OptimizedCurves.Alpha0[i] = pp.Curves.Alpha0[mod( j, m )];
                    pp.OptimizedCurves.Beta[i] = pp.Curves.Beta[mod( j, m )];
                    s[i] = t[i] = 1.0;
                }
                else
                {
                    pp.OptimizedCurves.Tag[i] = POTRACE_CURVETO;
                    pp.OptimizedCurves.ControlPoints[i, 0] = opt[j].c[0];
                    pp.OptimizedCurves.ControlPoints[i, 1] = opt[j].c[1];
                    pp.OptimizedCurves.ControlPoints[i, 2] = pp.Curves.ControlPoints[mod( j, m ), 2];
                    pp.OptimizedCurves.Vertex[i] = interval( opt[j].s, pp.Curves.ControlPoints[mod( j, m ), 2], pp.Curves.Vertex[mod( j, m )] );
                    pp.OptimizedCurves.Alpha[i] = opt[j].alpha;
                    pp.OptimizedCurves.Alpha0[i] = opt[j].alpha;
                    s[i] = opt[j].s;
                    t[i] = opt[j].t;
                }
                j = pt[j];
            }

            /* calculate beta parameters */
            for( i = 0; i < om; i++ )
            {
                i1 = mod( i + 1, om );
                pp.OptimizedCurves.Beta[i] = s[i] / (s[i] + t[i1]);
            }

        }




        static void getContur( bool[,] bm, int x, int y, ArrayList plistp )
        {


            Path Contur = findpath( bm, new IntPoint( x, y ) );

            Xor_Path( bm, Contur );
            ArrayList PolyPath = new ArrayList();
            // only area > turdsize is taken

            if( Contur.area > turdsize )
            {
                plistp.Add( PolyPath );
                PolyPath.Add( Contur ); // Path with index 0 is a conture
            }


            while( FindNext( bm, ref x, ref y, Contur ) )
            {
                Path Hole = findpath( bm, new IntPoint( x, y ) );
                //Path Hole = findpath(bm, x, y);
                Xor_Path( bm, Hole );
                if( Hole.area > turdsize )

                    PolyPath.Add( Hole ); // Path with index > 0 is a hole,
                while( FindNext( bm, ref x, ref y, Hole ) ) // 13.07.12 von if auf while
                    getContur( bm, x, y, plistp );

            }

        }
        /// <summary>
        /// It is the main function, which yields the curveinformations related to a given binary bitmap.
        /// It fills the ArrayList ListOfCurveArrays with curvepathes. 
        /// Each of this pathes is an list of connecting curves.
        /// 
        /// Example:
        /// <pre>
        /// Call first:
        ///     ArrayList ListOfCurveArrays = new ArrayList();
        ///     potrace_trace(bm, ListOfCurveArrays);
        /// Paint the result:
        ///    GraphicsPath gp = new GraphicsPath();
        ///     for (int i = 0; i &lt; ListOfCurveArrays.Count; i++)
        ///    {
        ///        ArrayList CurveArray = (ArrayList)ListOfCurveArrays[i];
        ///        GraphicsPath Contour=null;
        ///        GraphicsPath Hole = null;
        ///        GraphicsPath Current=null;
        ///        for (int j = 0; j &lt; CurveArray.Count; j++)
        ///        {
        ///            if (j == 0)
        ///            {
        ///                Contour = new GraphicsPath();
        ///                Current = Contour;
        ///            }
        ///            else
        ///            {
        ///                Hole = new GraphicsPath();
        ///                Current = Hole;
        ///             }
        ///            Potrace.Curve[] Curves = (Curve[])CurveArray[j];
        ///            for (int k = 0; k &lt; Curves.Length; k++)
        ///            {
        ///                if (Curves[k].Kind == Potrace.CurveKind.Bezier)
        ///                    Current.AddBezier((float)Curves[k].A.x, (float)Curves[k].A.y, (float)Curves[k].ControlPointA.x, (float)Curves[k].ControlPoint.y,
        ///                                      (float)Curves[k].ControlPointB.x, (float)Curves[k].ControlPointB.y, (float)Curves[k].B.x, (float)Curves[k].B.y);
        ///                else
        ///                    Current.AddLine((float)Curves[k].A.x, (float)Curves[k].A.y, (float)Curves[k].B.x, (float)Curves[k].B.y);
        ///            }
        ///            if (j > 0) Contour.AddPath(Hole, false);
        ///        }
        ///        gp.AddPath(Contour, false);
        ///    }
        ///     // any Graphic g
        ///    Graphics g = CreateGraphics();
        ///     // Paint the fill
        ///    g.FillPath(Brushes.Black, gp);
        ///     // Paint the border
        ///    g.DrawPath(Pens.Red,gp);
        /// </pre>
        /// </summary>
        /// <param name="ListOfCurveArrays">A list in which the curveinformations will be stored.</param>
        /// 
        /// <param name="bm">A binary bitmap, which holds the pixelinformation about the image.</param>

        public static void potrace_trace( bool[,] bm, ArrayList ListOfCurveArrays )
        {
            ArrayList plistp = new ArrayList();
            bm_to_pathlist( bm, plistp );
            process_path( plistp );
            PathList_to_ListOfCurveArrays( plistp, ListOfCurveArrays );
        }
        static void AddCurve( ArrayList Curves, DoublePoint A, DoublePoint ControlPointA, DoublePoint ControlPointB, DoublePoint B )
        {
            //   Curves.Add(new Curve(CurveKind.Bezier, A, ControlPointA, ControlPointB, B));
            //   return;
            CurveKind Kind;
            if( (Math.Abs( xprod( new DoublePoint( ControlPointA.X - A.X, ControlPointA.Y - A.Y ),
                                   new DoublePoint( B.X - A.X, B.Y - A.Y ) ) ) < 0.01) &&
                               (Math.Abs( xprod( new DoublePoint( ControlPointB.X - B.X, ControlPointB.Y - B.Y ),
                                   new DoublePoint( B.X - A.X, B.Y - A.Y ) ) ) < 0.01) )
                Kind = CurveKind.Line;
            else
                Kind = CurveKind.Bezier;
            /*    Curves.Add(new Curve(Kind,A,ControlPointA,ControlPointB,B));
                return;*/
            if( (Kind == CurveKind.Line) )
            {
                if( (Curves.Count > 0) && (((Curve)Curves[Curves.Count - 1]).Kind == CurveKind.Line) )
                {
                    Curve C = (Curve)Curves[Curves.Count - 1];
                    if( (Math.Abs( xprod( new DoublePoint( C.B.X - C.A.X, C.B.Y - C.A.Y ), new DoublePoint( B.X - A.X, B.Y - A.Y ) ) ) < 0.01) &&
                        (iprod( C.B, C.A, B ) < 0) )
                        Curves[Curves.Count - 1] = new Curve( Kind, C.A, C.A, C.A, B );
                    else
                        Curves.Add( new Curve( CurveKind.Line, A, ControlPointA, ControlPointB, B ) );



                }
                else
                    Curves.Add( new Curve( CurveKind.Line, A, ControlPointA, ControlPointB, B ) );



            }
            else
                Curves.Add( new Curve( CurveKind.Bezier, A, ControlPointA, ControlPointB, B ) );

        }
        static void PathList_to_ListOfCurveArrays( ArrayList plistp, ArrayList ListOfCurveArrays )
        {
            ArrayList plist;

            /* call downstream function with each path */
            for( int j = 0; j < plistp.Count; j++ )
            {
                plist = (ArrayList)plistp[j];
                ArrayList clist = new ArrayList();
                ListOfCurveArrays.Add( clist );

                for( int i = 0; i < plist.Count; i++ )
                {
                    Path p = (Path)plist[i];
                    DoublePoint A = p.Curves.ControlPoints[p.Curves.Count - 1, 2];
                    ArrayList Curves = new ArrayList();
                    for( int k = 0; k < p.Curves.Count; k++ )
                    {
                        DoublePoint C = p.Curves.ControlPoints[k, 0];
                        DoublePoint D = p.Curves.ControlPoints[k, 1];
                        DoublePoint E = p.Curves.ControlPoints[k, 2];
                        if( p.Curves.Tag[k] == POTRACE_CORNER )
                        {
                            AddCurve( Curves, A, A, D, D );
                            AddCurve( Curves, D, D, E, E );

                        }
                        else
                            AddCurve( Curves, A, C, D, E );
                        A = E;
                    }
                    if( Curves.Count > 0 )
                    {
                        Curve CL = (Curve)Curves[Curves.Count - 1];
                        Curve CF = (Curve)Curves[0];
                        if( (CL.Kind == CurveKind.Line) && ((CF.Kind == CurveKind.Line))
                            && (iprod( CL.B, CL.A, CF.B ) < 0)
                            && (Math.Abs( xprod( new DoublePoint( CF.B.X - CF.A.X, CF.B.Y - CF.A.Y ), new DoublePoint( CL.A.X - CL.A.X, CL.B.Y - CL.A.Y ) ) ) < 0.01) )
                        {
                            Curves[0] = new Curve( CurveKind.Line, CL.A, CL.A, CL.A, CF.B );
                            Curves.RemoveAt( Curves.Count - 1 );
                        }


                        Curve[] CList = new Curve[Curves.Count];
                        for( int ci = 0; ci < Curves.Count; ci++ ) CList[ci] = (Curve)Curves[ci];
                        clist.Add( CList );
                    }
                }
                //---- Check Last with first



            }

        }

        /// <summary>
        /// Decompose the given bitmap into paths. Returns a linked list of
        /// Path objects with the fields len, pt, area filled
        /// </summary>
        /// <param name="bm">A binary bitmap which holds the imageinformations.</param>
        /// <param name="plistp">List of Path objects</param>
        static void bm_to_pathlist( bool[,] bm, ArrayList plistp )
        {
            int x = 0, y = 0;
            while( FindNext( bm, ref x, ref y ) )
                getContur( bm, x, y, plistp );
        }


        static void process_path( ArrayList plistp )
        {
            Path p;
            ArrayList plist;
            /* call downstream function with each path */
            for( int j = 0; j < plistp.Count; j++ )
            {
                plist = (ArrayList)plistp[j];
                for( int i = 0; i < plist.Count; i++ )
                {
                    p = (Path)plist[i];
                    calc_sums( p );
                    calc_lon( p );
                    BestPolygon( p );
                    AdjustVertices( p );
                    smooth( p.Curves, 1, alphamax );
                    if( curveoptimizing )
                    {
                        opticurve( p, opttolerance );
                        p.FCurves = p.OptimizedCurves;
                    }
                    else
                    {
                        p.FCurves = p.Curves;
                    }
                    p.Curves = p.FCurves;
                }
            }

        }


        #endregion
    }
}
