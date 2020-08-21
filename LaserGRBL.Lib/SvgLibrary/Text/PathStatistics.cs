using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace Svg
{
    public class PathStatistics
    {
        private const double GqBreak_TwoPoint = 0.57735026918962573;
        private const double GqBreak_ThreePoint = 0.7745966692414834;
        private const double GqBreak_FourPoint_01 = 0.33998104358485631;
        private const double GqBreak_FourPoint_02 = 0.86113631159405257;
        private const double GqWeight_FourPoint_01 = 0.65214515486254621;
        private const double GqWeight_FourPoint_02 = 0.34785484513745385;

        private PathData _data;
        private double _totalLength;
        private List<ISegment> _segments = new List<ISegment>();

        public double TotalLength { get { return _totalLength; } }

        public PathStatistics(PathData data)
        {
            _data = data;
            int i = 1;
            _totalLength = 0;
            ISegment newSegment;
            while (i < _data.Points.Length)
            {
                switch (_data.Types[i])
                {
                    case 1:
                        newSegment = new LineSegment(_data.Points[i - 1], _data.Points[i]);
                        i++;
                        break;
                    case 3:
                        newSegment = new CubicBezierSegment(_data.Points[i - 1], _data.Points[i], _data.Points[i + 1], _data.Points[i + 2]);
                        i+= 3;
                        break;
                    default:
                        throw new NotSupportedException();
                }
                newSegment.StartOffset = _totalLength;
                _segments.Add(newSegment);
                _totalLength += newSegment.Length;
            }
        }


        public void LocationAngleAtOffset(double offset, out PointF point, out float angle)
        {
            _segments[BinarySearchForSegment(offset, 0, _segments.Count - 1)].LocationAngleAtOffset(offset, out point, out angle);
        }
        public bool OffsetOnPath(double offset)
        {
            var seg = _segments[BinarySearchForSegment(offset, 0, _segments.Count - 1)];
            offset -= seg.StartOffset;
            return (offset >= 0 && offset <= seg.Length);
        }

        private int BinarySearchForSegment(double offset, int first, int last)
        {
            if (last == first)
            {
                return first;
            }
            else if ((last - first) == 1)
            {
                return (offset >= _segments[last].StartOffset ? last : first);
            }
            else
            {
                var mid = (last + first) / 2;
                if (offset < _segments[mid].StartOffset)
                {
                    return BinarySearchForSegment(offset, first, mid);
                }
                else
                {
                    return BinarySearchForSegment(offset, mid, last);
                }
            }
        }

        private interface ISegment
        {
            double StartOffset { get; set; }
            double Length { get; }
            void LocationAngleAtOffset(double offset, out PointF point, out float rotation);
        }

        private class LineSegment : ISegment
        {
            private double _length;
            private double _rotation;
            private PointF _start;
            private PointF _end;

            public double StartOffset { get; set; }
            public double Length { get { return _length; } }

            public LineSegment(PointF start, PointF end)
            {
                _start = start;
                _end = end;
                _length = Math.Sqrt(Math.Pow(end.X - start.X, 2) + Math.Pow(end.Y - start.Y, 2));
                _rotation = Math.Atan2(end.Y - start.Y, end.X - start.X) * 180 / Math.PI;
            }

            public void LocationAngleAtOffset(double offset, out PointF point, out float rotation)
            {
                offset -= StartOffset;
                if (offset < 0 || offset > _length) throw new ArgumentOutOfRangeException();
                point = new PointF((float)(_start.X + (offset / _length) * (_end.X - _start.X)),
                                   (float)(_start.Y + (offset / _length) * (_end.Y - _start.Y)));
                rotation = (float)_rotation;
            }
        }

        private class CubicBezierSegment : ISegment
        {
            private PointF _p0;
            private PointF _p1;
            private PointF _p2;
            private PointF _p3;
            private double _length;
            private Func<double, double> _integral;
            private SortedList<double, double> _lengths = new SortedList<double, double>();

            public double StartOffset { get; set; }
            public double Length { get { return _length; } }

            public CubicBezierSegment(PointF p0, PointF p1, PointF p2, PointF p3)
            {
                _p0 = p0;
                _p1 = p1;
                _p2 = p2;
                _p3 = p3;
                _integral = (t) => CubicBezierArcLengthIntegrand(_p0, _p1, _p2, _p3, t);
                _length = GetLength(0, 1, 0.00000001f);
                _lengths.Add(0, 0);
                _lengths.Add(_length, 1);
            }

            private double GetLength(double left, double right, double epsilon)
            {
                var fullInt = GaussianQuadrature(_integral, left, right, 4);
                return Subdivide(left, right, fullInt, 0, epsilon);
            }
            private double Subdivide(double left, double right, double fullInt, double totalLength, double epsilon)
            {
                var mid = (left + right) / 2;
                var leftValue = GaussianQuadrature(_integral, left, mid, 4);
                var rightValue = GaussianQuadrature(_integral, mid, right, 4);
                if (Math.Abs(fullInt - (leftValue + rightValue)) > epsilon) {
                    var leftSub = Subdivide(left, mid, leftValue, totalLength, epsilon / 2.0);
                    totalLength += leftSub;
                    AddElementToTable(mid, totalLength);
                    return Subdivide(mid, right, rightValue, totalLength, epsilon / 2.0) + leftSub;
                }
                else
                {
                    return leftValue + rightValue;
                }
            }
            private void AddElementToTable(double position, double totalLength)
            {
                _lengths.Add(totalLength, position);
            }

            public void LocationAngleAtOffset(double offset, out PointF point, out float rotation)
            {
                offset -= StartOffset;
                if (offset < 0 || offset > _length) throw new ArgumentOutOfRangeException();

                var t = BinarySearchForParam(offset, 0, _lengths.Count - 1);
                point = CubicBezierCurve(_p0, _p1, _p2, _p3, t);
                var deriv = CubicBezierDerivative(_p0, _p1, _p2, _p3, t);
                rotation = (float)(Math.Atan2(deriv.Y, deriv.X) * 180.0 / Math.PI);
            }
            private double BinarySearchForParam(double length, int first, int last)
            {
                if (last == first)
                {
                    return _lengths.Values[last];
                }
                else if ((last - first) == 1)
                {
                    return _lengths.Values[first] + (_lengths.Values[last] - _lengths.Values[first]) * 
                        (length - _lengths.Keys[first]) / (_lengths.Keys[last] - _lengths.Keys[first]);
                }
                else
                {
                    var mid = (last + first) / 2;
                    if (length < _lengths.Keys[mid])
                    {
                        return BinarySearchForParam(length, first, mid);
                    }
                    else
                    {
                        return BinarySearchForParam(length, mid, last);
                    }
                }
            }

            /// <summary>
            /// Evaluates the integral of the function over the integral using the specified number of points
            /// </summary>
            /// <param name="func"></param>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <param name="points"></param>
            public static double GaussianQuadrature(Func<double, double> func, double a, double b, int points)
            {
                switch (points)
                {
                    case 1:
                        return (b - a) * func.Invoke((a + b) / 2.0);
                    case 2:
                        return (b - a) / 2.0 * (func.Invoke((b - a) / 2.0 * -1 * GqBreak_TwoPoint + (a + b) / 2.0) + 
                                                func.Invoke((b - a) / 2.0 * GqBreak_TwoPoint + (a + b) / 2.0));
                    case 3:
                        return (b - a) / 2.0 * (5.0 / 9 * func.Invoke((b - a) / 2.0 * -1 * GqBreak_ThreePoint + (a + b) / 2.0) +
                                                8.0 / 9 * func.Invoke((a + b) / 2.0) +
                                                5.0 / 9 * func.Invoke((b - a) / 2.0 * GqBreak_ThreePoint + (a + b) / 2.0));
                    case 4:
                        return (b - a) / 2.0 * (GqWeight_FourPoint_01 * func.Invoke((b - a) / 2.0 * -1 * GqBreak_FourPoint_01 + (a + b) / 2.0) +
                                                GqWeight_FourPoint_01 * func.Invoke((b - a) / 2.0 * GqBreak_FourPoint_01 + (a + b) / 2.0) +
                                                GqWeight_FourPoint_02 * func.Invoke((b - a) / 2.0 * -1 * GqBreak_FourPoint_02 + (a + b) / 2.0) +
                                                GqWeight_FourPoint_02 * func.Invoke((b - a) / 2.0 * GqBreak_FourPoint_02 + (a + b) / 2.0));
                }
                throw new NotSupportedException();
            }

            /// <remarks>http://en.wikipedia.org/wiki/B%C3%A9zier_curve</remarks>
            private PointF CubicBezierCurve(PointF p0, PointF p1, PointF p2, PointF p3, double t)
            {
                return new PointF((float)(Math.Pow(1 - t, 3) * p0.X + 3 * Math.Pow(1 - t, 2) * t * p1.X +
                                            3 * (1 - t) * Math.Pow(t, 2) * p2.X + Math.Pow(t, 3) * p3.X),
                                    (float)(Math.Pow(1 - t, 3) * p0.Y + 3 * Math.Pow(1 - t, 2) * t * p1.Y +
                                            3 * (1 - t) * Math.Pow(t, 2) * p2.Y + Math.Pow(t, 3) * p3.Y));
            }

            /// <remarks>http://www.cs.mtu.edu/~shene/COURSES/cs3621/NOTES/spline/Bezier/bezier-der.html</remarks>
            private PointF CubicBezierDerivative(PointF p0, PointF p1, PointF p2, PointF p3, double t)
            {
                return new PointF((float)(3 * Math.Pow(1 - t, 2) * (p1.X - p0.X) + 6 * (1 - t) * t * (p2.X - p1.X) + 3 * Math.Pow(t, 2) * (p3.X - p2.X)),
                                  (float)(3 * Math.Pow(1 - t, 2) * (p1.Y - p0.Y) + 6 * (1 - t) * t * (p2.Y - p1.Y) + 3 * Math.Pow(t, 2) * (p3.Y - p2.Y)));
            }


            private double CubicBezierArcLengthIntegrand(PointF p0, PointF p1, PointF p2, PointF p3, double t)
            {
                return Math.Sqrt(Math.Pow(3 * Math.Pow(1 - t, 2) * (p1.X - p0.X) + 6 * (1 - t) * t * (p2.X - p1.X) + 3 * Math.Pow(t, 2) * (p3.X - p2.X), 2) +
                                 Math.Pow(3 * Math.Pow(1 - t, 2) * (p1.Y - p0.Y) + 6 * (1 - t) * t * (p2.Y - p1.Y) + 3 * Math.Pow(t, 2) * (p3.Y - p2.Y), 2));
            }
        }
    }
}
