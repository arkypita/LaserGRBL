using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Svg.DataTypes;

namespace Svg
{
    [SvgElement("marker")]
    public class SvgMarker : SvgVisualElement, ISvgViewPort
    {
        private SvgOrient _svgOrient = new SvgOrient();

        [SvgAttribute("refX")]
        public virtual SvgUnit RefX
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("refX"); }
            set { this.Attributes["refX"] = value; }
        }

        [SvgAttribute("refY")]
        public virtual SvgUnit RefY
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("refY"); }
            set { this.Attributes["refY"] = value; }
        }


        [SvgAttribute("orient")]
        public virtual SvgOrient Orient
        {
            get { return _svgOrient; }
            set { _svgOrient = value; }
        }


        [SvgAttribute("overflow")]
        public virtual SvgOverflow Overflow
        {
            get { return this.Attributes.GetAttribute<SvgOverflow>("overflow"); }
            set { this.Attributes["overflow"] = value; }
        }


        [SvgAttribute("viewBox")]
        public virtual SvgViewBox ViewBox
        {
            get { return this.Attributes.GetAttribute<SvgViewBox>("viewBox"); }
            set { this.Attributes["viewBox"] = value; }
        }


        [SvgAttribute("preserveAspectRatio")]
        public virtual SvgAspectRatio AspectRatio
        {
            get { return this.Attributes.GetAttribute<SvgAspectRatio>("preserveAspectRatio"); }
            set { this.Attributes["preserveAspectRatio"] = value; }
        }


        [SvgAttribute("markerWidth")]
        public virtual SvgUnit MarkerWidth
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("markerWidth"); }
            set { this.Attributes["markerWidth"] = value; }
        }

        [SvgAttribute("markerHeight")]
        public virtual SvgUnit MarkerHeight
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("markerHeight"); }
            set { this.Attributes["markerHeight"] = value; }
        }

        [SvgAttribute("markerUnits")]
        public virtual SvgMarkerUnits MarkerUnits
        {
            get { return this.Attributes.GetAttribute<SvgMarkerUnits>("markerUnits"); }
            set { this.Attributes["markerUnits"] = value; }
        }

        public SvgMarker()
        {
            MarkerUnits = SvgMarkerUnits.StrokeWidth;
            MarkerHeight = 3;
            MarkerWidth = 3;
            Overflow = SvgOverflow.Hidden;
        }

        public override System.Drawing.Drawing2D.GraphicsPath Path(ISvgRenderer renderer)
        {
            var path = this.Children.FirstOrDefault(x => x is SvgVisualElement);
            if (path != null)
                return (path as SvgVisualElement).Path(renderer);
            return null;
        }

        public override System.Drawing.RectangleF Bounds
        {
            get
            {
                var path = this.Path(null);
                if (path != null)
                    return path.GetBounds();
                return new System.Drawing.RectangleF();
            }
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgMarker>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgMarker;
            newObj.RefX = this.RefX;
            newObj.RefY = this.RefY;
            newObj.Orient = this.Orient;
            newObj.ViewBox = this.ViewBox;
            newObj.Overflow = this.Overflow;
            newObj.AspectRatio = this.AspectRatio;

            return newObj;
        }

        /// <summary>
        /// Render this marker using the slope of the given line segment
        /// </summary>
        /// <param name="pRenderer"></param>
        /// <param name="pOwner"></param>
        /// <param name="pMarkerPoint1"></param>
        /// <param name="pMarkerPoint2"></param>
        public void RenderMarker(ISvgRenderer pRenderer, SvgVisualElement pOwner, PointF pRefPoint, PointF pMarkerPoint1, PointF pMarkerPoint2)
        {
            float fAngle1 = 0f;
            if (Orient.IsAuto)
            {
                // Only calculate this if needed.
                float xDiff = pMarkerPoint2.X - pMarkerPoint1.X;
                float yDiff = pMarkerPoint2.Y - pMarkerPoint1.Y;
                fAngle1 = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);
            }

            RenderPart2(fAngle1, pRenderer, pOwner, pRefPoint);
        }

        /// <summary>
        /// Render this marker using the average of the slopes of the two given line segments
        /// </summary>
        /// <param name="pRenderer"></param>
        /// <param name="pOwner"></param>
        /// <param name="pMarkerPoint1"></param>
        /// <param name="pMarkerPoint2"></param>
        /// <param name="pMarkerPoint3"></param>
        public void RenderMarker(ISvgRenderer pRenderer, SvgVisualElement pOwner, PointF pRefPoint, PointF pMarkerPoint1, PointF pMarkerPoint2, PointF pMarkerPoint3)
        {
            float xDiff = pMarkerPoint2.X - pMarkerPoint1.X;
            float yDiff = pMarkerPoint2.Y - pMarkerPoint1.Y;
            float fAngle1 = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);

            xDiff = pMarkerPoint3.X - pMarkerPoint2.X;
            yDiff = pMarkerPoint3.Y - pMarkerPoint2.Y;
            float fAngle2 = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);

            RenderPart2((fAngle1 + fAngle2) / 2, pRenderer, pOwner, pRefPoint);
        }

        /// <summary>
        /// Common code for rendering a marker once the orientation angle has been calculated
        /// </summary>
        /// <param name="fAngle"></param>
        /// <param name="pRenderer"></param>
        /// <param name="pOwner"></param>
        /// <param name="pMarkerPoint"></param>
        private void RenderPart2(float fAngle, ISvgRenderer pRenderer, SvgVisualElement pOwner, PointF pMarkerPoint)
        {
            using (var pRenderPen = CreatePen(pOwner, pRenderer))
            {
                using (var markerPath = GetClone(pOwner))
                {
                    using (var transMatrix = new Matrix())
                    {
                        transMatrix.Translate(pMarkerPoint.X, pMarkerPoint.Y);
                        if (Orient.IsAuto)
                            transMatrix.Rotate(fAngle);
                        else
                            transMatrix.Rotate(Orient.Angle);
                        switch (MarkerUnits)
                        {
                            case SvgMarkerUnits.StrokeWidth:
                                if (ViewBox.Width > 0 && ViewBox.Height > 0)
                                {
                                    transMatrix.Translate(AdjustForViewBoxWidth(-RefX.ToDeviceValue(pRenderer, UnitRenderingType.Horizontal, this) *
                                                            pOwner.StrokeWidth.ToDeviceValue(pRenderer, UnitRenderingType.Other, this)),
                                                          AdjustForViewBoxHeight(-RefY.ToDeviceValue(pRenderer, UnitRenderingType.Vertical, this) *
                                                            pOwner.StrokeWidth.ToDeviceValue(pRenderer, UnitRenderingType.Other, this)));
                                }
                                else
                                {
                                    // SvgMarkerUnits.UserSpaceOnUse
                                    //	TODO: We know this isn't correct.
                                    //        But use this until the TODOs from AdjustForViewBoxWidth and AdjustForViewBoxHeight are done.
                                    //  MORE see Unit Test "MakerEndTest.TestArrowCodeCreation()"
                                    transMatrix.Translate(-RefX.ToDeviceValue(pRenderer, UnitRenderingType.Horizontal, this),
                                                         -RefY.ToDeviceValue(pRenderer, UnitRenderingType.Vertical, this));
                                }
                                break;
                            case SvgMarkerUnits.UserSpaceOnUse:
                                transMatrix.Translate(-RefX.ToDeviceValue(pRenderer, UnitRenderingType.Horizontal, this),
                                                      -RefY.ToDeviceValue(pRenderer, UnitRenderingType.Vertical, this));
                                break;
                        }
                        markerPath.Transform(transMatrix);
                        if (pRenderPen != null) pRenderer.DrawPath(pRenderPen, markerPath);

                        SvgPaintServer pFill = this.Children.First().Fill;
                        SvgFillRule pFillRule = FillRule;								// TODO: What do we use the fill rule for?
                        float fOpacity = FillOpacity;

                        if (pFill != null)
                        {
                            using (var pBrush = pFill.GetBrush(this, pRenderer, fOpacity))
                            {
                                pRenderer.FillPath(pBrush, markerPath);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Create a pen that can be used to render this marker
        /// </summary>
        /// <param name="pStroke"></param>
        /// <returns></returns>
        private Pen CreatePen(SvgVisualElement pPath, ISvgRenderer renderer)
        {
            if (pPath.Stroke == null) return null;
            Brush pBrush = pPath.Stroke.GetBrush(this, renderer, Opacity);
            switch (MarkerUnits)
            {
                case SvgMarkerUnits.StrokeWidth:
                    return (new Pen(pBrush, StrokeWidth.ToDeviceValue(renderer, UnitRenderingType.Other, this) * 
                                            pPath.StrokeWidth.ToDeviceValue(renderer, UnitRenderingType.Other, this)));
                case SvgMarkerUnits.UserSpaceOnUse:
                    return (new Pen(pBrush, StrokeWidth.ToDeviceValue(renderer, UnitRenderingType.Other, this)));
            }
            return (new Pen(pBrush, StrokeWidth.ToDeviceValue(renderer, UnitRenderingType.Other, this)));
        }

        /// <summary>
        /// Get a clone of the current path, scaled for the stroke width
        /// </summary>
        /// <returns></returns>
        private GraphicsPath GetClone(SvgVisualElement pPath)
        {
            GraphicsPath pRet = Path(null).Clone() as GraphicsPath;
            switch (MarkerUnits)
            {
                case SvgMarkerUnits.StrokeWidth:
                    using (var transMatrix = new Matrix())
                    {
                        transMatrix.Scale(AdjustForViewBoxWidth(pPath.StrokeWidth), AdjustForViewBoxHeight(pPath.StrokeWidth));
                        pRet.Transform(transMatrix);
                    }
                    break;
                case SvgMarkerUnits.UserSpaceOnUse:
                    break;
            }
            return (pRet);
        }

        /// <summary>
        /// Adjust the given value to account for the width of the viewbox in the viewport
        /// </summary>
        /// <param name="fWidth"></param>
        /// <returns></returns>
        private float AdjustForViewBoxWidth(float fWidth)
        {
            //	TODO: We know this isn't correct
            return (ViewBox.Width <= 0 ? 1 : fWidth / ViewBox.Width);
        }

        /// <summary>
        /// Adjust the given value to account for the height of the viewbox in the viewport
        /// </summary>
        /// <param name="fWidth"></param>
        /// <returns></returns>
        private float AdjustForViewBoxHeight(float fHeight)
        {
            //	TODO: We know this isn't correct
            return (ViewBox.Height <= 0 ? 1 : fHeight / ViewBox.Height);
        }
    }
}