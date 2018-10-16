using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using Svg.Transforms;

namespace Svg
{
    /// <summary>
    /// Defines a path that can be used by other <see cref="ISvgClipable"/> elements.
    /// </summary>
    [SvgElement("clipPath")]
    public sealed class SvgClipPath : SvgElement
    {
        private bool _pathDirty = true;

        /// <summary>
        /// Specifies the coordinate system for the clipping path.
        /// </summary>
        [SvgAttribute("clipPathUnits")]
        public SvgCoordinateUnits ClipPathUnits { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgClipPath"/> class.
        /// </summary>
        public SvgClipPath()
        {
            this.ClipPathUnits = SvgCoordinateUnits.Inherit;
        }

        private GraphicsPath cachedClipPath = null;

        /// <summary>
        /// Gets this <see cref="SvgClipPath"/>'s region to be used as a clipping region.
        /// </summary>
        /// <returns>A new <see cref="Region"/> containing the <see cref="Region"/> to be used for clipping.</returns>
        public Region GetClipRegion(SvgVisualElement owner)
        {
            if (cachedClipPath == null || this._pathDirty)
            {
                cachedClipPath = new GraphicsPath();

                foreach (SvgElement element in this.Children)
                {
                    this.CombinePaths(cachedClipPath, element);
                }

                this._pathDirty = false;
            }

            var result = cachedClipPath;
            if (ClipPathUnits == SvgCoordinateUnits.ObjectBoundingBox)
            {
                result = (GraphicsPath)cachedClipPath.Clone();
                using (var transform = new Matrix())
                {
                    var bounds = owner.Bounds;
                    transform.Scale(bounds.Width, bounds.Height, MatrixOrder.Append);
                    transform.Translate(bounds.Left, bounds.Top, MatrixOrder.Append);
                    result.Transform(transform);
                }
            }

            return new Region(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="element"></param>
        private void CombinePaths(GraphicsPath path, SvgElement element)
        {
            var graphicsElement = element as SvgVisualElement;

            if (graphicsElement != null && graphicsElement.Path(null) != null)
            {
                path.FillMode = (graphicsElement.ClipRule == SvgClipRule.NonZero) ? FillMode.Winding : FillMode.Alternate;

                GraphicsPath childPath = graphicsElement.Path(null);

                if (graphicsElement.Transforms != null)
                {
                    foreach (SvgTransform transform in graphicsElement.Transforms)
                    {
                        childPath.Transform(transform.Matrix);
                    }
                }

                if (childPath.PointCount > 0) path.AddPath(childPath, false);
            }

            foreach (SvgElement child in element.Children)
            {
                this.CombinePaths(path, child);
            }
        }

        /// <summary>
        /// Called by the underlying <see cref="SvgElement"/> when an element has been added to the
        /// <see cref="Children"/> collection.
        /// </summary>
        /// <param name="child">The <see cref="SvgElement"/> that has been added.</param>
        /// <param name="index">An <see cref="int"/> representing the index where the element was added to the collection.</param>
        protected override void AddElement(SvgElement child, int index)
        {
            base.AddElement(child, index);
            this._pathDirty = true;
        }

        /// <summary>
        /// Called by the underlying <see cref="SvgElement"/> when an element has been removed from the
        /// <see cref="SvgElement.Children"/> collection.
        /// </summary>
        /// <param name="child">The <see cref="SvgElement"/> that has been removed.</param>
        protected override void RemoveElement(SvgElement child)
        {
            base.RemoveElement(child);
            this._pathDirty = true;
        }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="ISvgRenderer"/> object.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> object to render to.</param>
        protected override void Render(ISvgRenderer renderer)
        {
            // Do nothing
        }


		public override SvgElement DeepCopy()
		{
			return DeepCopy<SvgClipPath>();
		}

		public override SvgElement DeepCopy<T>()
		{
			var newObj = base.DeepCopy<T>() as SvgClipPath;
			newObj.ClipPathUnits = this.ClipPathUnits;
			return newObj;
		}
    }
}
