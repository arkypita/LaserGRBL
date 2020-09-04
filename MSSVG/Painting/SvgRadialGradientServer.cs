using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Svg
{
    [SvgElement("radialGradient")]
    public sealed class SvgRadialGradientServer : SvgGradientServer
    {
        [SvgAttribute("cx")]
        public SvgUnit CenterX
        {
            get
            {
                return this.Attributes.GetAttribute<SvgUnit>("cx");
            }
            set
            {
                this.Attributes["cx"] = value;
            }
        }

        [SvgAttribute("cy")]
        public SvgUnit CenterY
        {
            get
            {
                return this.Attributes.GetAttribute<SvgUnit>("cy");
            }
            set
            {
                this.Attributes["cy"] = value;
            }
        }

        [SvgAttribute("r")]
        public SvgUnit Radius
        {
            get
            {
                return this.Attributes.GetAttribute<SvgUnit>("r");
            }
            set
            {
                this.Attributes["r"] = value;
            }
        }

        [SvgAttribute("fx")]
        public SvgUnit FocalX
        {
            get
            {
                var value = this.Attributes.GetAttribute<SvgUnit>("fx");

                if (value.IsEmpty || value.IsNone)
                {
                    value = this.CenterX;
                }

                return value;
            }
            set
            {
                this.Attributes["fx"] = value;
            }
        }

        [SvgAttribute("fy")]
        public SvgUnit FocalY
        {
            get
            {
                var value = this.Attributes.GetAttribute<SvgUnit>("fy");

                if (value.IsEmpty || value.IsNone)
                {
                    value = this.CenterY;
                }

                return value;
            }
            set
            {
                this.Attributes["fy"] = value;
            }
        }

        public SvgRadialGradientServer()
        {
            CenterX = new SvgUnit(SvgUnitType.Percentage, 50F);
            CenterY = new SvgUnit(SvgUnitType.Percentage, 50F);
            Radius = new SvgUnit(SvgUnitType.Percentage, 50F);
        }

        private object _lockObj = new Object();

        private SvgUnit NormalizeUnit(SvgUnit orig)
        {
            return (orig.Type == SvgUnitType.Percentage && this.GradientUnits == SvgCoordinateUnits.ObjectBoundingBox ?
                    new SvgUnit(SvgUnitType.User, orig.Value / 100) :
                    orig);
        }

        public override Brush GetBrush(SvgVisualElement renderingElement, ISvgRenderer renderer, float opacity, bool forStroke = false)
        {
            LoadStops(renderingElement);

            try
            {
                if (this.GradientUnits == SvgCoordinateUnits.ObjectBoundingBox) renderer.SetBoundable(renderingElement);

                // Calculate the path and transform it appropriately
                var center = new PointF(NormalizeUnit(CenterX).ToDeviceValue(renderer, UnitRenderingType.Horizontal, this),
                                        NormalizeUnit(CenterY).ToDeviceValue(renderer, UnitRenderingType.Vertical, this));
                var focals = new PointF[] {new PointF(NormalizeUnit(FocalX).ToDeviceValue(renderer, UnitRenderingType.Horizontal, this),
                                                      NormalizeUnit(FocalY).ToDeviceValue(renderer, UnitRenderingType.Vertical, this)) };
                var specifiedRadius = NormalizeUnit(Radius).ToDeviceValue(renderer, UnitRenderingType.Other, this);
                var path = new GraphicsPath();
                path.AddEllipse(
                    center.X - specifiedRadius, center.Y - specifiedRadius,
                    specifiedRadius * 2, specifiedRadius * 2
                );

                using (var transform = EffectiveGradientTransform)
                {
                    var bounds = renderer.GetBoundable().Bounds;
                    transform.Translate(bounds.X, bounds.Y, MatrixOrder.Prepend);
                    if (this.GradientUnits == SvgCoordinateUnits.ObjectBoundingBox)
                    {
                        transform.Scale(bounds.Width, bounds.Height, MatrixOrder.Prepend);
                    }
                    path.Transform(transform);
                    transform.TransformPoints(focals);
                }


                // Calculate any required scaling
                var scaleBounds = RectangleF.Inflate(renderingElement.Bounds, renderingElement.StrokeWidth, renderingElement.StrokeWidth);
								var scale = CalcScale(scaleBounds, path);

                // Not ideal, but this makes sure that the rest of the shape gets properly filled or drawn
                if (scale > 1.0f && SpreadMethod == SvgGradientSpreadMethod.Pad)
                {
                    var stop = Stops.Last();
                    var origColor = stop.GetColor(renderingElement);
                    var renderColor = System.Drawing.Color.FromArgb((int)Math.Round(opacity * stop.Opacity * 255), origColor);

                    var origClip = renderer.GetClip();
                    try
                    {
                        using (var solidBrush = new SolidBrush(renderColor))
                        {
                            var newClip = origClip.Clone();
                            newClip.Exclude(path);
                            renderer.SetClip(newClip);

                            var renderPath = (GraphicsPath)renderingElement.Path(renderer);
                            if (forStroke)
                            {
                                using (var pen = new Pen(solidBrush, renderingElement.StrokeWidth.ToDeviceValue(renderer, UnitRenderingType.Other, renderingElement)))
                                {
                                    renderer.DrawPath(pen, renderPath);
                                }
                            }
                            else
                            {
                                renderer.FillPath(solidBrush, renderPath);
                            }
                        }
                    }
                    finally
                    {
                        renderer.SetClip(origClip);
                    }
                }

                // Get the color blend and any tweak to the scaling
                var blend = CalculateColorBlend(renderer, opacity, scale, out scale);

                // Transform the path based on the scaling
                var gradBounds = path.GetBounds();
                var transCenter = new PointF(gradBounds.Left + gradBounds.Width / 2, gradBounds.Top + gradBounds.Height / 2);
                using (var scaleMat = new Matrix())
                {
                    scaleMat.Translate(-1 * transCenter.X, -1 * transCenter.Y, MatrixOrder.Append);
                    scaleMat.Scale(scale, scale, MatrixOrder.Append);
                    scaleMat.Translate(transCenter.X, transCenter.Y, MatrixOrder.Append);
                    path.Transform(scaleMat);
                }

                // calculate the brush
                var brush = new PathGradientBrush(path);
                brush.CenterPoint = focals[0];
                brush.InterpolationColors = blend;

                return brush;
            }
            finally
            {
                if (this.GradientUnits == SvgCoordinateUnits.ObjectBoundingBox) renderer.PopBoundable();
            }
        }

        /// <summary>
        /// Determine how much (approximately) the path must be scaled to contain the rectangle
        /// </summary>
        /// <param name="bounds">Bounds that the path must contain</param>
        /// <param name="path">Path of the gradient</param>
        /// <returns>Scale factor</returns>
        /// <remarks>
        /// This method continually transforms the rectangle (fewer points) until it is contained by the path
        /// and returns the result of the search.  The scale factor is set to a constant 95%
        /// </remarks>
        private float CalcScale(RectangleF bounds, GraphicsPath path, Graphics graphics = null)
        {
            var points = new PointF[] {
                new PointF(bounds.Left, bounds.Top), 
                new PointF(bounds.Right, bounds.Top), 
                new PointF(bounds.Right, bounds.Bottom), 
                new PointF(bounds.Left, bounds.Bottom) 
            };
            var pathBounds = path.GetBounds();
            var pathCenter = new PointF(pathBounds.X + pathBounds.Width / 2, pathBounds.Y + pathBounds.Height / 2);
            using (var transform = new Matrix())
            {
                transform.Translate(-1 * pathCenter.X, -1 * pathCenter.Y, MatrixOrder.Append);
                transform.Scale(.95f, .95f, MatrixOrder.Append);
                transform.Translate(pathCenter.X, pathCenter.Y, MatrixOrder.Append);

                while (!(path.IsVisible(points[0]) && path.IsVisible(points[1]) &&
                         path.IsVisible(points[2]) && path.IsVisible(points[3])))
                {
										var previousPoints = new PointF[] 
										{
												new PointF(points[0].X, points[0].Y), 
												new PointF(points[1].X, points[1].Y), 
												new PointF(points[2].X, points[2].Y), 
												new PointF(points[3].X, points[3].Y) 
										};

                    transform.TransformPoints(points);

										if (Enumerable.SequenceEqual(previousPoints, points))
										{
											break;
										}
                }
            }
            return bounds.Height / (points[2].Y - points[1].Y);
        }

        //New plan:
        // scale the outer rectangle to always encompass ellipse
        // cut the ellipse in half (either vertical or horizontal) 
        // determine the region on each side of the ellipse
        private static IEnumerable<GraphicsPath> GetDifference(RectangleF subject, GraphicsPath clip)
        {
            var clipFlat = (GraphicsPath)clip.Clone();
            clipFlat.Flatten();
            var clipBounds = clipFlat.GetBounds();
            var bounds = RectangleF.Union(subject, clipBounds);
            bounds.Inflate(bounds.Width * .3f, bounds.Height * 0.3f);

            var clipMidPoint = new PointF((clipBounds.Left + clipBounds.Right) / 2, (clipBounds.Top + clipBounds.Bottom) / 2);
            var leftPoints = new List<PointF>();
            var rightPoints = new List<PointF>();
            foreach (var pt in clipFlat.PathPoints)
            {
                if (pt.X <= clipMidPoint.X)
                {
                    leftPoints.Add(pt);
                }
                else
                {
                    rightPoints.Add(pt);
                }
            }
            leftPoints.Sort((p, q) => p.Y.CompareTo(q.Y));
            rightPoints.Sort((p, q) => p.Y.CompareTo(q.Y));

            var point = new PointF((leftPoints.Last().X + rightPoints.Last().X) / 2,
                                   (leftPoints.Last().Y + rightPoints.Last().Y) / 2);
            leftPoints.Add(point);
            rightPoints.Add(point);
            point = new PointF(point.X, bounds.Bottom);
            leftPoints.Add(point);
            rightPoints.Add(point);

            leftPoints.Add(new PointF(bounds.Left, bounds.Bottom));
            leftPoints.Add(new PointF(bounds.Left, bounds.Top));
            rightPoints.Add(new PointF(bounds.Right, bounds.Bottom));
            rightPoints.Add(new PointF(bounds.Right, bounds.Top));

            point = new PointF((leftPoints.First().X + rightPoints.First().X) / 2, bounds.Top);
            leftPoints.Add(point);
            rightPoints.Add(point);
            point = new PointF(point.X, (leftPoints.First().Y + rightPoints.First().Y) / 2);
            leftPoints.Add(point);
            rightPoints.Add(point);

            var path = new GraphicsPath(FillMode.Winding);
            path.AddPolygon(leftPoints.ToArray());
            yield return path;

            path.Reset();
            path.AddPolygon(rightPoints.ToArray());
            yield return path;
        }

        private static GraphicsPath CreateGraphicsPath(PointF origin, PointF centerPoint, float effectiveRadius)
        {
            var path = new GraphicsPath();

            path.AddEllipse(
                origin.X + centerPoint.X - effectiveRadius,
                origin.Y + centerPoint.Y - effectiveRadius,
                effectiveRadius * 2,
                effectiveRadius * 2
            );

            return path;
        }

        private ColorBlend CalculateColorBlend(ISvgRenderer renderer, float opacity, float scale, out float outScale)
        {
            var colorBlend = GetColorBlend(renderer, opacity, true);
            float newScale;
            List<float> pos;
            List<Color> colors;

            outScale = scale;
            if (scale > 1)
            {
                switch (this.SpreadMethod)
                {
                    case SvgGradientSpreadMethod.Reflect:
                        newScale = (float)Math.Ceiling(scale);
                        pos = (from p in colorBlend.Positions select 1 + (p - 1) / newScale).ToList();
                        colors = colorBlend.Colors.ToList();

                        for (var i = 1; i < newScale; i++)
                        {
                            if (i % 2 == 1)
                            {
                                for (int j = 1; j < colorBlend.Positions.Length; j++)
                                {
                                    pos.Insert(0, (newScale - i - 1) / newScale + 1 - colorBlend.Positions[j]);
                                    colors.Insert(0, colorBlend.Colors[j]);
                                }
                            }
                            else
                            {
                                for (int j = 0; j < colorBlend.Positions.Length - 1; j++)
                                {
                                    pos.Insert(j, (newScale - i - 1) / newScale + colorBlend.Positions[j]);
                                    colors.Insert(j, colorBlend.Colors[j]);
                                }
                            }
                        }

                        colorBlend.Positions = pos.ToArray();
                        colorBlend.Colors = colors.ToArray();
                        outScale = newScale;
                        break;
                    case SvgGradientSpreadMethod.Repeat:
                        newScale = (float)Math.Ceiling(scale);
                        pos = (from p in colorBlend.Positions select p / newScale).ToList();
                        colors = colorBlend.Colors.ToList();

                        for (var i = 1; i < newScale; i++)
                        {
                            pos.AddRange(from p in colorBlend.Positions select (i + (p <= 0 ? 0.001f : p)) / newScale);
                            colors.AddRange(colorBlend.Colors);
                        }

                        colorBlend.Positions = pos.ToArray();
                        colorBlend.Colors = colors.ToArray();
                        outScale = newScale;
                        break;
                    default:
                        outScale = 1.0f;
                        //for (var i = 0; i < colorBlend.Positions.Length - 1; i++)
                        //{
                        //    colorBlend.Positions[i] = 1 - (1 - colorBlend.Positions[i]) / scale;
                        //}

                        //colorBlend.Positions = new[] { 0F }.Concat(colorBlend.Positions).ToArray();
                        //colorBlend.Colors = new[] { colorBlend.Colors.First() }.Concat(colorBlend.Colors).ToArray();

                        break;
                }
            }

            return colorBlend;
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgRadialGradientServer>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgRadialGradientServer;

            newObj.CenterX = this.CenterX;
            newObj.CenterY = this.CenterY;
            newObj.Radius = this.Radius;
            newObj.FocalX = this.FocalX;
            newObj.FocalY = this.FocalY;

            return newObj;
        }
    }
}
