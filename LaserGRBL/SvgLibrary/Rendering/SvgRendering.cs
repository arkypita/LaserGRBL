using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Svg
{
    /// <summary>
    /// The creator of SVG content might want to provide a hint about what tradeoffs to make as the browser renders <path> element or basic shapes. The shape-rendering attribute provides these hints.
    /// </summary>
    /// <references>https://developer.mozilla.org/en-US/docs/Web/SVG/Attribute/shape-rendering</references>
    /// <remarks>
    /// Default is <see cref="Inherit"/>. That means the value comes from the parent element. If parents are also not set, then the value is <see cref="Auto"/>.
    /// </remarks>
    [TypeConverter(typeof(SvgShapeRenderingConverter))]
    public enum SvgShapeRendering
    {
        /// <summary>
        /// Indicates that the SVG shape rendering properties from the parent will be used.
        /// </summary>
        /// <AnitAlias>Based of parent. If parents are also not set, then <see cref="Auto"/></AnitAlias>
        Inherit,

        /// <summary>
        /// Indicates that the user agent shall make appropriate tradeoffs to balance speed, crisp edges and geometric precision, but with geometric precision given more importance than speed and crisp edges.
        /// </summary>
        /// <AnitAlias>true</AnitAlias>
        Auto,

        /// <summary>
        /// Indicates that the user agent shall emphasize rendering speed over geometric precision and crisp edges. This option will sometimes cause the user agent to turn off shape anti-aliasing.
        /// </summary>
        /// <AnitAlias>false</AnitAlias>
        OptimizeSpeed,

        /// <summary>
        /// Indicates that the user agent shall attempt to emphasize the contrast between clean edges of artwork over rendering speed and geometric precision. To achieve crisp edges, the user agent might turn off anti-aliasing for all lines and curves or possibly just for straight lines which are close to vertical or horizontal. Also, the user agent might adjust line positions and line widths to align edges with device pixels.
        /// </summary>
        /// <AnitAlias>false</AnitAlias>
        CrispEdges,

        /// <summary>
        /// Indicates that the user agent shall emphasize geometric precision over speed and crisp edges.
        /// </summary>
        /// <AnitAlias>false</AnitAlias>
        GeometricPrecision
    }


    /// <summary>
    /// The creator of SVG content might want to provide a hint about what tradeoffs to make as the browser renders text. The text-rendering attribute provides these hints.
    /// </summary>
    /// <references>https://developer.mozilla.org/en-US/docs/Web/SVG/Attribute/text-rendering</references>
    /// <remarks>Not Implemented yet.</remarks>
    [TypeConverter(typeof(SvgTextRenderingConverter))]
    public enum SvgTextRendering
    {
        /// <summary>
        /// Indicates that the SVG shape rendering properties from the parent will be used.
        /// </summary>
        Inherit,

        /// <summary>
        /// Indicates that the browser shall make appropriate tradeoffs to balance speed, legibility and geometric precision, but with legibility given more importance than speed and geometric precision.
        /// </summary>
        Auto,

        /// <summary>
        /// Indicates that the user agent shall emphasize rendering speed over legibility and geometric precision. This option will sometimes cause some browsers to turn off text anti-aliasing.
        /// </summary>
        OptimizeSpeed,

        /// <summary>
        /// Indicates that the browser shall emphasize legibility over rendering speed and geometric precision. The user agent will often choose whether to apply anti-aliasing techniques, built-in font hinting or both to produce the most legible text.
        /// </summary>
        OptimizeLegibility,

        /// <summary>
        /// Indicates that the browser shall emphasize geometric precision over legibility and rendering speed. This option will usually cause the user agent to suspend the use of hinting so that glyph outlines are drawn with comparable geometric precision to the rendering of path data.
        /// </summary>
        GeometricPrecision
    }


    /// <summary>
    /// The image-rendering attribute provides a hint to the browser about how to make speed vs. quality tradeoffs as it performs image processing.
    /// </summary>
    /// <references>https://developer.mozilla.org/en-US/docs/Web/SVG/Attribute/image-rendering</references>
    /// <remarks>Not Implemented yet.</remarks>
    [TypeConverter(typeof(SvgImageRenderingConverter))]
    public enum SvgImageRendering
    {
        /// <summary>
        /// Indicates that the SVG shape rendering properties from the parent will be used.
        /// </summary>
        Inherit,

        /// <summary>
        /// Indicates that the user agent shall make appropriate tradeoffs to balance speed and quality, but quality shall be given more importance than speed.
        /// </summary>
        Auto,

        /// <summary>
        /// Indicates that the user agent shall emphasize rendering speed over quality.
        /// </summary>
        OptimizeSpeed,

        /// <summary>
        /// Indicates that the user agent shall emphasize quality over rendering speed.
        /// </summary>
        OptimizeQuality
    }
}
