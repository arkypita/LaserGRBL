using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Svg.DataTypes;

namespace Svg.FilterEffects
{
    /// <summary>
    /// A filter effect consists of a series of graphics operations that are applied to a given source graphic to produce a modified graphical result.
    /// </summary>
    [SvgElement("filter")]
    public sealed class SvgFilter : SvgElement
    {
        private Bitmap sourceGraphic;
        private Bitmap sourceAlpha;

	
		/// <summary>
		/// Gets or sets the position where the left point of the filter.
		/// </summary>
		[SvgAttribute("x")]
		public SvgUnit X
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("x"); }
            set { this.Attributes["x"] = value; }
        }

		/// <summary>
		/// Gets or sets the position where the top point of the filter.
		/// </summary>
		[SvgAttribute("y")]
		public SvgUnit Y 
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("y"); }
            set { this.Attributes["y"] = value; }
        }


        /// <summary>
        /// Gets or sets the width of the resulting filter graphic.
        /// </summary>
        [SvgAttribute("width")]
        public SvgUnit Width
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("width"); }
            set { this.Attributes["width"] = value; }
        }

        /// <summary>
        /// Gets or sets the height of the resulting filter graphic.
        /// </summary>
        [SvgAttribute("height")]
        public SvgUnit Height
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("height"); }
            set { this.Attributes["height"] = value; }
        }


        /// <summary>
		/// Gets or sets the color-interpolation-filters of the resulting filter graphic.
		/// NOT currently mapped through to bitmap
        /// </summary>
        [SvgAttribute("color-interpolation-filters")]
        public SvgColourInterpolation ColorInterpolationFilters
        {
            get { return this.Attributes.GetAttribute<SvgColourInterpolation>("color-interpolation-filters"); }
            set { this.Attributes["color-interpolation-filters"] = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgFilter"/> class.
        /// </summary>
        public SvgFilter()
        {
        }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="ISvgRenderer"/> object.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> object to render to.</param>
        protected override void Render(ISvgRenderer renderer)
        {
			base.RenderChildren(renderer);
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public override object Clone()
        {
            return (SvgFilter)this.MemberwiseClone();
        }

        private Matrix GetTransform(SvgVisualElement element)
        {
            var transformMatrix = new Matrix();
            foreach (var transformation in element.Transforms)
            {
                transformMatrix.Multiply(transformation.Matrix);
            }
            return transformMatrix;
        }

        private RectangleF GetPathBounds(SvgVisualElement element, ISvgRenderer renderer, Matrix transform)
        {
            var bounds = element.Path(renderer).GetBounds();
            var pts = new PointF[] { bounds.Location, new PointF(bounds.Right, bounds.Bottom) };
            transform.TransformPoints(pts);

            return new RectangleF(Math.Min(pts[0].X, pts[1].X), Math.Min(pts[0].Y, pts[1].Y),
                                  Math.Abs(pts[0].X - pts[1].X), Math.Abs(pts[0].Y - pts[1].Y));
        }

        public void ApplyFilter(SvgVisualElement element, ISvgRenderer renderer, Action<ISvgRenderer> renderMethod)
        {
            var inflate = 0.5f;
            var transform = GetTransform(element);
            var bounds = GetPathBounds(element, renderer, transform);

            if (bounds.Width == 0 || bounds.Height == 0)
                return;

            var buffer = new ImageBuffer(bounds, inflate, renderer, renderMethod) { Transform = transform };

            IEnumerable<SvgFilterPrimitive> primitives = this.Children.OfType<SvgFilterPrimitive>();

            if (primitives.Count() > 0)
            {
                foreach (var primitive in primitives)
                {
                    primitive.Process(buffer);
                }

                // Render the final filtered image
                var bufferImg = buffer.Buffer;
                //bufferImg.Save(@"C:\test.png");
                var imgDraw = RectangleF.Inflate(bounds, inflate * bounds.Width, inflate * bounds.Height);
                var prevClip = renderer.GetClip();
                renderer.SetClip(new Region(imgDraw));
                renderer.DrawImage(bufferImg, imgDraw, new RectangleF(bounds.X, bounds.Y, imgDraw.Width, imgDraw.Height), GraphicsUnit.Pixel);
                renderer.SetClip(prevClip);
            }
        }
                

        #region Defaults

        private void ResetDefaults()
        {
            if (this.sourceGraphic != null)
            {
                this.sourceGraphic.Dispose();
                this.sourceGraphic = null;
            }

            if (this.sourceAlpha != null)
            {
                this.sourceAlpha.Dispose();
                this.sourceAlpha = null;
            }
        }

        
        #endregion



		public override SvgElement DeepCopy()
		{
			return DeepCopy<SvgFilter>();
		}

		public override SvgElement DeepCopy<T>()
		{
			var newObj = base.DeepCopy<T>() as SvgFilter;
			newObj.Height = this.Height;
			newObj.Width = this.Width;
			newObj.X = this.X;
			newObj.Y = this.Y;
			newObj.ColorInterpolationFilters = this.ColorInterpolationFilters;
			return newObj;
		}
    }
}