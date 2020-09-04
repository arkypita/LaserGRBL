using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Svg.ExtensionMethods;

namespace Svg
{
    /// <summary>
    /// Represents and SVG line element.
    /// </summary>
    [SvgElement("line")]
    public class SvgLine : SvgPathBasedElement
    {
        private SvgUnit _startX;
        private SvgUnit _startY;
        private SvgUnit _endX;
        private SvgUnit _endY;
        private GraphicsPath _path;

        [SvgAttribute("x1")]
        public SvgUnit StartX
        {
            get { return this._startX; }
            set 
            { 
            	if(_startX != value)
            	{
            		this._startX = value;
            		this.IsPathDirty = true;
            		OnAttributeChanged(new AttributeEventArgs{ Attribute = "x1", Value = value });
            	}
            }
        }

        [SvgAttribute("y1")]
        public SvgUnit StartY
        {
            get { return this._startY; }
            set 
            { 
            	if(_startY != value)
            	{
            		this._startY = value;
            		this.IsPathDirty = true;
            		OnAttributeChanged(new AttributeEventArgs{ Attribute = "y1", Value = value });
            	}
            }
        }

        [SvgAttribute("x2")]
        public SvgUnit EndX
        {
            get { return this._endX; }
            set 
            { 
            	if(_endX != value)
            	{
            		this._endX = value;
            		this.IsPathDirty = true;
            		OnAttributeChanged(new AttributeEventArgs{ Attribute = "x2", Value = value });
            	}
            }
        }

        [SvgAttribute("y2")]
        public SvgUnit EndY
        {
            get { return this._endY; }
            set 
            { 
            	if(_endY != value)
            	{
            		this._endY = value;
            		this.IsPathDirty = true;
            		OnAttributeChanged(new AttributeEventArgs{ Attribute = "y2", Value = value });
            	}
            }
        }

        /// <summary>
        /// Gets or sets the marker (end cap) of the path.
        /// </summary>
        [SvgAttribute("marker-end")]
        public Uri MarkerEnd
        {
            get { return this.Attributes.GetAttribute<Uri>("marker-end").ReplaceWithNullIfNone(); }
            set { this.Attributes["marker-end"] = value; }
        }


        /// <summary>
        /// Gets or sets the marker (start cap) of the path.
        /// </summary>
        [SvgAttribute("marker-mid")]
        public Uri MarkerMid
        {
            get { return this.Attributes.GetAttribute<Uri>("marker-mid").ReplaceWithNullIfNone(); }
            set { this.Attributes["marker-mid"] = value; }
        }


        /// <summary>
        /// Gets or sets the marker (start cap) of the path.
        /// </summary>
        [SvgAttribute("marker-start")]
        public Uri MarkerStart
        {
	        get { return this.Attributes.GetAttribute<Uri>("marker-start").ReplaceWithNullIfNone(); }
            set { this.Attributes["marker-start"] = value; }
        }

        public override SvgPaintServer Fill
        {
            get { return null; /* Line can't have a fill */ }
            set
            {
                // Do nothing
            }
        }

        public SvgLine()
        {
        }

        public override System.Drawing.Drawing2D.GraphicsPath Path(ISvgRenderer renderer)
        {
            if ((this._path == null || this.IsPathDirty) && base.StrokeWidth > 0)
            {
                PointF start = new PointF(this.StartX.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this), 
                                          this.StartY.ToDeviceValue(renderer, UnitRenderingType.Vertical, this));
                PointF end = new PointF(this.EndX.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this), 
                                        this.EndY.ToDeviceValue(renderer, UnitRenderingType.Vertical, this));

                this._path = new GraphicsPath();

                // If it is to render, don't need to consider stroke width.
                // i.e stroke width only to be considered when calculating boundary
                if (renderer != null)
                {
                  this._path.AddLine(start, end);
                  this.IsPathDirty = false;
                }
                else
                {	 // only when calculating boundary 
                  _path.StartFigure();
                  var radius = base.StrokeWidth / 2;
                  _path.AddEllipse(start.X - radius, start.Y - radius, 2 * radius, 2 * radius);
                  _path.AddEllipse(end.X - radius, end.Y - radius, 2 * radius, 2 * radius);
                  _path.CloseFigure();
                }
            }
            return this._path;
        }

        /// <summary>
        /// Renders the stroke of the <see cref="SvgVisualElement"/> to the specified <see cref="ISvgRenderer"/>
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> object to render to.</param>
        protected internal override bool RenderStroke(ISvgRenderer renderer)
        {
            var result = base.RenderStroke(renderer);
            var path = this.Path(renderer);

            if (this.MarkerStart != null)
            {
                SvgMarker marker = this.OwnerDocument.GetElementById<SvgMarker>(this.MarkerStart.ToString());
                marker.RenderMarker(renderer, this, path.PathPoints[0], path.PathPoints[0], path.PathPoints[1]);
            }

            if (this.MarkerMid != null)
            {
                SvgMarker marker = this.OwnerDocument.GetElementById<SvgMarker>(this.MarkerMid.ToString());
                for (int i = 1; i <= path.PathPoints.Length - 2; i++)
                    marker.RenderMarker(renderer, this, path.PathPoints[i], path.PathPoints[i - 1], path.PathPoints[i], path.PathPoints[i + 1]);
            }

            if (this.MarkerEnd != null)
            {
                SvgMarker marker = this.OwnerDocument.GetElementById<SvgMarker>(this.MarkerEnd.ToString());
                marker.RenderMarker(renderer, this, path.PathPoints[path.PathPoints.Length - 1], path.PathPoints[path.PathPoints.Length - 2], path.PathPoints[path.PathPoints.Length - 1]);
            }

            return result;
        }

		public override SvgElement DeepCopy()
		{
			return DeepCopy<SvgLine>();
		}

		public override SvgElement DeepCopy<T>()
		{
			var newObj = base.DeepCopy<T>() as SvgLine;
			newObj.StartX = this.StartX;
			newObj.EndX = this.EndX;
			newObj.StartY = this.StartY;
			newObj.EndY = this.EndY;
			if (this.Fill != null)
				newObj.Fill = this.Fill.DeepCopy() as SvgPaintServer;

			return newObj;
		}

    }
}
