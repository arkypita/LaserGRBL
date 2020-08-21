using System.Drawing.Drawing2D;

namespace Svg
{
    /// <summary>
    /// Represents and SVG ellipse element.
    /// </summary>
    [SvgElement("ellipse")]
    public class SvgEllipse : SvgPathBasedElement
    {
        private SvgUnit _radiusX;
        private SvgUnit _radiusY;
        private SvgUnit _centerX;
        private SvgUnit _centerY;
        private GraphicsPath _path;

        [SvgAttribute("cx")]
        public virtual SvgUnit CenterX
        {
            get { return this._centerX; }
            set
            {
            	if(_centerX != value)
            	{
            		this._centerX = value;
            		this.IsPathDirty = true;
            		OnAttributeChanged(new AttributeEventArgs{ Attribute = "cx", Value = value });
            	}
            }
        }

        [SvgAttribute("cy")]
        public virtual SvgUnit CenterY
        {
        	get { return this._centerY; }
        	set
        	{
        		if(_centerY != value)
        		{
        			this._centerY = value;
        			this.IsPathDirty = true;
        			OnAttributeChanged(new AttributeEventArgs{ Attribute = "cy", Value = value });
        		}
        	}
        }

        [SvgAttribute("rx")]
        public virtual SvgUnit RadiusX
        {
        	get { return this._radiusX; }
        	set
        	{
        		if(_radiusX != value)
        		{
        			this._radiusX = value;
        			this.IsPathDirty = true;
        			OnAttributeChanged(new AttributeEventArgs{ Attribute = "rx", Value = value });
        		}
        	}
        }

        [SvgAttribute("ry")]
        public virtual SvgUnit RadiusY
        {
        	get { return this._radiusY; }
        	set
        	{
        		if(_radiusY != value)
        		{
        			this._radiusY = value;
        			this.IsPathDirty = true;
        			OnAttributeChanged(new AttributeEventArgs{ Attribute = "ry", Value = value });
        		}
        	}
        }

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        /// <value></value>
        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            if (this._path == null || this.IsPathDirty)
            {
							float halfStrokeWidth = base.StrokeWidth / 2;

							// If it is to render, don't need to consider stroke width.
							// i.e stroke width only to be considered when calculating boundary
							if (renderer != null)
							{
								halfStrokeWidth = 0;
								this.IsPathDirty = false;
							}

                var center = SvgUnit.GetDevicePoint(this._centerX, this._centerY, renderer, this);
								var radius = SvgUnit.GetDevicePoint(this._radiusX + halfStrokeWidth, this._radiusY + halfStrokeWidth, renderer, this);

                this._path = new GraphicsPath();
                _path.StartFigure();
                _path.AddEllipse(center.X - radius.X, center.Y - radius.Y, 2 * radius.X, 2 * radius.Y);
                _path.CloseFigure();
            }
            return _path;
        }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="Graphics"/> object.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> object to render to.</param>
        protected override void Render(ISvgRenderer renderer)
        {
            if (this._radiusX.Value > 0.0f && this._radiusY.Value > 0.0f)
            {
                base.Render(renderer);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgEllipse"/> class.
        /// </summary>
        public SvgEllipse()
        {
        }



		public override SvgElement DeepCopy()
		{
			return DeepCopy<SvgEllipse>();
		}

		public override SvgElement DeepCopy<T>()
		{
			var newObj = base.DeepCopy<T>() as SvgEllipse;
			newObj.CenterX = this.CenterX;
			newObj.CenterY = this.CenterY;
			newObj.RadiusX = this.RadiusX;
			newObj.RadiusY = this.RadiusY;
			return newObj;
		}
    }
}