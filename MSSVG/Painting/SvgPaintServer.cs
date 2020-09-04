using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    /// <summary>
    /// Represents the base class for all paint servers that are intended to be used as a fill or stroke.
    /// </summary>
    [TypeConverter(typeof(SvgPaintServerFactory))]
    public abstract class SvgPaintServer : SvgElement
    {
        public Func<SvgPaintServer> GetCallback { get; set; }

        /// <summary>
        /// An unspecified <see cref="SvgPaintServer"/>.
        /// </summary>
        public static readonly SvgPaintServer None = new SvgColourServer();

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgPaintServer"/> class.
        /// </summary>
        public SvgPaintServer()
        {
        }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="ISvgRenderer"/> object.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> object to render to.</param>
        protected override void Render(ISvgRenderer renderer)
        {
            // Never render paint servers or their children
        }

        /// <summary>
        /// Gets a <see cref="Brush"/> representing the current paint server.
        /// </summary>
        /// <param name="styleOwner">The owner <see cref="SvgVisualElement"/>.</param>
        /// <param name="opacity">The opacity of the brush.</param>
        public abstract Brush GetBrush(SvgVisualElement styleOwner, ISvgRenderer renderer, float opacity, bool forStroke = false);

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return String.Format("url(#{0})", this.ID);
        }



    }
}