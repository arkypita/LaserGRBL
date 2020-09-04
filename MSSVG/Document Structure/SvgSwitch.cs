using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    /// <summary>
    /// The ‘switch’ element evaluates the ‘requiredFeatures’, ‘requiredExtensions’ and ‘systemLanguage’ attributes on its direct child elements in order, and then processes and renders the first child for which these attributes evaluate to true
    /// </summary>
    [SvgElement("switch")]
    public class SvgSwitch : SvgVisualElement
    {
        public SvgSwitch()
        {
        }

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        /// <value></value>
        public override System.Drawing.Drawing2D.GraphicsPath Path(ISvgRenderer renderer)
        {
            return GetPaths(this, renderer);
        }

        /// <summary>
        /// Gets the bounds of the element.
        /// </summary>
        /// <value>The bounds.</value>
        public override System.Drawing.RectangleF Bounds
        {
            get
            {
                var r = new RectangleF();
                foreach (var c in this.Children)
                {
                    if (c is SvgVisualElement)
                    {
                        // First it should check if rectangle is empty or it will return the wrong Bounds.
                        // This is because when the Rectangle is Empty, the Union method adds as if the first values where X=0, Y=0
                        if (r.IsEmpty)
                        {
                            r = ((SvgVisualElement)c).Bounds;
                        }
                        else
                        {
                            var childBounds = ((SvgVisualElement)c).Bounds;
                            if (!childBounds.IsEmpty)
                            {
                                r = RectangleF.Union(r, childBounds);
                            }
                        }
                    }
                }

                return r;
            }
        }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="Graphics"/> object.
        /// </summary>
        /// <param name="renderer">The <see cref="Graphics"/> object to render to.</param>
        protected override void Render(ISvgRenderer renderer)
        {
            if (!Visible || !Displayable)
                return;

            this.PushTransforms(renderer);
            this.SetClip(renderer);
            base.RenderChildren(renderer);
            this.ResetClip(renderer);
            this.PopTransforms(renderer);
        }


        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgSwitch>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgSwitch;
            if (this.Fill != null)
                newObj.Fill = this.Fill.DeepCopy() as SvgPaintServer;
            return newObj;
        }
    }
}
