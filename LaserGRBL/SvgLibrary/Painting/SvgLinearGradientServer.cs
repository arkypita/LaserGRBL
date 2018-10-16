using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Svg
{
    [SvgElement("linearGradient")]
    public sealed class SvgLinearGradientServer : SvgGradientServer
    {
        [SvgAttribute("x1")]
        public SvgUnit X1
        {
            get
            {
                return this.Attributes.GetAttribute<SvgUnit>("x1");
            }
            set
            {
                Attributes["x1"] = value;
            }
        }

        [SvgAttribute("y1")]
        public SvgUnit Y1
        {
            get
            {
                return this.Attributes.GetAttribute<SvgUnit>("y1");
            }
            set
            {
                this.Attributes["y1"] = value;
            }
        }

        [SvgAttribute("x2")]
        public SvgUnit X2
        {
            get
            {
                return this.Attributes.GetAttribute<SvgUnit>("x2");
            }
            set
            {
                Attributes["x2"] = value;
            }
        }

        [SvgAttribute("y2")]
        public SvgUnit Y2
        {
            get
            {
                return this.Attributes.GetAttribute<SvgUnit>("y2");
            }
            set
            {
                this.Attributes["y2"] = value;
            }
        }

        private bool IsInvalid
        {
            get
            {
                // Need at least 2 colours to do the gradient fill
                return this.Stops.Count < 2;
            }
        }

        public SvgLinearGradientServer()
        {
            X1 = new SvgUnit(SvgUnitType.Percentage, 0F);
            Y1 = new SvgUnit(SvgUnitType.Percentage, 0F);
            X2 = new SvgUnit(SvgUnitType.Percentage, 100F);
            Y2 = new SvgUnit(SvgUnitType.Percentage, 0F);
        }

        public override Brush GetBrush(SvgVisualElement renderingElement, ISvgRenderer renderer, float opacity, bool forStroke = false)
        {
            LoadStops(renderingElement);

            if (this.Stops.Count < 1) return null;
            if (this.Stops.Count == 1) 
            {
                var stopColor = this.Stops[0].GetColor(renderingElement); 
                int alpha = (int)Math.Round((opacity * (stopColor.A/255.0f) ) * 255);
                Color colour = System.Drawing.Color.FromArgb(alpha, stopColor);
                return new SolidBrush(colour);
            }

            try
            {
                if (this.GradientUnits == SvgCoordinateUnits.ObjectBoundingBox) renderer.SetBoundable(renderingElement);

                var points = new PointF[] {
                    SvgUnit.GetDevicePoint(NormalizeUnit(this.X1), NormalizeUnit(this.Y1), renderer, this),
                    SvgUnit.GetDevicePoint(NormalizeUnit(this.X2), NormalizeUnit(this.Y2), renderer, this)
                };

                var bounds = renderer.GetBoundable().Bounds;
                if (bounds.Width <= 0 || bounds.Height <= 0 || ((points[0].X == points[1].X) && (points[0].Y == points[1].Y))) 
                {
                    if (this.GetCallback != null) return GetCallback().GetBrush(renderingElement, renderer, opacity, forStroke);
                    return null;
                }

                using (var transform = EffectiveGradientTransform)
                {
                    var midPoint = new PointF((points[0].X + points[1].X) / 2, (points[0].Y + points[1].Y) / 2);
                    transform.Translate(bounds.X, bounds.Y, MatrixOrder.Prepend);
                    if (this.GradientUnits == SvgCoordinateUnits.ObjectBoundingBox)
                    {
                        // Transform a normal (i.e. perpendicular line) according to the transform
                        transform.Scale(bounds.Width, bounds.Height, MatrixOrder.Prepend);
                        transform.RotateAt(-90.0f, midPoint, MatrixOrder.Prepend);
                    }
                    transform.TransformPoints(points);
                }

                if (this.GradientUnits == SvgCoordinateUnits.ObjectBoundingBox)
                {
                    // Transform the normal line back to a line such that the gradient still starts in the correct corners, but
                    // has the proper normal vector based on the transforms.  If you work out the geometry, these formulas should work.
                    var midPoint = new PointF((points[0].X + points[1].X) / 2, (points[0].Y + points[1].Y) / 2);
                    var dy = (points[1].Y - points[0].Y);
                    var dx = (points[1].X - points[0].X);
                    var x2 = points[0].X;
                    var y2 = points[1].Y;

                    if (Math.Round(dx, 4) == 0)
                    {
                        points[0] = new PointF(midPoint.X + dy / 2 * bounds.Width / bounds.Height, midPoint.Y);
                        points[1] = new PointF(midPoint.X - dy / 2 * bounds.Width / bounds.Height, midPoint.Y);
                    }
                    else if (Math.Round(dy, 4) == 0)
                    {
                        points[0] = new PointF(midPoint.X, midPoint.Y - dx / 2 * bounds.Height / bounds.Width);
                        points[1] = new PointF(midPoint.X, midPoint.Y + dx / 2 * bounds.Height / bounds.Width); ;
                    }
                    else
                    {
                        var startX = (float)((dy * dx * (midPoint.Y - y2) + Math.Pow(dx, 2) * midPoint.X + Math.Pow(dy, 2) * x2) /
                        (Math.Pow(dx, 2) + Math.Pow(dy, 2)));
                        var startY = dy * (startX - x2) / dx + y2;
                        points[0] = new PointF(startX, startY);
                        points[1] = new PointF(midPoint.X + (midPoint.X - startX), midPoint.Y + (midPoint.Y - startY));
                    }
                }

                var effectiveStart = points[0];
                var effectiveEnd = points[1];

                if (PointsToMove(renderingElement, points[0], points[1]) > LinePoints.None)
                {
                    var expansion = ExpandGradient(renderingElement, points[0], points[1]);
                    effectiveStart = expansion.StartPoint;
                    effectiveEnd = expansion.EndPoint;
                }

                var result = new LinearGradientBrush(effectiveStart, effectiveEnd, System.Drawing.Color.Transparent, System.Drawing.Color.Transparent)
                {
                    InterpolationColors = CalculateColorBlend(renderer, opacity, points[0], effectiveStart, points[1], effectiveEnd),
                    WrapMode = WrapMode.TileFlipX
                };
                return result;
            }
            finally
            {
                if (this.GradientUnits == SvgCoordinateUnits.ObjectBoundingBox) renderer.PopBoundable();
            }
        }

        private SvgUnit NormalizeUnit(SvgUnit orig)
        {
            return (orig.Type == SvgUnitType.Percentage && this.GradientUnits == SvgCoordinateUnits.ObjectBoundingBox ?
                    new SvgUnit(SvgUnitType.User, orig.Value / 100) :
                    orig);
        }

        [Flags]
        private enum LinePoints
        {
            None = 0,
            Start = 1,
            End = 2
        }

        private LinePoints PointsToMove(ISvgBoundable boundable, PointF specifiedStart, PointF specifiedEnd)
        {
            var bounds = boundable.Bounds;
            if (specifiedStart.X == specifiedEnd.X)
            {
                return (bounds.Top < specifiedStart.Y && specifiedStart.Y < bounds.Bottom ? LinePoints.Start : LinePoints.None) |
                       (bounds.Top < specifiedEnd.Y && specifiedEnd.Y < bounds.Bottom ? LinePoints.End : LinePoints.None);
            }
            else if (specifiedStart.Y == specifiedEnd.Y)
            {
                return (bounds.Left < specifiedStart.X && specifiedStart.X < bounds.Right ? LinePoints.Start : LinePoints.None) |
                       (bounds.Left < specifiedEnd.X && specifiedEnd.X < bounds.Right ? LinePoints.End : LinePoints.None);
            }
            return (boundable.Bounds.Contains(specifiedStart) ? LinePoints.Start : LinePoints.None) |
                   (boundable.Bounds.Contains(specifiedEnd) ? LinePoints.End : LinePoints.None);
        }

        public struct GradientPoints
        {
            public PointF StartPoint;
            public PointF EndPoint;

            public GradientPoints(PointF startPoint, PointF endPoint)
            {
                this.StartPoint = startPoint;
                this.EndPoint = endPoint;
            }
        }

        private GradientPoints ExpandGradient(ISvgBoundable boundable, PointF specifiedStart, PointF specifiedEnd)
        {
            var pointsToMove = PointsToMove(boundable, specifiedStart, specifiedEnd);
            if (pointsToMove == LinePoints.None)
            {
                Debug.Fail("Unexpectedly expanding gradient when not needed!");
                return new GradientPoints(specifiedStart, specifiedEnd);
            }

            var bounds = boundable.Bounds;
            var effectiveStart = specifiedStart;
            var effectiveEnd = specifiedEnd;
            var intersectionPoints = CandidateIntersections(bounds, specifiedStart, specifiedEnd);

            Debug.Assert(intersectionPoints.Count == 2, "Unanticipated number of intersection points");

            if (!(Math.Sign(intersectionPoints[1].X - intersectionPoints[0].X) == Math.Sign(specifiedEnd.X - specifiedStart.X) &&
                  Math.Sign(intersectionPoints[1].Y - intersectionPoints[0].Y) == Math.Sign(specifiedEnd.Y - specifiedStart.Y)))
            {
                intersectionPoints = intersectionPoints.Reverse().ToList();
            }

            if ((pointsToMove & LinePoints.Start) > 0) effectiveStart = intersectionPoints[0];
            if ((pointsToMove & LinePoints.End) > 0) effectiveEnd = intersectionPoints[1];

            switch (SpreadMethod)
            {
                case SvgGradientSpreadMethod.Reflect:
                case SvgGradientSpreadMethod.Repeat:
                    var specifiedLength = CalculateDistance(specifiedStart, specifiedEnd);
                    var specifiedUnitVector = new PointF((specifiedEnd.X - specifiedStart.X) / (float)specifiedLength, (specifiedEnd.Y - specifiedStart.Y) / (float)specifiedLength);
                    var oppUnitVector = new PointF(-specifiedUnitVector.X, -specifiedUnitVector.Y);

                    var startExtend = (float)(Math.Ceiling(CalculateDistance(effectiveStart, specifiedStart) / specifiedLength) * specifiedLength);
                    effectiveStart = MovePointAlongVector(specifiedStart, oppUnitVector, startExtend);
                    var endExtend = (float)(Math.Ceiling(CalculateDistance(effectiveEnd, specifiedEnd) / specifiedLength) * specifiedLength);
                    effectiveEnd = MovePointAlongVector(specifiedEnd, specifiedUnitVector, endExtend);
                    break;
            }

            return new GradientPoints(effectiveStart, effectiveEnd);
        }

        private IList<PointF> CandidateIntersections(RectangleF bounds, PointF p1, PointF p2)
        {
            var results = new List<PointF>();
            if (Math.Round(Math.Abs(p1.Y - p2.Y), 4) == 0)
            {
                results.Add(new PointF(bounds.Left, p1.Y));
                results.Add(new PointF(bounds.Right, p1.Y));
            }
            else if (Math.Round(Math.Abs(p1.X - p2.X), 4) == 0)
            {
                results.Add(new PointF(p1.X, bounds.Top));
                results.Add(new PointF(p1.X, bounds.Bottom));
            }
            else
            {
                PointF candidate;
                // Save some effort and duplication in the trivial case
                if ((p1.X == bounds.Left || p1.X == bounds.Right) && (p1.Y == bounds.Top || p1.Y == bounds.Bottom))
                {
                    results.Add(p1);
                }
                else
                {
                    candidate = new PointF(bounds.Left, (p2.Y - p1.Y) / (p2.X - p1.X) * (bounds.Left - p1.X) + p1.Y);
                    if (bounds.Top <= candidate.Y && candidate.Y <= bounds.Bottom) results.Add(candidate);
                    candidate = new PointF(bounds.Right, (p2.Y - p1.Y) / (p2.X - p1.X) * (bounds.Right - p1.X) + p1.Y);
                    if (bounds.Top <= candidate.Y && candidate.Y <= bounds.Bottom) results.Add(candidate);
                }
                if ((p2.X == bounds.Left || p2.X == bounds.Right) && (p2.Y == bounds.Top || p2.Y == bounds.Bottom))
                {
                    results.Add(p2);
                }
                else
                {
                    candidate = new PointF((bounds.Top - p1.Y) / (p2.Y - p1.Y) * (p2.X - p1.X) + p1.X, bounds.Top);
                    if (bounds.Left <= candidate.X && candidate.X <= bounds.Right) results.Add(candidate);
                    candidate = new PointF((bounds.Bottom - p1.Y) / (p2.Y - p1.Y) * (p2.X - p1.X) + p1.X, bounds.Bottom);
                    if (bounds.Left <= candidate.X && candidate.X <= bounds.Right) results.Add(candidate);
                }
            }

            return results;
        }

        private ColorBlend CalculateColorBlend(ISvgRenderer renderer, float opacity, PointF specifiedStart, PointF effectiveStart, PointF specifiedEnd, PointF effectiveEnd)
        {
            float startExtend;
            float endExtend;
            List<Color> colors;
            List<float> positions;
            
            var colorBlend = GetColorBlend(renderer, opacity, false);

            var startDelta = CalculateDistance(specifiedStart, effectiveStart);
            var endDelta = CalculateDistance(specifiedEnd, effectiveEnd);

            if (!(startDelta > 0) && !(endDelta > 0))
            {
                return colorBlend;
            }

            var specifiedLength = CalculateDistance(specifiedStart, specifiedEnd);
            var specifiedUnitVector = new PointF((specifiedEnd.X - specifiedStart.X) / (float)specifiedLength, (specifiedEnd.Y - specifiedStart.Y) / (float)specifiedLength);

            var effectiveLength = CalculateDistance(effectiveStart, effectiveEnd);

            switch (SpreadMethod)
            {
                case SvgGradientSpreadMethod.Reflect:
                    startExtend = (float)(Math.Ceiling(CalculateDistance(effectiveStart, specifiedStart) / specifiedLength));
                    endExtend = (float)(Math.Ceiling(CalculateDistance(effectiveEnd, specifiedEnd) / specifiedLength));
                    colors = colorBlend.Colors.ToList();
                    positions = (from p in colorBlend.Positions select p + startExtend).ToList();

                    for (var i = 0; i < startExtend; i++)
                    {
                        if (i % 2 == 0)
                        {
                            for (var j = 1; j < colorBlend.Positions.Length; j++)
                            {
                                positions.Insert(0, (float)((startExtend - 1 - i) + 1 - colorBlend.Positions[j]));
                                colors.Insert(0, colorBlend.Colors[j]);
                            }
                        }
                        else
                        {
                            for (var j = 0; j < colorBlend.Positions.Length - 1; j++)
                            {
                                positions.Insert(j, (float)((startExtend - 1 - i) + colorBlend.Positions[j]));
                                colors.Insert(j, colorBlend.Colors[j]);
                            }
                        }
                    }

                    int insertPos;
                    for (var i = 0; i < endExtend; i++)
                    {
                        if (i % 2 == 0)
                        {
                            insertPos = positions.Count;
                            for (var j = 0; j < colorBlend.Positions.Length - 1; j++)
                            {
                                positions.Insert(insertPos, (float)((startExtend + 1 + i) + 1 - colorBlend.Positions[j]));
                                colors.Insert(insertPos, colorBlend.Colors[j]);
                            }
                        }
                        else
                        {
                            for (var j = 1; j < colorBlend.Positions.Length; j++)
                            {
                                positions.Add((float)((startExtend + 1 + i) + colorBlend.Positions[j]));
                                colors.Add(colorBlend.Colors[j]);
                            }
                        }
                    }

                    colorBlend.Colors = colors.ToArray();
                    colorBlend.Positions = (from p in positions select p / (startExtend + 1 + endExtend)).ToArray();
                    break;
                case SvgGradientSpreadMethod.Repeat:
                    startExtend = (float)(Math.Ceiling(CalculateDistance(effectiveStart, specifiedStart) / specifiedLength));
                    endExtend = (float)(Math.Ceiling(CalculateDistance(effectiveEnd, specifiedEnd) / specifiedLength));
                    colors = new List<Color>();
                    positions = new List<float>();

                    for (int i = 0; i < startExtend + endExtend + 1; i++)
                    {
                        for (int j = 0; j < colorBlend.Positions.Length; j++)
                        {
                            positions.Add((i + colorBlend.Positions[j] * 0.9999f) / (startExtend + endExtend + 1));
                            colors.Add(colorBlend.Colors[j]);
                        }
                    }
                    positions[positions.Count - 1] = 1.0f;

                    colorBlend.Colors = colors.ToArray();
                    colorBlend.Positions = positions.ToArray();

                    break;
                default:
                    for (var i = 0; i < colorBlend.Positions.Length; i++)
                    {
                        var originalPoint = MovePointAlongVector(specifiedStart, specifiedUnitVector, (float)specifiedLength * colorBlend.Positions[i]);

                        var distanceFromEffectiveStart = CalculateDistance(effectiveStart, originalPoint);

                        colorBlend.Positions[i] = (float)Math.Round(Math.Max(0F, Math.Min((distanceFromEffectiveStart / effectiveLength), 1.0F)), 5);
                    }

                    if (startDelta > 0)
                    {
                        colorBlend.Positions = new[] { 0F }.Concat(colorBlend.Positions).ToArray();
                        colorBlend.Colors = new[] { colorBlend.Colors.First() }.Concat(colorBlend.Colors).ToArray();
                    }

                    if (endDelta > 0)
                    {
                        colorBlend.Positions = colorBlend.Positions.Concat(new[] { 1F }).ToArray();
                        colorBlend.Colors = colorBlend.Colors.Concat(new[] { colorBlend.Colors.Last() }).ToArray();
                    }
                    break;
            }

            return colorBlend;
        }

        private static PointF CalculateClosestIntersectionPoint(PointF sourcePoint, IList<PointF> targetPoints)
        {
            Debug.Assert(targetPoints.Count == 2, "Unexpected number of intersection points!");

            return CalculateDistance(sourcePoint, targetPoints[0]) < CalculateDistance(sourcePoint, targetPoints[1]) ? targetPoints[0] : targetPoints[1];
        }

        private static PointF MovePointAlongVector(PointF start, PointF unitVector, float distance)
        {
            return start + new SizeF(unitVector.X * distance, unitVector.Y * distance);
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgLinearGradientServer>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgLinearGradientServer;
            newObj.X1 = this.X1;
            newObj.Y1 = this.Y1;
            newObj.X2 = this.X2;
            newObj.Y2 = this.Y2;
            return newObj;

        }

        private sealed class LineF
        {
            private float X1
            {
                get;
                set;
            }

            private float Y1
            {
                get;
                set;
            }

            private float X2
            {
                get;
                set;
            }

            private float Y2
            {
                get;
                set;
            }

            public LineF(float x1, float y1, float x2, float y2)
            {
                X1 = x1;
                Y1 = y1;
                X2 = x2;
                Y2 = y2;
            }

            public List<PointF> Intersection(RectangleF rectangle)
            {
                var result = new List<PointF>();

                AddIfIntersect(this, new LineF(rectangle.X, rectangle.Y, rectangle.Right, rectangle.Y), result);
                AddIfIntersect(this, new LineF(rectangle.Right, rectangle.Y, rectangle.Right, rectangle.Bottom), result);
                AddIfIntersect(this, new LineF(rectangle.Right, rectangle.Bottom, rectangle.X, rectangle.Bottom), result);
                AddIfIntersect(this, new LineF(rectangle.X, rectangle.Bottom, rectangle.X, rectangle.Y), result);

                return result;
            }

            /// <remarks>http://community.topcoder.com/tc?module=Static&d1=tutorials&d2=geometry2</remarks>
            private PointF? Intersection(LineF other)
            {
                const int precision = 8;

                var a1 = (double)Y2 - Y1;
                var b1 = (double)X1 - X2;
                var c1 = a1 * X1 + b1 * Y1;

                var a2 = (double)other.Y2 - other.Y1;
                var b2 = (double)other.X1 - other.X2;
                var c2 = a2 * other.X1 + b2 * other.Y1;

                var det = a1 * b2 - a2 * b1;
                if (det == 0)
                {
                    return null;
                }
                else
                {
                    var xi = (b2 * c1 - b1 * c2) / det;
                    var yi = (a1 * c2 - a2 * c1) / det;

                    if (Math.Round(Math.Min(X1, X2), precision) <= Math.Round(xi, precision) &&
                        Math.Round(xi, precision) <= Math.Round(Math.Max(X1, X2), precision) &&
                        Math.Round(Math.Min(Y1, Y2), precision) <= Math.Round(yi, precision) &&
                        Math.Round(yi, precision) <= Math.Round(Math.Max(Y1, Y2), precision) &&
                        Math.Round(Math.Min(other.X1, other.X2), precision) <= Math.Round(xi, precision) &&
                        Math.Round(xi, precision) <= Math.Round(Math.Max(other.X1, other.X2), precision) &&
                        Math.Round(Math.Min(other.Y1, other.Y2), precision) <= Math.Round(yi, precision) &&
                        Math.Round(yi, precision) <= Math.Round(Math.Max(other.Y1, other.Y2), precision))
                    {
                        return new PointF((float)xi, (float)yi);
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            private static void AddIfIntersect(LineF first, LineF second, ICollection<PointF> result)
            {
                var intersection = first.Intersection(second);

                if (intersection != null)
                {
                    result.Add(intersection.Value);
                }
            }
        }
    }
}